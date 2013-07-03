using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ShogiCore;

namespace ShogiBoard.Drawing {
    /// <summary>
    /// 局面の描画。pixelなレベルで、ものすごく将棋所のパクり。
    /// </summary>
    public class BlunderGraphics : IDisposable {
        /// <summary>
        /// 盤面画像
        /// </summary>
        public Bitmap BoardBitmap { get; set; }
        /// <summary>
        /// 線とかの画像
        /// </summary>
        public Bitmap LinesBitmap { get; set; }
        /// <summary>
        /// 駒画像
        /// </summary>
        public Bitmap[] PieceBitmaps { get; private set; }
        /// <summary>
        /// 最終手の移動先マス
        /// </summary>
        public Bitmap FocusToBitmap { get; set; }
        /// <summary>
        /// 最終手の移動元のマス
        /// </summary>
        public Bitmap FocusFromBitmap { get; set; }

        /// <summary>
        /// 最終手のマスに色を付けるならtrue。既定値はtrue。
        /// </summary>
        public bool ShowFocus { get; set; }
        /// <summary>
        /// 逆さま表示ならtrue。既定値はfalse。
        /// </summary>
        public bool ReverseView { get; set; }

        Size handSize;
        Size boardSize;
        Rectangle secondHandRect;
        Rectangle boardRect;
        Rectangle firstHandRect;
        Bitmap baseBitmap;
        Font font = new Font(FontFamily.GenericSerif, 14.0f);
        StringFormat stringFormat = new StringFormat() {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Center,
            FormatFlags = StringFormatFlags.NoWrap,
        };

        /// <summary>
        /// 初期化。
        /// </summary>
        public BlunderGraphics() {
            ShowFocus = true;
            // 盤面
            BoardBitmap = Resources.ban_kaya_a;
            LinesBitmap = Resources.masu_dot_xy;
            // 駒
            PieceBitmaps = new Bitmap[32] {
                null,
                Resources.Sfu,
                Resources.Skyo,
                Resources.Skei,
                Resources.Sgin,
                Resources.Skin,
                Resources.Skaku,
                Resources.Shi,
                Resources.Sou,
                Resources.Sto,
                Resources.Snkyo,
                Resources.Snkei,
                Resources.Sngin,
                null,
                Resources.Suma,
                Resources.Sryu,
                null,
                Resources.Gfu,
                Resources.Gkyo,
                Resources.Gkei,
                Resources.Ggin,
                Resources.Gkin,
                Resources.Gkaku,
                Resources.Ghi,
                Resources.Gou,
                Resources.Gto,
                Resources.Gnkyo,
                Resources.Gnkei,
                Resources.Gngin,
                null,
                Resources.Guma,
                Resources.Gryu,
            };
            FocusToBitmap = Resources.focus_bold_r;
            FocusFromBitmap = Resources.focus_thin_r;
            // ベース画像
            handSize = new Size(FocusToBitmap.Size.Width + 28, FocusToBitmap.Size.Height * 7 + 11);
            boardSize = new Size(BoardBitmap.Size.Width, BoardBitmap.Size.Height);
            secondHandRect = new Rectangle(3, 3, handSize.Width, handSize.Height);
            boardRect = new Rectangle(secondHandRect.Right + 10, 3, boardSize.Width, boardSize.Height);
            firstHandRect = new Rectangle(boardRect.Right + 10, boardRect.Bottom - handSize.Height, handSize.Width, handSize.Height);
            baseBitmap = new Bitmap(firstHandRect.Right + 3, firstHandRect.Bottom + 3);
            using (Graphics g = Graphics.FromImage(baseBitmap)) {
                g.FillRectangle(Brushes.Ivory, new Rectangle(new Point(), baseBitmap.Size));
                Rectangle sr = new Rectangle(0, 0, handSize.Width, handSize.Height);
                Rectangle br = new Rectangle(0, 0, boardSize.Width, boardSize.Height);
                g.DrawImage(BoardBitmap, secondHandRect, sr, GraphicsUnit.Pixel);
                g.DrawImage(BoardBitmap, firstHandRect, sr, GraphicsUnit.Pixel);
                g.DrawImage(BoardBitmap, boardRect, br, GraphicsUnit.Pixel);
                g.DrawImage(LinesBitmap, boardRect, br, GraphicsUnit.Pixel);
            }
        }

        /// <summary>
        /// 後始末
        /// </summary>
        public void Dispose() {
            baseBitmap.Dispose();
        }

        /// <summary>
        /// 描画サイズ。
        /// </summary>
        public Size Size {
            get { return baseBitmap.Size; }
        }
        /// <summary>
        /// 描画サイズ。
        /// </summary>
        public int Width {
            get { return baseBitmap.Width; }
        }
        /// <summary>
        /// 描画サイズ。
        /// </summary>
        public int Height {
            get { return baseBitmap.Height; }
        }

        /// <summary>
        /// GraphicsにBoardを描画。
        /// </summary>
        public void Draw(Graphics g, Board board) {
            // ベース画像
            g.DrawImage(baseBitmap, new Rectangle(new Point(), baseBitmap.Size));
            // 後手持ち駒
            for (Piece p = Piece.FU; p <= Piece.HI; p++) {
                DrawHand(g, board, 1, p);
            }
            // 最終手のフォーカス
            if (ShowFocus && 0 < board.History.Count) {
                Move move = board.GetLastMove();
                if (!move.IsEmpty && !move.IsSpecialState) {
                    Point pt = GetSquarePoint(Board.GetFile(move.To), Board.GetRank(move.To));
                    g.DrawImage(FocusToBitmap, new Rectangle(pt, FocusToBitmap.Size));
                    
                    pt = move.IsPut ?
                        GetHandPoint(board.Turn ^ 1, move.PutPiece) :
                        GetSquarePoint(Board.GetFile(move.From), Board.GetRank(move.From));
                    g.DrawImage(FocusFromBitmap, new Rectangle(pt, FocusFromBitmap.Size));
                }
            }
            // 盤上のコマ
            for (int file = 1; file <= 9; file++) {
                for (int rank = 1; rank <= 9; rank++) {
                    Piece p = board[file * 0x10 + rank + Board.Padding];
                    if (p == Piece.EMPTY) continue;
                    DrawPiece(g, file, rank, p);
                }
            }
            // 先手持ち駒
            for (Piece p = Piece.FU; p <= Piece.HI; p++) {
                DrawHand(g, board, 0, p);
            }
            // 手番表示
            Rectangle turnRect = new Rectangle(firstHandRect.Left, 59, firstHandRect.Width, 25);
            g.DrawString(board.Turn == 0 ? "先手番" : "後手番",
                font, Brushes.Black, turnRect, stringFormat);
        }

        /// <summary>
        /// 盤上の駒の描画
        /// </summary>
        public void DrawPiece(Graphics g, int file, int rank, Piece p) {
            DrawPiece(g, GetSquarePoint(file, rank), p);
        }

        /// <summary>
        /// 盤上の駒の描画
        /// </summary>
        public void DrawPiece(Graphics g, Point pt, Piece p) {
            g.DrawImage(PieceBitmaps[(byte)p], new Rectangle(pt, FocusToBitmap.Size));
        }

        /// <summary>
        /// 盤上の駒を消す
        /// </summary>
        public void ErasePiece(Graphics g, int file, int rank) {
            Point pt = GetSquarePoint(file, rank);
            Rectangle rc = new Rectangle(pt, FocusToBitmap.Size);
            g.DrawImage(baseBitmap, pt.X, pt.Y, rc, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// 持ち駒の描画
        /// </summary>
        public void DrawHand(Graphics g, Board board, int turn, Piece piece) {
            int count = board.GetHand(turn, piece);
            if (count <= 0) return;

            Point pt = GetHandPoint(turn, piece);
            Rectangle b = new Rectangle(pt.X + FocusToBitmap.Width, pt.Y, 28, FocusToBitmap.Height);
            Rectangle dst = Rectangle.Union(new Rectangle(pt, FocusToBitmap.Size), b);
            g.DrawImage(baseBitmap, dst, dst, GraphicsUnit.Pixel);
            if (0 < count) {
                Piece pp = piece | (Piece)(turn << PieceUtility.EnemyShift);
                g.DrawImage(PieceBitmaps[(byte)pp], new Rectangle(pt, FocusToBitmap.Size));
                g.DrawString(count.ToString(), font, Brushes.Black, b, stringFormat);
            }
        }

        /// <summary>
        /// マスの座標を返す
        /// </summary>
        public Point GetSquarePoint(int file, int rank) {
            if (ReverseView) {
                int rx = boardRect.Right - (12 + (FocusToBitmap.Width * (10 - file)));
                int ry = boardRect.Top + 11 + FocusToBitmap.Height * (9 - rank);
                return new Point(rx, ry);
            } else {
                int x = boardRect.Right - (12 + (FocusToBitmap.Width * file));
                int y = boardRect.Top + 11 + (FocusToBitmap.Height * (rank - 1));
                return new Point(x, y);
            }
        }

        /// <summary>
        /// 持ち駒の描画位置を返す
        /// </summary>
        private Point GetHandPoint(int turn, Piece piece) {
            int pos1 = Piece.HI - piece;
            int pos2 = piece - Piece.FU;
            if ((turn == 0) == ReverseView) { // 後手 or リバース先手
                int y = secondHandRect.Top + 11 + (FocusToBitmap.Height * pos2);
                return new Point(secondHandRect.Left, y);
            } else { // 先手 or リバース後手
                int y = firstHandRect.Top + (FocusToBitmap.Height * pos1);
                return new Point(firstHandRect.Left, y);
            }
        }
    }
}
