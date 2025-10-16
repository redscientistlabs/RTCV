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
    using System.Timers;

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

            bool killswitchWasEnabled = AutoKillSwitch.Enabled;

            // If the stockpile entry is from a different emulator, close the current one and wait until the new one has connected
            if (!String.Equals(StockpileManagerUISide.CurrentStashkey.EmuVer, new DirectoryInfo(RtcCore.EmuDir).Name, StringComparison.OrdinalIgnoreCase))
            {
                if (!(await VanguardImplementation.SwapImplementation(StockpileManagerUISide.CurrentStashkey.EmuVer)))
                    return;
            }

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

            logger.Trace("Unlocking Interface");
            UICore.UnlockInterface();
            logger.Trace("Load done");

            AutoKillSwitch.Enabled = killswitchWasEnabled;

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

                if ((string)AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME] != "" && (!(AllSpec.UISpec[UISPEC.SELECTEDDOMAINS] is string[] domains) || domains.Length == 0))
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
                            MessageBox.Show("The Glitch Harvester could not perform the INJECT action\n\nHave you made a corruption yet?");
     
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
                            MessageBox.Show("The Glitch Harvester could not perform the ORIGINAL action\n\nHave you made a corruption yet?");
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
            if (e.Button != MouseButtons.Right) return;
            
            Point locate = e.GetMouseLocation(sender);

            new ContextMenuBuilder()
                .AddItem("Blast + Send RAW To Stash", (ob, ev) => BlastRawStash())
                .AddItem("Corrupt", async (ob, ev) => await Task.Run(() => CorruptWithMode(GlitchHarvesterMode.CORRUPT)))
                .AddItem("Inject", async (ob, ev) => await Task.Run(() => CorruptWithMode(GlitchHarvesterMode.INJECT)))
                .AddItem("Original", async (ob, ev) => await Task.Run(() => CorruptWithMode(GlitchHarvesterMode.ORIGINAL)))
                .Build()
                .Show(this, locate);
            
            return;
            
            void CorruptWithMode(GlitchHarvesterMode mode)
            {
                ghModeStore = ghMode;
                ghMode = mode;
                S.GET<GlitchHarvesterBlastForm>().Corrupt(sender, e);
                ghMode = ghModeStore;
                RedrawActionUI();
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
                new ContextMenuBuilder().AddItem("Configure Reroll", (ob, ev) =>
                {
                    S.GET<CoreForm>().OpenSettings(null, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
                    S.GET<SettingsForm>().lbForm.SetFocusedForm(S.GET<SettingsCorruptForm>());
                    S.GET<CoreForm>().BringToFront();
                }).Build().Show(this, locate);
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
            new ContextMenuBuilder()
                .AddHeader("Glitch Harvester Mode")
                .AddItem("Corrupt", (ob, ev) =>
                {
                    ghMode = GlitchHarvesterMode.CORRUPT;
                    RedrawActionUI();
                }, isChecked: ghMode == GlitchHarvesterMode.CORRUPT)
                .AddItem("Inject", (ob, ev) =>
                {
                    ghMode = GlitchHarvesterMode.INJECT;
                    RedrawActionUI();
                }, isChecked: ghMode == GlitchHarvesterMode.INJECT)
                .AddItem("Original", (ob, ev) =>
                {
                    ghMode = GlitchHarvesterMode.ORIGINAL;
                    RedrawActionUI();
                }, isChecked: ghMode == GlitchHarvesterMode.ORIGINAL)
                .AddSeparator()
                .AddHeader("Behaviors")
                .AddItem("Auto-Load State", (ob, ev) =>
                {
                    loadBeforeOperation = !loadBeforeOperation;
                    RedrawActionUI();
                }, isChecked: loadBeforeOperation)
                .AddItem("Load on Select", (ob, ev) =>
                {
                    LoadOnSelect = !LoadOnSelect;
                    RedrawActionUI();
                }, isChecked: LoadOnSelect)
                .AddItem("Stash Results", (ob, ev) =>
                {
                    StockpileManagerUISide.StashAfterOperation = !StockpileManagerUISide.StashAfterOperation;
                    RedrawActionUI();
                }, isChecked: StockpileManagerUISide.StashAfterOperation)
                .AddItem("Load Stash Items When Selected With Arrows", (ob, ev) =>
                {
                    S.GET<StashHistoryForm>().LoadWhenSelectedWithArrows = Params.ToggleParam("LOAD_STASH_ON_ARROW_CLICK");
                    RedrawActionUI();
                }, isChecked: Params.IsParamSet("LOAD_STASH_ON_ARROW_CLICK"))
                .AddItem("Compress Savestates", (ob, ev) =>
                {
                    Params.ToggleParam("COMPRESS_SAVESTATES");
                    RedrawActionUI();
                }, isChecked: Params.IsParamSet("COMPRESS_SAVESTATES"))
                .Build()
                .Show(this, locate);
        }

        private void RenderOutput(object sender, MouseEventArgs e)
        {
            Point locate = e.GetMouseLocation(sender);
            
            new ContextMenuBuilder()
                .AddHeader("Render Output")
                .AddItem("Start/Stop Rendering", (ob, ev) =>
                {
                    if (Render.IsRendering)
                    {
                        Render.StopRender();
                    }
                    else
                    {
                        Render.StartRender();
                    }
                }, isChecked: Render.IsRendering)
                .AddItem("Open RENDEROUTPUT Folder", (ob, ev) =>
                {
                    Process.Start(Path.Combine(RtcCore.RtcDir, "RENDEROUTPUT"));
                })
                .AddSeparator()
                .AddHeader("Render Type")
                .AddItem("WAV", (ob, ev) =>
                {
                    Render.RenderType = Render.RENDERTYPE.WAV;
                }, isChecked: Render.RenderType == Render.RENDERTYPE.WAV)
                .AddItem("AVI", (ob, ev) =>
                {
                    Render.RenderType = Render.RENDERTYPE.AVI;
                }, isChecked: Render.RenderType == Render.RENDERTYPE.AVI)
                .AddItem("MPEG", (ob, ev) =>
                {
                    Render.RenderType = Render.RENDERTYPE.MPEG;
                }, isChecked: Render.RenderType == Render.RENDERTYPE.MPEG)
                .AddSeparator()
                .AddHeader("Behaviors")
                .AddItem("Render File on Load", (ob, ev) =>
                {
                    Render.RenderAtLoad = !Render.RenderAtLoad;
                }, isChecked: Render.RenderAtLoad)
                .Build()
                .Show(this, locate);
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
