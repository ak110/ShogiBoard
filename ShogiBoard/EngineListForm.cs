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
    public partial class EngineListForm : Form {
        EngineList engineList;
        string addEnginePath;

        class ListViewItemTagSorter<TagType> : System.Collections.IComparer {
            int System.Collections.IComparer.Compare(object x, object y) {
                return Compare((ListViewItem)x, (ListViewItem)y);
            }

            public int Compare(ListViewItem x, ListViewItem y) {
                return Compare((TagType)x.Tag, (TagType)y.Tag);
            }

            private int Compare(TagType x, TagType y) {
                return Comparer<TagType>.Default.Compare(x, y);
            }
        }

        public EngineListForm(EngineList engineList, string addEnginePath = null) {
            InitializeComponent();
            this.engineList = engineList;
            this.addEnginePath = addEnginePath;
            listView1.BeginUpdate();
            foreach (Engine engine in engineList.Engines) {
                listView1.Items.Add(ToListViewItem(engine));
            }
            listView1.ListViewItemSorter = new ListViewItemTagSorter<Engine>();
            if (0 < listView1.Items.Count) {
                listView1.Items[0].Selected = true;
            }
            listView1.EndUpdate();
        }

        private void EngineListForm_Shown(object sender, EventArgs e) {
            listView1.Focus();
            if (!string.IsNullOrEmpty(addEnginePath))
                AddEngineAsync(addEnginePath);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e) {
            // 選択されている場合のみ有効にする項目
            bool selected = 0 < listView1.SelectedItems.Count;
            設定MToolStripMenuItem.Enabled = selected;
            削除DToolStripMenuItem.Enabled = selected;
            複製を追加CToolStripMenuItem.Enabled = selected;
        }

        private void 追加AToolStripMenuItem_Click(object sender, EventArgs e) {
            AddEngine(new Engine());
        }

        private void 設定MToolStripMenuItem_Click(object sender, EventArgs e) {
            if (0 < listView1.SelectedItems.Count) {
                Engine engine = (Engine)listView1.SelectedItems[0].Tag;
                using (EngineForm form = new EngineForm(engine, engineList)) {
                    if (form.ShowDialog(this) == DialogResult.OK) {
                        // 保存
                        ConfigSerializer.Serialize(engineList);
                    }
                }
            }
        }

        private void 削除DToolStripMenuItem_Click(object sender, EventArgs e) {
            if (0 < listView1.SelectedItems.Count) {
                ListViewItem item = listView1.SelectedItems[0];
                Engine engine = (Engine)item.Tag;
                if (MessageBox.Show(this, engine.Name + "を削除します。よろしいですか?", "確認",
                    MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK) {
                    if (engineList.Engines.Remove(engine)) {
                        listView1.Items.Remove(item);
                        // 書き込み
                        ConfigSerializer.Serialize(engineList);
                    }
                }
            }
        }

        private void 複製を追加CToolStripMenuItem_Click(object sender, EventArgs e) {
            if (0 < listView1.SelectedItems.Count) {
                Engine engine = (Engine)listView1.SelectedItems[0].Tag;
                AddEngine(engine.Clone());
            }
        }

        /// <summary>
        /// ダブルクリックは「変更」扱い
        /// </summary>
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                設定MToolStripMenuItem_Click(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Deleteキーで「削除」、Escキーで閉じる。
        /// </summary>
        private void listView1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                削除DToolStripMenuItem_Click(this, EventArgs.Empty);
                e.Handled = true;
            } else if (e.KeyCode == Keys.Escape) {
                Close();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Escキーで閉じる。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EngineListForm_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                Close();
                e.Handled = true;
            }
        }

        private void listView1_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files.Length == 1 && File.Exists(files[0])) {
                    e.Effect = DragDropEffects.Link;
                }
            }
        }

        /// <summary>
        /// D&Dによる追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_DragDrop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files.Length == 1 && File.Exists(files[0])) {
                    // ずっとD&D元のエクスプローラが固まるのが嫌なので一度制御を返してからInvoke()してダイアログ開く
                    AddEngineAsync(files[0]);
                }
            }
        }

        /// <summary>
        /// エンジンの追加(非同期版)
        /// </summary>
        private void AddEngineAsync(string path) {
            ThreadPool.QueueUserWorkItem(new WaitCallback(arg => {
                FormUtility.SafeInvoke(this, () => {
                    AddEngine(new Engine { Path = path });
                });
            }));
        }

        /// <summary>
        /// エンジンの追加
        /// </summary>
        /// <param name="engine"></param>
        private void AddEngine(Engine engine) {
            using (EngineForm form = new EngineForm(engine, engineList)) {
                if (form.ShowDialog(this) == DialogResult.OK) {
                    listView1.Items.Add(ToListViewItem(engine));
                    engineList.Engines.Add(engine);
                    engineList.Engines.Sort();
                    // 書き込み
                    ConfigSerializer.Serialize(engineList);
                }
            }
        }

        /// <summary>
        /// EngineからListViewItemを作成
        /// </summary>
        private ListViewItem ToListViewItem(Engine engine) {
            return new ListViewItem(new[] {
                engine.Name,
                engine.Path,
                engine.Author,
            }) {
                Tag = engine,
            };
        }
    }
}
