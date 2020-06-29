namespace RTCV.UI
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.Common;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using static RTCV.UI.UI_Extensions;

    public partial class RTC_CorruptionEngine_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        //private int defaultPrecision = -1;
        private bool updatingMinMax = false;

        public string CurrentVectorLimiterListName
        {
            get {
                ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)cbVectorLimiterList).SelectedItem;

                if (item == null) //this shouldn't ever happen unless the list files are missing
                    MessageBox.Show("Error: No vector engine limiter list selected. Bad install?");

                return item?.Name;
            }
        }

        public string CurrentVectorValueListName
        {
            get
            {
                ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)cbVectorValueList).SelectedItem;

                if (item == null) //this shouldn't ever happen unless the list files are missing
                    MessageBox.Show("Error: No vector engine value list selected. Bad install?");

                return item?.Name;
            }
        }

        public RTC_CorruptionEngine_Form()
        {
            InitializeComponent();

            this.undockedSizable = false;
        }

        private void RTC_CorruptionEngine_Form_Load(object sender, EventArgs e)
        {
            nmAlignment.registerSlave(S.GET<RTC_CustomEngineConfig_Form>().nmAlignment);
            gbNightmareEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            gbHellgenieEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            gbDistortionEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            gbFreezeEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            gbPipeEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            gbVectorEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            gbClusterEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            gbBlastGeneratorEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            gbCustomEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

            cbSelectedEngine.SelectedIndex = 0;
            cbBlastType.SelectedIndex = 0;
            cbCustomPrecision.SelectedIndex = 0;

            cbVectorValueList.DataSource = null;
            cbVectorLimiterList.DataSource = null;
            cbClusterLimiterList.DataSource = null;
            cbVectorValueList.DisplayMember = "Name";
            cbVectorLimiterList.DisplayMember = "Name";
            cbClusterLimiterList.DisplayMember = "Name";

            cbVectorValueList.ValueMember = "Value";
            cbVectorLimiterList.ValueMember = "Value";
            cbClusterLimiterList.ValueMember = "Value";

            //Do this here as if it's stuck into the designer, it keeps defaulting out
            cbVectorValueList.DataSource = CorruptCore.RtcCore.ValueListBindingSource;
            cbVectorLimiterList.DataSource = CorruptCore.RtcCore.LimiterListBindingSource;
            cbClusterLimiterList.DataSource = CorruptCore.RtcCore.LimiterListBindingSource;

            if (CorruptCore.RtcCore.LimiterListBindingSource.Count > 0)
            {
                cbVectorLimiterList_SelectedIndexChanged(cbVectorLimiterList, null);
                cbVectorLimiterList_SelectedIndexChanged(cbClusterLimiterList, null);
            }
            if (CorruptCore.RtcCore.ValueListBindingSource.Count > 0)
            {
                cbVectorValueList_SelectedIndexChanged(cbVectorValueList, null);
            }

            clusterChunkSize.ValueChanged += clusterChunkSize_ValueChanged;
            clusterChunkModifier.ValueChanged += clusterChunkModifier_ValueChanged;

            for (int j = 0; j < RTC_ClusterEngine.ShuffleTypes.Length; j++)
            {
                cbClusterMethod.Items.Add(RTC_ClusterEngine.ShuffleTypes[j]);
            }
            cbClusterMethod.SelectedIndex = 0;

            for (int j = 0; j < RTC_ClusterEngine.Directions.Length; j++)
            {
                clusterDirection.Items.Add(RTC_ClusterEngine.Directions[j]);
            }
            clusterDirection.SelectedIndex = 0;
        }

        private void nmDistortionDelay_ValueChanged(object sender, EventArgs e)
        {
            RTC_DistortionEngine.Delay = Convert.ToInt32(nmDistortionDelay.Value);
        }

        private void btnResyncDistortionEngine_Click(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
        }

        private void cbSelectedEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            gbNightmareEngine.Visible = false;
            gbHellgenieEngine.Visible = false;
            gbDistortionEngine.Visible = false;
            gbFreezeEngine.Visible = false;
            gbPipeEngine.Visible = false;
            gbVectorEngine.Visible = false;
            gbClusterEngine.Visible = false;
            gbBlastGeneratorEngine.Visible = false;
            gbCustomEngine.Visible = false;
            cbCustomPrecision.Enabled = false;
            nmAlignment.Maximum = CorruptCore.RtcCore.CurrentPrecision - 1;

            //S.GET<RTC_GlitchHarvesterIntensity_Form>().Visible = true;
            S.GET<RTC_GeneralParameters_Form>().Show();
            S.GET<RTC_MemoryDomains_Form>().Show();
            S.GET<RTC_GlitchHarvesterIntensity_Form>().Show();

            switch (cbSelectedEngine.SelectedItem.ToString())
            {
                case "Nightmare Engine":
                    CorruptCore.RtcCore.SelectedEngine = CorruptionEngine.NIGHTMARE;
                    gbNightmareEngine.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = RTCV.NetCore.AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Hellgenie Engine":
                    CorruptCore.RtcCore.SelectedEngine = CorruptionEngine.HELLGENIE;
                    gbHellgenieEngine.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = RTCV.NetCore.AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Distortion Engine":
                    CorruptCore.RtcCore.SelectedEngine = CorruptionEngine.DISTORTION;
                    gbDistortionEngine.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = RTCV.NetCore.AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Freeze Engine":
                    CorruptCore.RtcCore.SelectedEngine = CorruptionEngine.FREEZE;
                    gbFreezeEngine.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = RTCV.NetCore.AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Pipe Engine":
                    CorruptCore.RtcCore.SelectedEngine = CorruptionEngine.PIPE;
                    gbPipeEngine.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = RTCV.NetCore.AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Vector Engine":
                    CorruptCore.RtcCore.SelectedEngine = CorruptionEngine.VECTOR;
                    nmAlignment.Maximum = 3;
                    gbVectorEngine.Visible = true;

                    S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = RTCV.NetCore.AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Cluster Engine":
                    CorruptCore.RtcCore.SelectedEngine = CorruptionEngine.CLUSTER;
                    nmAlignment.Maximum = 3;
                    gbClusterEngine.Visible = true;

                    S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = RTCV.NetCore.AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Custom Engine":
                    CorruptCore.RtcCore.SelectedEngine = CorruptionEngine.CUSTOM;
                    gbCustomEngine.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = RTCV.NetCore.AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Blast Generator":
                    CorruptCore.RtcCore.SelectedEngine = CorruptionEngine.BLASTGENERATORENGINE;
                    gbBlastGeneratorEngine.Visible = true;

                    S.GET<UI_CoreForm>().AutoCorrupt = false;
                    S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = false;
                    S.GET<RTC_GeneralParameters_Form>().Hide();
                    S.GET<RTC_MemoryDomains_Form>().Hide();
                    S.GET<RTC_GlitchHarvesterIntensity_Form>().Hide();
                    break;

                default:
                    break;
            }

            if (cbSelectedEngine.SelectedItem.ToString() == "Blast Generator")
            {
                S.GET<RTC_GeneralParameters_Form>().labelBlastRadius.Visible = false;
                S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Visible = false;
                S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Visible = false;
                S.GET<RTC_GeneralParameters_Form>().cbBlastRadius.Visible = false;
                S.GET<RTC_MemoryDomains_Form>().lbMemoryDomains.Visible = false;
            }
            else
            {
                S.GET<RTC_GeneralParameters_Form>().labelBlastRadius.Visible = true;
                S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Visible = true;
                S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Visible = true;
                S.GET<RTC_GeneralParameters_Form>().cbBlastRadius.Visible = true;
                S.GET<RTC_MemoryDomains_Form>().lbMemoryDomains.Visible = true;
            }

            cbSelectedEngine.BringToFront();
            pnCustomPrecision.BringToFront();

            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
        }

        public void SetLockBoxes(bool enabled)
        {
            dontUpdate = true;
            cbLockPipes.Checked = enabled;
            dontUpdate = false;
        }

        public void SetRewindBoxes(bool enabled)
        {
            dontUpdate = true;
            S.GET<RTC_SettingsCorrupt_Form>().SetRewindBoxes(enabled);
            cbClearFreezesOnRewind.Checked = enabled;
            cbClearCheatsOnRewind.Checked = enabled;
            cbClearPipesOnRewind.Checked = enabled;
            dontUpdate = false;
        }

        public bool dontUpdate = false;

        private void cbClearRewind_CheckedChanged(object sender, EventArgs e)
        {
            if (dontUpdate)
            {
                return;
            }

            SetRewindBoxes(((CheckBox)sender).Checked);

            S.GET<RTC_CustomEngineConfig_Form>().SetRewindBoxes(((CheckBox)sender).Checked);
            S.GET<RTC_SimpleMode_Form>().SetRewindBoxes(((CheckBox)sender).Checked);

            StepActions.ClearStepActionsOnRewind = cbClearFreezesOnRewind.Checked;
        }

        private void btnClearPipes_Click(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
        }

        private void cbLockPipes_CheckedChanged(object sender, EventArgs e)
        {
            S.GET<RTC_SettingsCorrupt_Form>().SetLockBoxes(cbLockPipes.Checked);
            StepActions.LockExecution = cbLockPipes.Checked;
        }

        private void cbVectorLimiterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)sender).SelectedItem;
            if (item != null)
            {
                RTC_VectorEngine.LimiterListHash = item.Value;
            }
        }

        private void cbVectorValueList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)sender).SelectedItem;
            if (item != null)
            {
                RTC_VectorEngine.ValueListHash = item.Value;
            }
        }

        private void btnClearCheats_Click(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
        }

        public void UpdateMinMaxBoxes(int precision)
        {
            updatingMinMax = true;
            switch (precision)
            {
                case 1:
                    nmMinValueNightmare.Maximum = byte.MaxValue;
                    nmMaxValueNightmare.Maximum = byte.MaxValue;

                    nmMinValueHellgenie.Maximum = byte.MaxValue;
                    nmMaxValueHellgenie.Maximum = byte.MaxValue;

                    nmMinValueNightmare.Value = RTC_NightmareEngine.MinValue8Bit;
                    nmMaxValueNightmare.Value = RTC_NightmareEngine.MaxValue8Bit;

                    nmMinValueHellgenie.Value = RTC_HellgenieEngine.MinValue8Bit;
                    nmMaxValueHellgenie.Value = RTC_HellgenieEngine.MaxValue8Bit;

                    break;

                case 2:
                    nmMinValueNightmare.Maximum = ushort.MaxValue;
                    nmMaxValueNightmare.Maximum = ushort.MaxValue;

                    nmMinValueHellgenie.Maximum = ushort.MaxValue;
                    nmMaxValueHellgenie.Maximum = ushort.MaxValue;

                    nmMinValueNightmare.Value = RTC_NightmareEngine.MinValue16Bit;
                    nmMaxValueNightmare.Value = RTC_NightmareEngine.MaxValue16Bit;

                    nmMinValueHellgenie.Value = RTC_HellgenieEngine.MinValue16Bit;
                    nmMaxValueHellgenie.Value = RTC_HellgenieEngine.MaxValue16Bit;

                    break;
                case 4:
                    nmMinValueNightmare.Maximum = uint.MaxValue;
                    nmMaxValueNightmare.Maximum = uint.MaxValue;

                    nmMinValueHellgenie.Maximum = uint.MaxValue;
                    nmMaxValueHellgenie.Maximum = uint.MaxValue;

                    nmMinValueNightmare.Value = RTC_NightmareEngine.MinValue32Bit;
                    nmMaxValueNightmare.Value = RTC_NightmareEngine.MaxValue32Bit;

                    nmMinValueHellgenie.Value = RTC_HellgenieEngine.MinValue32Bit;
                    nmMaxValueHellgenie.Value = RTC_HellgenieEngine.MaxValue32Bit;

                    break;
                case 8:
                    nmMinValueNightmare.Maximum = ulong.MaxValue;
                    nmMaxValueNightmare.Maximum = ulong.MaxValue;

                    nmMinValueHellgenie.Maximum = ulong.MaxValue;
                    nmMaxValueHellgenie.Maximum = ulong.MaxValue;

                    nmMinValueNightmare.Value = RTC_NightmareEngine.MinValue64Bit;
                    nmMaxValueNightmare.Value = RTC_NightmareEngine.MaxValue64Bit;

                    nmMinValueHellgenie.Value = RTC_HellgenieEngine.MinValue64Bit;
                    nmMaxValueHellgenie.Value = RTC_HellgenieEngine.MaxValue64Bit;

                    break;
            }
            updatingMinMax = false;
        }

        private void cbCustomPrecision_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbCustomPrecision.Enabled = false;
            S.GET<RTC_CustomEngineConfig_Form>().cbCustomPrecision.Enabled = false;
            try
            {
                if (cbCustomPrecision.SelectedIndex != -1)
                {
                    int precision = 0;
                    switch (cbCustomPrecision.SelectedIndex)
                    {
                        case 0:
                            precision = 1;
                            break;
                        case 1:
                            precision = 2;
                            break;
                        case 2:
                            precision = 4;
                            break;
                        case 3:
                            precision = 8;
                            break;
                    }
                    CorruptCore.RtcCore.CurrentPrecision = precision;

                    UpdateMinMaxBoxes(precision);
                    nmAlignment.Maximum = precision - 1;
                    S.GET<RTC_CustomEngineConfig_Form>().cbCustomPrecision.SelectedIndex = cbCustomPrecision.SelectedIndex;
                    S.GET<RTC_CustomEngineConfig_Form>().UpdateMinMaxBoxes(precision);
                }
            }
            finally
            {
                cbCustomPrecision.Enabled = true;
                S.GET<RTC_CustomEngineConfig_Form>().cbCustomPrecision.Enabled = true;
            }
        }

        private void nmAlignment_ValueChanged(object sender, EventArgs e)
        {
            CorruptCore.RtcCore.Alignment = Convert.ToInt32(nmAlignment.Value);
        }

        private void btnOpenBlastGenerator_Click(object sender, EventArgs e)
        {
            if (S.GET<RTC_BlastGenerator_Form>() != null)
            {
                S.GET<RTC_BlastGenerator_Form>().Close();
            }

            S.SET(new RTC_BlastGenerator_Form());
            S.GET<RTC_BlastGenerator_Form>().LoadNoStashKey();
        }

        private void nmMinValueNightmare_ValueChanged(object sender, EventArgs e)
        {
            //We don't want to trigger this if it caps when stepping downwards
            if (updatingMinMax)
            {
                return;
            }

            ulong value = Convert.ToUInt64(nmMinValueNightmare.Value);

            switch (CorruptCore.RtcCore.CurrentPrecision)
            {
                case 1:
                    RTC_NightmareEngine.MinValue8Bit = value;
                    break;
                case 2:
                    RTC_NightmareEngine.MinValue16Bit = value;
                    break;
                case 4:
                    RTC_NightmareEngine.MinValue32Bit = value;
                    break;
                case 8:
                    RTC_NightmareEngine.MinValue64Bit = value;
                    break;
            }
        }

        private void nmMaxValueNightmare_ValueChanged(object sender, EventArgs e)
        {
            //We don't want to trigger this if it caps when stepping downwards
            if (updatingMinMax)
            {
                return;
            }

            ulong value = Convert.ToUInt64(nmMaxValueNightmare.Value);

            switch (CorruptCore.RtcCore.CurrentPrecision)
            {
                case 1:
                    RTC_NightmareEngine.MaxValue8Bit = value;
                    break;
                case 2:
                    RTC_NightmareEngine.MaxValue16Bit = value;
                    break;
                case 4:
                    RTC_NightmareEngine.MaxValue32Bit = value;
                    break;
                case 8:
                    RTC_NightmareEngine.MaxValue64Bit = value;
                    break;
            }
        }

        private void nmMinValueHellgenie_ValueChanged(object sender, EventArgs e)
        {
            //We don't want to trigger this if it caps when stepping downwards
            if (updatingMinMax)
            {
                return;
            }

            ulong value = Convert.ToUInt64(nmMinValueHellgenie.Value);

            switch (CorruptCore.RtcCore.CurrentPrecision)
            {
                case 1:
                    RTC_HellgenieEngine.MinValue8Bit = value;
                    break;
                case 2:
                    RTC_HellgenieEngine.MinValue16Bit = value;
                    break;
                case 4:
                    RTC_HellgenieEngine.MinValue32Bit = value;
                    break;
                case 8:
                    RTC_HellgenieEngine.MinValue64Bit = value;
                    break;
            }
        }

        private void nmMaxValueHellgenie_ValueChanged(object sender, EventArgs e)
        {
            //We don't want to trigger this if it caps when stepping downwards
            if (updatingMinMax)
            {
                return;
            }

            ulong value = Convert.ToUInt64(nmMaxValueHellgenie.Value);

            switch (CorruptCore.RtcCore.CurrentPrecision)
            {
                case 1:
                    RTC_HellgenieEngine.MaxValue8Bit = value;
                    break;
                case 2:
                    RTC_HellgenieEngine.MaxValue16Bit = value;
                    break;
                case 4:
                    RTC_HellgenieEngine.MaxValue32Bit = value;
                    break;
                case 8:
                    RTC_HellgenieEngine.MaxValue64Bit = value;
                    break;
            }
        }

        private void cbBlastType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbBlastType.SelectedItem.ToString())
            {
                case "RANDOM":
                    RTC_NightmareEngine.Algo = NightmareAlgo.RANDOM;
                    nmMinValueNightmare.Enabled = true;
                    nmMaxValueNightmare.Enabled = true;
                    break;

                case "RANDOMTILT":
                    RTC_NightmareEngine.Algo = NightmareAlgo.RANDOMTILT;
                    nmMinValueNightmare.Enabled = true;
                    nmMaxValueNightmare.Enabled = true;
                    break;

                case "TILT":
                    RTC_NightmareEngine.Algo = NightmareAlgo.TILT;
                    nmMinValueNightmare.Enabled = false;
                    nmMaxValueNightmare.Enabled = false;
                    break;
            }
        }

        private void btnOpenCustomEngine_Click(object sender, EventArgs e)
        {
            S.GET<RTC_CustomEngineConfig_Form>().Show();
            S.GET<RTC_CustomEngineConfig_Form>().Focus();
        }

        private void cbClusterLimiterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)sender).SelectedItem;
            if (item != null)
            {
                RTC_ClusterEngine.LimiterListHash = item.Value;
            }
        }

        private void clusterChunkSize_ValueChanged(object sender, EventArgs e)
        {
            RTC_ClusterEngine.ChunkSize = (int)clusterChunkSize.Value;
        }

        private void clusterChunkModifier_ValueChanged(object sender, EventArgs e)
        {
            RTC_ClusterEngine.Modifier = (int)clusterChunkModifier.Value;
        }

        private void cbClusterMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            RTC_ClusterEngine.ShuffleType = cbClusterMethod.SelectedItem.ToString();

            if (cbClusterMethod.SelectedItem.ToString().ToLower().Contains("rotate"))
            {
                clusterChunkModifier.Enabled = true;
            }
            else
            {
                clusterChunkModifier.Enabled = false;
            }
        }

        private void clusterSplitUnits_CheckedChanged(object sender, EventArgs e)
        {
            RTC_ClusterEngine.OutputMultipleUnits = clusterSplitUnits.Checked;
        }

        private void clusterDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            RTC_ClusterEngine.Direction = clusterDirection.SelectedItem.ToString();
        }

        private void clusterFilterAll_CheckedChanged(object sender, EventArgs e)
        {
            RTC_ClusterEngine.FilterAll = clusterFilterAll.Checked;
        }
    }
}
