using System;
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
            }
        }

        /// <summary>
        /// 空にする
        /// </summary>
        public void Clear() {
            labelNPS.Text = "NPS：";
            labelCurMove.Text = "探索手：";
            labelHashFull.Text = "ハッシュ使用率：";
            listView1.Items.Clear();
        }

        /// <summary>
        /// アタッチ
        /// </summary>
        /// <param name="player"></param>
        public void Attach(USIPlayer player) {
            this.player = player;
            player.CommandReceived += player_CommandReceived;
            player.InfoReceived += player_InfoReceived;
            labelEngine.Text = player.Driver.IdName; // エンジン名
        }

        /// <summary>
        /// デタッチ
        /// </summary>
        /// <param name="player"></param>
        public void Detach() {
            labelEngine.Text = ""; // エンジン名
            player.InfoReceived -= player_InfoReceived;
            player.CommandReceived -= player_CommandReceived;
            this.player = null;
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
                time.PadLeft(8), // 時間
                depth, // 深さ
                nodes, // ノード
                score, // 評価値
                pvOrString, // 情報
            }) { ToolTipText = toolTipText });
        }

        void player_CommandReceived(object sender, ShogiCore.USI.USICommandEventArgs e) {
        }

        void player_InfoReceived(object sender, ShogiCore.USI.USIInfoEventArgs e) {
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

            foreach (USIInfo info in e.SubCommands) {
                switch (info.Name) {
                    case "depth": infoDepth = info.Parameters; break;
                    case "seldepth": infoSelDepth = info.Parameters; break;
                    case "time": infoTime = info.Parameters; break;
                    case "nodes": infoNodes = info.Parameters; break;
                    case "nps": infoNPS = info.Parameters; break;
                    case "currmove": infoCurrMove = info.Parameters; break;
                    case "hashfull": infoHashFull = info.Parameters; break;
                    case "score":
                        if (player.LastScoreWasMate) {
                            int mateCount = Math.Abs(player.LastScore) - USIPlayer.MateValue;
                            string mateCountStr = mateCount == 0 ? "" : ":" + mateCount.ToString();
                            infoScore = (0 < player.LastScore ? "+Mate" : "-Mate") + mateCountStr;
                        } else {
                            infoScore = player.LastScore.ToString();
                        }
                        break;

                    case "pv":
                        var pvList = (info.Parameters ?? "").Split(' ');
                        infoPVOrString = Board == null ?
                            string.Join(" ", pvList.Select(x => SFENNotationReader.ToMoveData(x).ToString()).ToArray()) :
                            MoveUtility.ToPVString(pvList.Select(x => SFENNotationReader.ToMoveData(x)), -1, Board, Board.Turn);
                        pvLengthString = "PV長=" + pvList.Length;
                        break;

                    case "string":
                        infoPVOrString = info.Parameters;
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
                });
            } catch {
                // 無視
            }
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
