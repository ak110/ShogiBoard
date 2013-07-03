namespace ShogiBoard {
    partial class NetworkGameConnectionControl {
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
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.textBoxPass = new System.Windows.Forms.TextBox();
            this.checkBoxPV = new System.Windows.Forms.CheckBox();
            this.checkBoxKeepAlive = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Location = new System.Drawing.Point(0, 0);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(164, 19);
            this.textBoxAddress.TabIndex = 0;
            this.textBoxAddress.Text = "wdoor.c.u-tokyo.ac.jp:4081";
            // 
            // textBoxUser
            // 
            this.textBoxUser.Location = new System.Drawing.Point(170, 0);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(100, 19);
            this.textBoxUser.TabIndex = 1;
            this.textBoxUser.Text = "<username>";
            // 
            // textBoxPass
            // 
            this.textBoxPass.Location = new System.Drawing.Point(276, 0);
            this.textBoxPass.Name = "textBoxPass";
            this.textBoxPass.Size = new System.Drawing.Size(177, 19);
            this.textBoxPass.TabIndex = 2;
            this.textBoxPass.Text = "floodgate-900-0,<password>";
            this.toolTip1.SetToolTip(this.textBoxPass, "パスワード。floodgateの場合は先頭に「floodgate-900-0,」などを追加指定する。");
            // 
            // checkBoxPV
            // 
            this.checkBoxPV.AutoSize = true;
            this.checkBoxPV.Location = new System.Drawing.Point(458, 2);
            this.checkBoxPV.Name = "checkBoxPV";
            this.checkBoxPV.Size = new System.Drawing.Size(59, 16);
            this.checkBoxPV.TabIndex = 3;
            this.checkBoxPV.Text = "読み筋";
            this.toolTip1.SetToolTip(this.checkBoxPV, "サーバへ評価値・読み筋を送信する。\r\n※ floodgate・shogi-server用");
            this.checkBoxPV.UseVisualStyleBackColor = true;
            // 
            // checkBoxKeepAlive
            // 
            this.checkBoxKeepAlive.AutoSize = true;
            this.checkBoxKeepAlive.Location = new System.Drawing.Point(523, 2);
            this.checkBoxKeepAlive.Name = "checkBoxKeepAlive";
            this.checkBoxKeepAlive.Size = new System.Drawing.Size(75, 16);
            this.checkBoxKeepAlive.TabIndex = 4;
            this.checkBoxKeepAlive.Text = "KeepAlive";
            this.toolTip1.SetToolTip(this.checkBoxKeepAlive, "無通信時に接続が切れないよう、改行を送信する。\r\n※ floodgate・shogi-server用。WindowsではTCPのKeepAliveを使用するため通" +
        "常は不要。");
            this.checkBoxKeepAlive.UseVisualStyleBackColor = true;
            // 
            // NetworkGameConnectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxKeepAlive);
            this.Controls.Add(this.checkBoxPV);
            this.Controls.Add(this.textBoxPass);
            this.Controls.Add(this.textBoxUser);
            this.Controls.Add(this.textBoxAddress);
            this.Name = "NetworkGameConnectionControl";
            this.Size = new System.Drawing.Size(612, 19);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.TextBox textBoxPass;
        private System.Windows.Forms.CheckBox checkBoxPV;
        private System.Windows.Forms.CheckBox checkBoxKeepAlive;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
