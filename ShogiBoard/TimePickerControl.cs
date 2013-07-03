using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShogiBoard {
    public partial class TimePickerControl : UserControl {
        public TimePickerControl() {
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
        /// 値。秒単位の整数でやりとりする。
        /// </summary>
        public int ValueInSeconds {
            get {
                return (int)numericUpDown1.Value * 60 + (int)numericUpDown2.Value;
            }
            set {
                numericUpDown1.Value = value / 60;
                numericUpDown2.Value = value % 60;
            }
        }
    }
}
