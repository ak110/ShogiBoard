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
    /// 検討の設定ダイアログ
    /// </summary>
    public partial class ThinkForm : Form {
        VolatileConfig volatileConfig;

        public ThinkForm(VolatileConfig volatileConfig, EngineList engineList) {
            InitializeComponent();
            engineSelectControl1.Initialize(engineList);
            this.volatileConfig = volatileConfig;

            engineSelectControl1.SelectedItem = engineList.Select(
                volatileConfig.ThinkEngineName, volatileConfig.ThinkEnginePath);

            UpdateEnables();
        }

        private void buttonOk_Click(object sender, EventArgs e) {
            buttonSave_Click(this, EventArgs.Empty);
        }

        private void buttonSave_Click(object sender, EventArgs e) {
            Engine engine = engineSelectControl1.SelectedItem;
            if (engine == null) {
                volatileConfig.ThinkEngineName = null;
                volatileConfig.ThinkEnginePath = null;
            } else {
                // 念のため存在チェック
                string path = ShogiCore.USI.USIDriver.NormalizeEnginePath(engine.Path);
                if (!System.IO.File.Exists(path)) {
                    MessageBox.Show(this, path + " が存在しません。", "エラー");
                    DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
                volatileConfig.ThinkEngineName = engine.Name;
                volatileConfig.ThinkEnginePath = engine.Path;
                // 保存
                ConfigSerializer.Serialize(volatileConfig);
            }
        }

        private void engineSelectControl1_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateEnables();
        }

        private void UpdateEnables() {
            // エンジンの有無によってEnabledを変更
            bool hasEngine = engineSelectControl1.SelectedItem != null;;
            buttonOk.Enabled = hasEngine;
        }
    }
}
