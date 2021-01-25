namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class StockpilePlayerForm : ComponentForm, IBlockable
    {
        private bool currentlyLoading = false;

        public StockpilePlayerForm()
        {
            InitializeComponent();
            dgvStockpile.DragDrop += OnStockpileDragDrop;
            dgvStockpile.DragEnter += OnStockpileDragEnter;
            dgvStockpile.RowsAdded += (o, e) =>
            {
                RefreshNoteIcons();
            };
            dgvStockpile.ColumnHeaderMouseClick += (o, e) =>
            {
                RefreshNoteIcons();
            };
        }

        private void OnStockpileDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void OnStockpileDragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (files.Length > 0 && files[0]
                .Contains(".sks"))
            {
                if (Stockpile.Load(files[0]) is { Failed: false } r)
                {
                    var sks = r.Result;

                    foreach (StashKey key in sks.StashKeys)
                    {
                        dgvStockpile?.Rows.Add(key, key.GameName, key.SystemName, key.SystemCore, key.Note);
                    }
                }
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void TrySelectPrevious(object sender, EventArgs e)
        {
            try
            {
                btnPreviousItem.Visible = false;

                if (dgvStockpile.SelectedRows.Count == 0)
                {
                    return;
                }

                int CurrentSelectedIndex = dgvStockpile.SelectedRows[0].Index;

                if (CurrentSelectedIndex == 0)
                {
                    dgvStockpile.ClearSelection();
                    dgvStockpile.Rows[dgvStockpile.Rows.Count - 1].Selected = true;
                }
                else
                {
                    dgvStockpile.ClearSelection();
                    dgvStockpile.Rows[CurrentSelectedIndex - 1].Selected = true;
                }

                OnStockpileCellClick(dgvStockpile, new DataGridViewCellEventArgs(0, dgvStockpile.SelectedRows[0].Index));
            }
            finally
            {
                btnPreviousItem.Visible = true;
            }
        }

        private void TrySelectNext(object sender, EventArgs e)
        {
            try
            {
                btnNextItem.Visible = false;

                if (dgvStockpile.SelectedRows.Count == 0)
                {
                    return;
                }

                int CurrentSelectedIndex = dgvStockpile.SelectedRows[0].Index;

                if (CurrentSelectedIndex == dgvStockpile.Rows.Count - 1)
                {
                    dgvStockpile.ClearSelection();
                    dgvStockpile.Rows[0].Selected = true;
                }
                else
                {
                    dgvStockpile.ClearSelection();
                    dgvStockpile.Rows[CurrentSelectedIndex + 1].Selected = true;
                }

                OnStockpileCellClick(dgvStockpile, new DataGridViewCellEventArgs(0, dgvStockpile.SelectedRows[0].Index));
            }
            finally
            {
                btnNextItem.Visible = true;
            }
        }

        private void TryReload(object sender, EventArgs e)
        {
            try
            {
                btnReloadItem.Visible = false;
                OnStockpileCellClick(null, null);
            }
            finally
            {
                btnReloadItem.Visible = true;
            }
        }

        private void BlastLayerToggle(object sender, EventArgs e)
        {
            S.GET<GlitchHarvesterBlastForm>().BlastLayerToggle(null, null);
        }

        private async void OnStockpileMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

                ToolStripSeparator stripSeparator = new ToolStripSeparator();
                stripSeparator.Paint += OnStripSeparatorPaint;

                ContextMenuStrip columnsMenu = new ContextMenuStrip();
                (columnsMenu.Items.Add("Show Item Name", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["Item"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["Item"].Visible;
                (columnsMenu.Items.Add("Show Game Name", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["GameName"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["GameName"].Visible;
                (columnsMenu.Items.Add("Show System Name", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["SystemName"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["SystemName"].Visible;
                (columnsMenu.Items.Add("Show System Core", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["SystemCore"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["SystemCore"].Visible;
                (columnsMenu.Items.Add("Show Note", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["Note"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["Note"].Visible;
                columnsMenu.Items.Add(stripSeparator);
                (columnsMenu.Items.Add("Load on Select", null, new EventHandler((ob, ev) => { S.GET<GlitchHarvesterBlastForm>().LoadOnSelect ^= true; })) as ToolStripMenuItem).Checked = S.GET<GlitchHarvesterBlastForm>().LoadOnSelect;
                (columnsMenu.Items.Add("Clear Infinite Units on Rewind", null, new EventHandler((ob, ev) => { S.GET<CorruptionEngineForm>().HellgenieEngineControl.cbClearCheatsOnRewind.Checked ^= true; })) as ToolStripMenuItem).Checked = S.GET<CorruptionEngineForm>().HellgenieEngineControl.cbClearCheatsOnRewind.Checked;

                columnsMenu.Items.Add(stripSeparator);

                ((ToolStripMenuItem)columnsMenu.Items.Add("Manual Inject", null, new EventHandler((ob, ev) =>
                {
                    var sk = GetSelectedStashKey();
                    StashKey newSk = (StashKey)sk.Clone();

                    var t = StockpileManagerUISide.ApplyStashkey(newSk, false);
                    t.RunSynchronously();
                    S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = t.Result;
                }))).Enabled = (dgvStockpile.SelectedRows.Count == 1);

                columnsMenu.Show(this, locate);
            }
        }

        private void OnStripSeparatorPaint(object sender, PaintEventArgs e)
        {
            ToolStripSeparator stripSeparator = sender as ToolStripSeparator;
            ContextMenuStrip menuStrip = stripSeparator.Owner as ContextMenuStrip;
            e.Graphics.FillRectangle(new SolidBrush(Color.Transparent), new Rectangle(0, 0, stripSeparator.Width, stripSeparator.Height));
            using (Pen pen = new Pen(Color.LightGray, 1))
            {
                e.Graphics.DrawLine(pen, new Point(23, stripSeparator.Height / 2), new Point(menuStrip.Width, stripSeparator.Height / 2));
            }
        }

        private async void LoadStockpile(string fileName)
        {
            try
            {
                //We do this here and invoke because our unlock runs at the end of the awaited method, but there's a chance an error occurs
                //Thus, we want this to happen within the try block
                UICore.SetHotkeyTimer(false);
                UICore.LockInterface(false, true);
                S.GET<SaveProgressForm>().Dock = DockStyle.Fill;
                CoreForm.cfForm?.OpenSubForm(S.GET<SaveProgressForm>());

                StockpileManagerUISide.ClearCurrentStockpile();

                //Clear out the DGVs
                S.GET<StockpileManagerForm>().dgvStockpile.Rows.Clear(); // Clear the stockpile manager
                dgvStockpile.Rows.Clear(); // Clear the stockpile player

                var r = await Task.Run(() => Stockpile.Load(fileName));

                if (r.Failed)
                {
                    MessageBox.Show($"Loading the stockpile failed!\n" +
                                    $"{r.GetErrorsFormatted()}");
                }
                else
                {
                    var sks = r.Result;
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        foreach (StashKey key in sks.StashKeys) //Populate the dgv
                        {
                            dgvStockpile?.Rows.Add(key, key.GameName, key.SystemName, key.SystemCore, key.Note);
                        }
                    });
                }

                List<StashKey> keys = dgvStockpile.Rows.Cast<DataGridViewRow>().Select(x => (StashKey)x.Cells["Item"].Value).ToList();
                foreach (var sk in keys)
                {
                    StockpileManagerUISide.CheckAndFixMissingReference(sk, false, keys);
                }

                dgvStockpile.ClearSelection();
                RefreshNoteIcons();
            }
            finally
            {
                CoreForm.cfForm?.CloseSubForm();
                UICore.UnlockInterface();
                UICore.SetHotkeyTimer(true);
            }
        }

        private void TryLoadStockpile(object sender, MouseEventArgs e)
        {
            Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

            ContextMenuStrip LoadMenuItems = new ContextMenuStrip();
            LoadMenuItems.Items.Add("Load Stockpile", null, new EventHandler((ob, ev) =>
            {
                try
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
                }
                catch (Exception ex)
                {
                    if (CloudDebug.ShowErrorDialog(ex, true) == DialogResult.Abort)
                    {
                        throw new AbortEverythingException();
                    }
                }
            }));

            LoadMenuItems.Items.Add($"Load {RtcCore.VanguardImplementationName} settings from Stockpile", null, new EventHandler((ob, ev) =>
            {
                try
                {
                    AutoKillSwitch.Enabled = false;
                    Stockpile.LoadConfigFromStockpile();
                    AutoKillSwitch.Enabled = true;
                }
                catch (Exception ex)
                {
                    if (CloudDebug.ShowErrorDialog(ex, true) == DialogResult.Abort)
                    {
                        throw new AbortEverythingException();
                    }
                }
            }));

            LoadMenuItems.Items.Add($"Restore {RtcCore.VanguardImplementationName} config Backup", null, new EventHandler((ob, ev) =>
            {
                try
                {
                    AutoKillSwitch.Enabled = false;
                    Stockpile.RestoreEmuConfig();
                    AutoKillSwitch.Enabled = true;
                }
                finally
                {
                }
            })).Enabled = (File.Exists(Path.Combine(RtcCore.EmuDir, "backup_config.ini")));

            LoadMenuItems.Show(this, locate);
        }

        private void OnStockpileCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (currentlyLoading || !S.GET<GlitchHarvesterBlastForm>().LoadOnSelect || e?.RowIndex == -1)
            {
                return;
            }

            try
            {
                //dgvStockpile.Enabled = false;
                currentlyLoading = true;

                if (e != null)
                {
                    var senderGrid = (DataGridView)sender;

                    StashKey sk = (StashKey)senderGrid.Rows[e.RowIndex].Cells["Item"].Value;

                    if (sk.Note != null)
                    {
                        tbNoteBox.Text = sk.Note.Replace("\n", Environment.NewLine);
                    }
                    else
                    {
                        tbNoteBox.Text = "";
                    }

                    if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                        e.RowIndex >= 0)
                    {
                        S.SET(new NoteEditorForm(sk, senderGrid.Rows[e.RowIndex].Cells["Note"]));
                        S.GET<NoteEditorForm>().Show();
                        return;
                    }
                }

                if (dgvStockpile.SelectedRows.Count > 0)
                {
                    //Shut autocorrupt off because people (Vinny) kept turning it on to add to corruptions then forgetting to turn it off
                    S.GET<CoreForm>().AutoCorrupt = false;

                    S.GET<GlitchHarvesterBlastForm>().ghMode = GlitchHarvesterMode.CORRUPT;
                    StockpileManagerUISide.CurrentStashkey = GetSelectedStashKey();
                    StockpileManagerUISide.ApplyStashkey(StockpileManagerUISide.CurrentStashkey);

                    S.GET<StashHistoryForm>().lbStashHistory.ClearSelected();
                    S.GET<StockpileManagerForm>().dgvStockpile.ClearSelection();

                    S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = !(StockpileManagerUISide.CurrentStashkey.BlastLayer == null || StockpileManagerUISide.CurrentStashkey.BlastLayer.Layer.Count == 0);
                }
            }
            finally
            {
                currentlyLoading = false;
                //dgvStockpile.Enabled = true;
            }
        }

        private StashKey GetSelectedStashKey() => (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);

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
                    dataRow.Cells["Note"].Value = string.Empty;
                }
                else
                {
                    dataRow.Cells["Note"].Value = "📝";
                }
            }
        }
    }
}
