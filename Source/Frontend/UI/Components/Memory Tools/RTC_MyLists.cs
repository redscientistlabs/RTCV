namespace RTCV.UI
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using static RTCV.UI.UI_Extensions;

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
                    importList(f);
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
                string listPath = Path.Combine(RtcCore.listsDir, item.ToString().Replace("[DISABLED] ", "$"));

                if (File.Exists(listPath))
                    File.Delete(listPath);
            }

            RefreshLists();
        }

        public void RefreshLists()
        {
            lbKnownLists.Items.Clear();

            if (!Directory.Exists(RtcCore.listsDir))
                Directory.CreateDirectory(RtcCore.listsDir);

            var files = Directory.GetFiles(RtcCore.listsDir).OrderBy(it => it.Replace("$", ""));
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
            string listPath = Path.Combine(RtcCore.listsDir, listName);
            string name = "";
            string value = listName.Trim();
            string path = "";
            if (UI_Extensions.GetInputBox("Renaming List", "Enter the new List name:", ref value) == DialogResult.OK)
            {
                name = value.Trim();

                path = Path.Combine(RtcCore.listsDir, name + ".txt");
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
            string path = Path.Combine(RtcCore.listsDir, listName);

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
                string filename = saveFileDialog1.FileName;

                if (File.Exists(filename))
                    File.Delete(filename);

                File.Copy(path, filename);
            }
        }

        private void importList(string path)
        {
            string shortPath = path.Substring(path.LastIndexOf('\\') + 1);
            string targetPath = Path.Combine(RtcCore.listsDir, shortPath);

            if (File.Exists(targetPath))
            {
                var result = MessageBox.Show("This file already exist in your VMDs folder, do you want to overwrite it?", "Overwrite file?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    return;

                File.Delete(targetPath);
            }

            File.Copy(path, targetPath);

            RefreshLists();
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

                string pathDisabled = Path.Combine(RtcCore.listsDir, "$" + cleanListName);
                string pathEnabled = Path.Combine(RtcCore.listsDir, cleanListName);

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
            UICore.LoadLists(RtcCore.listsDir);
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
            UICore.LoadLists(RtcCore.listsDir);
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
                        importList(filename);
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
