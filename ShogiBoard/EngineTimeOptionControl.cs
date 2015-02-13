using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShogiBoard {
    public partial class EngineTimeOptionControl : UserControl {
        public EngineTimeOptionControl() {
            InitializeComponent();
        }

        /// <summary>
        /// 時間制御方法
        /// </summary>
        public VolatileConfig.TimeControl TimeControl {
            get {
                if (radioButton1.Checked) return VolatileConfig.TimeControl.Normal;
                if (radioButton2.Checked) return VolatileConfig.TimeControl.Depth;
                return VolatileConfig.TimeControl.Nodes;
            }
            set {
                switch (value) {
                    case VolatileConfig.TimeControl.Normal: radioButton1.Checked = true; break;
                    case VolatileConfig.TimeControl.Depth: radioButton2.Checked = true; break;
                    case VolatileConfig.TimeControl.Nodes: radioButton3.Checked = true; break;
                    default: goto case VolatileConfig.TimeControl.Normal;
                }
            }
        }

        /// <summary>
        /// 読む深さ
        /// </summary>
        public int Depth {
            get { return (int)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        /// <summary>
        /// 読むノード数
        /// </summary>
        public long Nodes {
            get { return (long)numericUpDown2.Value; }
            set { numericUpDown2.Value = value; }
        }

        /// <summary>
        /// ラジオボタンの状態が変更されたぞイベント
        /// </summary>
        private void radioButtons_CheckedChanged(object sender, EventArgs e) {
            numericUpDown1.Enabled = radioButton2.Checked;
            numericUpDown2.Enabled = radioButton3.Checked;
        }
    }
}
