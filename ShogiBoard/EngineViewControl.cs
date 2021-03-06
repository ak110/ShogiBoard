﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShogiCore;
using ShogiCore.Notation;
using ShogiCore.USI;
using System.Threading;

namespace ShogiBoard {
    public partial class EngineViewControl : UserControl {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        USIPlayer player;
        EngineStatisticsForGame stat = new EngineStatisticsForGame(); // ダミーを入れておく

        /// <summary>
        /// 局面(PV構築など用に参照を設定しておく)
        /// </summary>
        public Board Board { get; set; }

        public EngineViewControl() {
            InitializeComponent();
        }

        private void EngineViewControl_Load(object sender, EventArgs e) {
            // デザイナ用データ削除
            if (!DesignMode) {
                Clear();
                labelEngine.Text = ""; // エンジン名
                labelPonder.Text = "予想手：";
            }
        }

        /// <summary>
        /// 空にする
        /// </summary>
        public void Clear() {
            labelNPS.Text = "NPS：";
            labelCurMove.Text = "探索手：";
            labelHashFull.Text = "ハッシュ使用率：";
            labelMeanDepth.Text = "平均深さ：-";
            labelMeanNPS.Text = "平均NPS：-";
            listView1.Items.Clear();
        }

        /// <summary>
        /// アタッチ
        /// </summary>
        /// <param name="player"></param>
        public void Attach(USIPlayer player) {
            this.player = player;
            player.CommandReceived += Player_CommandReceived;
            player.InfoReceived += Player_InfoReceived;
            player.PositionChanged += Player_PositionChanged;
            player.PonderStarting += Player_PonderStarting;
            player.PonderHit += Player_PonderHit;
            player.PonderStopped += Player_PonderStopped;
        }

        /// <summary>
        /// デタッチ
        /// </summary>
        /// <param name="player"></param>
        public void Detach() {
            if (player != null) {
                player.CommandReceived -= Player_CommandReceived;
                player.InfoReceived -= Player_InfoReceived;
                player.PositionChanged -= Player_PositionChanged;
                player.PonderStarting -= Player_PonderStarting;
                player.PonderHit -= Player_PonderHit;
                player.PonderStopped -= Player_PonderStopped;
                player = null;
            }
        }

        /// <summary>
        /// プレイヤー情報を更新
        /// </summary>
        public void GameStart(USIPlayer player, EngineStatisticsForGame stat) {
            this.stat = stat;
            FormUtility.SafeInvoke(this, () => {
                Clear();
                labelEngine.Text = player.Name; // プレイヤー名
                toolTip1.SetToolTip(labelEngine,
                    player.Name + Environment.NewLine +
                    "名前: " + player.Driver.IdName + Environment.NewLine +
                    "作者: " + player.Driver.IdAuthor);
            });
        }

        /// <summary>
        /// リストへ項目を追加
        /// </summary>
        /// <param name="time"></param>
        /// <param name="depth"></param>
        /// <param name="nodes"></param>
        /// <param name="score"></param>
        /// <param name="pvOrString"></param>
        /// <param name="toolTipText"></param>
        public void AddListItem(string time = null, string depth = null, string nodes = null, string score = null, string pvOrString = null, string toolTipText = null) {
            if (InvokeRequired) {
                FormUtility.SafeInvoke(this, () => { AddListItem(time, depth, nodes, score, pvOrString, toolTipText); });
                return;
            }
            listView1.Items.Insert(0, new ListViewItem(new[] {
                (time + "").PadLeft(8), // 時間
                depth, // 深さ
                nodes, // ノード
                score, // 評価値
                pvOrString, // 情報
            }) { ToolTipText = toolTipText });
        }

        void Player_CommandReceived(object sender, ShogiCore.USI.USICommandEventArgs e) {
        }

        void Player_InfoReceived(object sender, ShogiCore.USI.USIInfoEventArgs e) {
            string infoDepth = "";
            string infoSelDepth = "";
            string infoTime = "";
            string infoNodes = "";
            string infoNPS = "";
            string infoScore = "";
            string infoCurrMove = "";
            string infoHashFull = "";
            string infoPVOrString = null;
            string pvLengthString = null;
            bool lowerBound = false, upperBound = false;

            foreach (USIInfo info in e.SubCommands) {
                switch (info.Name) {
                    case "depth": infoDepth = info.Parameters.FirstOrDefault(); break;
                    case "seldepth": infoSelDepth = info.Parameters.FirstOrDefault(); break;
                    case "time": infoTime = info.Parameters.FirstOrDefault(); break;
                    case "nodes": infoNodes = info.Parameters.FirstOrDefault(); break;
                    case "nps": infoNPS = info.Parameters.FirstOrDefault(); break;
                    case "currmove": infoCurrMove = info.Parameters.FirstOrDefault(); break;
                    case "hashfull": infoHashFull = info.Parameters.FirstOrDefault(); break;
                    case "score":
                        if (player.LastScoreWasMate) {
                            int mateCount = Math.Abs(player.LastScore) - USIPlayer.MateValue;
                            string mateCountStr = mateCount == 0 ? "" : ":" + mateCount.ToString();
                            infoScore = (0 < player.LastScore ? "+Mate" : "-Mate") + mateCountStr;
                        } else {
                            infoScore = player.LastScoreString;
                        }
                        break;
                    case "lowerbound": lowerBound = true; break;
                    case "upperbound": upperBound = true; break;

                    case "pv": {
                            var pvList = info.Parameters;
                            BoardData b = Board == null ? null : Board.ToBoardData();
                            List<string> itemList = new List<string>();
                            int pvLength = 0;
                            foreach (var pv in pvList) {
                                try {
                                    MoveData moveData = SFENNotationReader.ToMoveData(pv);
                                    if (b == null) {
                                        itemList.Add(moveData.ToString());
                                    } else {
                                        itemList.Add(moveData.ToString(b));
                                        b.Do(moveData);
                                    }
                                    pvLength++;
                                } catch (NotationException) {
                                    itemList.Add(pv);
                                }
                            }
                            infoPVOrString = string.Concat(itemList.ToArray());
                            pvLengthString = "PV長=" + pvLength;
                        }
                        break;

                    case "string":
                        infoPVOrString = string.Join(" ", info.Parameters);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(infoSelDepth)) {
                infoDepth += "/" + infoSelDepth;
            }
            long time, nodes, hashFull;
            if (long.TryParse(infoTime, out time)) {
                infoTime = time.ToString("#,##0");
            }
            if (long.TryParse(infoNodes, out nodes)) {
                infoNodes = nodes.ToString("#,##0");
            }
            if (long.TryParse(infoHashFull, out hashFull)) {
                infoHashFull = (hashFull / 10.0).ToString("0.0").PadLeft(5) + "%";
            }
            if (lowerBound) infoScore += "↑";
            if (upperBound) infoScore += "↓";

            string toolTipText = pvLengthString;
            if (!string.IsNullOrEmpty(infoDepth)) {
                toolTipText = "深さ=" + infoDepth + " " + toolTipText;
            }

            try {
                FormUtility.SafeInvoke(this, () => {
                    if (!string.IsNullOrEmpty(infoNPS)) {
                        long nps;
                        labelNPS.Text = long.TryParse(infoNPS, out nps) ? "NPS：" + nps.ToString("#,##0") : "NPS：" + infoNPS;
                    }
                    if (!string.IsNullOrEmpty(infoCurrMove)) {
                        if (Board == null) {
                            labelCurMove.Text = "探索手：" + SFENNotationReader.ToMoveData(infoCurrMove).ToString();
                        } else {
                            labelCurMove.Text = "探索手：" + ShogiCore.Move.FromNotation(Board,
                                SFENNotationReader.ToMoveData(infoCurrMove)).ToString(Board);
                        }
                    }
                    if (!string.IsNullOrEmpty(infoHashFull)) {
                        labelHashFull.Text = "ハッシュ使用率：" + infoHashFull;
                    }
                    if (!string.IsNullOrEmpty(infoPVOrString)) {
                        AddListItem(infoTime, infoDepth, infoNodes, infoScore, infoPVOrString, pvLengthString);
                    }
                    double? meanDepth = stat.MeanDepth;
                    double? meanNPS = stat.MeanNPS;
                    if (meanDepth.HasValue) {
                        labelMeanDepth.Text = "平均深さ：" + meanDepth.Value.ToString("#0.0");
                    } else {
                        labelMeanDepth.Text = "平均深さ：-";
                    }
                    if (meanNPS.HasValue) {
                        labelMeanNPS.Text = "平均NPS：" + meanNPS.Value.ToString("#,##0");
                    } else {
                        labelMeanNPS.Text = "平均NPS：-";
                    }
                });
            } catch {
                // 無視
            }
        }

        private void Player_PositionChanged(object sender, DataEventArgs<Board> e) {
            Board = e.Data;
            FormUtility.SafeInvoke(this, () => { Clear(); });
        }

        private void Player_PonderStarting(object sender, DataEventArgs<Board> e) {
            // 予想手を表示
            var ponder = SFENNotationReader.ToMoveData(player.LastPonderMove).ToString(e.Data.ToBoardData());
            FormUtility.SafeInvoke(this, () => { labelPonder.Text = "予想手：" + ponder; });
        }

        private void Player_PonderHit(object sender, DataEventArgs<Board> e) {
            // 予想手をクリア
            FormUtility.SafeInvoke(this, () => { labelPonder.Text = "予想手："; });
        }

        private void Player_PonderStopped(object sender, DataEventArgs<Board> e) {
            // 予想手をクリア
            FormUtility.SafeInvoke(this, () => { labelPonder.Text = "予想手："; });
        }


        private void EngineViewControl_SizeChanged(object sender, EventArgs e) {
            AdjustLayout();
        }

        private void listView1_Layout(object sender, LayoutEventArgs e) {
            AdjustLayout();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e) {
            選択した読み筋をコピーCToolStripMenuItem.Enabled = listView1.SelectedItems.Count == 1;
        }

        private void 選択した読み筋をコピーCToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count == 1) {
                ListViewItem item = listView1.SelectedItems[0] as ListViewItem;
                if (item != null && 5 <= item.SubItems.Count) {
                    Clipboard.SetText("[" + item.SubItems[3].Text + "] " + item.SubItems[4].Text);
                }
            }
        }

        private void 全ての読み筋をコピーAToolStripMenuItem_Click(object sender, EventArgs e) {
            StringBuilder str = new StringBuilder();
            foreach (ListViewItem item in listView1.Items) {
                if (item != null && 5 <= item.SubItems.Count) {
                    str.AppendLine("[" + item.SubItems[3].Text + "] " + item.SubItems[4].Text);
                }
            }
            if (0 < str.Length) {
                Clipboard.SetText(str.ToString());
            }
        }

        bool adjusting = false; // 怪しいフラグ
        /// <summary>
        /// 列幅の調整
        /// </summary>
        private void AdjustLayout() {
            if (adjusting) return;
            adjusting = true;
            listView1.SuspendLayout();
            try {
                int hw = listView1.Columns.Cast<ColumnHeader>().Sum(x => x.Width) - columnHeaderPV.Width;
                int pvW = listView1.Width - hw -
                    SystemInformation.Border3DSize.Width * 2 - // 違う気がするが…
                    SystemInformation.HorizontalScrollBarHeight;
                bool widthChanged = columnHeaderPV.Width != pvW;
                columnHeaderPV.Width = pvW;
                // 最大化から戻したときになんか横スクロールバーが出てしまうので怪しい対策
                if (widthChanged) {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(arg => {
                        FormUtility.SafeInvoke(this, () => {
                            listView1.Scrollable = false;
                            Application.DoEvents();
                            listView1.Scrollable = true;
                        });
                    }));
                }
            } catch (Exception e) {
                logger.Warn(e);
            } finally {
                listView1.ResumeLayout();
                adjusting = false;
            }
        }
    }
}
