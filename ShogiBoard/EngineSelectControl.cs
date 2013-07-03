using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShogiBoard {
    public partial class EngineSelectControl : UserControl {
        EngineList engineList;

        /// <summary>
        /// SelectedIndexChanged
        /// </summary>
        public event EventHandler SelectedIndexChanged;

        public EngineSelectControl() {
            InitializeComponent();
        }

        /// <summary>
        /// 初期化。必ず呼び出すこと。(なんかイマイチなのだが…)
        /// </summary>
        public void Initialize(EngineList engineList) {
            this.engineList = engineList;

            comboBoxEngine.Items.AddRange(engineList.Engines.ToArray());
        }

        /// <summary>
        /// 選択されている項目
        /// </summary>
        public Engine SelectedItem {
            get { return comboBoxEngine.SelectedItem as Engine; }
            set {
                comboBoxEngine.SelectedItem = value;
                if (comboBoxEngine.SelectedItem == null && 0 < comboBoxEngine.Items.Count) {
                    comboBoxEngine.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// 設定ボタン
        /// </summary>
        private void buttonEngineConfig_Click(object sender, EventArgs e) {
            Engine engine = comboBoxEngine.SelectedItem as Engine;
            if (engine != null) {
                using (EngineForm form = new EngineForm(engine, engineList)) {
                    if (form.ShowDialog(this) == DialogResult.OK) {
                        // 保存
                        ConfigSerializer.Serialize(engineList);
                        // 表示を更新
                        comboBoxEngine.BeginUpdate();
                        comboBoxEngine.Items.Clear();
                        comboBoxEngine.Items.AddRange(engineList.Engines.ToArray());
                        comboBoxEngine.SelectedItem = engine;
                        comboBoxEngine.EndUpdate();
                    }
                }
            }
        }

        private void comboBoxEngine_SelectedIndexChanged(object sender, EventArgs e) {
            bool hasEngine = SelectedItem != null;
            buttonEngineConfig.Enabled = hasEngine;
            // SelectedIndexChanged
            EventHandler SelectedIndexChanged = this.SelectedIndexChanged;
            if (SelectedIndexChanged != null) {
                SelectedIndexChanged(sender, e);
            }
        }
    }
}
