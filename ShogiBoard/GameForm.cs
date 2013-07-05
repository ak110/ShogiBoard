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
    /// 対局の設定ダイアログ
    /// </summary>
    public partial class GameForm : Form {
        VolatileConfig volatileConfig;

        public GameForm(VolatileConfig volatileConfig, EngineList engineList) {
            InitializeComponent();
            engineSelectControl1.Initialize(engineList);
            engineSelectControl2.Initialize(engineList);
            this.volatileConfig = volatileConfig;

            engineSelectControl1.SelectedItem = engineList.Select(
                volatileConfig.GameEngine1Name, volatileConfig.GameEngine1Path);
            engineSelectControl2.SelectedItem = engineList.Select(
                volatileConfig.GameEngine2Name, volatileConfig.GameEngine2Path);
            switch (volatileConfig.GameTimeIndex) {
                case 0: radioButton1.Checked = true; break;
                case 1: radioButton2.Checked = true; break;
                case 2: radioButton3.Checked = true; break;
                default: goto case 0;
            }
            gameTimePickerControl1.TimeA = volatileConfig.GameTimes[0].TimeA;
            gameTimePickerControl1.TimeB = volatileConfig.GameTimes[0].TimeB;
            gameTimePickerControl2.TimeA = volatileConfig.GameTimes[1].TimeA;
            gameTimePickerControl2.TimeB = volatileConfig.GameTimes[1].TimeB;
            gameTimePickerControl3.TimeA = volatileConfig.GameTimes[2].TimeA;
            gameTimePickerControl3.TimeB = volatileConfig.GameTimes[2].TimeB;
            numericUpDown1.Value = volatileConfig.GameCount;

            UpdateEnables();
        }

        private void buttonOk_Click(object sender, EventArgs e) {
            buttonSave_Click(this, EventArgs.Empty);
        }

        private void buttonSave_Click(object sender, EventArgs e) {
            {
                Engine engine = engineSelectControl1.SelectedItem;
                if (engine == null) {
                    volatileConfig.GameEngine1Name = null;
                    volatileConfig.GameEngine1Path = null;
                } else {
                    // 念のため存在チェック
                    string path = ShogiCore.USI.USIDriver.NormalizeEnginePath(engine.Path);
                    if (!System.IO.File.Exists(path)) {
                        MessageBox.Show(this, path + " が存在しません。", "エラー");
                        DialogResult = System.Windows.Forms.DialogResult.None;
                        return;
                    }
                    volatileConfig.GameEngine1Name = engine.Name;
                    volatileConfig.GameEngine1Path = engine.Path;
                }
            }
            {
                Engine engine = engineSelectControl2.SelectedItem;
                if (engine == null) {
                    volatileConfig.GameEngine2Name = null;
                    volatileConfig.GameEngine2Path = null;
                } else {
                    // 念のため存在チェック
                    string path = ShogiCore.USI.USIDriver.NormalizeEnginePath(engine.Path);
                    if (!System.IO.File.Exists(path)) {
                        MessageBox.Show(this, path + " が存在しません。", "エラー");
                        DialogResult = System.Windows.Forms.DialogResult.None;
                        return;
                    }
                    volatileConfig.GameEngine2Name = engine.Name;
                    volatileConfig.GameEngine2Path = engine.Path;
                }
            }
            if (radioButton1.Checked) volatileConfig.GameTimeIndex = 0;
            else if (radioButton2.Checked) volatileConfig.GameTimeIndex = 1;
            else if (radioButton3.Checked) volatileConfig.GameTimeIndex = 2;
            else volatileConfig.GameTimeIndex = 0;
            volatileConfig.GameTimes[0].TimeA = gameTimePickerControl1.TimeA;
            volatileConfig.GameTimes[0].TimeB = gameTimePickerControl1.TimeB;
            volatileConfig.GameTimes[1].TimeA = gameTimePickerControl2.TimeA;
            volatileConfig.GameTimes[1].TimeB = gameTimePickerControl2.TimeB;
            volatileConfig.GameTimes[2].TimeA = gameTimePickerControl3.TimeA;
            volatileConfig.GameTimes[2].TimeB = gameTimePickerControl3.TimeB;
            volatileConfig.GameCount = (int)numericUpDown1.Value;

            // 保存
            ConfigSerializer.Serialize(volatileConfig);
        }

        private void engineSelectControl1_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateEnables();
        }

        private void UpdateEnables() {
            // エンジンの有無によってEnabledを変更
            bool hasEngine1 = engineSelectControl1.SelectedItem != null; ;
            bool hasEngine2 = engineSelectControl2.SelectedItem != null; ;
            buttonOk.Enabled = hasEngine1 && hasEngine2;
        }
    }
}
