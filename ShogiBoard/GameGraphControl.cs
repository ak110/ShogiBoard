using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ShogiBoard {
    public partial class GameGraphControl : UserControl {
        /// <summary>
        /// 1手あたりのBitmapの幅
        /// </summary>
        const int WidthPerMoves = 4;
        /// <summary>
        /// 評価値の点の高さ
        /// </summary>
        const int ValueLineHeight = 8;
        /// <summary>
        /// Bitmapの高さ
        /// </summary>
        const int BitmapHeight = 200;
        /// <summary>
        /// 最初の表示可能手数
        /// </summary>
        const int InitialMoveCount = 150;

        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        object bitmapLock = new object();
        Bitmap bitmap;
        int bitmapWidth;
        Pen[] valuePens = new[] {
            new Pen(Color.Black, ValueLineHeight),
            new Pen(Color.White, ValueLineHeight),
        };

        struct GraphData {
            public int MoveCount;
            public int Value;
            public double Time;
        }
        List<GraphData> graphData = new List<GraphData>();

        Queue<Func<bool>> drawingQueue = new Queue<Func<bool>>();
        volatile bool threadValid = true;
        Thread thread;

        int moveCount = 0;

        public GameGraphControl() {
            InitializeComponent();
            labelLine.Text = "";
            Disposed += GameGraphControl_Disposed;
            Clear();
            thread = new Thread(DrawingThread);
            thread.Start();
        }

        void GameGraphControl_Disposed(object sender, EventArgs e) {
            lock (drawingQueue) {
                threadValid = false;
                Monitor.PulseAll(drawingQueue);
            }
            thread.Join();

            bitmap.Dispose();
            foreach (Pen pen in valuePens) pen.Dispose();
        }

        private void panel2_Paint(object sender, PaintEventArgs e) {
            lock (bitmapLock) {
                e.Graphics.DrawImage(bitmap,
                    new Rectangle(0, 0, panel2.Width, panel2.Height),
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    GraphicsUnit.Pixel);
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void ClearAsync() {
            lock (drawingQueue) {
                drawingQueue.Enqueue(Clear);
                Monitor.Pulse(drawingQueue);
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        private bool Clear() {
            lock (bitmapLock) {
                if (bitmap != null) bitmap.Dispose();
                bitmapWidth = WidthPerMoves * InitialMoveCount;
                bitmap = new Bitmap(bitmapWidth, BitmapHeight);
                using (Graphics g = Graphics.FromImage(bitmap))
                using (Brush bgBrush = new SolidBrush(BackColor)) {
                    g.FillRectangle(bgBrush, 0, 0, bitmap.Width, BitmapHeight);
                    g.DrawLine(Pens.Gray, 0, BitmapHeight / 2, bitmap.Width - 1, BitmapHeight / 2);
                }
            }
            graphData.Clear();
            return true;
        }

        /// <summary>
        /// 評価値と思考時間を追加
        /// </summary>
        /// <param name="value">評価値。int.MinValueなら評価値無し。</param>
        /// <param name="remainTime">残り時間</param>
        /// <param name="timeA">持ち時間。0以下で描画無し。</param>
        /// <param name="update">更新するのか否か</param>
        public void AddAsync(int moveCount, int value, int remainTime, int timeA, bool update) {
            double time = timeA <= 0 ? -1.0 : (double)remainTime / timeA;
            lock (drawingQueue) {
                drawingQueue.Enqueue(() => Add(moveCount, value, time, update));
                Monitor.Pulse(drawingQueue);
            }
        }

        /// <summary>
        /// 評価値と思考時間を追加
        /// </summary>
        /// <param name="moveCount">0から始まる手数</param>
        /// <param name="value">評価値</param>
        /// <param name="time">残り時間の割合。負なら描画無し。</param>
        private bool Add(int moveCount, int value, double time, bool update) {
            graphData.Add(new GraphData { MoveCount = moveCount, Value = value, Time = time });
            if (!update) return false;

            // ビットマップのリサイズ
            int w = moveCount * WidthPerMoves;
            if (bitmapWidth * 7 / 8 <= w) {
                do {
                    bitmapWidth += WidthPerMoves * 50;
                } while (bitmapWidth * 7 / 8 <= w);
                lock (bitmapLock) {
                    bitmap.Dispose();
                    bitmap = new Bitmap(bitmapWidth, BitmapHeight);
                }
                // 手数の再設定
                FormUtility.SafeInvoke(this, () => { SetMoveCount(this.moveCount); });
            }

            DrawAll();

            return update;
        }

        /// <summary>
        /// 描画
        /// </summary>
        private void DrawAll() {
            lock (bitmapLock) {
                using (Graphics g = Graphics.FromImage(bitmap))
                using (Brush bgBrush = new SolidBrush(BackColor))
                using (Brush timeBrush = new SolidBrush(Color.FromArgb(unchecked((int)0xffF9B4A8)))) {
                    // 背景色
                    g.FillRectangle(bgBrush, 0, 0, bitmap.Width, BitmapHeight);
                    // 持ち時間
                    GraphData[] lastData = new[] { // 手番毎の一つ前のデータ。最初はダミー。
                        new GraphData { MoveCount = -1, Time = 1.0, },
                        new GraphData { MoveCount = -1, Time = 1.0, },
                    };
                    foreach (GraphData gr in graphData) {
                        int turn = gr.MoveCount % 2;
                        int xCurr = gr.MoveCount * WidthPerMoves;
                        int xLast = (lastData[turn].MoveCount + 1) * WidthPerMoves;
                        int timeWidthLast = xCurr - xLast + WidthPerMoves / 2;
                        int timeHeightLast = (int)(lastData[turn].Time * (BitmapHeight / 2));
                        int timeHeightCurr = (int)(gr.Time * (BitmapHeight / 2));
                        int timeYLast, timeYCurr;
                        if (turn == 0) {
                            timeYLast = BitmapHeight / 2;
                            timeYCurr = BitmapHeight / 2;
                        } else {
                            timeYLast = (BitmapHeight / 2) - timeHeightLast;
                            timeYCurr = (BitmapHeight / 2) - timeHeightCurr;
                        }
                        // 残り時間（塗りつぶす）
                        if (-double.Epsilon < gr.Time) {
                            g.FillRectangle(timeBrush, xLast, timeYLast, timeWidthLast, timeHeightLast);
                            g.FillRectangle(timeBrush, xCurr, timeYCurr, WidthPerMoves, timeHeightCurr);
                        }
                        lastData[turn] = gr;
                    }
                    // x軸
                    g.DrawLine(Pens.Gray, 0, BitmapHeight / 2, bitmap.Width - 1, BitmapHeight / 2);
                    // 評価値
                    const int zeroCenter = BitmapHeight / 2;
                    const int YCenterMin = 0 + ValueLineHeight / 2;
                    const int YCenterMax = BitmapHeight - ValueLineHeight / 2;
                    int[] lastX = new[] { 0, 0 };
                    int[] lastYCenter = new[] { BitmapHeight / 2, BitmapHeight / 2 }; // 0
                    foreach (GraphData gr in graphData) {
                        if (gr.Value == int.MinValue) continue; // 無効値はスキップ
                        int turn = gr.MoveCount % 2;
                        int xCurr = gr.MoveCount * WidthPerMoves;
                        int yCenter = Math.Min(Math.Max(
                            gr.Value * (BitmapHeight / 2) / 2000 + zeroCenter,
                            YCenterMin), YCenterMax);

                        g.DrawLine(valuePens[turn], lastX[turn], lastYCenter[turn], xCurr, yCenter);

                        lastX[turn] = xCurr;
                        lastYCenter[turn] = yCenter;
                    }
                }
            }
        }

        /// <summary>
        /// 描画スレッド
        /// </summary>
        void DrawingThread() {
            try {
                DrawingThreadImpl();
            } catch (Exception e) {
                logger.Error("グラフ描画スレッドで例外発生", e);
            }
        }
        /// <summary>
        /// 描画スレッド
        /// </summary>
        void DrawingThreadImpl() {
            IAsyncResult ar = null;
            while (true) {
                try {
                    Func<bool> action;
                    lock (drawingQueue) {
                        if (!threadValid) break;
                        if (0 < drawingQueue.Count) {
                            action = drawingQueue.Dequeue();
                        } else {
                            Monitor.Wait(drawingQueue);
                            continue;
                        }
                    }

                    // 前回の描画が完了していなければ待機
                    if (ar != null) EndInvoke(ar);
                    // actionを実行
                    bool update = action();
                    // 次の描画
                    if (update) {
                        if (IsDisposed) break; // 念のため
                        ar = BeginInvoke(new MethodInvoker(() => {
                            try {
                                if (!IsDisposed) {
                                    panel2.Invalidate();
                                    panel2.Update();
                                }
                            } catch {
                            }
                        }));
                    }
                } catch (Exception e) {
                    logger.Warn("グラフ描画スレッドで例外発生", e);
                }
            }
        }

        /// <summary>
        /// 手数の設定
        /// </summary>
        public void SetMoveCount(int moveCount) {
            this.moveCount = moveCount;
            if (moveCount == 0) {
                FormUtility.SafeInvoke(this, () => { labelLine.Visible = false; });
            } else {
                int x = moveCount * WidthPerMoves * panel2.Width / bitmapWidth;

                FormUtility.SafeInvoke(this, () => {
                    SuspendLayout();
                    try {
                        labelLine.Location = new Point(x, 0);
                        labelLine.Size = new Size(1, Height);
                        labelLine.Visible = true;
                    } catch {
                        ResumeLayout();
                    }
                });
            }
        }
    }
}
