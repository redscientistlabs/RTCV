namespace RTCV.UI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Components.Controls;
    using RTCV.UI.Modular;

    public partial class SimpleModeForm : ComponentForm, IBlockable
    {
        private PlatformType platform = PlatformType.CLASSIC;

        private bool DontUpdateSpec = false;

        public SimpleModeForm()
        {
            InitializeComponent();
        }

        public void EnteringSimpleMode()
        {
            GlitchHarvesterIntensityForm ghiForm = S.GET<GlitchHarvesterIntensityForm>();
            ghiForm.AnchorToPanel(pnIntensity);
            //S.GET<CoreForm>().btnEngineConfig.Visible = false;
            S.GET<CoreForm>().btnEngineConfig.Text = " Simple Mode";
            S.GET<CoreForm>().btnGlitchHarvester.Visible = false;
            S.GET<CoreForm>().btnStockpilePlayer.Visible = false;
            S.GET<CoreForm>().btnOpenCustomLayout.Visible = false;
            S.GET<CoreForm>().btnAutoCorrupt.Visible = false;
            S.GET<CoreForm>().btnManualBlast.Visible = false;

            if (rbClassicPlatforms.Checked)
            {
                SelectClassicPlatforms(null, null);
            }

            if (rbModernPlatforms.Checked)
            {
                SelectModernPlatforms(null, null);
            }

            Params.SetParam("SIMPLE_MODE"); //Set RTC in Simple Mode
        }

        public static void LeavingSimpleMode()
        {
            Params.RemoveParam("SIMPLE_MODE"); //Set RTC in Normal Mode

            //S.GET<CoreForm>().btnEngineConfig.Visible = true;
            S.GET<CoreForm>().btnEngineConfig.Text = " Engine Config";
            S.GET<CoreForm>().btnGlitchHarvester.Visible = true;
            S.GET<CoreForm>().btnStockpilePlayer.Visible = true;
            S.GET<CoreForm>().btnOpenCustomLayout.Visible = true;
            S.GET<CoreForm>().btnAutoCorrupt.Visible = true;
            S.GET<CoreForm>().btnManualBlast.Visible = true;

            S.GET<CoreForm>().OpenEngineConfig(null, null);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void BlastLayerToggle(object sender, EventArgs e)
        {
            S.GET<GlitchHarvesterBlastForm>().BlastLayerToggle(null, null);
        }

        private void ManualBlast(object sender, EventArgs e)
        {
            S.GET<CoreForm>().ManualBlast(sender, e);
        }

        private void StartAutoCorrupt(object sender, EventArgs e)
        {
            S.GET<CoreForm>().StartAutoCorrupt(sender, e);
            //btnAutoCorrupt.Text = S.GET<CoreForm>().btnAutoCorrupt.Text;
        }

        private void CreateGlitchHarvesterSavestate(object sender, EventArgs e)
        {
            //Select first savestate slot if none is selected
            var selectedHolder = S.GET<SavestateManagerForm>().savestateList.SelectedHolder;
            if (selectedHolder == null)
            {
                //Generate object sender and MouseEventArgs e data for the button click
                SavestateHolder holder = (SavestateHolder)S.GET<SavestateManagerForm>().savestateList.flowPanel.Controls[0];
                Button _sender = holder.btnSavestate;
                MouseEventArgs _e = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);

                //Click first GH Savestate
                S.GET<SavestateManagerForm>().savestateList.BtnSavestate_MouseDown(_sender, _e);
            }

            //Switch to Save
            S.GET<SavestateManagerForm>().savestateList.btnSaveLoad.Text = "SAVE";
            S.GET<SavestateManagerForm>().savestateList.btnSaveLoad.ForeColor = Color.OrangeRed;

            //Trigger Save button
            S.GET<SavestateManagerForm>().savestateList.HandleSaveLoadClick(null, null);
        }

        private void GlitchHarvesterLoadAndCorrupt(object sender, EventArgs e)
        {
            if (S.GET<StashHistoryForm>().lbStashHistory.Items.Count >= 20)
            {
                S.GET<StashHistoryForm>().RemoveFirstStashHistoryItem();
            }

            S.GET<GlitchHarvesterBlastForm>().Corrupt(null, null);
        }

        private void SelectClassicPlatforms(object sender, EventArgs e)
        {
            SwitchPlatformType(PlatformType.CLASSIC);
        }

        private void SelectModernPlatforms(object sender, EventArgs e)
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

            S.GET<CoreForm>().btnAutoCorrupt.Visible = false;
            S.GET<CoreForm>().btnManualBlast.Visible = false;

            platform = pt;

            gbEngineParameters.Visible = true;
            gbRealTimeCorruption.Visible = true;
            gbSimpleGlitchHarvester.Visible = true;

            btnBlastToggle.Visible = true;
        }

        private void ShuffleAlgorithm(object sender, EventArgs e)
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

            S.GET<CoreForm>().btnAutoCorrupt.Visible = false;
            S.GET<CoreForm>().btnManualBlast.Visible = false;
        }

        public void ShuffleModern()
        {
            SelectComboBoxRandomItem(S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorLimiterList);
            SelectComboBoxRandomItem(S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorValueList);

            string limiter = S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorLimiterList.Text;
            string value = S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorValueList.Text;

            lbEngineDescription.Text = $"Auto-Selected Engine: Vector Engine\n" +
                                       $"Parameters: Limiter:{limiter} , Value:{value}\n" +
                                       $"\n" +
                                       $"This engine is made for corrupting 3d games\n" +
                                       $"and 2d games made for 3d-era consoles.";
        }

        private static void SelectComboBoxRandomItem(ComboBox cb)
        {
            Random RND = new Random((int)DateTime.Now.Ticks);
            int nbItems = cb.Items.Count;
            int SelectIndex = RND.Next(0, nbItems - 1);
            cb.SelectedIndex = SelectIndex;
        }

        public static void SelectEngineByName(string name)
        {
            int selectedEngineIndex = -1;

            ComboBox cb = S.GET<CorruptionEngineForm>().cbSelectedEngine;

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
            var ui = S.GET<CoreForm>();
            if (ui.AutoCorrupt)
            {
                StartAutoCorrupt(null, null);
            }

            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearStepBlastUnits, null, true);
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

            if (S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorLimiterList.Items.Count > 0)
            {
                S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorLimiterList.SelectedIndex = 0;
                S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorValueList.SelectedIndex = 0;
            }
            else
            {
                throw new Exception("No vector lists could be found. Your RTCV installation might be broken.");
            }

            string limiter = S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorLimiterList.Text;
            string value = S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorValueList.Text;

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

        private void ClearInfiniteUnits(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearStepBlastUnits, null, true);
        }

        private void SwitchToNormalMode(object sender, EventArgs e)
        {
            LeavingSimpleMode();
            this.Hide();
        }

        private void LoadGlitchHarvesterSavestate(object sender, EventArgs e)
        {
            //Select first savestate slot if none is selected
            var selectedHolder = S.GET<SavestateManagerForm>().savestateList.SelectedHolder;
            if (selectedHolder == null)
            {
                //Generate object sender and MouseEventArgs e data for the button click
                SavestateHolder holder = (SavestateHolder)S.GET<SavestateManagerForm>().savestateList.flowPanel.Controls[0];
                Button _sender = holder.btnSavestate;
                MouseEventArgs _e = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);

                //Click first GH Savestate
                S.GET<SavestateManagerForm>().savestateList.BtnSavestate_MouseDown(_sender, _e);
            }

            //Switch to Save
            S.GET<SavestateManagerForm>().savestateList.btnSaveLoad.Text = "LOAD";
            S.GET<SavestateManagerForm>().savestateList.btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);

            //Trigger Save button
            S.GET<SavestateManagerForm>().savestateList.HandleSaveLoadClick(null, null);
        }

        public void SetRewindBoxes(bool enabled)
        {
            DontUpdateSpec = true;
            cbClearRewind.Checked = enabled;
            DontUpdateSpec = false;
        }

        private void OnClearRewindChanged(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            S.GET<CorruptionEngineForm>().SetRewindBoxes(cbClearRewind.Checked);
            S.GET<CustomEngineConfigForm>().SetRewindBoxes(cbClearRewind.Checked);

            StepActions.ClearStepActionsOnRewind = cbClearRewind.Checked;
        }
    }

    public enum PlatformType { CLASSIC, MODERN }
}
