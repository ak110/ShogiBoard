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
using ShogiCore.USI;
using System.Runtime.InteropServices;

namespace ShogiBoard {
    public partial class EngineListForm : Form {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        }

        private void EngineListForm_Load(object sender, EventArgs e) {
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
                        ApplyToListViewItem(listView1.SelectedItems[0], engine);
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
        /// ダブルクリックは「設定」扱い
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
            var item = new ListViewItem(new[] {
                engine.Name,
                engine.Path,
                engine.Author,
            }) {
                Tag = engine,
            };
            SetIconToListViewItem(item);
            return item;
        }

        /// <summary>
        /// Engineの情報をListViewItemへ反映
        /// </summary>
        private void ApplyToListViewItem(ListViewItem item, Engine engine) {
            item.SubItems[0].Text = engine.Name;
            item.SubItems[1].Text = engine.Path;
            item.SubItems[2].Text = engine.Author;
            item.Tag = engine;
            SetIconToListViewItem(item);
        }

        /// <summary>
        /// リストビューアイテムにアイコンを設定
        /// </summary>
        private void SetIconToListViewItem(ListViewItem item) {
            string path = "";
            try {
                path = USIDriver.NormalizeEnginePath(((Engine)item.Tag).Path);
                if (File.Exists(path)) {
                    Icon icon = GetIcon(path);
                    FormUtility.SafeInvoke(listView1, () => {
                        item.ImageIndex = imageList1.Images.Add(icon.ToBitmap(), Color.Transparent);
                        if (0 <= item.Index) {
                            listView1.RedrawItems(item.Index, item.Index, false);
                        }
                    });
                } else {
                    logger.Debug("アイコンの読み込みに失敗: " + path);
                }
            } catch (Exception e) {
                logger.Warn("アイコンの読み込みに失敗: " + path, e);
            }
        }

        #region GetIcon

        /// <summary>
        /// Icon.ExtractAssociatedIcon()がしょんぼりなので、自前のSHGetFileInfoのラッパー。
        /// </summary>
        /// <param name="path">取得するファイルやフォルダのパス</param>
        /// <returns>失敗時はnull</returns>
        static Icon GetIcon(string path) {
            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr hSuccess = SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON | SHGFI_LARGEICON);
            if (hSuccess == IntPtr.Zero) {
                throw new Exception(path + " のアイコンの取得に失敗しました");
            }
            return Icon.FromHandle(shinfo.hIcon);
        }

        /// <summary>
        /// GetIcon()したアイコンのリソースの解放。
        /// Icon.Dispose()する前に呼ぶ必要があるような気がしなくもない。
        /// </summary>
        static void ReleaseIcon(Icon icon) {
            DestroyIcon(icon.Handle);
        }

        #region SHGetFileInfo()関係
        private struct SHFILEINFO {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        [DllImport("user32.dll")]
        extern static bool DestroyIcon(IntPtr handle);

        #endregion        
        #endregion
    }
}
