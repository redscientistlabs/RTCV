namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;
    using System.IO;
    using System.Threading.Tasks;

    public partial class StashHistoryForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public bool DontLoadSelectedStash { get; set; }
        public bool LoadWhenSelectedWithArrows { get; set; } = Params.IsParamSet("LOAD_STASH_ON_ARROW_CLICK");

        public StashHistoryForm()
        {
            InitializeComponent();

            lbStashHistory.DataSource = StockpileManagerUISide.StashHistory;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var f in files)
            {
                if (f.Contains(".bl"))
                {
                    BlastLayer temp = BlastTools.LoadBlastLayerFromFile(f);
                    StockpileManagerUISide.Import(temp);
                    S.GET<StashHistoryForm>().RefreshStashHistory();
                }
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        public void AddStashToStockpileButtonClick(object sender, EventArgs e) => AddStashToStockpileFromUI();
        public bool AddStashToStockpileFromUI()
        {
            if (StockpileManagerUISide.CurrentStashkey != null && StockpileManagerUISide.CurrentStashkey.Alias != StockpileManagerUISide.CurrentStashkey.Key)
            {
                return AddStashToStockpile(false);
            }
            else
            {
                return AddStashToStockpile(true);
            }
            
        }

        public bool AddStashToStockpile(bool askForName = true, string itemName = null)
        {
            if (lbStashHistory.Items.Count == 0 || lbStashHistory.SelectedIndex == -1)
            {
                MessageBox.Show("Can't add the Stash to the Stockpile because none is selected in the Stash History");
                return false;
            }

            string Name = "";
            string value = "";



            StashKey sk = (StashKey)lbStashHistory.SelectedItem;
            StockpileManagerUISide.CurrentStashkey = sk;

            // Disabled with the addition of cross-emulator stockpiles
            /*
            //If we don't support mixed stockpiles
            if (!((bool?)AllSpec.VanguardSpec[VSPEC.SUPPORTS_MIXED_STOCKPILE] ?? false))
            {
                if (S.GET<StockpileManagerForm>().dgvStockpile.Rows.Count > 0)
                {
                    string firstGameName = ((StashKey)S.GET<StockpileManagerForm>().dgvStockpile[0, 0].Value).GameName;
                    if (sk.GameName != firstGameName)
                    {
                        string name = (AllSpec.VanguardSpec[VSPEC.NAME] as string) ?? "Vanguard implementation";
                        MessageBox.Show($"{name} does not support mixed stockpiles.");
                        return false;
                    }
                }
            }
            */

            if (askForName)
            {
                if (RTCV.UI.Forms.InputBox.ShowDialog("Renaming Stashkey", "Enter the new Stash name:", ref value) == DialogResult.OK)
                {
                    Name = value.Trim();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Name = StockpileManagerUISide.CurrentStashkey.Alias;

                if (!string.IsNullOrWhiteSpace(itemName))
                {
                    if(itemName.Contains("\\"))
                    {
                        //assume it's a full path
                        var fi = new System.IO.FileInfo(itemName);
                        Name = System.IO.Path.GetFileNameWithoutExtension(fi.Name);
                    }
                    else
                        Name = itemName;
                }
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                StockpileManagerUISide.CurrentStashkey.Alias = StockpileManagerUISide.CurrentStashkey.Key;
            }
            else
            {
                StockpileManagerUISide.CurrentStashkey.Alias = Name;
            }

            if (Params.IsParamSet("RASTERIZE_VMD_UPON_STOCKPILING"))
                sk.BlastLayer.RasterizeVMDs();

            DataGridViewRow dataRow = S.GET<StockpileManagerForm>().dgvStockpile.Rows[S.GET<StockpileManagerForm>().dgvStockpile.Rows.Add()];
            dataRow.Cells["Item"].Value = sk;
            dataRow.Cells["GameName"].Value = sk.GameName;
            dataRow.Cells["SystemName"].Value = sk.SystemName;
            dataRow.Cells["SystemCore"].Value = sk.SystemCore;
            dataRow.Cells["EmuVer"].Value = sk.EmuVer;
            
            S.GET<StockpileManagerForm>().RefreshNoteIcons();

            StockpileManagerUISide.StashHistory.Remove(sk);

            RefreshStashHistory();

            DontLoadSelectedStash = true;
            lbStashHistory.ClearSelected();
            DontLoadSelectedStash = false;

            int nRowIndex = S.GET<StockpileManagerForm>().dgvStockpile.Rows.Count - 1;

            S.GET<StockpileManagerForm>().dgvStockpile.ClearSelection();
            S.GET<StockpileManagerForm>().dgvStockpile.Rows[nRowIndex].Selected = true;

            StockpileManagerUISide.StockpileChanged();

            S.GET<StockpileManagerForm>().UnsavedEdits = true;

            //Ensure it is redrawn to prevent weird issues such as the merge button not returning into the corrupt button
            S.GET<GlitchHarvesterBlastForm>().RedrawActionUI();

            return true;
        }

        public void RefreshStashHistory(object sender = null, EventArgs e = null)
        {
            DontLoadSelectedStash = true;
            var lastSelect = lbStashHistory.SelectedIndex;

            DontLoadSelectedStash = true;
            lbStashHistory.DataSource = null;
            lbStashHistory.SelectedIndex = -1;

            DontLoadSelectedStash = true;
            //lbStashHistory.BeginUpdate();
            lbStashHistory.DataSource = StockpileManagerUISide.StashHistory;
            //lbStashHistory.EndUpdate();

            DontLoadSelectedStash = true;
            if (lastSelect < lbStashHistory.Items.Count)
            {
                lbStashHistory.SelectedIndex = lastSelect;
            }

            DontLoadSelectedStash = false;
        }

        public void RemoveFirstStashHistoryItem()
        {
            DontLoadSelectedStash = true;
            lbStashHistory.DataSource = null;
            lbStashHistory.SelectedIndex = -1;

            DontLoadSelectedStash = true;
            //lbStashHistory.BeginUpdate();
            StockpileManagerUISide.RemoveFirstStashItem();
            lbStashHistory.DataSource = StockpileManagerUISide.StashHistory;
            DontLoadSelectedStash = false;
        }

        private void HandleStashHistoryMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);
                
                BlastLayer bl = null;
                if(lbStashHistory.SelectedIndex != -1)
                    bl = StockpileManagerUISide.StashHistory[lbStashHistory.SelectedIndex].BlastLayer;
                
                bool selectionExists = lbStashHistory.SelectedIndex != -1;
                
                new ContextMenuBuilder()
                    .If(bl != null).AddText($"Layer Size: {bl?.Layer?.Count ?? 0}", false).EndIf()
                    .AddItem("Open Selected Item in Blast Editor", (ob, ev) =>
                    {
                        if (S.GET<BlastEditorForm>() != null)
                        {
                            StashKey sk = StockpileManagerUISide.StashHistory[lbStashHistory.SelectedIndex];
                            BlastEditorForm.OpenBlastEditor((StashKey)sk.Clone());
                        }
                    }, selectionExists)
                    .AddItem("Sanitize", (ob, ev) =>
                    {
                        if (S.GET<BlastEditorForm>() != null)
                        {
                            StashKey sk = StockpileManagerUISide.StashHistory[lbStashHistory.SelectedIndex];
                            SanitizeToolForm.OpenSanitizeTool((StashKey)sk.Clone(), false);
                        }
                    }, selectionExists)
                    .AddSeparator()
                    .AddItem("Rename Selected Item", (ob, ev) =>
                    {
                        StashKey sk = StockpileManagerUISide.StashHistory[lbStashHistory.SelectedIndex];
                        StockpileManagerForm.RenameStashKey(sk);
                        RefreshStashHistory();
                    }, selectionExists)
                    .AddItem("Generate VMD from Selected Item", (ob, ev) =>
                    {
                        StashKey sk = StockpileManagerUISide.StashHistory[lbStashHistory.SelectedIndex];
                        sk.BlastLayer.RasterizeVMDs();
                        MemoryDomains.GenerateVmdFromStashkey(sk);
                        S.GET<VmdPoolForm>().RefreshVMDs();
                    }, selectionExists)
                    .AddSeparator()
                    .AddItem("Merge Selected Stashkeys", async (ob, ev) =>
                    {
                        List<StashKey> sks = new List<StashKey>();
                        foreach (StashKey sk in lbStashHistory.SelectedItems)
                        {
                            sks.Add(sk);
                        }

                        await StockpileManagerUISide.MergeStashkeys(sks);

                        RefreshStashHistorySelectLast();
                    }, selectionExists && lbStashHistory.SelectedItems.Count > 1)
                    /*
                    .If(!RTC_NetcoreImplementation.isStandaloneUI).AddSeparator()
                    .If(!RTC_NetcoreImplementation.isStandaloneUI).AddItem("[Multiplayer] Pull State From Peer", (ob, ev) =>
                    {
                        S.GET<RTC_Multiplayer_Form>().cbPullStateToGlitchHarvester.Checked = true;
                        RTC_NetcoreImplementation.Multiplayer.SendCommand(new RTC_Command(CommandType.PULLSTATE), false);
                    }, RTC_NetcoreImplementation.Multiplayer != null && RTC_NetcoreImplementation.Multiplayer.side != NetworkSide.DISCONNECTED)
                    */
                    .Build()
                    .Show(this, locate);
            }
        }

        public void RefreshStashHistorySelectLast()
        {
            RefreshStashHistory();
            DontLoadSelectedStash = true;
            lbStashHistory.ClearSelected();
            DontLoadSelectedStash = true;
            lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
        }

        public void HandleStashHistorySelectionChange(object sender, EventArgs e)
        {
            try
            {
                lbStashHistory.Enabled = false;
                btnStashUP.Enabled = false;
                btnStashDOWN.Enabled = false;
                btnAddStashToStockpile.Enabled = false;

                if (DontLoadSelectedStash || lbStashHistory.SelectedIndex == -1)
                {
                    DontLoadSelectedStash = false;
                    return;
                }

                S.GET<StockpileManagerForm>().dgvStockpile.ClearSelection();
                S.GET<StockpilePlayerForm>().dgvStockpile.ClearSelection();

                var blastForm = S.GET<GlitchHarvesterBlastForm>();

                if (S.GET<GlitchHarvesterBlastForm>().MergeMode)
                {
                    blastForm.ghMode = GlitchHarvesterMode.CORRUPT;
                    S.GET<StockpileManagerForm>().btnRenameSelected.Visible = true;
                    S.GET<StockpileManagerForm>().btnRemoveSelectedStockpile.Text = "  Remove Item";

                    if (blastForm.ghMode == GlitchHarvesterMode.CORRUPT)
                    {
                        blastForm.btnCorrupt.Text = "  Corrupt";
                    }
                    else if (blastForm.ghMode == GlitchHarvesterMode.INJECT)
                    {
                        blastForm.btnCorrupt.Text = "  Inject";
                    }
                    else if (blastForm.ghMode == GlitchHarvesterMode.ORIGINAL)
                    {
                        blastForm.btnCorrupt.Text = "  Original";
                    }
                }

                StockpileManagerUISide.CurrentStashkey = StockpileManagerUISide.StashHistory[lbStashHistory.SelectedIndex];

                if (!blastForm.LoadOnSelect)
                {
                    return;
                }

                blastForm.OneTimeExecute();
            }
            finally
            {
                lbStashHistory.Enabled = true;
                btnStashUP.Enabled = true;
                btnStashDOWN.Enabled = true;
                btnAddStashToStockpile.Enabled = true;
                //((Control)sender).Focus();
                S.GET<GlitchHarvesterBlastForm>().RedrawActionUI();
            }
        }

        private void ClearSelectedSKs(object sender, MouseEventArgs e)
        {
            DontLoadSelectedStash = true;
            lbStashHistory.ClearSelected();
            DontLoadSelectedStash = true;
            S.GET<StockpileManagerForm>().dgvStockpile.ClearSelection();
            S.GET<GlitchHarvesterBlastForm>().RedrawActionUI();
        }

        private void ClearStashHistory(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear the stash?", "Clear stash?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                StockpileManagerUISide.StashHistory.Clear();
                this.RefreshStashHistory();

                //Force clean up
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void MoveSelectedStashUp(object sender, EventArgs e)
        {
            if (this.lbStashHistory.SelectedIndex == -1)
            {
                return;
            }

            if (this.lbStashHistory.SelectedIndex == 0)
            {
                this.lbStashHistory.ClearSelected();
                if (!this.LoadWhenSelectedWithArrows)
                {
                    this.DontLoadSelectedStash = true;
                }

                this.lbStashHistory.SelectedIndex = this.lbStashHistory.Items.Count - 1;
            }
            else
            {
                int newPos = this.lbStashHistory.SelectedIndex - 1;
                this.lbStashHistory.ClearSelected();
                if (!this.LoadWhenSelectedWithArrows)
                {
                    this.DontLoadSelectedStash = true;
                }

                this.lbStashHistory.SelectedIndex = newPos;
            }
        }

        public void MoveSelectedStashDown(object sender, EventArgs e)
        {
            if (this.lbStashHistory.SelectedIndex == -1)
            {
                return;
            }

            if (this.lbStashHistory.SelectedIndex == this.lbStashHistory.Items.Count - 1)
            {
                this.lbStashHistory.ClearSelected();
                if (!this.LoadWhenSelectedWithArrows)
                {
                    this.DontLoadSelectedStash = true;
                }

                this.lbStashHistory.SelectedIndex = 0;
            }
            else
            {
                int newPos = this.lbStashHistory.SelectedIndex + 1;
                this.lbStashHistory.ClearSelected();
                if (!this.LoadWhenSelectedWithArrows)
                {
                    this.DontLoadSelectedStash = true;
                }

                this.lbStashHistory.SelectedIndex = newPos;
            }
        }
    }
}
