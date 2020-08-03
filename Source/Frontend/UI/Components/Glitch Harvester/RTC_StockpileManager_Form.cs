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
    using static RTCV.UI.UI_Extensions;

    public partial class RTC_StockpileManager_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public bool DontLoadSelectedStockpile = false;

        private Color? originalSaveButtonColor = null;
        private bool _UnsavedEdits = false;
        public bool UnsavedEdits
        {
            get => _UnsavedEdits;
            set
            {
                _UnsavedEdits = value;

                if (_UnsavedEdits && btnSaveStockpile.Enabled)
                {
                    if (originalSaveButtonColor == null)
                    {
                        originalSaveButtonColor = btnSaveStockpile.BackColor;
                    }

                    btnSaveStockpile.BackColor = Color.Tomato;
                }
                else
                {
                    if (originalSaveButtonColor != null)
                    {
                        btnSaveStockpile.BackColor = originalSaveButtonColor.Value;
                    }
                }
            }
        }

        public RTC_StockpileManager_Form()
        {
            InitializeComponent();

            popoutAllowed = true;
            this.undockedSizable = true;

            dgvStockpile.RowsAdded += (o, e) =>
            {
                RefreshNoteIcons();
            };
        }

        public void dgvStockpile_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e == null || e.RowIndex == -1)
            {
                return;
            }

            try
            {
                S.GET<RTC_StashHistory_Form>().btnAddStashToStockpile.Enabled = false;
                dgvStockpile.Enabled = false;
                btnStockpileUP.Enabled = false;
                btnStockpileDOWN.Enabled = false;

                // Stockpile Note handling
                if (e != null)
                {
                    var senderGrid = (DataGridView)sender;

                    if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                        e.RowIndex >= 0)
                    {
                        StashKey sk = (StashKey)senderGrid.Rows[e.RowIndex].Cells["Item"].Value;
                        S.SET(new RTC_NoteEditor_Form(sk, senderGrid.Rows[e.RowIndex].Cells["Note"]));
                        S.GET<RTC_NoteEditor_Form>().Show();

                        return;
                    }
                }

                S.GET<RTC_StashHistory_Form>().lbStashHistory.ClearSelected();
                S.GET<RTC_StockpilePlayer_Form>().dgvStockpile.ClearSelection();

                S.GET<RTC_GlitchHarvesterBlast_Form>().RedrawActionUI();

                if (dgvStockpile.SelectedRows.Count == 0)
                {
                    return;
                }

                StockpileManager_UISide.CurrentStashkey = (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);

                List<StashKey> keys = dgvStockpile.Rows.Cast<DataGridViewRow>().Select(x => (StashKey)x.Cells[0].Value).ToList();
                if (!StockpileManager_UISide.CheckAndFixMissingReference(StockpileManager_UISide.CurrentStashkey, false, keys))
                {
                    return;
                }

                if (!S.GET<RTC_GlitchHarvesterBlast_Form>().LoadOnSelect)
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

                    //dgv is stupid.
                    //If you shift-select you get things in the order you'd expect (start > end).
                    //If you ctrl+select, you get things in the reverse order (the most recent selected gets inserted at the start of the list)
                    if (IsControlDown())
                        sks.Reverse();
                    StockpileManager_UISide.MergeStashkeys(sks);

                    if (Render.RenderAtLoad && S.GET<RTC_GlitchHarvesterBlast_Form>().loadBeforeOperation)
                    {
                        Render.StartRender();
                    }

                    S.GET<RTC_StashHistory_Form>().RefreshStashHistory();
                    return;
                }

                S.GET<RTC_GlitchHarvesterBlast_Form>().OneTimeExecute();
            }
            finally
            {
                logger.Trace("Stockpile Manager load done, unlocking UI");
                dgvStockpile.Enabled = true;
                btnStockpileUP.Enabled = true;
                btnStockpileDOWN.Enabled = true;
                S.GET<RTC_StashHistory_Form>().btnAddStashToStockpile.Enabled = true;
            }

            S.GET<RTC_GlitchHarvesterBlast_Form>().RedrawActionUI();
        }
        private bool IsControlDown()
        {
            return (Control.ModifierKeys & Keys.Control) != 0;
        }

        private void dgvStockpile_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

                ContextMenuStrip columnsMenu = new ContextMenuStrip();
                (columnsMenu.Items.Add("Show Item Name", null,
                        (ob, ev) => { dgvStockpile.Columns["Item"].Visible ^= true; }) as ToolStripMenuItem).Checked =
                    dgvStockpile.Columns["Item"].Visible;
                (columnsMenu.Items.Add("Show Game Name", null,
                        (ob, ev) => { dgvStockpile.Columns["GameName"].Visible ^= true; }) as ToolStripMenuItem)
                    .Checked =
                    dgvStockpile.Columns["GameName"].Visible;
                (columnsMenu.Items.Add("Show System Name", null,
                        (ob, ev) => { dgvStockpile.Columns["SystemName"].Visible ^= true; }) as ToolStripMenuItem)
                    .Checked =
                    dgvStockpile.Columns["SystemName"].Visible;
                (columnsMenu.Items.Add("Show System Core", null,
                        (ob, ev) => { dgvStockpile.Columns["SystemCore"].Visible ^= true; }) as ToolStripMenuItem)
                    .Checked =
                    dgvStockpile.Columns["SystemCore"].Visible;
                (columnsMenu.Items.Add("Show Note", null, (ob, ev) => { dgvStockpile.Columns["Note"].Visible ^= true; })
                    as ToolStripMenuItem).Checked = dgvStockpile.Columns["Note"].Visible;

                columnsMenu.Items.Add(new ToolStripSeparator());
                ((ToolStripMenuItem)columnsMenu.Items.Add("Open Selected Item in Blast Editor", null, new EventHandler((ob, ev) =>
                {
                    if (S.GET<RTC_NewBlastEditor_Form>() != null)
                    {
                        var sk = (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);
                        RTC_NewBlastEditor_Form.OpenBlastEditor(sk);
                    }
                }))).Enabled = (dgvStockpile.SelectedRows.Count == 1);

                ((ToolStripMenuItem)columnsMenu.Items.Add("Sanitize", null, new EventHandler((ob, ev) =>
                {
                    if (S.GET<RTC_NewBlastEditor_Form>() != null)
                    {
                        var sk = (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);
                        RTC_NewBlastEditor_Form.OpenBlastEditor(sk);
                        S.GET<RTC_NewBlastEditor_Form>().btnSanitizeTool_Click(null, null);
                    }
                }))).Enabled = (dgvStockpile.SelectedRows.Count == 1);

                columnsMenu.Items.Add(new ToolStripSeparator());

                ((ToolStripMenuItem)columnsMenu.Items.Add("Manual Inject", null, new EventHandler((ob, ev) =>
                {
                    var sk = (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);
                    StashKey newSk = (StashKey)sk.Clone();
                    S.GET<RTC_GlitchHarvesterBlast_Form>().IsCorruptionApplied = StockpileManager_UISide.ApplyStashkey(newSk, false, false);
                }))).Enabled = (dgvStockpile.SelectedRows.Count == 1);

                columnsMenu.Items.Add(new ToolStripSeparator());
                ((ToolStripMenuItem)columnsMenu.Items.Add("Generate VMD from Selected Item", null, new EventHandler((ob, ev) =>
                {
                    var sk = (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);
                    MemoryDomains.GenerateVmdFromStashkey(sk);
                    S.GET<RTC_VmdPool_Form>().RefreshVMDs();
                }))).Enabled = (dgvStockpile.SelectedRows.Count == 1);

                ((ToolStripMenuItem)columnsMenu.Items.Add("Merge Selected Stashkeys", null, new EventHandler((ob, ev) =>
                {
                    List<StashKey> sks = new List<StashKey>();
                    foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
                    {
                        sks.Add((StashKey)row.Cells[0].Value);
                    }

                    StockpileManager_UISide.MergeStashkeys(sks);
                    S.GET<RTC_StashHistory_Form>().RefreshStashHistory();
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

                /*
                if (!RTC_NetcoreImplementation.isStandaloneUI)
                {
                    ((ToolStripMenuItem)columnsMenu.Items.Add("[Multiplayer] Send Selected Item as a Blast", null, new EventHandler((ob, ev) => { RTC_NetcoreImplementation.Multiplayer?.SendBlastlayer(); }))).Enabled = RTC_NetcoreImplementation.Multiplayer != null && RTC_NetcoreImplementation.Multiplayer.side != NetworkSide.DISCONNECTED;
                    ((ToolStripMenuItem)columnsMenu.Items.Add("[Multiplayer] Send Selected Item as a Game State", null, new EventHandler((ob, ev) => { RTC_NetcoreImplementation.Multiplayer?.SendStashkey(); }))).Enabled = RTC_NetcoreImplementation.Multiplayer != null && RTC_NetcoreImplementation.Multiplayer.side != NetworkSide.DISCONNECTED;
                }*/

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

        public bool RenameStashKey(StashKey sk)
        {
            string value = sk.Alias;

            if (GetInputBox("Glitch Harvester", "Enter the new Stash name:", ref value) == DialogResult.OK && !string.IsNullOrWhiteSpace(value))
            {
                sk.Alias = value.Trim();
                return true;
            }

            return false;
        }

        private void btnRenameSelected_Click(object sender, EventArgs e)
        {
            if (!btnRenameSelected.Visible)
            {
                return;
            }

            if (dgvStockpile.SelectedRows.Count != 0)
            {
                if (RenameStashKey(dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey))
                {
                    StockpileManager_UISide.StockpileChanged();
                    dgvStockpile.Refresh();
                    UnsavedEdits = true;
                }

                //lbStockpile.RefreshItemsReal();
            }
        }

        private void btnRemoveSelectedStockpile_Click(object sender, EventArgs e)
        {
            RemoveSelected();
        }

        public void RemoveSelected()
        {
            if (Control.ModifierKeys == Keys.Control || (dgvStockpile.SelectedRows.Count != 0 && (MessageBox.Show("Are you sure you want to remove the selected stockpile entries?", "Delete Stockpile Entry?", MessageBoxButtons.YesNo) == DialogResult.Yes)))
            {
                foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
                {
                    dgvStockpile.Rows.Remove(row);
                }
                StockpileManager_UISide.StockpileChanged();
                UnsavedEdits = true;
                S.GET<RTC_GlitchHarvesterBlast_Form>().RedrawActionUI();
            }
        }

        private void btnClearStockpile_Click(object sender, EventArgs e)
        {
            ClearStockpile();
        }

        public void ClearStockpile(bool force = false)
        {
            if (force || MessageBox.Show("Are you sure you want to clear the stockpile?", "Clearing stockpile", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dgvStockpile.Rows.Clear();

                StockpileManager_UISide.ClearCurrentStockpile();

                btnSaveStockpile.Enabled = false;
                UnsavedEdits = false;

                S.GET<RTC_GlitchHarvesterBlast_Form>().RedrawActionUI();
            }
        }

        private async void LoadStockpile(string filename)
        {
            logger.Trace("Entered LoadStockpile {0}", Thread.CurrentThread.ManagedThreadId);
            if (UnsavedEdits && MessageBox.Show("You have unsaved edits in the Glitch Harvester Stockpile. \n\n Are you sure you want to load without saving?",
                "Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            var ghForm = UI_CanvasForm.GetExtraForm("Glitch Harvester");
            try
            {
                //We do this here and invoke because our unlock runs at the end of the awaited method, but there's a chance an error occurs
                //Thus, we want this to happen within the try block
                UICore.SetHotkeyTimer(false);

                logger.Trace("Blocking UI");
                UICore.LockInterface(false, true);
                logger.Trace("UI Blocked");

                logger.Trace("Opening SaveProgress Form");
                S.GET<UI_SaveProgress_Form>().Dock = DockStyle.Fill;
                ghForm?.OpenSubForm(S.GET<UI_SaveProgress_Form>());

                logger.Trace("Clearing Current Stockpile");
                StockpileManager_UISide.ClearCurrentStockpile();
                dgvStockpile.Rows.Clear();

                S.GET<RTC_StockpilePlayer_Form>().dgvStockpile.Rows.Clear();
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
                    StockpileManager_UISide.SetCurrentStockpile(sks);

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
                StockpileManager_UISide.StockpileChanged();

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

        private async void ImportStockpile(string filename)
        {
            var ghForm = UI_CanvasForm.GetExtraForm("Glitch Harvester");
            try
            {
                UICore.SetHotkeyTimer(false);
                UICore.LockInterface(false, true);
                S.GET<UI_SaveProgress_Form>().Dock = DockStyle.Fill;
                ghForm?.OpenSubForm(S.GET<UI_SaveProgress_Form>());

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
            logger.Trace("Entering SaveStockpile {0}\n{1}", System.Threading.Thread.CurrentThread.ManagedThreadId, Environment.StackTrace);
            var ghForm = UI_CanvasForm.GetExtraForm("Glitch Harvester");
            try
            {
                //We do this here and invoke because our unlock runs at the end of the awaited method, but there's a chance an error occurs
                //Thus, we want this to happen within the try block
                UICore.SetHotkeyTimer(false);
                UICore.LockInterface(false, true);
                S.GET<UI_SaveProgress_Form>().Dock = DockStyle.Fill;
                ghForm?.OpenSubForm(S.GET<UI_SaveProgress_Form>());

                var r = await Task.Run(() => Stockpile.Save(sks, path, RTCV.NetCore.Params.IsParamSet("INCLUDE_REFERENCED_FILES"), RTCV.NetCore.Params.IsParamSet("COMPRESS_STOCKPILE")));

                if (r)
                {
                    StockpileManager_UISide.SetCurrentStockpile(sks);
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

        private void btnLoadStockpile_Click(object sender, MouseEventArgs e)
        {
            logger.Trace("Entering LoadStockpile {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
            CorruptCore.RtcCore.CheckForProblematicProcesses();

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
            })).Enabled = (File.Exists(Path.Combine(CorruptCore.RtcCore.EmuDir, "backup_config.ini")));

            loadMenuItems.Show(this, locate);
        }

        public void btnSaveStockpileAs_Click(object sender, EventArgs e)
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

        private void btnSaveStockpile_Click(object sender, EventArgs e)
        {
            Stockpile sks = new Stockpile(dgvStockpile);
            SaveStockpile(sks, StockpileManager_UISide.GetCurrentStockpilePath());
        }

        private void sendCurrentStockpileToSKS()
        {
            foreach (DataGridViewRow dataRow in dgvStockpile.Rows)
            {
                StashKey sk = (StashKey)dataRow.Cells["Item"].Value;
            }
        }

        private void btnStockpileMoveSelectedUp_Click(object sender, EventArgs e)
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

            StockpileManager_UISide.StockpileChanged();
            S.GET<RTC_GlitchHarvesterBlast_Form>().RedrawActionUI();
        }

        private void btnStockpileMoveSelectedDown_Click(object sender, EventArgs e)
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

            StockpileManager_UISide.StockpileChanged();
            S.GET<RTC_GlitchHarvesterBlast_Form>().RedrawActionUI();
        }

        private void dgvStockpile_DragDrop(object sender, DragEventArgs e)
        {
            bool alreadyLoadedAStockpile = false;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var f in files)
            {
                if (f.Contains(".bl"))
                {
                    BlastLayer temp = BlastTools.LoadBlastLayerFromFile(f);
                    StockpileManager_UISide.Import(temp);
                    S.GET<RTC_StashHistory_Form>().RefreshStashHistorySelectLast();
                    S.GET<RTC_StashHistory_Form>().AddStashToStockpile(true);
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
            S.GET<RTC_GlitchHarvesterBlast_Form>().RedrawActionUI();
        }

        private void dgvStockpile_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void btnImportStockpile_Click(object sender, EventArgs e)
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

        private void btnStockpileUP_Click(object sender, EventArgs e)
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

            dgvStockpile_CellClick(dgvStockpile, null);
        }

        private void btnStockpileDOWN_Click(object sender, EventArgs e)
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

            dgvStockpile_CellClick(dgvStockpile, null);
        }

        private void RTC_StockpileManager_Form_Load(object sender, EventArgs e)
        {
            dgvStockpile.AllowDrop = true;
            dgvStockpile.DragDrop += dgvStockpile_DragDrop;
            dgvStockpile.DragEnter += dgvStockpile_DragEnter;
        }

        private void btnGlitchHarvesterSettings_MouseDown(object sender, MouseEventArgs e)
        {
            Point locate = e.GetMouseLocation(sender);
            ContextMenuStrip ghSettingsMenu = new ContextMenuStrip();

            ghSettingsMenu.Items.Add(new ToolStripLabel("Stockpile Manager settings")
            {
                Font = new Font("Segoe UI", 12)
            });

            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Compress Stockpiles", null, new EventHandler((ob, ev) =>
            {
                if (RTCV.NetCore.Params.IsParamSet("COMPRESS_STOCKPILE"))
                {
                    RTCV.NetCore.Params.RemoveParam("COMPRESS_STOCKPILE");
                }
                else
                {
                    RTCV.NetCore.Params.SetParam("COMPRESS_STOCKPILE");
                }
            }))).Checked = RTCV.NetCore.Params.IsParamSet("COMPRESS_STOCKPILE");

            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Include referenced files", null, new EventHandler((ob, ev) =>
            {
                if (RTCV.NetCore.Params.IsParamSet("INCLUDE_REFERENCED_FILES"))
                {
                    RTCV.NetCore.Params.RemoveParam("INCLUDE_REFERENCED_FILES");
                }
                else
                {
                    RTCV.NetCore.Params.SetParam("INCLUDE_REFERENCED_FILES");
                }
            }))).Checked = RTCV.NetCore.Params.IsParamSet("INCLUDE_REFERENCED_FILES");

            ghSettingsMenu.Show(this, locate);
        }
    }
}
