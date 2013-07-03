using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShogiBoard {
    public partial class EngineOptionsControl : UserControl {
        /// <summary>
        /// 1行の高さ
        /// </summary>
        const int RowHeight = 24;

        class OptionEntry {
            public string Name;
            public Func<string> GetValue;
        }

        List<OptionEntry> options = new List<OptionEntry>();

        public EngineOptionsControl() {
            InitializeComponent();
        }

        public new bool Enabled {
            get { return base.Enabled; }
            set {
                base.Enabled = value;
                foreach (Control c in panel1.Controls) c.Enabled = value;
            }
        }

        /// <summary>
        /// 空にする
        /// </summary>
        public void Clear() {
            panel1.Height = 0;
            panel1.Controls.Clear();
            options.Clear();
        }

        /// <summary>
        /// 追加
        /// </summary>
        public void Add(string type, string name, string value, string min = "", string max = "", List<string> var = null) {
            type = type.ToLowerInvariant();
            if (value == "<empty>") value = "";

            panel1.Height += RowHeight;

            OptionEntry entry = new OptionEntry() { Name = name };

            Label label = new Label();
            label.Text = name;
            panel1.Controls.Add(label);
            label.Location = new Point(0, panel1.Height - RowHeight + (RowHeight - label.Height) / 2);

            Control valueControl;

            switch (type) {
                case "check": //
                    {
                        CheckBox checkBox = new CheckBox();
                        checkBox.Text = name;
                        bool @checked;
                        if (bool.TryParse(name, out @checked)) checkBox.Checked = @checked;

                        valueControl = checkBox;
                        entry.GetValue = () => checkBox.Checked ? "true" : "false";
                    }
                    break;

                case "spin": //
                    {
                        NumericUpDown spin = new NumericUpDown();
                        spin.ThousandsSeparator = true;
                        {
                            long spinMin;
                            if (long.TryParse(min, out spinMin)) {
                                spin.Minimum = spinMin;
                            } else {
                                spin.Minimum = 0;
                            }
                        }
                        {
                            long spinMax;
                            if (long.TryParse(max, out spinMax)) {
                                spin.Maximum = Math.Max(spin.Minimum, spinMax);
                            } else {
                                spin.Maximum = long.MaxValue;
                            }
                        }
                        {
                            long spinValue;
                            if (long.TryParse(value, out spinValue)) {
                                spin.Value = spinValue;
                            } else {
                                spin.Value = spin.Minimum;
                            }
                        }

                        valueControl = spin;
                        entry.GetValue = () => ((long)spin.Value).ToString();
                    }
                    break;

                case "combo": //
                    {
                        ComboBox comboBox = new ComboBox();
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        if (var != null) {
                            comboBox.Items.AddRange(var.ToArray());
                        }
                        comboBox.SelectedIndex = comboBox.Items.IndexOf(value);

                        valueControl = comboBox;
                        entry.GetValue = () => (string)comboBox.SelectedItem;
                    }
                    break;

                case "button": // 未対応
                    valueControl = new Label(); // dummy
                    entry.GetValue = null;
                    break;

                case "string":
                case "filename": //
                    {
                        TextBox textBox = new TextBox();
                        textBox.Text = value;

                        valueControl = textBox;
                        entry.GetValue = () => textBox.Text;
                    }
                    break;

                case "readonly": // 勝手に拡張
                    {
                        TextBox textBox = new TextBox();
                        textBox.Text = value;
                        textBox.ReadOnly = true;

                        valueControl = textBox;
                        entry.GetValue = () => textBox.Text;
                    }
                    break;

                default:
                    goto case "string";
            }

            int x = panel1.Width / 2;
            valueControl.Width = x;
            panel1.Controls.Add(valueControl);
            valueControl.Location = new Point(x, panel1.Height - RowHeight + (RowHeight - valueControl.Height) / 2);

            options.Add(entry);
        }

        /// <summary>
        /// 設定値をDictionaryへ。
        /// </summary>
        public List<Engine.Option> ToList() {
            return options
                .Where(x => x.GetValue != null)
                .Select(x => new Engine.Option { Name = x.Name, Value = x.GetValue() })
                .ToList();
        }

        /// <summary>
        /// DictionaryからAdd
        /// </summary>
        public void FromList(IEnumerable<Engine.Option> list) {
            Clear();
            foreach (var p in list) {
                Add("readonly", p.Name, p.Value);
            }
        }
    }
}
