using ShogiCore.USI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ShogiBoard {
    public partial class ConfigForm : Form {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Config config;

        public ConfigForm(Config config) {
            InitializeComponent();
            this.config = config;
            comboBox1.Items.Add(ProcessPriorityClass.AboveNormal);
            comboBox1.Items.Add(ProcessPriorityClass.Normal);
            comboBox1.Items.Add(ProcessPriorityClass.BelowNormal);
            comboBox1.Items.Add(ProcessPriorityClass.Idle);
            // Configからコントロールへ
            comboBox1.SelectedItem = config.EnginePriority;
        }

        private void buttonOk_Click(object sender, EventArgs e) {
            // コントロールからConfigへ
            config.EnginePriority = (ProcessPriorityClass)comboBox1.SelectedItem;
            // 保存
            ConfigSerializer.Serialize(config);
        }
    }
}
