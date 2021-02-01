namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class GlitchHarvesterBlastForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public bool MergeMode { get; private set; } = false;
        public GlitchHarvesterMode ghMode { get; set; } = GlitchHarvesterMode.CORRUPT;

        public bool LoadOnSelect { get; set; } = true;
        public bool loadBeforeOperation { get; set; } = true;

        private Color? originalRenderOutputButtonColor = null;

        private bool isCorruptionApplied;
        public bool IsCorruptionApplied
        {
            get => isCorruptionApplied;
            set
            {
                if (value)
                {
                    btnBlastToggle.BackColor = Color.FromArgb(224, 128, 128);
                    btnBlastToggle.ForeColor = Color.Black;
                    btnBlastToggle.Text = "BlastLayer : ON";

                    S.GET<StockpilePlayerForm>().btnBlastToggle.BackColor = Color.FromArgb(224, 128, 128);
                    S.GET<StockpilePlayerForm>().btnBlastToggle.ForeColor = Color.Black;
                    S.GET<StockpilePlayerForm>().btnBlastToggle.Text = "BlastLayer : ON     (Attempts to uncorrupt/recorrupt in real-time)";

                    S.GET<SimpleModeForm>().btnBlastToggle.BackColor = Color.FromArgb(224, 128, 128);
                    S.GET<SimpleModeForm>().btnBlastToggle.ForeColor = Color.Black;
                    S.GET<SimpleModeForm>().btnBlastToggle.Text = "BlastLayer : ON     (Attempts to uncorrupt/recorrupt in real-time)";
                }
                else
                {
                    btnBlastToggle.BackColor = S.GET<CoreForm>().btnLogo.BackColor;
                    btnBlastToggle.ForeColor = Color.White;
                    btnBlastToggle.Text = "BlastLayer : OFF";

                    S.GET<StockpilePlayerForm>().btnBlastToggle.BackColor = S.GET<CoreForm>().btnLogo.BackColor;
                    S.GET<StockpilePlayerForm>().btnBlastToggle.ForeColor = Color.White;
                    S.GET<StockpilePlayerForm>().btnBlastToggle.Text = "BlastLayer : OFF    (Attempts to uncorrupt/recorrupt in real-time)";

                    S.GET<SimpleModeForm>().btnBlastToggle.BackColor = S.GET<CoreForm>().btnLogo.BackColor;
                    S.GET<SimpleModeForm>().btnBlastToggle.ForeColor = Color.White;
                    S.GET<SimpleModeForm>().btnBlastToggle.Text = "BlastLayer : OFF    (Attempts to uncorrupt/recorrupt in real-time)";
                }

                isCorruptionApplied = value;
            }
        }

        public GlitchHarvesterBlastForm()
        {
            InitializeComponent();

            popoutAllowed = true;
            this.undockedSizable = false;

            //cbRenderType.SelectedIndex = 0;

            //Registers the drag and drop with the blast edirot form
            AllowDrop = true;
            this.DragEnter += OnDragEnter;
            this.DragDrop += OnDragDrop;
        }

        private async void OnDragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var f in files)
            {
                if (f.Contains(".bl"))
                {
                    BlastLayer bl = BlastTools.LoadBlastLayerFromFile(f);
                    var newStashKey = new StashKey(RtcCore.GetRandomKey(), null, bl);
                    S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = await StockpileManagerUISide.ApplyStashkey(newStashKey, false, false);
                }
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        public async void OneTimeExecute()
        {
            logger.Trace("Entering OneTimeExecute()");
            //Disable autocorrupt
            S.GET<CoreForm>().AutoCorrupt = false;

            if (ghMode == GlitchHarvesterMode.CORRUPT)
            {
                IsCorruptionApplied = await StockpileManagerUISide.ApplyStashkey(StockpileManagerUISide.CurrentStashkey, loadBeforeOperation);
            }
            else if (ghMode == GlitchHarvesterMode.INJECT)
            {
                IsCorruptionApplied = await StockpileManagerUISide.InjectFromStashkey(StockpileManagerUISide.CurrentStashkey, loadBeforeOperation);
                S.GET<StashHistoryForm>().RefreshStashHistory();
            }
            else if (ghMode == GlitchHarvesterMode.ORIGINAL)
            {
                IsCorruptionApplied = await StockpileManagerUISide.OriginalFromStashkey(StockpileManagerUISide.CurrentStashkey);
            }

            if (Render.RenderAtLoad && loadBeforeOperation)
            {
                Render.StartRender();
            }
            else
            {
                Render.StopRender();
            }
            logger.Trace("Exiting OneTimeExecute()");
        }

        public void RedrawActionUI()
        {
            // Merge tool and ui change
            if (S.GET<StockpileManagerForm>().dgvStockpile.SelectedRows.Count > 1)
            {
                MergeMode = true;
                btnCorrupt.Text = "  Merge";
                S.GET<StockpileManagerForm>().btnRenameSelected.Visible = false;
                S.GET<StockpileManagerForm>().btnRemoveSelectedStockpile.Text = "  Remove Items";
            }
            else
            {
                MergeMode = false;
                S.GET<StockpileManagerForm>().btnRenameSelected.Visible = true;
                S.GET<StockpileManagerForm>().btnRemoveSelectedStockpile.Text = "  Remove Item";

                if (ghMode == GlitchHarvesterMode.CORRUPT)
                {
                    btnCorrupt.Text = "  Corrupt";
                }
                else if (ghMode == GlitchHarvesterMode.INJECT)
                {
                    btnCorrupt.Text = "  Inject";
                }
                else if (ghMode == GlitchHarvesterMode.ORIGINAL)
                {
                    btnCorrupt.Text = "  Original";
                }
            }
        }

        public void refreshRenderOutputButton()
        {
            if (Render.IsRendering)
            {
                if (originalRenderOutputButtonColor == null)
                {
                    originalRenderOutputButtonColor = btnRenderOutput.BackColor;
                }

                btnRenderOutput.BackColor = Color.LimeGreen;
            }
            else
            {
                if (originalRenderOutputButtonColor != null)
                {
                    btnRenderOutput.BackColor = originalRenderOutputButtonColor.Value;
                }
            }
        }

        public async void Corrupt(object sender, EventArgs e)
        {
            logger.Trace("btnCorrupt Clicked");

            if (sender != null)
            {
                if (!(btnCorrupt.Visible || AllSpec.VanguardSpec[VSPEC.REPLACE_MANUALBLAST_WITH_GHCORRUPT] != null && S.GET<CoreForm>().btnManualBlast.Visible))
                {
                    return;
                }
            }

            try
            {
                SetBlastButtonVisibility(false);

                if (!(AllSpec.UISpec["SELECTEDDOMAINS"] is string[] domains) || domains.Length == 0)
                {
                    MessageBox.Show("Can't corrupt with no domains selected.");
                    return;
                }

                //Shut off autocorrupt if it's on.
                //Leave this check here so we don't wastefully update the spec
                if (S.GET<CoreForm>().AutoCorrupt)
                {
                    S.GET<CoreForm>().AutoCorrupt = false;
                }

                StashKey psk = StockpileManagerUISide.CurrentSavestateStashKey;

                if (MergeMode)
                {
                    List<StashKey> sks = new List<StashKey>();

                    //Reverse before merging because DataGridView selectedrows is backwards for some odd reason
                    var reversed = S.GET<StockpileManagerForm>().dgvStockpile.SelectedRows.Cast<DataGridViewRow>().Reverse();
                    foreach (DataGridViewRow row in reversed)
                    {
                        sks.Add((StashKey)row.Cells[0].Value);
                    }

                    IsCorruptionApplied = await StockpileManagerUISide.MergeStashkeys(sks);

                    S.GET<StashHistoryForm>().RefreshStashHistorySelectLast();
                    //lbStashHistory.TopIndex = lbStashHistory.Items.Count - 1;

                    return;
                }

                if (ghMode == GlitchHarvesterMode.CORRUPT)
                {
                    string romFilename = (string)AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME];

                    if (romFilename?.Contains("|") ?? false)
                    {
                        MessageBox.Show($"The Glitch Harvester attempted to corrupt a game bound to the following file:\n{romFilename}\n\nIt cannot be processed because the rom seems to be inside a Zip Archive\n(Bizhawk returned a filename with the chracter | in it)");
                        return;
                    }

                    S.GET<StashHistoryForm>().DontLoadSelectedStash = true;
                    IsCorruptionApplied = await StockpileManagerUISide.Corrupt(loadBeforeOperation);
                    S.GET<StashHistoryForm>().RefreshStashHistorySelectLast();
                }
                else if (ghMode == GlitchHarvesterMode.INJECT)
                {
                    if (StockpileManagerUISide.CurrentStashkey == null)
                    {
                        if (StockpileManagerUISide.LastStashkey != null)
                        {
                            StockpileManagerUISide.CurrentStashkey = StockpileManagerUISide.LastStashkey;
                        }
                        else
                        {
                            throw new Exception("Inject tried to fetch the LastStashkey backup but this one was also null! Try to re-load your savestate and then re-select your corruption in the stash history or stockpile. That might fix it. If it still doesn't work after that, report to the devs pls");
                        }
                    }

                    S.GET<StashHistoryForm>().DontLoadSelectedStash = true;

                    IsCorruptionApplied = await StockpileManagerUISide.InjectFromStashkey(StockpileManagerUISide.CurrentStashkey, loadBeforeOperation);
                    S.GET<StashHistoryForm>().RefreshStashHistorySelectLast();
                }
                else if (ghMode == GlitchHarvesterMode.ORIGINAL)
                {
                    if (StockpileManagerUISide.CurrentStashkey == null)
                    {
                        if (StockpileManagerUISide.LastStashkey != null)
                        {
                            StockpileManagerUISide.CurrentStashkey = StockpileManagerUISide.LastStashkey;
                        }
                        else
                        {
                            throw new Exception("CurrentStashkey in original was somehow null! Report this to the devs and tell them how you caused this.");
                        }
                    }

                    S.GET<StashHistoryForm>().DontLoadSelectedStash = true;
                    IsCorruptionApplied = await StockpileManagerUISide.OriginalFromStashkey(StockpileManagerUISide.CurrentStashkey);
                }

                if (Render.RenderAtLoad && loadBeforeOperation)
                {
                    Render.StartRender();
                }
                else
                {
                    Render.StopRender();
                }

                logger.Trace("Blast done");
            }
            finally
            {
                SetBlastButtonVisibility(true);
            }
        }

        private void BlastRawStash()
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Basic.ManualBlast, true);
            SendRawToStash(null, null);
        }

        public void btnCorrupt_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = e.GetMouseLocation(sender);

                ContextMenuStrip columnsMenu = new ContextMenuStrip();
                columnsMenu.Items.Add("Blast + Send RAW To Stash", null, new EventHandler((ob, ev) =>
                {
                    BlastRawStash();
                }));
                columnsMenu.Show(this, locate);
            }
        }

        public void SendRawToStash(object sender, EventArgs e) => SendRawToStash();
        public StashKey SendRawToStash(bool bypassChecks = false)
        {
            if (!btnSendRaw.Visible && !bypassChecks)
            {
                return null;
            }

            try
            {
                SetBlastButtonVisibility(false);

                string romFilename = (string)AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME];
                if (romFilename == null)
                {
                    return null;
                }

                if (romFilename.Contains("|"))
                {
                    MessageBox.Show($"The Glitch Harvester attempted to corrupt a game bound to the following file:\n{romFilename}\n\nIt cannot be processed because the rom seems to be inside a Zip Archive\n(Bizhawk returned a filename with the chracter | in it)");
                    return null;
                }

                StashKey sk = LocalNetCoreRouter.QueryRoute<StashKey>(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.KeyGetRawBlastLayer, true);

                StockpileManagerUISide.CurrentStashkey = sk;
                StockpileManagerUISide.StashHistory.Add(StockpileManagerUISide.CurrentStashkey);

                S.GET<StashHistoryForm>().DontLoadSelectedStash = true;
                S.GET<StashHistoryForm>().RefreshStashHistorySelectLast();
                S.GET<StashHistoryForm>().DontLoadSelectedStash = true;
                S.GET<StockpileManagerForm>().dgvStockpile.ClearSelection();
                S.GET<StashHistoryForm>().DontLoadSelectedStash = false;
            }
            finally
            {
                SetBlastButtonVisibility(true);
            }

            return StockpileManagerUISide.CurrentStashkey;
        }

        public void BlastLayerToggle(object sender, EventArgs e)
        {
            if (StockpileManagerUISide.CurrentStashkey?.BlastLayer?.Layer == null || StockpileManagerUISide.CurrentStashkey?.BlastLayer?.Layer.Count == 0)
            {
                IsCorruptionApplied = false;
                return;
            }

            if (!IsCorruptionApplied)
            {
                IsCorruptionApplied = true;

                LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.SetApplyCorruptBL, true);
            }
            else
            {
                IsCorruptionApplied = false;

                LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.SetApplyUncorruptBL, true);
                LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearStepBlastUnits, null, true);
            }
        }

        private void OnRerollButtonMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = this.PointToClient(Cursor.Position);
                ContextMenuStrip rerollMenu = new ContextMenuStrip();
                rerollMenu.Items.Add("Configure Reroll", null, new EventHandler((ob, ev) =>
                {
                    S.GET<CoreForm>().OpenSettings(null, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
                    S.GET<SettingsForm>().lbForm.SetFocusedForm(S.GET<SettingsCorruptForm>());
                    S.GET<CoreForm>().BringToFront();
                }));

                rerollMenu.Show(this, locate);
            }
        }

        public async void RerollSelected(object sender, EventArgs e)
        {
            if (!btnRerollSelected.Visible)
            {
                return;
            }

            try
            {
                SetBlastButtonVisibility(false);

                if (S.GET<StashHistoryForm>().lbStashHistory.SelectedIndex != -1)
                {
                    StockpileManagerUISide.CurrentStashkey = (StashKey)StockpileManagerUISide.StashHistory[S.GET<StashHistoryForm>().lbStashHistory.SelectedIndex].Clone();
                }
                else if (S.GET<StockpileManagerForm>().dgvStockpile.SelectedRows.Count != 0 && S.GET<StockpileManagerForm>().GetSelectedStashKey() != null)
                {
                    StockpileManagerUISide.CurrentStashkey = (StashKey)S.GET<StockpileManagerForm>().GetSelectedStashKey()?.Clone();
                    //StockpileManager_UISide.unsavedEdits = true;
                }
                else
                {
                    return;
                }

                if (StockpileManagerUISide.CurrentStashkey != null)
                {
                    StockpileManagerUISide.CurrentStashkey.BlastLayer.Reroll();

                    if (StockpileManagerUISide.AddCurrentStashkeyToStash())
                    {
                        S.GET<StockpileManagerForm>().dgvStockpile.ClearSelection();
                        S.GET<StashHistoryForm>()
                            .RefreshStashHistory();
                        S.GET<StashHistoryForm>()
                            .lbStashHistory.ClearSelected();
                        S.GET<StashHistoryForm>()
                            .DontLoadSelectedStash = true;
                        S.GET<StashHistoryForm>()
                            .lbStashHistory.SelectedIndex = S.GET<StashHistoryForm>()
                            .lbStashHistory.Items.Count - 1;
                    }

                    IsCorruptionApplied = await StockpileManagerUISide.ApplyStashkey(StockpileManagerUISide.CurrentStashkey);
                }
            }
            finally
            {
                SetBlastButtonVisibility(true);
            }
        }

        public void SetBlastButtonVisibility(bool visible)
        {
            btnCorrupt.Visible = visible;
            btnRerollSelected.Visible = visible;
            btnSendRaw.Visible = visible;

            if (AllSpec.VanguardSpec[VSPEC.REPLACE_MANUALBLAST_WITH_GHCORRUPT] != null)
            {
                S.GET<CoreForm>().btnManualBlast.Visible = visible;
            }
        }

        private void OpenGlitchHarvesterSettings(object sender, MouseEventArgs e)
        {
            Point locate = e.GetMouseLocation(sender);
            ContextMenuStrip ghSettingsMenu = new ContextMenuStrip();

            ghSettingsMenu.Items.Add(new ToolStripLabel("Glitch Harvester Mode")
            {
                Font = new Font("Segoe UI", 12)
            });

            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Corrupt", null, new EventHandler((ob, ev) =>
            {
                ghMode = GlitchHarvesterMode.CORRUPT;
                RedrawActionUI();
            }))).Checked = (ghMode == GlitchHarvesterMode.CORRUPT);
            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Inject", null, new EventHandler((ob, ev) =>
            {
                ghMode = GlitchHarvesterMode.INJECT;
                RedrawActionUI();
            }))).Checked = (ghMode == GlitchHarvesterMode.INJECT);
            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Original", null, new EventHandler((ob, ev) =>
            {
                ghMode = GlitchHarvesterMode.ORIGINAL;
                RedrawActionUI();
            }))).Checked = (ghMode == GlitchHarvesterMode.ORIGINAL);

            ghSettingsMenu.Items.Add(new ToolStripSeparator());

            ghSettingsMenu.Items.Add(new ToolStripLabel("Behaviors")
            {
                Font = new Font("Segoe UI", 12)
            });

            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Auto-Load State", null, new EventHandler((ob, ev) =>
            {
                loadBeforeOperation = loadBeforeOperation ^= true;
                RedrawActionUI();
            }))).Checked = loadBeforeOperation;
            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Load on select", null, new EventHandler((ob, ev) =>
            {
                LoadOnSelect = LoadOnSelect ^= true;
                RedrawActionUI();
            }))).Checked = LoadOnSelect;
            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Stash results", null, new EventHandler((ob, ev) =>
            {
                StockpileManagerUISide.StashAfterOperation = StockpileManagerUISide.StashAfterOperation ^= true;
                RedrawActionUI();
            }))).Checked = StockpileManagerUISide.StashAfterOperation;

            ghSettingsMenu.Show(this, locate);
        }

        private void RenderOutput(object sender, MouseEventArgs e)
        {
            Point locate = e.GetMouseLocation(sender);
            ContextMenuStrip ghSettingsMenu = new ContextMenuStrip();

            ghSettingsMenu.Items.Add(new ToolStripLabel("Render Output")
            {
                Font = new Font("Segoe UI", 12)
            });

            ((ToolStripMenuItem)ghSettingsMenu.Items.Add((Render.IsRendering ? "Stop rendering" : "Start rendering"), null, new EventHandler((ob, ev) =>
            {
                if (Render.IsRendering)
                {
                    Render.StopRender();
                }
                else
                {
                    Render.StartRender();
                }
            }))).Checked = Render.IsRendering;

            ghSettingsMenu.Items.Add("Open RENDEROUTPUT Folder", null, new EventHandler((ob, ev) =>
            {
                Process.Start(Path.Combine(RtcCore.RtcDir, "RENDEROUTPUT"));
            }));

            ghSettingsMenu.Items.Add(new ToolStripSeparator());

            ghSettingsMenu.Items.Add(new ToolStripLabel("Render Type")
            {
                Font = new Font("Segoe UI", 12)
            });

            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("WAV", null, new EventHandler((ob, ev) =>
            {
                Render.RenderType = Render.RENDERTYPE.WAV;
            }))).Checked = Render.RenderType == Render.RENDERTYPE.WAV;
            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("AVI", null, new EventHandler((ob, ev) =>
            {
                Render.RenderType = Render.RENDERTYPE.AVI;
            }))).Checked = Render.RenderType == Render.RENDERTYPE.AVI;
            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("MPEG", null, new EventHandler((ob, ev) =>
            {
                Render.RenderType = Render.RENDERTYPE.MPEG;
            }))).Checked = Render.RenderType == Render.RENDERTYPE.MPEG;

            ghSettingsMenu.Items.Add(new ToolStripSeparator());

            ghSettingsMenu.Items.Add(new ToolStripLabel("Behaviors")
            {
                Font = new Font("Segoe UI", 12)
            });

            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Render file at load", null, new EventHandler((ob, ev) =>
            {
                Render.RenderAtLoad = Render.RenderAtLoad ^= true;
            }))).Checked = Render.RenderAtLoad;

            ghSettingsMenu.Show(this, locate);
        }
    }

    public enum GlitchHarvesterMode
    {
        CORRUPT,
        INJECT,
        ORIGINAL,
        MERGE,
    }
}
