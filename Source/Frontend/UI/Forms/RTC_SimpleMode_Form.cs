namespace RTCV.UI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Components.Controls;
    using static RTCV.UI.UI_Extensions;

    public partial class RTC_SimpleMode_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public bool DontLoadSelectedStockpile = false;
        private PlatformType platform = PlatformType.CLASSIC;

        public bool DontUpdateSpec = false;

        public RTC_SimpleMode_Form()
        {
            InitializeComponent();
        }

        public void EnteringSimpleMode()
        {
            RTC_GlitchHarvesterIntensity_Form ghiForm = S.GET<RTC_GlitchHarvesterIntensity_Form>();
            ghiForm.AnchorToPanel(pnIntensity);
            //S.GET<UI_CoreForm>().btnEngineConfig.Visible = false;
            S.GET<UI_CoreForm>().btnEngineConfig.Text = " Simple Mode";
            S.GET<UI_CoreForm>().btnGlitchHarvester.Visible = false;
            S.GET<UI_CoreForm>().btnStockpilePlayer.Visible = false;
            S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = false;
            S.GET<UI_CoreForm>().btnManualBlast.Visible = false;

            if (rbClassicPlatforms.Checked)
            {
                rbClassicPlatforms_CheckedChanged(null, null);
            }

            if (rbModernPlatforms.Checked)
            {
                rbModernPlatforms_CheckedChanged(null, null);
            }

            NetCore.Params.SetParam("SIMPLE_MODE"); //Set RTC in Simple Mode
        }

        public void LeavingSimpleMode()
        {
            NetCore.Params.RemoveParam("SIMPLE_MODE"); //Set RTC in Normal Mode

            //S.GET<UI_CoreForm>().btnEngineConfig.Visible = true;
            S.GET<UI_CoreForm>().btnEngineConfig.Text = " Engine Config";
            S.GET<UI_CoreForm>().btnGlitchHarvester.Visible = true;
            S.GET<UI_CoreForm>().btnStockpilePlayer.Visible = true;
            S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = true;
            S.GET<UI_CoreForm>().btnManualBlast.Visible = true;

            S.GET<UI_CoreForm>().btnEngineConfig_Click(null, null);
        }

        private void RTC_SimpleMode_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void btnBlastToggle_Click(object sender, EventArgs e)
        {
            S.GET<RTC_GlitchHarvesterBlast_Form>().btnBlastToggle_Click(null, null);
        }

        private void btnManualBlast_Click(object sender, EventArgs e)
        {
            S.GET<UI_CoreForm>().btnManualBlast_Click(sender, e);
        }

        private void btnAutoCorrupt_Click(object sender, EventArgs e)
        {
            S.GET<UI_CoreForm>().btnAutoCorrupt_Click(sender, e);
            //btnAutoCorrupt.Text = S.GET<UI_CoreForm>().btnAutoCorrupt.Text;
        }

        private void btnCreateGhSavestate_Click(object sender, EventArgs e)
        {
            //Select first savestate slot if none is selected
            var selectedHolder = S.GET<RTC_SavestateManager_Form>().savestateList.selectedHolder;
            if (selectedHolder == null)
            {
                //Generate object sender and MouseEventArgs e data for the button click
                SavestateHolder holder = (SavestateHolder)S.GET<RTC_SavestateManager_Form>().savestateList.flowPanel.Controls[0];
                Button _sender = holder.btnSavestate;
                MouseEventArgs _e = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);

                //Click first GH Savestate
                S.GET<RTC_SavestateManager_Form>().savestateList.BtnSavestate_MouseDown(_sender, _e);
            }

            //Switch to Save
            S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad.Text = "SAVE";
            S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad.ForeColor = Color.OrangeRed;

            //Trigger Save button
            S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad_Click(null, null);
        }

        private void btnGlitchHarvesterCorrupt_Click(object sender, EventArgs e)
        {
            if (S.GET<RTC_StashHistory_Form>().lbStashHistory.Items.Count >= 20)
            {
                S.GET<RTC_StashHistory_Form>().RemoveFirstStashHistoryItem();
            }

            S.GET<RTC_GlitchHarvesterBlast_Form>().btnCorrupt_Click(null, null);
        }

        private void rbClassicPlatforms_CheckedChanged(object sender, EventArgs e)
        {
            SwitchPlatformType(PlatformType.CLASSIC);
        }

        private void rbModernPlatforms_CheckedChanged(object sender, EventArgs e)
        {
            SwitchPlatformType(PlatformType.MODERN);
        }

        public void SwitchPlatformType(PlatformType pt)
        {
            switch (pt)
            {
                case PlatformType.CLASSIC:
                    SelectNightmareEngine();
                    break;

                case PlatformType.MODERN:
                    SelectVectorEngine();
                    break;
            }

            S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = false;
            S.GET<UI_CoreForm>().btnManualBlast.Visible = false;

            platform = pt;

            gbEngineParameters.Visible = true;
            gbRealTimeCorruption.Visible = true;
            gbSimpleGlitchHarvester.Visible = true;

            btnBlastToggle.Visible = true;
        }

        public void Shuffle()
        {
            if (platform == PlatformType.CLASSIC)
            {
                ShuffleClassic();
            }
            else if (platform == PlatformType.MODERN)
            {
                ShuffleModern();
            }
        }

        public void ShuffleClassic()
        {
            //TODO: If vanguard implementation doesn't support real-time
            //SelectNightmareEngine();

            string originalText = lbEngineDescription.Text;

            Random RND = new Random((int)DateTime.Now.Ticks);
            int engineSelect = RND.Next(1, 5);

            switch (engineSelect)
            {
                case 1:
                    SelectNightmareEngine();
                    break;
                case 2:
                    SelectHellgenieEngine();
                    break;
                case 3:
                    SelectFreezeEngine();
                    break;
                case 4:
                    SelectDistortionEngine();
                    break;
                case 5:
                    SelectPipeEngine();
                    break;
            }

            if (originalText == lbEngineDescription.Text)
            {
                ShuffleClassic();
            }

            S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = false;
            S.GET<UI_CoreForm>().btnManualBlast.Visible = false;
        }

        public void ShuffleModern()
        {
            SelectComboBoxRandomItem(S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList);
            SelectComboBoxRandomItem(S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList);

            string limiter = S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.Text;
            string value = S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.Text;

            lbEngineDescription.Text = $"Auto-Selected Engine: Vector Engine\n" +
                                       $"Parameters: Limiter:{limiter} , Value:{value}\n" +
                                       $"\n" +
                                       $"This engine is made for corrupting 3d games\n" +
                                       $"and 2d games made for 3d-era consoles.";
        }

        public void SelectComboBoxRandomItem(ComboBox cb)
        {
            Random RND = new Random((int)DateTime.Now.Ticks);
            int nbItems = cb.Items.Count;
            int SelectIndex = RND.Next(0, nbItems - 1);
            cb.SelectedIndex = SelectIndex;
        }

        public void SelectEngineByName(string name)
        {
            int selectedEngineIndex = -1;

            ComboBox cb = S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine;

            for (int i = 0; i < cb.Items.Count; i++)
            {
                if (cb.Items[i].ToString() == name)
                {
                    selectedEngineIndex = i;
                    break;
                }
            }

            if (selectedEngineIndex == -1)
            {
                throw new Exception($"Could not load {name}");
            }

            cb.SelectedIndex = selectedEngineIndex;
        }

        public void ResetSession()
        {
            var ui = S.GET<UI_CoreForm>();
            if (ui.AutoCorrupt)
            {
                btnAutoCorrupt_Click(null, null);
            }

            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
        }

        public void SelectNightmareEngine()
        {
            ResetSession();
            SelectEngineByName("Nightmare Engine");
            SetInfiniteUnitVisibility(false);

            lbEngineDescription.Text = $"Auto-Selected Engine: Nightmare Engine\n" +
                                       $"\n" +
                                       $"This engine is made for corrupting 2d games.\n" +
                                       $"It generates garbage data and writes it \n" +
                                       $"to the game's memory.";
        }

        public void SelectHellgenieEngine()
        {
            ResetSession();
            SelectEngineByName("Hellgenie Engine");
            SetInfiniteUnitVisibility(true);

            lbEngineDescription.Text = $"Auto-Selected Engine: Hellgenie Engine\n" +
                                      $"\n" +
                                      $"This engine generates garbage data and then\n" +
                                      $"continuously writes it to the game's memory.";
        }

        public void SelectFreezeEngine()
        {
            ResetSession();
            SelectEngineByName("Freeze Engine");
            SetInfiniteUnitVisibility(true);

            lbEngineDescription.Text = $"Auto-Selected Engine: Freeze Engine\n" +
                                       $"\n" +
                                       $"This engine randomly selects addresses and then\n" +
                                       $"freezes their value in place.";
        }

        public void SelectDistortionEngine()
        {
            ResetSession();
            SelectEngineByName("Distortion Engine");
            SetInfiniteUnitVisibility(false);

            lbEngineDescription.Text = $"Auto-Selected Engine: Distortion Engine\n" +
                                       $"\n" +
                                       $"This engine randomly selects addresses and then\n" +
                                       $"backups their current value. It will then restore\n" +
                                       $"these values later to corrupt the game.";
        }

        public void SelectPipeEngine()
        {
            ResetSession();
            SelectEngineByName("Pipe Engine");
            SetInfiniteUnitVisibility(true);

            lbEngineDescription.Text = $"Auto-Selected Engine: Pipe Engine\n" +
                                       $"\n" +
                                       $"This engine randomly links memory adresses together,\n" +
                                       $"transporting the value of one to another.";
        }

        public void SelectVectorEngine()
        {
            ResetSession();
            SelectEngineByName("Vector Engine");
            SetInfiniteUnitVisibility(false);

            if (S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.Items.Count > 0)
            {
                S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.SelectedIndex = 0;
                S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.SelectedIndex = 0;
            }
            else
            {
                throw new Exception("No vector lists could be found. Your RTCV installation might be broken.");
            }

            string limiter = S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.Text;
            string value = S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.Text;

            lbEngineDescription.Text = $"Auto-Selected Engine: Vector Engine\n" +
                                       $"Parameters: Limiter:{limiter} , Value:{value}\n" +
                                       $"\n" +
                                       $"This engine is made for corrupting 3d games\n" +
                                       $"and 2d games made for 3d-era consoles (excluding PSX).";
        }

        public void SetInfiniteUnitVisibility(bool visible)
        {
            btnClearInfiniteUnits.Visible = visible;
            cbClearRewind.Visible = visible;
            lbMaxUnits.Visible = visible;
            updownMaxInfiniteUnits.Visible = visible;
        }

        private void btnShuffleAlgorithm_Click(object sender, EventArgs e)
        {
            Shuffle();
        }

        private void btnClearInfiniteUnits_Click(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
        }

        private void btnSwitchNormalMode_Click(object sender, EventArgs e)
        {
            LeavingSimpleMode();
            this.Hide();
        }

        private void btnLoadGhSavestate_Click(object sender, EventArgs e)
        {
            //Select first savestate slot if none is selected
            var selectedHolder = S.GET<RTC_SavestateManager_Form>().savestateList.selectedHolder;
            if (selectedHolder == null)
            {
                //Generate object sender and MouseEventArgs e data for the button click
                SavestateHolder holder = (SavestateHolder)S.GET<RTC_SavestateManager_Form>().savestateList.flowPanel.Controls[0];
                Button _sender = holder.btnSavestate;
                MouseEventArgs _e = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);

                //Click first GH Savestate
                S.GET<RTC_SavestateManager_Form>().savestateList.BtnSavestate_MouseDown(_sender, _e);
            }

            //Switch to Save
            S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad.Text = "LOAD";
            S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);

            //Trigger Save button
            S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad_Click(null, null);
        }

        public void SetRewindBoxes(bool enabled)
        {
            DontUpdateSpec = true;
            cbClearRewind.Checked = enabled;
            DontUpdateSpec = false;
        }

        private void CbClearRewind_CheckedChanged(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            S.GET<RTC_CorruptionEngine_Form>().SetRewindBoxes(cbClearRewind.Checked);
            S.GET<RTC_CustomEngineConfig_Form>().SetRewindBoxes(cbClearRewind.Checked);

            StepActions.ClearStepActionsOnRewind = cbClearRewind.Checked;
        }

        private void cbClearRewind_CheckedChanged(object sender, EventArgs e)
        {
        }
    }

    public enum PlatformType { CLASSIC, MODERN }
}
