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

    public partial class RTC_GlitchHarvesterBlast_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public bool MergeMode = false;
        public GlitchHarvesterMode ghMode = GlitchHarvesterMode.CORRUPT;

        public bool LoadOnSelect = true;
        public bool loadBeforeOperation = true;

        public Color? originalRenderOutputButtonColor = null;

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

                    S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.BackColor = Color.FromArgb(224, 128, 128);
                    S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.ForeColor = Color.Black;
                    S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.Text = "BlastLayer : ON     (Attempts to uncorrupt/recorrupt in real-time)";

                    S.GET<RTC_SimpleMode_Form>().btnBlastToggle.BackColor = Color.FromArgb(224, 128, 128);
                    S.GET<RTC_SimpleMode_Form>().btnBlastToggle.ForeColor = Color.Black;
                    S.GET<RTC_SimpleMode_Form>().btnBlastToggle.Text = "BlastLayer : ON     (Attempts to uncorrupt/recorrupt in real-time)";
                }
                else
                {
                    btnBlastToggle.BackColor = S.GET<UI_CoreForm>().btnLogo.BackColor;
                    btnBlastToggle.ForeColor = Color.White;
                    btnBlastToggle.Text = "BlastLayer : OFF";

                    S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.BackColor = S.GET<UI_CoreForm>().btnLogo.BackColor;
                    S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.ForeColor = Color.White;
                    S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.Text = "BlastLayer : OFF    (Attempts to uncorrupt/recorrupt in real-time)";

                    S.GET<RTC_SimpleMode_Form>().btnBlastToggle.BackColor = S.GET<UI_CoreForm>().btnLogo.BackColor;
                    S.GET<RTC_SimpleMode_Form>().btnBlastToggle.ForeColor = Color.White;
                    S.GET<RTC_SimpleMode_Form>().btnBlastToggle.Text = "BlastLayer : OFF    (Attempts to uncorrupt/recorrupt in real-time)";
                }

                isCorruptionApplied = value;
            }
        }

        public RTC_GlitchHarvesterBlast_Form()
        {
            InitializeComponent();

            popoutAllowed = true;
            this.undockedSizable = false;

            //cbRenderType.SelectedIndex = 0;

            //Registers the drag and drop with the blast edirot form
            AllowDrop = true;
            this.DragEnter += RTC_GlitchHarvesterBlast_Form_DragEnter;
            this.DragDrop += RTC_GlitchHarvesterBlast_Form_DragDrop;
        }

        private void RTC_GlitchHarvesterBlast_Form_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var f in files)
            {
                if (f.Contains(".bl"))
                {
                    BlastLayer bl = BlastTools.LoadBlastLayerFromFile(f);
                    var newStashKey = new StashKey(RtcCore.GetRandomKey(), null, bl);
                    S.GET<RTC_GlitchHarvesterBlast_Form>().IsCorruptionApplied = StockpileManager_UISide.ApplyStashkey(newStashKey, false, false);
                }
            }
        }

        private void RTC_GlitchHarvesterBlast_Form_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        public void OneTimeExecute()
        {
            logger.Trace("Entering OneTimeExecute()");
            //Disable autocorrupt
            S.GET<UI_CoreForm>().AutoCorrupt = false;

            if (ghMode == GlitchHarvesterMode.CORRUPT)
            {
                IsCorruptionApplied = StockpileManager_UISide.ApplyStashkey(StockpileManager_UISide.CurrentStashkey, loadBeforeOperation);
            }
            else if (ghMode == GlitchHarvesterMode.INJECT)
            {
                IsCorruptionApplied = StockpileManager_UISide.InjectFromStashkey(StockpileManager_UISide.CurrentStashkey, loadBeforeOperation);
                S.GET<RTC_StashHistory_Form>().RefreshStashHistory();
            }
            else if (ghMode == GlitchHarvesterMode.ORIGINAL)
            {
                IsCorruptionApplied = StockpileManager_UISide.OriginalFromStashkey(StockpileManager_UISide.CurrentStashkey);
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
            if (S.GET<RTC_StockpileManager_Form>().dgvStockpile.SelectedRows.Count > 1)
            {
                MergeMode = true;
                btnCorrupt.Text = "  Merge";
                S.GET<RTC_StockpileManager_Form>().btnRenameSelected.Visible = false;
                S.GET<RTC_StockpileManager_Form>().btnRemoveSelectedStockpile.Text = "  Remove Items";
            }
            else
            {
                MergeMode = false;
                S.GET<RTC_StockpileManager_Form>().btnRenameSelected.Visible = true;
                S.GET<RTC_StockpileManager_Form>().btnRemoveSelectedStockpile.Text = "  Remove Item";

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

        public void btnCorrupt_Click(object sender, EventArgs e)
        {
            logger.Trace("btnCorrupt Clicked");

            if (sender != null)
            {
                if (!(btnCorrupt.Visible || AllSpec.VanguardSpec[VSPEC.REPLACE_MANUALBLAST_WITH_GHCORRUPT] != null && S.GET<UI_CoreForm>().btnManualBlast.Visible))
                {
                    return;
                }
            }

            try
            {
                SetBlastButtonVisibility(false);

                var domains = RTCV.NetCore.AllSpec.UISpec["SELECTEDDOMAINS"] as string[];
                if (domains == null || domains.Length == 0)
                {
                    MessageBox.Show("Can't corrupt with no domains selected.");
                    return;
                }

                //Shut off autocorrupt if it's on.
                //Leave this check here so we don't wastefully update the spec
                if (S.GET<UI_CoreForm>().AutoCorrupt)
                {
                    S.GET<UI_CoreForm>().AutoCorrupt = false;
                }

                StashKey psk = StockpileManager_UISide.CurrentSavestateStashKey;

                if (MergeMode)
                {
                    List<StashKey> sks = new List<StashKey>();

                    //Reverse before merging because DataGridView selectedrows is backwards for some odd reason
                    var reversed = S.GET<RTC_StockpileManager_Form>().dgvStockpile.SelectedRows.Cast<DataGridViewRow>().Reverse();
                    foreach (DataGridViewRow row in reversed)
                    {
                        sks.Add((StashKey)row.Cells[0].Value);
                    }

                    IsCorruptionApplied = StockpileManager_UISide.MergeStashkeys(sks);

                    S.GET<RTC_StashHistory_Form>().RefreshStashHistorySelectLast();
                    //lbStashHistory.TopIndex = lbStashHistory.Items.Count - 1;

                    return;
                }

                if (ghMode == GlitchHarvesterMode.CORRUPT)
                {
                    string romFilename = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME];

                    if (romFilename?.Contains("|") ?? false)
                    {
                        MessageBox.Show($"The Glitch Harvester attempted to corrupt a game bound to the following file:\n{romFilename}\n\nIt cannot be processed because the rom seems to be inside a Zip Archive\n(Bizhawk returned a filename with the chracter | in it)");
                        return;
                    }

                    S.GET<RTC_StashHistory_Form>().DontLoadSelectedStash = true;
                    IsCorruptionApplied = StockpileManager_UISide.Corrupt(loadBeforeOperation);
                    S.GET<RTC_StashHistory_Form>().RefreshStashHistorySelectLast();
                }
                else if (ghMode == GlitchHarvesterMode.INJECT)
                {
                    if (StockpileManager_UISide.CurrentStashkey == null)
                    {
                        if (StockpileManager_UISide.LastStashkey != null)
                            StockpileManager_UISide.CurrentStashkey = StockpileManager_UISide.LastStashkey;
                        else
                            throw new Exception("Inject tried to fetch the LastStashkey backup but this one was also null! Try to re-load your savestate and then re-select your corruption in the stash history or stockpile. That might fix it. If it still doesn't work after that, report to the devs pls");
                    }

                    S.GET<RTC_StashHistory_Form>().DontLoadSelectedStash = true;

                    IsCorruptionApplied = StockpileManager_UISide.InjectFromStashkey(StockpileManager_UISide.CurrentStashkey, loadBeforeOperation);
                    S.GET<RTC_StashHistory_Form>().RefreshStashHistorySelectLast();
                }
                else if (ghMode == GlitchHarvesterMode.ORIGINAL)
                {
                    if (StockpileManager_UISide.CurrentStashkey == null)
                    {
                        if (StockpileManager_UISide.LastStashkey != null)
                            StockpileManager_UISide.CurrentStashkey = StockpileManager_UISide.LastStashkey;
                        else
                            throw new Exception("CurrentStashkey in original was somehow null! Report this to the devs and tell them how you caused this.");
                    }

                    S.GET<RTC_StashHistory_Form>().DontLoadSelectedStash = true;
                    IsCorruptionApplied = StockpileManager_UISide.OriginalFromStashkey(StockpileManager_UISide.CurrentStashkey);
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

        private void btnOpenRenderFolder_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(CorruptCore.RtcCore.RtcDir, "RENDEROUTPUT"));
        }

        private void BlastRawStash()
        {
            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.MANUALBLAST, true);
            btnSendRaw_Click(null, null);
        }

        private void btnCorrupt_MouseDown(object sender, MouseEventArgs e)
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

        public void btnSendRaw_Click(object sender, EventArgs e)
        {
            if (!btnSendRaw.Visible)
            {
                return;
            }

            try
            {
                SetBlastButtonVisibility(false);

                string romFilename = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME];
                if (romFilename == null)
                {
                    return;
                }

                if (romFilename.Contains("|"))
                {
                    MessageBox.Show($"The Glitch Harvester attempted to corrupt a game bound to the following file:\n{romFilename}\n\nIt cannot be processed because the rom seems to be inside a Zip Archive\n(Bizhawk returned a filename with the chracter | in it)");
                    return;
                }

                StashKey sk = LocalNetCoreRouter.QueryRoute<StashKey>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_KEY_GETRAWBLASTLAYER, true);

                StockpileManager_UISide.CurrentStashkey = sk;
                StockpileManager_UISide.StashHistory.Add(StockpileManager_UISide.CurrentStashkey);

                S.GET<RTC_StashHistory_Form>().DontLoadSelectedStash = true;
                S.GET<RTC_StashHistory_Form>().RefreshStashHistorySelectLast();
                S.GET<RTC_StashHistory_Form>().DontLoadSelectedStash = true;
                S.GET<RTC_StockpileManager_Form>().dgvStockpile.ClearSelection();
                S.GET<RTC_StashHistory_Form>().DontLoadSelectedStash = false;
            }
            finally
            {
                SetBlastButtonVisibility(true);
            }
        }

        public void btnBlastToggle_Click(object sender, EventArgs e)
        {
            if (StockpileManager_UISide.CurrentStashkey?.BlastLayer?.Layer == null || StockpileManager_UISide.CurrentStashkey?.BlastLayer?.Layer.Count == 0)
            {
                IsCorruptionApplied = false;
                return;
            }

            if (!IsCorruptionApplied)
            {
                IsCorruptionApplied = true;

                LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_SET_APPLYCORRUPTBL, true);
            }
            else
            {
                IsCorruptionApplied = false;

                LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_SET_APPLYUNCORRUPTBL, true);
                LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
            }
        }

        private void btnRerollSelected_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = this.PointToClient(Cursor.Position);
                ContextMenuStrip rerollMenu = new ContextMenuStrip();
                rerollMenu.Items.Add("Configure Reroll", null, new EventHandler((ob, ev) =>
                {
                    S.GET<UI_CoreForm>().btnSettings_MouseDown(null, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
                    S.GET<RTC_Settings_Form>().lbForm.SetFocusedForm(S.GET<RTC_SettingsCorrupt_Form>());
                    S.GET<UI_CoreForm>().BringToFront();
                }));

                rerollMenu.Show(this, locate);
            }
        }

        public void btnRerollSelected_Click(object sender, EventArgs e)
        {
            if (!btnRerollSelected.Visible)
            {
                return;
            }

            try
            {
                SetBlastButtonVisibility(false);

                if (S.GET<RTC_StashHistory_Form>().lbStashHistory.SelectedIndex != -1)
                {
                    StockpileManager_UISide.CurrentStashkey = (StashKey)StockpileManager_UISide.StashHistory[S.GET<RTC_StashHistory_Form>().lbStashHistory.SelectedIndex].Clone();
                }
                else if (S.GET<RTC_StockpileManager_Form>().dgvStockpile.SelectedRows.Count != 0 && S.GET<RTC_StockpileManager_Form>().dgvStockpile.SelectedRows[0].Cells[0].Value != null)
                {
                    StockpileManager_UISide.CurrentStashkey = (StashKey)(S.GET<RTC_StockpileManager_Form>().dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey)?.Clone();
                    //StockpileManager_UISide.unsavedEdits = true;
                }
                else
                {
                    return;
                }

                if (StockpileManager_UISide.CurrentStashkey != null)
                {
                    StockpileManager_UISide.CurrentStashkey.BlastLayer.Reroll();

                    if (StockpileManager_UISide.AddCurrentStashkeyToStash())
                    {
                        S.GET<RTC_StockpileManager_Form>().dgvStockpile.ClearSelection();
                        S.GET<RTC_StashHistory_Form>()
                            .RefreshStashHistory();
                        S.GET<RTC_StashHistory_Form>()
                            .lbStashHistory.ClearSelected();
                        S.GET<RTC_StashHistory_Form>()
                            .DontLoadSelectedStash = true;
                        S.GET<RTC_StashHistory_Form>()
                            .lbStashHistory.SelectedIndex = S.GET<RTC_StashHistory_Form>()
                            .lbStashHistory.Items.Count - 1;
                    }

                    IsCorruptionApplied = StockpileManager_UISide.ApplyStashkey(StockpileManager_UISide.CurrentStashkey);
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
                S.GET<UI_CoreForm>().btnManualBlast.Visible = visible;
            }
        }

        private void btnGlitchHarvesterSettings_MouseDown(object sender, MouseEventArgs e)
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
                StockpileManager_UISide.StashAfterOperation = StockpileManager_UISide.StashAfterOperation ^= true;
                RedrawActionUI();
            }))).Checked = StockpileManager_UISide.StashAfterOperation;

            ghSettingsMenu.Show(this, locate);
        }

        private void btnRenderOutput_MouseDown(object sender, MouseEventArgs e)
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
                Process.Start(Path.Combine(CorruptCore.RtcCore.RtcDir, "RENDEROUTPUT"));
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
