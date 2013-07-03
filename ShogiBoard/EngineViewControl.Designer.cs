namespace ShogiBoard {
    partial class EngineViewControl {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "9,999,999"),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "999/999", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)))),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "9,999,999,999"),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "-Mate:998"),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "▲76歩(77)△34歩(33)▲66歩(67)△33角(22)▲78銀(79)△22飛(82)▲77角(88)△24歩(23)▲67銀(78)△25歩(24)▲" +
                    "86歩(87)△82銀(71)▲48銀(39)△42銀(31)▲36歩(37)△44歩(43)▲37銀(48)△43銀(42)▲88飛(28)")}, -1);
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDepth = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderNodes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPV = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.選択した読み筋をコピーCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全ての読み筋をコピーAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelNPS = new System.Windows.Forms.Label();
            this.labelCurMove = new System.Windows.Forms.Label();
            this.labelHashFull = new System.Windows.Forms.Label();
            this.labelEngine = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTime,
            this.columnHeaderDepth,
            this.columnHeaderNodes,
            this.columnHeaderValue,
            this.columnHeaderPV});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listView1.Location = new System.Drawing.Point(0, 12);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(922, 114);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.Layout += new System.Windows.Forms.LayoutEventHandler(this.listView1_Layout);
            // 
            // columnHeaderTime
            // 
            this.columnHeaderTime.Text = "時間";
            this.columnHeaderTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderTime.Width = 64;
            // 
            // columnHeaderDepth
            // 
            this.columnHeaderDepth.Text = "深さ";
            this.columnHeaderDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeaderDepth.Width = 54;
            // 
            // columnHeaderNodes
            // 
            this.columnHeaderNodes.Text = "ノード数";
            this.columnHeaderNodes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderNodes.Width = 90;
            // 
            // columnHeaderValue
            // 
            this.columnHeaderValue.Text = "評価値";
            this.columnHeaderValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderValue.Width = 66;
            // 
            // columnHeaderPV
            // 
            this.columnHeaderPV.Text = "読み筋";
            this.columnHeaderPV.Width = 621;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.選択した読み筋をコピーCToolStripMenuItem,
            this.全ての読み筋をコピーAToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(227, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // 選択した読み筋をコピーCToolStripMenuItem
            // 
            this.選択した読み筋をコピーCToolStripMenuItem.Name = "選択した読み筋をコピーCToolStripMenuItem";
            this.選択した読み筋をコピーCToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.選択した読み筋をコピーCToolStripMenuItem.Text = "選択した読み筋をコピー(&C)";
            this.選択した読み筋をコピーCToolStripMenuItem.Click += new System.EventHandler(this.選択した読み筋をコピーCToolStripMenuItem_Click);
            // 
            // 全ての読み筋をコピーAToolStripMenuItem
            // 
            this.全ての読み筋をコピーAToolStripMenuItem.Name = "全ての読み筋をコピーAToolStripMenuItem";
            this.全ての読み筋をコピーAToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.全ての読み筋をコピーAToolStripMenuItem.Text = "全ての読み筋をコピー(&A)";
            this.全ての読み筋をコピーAToolStripMenuItem.Click += new System.EventHandler(this.全ての読み筋をコピーAToolStripMenuItem_Click);
            // 
            // labelNPS
            // 
            this.labelNPS.AutoSize = true;
            this.labelNPS.Font = new System.Drawing.Font("ＭＳ ゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelNPS.Location = new System.Drawing.Point(137, 0);
            this.labelNPS.Name = "labelNPS";
            this.labelNPS.Size = new System.Drawing.Size(89, 11);
            this.labelNPS.TabIndex = 1;
            this.labelNPS.Text = "NPS：9,999,999";
            // 
            // labelCurMove
            // 
            this.labelCurMove.AutoSize = true;
            this.labelCurMove.Font = new System.Drawing.Font("ＭＳ ゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelCurMove.Location = new System.Drawing.Point(232, 0);
            this.labelCurMove.Name = "labelCurMove";
            this.labelCurMove.Size = new System.Drawing.Size(113, 11);
            this.labelCurMove.TabIndex = 2;
            this.labelCurMove.Text = "探索手：▲76歩(77)";
            // 
            // labelHashFull
            // 
            this.labelHashFull.AutoSize = true;
            this.labelHashFull.Font = new System.Drawing.Font("ＭＳ ゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelHashFull.Location = new System.Drawing.Point(486, 0);
            this.labelHashFull.Name = "labelHashFull";
            this.labelHashFull.Size = new System.Drawing.Size(137, 11);
            this.labelHashFull.TabIndex = 3;
            this.labelHashFull.Text = "ハッシュ使用率：100.0%";
            // 
            // labelEngine
            // 
            this.labelEngine.AutoSize = true;
            this.labelEngine.Font = new System.Drawing.Font("ＭＳ ゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelEngine.Location = new System.Drawing.Point(0, 0);
            this.labelEngine.Name = "labelEngine";
            this.labelEngine.Size = new System.Drawing.Size(119, 11);
            this.labelEngine.TabIndex = 0;
            this.labelEngine.Text = "BunderXX-i73960X_4c";
            // 
            // EngineViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.labelCurMove);
            this.Controls.Add(this.labelNPS);
            this.Controls.Add(this.labelHashFull);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.labelEngine);
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "EngineViewControl";
            this.Size = new System.Drawing.Size(922, 126);
            this.Load += new System.EventHandler(this.EngineViewControl_Load);
            this.SizeChanged += new System.EventHandler(this.EngineViewControl_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeaderPV;
        private System.Windows.Forms.ColumnHeader columnHeaderDepth;
        private System.Windows.Forms.ColumnHeader columnHeaderNodes;
        private System.Windows.Forms.ColumnHeader columnHeaderValue;
        private System.Windows.Forms.ColumnHeader columnHeaderTime;
        private System.Windows.Forms.Label labelNPS;
        private System.Windows.Forms.Label labelCurMove;
        private System.Windows.Forms.Label labelHashFull;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 選択した読み筋をコピーCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全ての読み筋をコピーAToolStripMenuItem;
        private System.Windows.Forms.Label labelEngine;

    }
}
