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
                AutoSave.StockpileUnsaved = value;
                
                UpdateSaveButtonColor(value);
            }
        }

        public StockpileManagerForm()
        {
            InitializeComponent();

            PopoutAllowed = true;
            undockedSizable = true;

            dgvStockpile.RowsAdded += (o, e) =>
            {
                RefreshNoteIcons();
            };
            btnSaveStockpile.BackColorChanged += (o, e) => UpdateSaveButtonColor(UnsavedEdits);
            btnSaveStockpileAs.BackColorChanged += (o, e) => UpdateSaveButtonColor(UnsavedEdits);
        }

        public async void HandleCellClick(object sender, DataGridViewCellEventArgs e)
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

                    await StockpileManagerUISide.MergeStashkeys(sks);

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
                DataGridView.HitTestInfo hti = dgvStockpile.HitTest(e.X, e.Y);
                // Necessary to show the context menu for the row we just clicked rather than the one that was selected before
                if (hti.RowIndex != -1)
                {
                    foreach (DataGridViewRow row in this.dgvStockpile.SelectedRows)
                        row.Selected = false;
                    
                    this.dgvStockpile.Rows[hti.RowIndex].Selected = true;
                }
                
                Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

                BlastLayer bl = null;
                
                int rowCount = dgvStockpile.SelectedRows.Count;
                bool oneRowSelected = rowCount == 1;

                if (oneRowSelected)
                    bl = GetSelectedStashKey().BlastLayer;

                new ContextMenuBuilder()
                    .If(bl != null).AddText($"Layer Size: {bl?.Layer?.Count ?? 0}", false).EndIf()
                    .AddItem("Open Selected Item in Blast Editor", (ob, ev) =>
                    {
                        if (S.GET<BlastEditorForm>() != null)
                        {
                            var sk = GetSelectedStashKey();
                            BlastEditorForm.OpenBlastEditor((StashKey)sk.Clone());
                        }
                    }, rowCount == 1)
                    .AddItem("Sanitize", (ob, ev) =>
                    {
                        if (S.GET<BlastEditorForm>() != null)
                        {
                            var sk = this.GetSelectedStashKey();
                            SanitizeToolForm.OpenSanitizeTool((StashKey)sk.Clone(),false);
                        }
                    }, rowCount == 1)
                    .AddSeparator()
                    .AddItem("Manual Inject", async (ob, ev) =>
                    {
                        var sk = this.GetSelectedStashKey();
                        StashKey newSk = (StashKey)sk.Clone();

                        bool isCorrupted = await StockpileManagerUISide.ApplyStashkey(newSk, false, false);

                        if (StockpileManagerUISide.CurrentStashkey != null)
                            S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = isCorrupted;
                    }, rowCount == 1)
                    .AddSeparator()
                    .AddItem("Rename Selected Item", (ob, ev) =>
                    {
                        if (this.dgvStockpile.SelectedRows.Count != 0)
                        {
                            if (RenameStashKey(this.GetSelectedStashKey()))
                            {
                                StockpileManagerUISide.StockpileChanged();
                                this.dgvStockpile.Refresh();
                                this.UnsavedEdits = true;
                            }

                            //lbStockpile.RefreshItemsReal();   
                        }
                    }, rowCount == 1)
                    .AddItem("Generate VMD From Selected Item", (ob, ev) =>
                    {
                        var sk = this.GetSelectedStashKey();
                        MemoryDomains.GenerateVmdFromStashkey(sk);
                        S.GET<VmdPoolForm>().RefreshVMDs();
                    }, rowCount == 1)
                    .AddItem("Merge Selected Stashkeys", async (ob, ev) =>
                    {
                        List<StashKey> sks = new List<StashKey>();
                        foreach (DataGridViewRow row in this.dgvStockpile.SelectedRows)
                        {
                            sks.Add((StashKey)row.Cells[0].Value);
                        }

                        await StockpileManagerUISide.MergeStashkeys(sks);
                        S.GET<StashHistoryForm>().RefreshStashHistorySelectLast();
                    }, rowCount > 1)
                    .AddItem("Replace Associated ROM", (ob, ev) =>
                    {
                        List<StashKey> sks = new List<StashKey>();
                        foreach (DataGridViewRow row in this.dgvStockpile.SelectedRows)
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
                    }, rowCount >= 1)
                    .AddSeparator()
                    .AddItem($"Duplicate Selected Item{(rowCount > 1 ? "s" : "")}", (ob, ev) =>
                    {
                        this.DuplicateSelected();
                    }, rowCount > 0)
                    .AddItem($"Remove Selected Item{(rowCount > 1 ? "s" : "")}", (ob, ev) =>
                    {
                        this.RemoveSelected();
                    }, rowCount > 0)
                    .AddSeparator()
                    .AddItem("Add Savestate to Manager", (ob, ev) =>
                    {
                        S.GET<SavestateManagerForm>().savestateList.NewSavestateFromStockpile();
                    }, rowCount == 1)
                    .Build()
                    .Show(this, locate);
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

        // Check if any emulators in the stockpile are not installed, and prompt the user
        // to select an emu version if it's a legacy stockpile.
        public bool CheckForEmulators(Stockpile sks)
        {
            List<string> missingEmulators = new List<string> { };
            bool missingEmuVer = false;
            foreach (StashKey key in sks.StashKeys)
            {
                if (key.EmuVer != "")
                {
                    string emulatorPath = Path.Combine(RtcCore.RtcDir, "..\\..\\", key.EmuVer);
                    if (!Directory.Exists(emulatorPath) && !missingEmulators.Contains(key.EmuVer))
                        missingEmulators.Add(key.EmuVer);
                }
                // Update stashkey emulator version if it's empty
                else
                {
                    missingEmuVer = true;
                }
            }

            if (missingEmulators.Count > 0)
            {
                string missingEmulatorsString = "";
                foreach (string emulator in missingEmulators)
                {
                    missingEmulatorsString += emulator + "\n";
                }
                string missingEmulatorsMessage = "You are missing the following emulators used in this stockpile: \n\n" +
                                                  String.Join(Environment.NewLine, missingEmulatorsString + "\n" +
                                                  "Please install these emulators and then load the stockpile again.");
                MessageBox.Show(missingEmulatorsMessage, "Operation cancelled", MessageBoxButtons.OK);
                return false;
            }
            else if (missingEmuVer)
            {
                var form = new StockpileEmuVersionForm();

                // start/show the control
                form.ShowDialog();

                if (form.SelectedVersion != null)
                {
                    foreach (StashKey key in sks.StashKeys)
                    {
                        key.EmuVer = form.SelectedVersion;
                    }
                }
                else
                {
                    MessageBox.Show("Emulator system and version selection was cancelled, the stockpile will not be loaded.", "Operation cancelled", MessageBoxButtons.OK);
                    return false;
                }
            }
            return true;
        }

        public async void LoadStockpile(string filename)
        {
            logger.Trace("Entered LoadStockpile {0}", Thread.CurrentThread.ManagedThreadId);
            if (UnsavedEdits && MessageBox.Show("You have unsaved edits in the Glitch Harvester Stockpile. \n\n Are you sure you want to load without saving?",
                "Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

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
                this.ParentCanvas?.OpenSubForm(S.GET<SaveProgressForm>());

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


                    if (!CheckForEmulators(sks))
                        return;

                    logger.Trace("Populating DGV");
                    foreach (StashKey key in sks.StashKeys)
                    {
                        dgvStockpile?.Rows.Add(key, key.GameName, key.SystemName, key.SystemCore, key.EmuVer, key.Note);
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

                UnsavedEdits = true;
            }
            finally
            {
                logger.Trace("Closing Save form");
                this.ParentCanvas?.CloseSubForm();
                UICore.SetHotkeyTimer(true);
                logger.Trace("Unlocking Interface");
                UICore.UnlockInterface();
                logger.Trace("Load done");
            }
        }

        internal StashKey GetSelectedStashKey() => (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);

        private async void ImportStockpile(string filename)
        {
            try
            {
                UICore.SetHotkeyTimer(false);
                UICore.LockInterface(false, true);
                S.GET<SaveProgressForm>().Dock = DockStyle.Fill;
                this.ParentCanvas?.OpenSubForm(S.GET<SaveProgressForm>());

                var r = await Task.Run(() => Stockpile.Import(filename));

                if (!r.Failed)
                {
                    var sks = r.Result;
                    //Todo - Refactor this to get it out of the object
                    //Populate the dgv
                    RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Populating UI", 95));

                    if (!CheckForEmulators(sks))
                        return;

                    foreach (StashKey key in sks.StashKeys)
                    {
                        dgvStockpile?.Rows.Add(key, key.GameName, key.SystemName, key.SystemCore, key.EmuVer, key.Note);
                    }

                    UnsavedEdits = true;

                    RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Done", 100));
                }
            }
            finally
            {
                this.ParentCanvas?.CloseSubForm();
                UICore.UnlockInterface();
                UICore.SetHotkeyTimer(true);
                RefreshNoteIcons();
            }
        }

        private async void SaveStockpile(Stockpile sks, string path)
        {
            logger.Trace("Entering SaveStockpile {0}\n{1}", Thread.CurrentThread.ManagedThreadId, Environment.StackTrace);
            try
            {
                //We do this here and invoke because our unlock runs at the end of the awaited method, but there's a chance an error occurs
                //Thus, we want this to happen within the try block
                UICore.SetHotkeyTimer(false);
                UICore.LockInterface(false, true);
                S.GET<SaveProgressForm>().Dock = DockStyle.Fill;
                this.ParentCanvas?.OpenSubForm(S.GET<SaveProgressForm>());

                var r = await Task.Run(() =>
                {
                    lock (AutoSave.SavingLock)
                    {
                        return Stockpile.Save(sks, path, Params.IsParamSet("INCLUDE_REFERENCED_FILES"), Params.IsParamSet("COMPRESS_STOCKPILE"));
                    }
                });

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
                this.ParentCanvas?.CloseSubForm();
                UICore.UnlockInterface();
                UICore.SetHotkeyTimer(true);
            }
        }

        private void LoadStockpile(object sender, MouseEventArgs e)
        {
            logger.Trace("Entering LoadStockpile {0}", Thread.CurrentThread.ManagedThreadId);
            //RtcCore.CheckForProblematicProcesses();

            Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);
            
            new ContextMenuBuilder()
                .AddItem("Load Stockpile", (ob, ev) =>
                {
                    OpenFileDialog ofd = new OpenFileDialog
                    {
                        DefaultExt = "sks",
                        Title = "Open Stockpile File",
                        Filter = "SKS files|*.sks",
                        RestoreDirectory = true
                    };
                    if (ofd.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    string filename = ofd.FileName;
                    LoadStockpile(filename);
                })
                .AddItem($"Load {RtcCore.VanguardImplementationName} Settings From Stockpile", (ob, ev) =>
                {
                    if (UnsavedEdits && MessageBox.Show($"You have unsaved edits in the Glitch Harvester Stockpile. \n\n This will restart {RtcCore.VanguardImplementationName}. Are you sure you want to load without saving?",
                            "Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    bool wasEnabled = AutoKillSwitch.Enabled;
                    AutoKillSwitch.Enabled = false;
                    Stockpile.LoadConfigFromStockpile();
                    AutoKillSwitch.Enabled = wasEnabled;
                })
                .AddItem($"Restore {RtcCore.VanguardImplementationName} Config Backup", (ob, ev) =>
                {
                    if (UnsavedEdits && MessageBox.Show(
                            $"You have unsaved edits in the Glitch Harvester Stockpile. \n\n This will restart {RtcCore.VanguardImplementationName}. Are you sure you want to load without saving?",
                            "Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }

                    bool wasEnabled = AutoKillSwitch.Enabled;
                    AutoKillSwitch.Enabled = false;
                    Stockpile.RestoreEmuConfig();
                    AutoKillSwitch.Enabled = wasEnabled;
                }, File.Exists(Path.Combine(RtcCore.EmuDir, "backup_config.ini")))
                .Build()
                .Show(this, locate);
        }

        public void SaveStockpileAs(object sender, EventArgs e)
        {
            if (dgvStockpile.Rows.Count == 0)
            {
                MessageBox.Show("You cannot save the Stockpile because it is empty");
                return;
            }

            UICore.SetHotkeyTimer(false);
            string path;
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
        
        private void UpdateSaveButtonColor(bool unsaved)
        {
            if (unsaved)
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
                btnSaveStockpileAs.BackColor = c;
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

        private async void StockpileUp(object sender, EventArgs e)
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
                await Task.Run(() => HandleCellClick(dgvStockpile, new DataGridViewCellEventArgs(0, dgvStockpile.SelectedRows[0].Index)));
            }
        }

        private async void StockpileDown(object sender, EventArgs e)
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
                await Task.Run(() => HandleCellClick(dgvStockpile, new DataGridViewCellEventArgs(0, dgvStockpile.SelectedRows[0].Index)));
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            dgvStockpile.AllowDrop = true;
            dgvStockpile.DragDrop += HandleDragDrop;
            dgvStockpile.DragEnter += HandleDragEnter;
        }

        internal ToolStripButton UpdateEmuVersionButton;
        private void HandleGlitchHarvesterSettingsMouseDown(object sender, MouseEventArgs e)
        {
            Point locate = e.GetMouseLocation(sender);
            
            var columns = dgvStockpile.Columns;
            bool itemVisible = columns["Item"]!.Visible;
            bool gameNameVisible = columns["GameName"]!.Visible;
            bool systemNameVisible = columns["SystemName"]!.Visible;
            bool systemCoreVisible = columns["SystemCore"]!.Visible;
            bool emuVerVisible = columns["EmuVer"]!.Visible;
            
            new ContextMenuBuilder()
                .AddHeader("Stockpile Manager Settings")
                .AddText("Stockpile Items: " + dgvStockpile.Rows.Cast<DataGridViewRow>().Count(), false)
                
                .AddItem("Compress Stockpiles", (ob, ev) 
                    => Params.ToggleParam("COMPRESS_STOCKPILE"),
                    isChecked: Params.IsParamSet("COMPRESS_STOCKPILE"))
                
                .AddItem("Include Referenced Files", (ob, ev)
                    => Params.ToggleParam("INCLUDE_REFERENCED_FILES"),
                    isChecked: Params.IsParamSet("INCLUDE_REFERENCED_FILES"))
                
                .AddItem("Load Entry When Selected With Arrows", (ob, ev)
                    => _loadEntryWhenSelectedWithArrows = Params.ToggleParam("LOAD_STOCKPILE_ENTRY_ON_ARROW_CLICK"),
                    isChecked: Params.IsParamSet("LOAD_STOCKPILE_ENTRY_ON_ARROW_CLICK"))
                
                .AddSeparator()
                
                .AddItem("Show Item Name", (ob, ev)
                    => columns["Item"]!.Visible = !itemVisible,
                    isChecked: itemVisible)
                
                .AddItem("Show Game Name", (ob, ev)
                    => columns["GameName"]!.Visible = !gameNameVisible,
                    isChecked: gameNameVisible)
                
                .AddItem("Show System Name", (ob, ev)
                    => columns["SystemName"]!.Visible = !systemNameVisible,
                    isChecked: systemNameVisible)
                
                .AddItem("Show System Core", (ob, ev)
                    => columns["SystemCore"]!.Visible = !systemCoreVisible,
                    isChecked: systemCoreVisible)

                .AddItem("Show Emulator Version", (ob, ev)
                    => columns["EmuVer"]!.Visible = !emuVerVisible,
                    isChecked: emuVerVisible)

                .AddSeparator()

                .AddItem("Update Emulator Version", (ob, ev)
                    => UpdateEmuVersionButton_Click(ob, ev))
                
                .Build()
                .Show(this, locate);
        }

        private void UpdateEmuVersionButton_Click(object sender, EventArgs e)
        {
            var form = new StockpileEmuVersionForm(false);

            // start/show the control
            form.ShowDialog();

            if (form.SelectedVersion != null)
            {
                foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
                {
                    ((StashKey)row.Cells[0].Value).EmuVer = form.SelectedVersion;
                    row.Cells[EmuVer.Index].Value = form.SelectedVersion;
                }

                UnsavedEdits = true;
            }
        }

    }
}
