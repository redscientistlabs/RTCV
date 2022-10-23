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

    public partial class StashHistoryForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public bool DontLoadSelectedStash { get; set; } = false;

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

        public bool AddStashToStockpile(bool askForName = true)
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
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                StockpileManagerUISide.CurrentStashkey.Alias = StockpileManagerUISide.CurrentStashkey.Key;
            }
            else
            {
                StockpileManagerUISide.CurrentStashkey.Alias = Name;
            }

            sk.BlastLayer.RasterizeVMDs();

            DataGridViewRow dataRow = S.GET<StockpileManagerForm>().dgvStockpile.Rows[S.GET<StockpileManagerForm>().dgvStockpile.Rows.Add()];
            dataRow.Cells["Item"].Value = sk;
            dataRow.Cells["GameName"].Value = sk.GameName;
            dataRow.Cells["SystemName"].Value = sk.SystemName;
            dataRow.Cells["SystemCore"].Value = sk.SystemCore;

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

                ContextMenuStrip columnsMenu = new ContextMenuStrip();

                BlastLayer bl = null;

                if(lbStashHistory.SelectedIndex != -1)
                    bl = StockpileManagerUISide.StashHistory[lbStashHistory.SelectedIndex].BlastLayer;

                if(bl != null)
                    columnsMenu.Items.Add($"Layer Size: {bl.Layer?.Count ?? 0}", null).Enabled = false;

                ((ToolStripMenuItem)columnsMenu.Items.Add("Open Selected Item in Blast Editor", null, new EventHandler((ob, ev) =>
                {
                    if (S.GET<BlastEditorForm>() != null)
                    {
                        StashKey sk = StockpileManagerUISide.StashHistory[lbStashHistory.SelectedIndex];
                        BlastEditorForm.OpenBlastEditor(sk);
                    }
                }))).Enabled = lbStashHistory.SelectedIndex != -1;

                ((ToolStripMenuItem)columnsMenu.Items.Add("Sanitize", null, new EventHandler((ob, ev) =>
                {
                    if (S.GET<BlastEditorForm>() != null)
                    {
                        StashKey sk = StockpileManagerUISide.StashHistory[lbStashHistory.SelectedIndex];
                        SanitizeToolForm.OpenSanitizeTool(sk, false);
                    }
                }))).Enabled = lbStashHistory.SelectedIndex != -1;

                columnsMenu.Items.Add(new ToolStripSeparator());

                ((ToolStripMenuItem)columnsMenu.Items.Add("Rename selected item", null, new EventHandler((ob, ev) =>
                {
                    StashKey sk = StockpileManagerUISide.StashHistory[lbStashHistory.SelectedIndex];
                    StockpileManagerForm.RenameStashKey(sk);
                    RefreshStashHistory();
                }))).Enabled = lbStashHistory.SelectedIndex != -1;

                ((ToolStripMenuItem)columnsMenu.Items.Add("Generate VMD from Selected Item", null, new EventHandler((ob, ev) =>
                {
                    StashKey sk = StockpileManagerUISide.StashHistory[lbStashHistory.SelectedIndex];
                    sk.BlastLayer.RasterizeVMDs();
                    MemoryDomains.GenerateVmdFromStashkey(sk);
                    S.GET<VmdPoolForm>().RefreshVMDs();
                }))).Enabled = lbStashHistory.SelectedIndex != -1;

                columnsMenu.Items.Add(new ToolStripSeparator());

                ((ToolStripMenuItem)columnsMenu.Items.Add("Merge Selected Stashkeys", null, new EventHandler((ob, ev) =>
                {
                    List<StashKey> sks = new List<StashKey>();
                    foreach (StashKey sk in lbStashHistory.SelectedItems)
                    {
                        sks.Add(sk);
                    }

                    StockpileManagerUISide.MergeStashkeys(sks);

                    RefreshStashHistory();
                }))).Enabled = (lbStashHistory.SelectedIndex != -1 && lbStashHistory.SelectedItems.Count > 1);

                /*
                if (!RTC_NetcoreImplementation.isStandaloneUI)
                {
                    columnsMenu.Items.Add(new ToolStripSeparator());
                    ((ToolStripMenuItem)columnsMenu.Items.Add("[Multiplayer] Pull State from peer", null, new EventHandler((ob, ev) =>
                        {
                            S.GET<RTC_Multiplayer_Form>().cbPullStateToGlitchHarvester.Checked = true;
                            RTC_NetcoreImplementation.Multiplayer.SendCommand(new RTC_Command(CommandType.PULLSTATE), false);
                        }))).Enabled = RTC_NetcoreImplementation.Multiplayer != null && RTC_NetcoreImplementation.Multiplayer.side != NetworkSide.DISCONNECTED;
                }*/

                columnsMenu.Show(this, locate);
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
            StockpileManagerUISide.StashHistory.Clear();
            RefreshStashHistory();

            //Force clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void MoveSelectedStashUp(object sender, EventArgs e)
        {
            if (lbStashHistory.SelectedIndex == -1)
            {
                return;
            }

            if (lbStashHistory.SelectedIndex == 0)
            {
                lbStashHistory.ClearSelected();
                lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
            }
            else
            {
                int newPos = lbStashHistory.SelectedIndex - 1;
                lbStashHistory.ClearSelected();
                lbStashHistory.SelectedIndex = newPos;
            }
        }

        public void MoveSelectedStashDown(object sender, EventArgs e)
        {
            if (lbStashHistory.SelectedIndex == -1)
            {
                return;
            }

            if (lbStashHistory.SelectedIndex == lbStashHistory.Items.Count - 1)
            {
                lbStashHistory.ClearSelected();
                lbStashHistory.SelectedIndex = 0;
            }
            else
            {
                int newPos = lbStashHistory.SelectedIndex + 1;
                lbStashHistory.ClearSelected();
                lbStashHistory.SelectedIndex = newPos;
            }
        }
    }
}
