using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShogiBoard {
    public partial class GameTimePickerControl : UserControl {
        public GameTimePickerControl() {
            InitializeComponent();
        }

        /// <summary>
        /// Enabled
        /// </summary>
        public new bool Enabled {
            get { return numericUpDown1.Enabled; }
            set {
                numericUpDown1.Enabled = value;
                numericUpDown2.Enabled = value;
            }
        }

        /// <summary>
        /// 持ち時間[秒]
        /// </summary>
        public int TimeASeconds {
            get { return (int)numericUpDown1.Value * 60; }
            set { numericUpDown1.Value = value / 60; }
        }
        /// <summary>
        /// 秒読み[秒]
        /// </summary>
        public int TimeBSeconds {
            get { return (int)numericUpDown2.Value; }
            set { numericUpDown2.Value = value; }
        }
    }
}
