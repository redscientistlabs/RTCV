namespace RTCV.UI
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class RTC_MyLists_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_MyLists_Form()
        {
            InitializeComponent();
            AllowDrop = true;
            this.DragEnter += RTC_MyLists_Form_DragEnter;
            this.DragDrop += RTC_MyLists_Form_DragDrop;
        }

        private void RTC_MyLists_Form_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void RTC_MyLists_Form_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var f in files)
            {
                if (f.Contains(".txt"))
                {
                    ImportList(f);
                }
            }
            RefreshLists();
        }

        private void btnRemoveList_Click(object sender, EventArgs e)
        {
            if (lbKnownLists.SelectedIndex == -1)
                return;

            foreach (var item in lbKnownLists.SelectedItems)
            {
                string listPath = Path.Combine(RtcCore.ListsDir, item.ToString().Replace("[DISABLED] ", "$"));

                if (File.Exists(listPath))
                    File.Delete(listPath);
            }

            RefreshLists();
        }

        public void RefreshLists()
        {
            lbKnownLists.Items.Clear();

            if (!Directory.Exists(RtcCore.ListsDir))
                Directory.CreateDirectory(RtcCore.ListsDir);

            var files = Directory.GetFiles(RtcCore.ListsDir).OrderBy(it => it.Replace("$", ""));
            foreach (var file in files)
            {
                string shortfile = file.Substring(file.LastIndexOf('\\') + 1);
                lbKnownLists.Items.Add(shortfile.Replace("$", "[DISABLED] "));
            }

            btnImportList.Enabled = false;
            btnSaveList.Enabled = false;
            btnRenameList.Enabled = false;
            btnRemoveList.Enabled = false;
        }


        private static void RenameList(string listName)
        {
            string listPath = Path.Combine(RtcCore.ListsDir, listName);
            string name = "";
            string value = listName.Trim();
            string path = "";
            if (UI_Extensions.GetInputBox("Renaming List", "Enter the new List name:", ref value) == DialogResult.OK)
            {
                name = value.Trim();

                path = Path.Combine(RtcCore.ListsDir, name + ".txt");
            }
            else
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = CorruptCore.RtcCore.GetRandomKey();
            }

            if (File.Exists(path))
            {
                MessageBox.Show("There's already a List with this name. Aborting rename.");
                return;
            }

            File.Move(listPath, path);
        }

        private void RTC_MyLists_Form_Load(object sender, EventArgs e)
        {
            RefreshLists();
        }

        private void lbLoadedVmdList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnImportList.Enabled = false;
            btnSaveList.Enabled = false;
            btnRenameList.Enabled = false;
            btnRemoveList.Enabled = false;

            if (lbKnownLists.SelectedItem == null)
                return;


            if (lbKnownLists.SelectedItems.Count == 1)
            {
                btnSaveList.Enabled = true;
                btnRenameList.Enabled = true;
            }

            btnImportList.Enabled = true;
            btnRemoveList.Enabled = true;

            bool allDisabled = true;

            foreach (var item in lbKnownLists.SelectedItems)
            {
                if (!item.ToString().Contains("[DISABLED] "))
                {
                    allDisabled = false;
                    break;
                }
            }

            if (allDisabled)
                btnEnableDisableList.Text = "  Enable List";
            else
            {
                btnEnableDisableList.Text = "  Disable List";
            }
        }

        private void btnSaveList_Click(object sender, EventArgs e)
        {
            if (lbKnownLists.SelectedIndex == -1)
                return;

            string listName = lbKnownLists.SelectedItem.ToString().Replace("[DISABLED] ", "");
            string path = Path.Combine(RtcCore.ListsDir, listName);

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                DefaultExt = "txt",
                Title = "Save List to File",
                Filter = "Text file|*.txt",
                FileName = listName.Trim(),
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filename = saveFileDialog1.FileName;
                Common.ReplaceFile(path, filename);
            }
        }

        private void ImportList(string filename)
        {
            try
            {
                Common.CopyFile(filename, RtcCore.ListsDir, true);
                RefreshLists();
            }
            catch (Common.OverwriteCancelledException)
            {
            }
        }

        private void btnLoadVmd_Click(object sender, EventArgs e)
        {
            if (lbKnownLists.SelectedIndex == -1)
                return;

            foreach (var item in lbKnownLists.SelectedItems)
            {
                string listName = item.ToString();
                bool isDisabled = listName.Contains("[DISABLED] ");

                string cleanListName = listName.Replace("[DISABLED] ", "$");
                if (cleanListName[0] == '$')
                    cleanListName = cleanListName.Substring(1);

                string pathDisabled = Path.Combine(RtcCore.ListsDir, "$" + cleanListName);
                string pathEnabled = Path.Combine(RtcCore.ListsDir, cleanListName);

                if (btnEnableDisableList.Text.Contains("Disable"))
                {
                    if (!isDisabled)
                    {
                        File.Move(pathEnabled, pathDisabled);
                    }
                }
                else //button says enable
                {
                    if (isDisabled)
                    {
                        File.Move(pathDisabled, pathEnabled);
                    }
                }
            }

            CorruptCore.Filtering.ResetLoadedListsInUI();

            //reload lists
            UICore.LoadLists(RtcCore.ListsDir);
            UICore.LoadLists(Path.Combine(RtcCore.EmuDir, "LISTS"));

            RefreshLists();
        }

        private void btnRenameVMD_Click(object sender, EventArgs e)
        {
            if (lbKnownLists.SelectedIndex == -1)
                return;

            string vmdName = lbKnownLists.SelectedItem.ToString().Replace("[DISABLED] ", "$");

            RenameList(vmdName);

            RefreshLists();
        }

        private void btnRefreshVmdFiles_Click(object sender, EventArgs e)
        {
            CorruptCore.Filtering.ResetLoadedListsInUI();

            //reload lists
            UICore.LoadLists(RtcCore.ListsDir);
            UICore.LoadLists(Path.Combine(RtcCore.EmuDir, "LISTS"));

            RefreshLists();
        }

        private void btnImportVmd_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "vmd",
                Multiselect = true,
                Title = "Open VMD File",
                Filter = "VMD files|*.vmd",
                RestoreDirectory = true
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //string Filename = ofd.FileName.ToString();
                foreach (string filename in ofd.FileNames)
                {
                    try
                    {
                        ImportList(filename);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"The VMD file {filename} could not be loaded." + ex.Message);
                    }
                }

                RefreshLists();
            }
            else
            {
                return;
            }
        }
    }
}
