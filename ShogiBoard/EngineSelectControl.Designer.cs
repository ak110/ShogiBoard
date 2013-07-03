namespace ShogiBoard {
    partial class EngineSelectControl {
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
            this.buttonEngineConfig = new System.Windows.Forms.Button();
            this.comboBoxEngine = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonEngineConfig
            // 
            this.buttonEngineConfig.Location = new System.Drawing.Point(603, 0);
            this.buttonEngineConfig.Name = "buttonEngineConfig";
            this.buttonEngineConfig.Size = new System.Drawing.Size(75, 23);
            this.buttonEngineConfig.TabIndex = 5;
            this.buttonEngineConfig.Text = "設定(&C)";
            this.buttonEngineConfig.UseVisualStyleBackColor = true;
            this.buttonEngineConfig.Click += new System.EventHandler(this.buttonEngineConfig_Click);
            // 
            // comboBoxEngine
            // 
            this.comboBoxEngine.DropDownHeight = 480;
            this.comboBoxEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEngine.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBoxEngine.FormattingEnabled = true;
            this.comboBoxEngine.IntegralHeight = false;
            this.comboBoxEngine.Location = new System.Drawing.Point(56, 3);
            this.comboBoxEngine.Name = "comboBoxEngine";
            this.comboBoxEngine.Size = new System.Drawing.Size(541, 20);
            this.comboBoxEngine.TabIndex = 4;
            this.comboBoxEngine.SelectedIndexChanged += new System.EventHandler(this.comboBoxEngine_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "エンジン";
            // 
            // EngineSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonEngineConfig);
            this.Controls.Add(this.comboBoxEngine);
            this.Controls.Add(this.label4);
            this.Name = "EngineSelectControl";
            this.Size = new System.Drawing.Size(682, 25);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonEngineConfig;
        private System.Windows.Forms.ComboBox comboBoxEngine;
        private System.Windows.Forms.Label label4;
    }
}
