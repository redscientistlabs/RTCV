using RTCV.NetCore;

namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using RTCV.UI.Modular;
    using SlimDX.DirectInput;

    public partial class StockpileManagerForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        private Color? originalSaveButtonColor;
        private bool _UnsavedEdits;
        private bool _loadEntryWhenSelectedWithArrows = Params.IsParamSet("LOAD_STOCKPILE_ENTRY_ON_ARROW_CLICK");
        public bool UnsavedEdits
        {
            get => _UnsavedEdits;
            set
            {
                _UnsavedEdits = value;

                if (_UnsavedEdits)
                {
                    if (btnSaveStockpile.Enabled)
                    {
                        btnSaveStockpile.BackColor = Color.Tomato;
                    }
                    else
                    {
                        btnSaveStockpileAs.BackColor = Color.Tomato;
                    }
                }
                else
                {
                    const float light1 = 0.10f;
                    const float generalDarken = -0.50f;
                    Color c = Colors.GeneralColor.ChangeColorBrightness(generalDarken).ChangeColorBrightness(light1);
                    btnSaveStockpile.BackColor = c;
                    btnSaveStockpile.FlatAppearance.BorderColor = c;
                    btnSaveStockpileAs.BackColor = c;
                    btnSaveStockpileAs.FlatAppearance.BorderColor = c;
                }
            }
        }

        public StockpileManagerForm()
        {
            InitializeComponent();

            popoutAllowed = true;
            undockedSizable = true;

            dgvStockpile.RowsAdded += (o, e) =>
            {
                RefreshNoteIcons();
            };
            btnSaveStockpile.BackColorChanged += (o, e) => UnsavedEdits = UnsavedEdits; //this is pretty cursed but it works
            btnSaveStockpileAs.BackColorChanged += (o, e) => UnsavedEdits = UnsavedEdits;
        }

        public void HandleCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e == null || e.RowIndex == -1)
            {
                return;
            }

            try
            {
                S.GET<StashHistoryForm>().btnAddStashToStockpile.Enabled = false;
                dgvStockpile.Enabled = false;
                btnStockpileUP.Enabled = false;
                btnStockpileDOWN.Enabled = false;

                // Stockpile Note handling
                var senderGrid = (DataGridView)sender;

                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                    e.RowIndex >= 0)
                {
                    StashKey sk = (StashKey)senderGrid.Rows[e.RowIndex].Cells["Item"].Value;
                    S.SET(new NoteEditorForm(sk, senderGrid.Rows[e.RowIndex].Cells["Note"]));
                    S.GET<NoteEditorForm>().Show();

                    return;
                }

                S.GET<StashHistoryForm>().lbStashHistory.ClearSelected();
                S.GET<StockpilePlayerForm>().dgvStockpile.ClearSelection();

                S.GET<GlitchHarvesterBlastForm>().RedrawActionUI();

                if (dgvStockpile.SelectedRows.Count == 0)
                {
                    return;
                }

                StockpileManagerUISide.CurrentStashkey = GetSelectedStashKey();

                List<StashKey> keys = dgvStockpile.Rows.Cast<DataGridViewRow>().Select(x => (StashKey)x.Cells[0].Value).ToList();
                if (!StockpileManagerUISide.CheckAndFixMissingReference(StockpileManagerUISide.CurrentStashkey, false, keys))
                {
                    return;
                }

                if (!S.GET<GlitchHarvesterBlastForm>().LoadOnSelect)
                {
                    return;
                }

                // Merge Execution
                if (dgvStockpile.SelectedRows.Count > 1)
                {
                    List<StashKey> sks = new List<StashKey>();

                    foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
                    {
                        sks.Add((StashKey)row.Cells[0].Value);
                    }

                    //Removing this check makes Merge behave properly in all cases:
                    //Shift+select uses the topmost savestate of the selection
                    //Ctrl+select uses the savestate from the first item that was selected
                    //Using the 'Merge' button follows the rules above to determine which savestate to use

                    //if (IsControlDown())
                    //{
                    //    sks.Reverse();
                    //}

                    sks.Reverse();

                    StockpileManagerUISide.MergeStashkeys(sks);

                    if (Render.RenderAtLoad && S.GET<GlitchHarvesterBlastForm>().loadBeforeOperation)
                    {
                        Render.StartRender();
                    }

                    S.GET<StashHistoryForm>().RefreshStashHistory();
                    return;
                }

                S.GET<GlitchHarvesterBlastForm>().OneTimeExecute();
            }
            finally
            {
                logger.Trace("Stockpile Manager load done, unlocking UI");
                dgvStockpile.Enabled = true;
                btnStockpileUP.Enabled = true;
                btnStockpileDOWN.Enabled = true;
                S.GET<StashHistoryForm>().btnAddStashToStockpile.Enabled = true;
            }

            S.GET<GlitchHarvesterBlastForm>().RedrawActionUI();
        }
        private static bool IsControlDown()
        {
            return (ModifierKeys & Keys.Control) != 0;
        }

        private void HandleStockpileMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

                ContextMenuStrip columnsMenu = new ContextMenuStrip();



                BlastLayer bl = null;

                if (dgvStockpile.SelectedRows.Count == 1)
                    bl = GetSelectedStashKey().BlastLayer;

                if (bl != null)
                    columnsMenu.Items.Add($"Layer Size: {bl.Layer?.Count ?? 0}", null).Enabled = false;


                ((ToolStripMenuItem)columnsMenu.Items.Add("Open Selected Item in Blast Editor", null, new EventHandler((ob, ev) =>
                {
                    if (S.GET<BlastEditorForm>() != null)
                    {
                        var sk = GetSelectedStashKey();
                        BlastEditorForm.OpenBlastEditor((StashKey)sk.Clone());
                    }
                }))).Enabled = (dgvStockpile.SelectedRows.Count == 1);

                ((ToolStripMenuItem)columnsMenu.Items.Add("Sanitize", null, new EventHandler((ob, ev) =>
                {
                    if (S.GET<BlastEditorForm>() != null)
                    {
                        var sk = GetSelectedStashKey();
                        SanitizeToolForm.OpenSanitizeTool((StashKey)sk.Clone(),false);
                    }
                }))).Enabled = (dgvStockpile.SelectedRows.Count == 1);

                columnsMenu.Items.Add(new ToolStripSeparator());

                ((ToolStripMenuItem)columnsMenu.Items.Add("Manual Inject", null, new EventHandler((ob, ev) =>
                {
                    var sk = GetSelectedStashKey();
                    StashKey newSk = (StashKey)sk.Clone();

                    bool IsCorrupted = StockpileManagerUISide.ApplyStashkey(newSk, false, false);

                    if (StockpileManagerUISide.CurrentStashkey != null)
                        S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = IsCorrupted;
                }))).Enabled = (dgvStockpile.SelectedRows.Count == 1);

                columnsMenu.Items.Add(new ToolStripSeparator());

                ((ToolStripMenuItem)columnsMenu.Items.Add("Rename selected item", null, new EventHandler((ob, ev) =>
                {
                    if (dgvStockpile.SelectedRows.Count != 0)
                    {
                        if (RenameStashKey(GetSelectedStashKey()))
                        {
                            StockpileManagerUISide.StockpileChanged();
                            dgvStockpile.Refresh();
                            UnsavedEdits = true;
                        }

                        //lbStockpile.RefreshItemsReal();   
                    }
                }))).Enabled = (dgvStockpile.SelectedRows.Count == 1);

                ((ToolStripMenuItem)columnsMenu.Items.Add("Generate VMD from Selected Item", null, new EventHandler((ob, ev) =>
                {
                    var sk = GetSelectedStashKey();
                    MemoryDomains.GenerateVmdFromStashkey(sk);
                    S.GET<VmdPoolForm>().RefreshVMDs();
                }))).Enabled = (dgvStockpile.SelectedRows.Count == 1);

                ((ToolStripMenuItem)columnsMenu.Items.Add("Merge Selected Stashkeys", null, new EventHandler((ob, ev) =>
                {
                    List<StashKey> sks = new List<StashKey>();
                    foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
                    {
                        sks.Add((StashKey)row.Cells[0].Value);
                    }

                    StockpileManagerUISide.MergeStashkeys(sks);
                    S.GET<StashHistoryForm>().RefreshStashHistory();
                }))).Enabled = (dgvStockpile.SelectedRows.Count > 1);




                ((ToolStripMenuItem)columnsMenu.Items.Add("Replace associated ROM", null, new EventHandler((ob, ev) =>
                {
                    List<StashKey> sks = new List<StashKey>();
                    foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
                    {
                        sks.Add((StashKey)row.Cells[0].Value);
                    }

                    OpenFileDialog ofd = new OpenFileDialog
                    {
                        DefaultExt = "*",
                        Title = "Select Replacement File",
                        Filter = "Any file|*.*",
                        RestoreDirectory = true
                    };
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string filename = ofd.FileName;
                        string oldFilename = sks.First().RomFilename;
                        foreach (var sk in sks.Where(x => x.RomFilename == oldFilename))
                        {
                            sk.RomFilename = filename;
                            sk.RomShortFilename = Path.GetFileName(sk.RomFilename);
                        }
                    }
                }))).Enabled = (dgvStockpile.SelectedRows.Count >= 1);

                columnsMenu.Items.Add(new ToolStripSeparator());

                ((ToolStripMenuItem)columnsMenu.Items.Add($"Duplicate selected item{(dgvStockpile.SelectedRows.Count > 1 ? "s" : "")}", null, new EventHandler((ob, ev) =>
                {
                    DuplicateSelected();
                }))).Enabled = (dgvStockpile.SelectedRows.Count > 0);

                ((ToolStripMenuItem)columnsMenu.Items.Add($"Remove selected item{(dgvStockpile.SelectedRows.Count > 1 ? "s" : "")}", null, new EventHandler((ob, ev) =>
                {
                        RemoveSelected();
                    
                }))).Enabled = (dgvStockpile.SelectedRows.Count > 0);

                columnsMenu.Show(this, locate);
            }
        }

        public void RefreshNoteIcons()
        {
            foreach (DataGridViewRow dataRow in dgvStockpile.Rows)
            {
                StashKey sk = (StashKey)dataRow.Cells["Item"].Value;
                if (sk == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(sk.Note))
                {
                    dataRow.Cells["Note"].Value = "";
                }
                else
                {
                    dataRow.Cells["Note"].Value = "📝";
                }
            }
        }

        internal static bool RenameStashKey(StashKey sk)
        {
            string value = sk.Alias;

            if (value == null)
                value = "";

            if (RTCV.UI.Forms.InputBox.ShowDialog("Renaming Stashkey", "Enter the new Stash name:", ref value) == DialogResult.OK && !string.IsNullOrWhiteSpace(value))
            {
                sk.Alias = value.Trim();
                return true;
            }

            return false;
        }


        private void RenamedSelected(object sender, EventArgs e)
        {
            if (!btnRenameSelected.Visible)
            {
                return;
            }

            if (dgvStockpile.SelectedRows.Count != 0)
            {
                if (RenameStashKey(GetSelectedStashKey()))
                {
                    StockpileManagerUISide.StockpileChanged();
                    dgvStockpile.Refresh();
                    UnsavedEdits = true;
                }

                //lbStockpile.RefreshItemsReal();
            }
        }

        private void RemoveSelectedStockpile(object sender, EventArgs e) => RemoveSelected();
        public void RemoveSelected()
        {
            if (ModifierKeys == Keys.Control || (dgvStockpile.SelectedRows.Count != 0 && (MessageBox.Show("Are you sure you want to remove the selected stockpile entries?", "Delete Stockpile Entry?", MessageBoxButtons.YesNo) == DialogResult.Yes)))
            {
                foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
                {
                    dgvStockpile.Rows.Remove(row);
                }
                StockpileManagerUISide.StockpileChanged();
                UnsavedEdits = true;
                S.GET<GlitchHarvesterBlastForm>().RedrawActionUI();
            }
        }
        public void DuplicateSelected()
        {
            List<StashKey> sks = new List<StashKey>();
            foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
            {
                sks.Add((StashKey)((StashKey)row.Cells[0].Value).Clone());
                sks.Last().Alias = (row.Cells[0].Value as StashKey)?.Alias ?? sks.Last().Alias;
            }
            foreach (var sk in sks)
            {
                StockpileManagerUISide.StashHistory.Add(sk);

                S.GET<StashHistoryForm>().RefreshStashHistory();
                S.GET<StockpileManagerForm>().dgvStockpile.ClearSelection();
                S.GET<StashHistoryForm>().lbStashHistory.ClearSelected();

                S.GET<StashHistoryForm>().DontLoadSelectedStash = true;
                S.GET<StashHistoryForm>().lbStashHistory.SelectedIndex = S.GET<StashHistoryForm>().lbStashHistory.Items.Count - 1;
                StockpileManagerUISide.CurrentStashkey = StockpileManagerUISide.StashHistory[S.GET<StashHistoryForm>().lbStashHistory.SelectedIndex];

                S.GET<StashHistoryForm>().AddStashToStockpile(false, sk.Alias);

            }
            StockpileManagerUISide.StockpileChanged();
            UnsavedEdits = true;
        }
        private void ClearStockpile(object sender, EventArgs e) => ClearStockpile();
        public void ClearStockpile(bool force = false)
        {
            if (force || MessageBox.Show("Are you sure you want to clear the stockpile?", "Clearing stockpile", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dgvStockpile.Rows.Clear();

                StockpileManagerUISide.ClearCurrentStockpile();

                btnSaveStockpile.Enabled = false;
                UnsavedEdits = false;

                S.GET<GlitchHarvesterBlastForm>().RedrawActionUI();
            }
        }

        public async void LoadStockpile(string filename)
        {
            logger.Trace("Entered LoadStockpile {0}", Thread.CurrentThread.ManagedThreadId);
            if (UnsavedEdits && MessageBox.Show("You have unsaved edits in the Glitch Harvester Stockpile. \n\n Are you sure you want to load without saving?",
                "Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            var ghForm = CanvasForm.GetExtraForm("Glitch Harvester");
            try
            {
                //We do this here and invoke because our unlock runs at the end of the awaited method, but there's a chance an error occurs
                //Thus, we want this to happen within the try block
                UICore.SetHotkeyTimer(false);

                logger.Trace("Blocking UI");
                UICore.LockInterface(false, true);
                logger.Trace("UI Blocked");

                logger.Trace("Opening SaveProgress Form");
                S.GET<SaveProgressForm>().Dock = DockStyle.Fill;
                ghForm?.OpenSubForm(S.GET<SaveProgressForm>());

                logger.Trace("Clearing Current Stockpile");
                StockpileManagerUISide.ClearCurrentStockpile();
                dgvStockpile.Rows.Clear();

                S.GET<StockpilePlayerForm>().dgvStockpile.Rows.Clear();
                logger.Trace("Starting Load Task");
                var r = await Task.Run(() => Stockpile.Load(filename));
                logger.Trace("Load Task Done");
                if (r.Failed)
                {
                    logger.Trace("Load Task Failed");
                    MessageBox.Show($"Loading the stockpile failed!\n" +
                                    $"{r.GetErrorsFormatted()}");
                }
                else
                {
                    logger.Trace("Load Task Success");
                    var sks = r.Result;
                    //Update the current stockpile to this one
                    StockpileManagerUISide.SetCurrentStockpile(sks);

                    logger.Trace("Populating DGV");
                    foreach (StashKey key in sks.StashKeys)
                    {
                        dgvStockpile?.Rows.Add(key, key.GameName, key.SystemName, key.SystemCore, key.Note);
                    }

                    btnSaveStockpile.Enabled = true;
                    RefreshNoteIcons();

                    if (r.HasWarnings())
                    {
                        MessageBox.Show($"The stockpile gave the following warnings:\n" +
                                        $"{r.GetWarningsFormatted()}");
                    }
                }

                dgvStockpile.ClearSelection();
                StockpileManagerUISide.StockpileChanged();

                UnsavedEdits = false;
            }
            finally
            {
                logger.Trace("Closing Save form");
                ghForm?.CloseSubForm();
                UICore.SetHotkeyTimer(true);
                logger.Trace("Unlocking Interface");
                UICore.UnlockInterface();
                logger.Trace("Load done");
            }
        }

        internal StashKey GetSelectedStashKey() => (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);

        private async void ImportStockpile(string filename)
        {
            var ghForm = CanvasForm.GetExtraForm("Glitch Harvester");
            try
            {
                UICore.SetHotkeyTimer(false);
                UICore.LockInterface(false, true);
                S.GET<SaveProgressForm>().Dock = DockStyle.Fill;
                ghForm?.OpenSubForm(S.GET<SaveProgressForm>());

                var r = await Task.Run(() => Stockpile.Import(filename));

                if (!r.Failed)
                {
                    var sks = r.Result;
                    //Todo - Refactor this to get it out of the object
                    //Populate the dgv
                    RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Populating UI", 95));

                    foreach (StashKey key in sks.StashKeys)
                    {
                        dgvStockpile?.Rows.Add(key, key.GameName, key.SystemName, key.SystemCore, key.Note);
                    }

                    UnsavedEdits = true;

                    RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Done", 100));
                }
            }
            finally
            {
                ghForm?.CloseSubForm();
                UICore.UnlockInterface();
                UICore.SetHotkeyTimer(true);
                RefreshNoteIcons();
            }
        }

        private async void SaveStockpile(Stockpile sks, string path)
        {
            logger.Trace("Entering SaveStockpile {0}\n{1}", Thread.CurrentThread.ManagedThreadId, Environment.StackTrace);
            var ghForm = CanvasForm.GetExtraForm("Glitch Harvester");
            try
            {
                //We do this here and invoke because our unlock runs at the end of the awaited method, but there's a chance an error occurs
                //Thus, we want this to happen within the try block
                UICore.SetHotkeyTimer(false);
                UICore.LockInterface(false, true);
                S.GET<SaveProgressForm>().Dock = DockStyle.Fill;
                ghForm?.OpenSubForm(S.GET<SaveProgressForm>());

                var r = await Task.Run(() => Stockpile.Save(sks, path, NetCore.Params.IsParamSet("INCLUDE_REFERENCED_FILES"), NetCore.Params.IsParamSet("COMPRESS_STOCKPILE")));

                if (r)
                {
                    StockpileManagerUISide.SetCurrentStockpile(sks);
                    sendCurrentStockpileToSKS();
                    UnsavedEdits = false;
                    btnSaveStockpile.Enabled = true;
                }
            }
            finally
            {
                ghForm?.CloseSubForm();
                UICore.UnlockInterface();
                UICore.SetHotkeyTimer(true);
            }
        }

        private void LoadStockpile(object sender, MouseEventArgs e)
        {
            logger.Trace("Entering LoadStockpile {0}", Thread.CurrentThread.ManagedThreadId);
            //RtcCore.CheckForProblematicProcesses();

            Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);

            ContextMenuStrip loadMenuItems = new ContextMenuStrip();
            loadMenuItems.Items.Add("Load Stockpile", null, new EventHandler((ob, ev) =>
            {
                string filename = "";
                OpenFileDialog ofd = new OpenFileDialog
                {
                    DefaultExt = "sks",
                    Title = "Open Stockpile File",
                    Filter = "SKS files|*.sks",
                    RestoreDirectory = true
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    filename = ofd.FileName;
                }
                else
                {
                    return;
                }

                LoadStockpile(filename);
            }));

            loadMenuItems.Items.Add($"Load {RtcCore.VanguardImplementationName} settings from Stockpile", null, new EventHandler((ob, ev) =>
            {
                try
                {
                    if (UnsavedEdits && MessageBox.Show($"You have unsaved edits in the Glitch Harvester Stockpile. \n\n This will restart {RtcCore.VanguardImplementationName}. Are you sure you want to load without saving?",
                        "Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    AutoKillSwitch.Enabled = false;
                    Stockpile.LoadConfigFromStockpile();
                    AutoKillSwitch.Enabled = true;
                }
                finally
                {
                }
            }));

            loadMenuItems.Items.Add($"Restore {RtcCore.VanguardImplementationName} config Backup", null, new EventHandler((ob, ev) =>
            {
                try
                {
                    if (UnsavedEdits && MessageBox.Show(
                        $"You have unsaved edits in the Glitch Harvester Stockpile. \n\n This will restart {RtcCore.VanguardImplementationName}. Are you sure you want to load without saving?",
                        "Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }

                    AutoKillSwitch.Enabled = false;
                    Stockpile.RestoreEmuConfig();
                    AutoKillSwitch.Enabled = true;
                }
                finally
                {
                }
            })).Enabled = (File.Exists(Path.Combine(RtcCore.EmuDir, "backup_config.ini")));

            loadMenuItems.Show(this, locate);
        }

        public void SaveStockpileAs(object sender, EventArgs e)
        {
            if (dgvStockpile.Rows.Count == 0)
            {
                MessageBox.Show("You cannot save the Stockpile because it is empty");
                return;
            }

            UICore.SetHotkeyTimer(false);
            string path = "";
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                DefaultExt = "sks",
                Title = "Save Stockpile File",
                Filter = "SKS files|*.sks",
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog1.FileName;
            }
            else
            {
                return;
            }

            Stockpile sks = new Stockpile(dgvStockpile);
            SaveStockpile(sks, path);
        }

        private void SaveStockpile(object sender, EventArgs e)
        {
            Stockpile sks = new Stockpile(dgvStockpile);
            SaveStockpile(sks, StockpileManagerUISide.GetCurrentStockpilePath());
        }

        private void sendCurrentStockpileToSKS()
        {
            foreach (DataGridViewRow dataRow in dgvStockpile.Rows)
            {
                StashKey sk = (StashKey)dataRow.Cells["Item"].Value;
            }
        }

        private void MoveSelectedStockpileUp(object sender, EventArgs e)
        {
            var selectedRows = dgvStockpile.SelectedRows.Cast<DataGridViewRow>().ToArray();
            foreach (DataGridViewRow row in selectedRows)
            {
                int pos = row.Index;
                dgvStockpile.Rows.RemoveAt(pos);

                if (pos == 0)
                {
                    dgvStockpile.Rows.Add(row);
                }
                else
                {
                    int newpos = pos - 1;
                    dgvStockpile.Rows.Insert(newpos, row);
                }
            }
            dgvStockpile.ClearSelection();
            foreach (DataGridViewRow row in selectedRows) //I don't know. Blame DGV
            {
                row.Selected = true;
            }

            UnsavedEdits = true;

            StockpileManagerUISide.StockpileChanged();
            S.GET<GlitchHarvesterBlastForm>().RedrawActionUI();
        }

        private void MoveSelectedStockpileDown(object sender, EventArgs e)
        {
            var selectedRows = dgvStockpile.SelectedRows.Cast<DataGridViewRow>().ToArray();
            foreach (DataGridViewRow row in selectedRows)
            {
                int pos = row.Index;
                int count = dgvStockpile.Rows.Count;
                dgvStockpile.Rows.RemoveAt(pos);

                if (pos == count - 1)
                {
                    int newpos = 0;
                    dgvStockpile.Rows.Insert(newpos, row);
                }
                else
                {
                    int newpos = pos + 1;
                    dgvStockpile.Rows.Insert(newpos, row);
                }
            }
            dgvStockpile.ClearSelection();
            foreach (DataGridViewRow row in selectedRows) //I don't know. Blame DGV
            {
                row.Selected = true;
            }

            UnsavedEdits = true;

            StockpileManagerUISide.StockpileChanged();
            S.GET<GlitchHarvesterBlastForm>().RedrawActionUI();
        }

        private void HandleDragDrop(object sender, DragEventArgs e)
        {
            bool alreadyLoadedAStockpile = false;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (files == null)
                return;

            foreach (var f in files)
            {
                if (f.Contains(".bl"))
                {
                    BlastLayer temp = BlastTools.LoadBlastLayerFromFile(f);
                    StockpileManagerUISide.Import(temp);
                    S.GET<StashHistoryForm>().RefreshStashHistorySelectLast();
                    S.GET<StashHistoryForm>().AddStashToStockpile(false,f);
                }
                else if (f.Contains(".sks"))
                {
                    if (!alreadyLoadedAStockpile)
                    {
                        LoadStockpile(f);
                        alreadyLoadedAStockpile = true;
                    }
                    else
                    {
                        ImportStockpile(f);
                    }
                }
            }


            //Bring the UI back to normal after a drag+drop to prevent weird merge stuff
            S.GET<GlitchHarvesterBlastForm>().RedrawActionUI();
        }

        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void ImportStockpile(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "*",
                Title = "Select stockpile to import",
                Filter = "Any file|*.sks",
                RestoreDirectory = true
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ImportStockpile(ofd.FileName);
            }
        }

        private void StockpileUp(object sender, EventArgs e)
        {
            if (dgvStockpile.SelectedRows.Count == 0)
            {
                return;
            }

            int currentSelectedIndex = dgvStockpile.SelectedRows[0].Index;

            if (currentSelectedIndex == 0)
            {
                dgvStockpile.ClearSelection();
                dgvStockpile.Rows[dgvStockpile.Rows.Count - 1].Selected = true;
            }
            else
            {
                dgvStockpile.ClearSelection();
                dgvStockpile.Rows[currentSelectedIndex - 1].Selected = true;
            }

            if (_loadEntryWhenSelectedWithArrows)
            {
                HandleCellClick(dgvStockpile, new DataGridViewCellEventArgs(0, dgvStockpile.SelectedRows[0].Index));
            }
        }

        private void StockpileDown(object sender, EventArgs e)
        {
            if (dgvStockpile.SelectedRows.Count == 0)
            {
                return;
            }

            int currentSelectedIndex = dgvStockpile.SelectedRows[0].Index;

            if (currentSelectedIndex == dgvStockpile.Rows.Count - 1)
            {
                dgvStockpile.ClearSelection();
                dgvStockpile.Rows[0].Selected = true;
            }
            else
            {
                dgvStockpile.ClearSelection();
                dgvStockpile.Rows[currentSelectedIndex + 1].Selected = true;
            }

            if (_loadEntryWhenSelectedWithArrows)
            {
                HandleCellClick(dgvStockpile, new DataGridViewCellEventArgs(0, dgvStockpile.SelectedRows[0].Index));
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            dgvStockpile.AllowDrop = true;
            dgvStockpile.DragDrop += HandleDragDrop;
            dgvStockpile.DragEnter += HandleDragEnter;
        }

        private void HandleGlitchHarvesterSettingsMouseDown(object sender, MouseEventArgs e)
        {
            Point locate = e.GetMouseLocation(sender);
            ContextMenuStrip ghSettingsMenu = new ContextMenuStrip();

            ghSettingsMenu.Items.Add(new ToolStripLabel("Stockpile Manager settings")
            {
                Font = new Font("Segoe UI", 12)
            });

            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Stockpile items: " + dgvStockpile.Rows.Cast<DataGridViewRow>().Count().ToString() , null, new EventHandler((ob, ev) =>
            {

            }))).Enabled = false;

            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Compress Stockpiles", null, new EventHandler((ob, ev) =>
            {
                if (Params.IsParamSet("COMPRESS_STOCKPILE"))
                {
                    Params.RemoveParam("COMPRESS_STOCKPILE");
                }
                else
                {
                    Params.SetParam("COMPRESS_STOCKPILE");
                }
            }))).Checked = Params.IsParamSet("COMPRESS_STOCKPILE");

            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Include referenced files", null, new EventHandler((ob, ev) =>
            {
                if (Params.IsParamSet("INCLUDE_REFERENCED_FILES"))
                {
                    Params.RemoveParam("INCLUDE_REFERENCED_FILES");
                }
                else
                {
                    Params.SetParam("INCLUDE_REFERENCED_FILES");
                }
            }))).Checked = Params.IsParamSet("INCLUDE_REFERENCED_FILES");
            
            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Load entry when selected with arrows", null, new EventHandler((ob, ev) =>
            {
                bool set = Params.IsParamSet("LOAD_STOCKPILE_ENTRY_ON_ARROW_CLICK");
                if (set)
                {
                    Params.RemoveParam("LOAD_STOCKPILE_ENTRY_ON_ARROW_CLICK");
                }
                else
                {
                    Params.SetParam("LOAD_STOCKPILE_ENTRY_ON_ARROW_CLICK");
                }

                _loadEntryWhenSelectedWithArrows = !set;
            }))).Checked = Params.IsParamSet("LOAD_STOCKPILE_ENTRY_ON_ARROW_CLICK");

            ghSettingsMenu.Items.Add(new ToolStripSeparator());

            (ghSettingsMenu.Items.Add("Show Item Name", null,
                    (ob, ev) => { dgvStockpile.Columns["Item"].Visible ^= true; }) as ToolStripMenuItem).Checked =
                dgvStockpile.Columns["Item"].Visible;
            (ghSettingsMenu.Items.Add("Show Game Name", null,
                    (ob, ev) => { dgvStockpile.Columns["GameName"].Visible ^= true; }) as ToolStripMenuItem)
                .Checked =
                dgvStockpile.Columns["GameName"].Visible;
            (ghSettingsMenu.Items.Add("Show System Name", null,
                    (ob, ev) => { dgvStockpile.Columns["SystemName"].Visible ^= true; }) as ToolStripMenuItem)
                .Checked =
                dgvStockpile.Columns["SystemName"].Visible;
            (ghSettingsMenu.Items.Add("Show System Core", null,
                    (ob, ev) => { dgvStockpile.Columns["SystemCore"].Visible ^= true; }) as ToolStripMenuItem)
                .Checked =
                dgvStockpile.Columns["SystemCore"].Visible;
            (ghSettingsMenu.Items.Add("Show Note", null, (ob, ev) => { dgvStockpile.Columns["Note"].Visible ^= true; })
                as ToolStripMenuItem).Checked = dgvStockpile.Columns["Note"].Visible;

            ghSettingsMenu.Show(this, locate);
        }
    }
}
