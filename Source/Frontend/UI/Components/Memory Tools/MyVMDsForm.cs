namespace RTCV.UI
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class MyVMDsForm : ComponentForm, IAutoColorize, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public MyVMDsForm()
        {
            InitializeComponent();
        }

        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void HandleDragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var f in files)
            {
                if (f.Contains(".vmd"))
                {
                    ImportVMD(f);
                }
            }
            RefreshVMDs();
        }

        private void UnloadVMD(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
                return;

            foreach (var item in lbLoadedVmdList.SelectedItems)
            {
                string vmdPath = Path.Combine(RtcCore.VmdsDir, item.ToString());

                if (File.Exists(vmdPath))
                    File.Delete(vmdPath);
            }

            RefreshVMDs();
        }

        public void RefreshVMDs()
        {
            lbLoadedVmdList.Items.Clear();

            if (!Directory.Exists(RtcCore.VmdsDir))
                Directory.CreateDirectory(RtcCore.VmdsDir);

            var files = Directory.GetFiles(RtcCore.VmdsDir);
            foreach (var file in files)
            {
                string shortfile = file.Substring(file.LastIndexOf('\\') + 1);
                lbLoadedVmdList.Items.Add(shortfile);
            }

            btnLoadVmd.Enabled = false;
            btnSaveVmd.Enabled = false;
            btnRenameVMD.Enabled = false;
            btnUnloadVmd.Enabled = false;
        }


        private static void RenameVMD(string vmdName)
        {
            string vmdPath = Path.Combine(RtcCore.VmdsDir, vmdName);
            string name = "";
            string value = vmdName.Trim().Replace("[V]", "");
            string path = "";
            if (RTCV.UI.Forms.InputBox.ShowDialog("Renaming VMD", "Enter the new VMD name:", ref value) == DialogResult.OK)
            {
                name = value.Trim();

                path = Path.Combine(RtcCore.VmdsDir, name + ".vmd");
            }
            else
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = RtcCore.GetRandomKey();
            }

            if (File.Exists(path))
            {
                MessageBox.Show("There's already a VMD with this name. Aborting rename.");
                return;
            }

            File.Move(vmdPath, path);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RefreshVMDs();
        }

        private void HandleLoadedVmdListSelectionChange(object sender, EventArgs e)
        {
            btnLoadVmd.Enabled = false;
            btnSaveVmd.Enabled = false;
            btnRenameVMD.Enabled = false;
            btnUnloadVmd.Enabled = false;

            if (lbLoadedVmdList.SelectedItem == null)
                return;


            if (lbLoadedVmdList.SelectedItems.Count == 1)
            {
                btnSaveVmd.Enabled = true;
                btnRenameVMD.Enabled = true;
            }

            btnLoadVmd.Enabled = true;
            btnUnloadVmd.Enabled = true;
        }

        private void SaveVMD(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
                return;

            string vmdName = lbLoadedVmdList.SelectedItem.ToString();
            string path = Path.Combine(RtcCore.VmdsDir, vmdName);

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                DefaultExt = "vmd",
                Title = "Save VMD to File",
                Filter = "VMD file|*.vmd",
                FileName = vmdName.Trim().Replace("[V]", ""),
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filename = saveFileDialog1.FileName;
                Common.ReplaceFile(path, filename);
            }
        }

        private void ImportVMD(string filename)
        {
            try
            {
                Common.CopyFile(filename, RtcCore.VmdsDir, true);
                RefreshVMDs();
            }
            catch (Common.OverwriteCancelledException)
            {
            }
        }

        private void LoadVMD(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
                return;

            foreach (var item in lbLoadedVmdList.SelectedItems)
            {
                string vmdName = item.ToString();
                string path = Path.Combine(RtcCore.VmdsDir, vmdName);

                S.GET<VmdPoolForm>().loadVmd(path, true);
            }

            //switch to VMD Pool
            foreach (var item in UICore.mtForm.cbSelectBox.Items)
            {
                if (((dynamic)item).value is VmdPoolForm)
                {
                    UICore.mtForm.cbSelectBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void RenameVMD(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
                return;

            string vmdName = lbLoadedVmdList.SelectedItem.ToString();

            RenameVMD(vmdName);

            RefreshVMDs();
        }

        private void RefreshVMDFiles(object sender, EventArgs e)
        {
            RefreshVMDs();
        }

        private void ImportVMD(object sender, EventArgs e)
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
                        ImportVMD(filename);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"The VMD file {filename} could not be loaded." + ex.Message);
                    }
                }

                RefreshVMDs();
            }
            else
            {
                return;
            }
        }
    }
}
