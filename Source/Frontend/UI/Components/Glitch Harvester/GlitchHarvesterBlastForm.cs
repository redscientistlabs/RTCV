namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using CorruptCore;
    using NetCore;
    using RTCV.Common;
    using Modular;
    using System.Threading;
    using System.Threading.Tasks;
    using RTCV.NetCore.Enums;

    public partial class GlitchHarvesterBlastForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public bool MergeMode { get; private set; } = false;
        public GlitchHarvesterMode ghMode { get; set; } = GlitchHarvesterMode.CORRUPT; //Current Glitch Harvester mode
        public GlitchHarvesterMode ghModeStore { get; set; } = GlitchHarvesterMode.CORRUPT; //Temporary Variable used for borrowing different corruption methods
        public bool LoadOnSelect { get; set; } = true;
        public bool loadBeforeOperation { get; set; } = true;

        private Color? originalRenderOutputButtonColor = null;
        private bool updatingBackColor = false;

        private bool isCorruptionApplied;
        public bool IsCorruptionApplied
        {
            get => isCorruptionApplied;
            set
            {
                this.UpdateBlastToggleColor(value);

                isCorruptionApplied = value;
            }
        }

        public GlitchHarvesterBlastForm()
        {
            InitializeComponent();

            PopoutAllowed = true;

            //cbRenderType.SelectedIndex = 0;

            //Registers the drag and drop with the blast editor form
            AllowDrop = true;
            this.DragEnter += OnDragEnter;
            this.DragDrop += OnDragDrop;
            this.btnBlastToggle.BackColorChanged += (s, e) =>
            {
                if (!this.updatingBackColor)
                {
                    this.UpdateBlastToggleColor(this.IsCorruptionApplied);
                }
            };
        }

        private void UpdateBlastToggleColor(bool value)
        {
            this.updatingBackColor = true;
            
            if (value)
            {
                this.btnBlastToggle.BackColor = Color.FromArgb(224, 128, 128);
                this.btnBlastToggle.ForeColor = Color.Black;
                this.btnBlastToggle.Text = "BlastLayer : ON";

                S.GET<StockpilePlayerForm>().btnBlastToggle.BackColor = Color.FromArgb(224, 128, 128);
                S.GET<StockpilePlayerForm>().btnBlastToggle.ForeColor = Color.Black;
                S.GET<StockpilePlayerForm>().btnBlastToggle.Text = "BlastLayer : ON     (Attempts to uncorrupt/recorrupt in real-time)";

                S.GET<SimpleModeForm>().btnBlastToggle.BackColor = Color.FromArgb(224, 128, 128);
                S.GET<SimpleModeForm>().btnBlastToggle.ForeColor = Color.Black;
                S.GET<SimpleModeForm>().btnBlastToggle.Text = "BlastLayer : ON     (Attempts to uncorrupt/recorrupt in real-time)";
            }
            else
            {
                this.btnBlastToggle.BackColor = S.GET<CoreForm>().btnLogo.BackColor;
                this.btnBlastToggle.ForeColor = Color.White;
                this.btnBlastToggle.Text = "BlastLayer : OFF";

                S.GET<StockpilePlayerForm>().btnBlastToggle.BackColor = S.GET<CoreForm>().btnLogo.BackColor;
                S.GET<StockpilePlayerForm>().btnBlastToggle.ForeColor = Color.White;
                S.GET<StockpilePlayerForm>().btnBlastToggle.Text = "BlastLayer : OFF    (Attempts to uncorrupt/recorrupt in real-time)";

                S.GET<SimpleModeForm>().btnBlastToggle.BackColor = S.GET<CoreForm>().btnLogo.BackColor;
                S.GET<SimpleModeForm>().btnBlastToggle.ForeColor = Color.White;
                S.GET<SimpleModeForm>().btnBlastToggle.Text = "BlastLayer : OFF    (Attempts to uncorrupt/recorrupt in real-time)";
            }

            this.updatingBackColor = false;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var f in files)
            {
                if (f.Contains(".bl"))
                {
                    BlastLayer bl = BlastTools.LoadBlastLayerFromFile(f);
                    var newStashKey = new StashKey(RtcCore.GetRandomKey(), null, bl);
                    S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = StockpileManagerUISide.ApplyStashkey(newStashKey, false, false);
                }
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        public void OneTimeExecute()
        {
            logger.Trace("Entering OneTimeExecute()");
            //Disable autocorrupt
            S.GET<CoreForm>().AutoCorrupt = false;

            bool killswitchWasEnabled = AutoKillSwitch.Enabled;
            // If the stockpile entry is from a different emulator, close the current one and wait until the new one has connected
            if (StockpileManagerUISide.CurrentStashkey.EmuVer != new DirectoryInfo(RtcCore.EmuDir).Name)
            {
                logger.Trace("different emulator found, switching");

                AutoKillSwitch.Enabled = false;
                UICore.isSwapping = true;
                
                logger.Trace("Blocking UI");
                UICore.LockInterface(false, true);
                logger.Trace("UI Blocked");

                S.GET<SaveProgressForm>().Dock = DockStyle.Fill;
                this.ParentCanvas?.OpenSubForm(S.GET<SaveProgressForm>());
                RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs($"Switching from " + new DirectoryInfo(RtcCore.EmuDir).Name + " to " + StockpileManagerUISide.CurrentStashkey.EmuVer, 0));

                LocalNetCoreRouter.Route(NetCore.Endpoints.Vanguard, NetCore.Commands.Remote.EventCloseEmulator);
                VanguardImplementation.Shutdown();

                //UICore.FirstConnect = true;
                CorruptCore.RtcCore.EmuDir = Path.Combine(Path.Combine(new DirectoryInfo(RtcCore.RtcDir).Parent.Parent.FullName, StockpileManagerUISide.CurrentStashkey.EmuVer));

                logger.Trace("Starting the new process");
                var oldEmuDir = CorruptCore.RtcCore.EmuDir;
                var info = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    WorkingDirectory = oldEmuDir,
                    FileName = Path.Combine(oldEmuDir, "RESTARTDETACHEDRTC.bat"),
                };

                if (!File.Exists(info.FileName))
                {
                    MessageBox.Show($"Couldn't find {info.FileName}! Killswitch will not work.");

                    this.ParentCanvas?.CloseSubForm();
                    logger.Trace("Unlocking Interface");
                    UICore.UnlockInterface();
                    logger.Trace("Load cancelled");

                    AutoKillSwitch.Enabled = killswitchWasEnabled;
                    UICore.isSwapping = false;

                    return;
                }

                Process.Start(info);
                VanguardImplementation.StartServer();
                var previous_status = VanguardImplementation.connector.netConn.status;
                var reconnected = false;
                while (!reconnected)
                {
                    if (previous_status != VanguardImplementation.connector.netConn.status)
                    {
                        if (VanguardImplementation.connector.netConn.status == NetworkStatus.CONNECTED)
                        {
                            reconnected = true;
                        }
                        else
                        {
                            previous_status = VanguardImplementation.connector.netConn.status;
                        }
                    }
                    logger.Trace("sleeping");
                    Thread.Sleep(250);
                }

                RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs($"Loading stockpile entry", 50));
            }

            if (ghMode == GlitchHarvesterMode.CORRUPT)
            {
                IsCorruptionApplied = StockpileManagerUISide.ApplyStashkey(StockpileManagerUISide.CurrentStashkey, loadBeforeOperation);
            }
            else if (ghMode == GlitchHarvesterMode.INJECT)
            {
                IsCorruptionApplied = StockpileManagerUISide.InjectFromStashkey(StockpileManagerUISide.CurrentStashkey, loadBeforeOperation);
                S.GET<StashHistoryForm>().RefreshStashHistory();
            }
            else if (ghMode == GlitchHarvesterMode.ORIGINAL)
            {
                IsCorruptionApplied = StockpileManagerUISide.OriginalFromStashkey(StockpileManagerUISide.CurrentStashkey);
            }

            if (Render.RenderAtLoad && loadBeforeOperation)
            {
                Render.StartRender();
            }
            else
            {
                Render.StopRender();
            }

            RtcCore.OnProgressBarUpdate(this, new ProgressBarEventArgs($"Done", 100));

            this.ParentCanvas?.CloseSubForm();
            logger.Trace("Unlocking Interface");
            UICore.UnlockInterface();
            logger.Trace("Load done");

            AutoKillSwitch.Enabled = killswitchWasEnabled;
            UICore.isSwapping = false;

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

        public void Corrupt(object sender, EventArgs e)
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

                if (!(AllSpec.UISpec[UISPEC.SELECTEDDOMAINS] is string[] domains) || domains.Length == 0)
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

                    IsCorruptionApplied = StockpileManagerUISide.MergeStashkeys(sks);

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
                    IsCorruptionApplied = StockpileManagerUISide.Corrupt(loadBeforeOperation);
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
                            MessageBox.Show("The Glitch Harvester could not perform the INJECT action\n\nHave you made a corruption yet?");
     
                        }
                        
                    }

                    S.GET<StashHistoryForm>().DontLoadSelectedStash = true;

                    IsCorruptionApplied = StockpileManagerUISide.InjectFromStashkey(StockpileManagerUISide.CurrentStashkey, loadBeforeOperation);
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
                            MessageBox.Show("The Glitch Harvester could not perform the ORIGINAL action\n\nHave you made a corruption yet?");
                        }
                    }

                    S.GET<StashHistoryForm>().DontLoadSelectedStash = true;
                    IsCorruptionApplied = StockpileManagerUISide.OriginalFromStashkey(StockpileManagerUISide.CurrentStashkey);
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
                columnsMenu.Items.Add("Corrupt", null, new EventHandler((ob, ev) =>
                {
                    ghModeStore = ghMode;
                    ghMode = GlitchHarvesterMode.CORRUPT;
                    S.GET<GlitchHarvesterBlastForm>().Corrupt(sender, e);
                    ghMode = ghModeStore;
                    RedrawActionUI();

                }));
                columnsMenu.Show(this, locate);
                columnsMenu.Items.Add("Inject", null, new EventHandler((ob, ev) =>
                {
                    ghModeStore = ghMode;
                    ghMode = GlitchHarvesterMode.INJECT;
                    S.GET<GlitchHarvesterBlastForm>().Corrupt(sender, e);
                    ghMode = ghModeStore;
                    RedrawActionUI();
                }));
                columnsMenu.Show(this, locate);
                columnsMenu.Items.Add("Original", null, new EventHandler((ob, ev) =>
                {
                    ghModeStore = ghMode;
                    ghMode = GlitchHarvesterMode.ORIGINAL;
                    S.GET<GlitchHarvesterBlastForm>().Corrupt(sender, e);
                    ghMode = ghModeStore;
                    RedrawActionUI();
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

        public void RerollSelected(object sender, EventArgs e)
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
                    var currentBl = StockpileManagerUISide.CurrentStashkey.BlastLayer;
                    //reroll on Emu Side always
                    var newBl = LocalNetCoreRouter.QueryRoute<BlastLayer>(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.RerollBlastLayer, currentBl, true);
                    StockpileManagerUISide.CurrentStashkey.BlastLayer = newBl;

                    //StockpileManagerUISide.CurrentStashkey.BlastLayer.Reroll();

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

                    IsCorruptionApplied = StockpileManagerUISide.ApplyStashkey(StockpileManagerUISide.CurrentStashkey);
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
            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Load stash items when selected with arrows", null, new EventHandler((ob, ev) =>
            {
                S.GET<StashHistoryForm>().LoadWhenSelectedWithArrows = Params.ToggleParam("LOAD_STASH_ON_ARROW_CLICK");
                RedrawActionUI();
            }))).Checked = Params.IsParamSet("LOAD_STASH_ON_ARROW_CLICK");
            ((ToolStripMenuItem)ghSettingsMenu.Items.Add("Compress savestates", null, new EventHandler((ob, ev) =>
            {
                Params.ToggleParam("COMPRESS_SAVESTATES");
                RedrawActionUI();
            }))).Checked = Params.IsParamSet("COMPRESS_SAVESTATES");

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

        private void btnNewBlastLayerEditor_Click(object sender, EventArgs e)
        {
            BlastEditorForm.OpenBlastEditor();
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
