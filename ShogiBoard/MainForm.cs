using ShogiCore;
using ShogiCore.CSA;
using ShogiCore.Notation;
using ShogiCore.USI;
using ShogiBoard.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ShogiBoard {
    public partial class MainForm : Form {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ConfigLoader configLoader = new ConfigLoader();

        /// <summary>
        /// 現在の局面
        /// </summary>
        public Board Board { get; private set; }
        /// <summary>
        /// プレイヤー。対局・検討・詰将棋解答中か否かは、このプロパティを用いてPlayers[0]がnullか否かで判定する。
        /// </summary>
        public IPlayer[] Players { get; private set; }

        /// <summary>
        /// 持ち時間情報
        /// </summary>
        struct TimeData {
            /// <summary>
            /// 持ち時間[ミリ秒]
            /// </summary>
            public int TimeA { get; set; }
            /// <summary>
            /// 秒読み[ミリ秒]
            /// </summary>
            public int TimeB { get; set; }
            /// <summary>
            /// 残り持ち時間[ミリ秒]
            /// </summary>
            public int RemainTime { get; set; }
            /// <summary>
            /// リセット
            /// </summary>
            public void Reset() {
                RemainTime = TimeA;
            }
            /// <summary>
            /// 時間を消費する
            /// </summary>
            /// <param name="time">時間[s]</param>
            /// <returns>時間切れならfalse。</returns>
            public bool ConsumeTime(int time) {
                if (TimeA <= 0) {
                    if (TimeB <= 0) {
                        // 持ち時間不明
                        return true;
                    } else {
                        // 秒読みのみ
                        // shogi-serverの実装は < のはず（秒読み2秒で最大1秒+端数までOK）だが、
                        // 秒読み1秒で最大1秒+端数まで考えるプログラムが多い気がするので変えてしまう。
                        return time <= TimeB;
                    }
                } else {
                    if (RemainTime < time) {
                        bool result = (time - RemainTime) <= TimeB;
                        RemainTime = 0;
                        return result;
                    } else {
                        RemainTime -= time;
                        return true;
                    }
                }
            }
        }
        /// <summary>
        /// 持ち時間情報
        /// </summary>
        readonly TimeData[] timeData = new TimeData[2];

        /// <summary>
        /// listBox1に入れるオブジェクト
        /// </summary>
        class MoveListBoxItem {
            /// <summary>
            /// 表示用文字列
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// 指し手データ
            /// </summary>
            public MoveDataEx MoveDataEx { get; set; }
            /// <summary>
            /// 初期化
            /// </summary>
            public MoveListBoxItem(string text, MoveDataEx moveDataEx) {
                Text = text;
                MoveDataEx = moveDataEx;
            }
            /// <summary>
            /// 文字列化
            /// </summary>
            public override string ToString() { return Text; }
        }

        volatile bool threadValid = false;
        #region 対局用
        int gameCount;
        Thread gameThread;
        object gameLock = new object();
        int gameWinner;
        GameEndReason gameEndReason;
        #endregion
        #region 通信対局用
        int networkGameCount;
        Thread networkGameThread;
        object networkGameLock = new object();
        volatile CSAClient csaClient;
        /// <summary>
        /// 前回の指し手のコメント（* {評価値} {PV}）
        /// </summary>
        string lastMoveComment;
        /// <summary>
        /// 前回の指し手の評価値
        /// </summary>
        int? lastMoveValue;
        #endregion
        #region 対局・通信対局用
        /// <summary>
        /// CSA棋譜出力
        /// </summary>
        CSAFileWriter csaFileWriter;
        #endregion
        #region 検討・詰将棋解答用
        Thread singleEngineThread;
        object singleEngineThreadLock = new object();
        #endregion
        #region 対局用
        #endregion

        public MainForm() {
            InitializeComponent();
            ClearToolStripLabelResult();
            Board = new Board();
            Players = new IPlayer[2];

            // コンフィグの読み込み
            configLoader.ConfigLoadFailed += (sender, e) => {
                FormUtility.SafeInvoke(this, () => {
                    if (e.ToBeQuit) {
                        MessageBox.Show(this, "Engine.xmlの読み込みに失敗したため終了します。", "エラー");
                        Close();
                    } else {
                        MessageBox.Show(this, "ShogiBoard.Volatile.xmlの読み込みに失敗しました。", "エラー");
                    }
                });
            };
            configLoader.StartThread();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
        }

        private void MainForm_Load(object sender, EventArgs e) {
            // 初期局面を描画
            blunderViewControl.DrawAsync(Board);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            AbortSingleEngineThread();
            StopNetworkGame();
            StopGame();
        }

        #region メニューバー

        private void 終了XToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void 棋譜コピーKIF形式DToolStripMenuItem_Click(object sender, EventArgs e) {
            Clipboard.SetText(new KifuNotationWriter(KifuNotationWriter.Mode.KIF).WriteToString(GetNotation()));
        }

        private void 棋譜コピーCSA形式CToolStripMenuItem_Click(object sender, EventArgs e) {
            var notation = Board.ToNotation();
            notation.FirstPlayerName = playerInfoControlP.PlayerName;
            notation.SecondPlayerName = playerInfoControlN.PlayerName;
            Clipboard.SetText(new PCLNotationWriter().WriteToString(GetNotation()));
        }

        private void 棋譜コピーSFEN形式EToolStripMenuItem_Click(object sender, EventArgs e) {
            var notation = Board.ToNotation();
            notation.FirstPlayerName = playerInfoControlP.PlayerName;
            notation.SecondPlayerName = playerInfoControlN.PlayerName;
            Clipboard.SetText(new SFENNotationWriter().WriteToString(GetNotation()));
        }

        private void 局面コピーKIF形式QToolStripMenuItem_Click(object sender, EventArgs e) {
            Clipboard.SetText(new KifuNotationWriter(KifuNotationWriter.Mode.KIF).WriteToString(GetPositionNotation()));
        }

        private void 局面コピーCSA形式PToolStripMenuItem_Click(object sender, EventArgs e) {
            Clipboard.SetText(new PCLNotationWriter().WriteToString(GetPositionNotation()));
        }

        private void 局面コピーSFEN形式RToolStripMenuItem_Click(object sender, EventArgs e) {
            Clipboard.SetText(new SFENNotationWriter().WriteToString(GetPositionNotation()));
        }

        private void 棋譜局面貼り付けVToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                if (Clipboard.ContainsText()) {
                    LoadNotationFromString(Clipboard.GetText());
                }
            } catch (Exception ex) {
                logger.Warn("棋譜・局面貼り付け失敗", ex);
                MessageBox.Show(this, "貼り付けに失敗しました。" + Environment.NewLine + ex.Message, "エラー");
            }
        }

        private void 対局GToolStripMenuItem1_Click(object sender, EventArgs e) {
            using (GameForm form = new GameForm(configLoader.EngineList, configLoader.VolatileConfig)) {
                if (form.ShowDialog(this) == DialogResult.OK) {
                    // 対局開始
                    SetEnabledForGame(true, true);
                    StartGame();
                }
            }
        }

        private void 通信対局NToolStripMenuItem_Click(object sender, EventArgs e) {
            using (NetworkGameForm form = new NetworkGameForm(configLoader.EngineList, configLoader.Config, configLoader.VolatileConfig)) {
                if (form.ShowDialog(this) == DialogResult.OK) {
                    // 対局開始
                    SetEnabledForNetworkGame(true);
                    StartNetworkGame();
                }
            }
        }

        private void 通信対局切断DToolStripMenuItem_Click(object sender, EventArgs e) {
            StopNetworkGame();
        }

        private void 検討TToolStripMenuItem_Click(object sender, EventArgs e) {
            if (configLoader.EngineList.Engines.Count <= 0) {
                エンジン一覧EToolStripMenuItem_Click(this, EventArgs.Empty);
            } else {
                using (var form = new ThinkForm(configLoader.VolatileConfig, configLoader.EngineList)) {
                    if (form.ShowDialog(this) == DialogResult.OK) {
                        StartThink();
                    }
                }
            }
        }

        private void 詰将棋解答MToolStripMenuItem_Click(object sender, EventArgs e) {
            if (configLoader.EngineList.Engines.Count <= 0) {
                エンジン一覧EToolStripMenuItem_Click(this, EventArgs.Empty);
            } else {
                using (var form = new MateForm(configLoader.VolatileConfig, configLoader.EngineList)) {
                    if (form.ShowDialog(this) == DialogResult.OK) {
                        StartMate();
                    }
                }
            }
        }

        private void 中断AToolStripMenuItem_Click(object sender, EventArgs e) {
            StopSingleEngineThread();
            StopNetworkGame();
            StopGame();
        }

        private void エンジン一覧EToolStripMenuItem_Click(object sender, EventArgs e) {
            using (EngineListForm form = new EngineListForm(configLoader.EngineList)) {
                form.ShowDialog(this);
            }
        }

        private void ログフォルダを開くLToolStripMenuItem_Click(object sender, EventArgs e) {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (Directory.Exists(path)) {
                Process.Start(path);
            } else {
                MessageBox.Show(this, "ログフォルダ " + path + " が存在しません。", "エラー");
            }
        }

        #endregion

        #region ツールバー

        private void toolStripButton切_Click(object sender, EventArgs e) {
            通信対局切断DToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton投_Click(object sender, EventArgs e) {

        }

        private void toolStripButton待_Click(object sender, EventArgs e) {

        }

        private void toolStripButton急_Click(object sender, EventArgs e) {

        }

        private void toolStripButton中_Click(object sender, EventArgs e) {
            中断AToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton検_Click(object sender, EventArgs e) {
            if (configLoader.IsThinkEngineExists()) {
                StartThink();
            } else {
                検討TToolStripMenuItem_Click(this, EventArgs.Empty);
            }
        }

        private void toolStripButton詰_Click(object sender, EventArgs e) {
            if (configLoader.IsMateEngineExists()) {
                StartMate();
            } else {
                詰将棋解答MToolStripMenuItem_Click(this, EventArgs.Empty);
            }
        }

        private void toolStripLabelResult_DoubleClick(object sender, EventArgs e) {
            try {
                if (!string.IsNullOrEmpty(toolStripLabelResult.ToolTipText)) {
                    Clipboard.SetText(toolStripLabelResult.ToolTipText);
                }
            } catch {
            }
        }

        #endregion

        /// <summary>
        /// 指定手数の局面を表示する
        /// </summary>
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            int n = Math.Max(listBox1.SelectedIndex, 0); // -1は0手目扱い
            if (Board.History.Count == n) return;
            if (Board.History.Count < n) {
                // n手目まで進める
                while (Board.History.Count < n) {
                    MoveData moveData = ((MoveListBoxItem)listBox1.Items[Board.History.Count + 1]).MoveDataEx.MoveData;
                    if (moveData.IsEmpty) break; // 終局の理由とかの場合。
                    Board.Do(ShogiCore.Move.FromNotation(Board, moveData));
                }
            } else {
                // n手目まで戻す
                while (n < Board.History.Count) Board.Undo();
            }
            textBoxComment.Text = ((MoveListBoxItem)listBox1.SelectedItem).MoveDataEx.Comment;
            blunderViewControl.DrawAsync(Board);
            gameGraphControl1.SetMoveCount(n);
        }

        private void MainForm_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] pathList = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (pathList != null && pathList.Length == 1 && File.Exists(pathList[0])) {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] pathList = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (pathList != null && pathList.Length == 1 && File.Exists(pathList[0])) {
                    try {
                        LoadNotationFromString(File.ReadAllText(pathList[0], Encoding.GetEncoding(932))); // sjis
                    } catch (Exception ex) {
                        logger.Warn("棋譜・局面読み込み失敗", ex);
                        MessageBox.Show(this, "読み込みに失敗しました。" + Environment.NewLine + ex.Message, "エラー");
                    }
                }
            }
        }

        /// <summary>
        /// 棋譜の読み込み
        /// </summary>
        private void LoadNotationFromString(string notationString) {
            Notation notation = new NotationLoader().Load(notationString).FirstOrDefault();
            if (notation == null) return;

            playerInfoControlP.PlayerName = notation.FirstPlayerName;
            playerInfoControlN.PlayerName = notation.SecondPlayerName;
            timeData[0].TimeA = notation.TimeA;
            timeData[0].TimeB = notation.TimeB;
            timeData[1].TimeA = notation.TimeA;
            timeData[1].TimeB = notation.TimeB;
            timeData[0].Reset();
            timeData[1].Reset();

            Board.Reset(notation.InitialBoard);
            blunderViewControl.DrawAsync(Board);
            gameGraphControl1.SetMoveCount(0);

            listBox1.BeginUpdate();
            try {
                ClearMoveList();
                BoardData boardData = notation.InitialBoard == null ? BoardData.CreateEquality() : notation.InitialBoard.Clone();
                int moveCount = 0;
                foreach (MoveDataEx moveDataEx in notation.Moves) {
                    timeData[boardData.Turn].ConsumeTime(moveDataEx.Time);
                    boardData.Do(moveDataEx.MoveData);
                    moveCount++;
                    AddMoveToListForAppliedBoard(boardData.Turn ^ 1, boardData, moveDataEx, false, moveCount == notation.Moves.Length);
                }
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            } finally {
                listBox1.EndUpdate();
            }
            listBox1.Focus();
        }
        #region 対局

        /// <summary>
        /// 対局開始
        /// </summary>
        private void StartGame() {
            ShowPlayer2EngineView();
            threadValid = true;
            gameThread = new Thread(GameThread);
            gameThread.Name = "GameThread";
            gameThread.Start();
        }

        /// <summary>
        /// 対局停止
        /// </summary>
        private void StopGame() {
            if (gameThread == null || !gameThread.IsAlive) {
                gameThread = null;
                return;
            }

            CSAClient client;
            lock (gameLock) {
                client = this.csaClient; // ローカルへ。
                if (client == null) {
                    // スレッド停止フラグ
                    threadValid = false;
                } else {
                    // 対局中の閉じる確認
                    Monitor.Exit(gameLock);
                    if (MessageBox.Show(this, "対局を中断します。よろしいですか?", "確認", MessageBoxButtons.OKCancel) != DialogResult.OK) {
                        Monitor.Enter(gameLock);
                        return; // 停止しない。
                    }
                    Monitor.Enter(gameLock);
                    // スレッド停止フラグ
                    threadValid = false;
                    // 強制停止
                    foreach (var player in Players) player.Abort();
                }
            }
            // 5秒くらい待ってダメならAbort
            for (int i = 0; ; i++) {
                if (i < 50) {
                    if (gameThread.Join(100)) break;
                    Application.DoEvents(); // Invoke対策 _no
                } else {
                    gameThread.Abort();
                    break;
                }
            }
        }

        /// <summary>
        /// 対局スレッド
        /// </summary>
        private void GameThread() {
            HashSet<ulong> hashSet = new HashSet<ulong>(); // 終局時の盤面ハッシュのリスト。終局図が同じなら完全に重複なので結果から除外する。
            int[] resultCounts = new int[4];

            for (gameCount = 0; threadValid && (
                configLoader.VolatileConfig.GameCount == 0 ||
                gameCount < configLoader.VolatileConfig.GameCount); ) {
                try {
                    var engine1 = configLoader.EngineList.Select(configLoader.VolatileConfig.GameEngine1Name, configLoader.VolatileConfig.GameEngine1Path);
                    var engine2 = configLoader.EngineList.Select(configLoader.VolatileConfig.GameEngine2Name, configLoader.VolatileConfig.GameEngine2Path);

                    // USIエンジン起動
                    using (USIPlayer player1 = CreateUSIPlayer(engine1, 1))
                    using (USIPlayer player2 = CreateUSIPlayer(engine2, 2)) {
                        Players[0] = player1;
                        Players[1] = player2;
                        engineViewControl1.Attach(player1);
                        engineViewControl2.Attach(player2);
                        using (CSAFileWriter fileWriter = new CSAFileWriter()) {
                            try {
                                // 対局スレッド用。this.BoardはGUIスレッド用なので注意。
                                Board board = new ShogiCore.Board();

                                lock (gameLock) {
                                    if (!threadValid) break;
                                    csaFileWriter = fileWriter;
                                }
                                // 対局開始
                                int turnFlip = gameCount & 1;
                                DoGame(board, hashSet, resultCounts, new[] { engine1, engine2 }, turnFlip);
                                gameCount++;
                            } finally {
                                lock (gameLock) {
                                    csaClient = null;
                                    csaFileWriter = null;
                                }
                            }
                        }
                    }
                } catch (ThreadAbortException) {
                    throw;
                } catch (USIEngineException e) {
                    logger.Warn("USIエンジンの起動に失敗", e);
                    for (int i = 0; threadValid && i < 10; i++) {
                        SetTitleStatusText("USIエンジン起動失敗：" + e.InnerException.Message + " (10秒後に再接続)" + new string('.', i + 1));
                        Thread.Sleep(1000);
                    }
                } catch (Exception e) {
                    logger.Warn("対局中にエラー発生", e);
                    if (IsSocketException(e)) {
                        for (int i = 0; threadValid && i < 10; i++) {
                            SetTitleStatusText("対局中に通信エラー発生：" + e.Message + " (10秒後に再接続)" + new string('.', i + 1));
                            Thread.Sleep(1000);
                        }
                    } else {
                        SetTitleStatusText("対局中にエラー発生（停止）：" + e.Message);
                    }
                } finally {
                    engineViewControl2.Detach();
                    engineViewControl1.Detach();
                }
            }

            // 対局終了
            threadValid = false;
            FormUtility.SafeInvoke(this, () => {
                SetTitleStatusText(null);
                SetEnabledForGame(false, false);
            });
        }

        /// <summary>
        /// 対局1局分の処理
        /// </summary>
        private void DoGame(Board board, HashSet<ulong> hashSet, int[] resultCounts, Engine[] engines, int turnFlip) {
            gameWinner = -2;
            gameEndReason = GameEndReason.Unknown;

            var gameTime = configLoader.VolatileConfig.GameTimes[configLoader.VolatileConfig.GameTimeIndex];
            for (int i = 0; i < timeData.Length; i++) {
                timeData[i].TimeA = gameTime.TimeASeconds * 1000;
                timeData[i].TimeB = gameTime.TimeBSeconds * 1000;
                timeData[i].Reset();
            }
            lock (Board) {
                Board = board.Clone();
            }
            // 対局開始局面を描画
            blunderViewControl.DrawAsync(board);
            // 対局開始時の情報表示
            FormUtility.SafeInvoke(this, () => {
                // エンジン情報欄を初期化
                engineViewControl1.Board = board;
                engineViewControl1.Clear();
                engineViewControl2.Board = board;
                engineViewControl2.Clear();
                // 情報表示
                UpdateGameResult(resultCounts, engines, turnFlip);
                // 対局者名などの表示更新
                playerInfoControlP.PlayerName = engines[turnFlip].Name;
                playerInfoControlP.TimeASeconds = timeData[turnFlip].TimeA / 1000;
                playerInfoControlP.TimeBSeconds = timeData[turnFlip].TimeB / 1000;
                playerInfoControlN.PlayerName = engines[turnFlip ^ 1].Name;
                playerInfoControlN.TimeASeconds = timeData[turnFlip ^ 1].TimeA / 1000;
                playerInfoControlN.TimeBSeconds = timeData[turnFlip ^ 1].TimeB / 1000;
                playerInfoControlP.Reset();
                playerInfoControlN.Reset();
                ClearMoveList();
            });

            // GameIDはfloodgate風にでっち上げ。
            // wdoor+negabona-0-32+negabook@g14+gps_bonanza@g14+20130705233351
            string gameID = "ShogiBoard-" + gameTime.TimeASeconds + "-" + gameTime.TimeBSeconds + "+" + Players[0 ^ turnFlip].Name + "+" + Players[1 ^ turnFlip].Name + "+" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            csaFileWriter.Create(
                Players[0 ^ turnFlip].Name,
                Players[1 ^ turnFlip].Name,
                gameID,
                board.ToCSAStandard());

            while (gameWinner == -2) {
                if (!threadValid) return;

                MoveList moves = board.GetMovesSafe();
                if (moves.Count == 0) { // 合法手が無いなら勝負あり。
                    OnGameEnd(board.Turn ^ 1, GameEndReason.Mate); // 最後の一手の方の勝ち
                    break;
                }

                // TODO: 時間が切れた瞬間のAbort。(めんどい…)

                int playerIndex = (board.MoveCount & 1) ^ turnFlip;
                var player = Players[playerIndex];

                FormUtility.SafeInvoke(this, () => {
                    // 自分の時間消費の開始
                    GetPlayerInfoControl(board.Turn).StartTurn();
                    // エンジンの表示をクリア
                    GetEngineViewControl(playerIndex).Clear();
                });
                int startTime = Environment.TickCount;
                Move move = player.DoTurn(board,
                    timeData[board.Turn].TimeA,
                    timeData[board.Turn ^ 1].TimeA,
                    timeData[board.Turn].TimeB);
                int thinkTime = unchecked(Environment.TickCount - startTime);
                FormUtility.SafeInvoke(this, () => {
                    GetPlayerInfoControl(board.Turn).EndTurn();
                });
                if (move.IsEmpty) {
                    logger.Warn("指し手が不正: Empty");
                    break;
                }

                // 時間の処理
                int consumeTime = Math.Max(1000, thinkTime - thinkTime % 1000); // CSAルール：端数切り捨て最低1秒。
                bool timeUp = !timeData[board.Turn].ConsumeTime(consumeTime);
                if (timeUp) {
                    OnGameEnd(board.Turn ^ 1, GameEndReason.TimeUp);
                } else if (move == ShogiCore.Move.Win) {
                    if (board.IsNyuugyokuWin()) {
                        OnGameEnd(board.Turn, GameEndReason.Nyuugyoku);
                    } else {
                        OnGameEnd(board.Turn ^ 1, GameEndReason.IllegalWinDecl);
                    }
                } else if (move == ShogiCore.Move.Resign) {
                    OnGameEnd(board.Turn ^ 1, GameEndReason.Resign);
                } else if (!board.IsLegalMove(ref move)) {
                    OnGameEnd(board.Turn ^ 1, GameEndReason.IllegalMove);
                } else {
                    var boardData = board.ToBoardData();
                    var moveData = move.ToNotation();
                    var usiPlayer = player as USIPlayer;
                    string comment;
                    int? value;
                    if (usiPlayer == null || !usiPlayer.HasScore) { // scoreを受信してないならコメント無し。(PVが無くてもscoreがあればコメントあり)
                        comment = "";
                        value = null;
                    } else {
                        comment = BuildComment(usiPlayer, boardData.Clone(), move);
                        value = Board.NegativeByTurn(usiPlayer.LastScore, board.Turn);
                    }
                    csaFileWriter.AppendMove(PCLNotationWriter.ToString(boardData, moveData), comment);
                    AddMoveToList(boardData.Turn,
                        new MoveDataEx(moveData, comment, value, consumeTime),
                        true, true, moveData.ToString(boardData));

                    // 進める
                    board.Do(move);

                    // 千日手チェック
                    if (board.IsEndless(3)) {
                        if (board.IsPerpetualCheck(0)) {
                            // 連続王手の千日手
                            OnGameEnd(board.Turn, GameEndReason.Perpetual);
                        } else if (board.IsPerpetualCheck(1)) {
                            // 連続王手の千日手
                            OnGameEnd(board.Turn ^ 1, GameEndReason.Perpetual);
                        } else {
                            // 通常の千日手
                            OnGameEnd(-1, GameEndReason.Endless);
                        }
                    } else if (board.IsMate()) {
                        OnGameEnd(board.Turn ^ 1, GameEndReason.Mate); // 最後の一手の方の勝ち
                    }
                }
            }

            // 終局理由を画面へ
            AddMoveToList(GameEndReasonUtility.ToString(gameEndReason));
            // 時間消費の停止
            FormUtility.SafeInvoke(this, () => {
                playerInfoControlP.EndTurn();
                playerInfoControlN.EndTurn();
            });
            // 勝ち数の集計・統計情報の表示
            if (hashSet.Add(board.RandomizedFullHash)) {
                if (gameWinner == -1) { // 引き分け
                    resultCounts[1]++;
                } else {
                    resultCounts[(gameWinner ^ turnFlip) * 2]++;
                }
            } else {
                resultCounts[3]++; // 重複
                gameEndReason = GameEndReason.SameNotation;
            }
            UpdateGameResult(resultCounts, engines, turnFlip, gameEndReason);
        }

        /// <summary>
        /// 勝ち負けの情報表示部分を更新
        /// </summary>
        private void UpdateGameResult(int[] resultCounts, Engine[] engines, int turnFlip, GameEndReason gameEndReason = GameEndReason.Unknown) {
            var resultStr = string.Format("{0}-{1}-{2}(重複={3})", resultCounts[0], resultCounts[1], resultCounts[2], resultCounts[3]);
            toolStripLabelResult.Text = resultStr;

            int win = resultCounts[0];
            int draw = resultCounts[1];
            int lose = resultCounts[2];
            int N = win + lose;
            // 勝率 (引き分けは0.5勝扱い)
            double NN = N + draw;
            double wr = NN <= double.Epsilon ? 0 : (win + draw * 0.5) * 100.0 / NN;
            // R差 (引き分けは0.5勝扱い)
            double rating = N <= 0 ? 0 : MathUtility.WinRateToRatingDiff(wr / 100.0);
            // 勝率の95%信頼区間 (引き分けは除く)
            double wL, wH;
            if (N <= 0) {
                wL = wH = 0.0; // 仮
            } else {
                MathUtility.GetWinConfidence(win, lose, 0.05, out wL, out wH);
                wL *= 100.0;
                wH *= 100.0;
            }
            // 有意確率 (引き分けは除く)
            double wp = MathUtility.SignTest(win, lose) * 100.0;

            string s = resultStr + Environment.NewLine +
                "勝率：        " + wr.ToString("##0.0").PadLeft(5) + "%" + Environment.NewLine +
                "R差：         " + rating.ToString("0") + Environment.NewLine +
                "有意確率：    " + wp.ToString("##0.0").PadLeft(5) + "%" + Environment.NewLine +
                "95%信頼区間： " + wL.ToString("##0.0") + "% ～ " + wH.ToString("##0.0") + "%" + Environment.NewLine +
                "";
            toolStripLabelResult.ToolTipText = s;

            SetTitleStatusText("連続対局中：" +
                engines[0 ^ turnFlip].Name + " vs " +
                engines[1 ^ turnFlip].Name + "：" + resultStr);

            // 終局時はログる。
            if (gameEndReason != GameEndReason.Unknown) {
                logger.Info(GameEndReasonUtility.ToString(gameEndReason) + "：" +
                    Regex.Replace(s.TrimEnd().Replace(Environment.NewLine, " ")
                        .Replace(" ～ ", "～"), "： *", "=")); // 適当整形
            }
        }

        /// <summary>
        /// 対局終了
        /// </summary>
        private void OnGameEnd(int turn, GameEndReason reason) {
            gameWinner = turn;
            gameEndReason = reason;
            foreach (var player in Players) {
                var usiPlayer = player as USIPlayer;
                if (usiPlayer != null) {
                    Func<double?, string> ToNPSString =
                        nps => nps.HasValue ? nps.Value.ToString("#,##0") : "-";
                    logger.InfoFormat("{0} 平均NPS：全体={1} 序盤={2} 終盤={3}",
                        usiPlayer.Name,
                        ToNPSString(usiPlayer.MeanNPS),
                        ToNPSString(usiPlayer.MeanNPSOfOpening),
                        ToNPSString(usiPlayer.MeanNPSOfEndGame));
                }
            }
        }
        #endregion

        #region 通信対局

        /// <summary>
        /// 通信対局開始
        /// </summary>
        private void StartNetworkGame() {
            HidePlayer2EngineView();
            ClearToolStripLabelResult();
            threadValid = true;
            networkGameThread = new Thread(NetworkGameThread);
            networkGameThread.Name = "NetworkGameThread";
            networkGameThread.Start();
        }

        /// <summary>
        /// 通信対局停止
        /// </summary>
        private void StopNetworkGame() {
            if (networkGameThread == null || !networkGameThread.IsAlive) {
                networkGameThread = null;
                return;
            }

            CSAClient client;
            lock (networkGameLock) {
                client = this.csaClient; // ローカルへ。
                if (client == null) {
                    // スレッド停止フラグ
                    threadValid = false;
                } else {
                    // 対局中の閉じる確認
                    if (CSAState.GameWaiting < client.State && client.State < CSAState.Finished) {
                        Monitor.Exit(networkGameLock);
                        if (MessageBox.Show(this, "通信対局を中断します。よろしいですか?", "確認", MessageBoxButtons.OKCancel) != DialogResult.OK) {
                            Monitor.Enter(networkGameLock);
                            return; // 停止しない。
                        }
                        Monitor.Enter(networkGameLock);
                    }
                    // スレッド停止フラグ
                    threadValid = false;
                    // LOGOUT
                    lock (client) {
                        if (client.State == CSAState.Game) {
                            // 中断
                            client.SendChudan();
                        } else if (client.State != CSAState.Finished) {
                            // ログアウト
                            client.SendLogout();
                        }
                    }
                }
            }
            // 5秒くらい待ってダメならAbort
            for (int i = 0; ; i++) {
                if (i < 50) {
                    if (networkGameThread.Join(100)) break;
                    Application.DoEvents(); // Invoke対策 _no
                } else {
                    networkGameThread.Abort();
                    break;
                }
            }
        }

        /// <summary>
        /// 通信対局スレッド
        /// </summary>
        private void NetworkGameThread() {
            for (networkGameCount = 0; threadValid && (
                configLoader.VolatileConfig.NetworkGameCount == 0 ||
                networkGameCount < configLoader.VolatileConfig.NetworkGameCount); ) {
                try {
                    var engine = configLoader.EngineList.Select(configLoader.VolatileConfig.NetworkGameEngineName, configLoader.VolatileConfig.NetworkGameEnginePath);
                    var connection = configLoader.Config.NetworkGameConnections[configLoader.VolatileConfig.NetworkGameConnectionIndex];
                    string csaHost = connection.Address;
                    string csaID = connection.User;
                    string csaPW = connection.Pass;

                    // USIエンジン起動
                    using (USIPlayer player = CreateUSIPlayer(engine, 1)) {
                        Players[0] = player;
                        engineViewControl1.Attach(player);
                        // ログイン
                        SetTitleStatusText("対局待ち：" + csaHost + " - " + csaID);
                        using (CSAClient client = new CSAClient(csaHost))
                        using (CSAFileWriter fileWriter = new CSAFileWriter()) {
                            try {
                                // 通信対局スレッド用。this.BoardはGUIスレッド用なので注意。
                                Board board = new ShogiCore.Board();

                                lock (networkGameLock) {
                                    if (!threadValid) break;
                                    csaClient = client;
                                    csaFileWriter = fileWriter;
                                }
                                client.SendPV = connection.SendPV;
                                client.KeepAlive = connection.KeepAlive;
                                client.LogoutOnGameEnd = true;
                                // 終局時の処理
                                client.GameEnd += (sender, e) => {
                                    var p = Players[0];
                                    if (p != null) {
                                        p.GameEnd(board, e.CSAClient.LastGameResult);
                                    }
                                    networkGameCount++;
                                };
                                // 切断時の処理
                                client.Disconnected += (sender, e) => {
                                    var p = Players[0];
                                    if (p != null) {
                                        p.Abort();
                                    }
                                };
                                // 対局開始
                                NetworkGameLoop(client, board, csaID, csaPW, CSAClient.ProtocolModes.CSA);
                                // 時間消費の停止
                                FormUtility.SafeInvoke(this, () => {
                                    playerInfoControlP.EndTurn();
                                    playerInfoControlN.EndTurn();
                                });
                            } finally {
                                lock (networkGameLock) {
                                    csaClient = null;
                                    csaFileWriter = null;
                                }
                            }
                        }
                    }
                } catch (ThreadAbortException) {
                    throw;
                } catch (CSALoginFailedException) {
                    logger.Info("ログイン失敗");
                    for (int i = 0; threadValid && i < 10; i++) {
                        SetTitleStatusText("ログイン失敗：10秒後に再接続" + new string('.', i + 1));
                        Thread.Sleep(1000);
                    }
                } catch (USIEngineException e) {
                    logger.Warn("USIエンジンの起動に失敗", e);
                    for (int i = 0; threadValid && i < 10; i++) {
                        SetTitleStatusText("USIエンジン起動失敗：" + e.InnerException.Message + " (10秒後に再接続)" + new string('.', i + 1));
                        Thread.Sleep(1000);
                    }
                } catch (Exception e) {
                    logger.Warn("通信対局中にエラー発生", e);
                    if (IsSocketException(e)) {
                        for (int i = 0; threadValid && i < 10; i++) {
                            SetTitleStatusText("対局中に通信エラー発生：" + e.Message + " (10秒後に再接続)" + new string('.', i + 1));
                            Thread.Sleep(1000);
                        }
                    } else {
                        SetTitleStatusText("対局中にエラー発生（停止）：" + e.Message);
                    }
                } finally {
                    engineViewControl1.Detach();
                }
            }

            // 通信対局終了
            threadValid = false;
            FormUtility.SafeInvoke(this, () => {
                SetTitleStatusText(null);
                SetEnabledForNetworkGame(false);
            });
        }

        class CSALoginFailedException : Exception { }

        /// <summary>
        /// ExceptionがSocketExceptionに関連しているのかどうか
        /// </summary>
        private bool IsSocketException(Exception e) {
            return e is System.Net.Sockets.SocketException ||
                (e.InnerException != null && IsSocketException(e.InnerException));
        }

        /// <summary>
        /// 通信対局1局分の処理
        /// </summary>
        private void NetworkGameLoop(CSAClient client, Board board, string csaID, string csaPW, CSAClient.ProtocolModes csaProto) {
            // ログイン
            client.SendLogin(csaID, csaPW, csaProto);

            while (client.State != CSAState.Finished) {
                CSAInternalCommand command = client.ReceiveCommand();
                switch (command.CommandType) {
                    case CSAInternalCommandTypes.LoginFailed:
                        throw new CSALoginFailedException();

                    case CSAInternalCommandTypes.ExConnected: // ※ 拡張モードは未実装なので適当
                        client.SendExGame("floodgate-900-0", "*");
                        break;

                    case CSAInternalCommandTypes.TestConnected:
                        client.SendChallenge();
                        break;

                    case CSAInternalCommandTypes.GameSummaryReceived:
                        try {
                            lock (client.CurrentBoard) {
                                board = Board.FromNotation(client.InitialPosition);
                                BoardData boardData = board.ToBoardData(); //手抜き
                                for (int i = 0; i < client.Moves.Count; i++) {
                                    boardData.Do(client.Moves[i]);
                                    AddMoveToListForAppliedBoard(boardData.Turn ^ 1, boardData, new MoveDataEx { MoveData = client.Moves[i], Time = client.SecondsList[i] * 1000 }, true, i + 1 == client.Moves.Count);
                                    board.Do(ShogiCore.Move.FromNotation(board, client.Moves[i]));
                                }
                                lock (Board) {
                                    Board = board.Clone();
                                }
                            }
                            // 応答を送信
                            client.SendAgree();
                            // 対局開始局面を描画
                            blunderViewControl.DrawAsync(board);
                            // 持ち時間
                            timeData[0].TimeA = client.GameSummary.Times[0].Total_Time * 1000;
                            timeData[0].TimeB = client.GameSummary.Times[0].Byoyomi * 1000;
                            timeData[1].TimeA = client.GameSummary.Times[1].Total_Time * 1000;
                            timeData[1].TimeB = client.GameSummary.Times[0].Byoyomi * 1000;
                            timeData[0].Reset();
                            timeData[1].Reset();
                            // 対局開始時の情報表示
                            FormUtility.SafeInvoke(this, () => {
                                // エンジン情報欄を初期化
                                engineViewControl1.Board = board;
                                engineViewControl1.Clear();
                                // 情報表示
                                SetTitleStatusText("通信対局中：" + client.HostName);
                                // 対局者名などの表示更新
                                playerInfoControlP.PlayerName = client.GameSummary.NameP;
                                playerInfoControlP.TimeASeconds = client.GameSummary.Times[0].Total_Time;
                                playerInfoControlP.TimeBSeconds = client.GameSummary.Times[0].Byoyomi;
                                playerInfoControlN.PlayerName = client.GameSummary.NameN;
                                playerInfoControlN.TimeASeconds = client.GameSummary.Times[1].Total_Time;
                                playerInfoControlN.TimeBSeconds = client.GameSummary.Times[1].Byoyomi;
                                playerInfoControlP.Reset();
                                playerInfoControlN.Reset();
                                ClearMoveList();
                            });
                        } catch (Exception e) {
                            logger.Warn(e);
                            // なんかエラったらとりあえずREJECT。。
                            client.SendReject();
                        }
                        break;

                    case CSAInternalCommandTypes.Start:
                        // CSA棋譜の作成
                        csaFileWriter.Create(
                            client.GameSummary.NameP,
                            client.GameSummary.NameN,
                            client.GameSummary.Game_ID,
                            client.InitialPositionString);

                        FormUtility.SafeInvoke(this, () => {
                            GetPlayerInfoControl(board.Turn).StartTurn();
                        });
                        // 先手なら指し手を返す
                        if (client.GameSummary.To_Move == client.GameSummary.Your_Turn) {
                            NetworkGameDoTurn(client, board);
                        }
                        break;

                    case CSAInternalCommandTypes.SelfMove: // 自分の指し手がサーバから応答された場合
                        // 棋譜へ記録
                        csaFileWriter.AppendMove(command.ReceivedString, lastMoveComment);
                        // 指し手受信時の画面更新
                        timeData[board.Turn ^ 1].ConsumeTime(command.MoveDataEx.Time);
                        NetworkGameUpdateOnMoveReceived(client, board, command.MoveDataEx, lastMoveValue, lastMoveComment);
                        break;

                    case CSAInternalCommandTypes.EnemyMove: // 相手の指し手がサーバから来た場合
                        // 棋譜へ記録
                        csaFileWriter.AppendMove(command.ReceivedString);
                        // 受信した指し手でboardを進める。
                        board.Do(ShogiCore.Move.FromNotation(board, command.MoveDataEx.MoveData));
                        // 指し手受信時の画面更新
                        timeData[0].RemainTime = client.FirstTurnRemainSeconds * 1000;
                        timeData[1].RemainTime = client.SecondTurnRemainSeconds * 1000;
                        playerInfoControlP.RemainSeconds = client.FirstTurnRemainSeconds;
                        playerInfoControlN.RemainSeconds = client.SecondTurnRemainSeconds;
                        NetworkGameUpdateOnMoveReceived(client, board, command.MoveDataEx);
                        // 自分の番の処理
                        NetworkGameDoTurn(client, board);
                        break;

                    case CSAInternalCommandTypes.SpecialMove: // %TORYOなどがサーバから来た場合
                        // 棋譜へ記録
                        csaFileWriter.AppendMove(command.ReceivedString);
                        break;

                    case CSAInternalCommandTypes.Disconnected:
                        break;

                    default:
                        break;
                }
            }
            // 終局理由を画面へ
            if (csaFileWriter.Created) { // 対局が開始していたのかどうか(手抜き判定)
                switch (csaClient.LastGameEndReason) {
                    case GameEndReason.Resign: AddMoveToListForAppliedBoard(client.CurrentBoard.Turn, client.CurrentBoard, new MoveDataEx(MoveData.Resign)); break;
                    case GameEndReason.Nyuugyoku: AddMoveToListForAppliedBoard(client.CurrentBoard.Turn, client.CurrentBoard, new MoveDataEx(MoveData.Win)); break;
                    default: AddMoveToList(GameEndReasonUtility.ToString(csaClient.LastGameEndReason)); break;
                }
            }
        }

        /// <summary>
        /// 手番時の処理。player.DoTurn()して、結果の指し手でboardを進める。
        /// </summary>
        private void NetworkGameDoTurn(CSAClient client, Board board) {
            FormUtility.SafeInvoke(this, () => {
                // 自分の時間消費の開始
                GetPlayerInfoControl(board.Turn).StartTurn();
                // エンジンの表示をクリア
                engineViewControl1.Clear();
            });
            // 思考
            Move move = Players[0].DoTurn(board,
                client.FirstTurnRemainSeconds * 1000,
                client.SecondTurnRemainSeconds * 1000,
                client.GameSummary.Times[board.Turn].Byoyomi * 1000);
            // 評価値・読み筋
            USIPlayer usiPlayer = Players[0] as USIPlayer; // TODO: 整理
            if (usiPlayer == null || !usiPlayer.HasScore) { // scoreを受信してないならコメント無し。(PVが無くてもscoreがあればコメントあり)
                lastMoveComment = "";
                lastMoveValue = null;
            } else {
                lastMoveComment = BuildComment(client, board, move, usiPlayer);
                lastMoveValue = Board.NegativeByTurn(usiPlayer.LastScore, board.Turn);
            }
            // 指し手を送信
            if (move.IsSpecialState) {
                if (move == ShogiCore.Move.Win) { // 入玉勝ち宣言
                    client.SendKachi();
                } else if (move == ShogiCore.Move.Resign) { // 投了
                    client.SendToryo(lastMoveComment);
                } else {
                    logger.Warn("不正な指し手: " + move.ToString(board));
                }
            } else { // 指し手
                client.SendMove(move.ToNotation(), lastMoveComment);
            }
            board.Do(move);
            FormUtility.SafeInvoke(this, () => {
                // 相手の時間消費の開始
                GetPlayerInfoControl(board.Turn).StartTurn();
            });
        }

        /// <summary>
        /// 評価値・読み筋
        /// </summary>
        private string BuildComment(CSAClient client, Board board, Move move, USIPlayer usiPlayer) {
            try {
                BoardData b;
                lock (client.CurrentBoard) {
                    b = client.CurrentBoard.Clone();
                }
                return BuildComment(usiPlayer, b, move);
            } catch (Exception e) {
                logger.Warn("PV構築時に例外発生: " + move.ToString(board), e);
                return ""; // コメント無し
            }
        }

        /// <summary>
        /// 評価値・読み筋
        /// </summary>
        /// <param name="b">局面は破壊するので注意</param>
        private static string BuildComment(USIPlayer usiPlayer, BoardData b, Move move) {
            try {
                int score = usiPlayer.LastScore;
                if (b.Turn != 0) score = -score; // 後手番なら符号を反転
                string pvString = "";
                var moves = usiPlayer.LastPV.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => SFENNotationReader.ToMoveData(x));
                if (0 < moves.Count()) {
                    // PVの先頭がこれから指す手なら(通常はそのはず)、その手をskip。
                    MoveData curMoveData = move.ToNotation();
                    if (moves.First() == curMoveData) {
                        moves = moves.Skip(1);
                        b.Do(curMoveData); // 局面はPV生成用に進めておく。
                    }
                    pvString = PCLNotationWriter.ToString(b, moves);
                }
                return "* " + score.ToString() + " " + pvString;
            } catch (Exception e) {
                logger.Warn("PV構築時に例外発生: " + move.ToString(), e);
                return ""; // コメント無し
            }
        }

        /// <summary>
        /// 指し手受信時の更新処理
        /// </summary>
        /// <param name="board">boardはmoveDataEx.MoveDataが適用済みの局面</param>
        private void NetworkGameUpdateOnMoveReceived(CSAClient client, Board board, MoveDataEx moveDataEx, int? value = null, string comment = null) {
            // リストへ追加
            moveDataEx.Value = value;
            moveDataEx.Comment = comment;
            var b = board.ToBoardData();
            AddMoveToListForAppliedBoard(b.Turn ^ 1, b, moveDataEx);
            // 時間消費を終了
            FormUtility.SafeInvoke(this, () => {
                GetPlayerInfoControl(b.Turn ^ 1).EndTurn();
            });

        }

        #endregion

        #region 検討・詰将棋解答

        class SingleEngineThreadParameters {
            public bool IsThink { get; set; }
            public Board Board { get; set; }
        }

        /// <summary>
        /// 検討開始
        /// </summary>
        void StartThink() {
            HidePlayer2EngineView();
            ClearToolStripLabelResult();
            SetEnabledForGame(true, true);
            threadValid = true;
            singleEngineThread = new Thread(SingleEngineThread);
            singleEngineThread.Start(new SingleEngineThreadParameters {
                IsThink = true,
                Board = Board.Clone(),
            });
        }

        /// <summary>
        /// 詰将棋解答
        /// </summary>
        void StartMate() {
            HidePlayer2EngineView();
            ClearToolStripLabelResult();
            SetEnabledForGame(true, true);
            threadValid = true;
            singleEngineThread = new Thread(SingleEngineThread);
            singleEngineThread.Start(new SingleEngineThreadParameters {
                IsThink = false,
                Board = Board.Clone(),
            });
        }

        /// <summary>
        /// 検討・詰将棋解答スレッドの停止
        /// </summary>
        void StopSingleEngineThread() {
            if (singleEngineThread == null || !singleEngineThread.IsAlive) {
                singleEngineThread = null;
                return;
            }
            var player = Players[0] as USIPlayer;
            if (player != null) {
                player.Abort();
            }
        }

        /// <summary>
        /// 検討・詰将棋解答スレッドの強制停止
        /// </summary>
        void AbortSingleEngineThread() {
            if (singleEngineThread == null || !singleEngineThread.IsAlive) {
                singleEngineThread = null;
                return;
            }
            threadValid = false;
            var player = Players[0] as USIPlayer;
            if (player != null) {
                player.Driver.SendQuit();
            }
        }

        /// <summary>
        /// 検討・詰将棋解答スレッド
        /// </summary>
        void SingleEngineThread(object arg) {
            string typeName = "検討";
            try {
                var p = (SingleEngineThreadParameters)arg;
                typeName = p.IsThink ? "検討" : "詰将棋解答";

                FormUtility.SafeInvoke(this, () => {
                    engineViewControl1.Clear();
                    engineViewControl2.Hide();
                    splitContainer3.Panel2Collapsed = true;
                });

                var engine = p.IsThink ?
                    configLoader.EngineList.Select(configLoader.VolatileConfig.ThinkEngineName, configLoader.VolatileConfig.ThinkEnginePath) :
                    configLoader.EngineList.Select(configLoader.VolatileConfig.MateEngineName, configLoader.VolatileConfig.MateEnginePath);
                if (!File.Exists(USIDriver.NormalizeEnginePath(engine.Path))) {
                    FormUtility.SafeInvoke(this, () => {
                        MessageBox.Show(this, typeName + "用のエンジンが存在しません。", "エラー");
                    });
                    return;
                }
                using (USIPlayer player = CreateUSIPlayer(engine, 1)) {
                    Players[0] = player;
                    engineViewControl1.Attach(player);
                    try {
                        SetTitleStatusText(typeName + "中：" + engine.Name + " (" + System.IO.Path.GetFileName(engine.Path) + ")");
                        engineViewControl1.Board = p.Board;
                        if (!player.Driver.GameStarted) {
                            player.Driver.SendUSINewGame();
                        }
                        Notation notation = p.Board.ToNotation();
                        player.Driver.SendPosition(notation);

                        if (p.IsThink) {
                            DoThink(p, player);
                        } else {
                            DoMate(p, player);
                        }
                    } finally {
                        engineViewControl1.Detach();
                    }
                }
            } catch (Exception e) {
                logger.Warn(typeName + "時にエラー発生", e);
                FormUtility.SafeInvoke(this, () => {
                    MessageBox.Show(this, typeName + "時にエラー発生: " + e.Message, "エラー");
                });
            } finally {
                FormUtility.SafeInvoke(this, () => {
                    SetTitleStatusText(null);
                    SetEnabledForGame(false, true);
                });
            }
        }

        /// <summary>
        /// 検討
        /// </summary>
        private void DoThink(SingleEngineThreadParameters p, USIPlayer player) {
            Stopwatch sw = Stopwatch.StartNew();
            player.Driver.SendGoInfinite();
            while (threadValid) {
                USICommand command;
                if (!player.Driver.TryReceiveCommand(Timeout.Infinite, out command)) {
                    break;
                }
                if (!threadValid) break;
                if (command.Name == "bestmove") {
                    sw.Stop();
                    engineViewControl1.AddListItem(time: sw.ElapsedMilliseconds.ToString("#,##0"), pvOrString: "指し手：" + SFENNotationReader.ToMoveData(command.Parameters).ToString(p.Board.ToBoardData()));
                    break;
                }
            }
        }

        /// <summary>
        /// 詰将棋解答
        /// </summary>
        private void DoMate(SingleEngineThreadParameters p, USIPlayer player) {
            Stopwatch sw = Stopwatch.StartNew();
            player.Driver.SendGoMateInfinite();
            while (threadValid) {
                USICommand command;
                if (!player.Driver.TryReceiveCommand(Timeout.Infinite, out command)) {
                    break;
                }
                if (!threadValid) break;
                if (command.Name == "checkmate") {
                    sw.Stop();
                    switch (command.Parameters) {
                        case "notimplemented":
                            engineViewControl1.AddListItem(pvOrString: "エンジンが詰将棋解答に対応していません。");
                            return;

                        case "timeout":
                            engineViewControl1.AddListItem(time: sw.ElapsedMilliseconds.ToString("#,##0"), pvOrString: "時間内に詰みを見つけることが出来ませんでした。");
                            return;

                        case "nomate":
                            engineViewControl1.AddListItem(time: sw.ElapsedMilliseconds.ToString("#,##0"), pvOrString: "不詰です。");
                            return;

                        default:
                            break;
                    }

                    FormUtility.SafeInvoke(this, () => {
                        // Boardをp.Boardの局面で始まる形にする
                        lock (Board) {
                            Board.CopyFrom(p.Board);
                            Board.History.Clear();
                        }

                        // リストを詰み手順だけにする
                        ClearMoveList();
                        BoardData boardData = p.Board.ToBoardData();
                        foreach (string moveStr in (command.Parameters ?? "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)) {
                            MoveData moveData = SFENNotationReader.ToMoveData(moveStr);
                            boardData.Do(moveData);
                            AddMoveToListForAppliedBoard(boardData.Turn ^ 1, boardData, new MoveDataEx(moveData));
                        }

                        // メッセージ表示
                        engineViewControl1.AddListItem(time: sw.ElapsedMilliseconds.ToString("#,##0"), pvOrString: "詰みました。");
                    });
                }
            }
        }

        #endregion

        /// <summary>
        /// 通信対局時・終了時のメニューのEnabled制御
        /// </summary>
        /// <param name="start">開始時true、終了時false</param>
        private void SetEnabledForNetworkGame(bool start) {
            // 対局全般
            SetEnabledForGame(start, false);
            // 通信対局固有のもの
            通信対局NToolStripMenuItem.Enabled = !start;
            通信対局切断DToolStripMenuItem.Enabled = start;
            toolStripButton切.Enabled = start;
        }

        /// <summary>
        /// 対局時・終了時のメニューのEnabled制御
        /// </summary>
        /// <param name="start">開始時true、終了時false</param>
        private void SetEnabledForGame(bool start, bool abortable) {
            棋譜局面貼り付けVToolStripMenuItem.Enabled = !start;
            検討TToolStripMenuItem.Enabled = !start;
            詰将棋解答MToolStripMenuItem.Enabled = !start;
            中断AToolStripMenuItem.Enabled = abortable && start;
            対局GToolStripMenuItem.Enabled = !start;
            通信対局NToolStripMenuItem.Enabled = !start;
            //toolStripButton投.Enabled =
            //toolStripButton待.Enabled =
            //toolStripButton急.Enabled =
            toolStripButton中.Enabled = abortable && start;
            toolStripButton検.Enabled = !start;
            toolStripButton詰.Enabled = !start;
        }

        /// <summary>
        /// 2個目のエンジン情報表示部分を表示する
        /// </summary>
        private void ShowPlayer2EngineView() {
            splitContainer3.Panel2Collapsed = false;
            engineViewControl2.Show();
        }
        /// <summary>
        /// 2個目のエンジン情報表示部分を隠す
        /// </summary>
        private void HidePlayer2EngineView() {
            engineViewControl2.Hide();
            splitContainer3.Panel2Collapsed = true;
        }
        /// <summary>
        /// ツールバーのラベル部分を消す
        /// </summary>
        private void ClearToolStripLabelResult() {
            toolStripLabelResult.Text = "";
            toolStripLabelResult.ToolTipText = "";
        }

        /// <summary>
        /// USIPlayerの作成
        /// </summary>
        private USIPlayer CreateUSIPlayer(Engine engine, int logID) {
            USIPlayer player = null;
            try {
                SetTitleStatusText("USIエンジン起動中：" + engine.Name + " (" + System.IO.Path.GetFileName(engine.Path) + ")");
                player = new USIPlayer(engine.Path, null, logID);
                player.SetOption("USI_Ponder", engine.USIPonder ? "true" : "false");
                player.SetOption("USI_Hash", engine.USIHash.ToString());
                foreach (var p in engine.Options) {
                    player.SetOption(p.Name, p.Value);
                }
                player.GameStart();
                return player;
            } catch (Exception e) {
                try {
                    if (player != null) player.Dispose();
                } catch (Exception e2) {
                    logger.Info("USIエンジン起動失敗後の後処理に失敗", e2);
                }
                throw new USIEngineException("USIエンジンの起動に失敗しました。", e);
            }
        }

        /// <summary>
        /// playerのindexからEngineViewControlを取得。
        /// </summary>
        private EngineViewControl GetEngineViewControl(int playerIndex) {
            return playerIndex == 0 ? engineViewControl1 : engineViewControl2;
        }
        /// <summary>
        /// 手番からPlayerInfoControlを取得
        /// </summary>
        private PlayerInfoControl GetPlayerInfoControl(int turn) {
            return turn == 0 ? playerInfoControlP : playerInfoControlN;
        }

        /// <summary>
        /// 指し手リストを空にする
        /// </summary>
        private void ClearMoveList() {
            listBox1.Items.Clear();
            listBox1.Items.Add(new MoveListBoxItem(" === 開始局面 ===", new MoveDataEx(new MoveData())));
            gameGraphControl1.ClearAsync();
        }

        /// <summary>
        /// 指し手をリストへ追加
        /// </summary>
        /// <param name="b">moveが適用済みの局面</param>
        private void AddMoveToListForAppliedBoard(int turn, BoardData b, MoveDataEx moveDataEx, bool selectLast = true, bool updateGraph = true) {
            MoveData move = moveDataEx.MoveData;
            string moveString = move.IsPut || move.IsSpecialMove ?
                move.ToString(turn, Piece.EMPTY) :
                move.ToString(turn, b[move.ToFile, move.ToRank] ^ move.PromoteMask); // moveが適用済みなので移動先から駒の種類を取得
            AddMoveToList(turn, moveDataEx, selectLast, updateGraph, moveString);
        }

        /// <summary>
        /// 指し手をリストへ追加
        /// </summary>
        private void AddMoveToList(int turn, MoveDataEx moveDataEx, bool selectLast, bool updateGraph, string moveString) {
            FormUtility.SafeInvoke(this, () => {
                // 手数。リストの最初が「=== 開始局面 ===」なので、今入っている個数をそのまま表示（←適当）
                int moveCount = listBox1.Items.Count;
                // 指し手の文字列化
                const int MaxMoveStringLength = 12;
                moveString = moveString + new string(' ', Math.Max(0, MaxMoveStringLength - Encoding.GetEncoding(932).GetByteCount(moveString)));
                // リストへ追加
                if (moveDataEx.Time < 0) {
                    listBox1.Items.Add(new MoveListBoxItem(string.Format("{0,4} {1}", moveCount, moveString), moveDataEx));
                } else {
                    listBox1.Items.Add(new MoveListBoxItem(string.Format("{0,4} {1} {2,4}秒", moveCount, moveString, moveDataEx.Time / 1000), moveDataEx));
                }
                if (selectLast) {
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }
                // ついでにフォーカスもここにしておく
                listBox1.Focus();
                // グラフも描画
                if (moveDataEx.Value.HasValue || 0 <= timeData[turn].TimeA) {
                    gameGraphControl1.AddAsync(moveCount - 1, moveDataEx.Value ?? int.MinValue, timeData[turn].RemainTime, timeData[turn].TimeA, updateGraph);
                }
            });
        }

        /// <summary>
        /// 指し手をリストへ追加
        /// </summary>
        private void AddMoveToList(string moveString, bool selectLast = true) {
            FormUtility.SafeInvoke(this, () => {
                listBox1.Items.Add(new MoveListBoxItem(string.Format("{0,4} {1}", "", moveString), new MoveDataEx(new MoveData())));
                if (selectLast) {
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }
            });
        }

        /// <summary>
        /// タイトルバーの表示を変更
        /// </summary>
        private void SetTitleStatusText(string statusText) {
            if (InvokeRequired) {
                FormUtility.SafeInvoke(this, () => { SetTitleStatusText(statusText); });
            } else {
                string text = string.IsNullOrEmpty(statusText) ? "ShogiBoard" : "ShogiBoard - " + statusText;
                Text = text;
                logger.DebugFormat("Text = {0}", text);
            }
        }

        /// <summary>
        /// BoardからNotationを作成
        /// </summary>
        private Notation GetNotation() {
            Notation notation;
            lock (Board) {
                notation = Board.ToNotation();
                notation.FirstPlayerName = playerInfoControlP.PlayerName;
                notation.SecondPlayerName = playerInfoControlN.PlayerName;
            }
            return notation;
        }

        /// <summary>
        /// BoardからNotationを作成(現在局面)
        /// </summary>
        private Notation GetPositionNotation() {
            Notation notation = new Notation();
            lock (Board) {
                notation.InitialBoard = Board.ToBoardData();
                notation.FirstPlayerName = playerInfoControlP.PlayerName;
                notation.SecondPlayerName = playerInfoControlN.PlayerName;
            }
            return notation;
        }
    }
}
