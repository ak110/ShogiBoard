namespace ShogiBoard {
    partial class GameForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.gameTimePickerControl3 = new ShogiBoard.GameTimePickerControl();
            this.gameTimePickerControl2 = new ShogiBoard.GameTimePickerControl();
            this.gameTimePickerControl1 = new ShogiBoard.GameTimePickerControl();
            this.engineSelectControl2 = new ShogiBoard.EngineSelectControl();
            this.engineSelectControl1 = new ShogiBoard.EngineSelectControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.engineSelectControl1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(702, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "先手";
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(211, 308);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(96, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "設定して開始";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonSave.Location = new System.Drawing.Point(313, 308);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(96, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "設定のみ";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(415, 308);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(96, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "キャンセル";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.engineSelectControl2);
            this.groupBox2.Location = new System.Drawing.Point(12, 83);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(702, 65);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "後手";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.gameTimePickerControl3);
            this.groupBox3.Controls.Add(this.gameTimePickerControl2);
            this.groupBox3.Controls.Add(this.gameTimePickerControl1);
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.numericUpDown1);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(12, 154);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(702, 134);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "対局設定";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(450, 29);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(105, 19);
            this.numericUpDown1.TabIndex = 5;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(561, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "(0で無制限に連続対局)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(388, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "対局回数";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton3);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Location = new System.Drawing.Point(7, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(87, 74);
            this.panel1.TabIndex = 0;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(3, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(77, 16);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "時間設定1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(3, 25);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(77, 16);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "時間設定2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(3, 47);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(77, 16);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "時間設定3";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // gameTimePickerControl3
            // 
            this.gameTimePickerControl3.Location = new System.Drawing.Point(100, 66);
            this.gameTimePickerControl3.Name = "gameTimePickerControl3";
            this.gameTimePickerControl3.Size = new System.Drawing.Size(273, 21);
            this.gameTimePickerControl3.TabIndex = 3;
            this.gameTimePickerControl3.TimeASeconds = 599940;
            this.gameTimePickerControl3.TimeBSeconds = 9999;
            // 
            // gameTimePickerControl2
            // 
            this.gameTimePickerControl2.Location = new System.Drawing.Point(100, 44);
            this.gameTimePickerControl2.Name = "gameTimePickerControl2";
            this.gameTimePickerControl2.Size = new System.Drawing.Size(273, 21);
            this.gameTimePickerControl2.TabIndex = 2;
            this.gameTimePickerControl2.TimeASeconds = 599940;
            this.gameTimePickerControl2.TimeBSeconds = 9999;
            // 
            // gameTimePickerControl1
            // 
            this.gameTimePickerControl1.Location = new System.Drawing.Point(100, 19);
            this.gameTimePickerControl1.Name = "gameTimePickerControl1";
            this.gameTimePickerControl1.Size = new System.Drawing.Size(273, 21);
            this.gameTimePickerControl1.TabIndex = 1;
            this.gameTimePickerControl1.TimeASeconds = 599940;
            this.gameTimePickerControl1.TimeBSeconds = 9999;
            // 
            // engineSelectControl2
            // 
            this.engineSelectControl2.Location = new System.Drawing.Point(6, 18);
            this.engineSelectControl2.Name = "engineSelectControl2";
            this.engineSelectControl2.SelectedItem = null;
            this.engineSelectControl2.Size = new System.Drawing.Size(682, 25);
            this.engineSelectControl2.TabIndex = 0;
            this.engineSelectControl2.SelectedIndexChanged += new System.EventHandler(this.engineSelectControl1_SelectedIndexChanged);
            // 
            // engineSelectControl1
            // 
            this.engineSelectControl1.Location = new System.Drawing.Point(6, 18);
            this.engineSelectControl1.Name = "engineSelectControl1";
            this.engineSelectControl1.SelectedItem = null;
            this.engineSelectControl1.Size = new System.Drawing.Size(682, 25);
            this.engineSelectControl1.TabIndex = 0;
            this.engineSelectControl1.SelectedIndexChanged += new System.EventHandler(this.engineSelectControl1_SelectedIndexChanged);
            // 
            // GameForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(726, 342);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ShogiBoard - 対局";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private EngineSelectControl engineSelectControl1;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private EngineSelectControl engineSelectControl2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private GameTimePickerControl gameTimePickerControl3;
        private GameTimePickerControl gameTimePickerControl2;
        private GameTimePickerControl gameTimePickerControl1;
    }
}