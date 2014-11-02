using ShogiCore.USI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ShogiBoard {
    public partial class EngineForm : Form {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Engine engine;
        EngineList engineList; // 重複チェック用

        public EngineForm(Engine engine, EngineList engineList) {
            InitializeComponent();
            this.engine = engine;
            this.engineList = engineList;
            // Engineからコントロールへ
            textBoxName.Text = engine.Name;
            textBoxAuthor.Text = engine.Author;
            textBoxPath.Text = engine.Path;
            checkBoxUSIPonder.Checked = engine.USIPonder;
            numericUpDownHash.Value = engine.USIHash;
            checkBox1.Checked = engine.ByoyomiHack;
            engineOptionsControl1.FromList(engine.Options);
            // 有効無効を設定
            textBoxPath_TextChanged(this, EventArgs.Empty);
        }

        private void buttonOk_Click(object sender, EventArgs e) {
            // 重複チェックを行う
            if (engineList.Engines.Any(
                    x => !object.ReferenceEquals(x, engine) &&
                    x.Name == textBoxName.Text &&
                    x.Path == textBoxPath.Text)) {
                MessageBox.Show(this, "名前・パスが同じエンジンは複数登録できません。", "エラー");
                DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            // エンジンの存在チェックを行う
            string path = USIDriver.NormalizeEnginePath(textBoxPath.Text);
            if (!File.Exists(path)) {
                MessageBox.Show(this, path + " が存在しません。", "エラー");
                DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            // コントロールからEngineへ
            engine.Name = textBoxName.Text;
            engine.Author = textBoxAuthor.Text;
            engine.Path = textBoxPath.Text;
            engine.USIPonder = checkBoxUSIPonder.Checked;
            engine.USIHash = (int)numericUpDownHash.Value;
            engine.ByoyomiHack = checkBox1.Checked;
            engine.Options = engineOptionsControl1.ToList();
        }

        /// <summary>
        /// 表示直後の処理
        /// </summary>
        private void EngineForm_Shown(object sender, EventArgs e) {
            if (textBoxName.TextLength <= 0 && textBoxAuthor.TextLength <= 0 && 0 < textBoxPath.TextLength) {
                // NameとAutorが空でPathだけ指定されてたら
                // 多分きっと一覧へのエンジンのD&Dなのでエンジンから取得ボタン押下扱い
                buttonGetByEngine_Click(this, EventArgs.Empty);
            }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e) {
            UpdateEnables();
        }

        /// <summary>
        /// エンジンのパスが変更されたら活性非活性を調整
        /// </summary>
        private void textBoxPath_TextChanged(object sender, EventArgs e) {
            try {
                string path = USIDriver.NormalizeEnginePath(textBoxPath.Text);
                buttonGetByEngine.Enabled = File.Exists(path);
                UpdateEnables();
            } catch {
                // 入力は色々あり得るので黙殺
            }
        }

        /// <summary>
        /// コントロールの有効無効を設定
        /// </summary>
        private void UpdateEnables() {
            buttonOk.Enabled = 0 < textBoxName.TextLength && 0 < textBoxPath.TextLength;
        }

        private void textBoxPath_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files.Length == 1 && File.Exists(files[0])) {
                    e.Effect = DragDropEffects.Link;
                }
            }
        }

        private void textBoxPath_DragDrop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files.Length == 1 && File.Exists(files[0])) {
                    textBoxPath.Text = files[0];
                }
            }
        }

        /// <summary>
        /// エンジンから取得(エンジン名・作者名)
        /// </summary>
        private void buttonGetByEngine_Click(object sender, EventArgs e) {
            textBoxName.Enabled = false;
            textBoxAuthor.Enabled = false;
            buttonGetByEngine.Enabled = false;
            ThreadPool.QueueUserWorkItem(new WaitCallback(arg => {
                try {
                    using (USIDriver usi = new USIDriver(textBoxPath.Text)) {
                        usi.Start();
                        FormUtility.SafeInvoke(this, () => {
                            textBoxName.Text = usi.IdName;
                            textBoxAuthor.Text = usi.IdAuthor;
                        });
                    }
                } catch (Exception ex) {
                    logger.Warn("エンジンからの情報取得に失敗", ex);
                    FormUtility.SafeInvoke(this, () => {
                        // 空なら仮でファイル名を設定
                        if (textBoxName.TextLength <= 0) {
                            try {
                                textBoxName.Text = Path.GetFileNameWithoutExtension(textBoxPath.Text);
                            } catch (Exception ex2) {
                                logger.Warn("ファイル名の取得に失敗", ex2);
                            }
                        }
                        // エラーメッセージ
                        MessageBox.Show(this, "エンジンからの名前/作者取得に失敗しました。", "エラー");
                    });
                } finally {
                    FormUtility.SafeInvoke(this, () => {
                        buttonGetByEngine.Enabled = true;
                        textBoxAuthor.Enabled = true;
                        textBoxName.Enabled = true;
                    });
                }
            }));
        }

        /// <summary>
        /// ...
        /// </summary>
        private void button1_Click(object sender, EventArgs e) {
            string path = USIDriver.NormalizeEnginePath(textBoxPath.Text);
            string dir = Path.GetDirectoryName(path);
            if (Directory.Exists(dir)) {
                openFileDialog1.InitialDirectory = dir;
                openFileDialog1.FileName = Path.GetFileName(path);
            } else {
                openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory; // 適当
            }
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK) {
                textBoxPath.Text = openFileDialog1.FileName;
            }
        }

        /// <summary>
        /// エンジンから取得(設定)
        /// </summary>
        private void button2_Click(object sender, EventArgs e) {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            engineOptionsControl1.Enabled = false;
            ThreadPool.QueueUserWorkItem(new WaitCallback(arg => {
                try {
                    using (USIDriver usi = new USIDriver(textBoxPath.Text)) {
                        usi.Start();
                        FormUtility.SafeInvoke(this, () => {
                            engineOptionsControl1.Clear();
                            foreach (var opt in usi.Options) {
                                engineOptionsControl1.Add(
                                    opt.Type, opt.Name, opt.Default,
                                    opt.Min, opt.Max, opt.Var);
                            }
                        });
                    }
                } catch (Exception ex) {
                    logger.Warn("エンジンからの情報取得に失敗", ex);
                    FormUtility.SafeInvoke(this, () => {
                        MessageBox.Show(this, "エンジンからの名前/作者取得に失敗しました。", "エラー");
                    });
                } finally {
                    FormUtility.SafeInvoke(this, () => {
                        button2.Enabled = true;
                        button3.Enabled = true;
                        button4.Enabled = true;
                        engineOptionsControl1.Enabled = true;
                    });
                }
            }));
        }

        /// <summary>
        /// クリア
        /// </summary>
        private void button3_Click(object sender, EventArgs e) {
            engineOptionsControl1.Clear();
        }

        /// <summary>
        /// エンジンへ送信してクリア
        /// </summary>
        private void button4_Click(object sender, EventArgs e) {
            button2.Enabled = false;
            button4.Enabled = false;
            button3.Enabled = false;
            engineOptionsControl1.Enabled = false;
            var list = engineOptionsControl1.ToList();
            ThreadPool.QueueUserWorkItem(new WaitCallback(arg => {
                try {
                    using (USIDriver usi = new USIDriver(textBoxPath.Text)) {
                        usi.Start();
                        foreach (var p in list) {
                            usi.SendSetOption(p.Name, p.Value);
                        }
                    }
                } catch (Exception ex) {
                    logger.Warn("エンジンへの送信に失敗", ex);
                    FormUtility.SafeInvoke(this, () => {
                        MessageBox.Show(this, "エンジンへの送信に失敗しました。", "エラー");
                    });
                } finally {
                    FormUtility.SafeInvoke(this, () => {
                        button2.Enabled = true;
                        button4.Enabled = true;
                        button3.Enabled = true;
                        engineOptionsControl1.Enabled = true;
                        engineOptionsControl1.Clear();
                    });
                }
            }));
        }
    }
}
