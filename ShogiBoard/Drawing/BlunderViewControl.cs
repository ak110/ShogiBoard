using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ShogiCore;

namespace ShogiBoard.Drawing {
    /// <summary>
    /// BlunderGraphicsを用いた表示を行うコントロール
    /// </summary>
    public partial class BlunderViewControl : UserControl {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        BlunderGraphics graphics = new BlunderGraphics();
        Bitmap bitmap;

        Queue<Board> drawingQueue = new Queue<Board>();
        Thread drawingThread = null;
        volatile bool threadValid = false;

        public BlunderViewControl() {
            InitializeComponent();
            Disposed += new EventHandler(BlunderViewControl_Disposed);

            ClientSize = graphics.Size;
            bitmap = new Bitmap(graphics.Width, graphics.Height);
            using (Graphics g = Graphics.FromImage(bitmap)) {
                g.FillRectangle(Brushes.Black, 0, 0, graphics.Width, graphics.Height);
            }
        }

        /// <summary>
        /// 後始末
        /// </summary>
        void BlunderViewControl_Disposed(object sender, EventArgs e) {
            lock (drawingQueue) {
                threadValid = false;
                drawingQueue.Clear();
                Monitor.Pulse(drawingQueue);
            }
            if (drawingThread != null) {
                drawingThread.Join();
            }
            bitmap.Dispose();
            graphics.Dispose();
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw(Board board) {
            lock (bitmap) {
                using (Graphics g = Graphics.FromImage(bitmap)) {
                    graphics.Draw(g, board);
                }
            }
            Invalidate();
            Update();
        }

        /// <summary>
        /// 描画キューへ追加して非同期描画
        /// </summary>
        public void DrawAsync(Board board) {
            if (DesignMode) return; // デザインモードではなんかバグるのでとりあえず描画しない _no
            lock (drawingQueue) {
                drawingQueue.Enqueue(board.Clone());
                Monitor.Pulse(drawingQueue);
                if (drawingThread == null) {
                    threadValid = true;
                    drawingThread = new Thread(DrawingThread);
                    drawingThread.Start();
                }
            }
        }

        /// <summary>
        /// 描画スレッド
        /// </summary>
        private void DrawingThread() {
            IAsyncResult ar = null;
            while (true) {
                try {
                    Board board;
                    lock (drawingQueue) {
                        if (!threadValid) break;
                        if (0 < drawingQueue.Count) {
                            board = drawingQueue.Dequeue();
                        } else {
                            Monitor.Wait(drawingQueue);
                            continue;
                        }
                    }

                    // 前回の描画が完了していなければ待機
                    if (ar != null) EndInvoke(ar);
                    // bitmapを準備
                    lock (bitmap) {
                        using (Graphics g = Graphics.FromImage(bitmap)) {
                            graphics.Draw(g, board);
                        }
                    }
                    // 次の描画
                    if (IsDisposed) break; // 念のため
                    ar = BeginInvoke(new MethodInvoker(() => {
                        if (!IsDisposed) {
                            Invalidate();
                            Update();
                        }
                    }));
                } catch (Exception e) {
                    logger.Warn("描画スレッドで例外発生", e);
                }
            }
        }

        /// <summary>
        /// 背景の描画
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e) {
            // 何も処理させない。
        }

        /// <summary>
        /// 描画
        /// </summary>
        private void BlunderViewControl_Paint(object sender, PaintEventArgs e) {
            lock (bitmap) {
                e.Graphics.DrawImage(bitmap,
                    e.ClipRectangle.X, e.ClipRectangle.Y,
                    e.ClipRectangle, GraphicsUnit.Pixel);
            }
        }
    }
}
