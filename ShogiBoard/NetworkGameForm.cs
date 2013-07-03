using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShogiBoard {
    /// <summary>
    /// 通信対局設定
    /// </summary>
    public partial class NetworkGameForm : Form {
        EngineList engineList;
        Config config;
        VolatileConfig volatileConfig;

        public NetworkGameForm(EngineList engineList, Config config, VolatileConfig volatileConfig) {
            InitializeComponent();
            this.engineList = engineList;
            this.config = config;
            this.volatileConfig = volatileConfig;
            System.Diagnostics.Debug.Assert(
                config.NetworkGameConnections != null &&
                config.NetworkGameConnections.Length == 6);

            engineSelectControl1.Initialize(engineList);

            // コンフィグからフォームへ
            // エンジン
            engineSelectControl1.SelectedItem = engineList.Select(
                volatileConfig.NetworkGameEngineName,
                volatileConfig.NetworkGameEnginePath);
            // 接続先
            networkGameConnectionControl1.ReadFromConfig(config.NetworkGameConnections[0]);
            networkGameConnectionControl2.ReadFromConfig(config.NetworkGameConnections[1]);
            networkGameConnectionControl3.ReadFromConfig(config.NetworkGameConnections[2]);
            networkGameConnectionControl4.ReadFromConfig(config.NetworkGameConnections[3]);
            networkGameConnectionControl5.ReadFromConfig(config.NetworkGameConnections[4]);
            networkGameConnectionControl6.ReadFromConfig(config.NetworkGameConnections[5]);
            switch (volatileConfig.NetworkGameConnectionIndex) {
                case 0: radioButton1.Checked = true; break;
                case 1: radioButton2.Checked = true; break;
                case 2: radioButton3.Checked = true; break;
                case 3: radioButton4.Checked = true; break;
                case 4: radioButton5.Checked = true; break;
                case 5: radioButton6.Checked = true; break;
                default: goto case 0;
            }
            numericUpDown1.Value = volatileConfig.NetworkGameCount;

            // 表示を更新
            UpdateEnables();
        }

        private void buttonOk_Click(object sender, EventArgs e) {
            buttonSave_Click(this, EventArgs.Empty);
        }

        private void buttonSave_Click(object sender, EventArgs e) {
            // フォームからコンフィグへ
            // エンジン
            Engine engine = engineSelectControl1.SelectedItem;
            if (engine == null) {
                volatileConfig.NetworkGameEngineName = null;
                volatileConfig.NetworkGameEnginePath = null;
            } else {
                // 念のため存在チェック
                string path = ShogiCore.USI.USIDriver.NormalizeEnginePath(engine.Path);
                if (!System.IO.File.Exists(path)) {
                    MessageBox.Show(this, path + " が存在しません。", "エラー");
                    DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
                volatileConfig.NetworkGameEngineName = engine.Name;
                volatileConfig.NetworkGameEnginePath = engine.Path;
            }
            // 接続先
            if (radioButton1.Checked) volatileConfig.NetworkGameConnectionIndex = 0;
            else if (radioButton2.Checked) volatileConfig.NetworkGameConnectionIndex = 1;
            else if (radioButton3.Checked) volatileConfig.NetworkGameConnectionIndex = 2;
            else if (radioButton4.Checked) volatileConfig.NetworkGameConnectionIndex = 3;
            else if (radioButton5.Checked) volatileConfig.NetworkGameConnectionIndex = 4;
            else if (radioButton6.Checked) volatileConfig.NetworkGameConnectionIndex = 5;
            else volatileConfig.NetworkGameConnectionIndex = 0;
            networkGameConnectionControl1.WriteToConfig(config.NetworkGameConnections[0]);
            networkGameConnectionControl2.WriteToConfig(config.NetworkGameConnections[1]);
            networkGameConnectionControl3.WriteToConfig(config.NetworkGameConnections[2]);
            networkGameConnectionControl4.WriteToConfig(config.NetworkGameConnections[3]);
            networkGameConnectionControl5.WriteToConfig(config.NetworkGameConnections[4]);
            networkGameConnectionControl6.WriteToConfig(config.NetworkGameConnections[5]);
            volatileConfig.NetworkGameCount = (int)numericUpDown1.Value;
            // 保存
            ConfigSerializer.Serialize(config);
            ConfigSerializer.Serialize(volatileConfig);
        }

        /// <summary>
        /// Enables
        /// </summary>
        private void UpdateEnables() {
            networkGameConnectionControl1.SetEnabledAll(radioButton1.Checked);
            networkGameConnectionControl2.SetEnabledAll(radioButton2.Checked);
            networkGameConnectionControl3.SetEnabledAll(radioButton3.Checked);
            networkGameConnectionControl4.SetEnabledAll(radioButton4.Checked);
            networkGameConnectionControl5.SetEnabledAll(radioButton5.Checked);
            networkGameConnectionControl6.SetEnabledAll(radioButton6.Checked);
            bool hasEngine = engineSelectControl1.SelectedItem as Engine != null;
            buttonOk.Enabled = hasEngine;
            // TODO: OKボタンの非活性化条件、<username>も弾くとか。
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
            UpdateEnables();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) {
            UpdateEnables();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e) {
            UpdateEnables();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e) {
            UpdateEnables();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e) {
            UpdateEnables();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e) {
            UpdateEnables();
        }

        private void engineSelectControl1_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateEnables();
        }
    }
}
