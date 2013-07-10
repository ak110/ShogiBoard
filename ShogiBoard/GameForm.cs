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

        public GameForm(EngineList engineList, VolatileConfig volatileConfig) {
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
            gameTimePickerControl1.TimeASeconds = volatileConfig.GameTimes[0].TimeASeconds;
            gameTimePickerControl1.TimeBSeconds = volatileConfig.GameTimes[0].TimeBSeconds;
            gameTimePickerControl2.TimeASeconds = volatileConfig.GameTimes[1].TimeASeconds;
            gameTimePickerControl2.TimeBSeconds = volatileConfig.GameTimes[1].TimeBSeconds;
            gameTimePickerControl3.TimeASeconds = volatileConfig.GameTimes[2].TimeASeconds;
            gameTimePickerControl3.TimeBSeconds = volatileConfig.GameTimes[2].TimeBSeconds;
            checkBox2.Checked = volatileConfig.GameJudgeTimeUp;
            numericUpDown1.Value = volatileConfig.GameCount;
            switch (volatileConfig.GameStartPosType) {
                case 0: radioButton5.Checked = true; break;
                case 1: radioButton6.Checked = true; break;
                default: goto case 0;
            }
            textBox1.Text = volatileConfig.GameStartPosNotationPath;
            checkBox1.Checked = volatileConfig.GameStartPosNotationShuffle;
            numericUpDown2.Value = volatileConfig.GameStartPosNotationStartCount;

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
            volatileConfig.GameTimes[0].TimeASeconds = gameTimePickerControl1.TimeASeconds;
            volatileConfig.GameTimes[0].TimeBSeconds = gameTimePickerControl1.TimeBSeconds;
            volatileConfig.GameTimes[1].TimeASeconds = gameTimePickerControl2.TimeASeconds;
            volatileConfig.GameTimes[1].TimeBSeconds = gameTimePickerControl2.TimeBSeconds;
            volatileConfig.GameTimes[2].TimeASeconds = gameTimePickerControl3.TimeASeconds;
            volatileConfig.GameTimes[2].TimeBSeconds = gameTimePickerControl3.TimeBSeconds;
            volatileConfig.GameJudgeTimeUp = checkBox2.Checked;
            volatileConfig.GameCount = (int)numericUpDown1.Value;
            if (radioButton5.Checked) volatileConfig.GameStartPosType = 0;
            else volatileConfig.GameStartPosType = 1;
            volatileConfig.GameStartPosNotationPath = textBox1.Text;
            volatileConfig.GameStartPosNotationShuffle = checkBox1.Checked;
            volatileConfig.GameStartPosNotationStartCount = (int)numericUpDown2.Value;

            // 保存
            ConfigSerializer.Serialize(volatileConfig);
        }

        private void engineSelectControl1_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateEnables();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e) {
            UpdateEnables();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e) {
            UpdateEnables();
        }

        private void UpdateEnables() {
            // エンジンの有無によってEnabledを変更
            bool hasEngine1 = engineSelectControl1.SelectedItem != null;
            bool hasEngine2 = engineSelectControl2.SelectedItem != null;
            buttonOk.Enabled = hasEngine1 && hasEngine2;
            // 棋譜の局面かどうかによってEnabledを変更
            bool isNotationPosition = radioButton6.Checked;
            textBox1.Enabled = isNotationPosition;
            numericUpDown2.Enabled = isNotationPosition;
            button1.Enabled = isNotationPosition;
            button2.Enabled = isNotationPosition;
            checkBox1.Enabled = isNotationPosition;
        }

        private void button1_Click(object sender, EventArgs e) {
            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK) {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            folderBrowserDialog1.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK) {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
