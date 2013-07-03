using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShogiBoard {
    public partial class NetworkGameConnectionControl : UserControl {
        public NetworkGameConnectionControl() {
            InitializeComponent();
        }

        /// <summary>
        /// Enabledをまとめて設定
        /// </summary>
        public void SetEnabledAll(bool value) {
            base.Enabled = value;
            textBoxAddress.Enabled = value;
            textBoxUser.Enabled = value;
            textBoxPass.Enabled = value;
            checkBoxPV.Enabled = value;
            checkBoxKeepAlive.Enabled = value;
        }

        /// <summary>
        /// コンフィグからフォームへ
        /// </summary>
        public void ReadFromConfig(Config.NetworkGameConnection config) {
            textBoxAddress.Text = config.Address;
            textBoxUser.Text = config.User;
            textBoxPass.Text = config.Pass;
            checkBoxPV.Checked = config.SendPV;
            checkBoxKeepAlive.Checked = config.KeepAlive;
        }

        /// <summary>
        /// フォームからコンフィグへ
        /// </summary>
        public void WriteToConfig(Config.NetworkGameConnection config) {
            config.Address = textBoxAddress.Text;
            config.User = textBoxUser.Text;
            config.Pass = textBoxPass.Text;
            config.SendPV = checkBoxPV.Checked;
            config.KeepAlive = checkBoxKeepAlive.Checked;
        }
    }
}
