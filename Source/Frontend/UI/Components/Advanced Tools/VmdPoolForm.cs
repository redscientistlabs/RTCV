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

    public partial class VmdPoolForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public VmdPoolForm()
        {
            InitializeComponent();
        }

        internal void HandleDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        internal void HandleDragDrop(object sender, DragEventArgs e)
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

        private void UnloadVMD(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
            {
                return;
            }

            //Clear any active units to prevent bad things due to soon unloaded vmds
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearStepBlastUnits, null, true);
            foreach (var item in lbLoadedVmdList.SelectedItems)
            {
                string VmdName = item.ToString();
                //Go through the stash history and rasterize
                foreach (StashKey sk in S.GET<StashHistoryForm>().lbStashHistory.Items)
                {
                    sk.BlastLayer?.RasterizeVMDs(VmdName);
                }
                //CurrentStashKey can be separate
                StockpileManagerUISide.CurrentStashkey?.BlastLayer?.RasterizeVMDs(VmdName);

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
            if (RTCV.UI.Forms.InputBox.ShowDialog("BlastLayer to VMD", "Enter the new VMD name:", ref value) == DialogResult.OK)
            {
                name = value.Trim();
            }
            else
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = RtcCore.GetRandomKey();
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
            foreach (StashKey sk in S.GET<StashHistoryForm>().lbStashHistory.Items)
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
            if (StockpileManagerUISide.CurrentStashkey != null)
            {
                foreach (var bu in StockpileManagerUISide.CurrentStashkey.BlastLayer.Layer.Where(x => x.Domain == vmdName || x.SourceDomain == vmdName))
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

        private void HandleLoadedVmdListSelectionChange(object sender, EventArgs e)
        {
            btnSendToMyVMDs.Enabled = false;
            btnSaveVmd.Enabled = false;
            btnRenameVmd.Enabled = false;
            btnUnloadVmd.Enabled = false;


            if (lbLoadedVmdList.SelectedItem == null)
            {
                return;
            }

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

            if (cbShowVmdContents.Checked)
                tbVmdPrototype.Text = DisplayVMD(vmd);
            else
                tbVmdPrototype.Text = "";
        }

        private static string DisplayVMD(VirtualMemoryDomain vmd)
        {
            if (vmd == null)
                return "";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append($"===Singles==={Environment.NewLine}");

            foreach (var i in vmd.Proto.AddSingles)
            {
                sb.Append($"{i.ToHexString()}{Environment.NewLine}");
            }

            foreach (var i in vmd.Proto.RemoveSingles)
            {
                sb.Append($"-{i.ToHexString()}{Environment.NewLine}");
            }

            sb.Append($"{Environment.NewLine}");



            sb.Append($"===Ranges==={Environment.NewLine}");

            foreach (var i in vmd.Proto.AddRanges)
            {
                sb.Append($"{i[0].ToHexString()}-{i[1].ToHexString()}{Environment.NewLine}");
            }

            foreach (var i in vmd.Proto.RemoveRanges)
            {
                sb.Append($"-{i[0].ToHexString()}-{i[1].ToHexString()}{Environment.NewLine}");
            }

            return sb.ToString();
        }

        public void GetFocus()
        {
            foreach (var item in UICore.mtForm.cbSelectBox.Items)
            {
                if (((dynamic)item).value is VmdPoolForm)
                {
                    UICore.mtForm.cbSelectBox.SelectedItem = item;
                    break;
                }
            }


        }

        private void SaveVMD(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
            {
                return;
            }

            string vmdName = lbLoadedVmdList.SelectedItem.ToString();
            VirtualMemoryDomain vmd = MemoryDomains.VmdPool[vmdName];

            string unfixedName = vmdName.Trim().Replace("[V]", "") + ".vmd";
            string fixedName = MakeFileNameValid(unfixedName);
            if (unfixedName != fixedName && DialogResult.No == MessageBox.Show(
                    $"This VMD's name contains some invalid characters which will be replaced with look-alikes for the file's name. Do you wish to save {fixedName}?",
                    "Save file?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                return;
            
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                DefaultExt = "vmd",
                Title = "Save VMD to File",
                Filter = "VMD file|*.vmd",
                FileName = fixedName,
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
                proto.VmdName = ConvertLookalikesBack(filePath.Replace(".vmd", "").Replace(".VMD", ""));

                MemoryDomains.AddVMD(proto);
            }

            if (refreshvmds)
            {
                RefreshVMDs();
            }
        }


        private void HandleLoadVMDClick(object sender, EventArgs e)
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

        private void HandleRenameVMDClick(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
            {
                return;
            }

            string vmdName = lbLoadedVmdList.SelectedItem.ToString();

            RenameVMD(vmdName);

            RefreshVMDs();
        }

        private void SendToMyVMDs(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
            {
                return;
            }

            if (lbLoadedVmdList.SelectedItems.Count == 1)
            {
                string vmdName = lbLoadedVmdList.SelectedItem.ToString();
                VirtualMemoryDomain vmd = MemoryDomains.VmdPool[vmdName];

                string value = lbLoadedVmdList.SelectedItem.ToString().Trim().Replace("[V]", "");
                if (Forms.InputBox.ShowDialog("Add to My VMDs", "Confirm VMD name:", ref value) == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(value.Trim()))
                    {
                        MessageBox.Show("Invalid name");
                        return;
                    }

                    if (!Directory.Exists(RtcCore.VmdsDir))
                    {
                        Directory.CreateDirectory(RtcCore.VmdsDir);
                    }
                                        
                    string cleanValue = MakeFileNameValid(value);
                        
                    if (value != cleanValue && DialogResult.No == MessageBox.Show(
                            $"This VMD's name contains some invalid characters which will be replaced with look-alikes for the file's name. Do you wish to save {cleanValue}.vmd?",
                            "Save file?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                        return;
                    
                    string targetPath = Path.Combine(RtcCore.VmdsDir, cleanValue.Trim() + ".vmd");

                    if (File.Exists(targetPath))
                    {
                        var result = MessageBox.Show("This file already exists in your VMDs folder, do you want to overwrite it?", "Overwrite file?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                        {
                            return;
                        }

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

                    string cleanValue = MakeFileNameValid(itemValue);
                    if (itemValue != cleanValue && DialogResult.No == MessageBox.Show(
                            $"VMD {itemValue}'s name contains some invalid characters which will be replaced with look-alikes for the file's name. Do you wish to save {cleanValue}.vmd?",
                            "Save file?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                        return;
                    //string targetPath = Path.Combine(RtcCore.vmdsDir, value.Trim() + ".vmd");

                    string itemTargetPath = Path.Combine(RtcCore.VmdsDir, itemValue.Trim() + ".vmd");

                    if (File.Exists(itemTargetPath))
                    {
                        var result = MessageBox.Show("This file already exists in your VMDs folder, do you want to overwrite it?", "Overwrite file?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                        {
                            return;
                        }

                        File.Delete(itemTargetPath);
                    }

                    using (FileStream fs = File.Open(itemTargetPath, FileMode.Create))
                    {
                        JsonHelper.Serialize(vmd.Proto, fs);
                    }
                }
            }

            S.GET<MyVMDsForm>().RefreshVMDs();

            //Selects back the VMD Pool menu
            S.GET<VmdPoolForm>().GetFocus();

        }

        private void cbShowVmdContents_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowVmdContents.Checked)
            {
                HandleLoadedVmdListSelectionChange(null, null);
            }
            else
            {
                tbVmdPrototype.Text = "";
            }
        }
        private void btnReloadItem_Click(object sender, EventArgs e)
        {
            RefreshVMDs();
        }
        
        /// <summary>
        /// Replaces <see href="https://learn.microsoft.com/en-us/windows/win32/fileio/naming-a-file#naming-conventions">reserved characters</see> with valid look-alikes <br/>
        /// &lt; = ˂ <br/>
        /// &gt; = ˃ <br/>
        /// : = ꞉ <br/>
        /// " = ʺ <br/>
        /// / = ∕ <br/>
        /// \ = ⧵ <br/>
        /// | = │ <br/>
        /// ? = ʔ <br/>
        /// * = ∗ <br/>
        /// </summary>
        private static string MakeFileNameValid(string name)
        {
            return name.Replace("<", "\u02c2")
                .Replace(">", "\u02c3")
                .Replace(":", "\ua789")
                .Replace("\"", "\u02ba")
                .Replace("/", "\u2215")
                .Replace(@"\", "\u29f5")
                .Replace("|", "\u2502")
                .Replace("?", "\u0294")
                .Replace("*", "\u2217");
        }
        /// <summary>
        /// Converts a string made into a valid file name with <see cref="MakeFileNameValid"/> back to the original string.
        /// If the original name contained any of the look-alike characters, those will be replaced, too.
        /// </summary>
        private static string ConvertLookalikesBack(string text)
        {
            return text.Replace("\u02c2", "<")
                .Replace("\u02c3", ">")
                .Replace("\ua789", ":")
                .Replace("\u02ba", "\"")
                .Replace("\u2215", "/")
                .Replace("\u29f5", @"\")
                .Replace("\u2502", "|")
                .Replace("\u0294", "?")
                .Replace("\u2217", "*");
        }
    }
}
