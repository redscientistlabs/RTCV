namespace RTCV.UI
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;
    using static RTCV.UI.UI_Extensions;

    public partial class RTC_VmdPool_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_VmdPool_Form()
        {
            InitializeComponent();
            AllowDrop = true;
            this.DragEnter += RTC_VmdPool_Form_DragEnter;
            this.DragDrop += RTC_VmdPool_Form_DragDrop;
        }

        public void RTC_VmdPool_Form_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        public void RTC_VmdPool_Form_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var f in files)
            {
                if (f.Contains(".vmd"))
                {
                    loadVmd(f, false);
                }
            }
            RefreshVMDs();
        }

        private void btnUnloadVMD_Click(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
                return;

            //Clear any active units to prevent bad things due to soon unloaded vmds
            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
            foreach (var item in lbLoadedVmdList.SelectedItems)
            {
                string VmdName = item.ToString();
                //Go through the stash history and rasterize
                foreach (StashKey sk in S.GET<RTC_StashHistory_Form>().lbStashHistory.Items)
                {
                    sk.BlastLayer?.RasterizeVMDs(VmdName);
                }
                //CurrentStashKey can be separate
                StockpileManager_UISide.CurrentStashkey?.BlastLayer?.RasterizeVMDs(VmdName);

                MemoryDomains.RemoveVMD(VmdName);
            }
            RefreshVMDs();
        }

        public void RefreshVMDs()
        {
            lbLoadedVmdList.Items.Clear();
            lbLoadedVmdList.Items.AddRange(MemoryDomains.VmdPool.Values.Select(it => it.ToString()).ToArray());

            lbRealDomainValue.Text = "#####";
            lbVmdSizeValue.Text = "#####";

            btnSendToMyVMDs.Enabled = false;
            btnSaveVmd.Enabled = false;
            btnRenameVmd.Enabled = false;
            btnUnloadVmd.Enabled = false;
        }

        private static void RenameVMD(VirtualMemoryDomain VMD)
        {
            RenameVMD(VMD.ToString());
        }

        private static void RenameVMD(string vmdName)
        {
            if (!MemoryDomains.VmdPool.ContainsKey(vmdName))
            {
                return;
            }

            string name = "";
            string value = vmdName.Trim().Replace("[V]", "");
            if (UI_Extensions.GetInputBox("BlastLayer to VMD", "Enter the new VMD name:", ref value) == DialogResult.OK)
            {
                name = value.Trim();
            }
            else
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = CorruptCore.RtcCore.GetRandomKey();
            }

            if (MemoryDomains.VmdPool.ContainsKey(name))
            {
                MessageBox.Show("There's already a VMD with this name. Aborting rename.");
                return;
            }

            VirtualMemoryDomain VMD = MemoryDomains.VmdPool[vmdName];

            MemoryDomains.RemoveVMD(VMD);
            VMD.Name = name;
            VMD.Proto.VmdName = name;
            MemoryDomains.AddVMD(VMD);

            foreach (BlastUnit bu in StepActions.GetRawBlastLayer().Layer)
            {
                if (bu.Domain == vmdName)
                {
                    bu.Domain = "[V]" + name;
                }

                if (bu.SourceDomain == vmdName)
                {
                    bu.SourceDomain = "[V]" + name;
                }
            }
            //Go through the stash history and update any references
            foreach (StashKey sk in S.GET<RTC_StashHistory_Form>().lbStashHistory.Items)
            {
                foreach (var bu in sk.BlastLayer.Layer)
                {
                    if (bu.Domain == vmdName)
                    {
                        bu.Domain = "[V]" + name;
                    }

                    if (bu.SourceDomain == vmdName)
                    {
                        bu.SourceDomain = "[V]" + name;
                    }
                }
            }
            //CurrentStashKey can be separate
            if (StockpileManager_UISide.CurrentStashkey != null)
            {
                foreach (var bu in StockpileManager_UISide.CurrentStashkey.BlastLayer.Layer.Where(x => x.Domain == vmdName || x.SourceDomain == vmdName))
                {
                    if (bu.Domain == vmdName)
                    {
                        bu.Domain = "[V]" + name;
                    }

                    if (bu.SourceDomain == vmdName)
                    {
                        bu.SourceDomain = "[V]" + name;
                    }
                }
            }
        }

        private void RTC_VmdPool_Form_Load(object sender, EventArgs e)
        {
        }

        private void lbLoadedVmdList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSendToMyVMDs.Enabled = false;
            btnSaveVmd.Enabled = false;
            btnRenameVmd.Enabled = false;
            btnUnloadVmd.Enabled = false;


            if (lbLoadedVmdList.SelectedItem == null)
                return;


            if (lbLoadedVmdList.SelectedItems.Count == 1)
            {
                btnSaveVmd.Enabled = true;
                btnRenameVmd.Enabled = true;
            }

            btnSendToMyVMDs.Enabled = true;
            btnUnloadVmd.Enabled = true;

            string vmdName = lbLoadedVmdList.SelectedItem.ToString();
            MemoryInterface mi = MemoryDomains.VmdPool[vmdName];

            lbVmdSizeValue.Text = mi.Size.ToString() + " (0x" + mi.Size.ToString("X") + ")";

            var vmd = (mi as VirtualMemoryDomain);

            if (vmd.Compacted)
            {
                lbRealDomainValue.Text = (vmd.CompactPointerDomains.Length > 1 ? "Hybrid" : vmd.CompactPointerDomains.First());
            }
            else
            {
                if (vmd.PointerDomains.Distinct().Count() > 1)
                {
                    lbRealDomainValue.Text = "Hybrid";
                }
                else
                {
                    lbRealDomainValue.Text = vmd.PointerDomains.FirstOrDefault();
                }
            }

            //display proto here

            tbVmdPrototype.Text = DisplayVMD(vmd);
        }

        private string DisplayVMD(VirtualMemoryDomain vmd)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append($"===Singles==={Environment.NewLine}");

            foreach (var i in vmd.Proto.AddSingles)
                sb.Append($"{i.ToHexString()}{Environment.NewLine}");

            foreach (var i in vmd.Proto.RemoveSingles)
                sb.Append($"-{i.ToHexString()}{Environment.NewLine}");


            sb.Append($"{Environment.NewLine}");



            sb.Append($"===Ranges==={Environment.NewLine}");

            foreach (var i in vmd.Proto.AddRanges)
                sb.Append($"{i[0].ToHexString()}-{i[1].ToHexString()}{Environment.NewLine}");

            foreach (var i in vmd.Proto.RemoveRanges)
                sb.Append($"-{i[0].ToHexString()}-{i[1].ToHexString()}{Environment.NewLine}");

            return sb.ToString();
        }

        private void btnSaveVmd_Click(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
                return;

            string vmdName = lbLoadedVmdList.SelectedItem.ToString();
            VirtualMemoryDomain vmd = MemoryDomains.VmdPool[vmdName];

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                DefaultExt = "vmd",
                Title = "Save VMD to File",
                Filter = "VMD file|*.vmd",
                FileName = vmdName.Trim().Replace("[V]", "") + ".vmd",
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;

                //create json file for vmd
                using (FileStream fs = File.Open(filename, FileMode.Create))
                {
                    JsonHelper.Serialize(vmd.Proto, fs);
                }
            }
        }

        internal void loadVmd(string path, bool refreshvmds)
        {
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                VmdPrototype proto = null;
                proto = JsonHelper.Deserialize<VmdPrototype>(fs);

                string filePath = path.Substring(path.LastIndexOf('\\') + 1);
                proto.VmdName = filePath.Replace(".vmd", "").Replace(".VMD", "");

                MemoryDomains.AddVMD(proto);
            }

            if (refreshvmds)
            {
                RefreshVMDs();
            }
        }


        private void btnLoadVmd_Click(object sender, EventArgs e)
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
                        loadVmd(filename, false);
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

        private void btnRenameVMD_Click(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
                return;

            string vmdName = lbLoadedVmdList.SelectedItem.ToString();

            RenameVMD(vmdName);

            RefreshVMDs();
        }

        private void btnSendToMyVMDs_Click(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
                return;

            if (lbLoadedVmdList.SelectedItems.Count == 1)
            {
                string vmdName = lbLoadedVmdList.SelectedItem.ToString();
                VirtualMemoryDomain vmd = MemoryDomains.VmdPool[vmdName];

                string value = lbLoadedVmdList.SelectedItem.ToString().Trim().Replace("[V]", "");
                if (GetInputBox("Add to My VMDs", "Confirm VMD name:", ref value) == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(value.Trim()))
                    {
                        MessageBox.Show("Invalid name");
                        return;
                    }

                    if (!Directory.Exists(RtcCore.VmdsDir))
                        Directory.CreateDirectory(RtcCore.VmdsDir);

                    string targetPath = Path.Combine(RtcCore.VmdsDir, value.Trim() + ".vmd");

                    if (File.Exists(targetPath))
                    {
                        var result = MessageBox.Show("This file already exists in your VMDs folder, do you want to overwrite it?", "Overwrite file?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                            return;

                        File.Delete(targetPath);
                    }

                    //creates json file for vmd
                    using (FileStream fs = File.Open(targetPath, FileMode.Create))
                    {
                        JsonHelper.Serialize(vmd.Proto, fs);
                    }
                }
            }
            else //multiple selected
            {
                foreach (var item in lbLoadedVmdList.SelectedItems)
                {
                    string vmdName = item.ToString();
                    VirtualMemoryDomain vmd = MemoryDomains.VmdPool[vmdName];

                    string itemValue = item.ToString().Trim().Replace("[V]", "");

                    //string targetPath = Path.Combine(RtcCore.vmdsDir, value.Trim() + ".vmd");

                    string itemTargetPath = Path.Combine(RtcCore.VmdsDir, itemValue.Trim() + ".vmd");

                    if (File.Exists(itemTargetPath))
                    {
                        var result = MessageBox.Show("This file already exists in your VMDs folder, do you want to overwrite it?", "Overwrite file?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                            return;

                        File.Delete(itemTargetPath);
                    }

                    using (FileStream fs = File.Open(itemTargetPath, FileMode.Create))
                    {
                        JsonHelper.Serialize(vmd.Proto, fs);
                    }
                }
            }

            S.GET<RTC_MyVMDs_Form>().RefreshVMDs();

            //switch to My VMDs
            foreach (var item in UICore.mtForm.cbSelectBox.Items)
            {
                if (((dynamic)item).value is RTC_MyVMDs_Form)
                {
                    UICore.mtForm.cbSelectBox.SelectedItem = item;
                    break;
                }
            }
        }
    }
}
