using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;
using RTCV.NetCore;
using System.Diagnostics;

namespace RTCV.UI
{
	public partial class RTC_GlitchHarvesterBlast_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public GlitchHarvesterMode ghMode = GlitchHarvesterMode.CORRUPT;

        //delete me later on
        public Button btnRender = new Button();
        public CheckBox cbAutoLoadState = new CheckBox();
        public CheckBox cbLoadOnSelect = new CheckBox();
        public CheckBox cbRenderAtLoad = new CheckBox();
        public CheckBox cbStashCorrupted = new CheckBox();
        public ComboBox cbRenderType = new ComboBox();

        public bool AutoLoadState = true;
        public bool LoadOnSelect = true;
        public bool StashCorrupted = true;

        public bool RenderAtLoad = false;
        public string RenderType = "";


        public bool loadBeforeOperation = true;

        private bool isCorruptionApplied;
        public bool IsCorruptionApplied
        {
            get
            {
                return isCorruptionApplied;
            }
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
                }
                else
                {
                    btnBlastToggle.BackColor = S.GET<UI_CoreForm>().btnLogo.BackColor;
                    btnBlastToggle.ForeColor = Color.White;
                    btnBlastToggle.Text = "BlastLayer : OFF";

                    S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.BackColor = S.GET<UI_CoreForm>().btnLogo.BackColor;
                    S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.ForeColor = Color.White;
                    S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.Text = "BlastLayer : OFF    (Attempts to uncorrupt/recorrupt in real-time)";
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
        }

        public void OneTimeExecute()
        {
            //Disable autocorrupt
            S.GET<UI_CoreForm>().AutoCorrupt = false;

            if (ghMode == GlitchHarvesterMode.CORRUPT)
                IsCorruptionApplied = StockpileManager_UISide.ApplyStashkey(StockpileManager_UISide.CurrentStashkey, loadBeforeOperation);
            else if (ghMode == GlitchHarvesterMode.INJECT)
            {
                IsCorruptionApplied = StockpileManager_UISide.InjectFromStashkey(StockpileManager_UISide.CurrentStashkey, loadBeforeOperation);
                S.GET<RTC_StashHistory_Form>().RefreshStashHistory();
            }
            else if (ghMode == GlitchHarvesterMode.ORIGINAL)
                IsCorruptionApplied = StockpileManager_UISide.OriginalFromStashkey(StockpileManager_UISide.CurrentStashkey);

            if (StockpileManager_EmuSide.RenderAtLoad && loadBeforeOperation)
            {
                btnRender.Text = "Stop Render";
                btnRender.ForeColor = Color.OrangeRed;
            }
            else
            {
                btnRender.Text = "Start Render";
                btnRender.ForeColor = Color.White;
            }
        }

        public void RedrawActionUI()
        {
            // Merge tool and ui change
            if (S.GET<RTC_StockpileManager_Form>().dgvStockpile.SelectedRows.Count > 1)
            {
                ghMode = GlitchHarvesterMode.MERGE;
                btnCorrupt.Text = "  Merge";
                S.GET<RTC_StockpileManager_Form>().btnRenameSelected.Visible = false;
                S.GET<RTC_StockpileManager_Form>().btnRemoveSelectedStockpile.Text = "Remove Items";
            }
            else
            {
                S.GET<RTC_StockpileManager_Form>().btnRenameSelected.Visible = true;
                S.GET<RTC_StockpileManager_Form>().btnRemoveSelectedStockpile.Text = "Remove Item";

                if (ghMode == GlitchHarvesterMode.CORRUPT)
                    btnCorrupt.Text = "  Corrupt";
                else if (ghMode == GlitchHarvesterMode.INJECT)
                    btnCorrupt.Text = "  Inject";
                else if (ghMode == GlitchHarvesterMode.ORIGINAL)
                    btnCorrupt.Text = "  Original";
            }
        }

        public void btnCorrupt_Click(object sender, EventArgs e)
        {
            Console.WriteLine("btnCorrupt Clicked");
            if (!btnCorrupt.Visible)
                return;

            try
            {
                //Shut off autocorrupt if it's on.
                //Leave this check here so we don't wastefully update the spec
                if (S.GET<UI_CoreForm>().AutoCorrupt)
                    S.GET<UI_CoreForm>().AutoCorrupt = false;

                btnCorrupt.Visible = false;

                StashKey psk = StockpileManager_UISide.GetCurrentSavestateStashkey();

                if (ghMode == GlitchHarvesterMode.MERGE)
                {
                    List<StashKey> sks = new List<StashKey>();

                    //Reverse before merging because DataGridView selectedrows is backwards for some odd reason
                    var reversed = S.GET<RTC_StockpileManager_Form>().dgvStockpile.SelectedRows.Cast<DataGridViewRow>().Reverse();
                    foreach (DataGridViewRow row in reversed)
                        sks.Add((StashKey)row.Cells[0].Value);

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
                        throw new CustomException("CurrentStashkey in inject was somehow null! Report this to the devs and tell them how you caused this.", Environment.StackTrace);

                    S.GET<RTC_StashHistory_Form>().DontLoadSelectedStash = true;

                    IsCorruptionApplied = StockpileManager_UISide.InjectFromStashkey(StockpileManager_UISide.CurrentStashkey, loadBeforeOperation);
                    S.GET<RTC_StashHistory_Form>().RefreshStashHistorySelectLast();
                }
                else if (ghMode == GlitchHarvesterMode.ORIGINAL)
                {
                    if (StockpileManager_UISide.CurrentStashkey == null)
                        throw new CustomException("CurrentStashkey in original was somehow null! Report this to the devs and tell them how you caused this.", Environment.StackTrace);

                    S.GET<RTC_StashHistory_Form>().DontLoadSelectedStash = true;
                    IsCorruptionApplied = StockpileManager_UISide.OriginalFromStashkey(StockpileManager_UISide.CurrentStashkey);
                }

                if (StockpileManager_EmuSide.RenderAtLoad && loadBeforeOperation)
                {
                    btnRender.Text = "Stop Render";
                    btnRender.ForeColor = Color.OrangeRed;
                }
                else
                {
                    btnRender.Text = "Start Render";
                    btnRender.ForeColor = Color.White;
                }

                Console.WriteLine("Blast done");
            }
            finally
            {
                btnCorrupt.Visible = true;
            }
        }




        private void cbRenderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Render.setType(cbRenderType.SelectedItem.ToString());
        }

        private void btnOpenRenderFolder_Click(object sender, EventArgs e)
        {
            Process.Start(CorruptCore.CorruptCore.RtcDir + Path.DirectorySeparatorChar + "RENDEROUTPUT" + Path.DirectorySeparatorChar);
        }

        private void btnRender_Click(object sender, EventArgs e)
        {
            if (btnRender.Text == "Start Render")
            {
                if (Render.StartRender())
                {
                    btnRender.Text = "Stop Render";
                    btnRender.ForeColor = Color.OrangeRed;
                }
            }
            else
            {
                Render.StopRender();
                btnRender.Text = "Start Render";
                btnRender.ForeColor = Color.White;
            }
        }

        private void cbRenderAtLoad_CheckedChanged(object sender, EventArgs e)
        {
            StockpileManager_EmuSide.RenderAtLoad = cbRenderAtLoad.Checked;
        }

        private void BlastRawStash()
        {
            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.ASYNCBLAST, true);
            S.GET<RTC_GlitchHarvesterBlast_Form>().btnSendRaw_Click(null, null);
        }

        private void btnCorrupt_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

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
            try
            {
                btnSendRaw.Visible = false;


                string romFilename = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME];
                if (romFilename == null)
                    return;
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
                btnSendRaw.Visible = true;
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
                    S.GET<UI_CoreForm>().btnSettings_Click(null, null);
                    S.GET<RTC_Settings_Form>().lbForm.SetFocusedForm(S.GET<RTC_SettingsCorrupt_Form>());
                    S.GET<UI_CoreForm>().BringToFront();
                }));

                rerollMenu.Show(this, locate);
            }
        }

        public void btnRerollSelected_Click(object sender, EventArgs e)
        {
            

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
                return;

            if (StockpileManager_UISide.CurrentStashkey != null)
            {
                StockpileManager_UISide.CurrentStashkey.BlastLayer.Reroll();

                if (StockpileManager_UISide.AddCurrentStashkeyToStash())
                {
                    S.GET<RTC_StashHistory_Form>().RefreshStashHistory();
                    S.GET<RTC_StashHistory_Form>().lbStashHistory.ClearSelected();
                    S.GET<RTC_StashHistory_Form>().DontLoadSelectedStash = true;
                    S.GET<RTC_StashHistory_Form>().lbStashHistory.SelectedIndex = S.GET<RTC_StashHistory_Form>().lbStashHistory.Items.Count - 1;
                }

                StockpileManager_UISide.ApplyStashkey(StockpileManager_UISide.CurrentStashkey);
            }
        }

        private void cbAutoLoadState_CheckedChanged(object sender, EventArgs e)
        {
            loadBeforeOperation = cbAutoLoadState.Checked;
        }

        private void cbStashCorrupted_CheckedChanged(object sender, EventArgs e)
        {
            StockpileManager_UISide.StashAfterOperation = cbStashCorrupted.Checked;
        }

        private void RTC_GlitchHarvesterBlast_Form_Load(object sender, EventArgs e)
        {

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
