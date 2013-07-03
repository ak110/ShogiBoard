namespace ShogiBoard {
    partial class MainForm {
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.終了XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.編集EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.棋譜コピーKIF形式DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.棋譜コピーCSA形式CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.棋譜コピーSFEN形式EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.局面コピーKIF形式QToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.局面コピーCSA形式PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.局面コピーSFEN形式RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.棋譜局面貼り付けVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.対局GToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.通信対局NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.通信対局切断DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.検討TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.詰将棋解答MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.中断AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.設定CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.エンジン一覧EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ログフォルダを開くLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxComment = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton切 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton投 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton待 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton急 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton中 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton検 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton詰 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.blunderViewControl = new ShogiBoard.Drawing.BlunderViewControl();
            this.gameGraphControl1 = new ShogiBoard.GameGraphControl();
            this.playerInfoControlN = new ShogiBoard.PlayerInfoControl();
            this.playerInfoControlP = new ShogiBoard.PlayerInfoControl();
            this.engineViewControl1 = new ShogiBoard.EngineViewControl();
            this.engineViewControl2 = new ShogiBoard.EngineViewControl();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem,
            this.編集EToolStripMenuItem,
            this.対局GToolStripMenuItem,
            this.設定CToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1000, 26);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルFToolStripMenuItem
            // 
            this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.終了XToolStripMenuItem});
            this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
            this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(85, 22);
            this.ファイルFToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // 終了XToolStripMenuItem
            // 
            this.終了XToolStripMenuItem.Name = "終了XToolStripMenuItem";
            this.終了XToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.終了XToolStripMenuItem.Text = "終了(&X)";
            this.終了XToolStripMenuItem.Click += new System.EventHandler(this.終了XToolStripMenuItem_Click);
            // 
            // 編集EToolStripMenuItem
            // 
            this.編集EToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.棋譜コピーKIF形式DToolStripMenuItem,
            this.棋譜コピーCSA形式CToolStripMenuItem,
            this.棋譜コピーSFEN形式EToolStripMenuItem,
            this.toolStripSeparator6,
            this.局面コピーKIF形式QToolStripMenuItem,
            this.局面コピーCSA形式PToolStripMenuItem,
            this.局面コピーSFEN形式RToolStripMenuItem,
            this.toolStripSeparator5,
            this.棋譜局面貼り付けVToolStripMenuItem});
            this.編集EToolStripMenuItem.Name = "編集EToolStripMenuItem";
            this.編集EToolStripMenuItem.Size = new System.Drawing.Size(61, 22);
            this.編集EToolStripMenuItem.Text = "編集(&E)";
            // 
            // 棋譜コピーKIF形式DToolStripMenuItem
            // 
            this.棋譜コピーKIF形式DToolStripMenuItem.Name = "棋譜コピーKIF形式DToolStripMenuItem";
            this.棋譜コピーKIF形式DToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.棋譜コピーKIF形式DToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.棋譜コピーKIF形式DToolStripMenuItem.Text = "棋譜コピー(KIF形式)(&D)";
            this.棋譜コピーKIF形式DToolStripMenuItem.Click += new System.EventHandler(this.棋譜コピーKIF形式DToolStripMenuItem_Click);
            // 
            // 棋譜コピーCSA形式CToolStripMenuItem
            // 
            this.棋譜コピーCSA形式CToolStripMenuItem.Name = "棋譜コピーCSA形式CToolStripMenuItem";
            this.棋譜コピーCSA形式CToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.棋譜コピーCSA形式CToolStripMenuItem.Text = "棋譜コピー(CSA形式)(&C)";
            this.棋譜コピーCSA形式CToolStripMenuItem.Click += new System.EventHandler(this.棋譜コピーCSA形式CToolStripMenuItem_Click);
            // 
            // 棋譜コピーSFEN形式EToolStripMenuItem
            // 
            this.棋譜コピーSFEN形式EToolStripMenuItem.Name = "棋譜コピーSFEN形式EToolStripMenuItem";
            this.棋譜コピーSFEN形式EToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.棋譜コピーSFEN形式EToolStripMenuItem.Text = "棋譜コピー(SFEN形式)(&E)";
            this.棋譜コピーSFEN形式EToolStripMenuItem.Click += new System.EventHandler(this.棋譜コピーSFEN形式EToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(290, 6);
            // 
            // 局面コピーKIF形式QToolStripMenuItem
            // 
            this.局面コピーKIF形式QToolStripMenuItem.Name = "局面コピーKIF形式QToolStripMenuItem";
            this.局面コピーKIF形式QToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.局面コピーKIF形式QToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.局面コピーKIF形式QToolStripMenuItem.Text = "局面コピー(KIF形式)(&Q)";
            this.局面コピーKIF形式QToolStripMenuItem.Click += new System.EventHandler(this.局面コピーKIF形式QToolStripMenuItem_Click);
            // 
            // 局面コピーCSA形式PToolStripMenuItem
            // 
            this.局面コピーCSA形式PToolStripMenuItem.Name = "局面コピーCSA形式PToolStripMenuItem";
            this.局面コピーCSA形式PToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.局面コピーCSA形式PToolStripMenuItem.Text = "局面コピー(CSA形式)(&P)";
            this.局面コピーCSA形式PToolStripMenuItem.Click += new System.EventHandler(this.局面コピーCSA形式PToolStripMenuItem_Click);
            // 
            // 局面コピーSFEN形式RToolStripMenuItem
            // 
            this.局面コピーSFEN形式RToolStripMenuItem.Name = "局面コピーSFEN形式RToolStripMenuItem";
            this.局面コピーSFEN形式RToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.局面コピーSFEN形式RToolStripMenuItem.Text = "局面コピー(SFEN形式)(&R)";
            this.局面コピーSFEN形式RToolStripMenuItem.Click += new System.EventHandler(this.局面コピーSFEN形式RToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(290, 6);
            // 
            // 棋譜局面貼り付けVToolStripMenuItem
            // 
            this.棋譜局面貼り付けVToolStripMenuItem.Name = "棋譜局面貼り付けVToolStripMenuItem";
            this.棋譜局面貼り付けVToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.棋譜局面貼り付けVToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.棋譜局面貼り付けVToolStripMenuItem.Text = "棋譜・局面貼り付け(&V)";
            this.棋譜局面貼り付けVToolStripMenuItem.Click += new System.EventHandler(this.棋譜局面貼り付けVToolStripMenuItem_Click);
            // 
            // 対局GToolStripMenuItem
            // 
            this.対局GToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.通信対局NToolStripMenuItem,
            this.通信対局切断DToolStripMenuItem,
            this.toolStripSeparator2,
            this.検討TToolStripMenuItem,
            this.toolStripSeparator3,
            this.詰将棋解答MToolStripMenuItem,
            this.toolStripSeparator4,
            this.中断AToolStripMenuItem});
            this.対局GToolStripMenuItem.Name = "対局GToolStripMenuItem";
            this.対局GToolStripMenuItem.Size = new System.Drawing.Size(63, 22);
            this.対局GToolStripMenuItem.Text = "対局(&G)";
            // 
            // 通信対局NToolStripMenuItem
            // 
            this.通信対局NToolStripMenuItem.Name = "通信対局NToolStripMenuItem";
            this.通信対局NToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.通信対局NToolStripMenuItem.Text = "通信対局(&N)";
            this.通信対局NToolStripMenuItem.Click += new System.EventHandler(this.通信対局NToolStripMenuItem_Click);
            // 
            // 通信対局切断DToolStripMenuItem
            // 
            this.通信対局切断DToolStripMenuItem.Enabled = false;
            this.通信対局切断DToolStripMenuItem.Name = "通信対局切断DToolStripMenuItem";
            this.通信対局切断DToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.通信対局切断DToolStripMenuItem.Text = "通信対局切断(&D)";
            this.通信対局切断DToolStripMenuItem.Click += new System.EventHandler(this.通信対局切断DToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(202, 6);
            // 
            // 検討TToolStripMenuItem
            // 
            this.検討TToolStripMenuItem.Name = "検討TToolStripMenuItem";
            this.検討TToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.検討TToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.検討TToolStripMenuItem.Text = "検討(&T)";
            this.検討TToolStripMenuItem.Click += new System.EventHandler(this.検討TToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(202, 6);
            // 
            // 詰将棋解答MToolStripMenuItem
            // 
            this.詰将棋解答MToolStripMenuItem.Name = "詰将棋解答MToolStripMenuItem";
            this.詰将棋解答MToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.詰将棋解答MToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.詰将棋解答MToolStripMenuItem.Text = "詰将棋解答(&M)";
            this.詰将棋解答MToolStripMenuItem.Click += new System.EventHandler(this.詰将棋解答MToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(202, 6);
            // 
            // 中断AToolStripMenuItem
            // 
            this.中断AToolStripMenuItem.Enabled = false;
            this.中断AToolStripMenuItem.Name = "中断AToolStripMenuItem";
            this.中断AToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.中断AToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.中断AToolStripMenuItem.Text = "中断(&A)";
            this.中断AToolStripMenuItem.Click += new System.EventHandler(this.中断AToolStripMenuItem_Click);
            // 
            // 設定CToolStripMenuItem
            // 
            this.設定CToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.エンジン一覧EToolStripMenuItem,
            this.toolStripSeparator1,
            this.ログフォルダを開くLToolStripMenuItem});
            this.設定CToolStripMenuItem.Name = "設定CToolStripMenuItem";
            this.設定CToolStripMenuItem.Size = new System.Drawing.Size(62, 22);
            this.設定CToolStripMenuItem.Text = "設定(&C)";
            // 
            // エンジン一覧EToolStripMenuItem
            // 
            this.エンジン一覧EToolStripMenuItem.Name = "エンジン一覧EToolStripMenuItem";
            this.エンジン一覧EToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.エンジン一覧EToolStripMenuItem.Text = "エンジン一覧(&E)";
            this.エンジン一覧EToolStripMenuItem.Click += new System.EventHandler(this.エンジン一覧EToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // ログフォルダを開くLToolStripMenuItem
            // 
            this.ログフォルダを開くLToolStripMenuItem.Name = "ログフォルダを開くLToolStripMenuItem";
            this.ログフォルダを開くLToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.ログフォルダを開くLToolStripMenuItem.Text = "ログフォルダを開く(&L)";
            this.ログフォルダを開くLToolStripMenuItem.Click += new System.EventHandler(this.ログフォルダを開くLToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 51);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(1000, 631);
            this.splitContainer1.SplitterDistance = 460;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.blunderViewControl);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gameGraphControl1);
            this.splitContainer2.Panel2.Controls.Add(this.panel1);
            this.splitContainer2.Panel2.Controls.Add(this.playerInfoControlN);
            this.splitContainer2.Panel2.Controls.Add(this.playerInfoControlP);
            this.splitContainer2.Size = new System.Drawing.Size(1000, 460);
            this.splitContainer2.SplitterDistance = 578;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxComment);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(418, 269);
            this.panel1.TabIndex = 3;
            // 
            // textBoxComment
            // 
            this.textBoxComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxComment.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxComment.Location = new System.Drawing.Point(181, 0);
            this.textBoxComment.Multiline = true;
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxComment.Size = new System.Drawing.Size(237, 269);
            this.textBoxComment.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(181, 269);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.engineViewControl1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.engineViewControl2);
            this.splitContainer3.Size = new System.Drawing.Size(1000, 167);
            this.splitContainer3.SplitterDistance = 83;
            this.splitContainer3.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton切,
            this.toolStripSeparator7,
            this.toolStripButton投,
            this.toolStripButton待,
            this.toolStripButton急,
            this.toolStripButton中,
            this.toolStripSeparator8,
            this.toolStripButton検,
            this.toolStripButton詰,
            this.toolStripSeparator9,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 26);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1000, 25);
            this.toolStrip1.TabIndex = 3;
            // 
            // toolStripButton切
            // 
            this.toolStripButton切.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton切.Enabled = false;
            this.toolStripButton切.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripButton切.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton切.Image")));
            this.toolStripButton切.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton切.Name = "toolStripButton切";
            this.toolStripButton切.Size = new System.Drawing.Size(28, 22);
            this.toolStripButton切.Text = "切";
            this.toolStripButton切.ToolTipText = "切断";
            this.toolStripButton切.Click += new System.EventHandler(this.toolStripButton切_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton投
            // 
            this.toolStripButton投.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton投.Enabled = false;
            this.toolStripButton投.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripButton投.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton投.Image")));
            this.toolStripButton投.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton投.Name = "toolStripButton投";
            this.toolStripButton投.Size = new System.Drawing.Size(28, 22);
            this.toolStripButton投.Text = "投";
            this.toolStripButton投.ToolTipText = "投了";
            this.toolStripButton投.Click += new System.EventHandler(this.toolStripButton投_Click);
            // 
            // toolStripButton待
            // 
            this.toolStripButton待.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton待.Enabled = false;
            this.toolStripButton待.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripButton待.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton待.Image")));
            this.toolStripButton待.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton待.Name = "toolStripButton待";
            this.toolStripButton待.Size = new System.Drawing.Size(28, 22);
            this.toolStripButton待.Text = "待";
            this.toolStripButton待.ToolTipText = "待った";
            this.toolStripButton待.Click += new System.EventHandler(this.toolStripButton待_Click);
            // 
            // toolStripButton急
            // 
            this.toolStripButton急.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton急.Enabled = false;
            this.toolStripButton急.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripButton急.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton急.Image")));
            this.toolStripButton急.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton急.Name = "toolStripButton急";
            this.toolStripButton急.Size = new System.Drawing.Size(28, 22);
            this.toolStripButton急.Text = "急";
            this.toolStripButton急.ToolTipText = "すぐ指させる";
            this.toolStripButton急.Click += new System.EventHandler(this.toolStripButton急_Click);
            // 
            // toolStripButton中
            // 
            this.toolStripButton中.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton中.Enabled = false;
            this.toolStripButton中.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripButton中.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton中.Image")));
            this.toolStripButton中.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton中.Name = "toolStripButton中";
            this.toolStripButton中.Size = new System.Drawing.Size(28, 22);
            this.toolStripButton中.Text = "中";
            this.toolStripButton中.ToolTipText = "中断";
            this.toolStripButton中.Click += new System.EventHandler(this.toolStripButton中_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton検
            // 
            this.toolStripButton検.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton検.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripButton検.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton検.Image")));
            this.toolStripButton検.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton検.Name = "toolStripButton検";
            this.toolStripButton検.Size = new System.Drawing.Size(28, 22);
            this.toolStripButton検.Text = "検";
            this.toolStripButton検.ToolTipText = "検討";
            this.toolStripButton検.Click += new System.EventHandler(this.toolStripButton検_Click);
            // 
            // toolStripButton詰
            // 
            this.toolStripButton詰.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton詰.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripButton詰.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton詰.Image")));
            this.toolStripButton詰.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton詰.Name = "toolStripButton詰";
            this.toolStripButton詰.Size = new System.Drawing.Size(28, 22);
            this.toolStripButton詰.Text = "詰";
            this.toolStripButton詰.ToolTipText = "詰将棋解答";
            this.toolStripButton詰.Click += new System.EventHandler(this.toolStripButton詰_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(0, 22);
            // 
            // blunderViewControl
            // 
            this.blunderViewControl.BackColor = System.Drawing.Color.Transparent;
            this.blunderViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blunderViewControl.Location = new System.Drawing.Point(0, 0);
            this.blunderViewControl.Name = "blunderViewControl";
            this.blunderViewControl.Size = new System.Drawing.Size(578, 460);
            this.blunderViewControl.TabIndex = 0;
            // 
            // gameGraphControl1
            // 
            this.gameGraphControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(191)))), ((int)(((byte)(235)))));
            this.gameGraphControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gameGraphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameGraphControl1.Location = new System.Drawing.Point(0, 290);
            this.gameGraphControl1.Name = "gameGraphControl1";
            this.gameGraphControl1.Size = new System.Drawing.Size(418, 149);
            this.gameGraphControl1.TabIndex = 4;
            // 
            // playerInfoControlN
            // 
            this.playerInfoControlN.BackColor = System.Drawing.Color.White;
            this.playerInfoControlN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.playerInfoControlN.ByoyomiSeconds = 0;
            this.playerInfoControlN.Dock = System.Windows.Forms.DockStyle.Top;
            this.playerInfoControlN.Location = new System.Drawing.Point(0, 0);
            this.playerInfoControlN.Name = "playerInfoControlN";
            this.playerInfoControlN.PlayerName = "";
            this.playerInfoControlN.RemainSeconds = 0;
            this.playerInfoControlN.Size = new System.Drawing.Size(418, 21);
            this.playerInfoControlN.TabIndex = 2;
            this.playerInfoControlN.TimeASeconds = 0;
            this.playerInfoControlN.TimeBSeconds = 0;
            this.playerInfoControlN.Turn = 1;
            // 
            // playerInfoControlP
            // 
            this.playerInfoControlP.BackColor = System.Drawing.Color.White;
            this.playerInfoControlP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.playerInfoControlP.ByoyomiSeconds = 0;
            this.playerInfoControlP.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.playerInfoControlP.Location = new System.Drawing.Point(0, 439);
            this.playerInfoControlP.Name = "playerInfoControlP";
            this.playerInfoControlP.PlayerName = "";
            this.playerInfoControlP.RemainSeconds = 0;
            this.playerInfoControlP.Size = new System.Drawing.Size(418, 21);
            this.playerInfoControlP.TabIndex = 1;
            this.playerInfoControlP.TimeASeconds = 0;
            this.playerInfoControlP.TimeBSeconds = 0;
            this.playerInfoControlP.Turn = 0;
            // 
            // engineViewControl1
            // 
            this.engineViewControl1.BackColor = System.Drawing.Color.White;
            this.engineViewControl1.Board = null;
            this.engineViewControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.engineViewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.engineViewControl1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.engineViewControl1.Location = new System.Drawing.Point(0, 0);
            this.engineViewControl1.Name = "engineViewControl1";
            this.engineViewControl1.Size = new System.Drawing.Size(1000, 83);
            this.engineViewControl1.TabIndex = 0;
            // 
            // engineViewControl2
            // 
            this.engineViewControl2.BackColor = System.Drawing.Color.White;
            this.engineViewControl2.Board = null;
            this.engineViewControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.engineViewControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.engineViewControl2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.engineViewControl2.Location = new System.Drawing.Point(0, 0);
            this.engineViewControl2.Name = "engineViewControl2";
            this.engineViewControl2.Size = new System.Drawing.Size(1000, 80);
            this.engineViewControl2.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 682);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShogiBoard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.MainForm_DragOver);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 終了XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 対局GToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 通信対局NToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private EngineViewControl engineViewControl1;
        private EngineViewControl engineViewControl2;
        private PlayerInfoControl playerInfoControlN;
        private PlayerInfoControl playerInfoControlP;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ToolStripMenuItem 設定CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem エンジン一覧EToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ログフォルダを開くLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 通信対局切断DToolStripMenuItem;
        private Drawing.BlunderViewControl blunderViewControl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 検討TToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 詰将棋解答MToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 編集EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 棋譜局面貼り付けVToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem 中断AToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 棋譜コピーCSA形式CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 棋譜コピーKIF形式DToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 棋譜コピーSFEN形式EToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem 局面コピーCSA形式PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 局面コピーKIF形式QToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 局面コピーSFEN形式RToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private GameGraphControl gameGraphControl1;
        private System.Windows.Forms.TextBox textBoxComment;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton切;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton toolStripButton投;
        private System.Windows.Forms.ToolStripButton toolStripButton待;
        private System.Windows.Forms.ToolStripButton toolStripButton急;
        private System.Windows.Forms.ToolStripButton toolStripButton中;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton toolStripButton検;
        private System.Windows.Forms.ToolStripButton toolStripButton詰;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}

