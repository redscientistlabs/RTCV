namespace RTCV.UI
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.Common;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.UI.Modular;

    [SuppressMessage("Microsoft.Designer", "CA2213:Disposable types are not disposed", Justification = "Designer classes have their own Dispose method")]
    public partial class CorruptionEngineForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        internal Components.EngineConfig.Engines.FreezeEngine gbFreezeEngine = new Components.EngineConfig.Engines.FreezeEngine();
        internal Components.EngineConfig.Engines.NightmareEngine gbNightmareEngine = new Components.EngineConfig.Engines.NightmareEngine();
        internal Components.EngineConfig.Engines.HellgenieEngine gbHellgenieEngine = new Components.EngineConfig.Engines.HellgenieEngine();
        private Components.EngineConfig.Engines.DistortionEngine gbDistortionEngine = new Components.EngineConfig.Engines.DistortionEngine();

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

        public CorruptionEngineForm()
        {
            InitializeComponent();

            this.undockedSizable = false;

            this.Controls.Add(gbFreezeEngine);
            this.Controls.Add(gbNightmareEngine);
            this.Controls.Add(gbHellgenieEngine);
            this.Controls.Add(gbDistortionEngine);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            nmAlignment.registerSlave(S.GET<CustomEngineConfigForm>().nmAlignment);
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
            gbNightmareEngine.cbBlastType.SelectedIndex = 0;
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
            cbVectorValueList.DataSource = RtcCore.ValueListBindingSource;
            cbVectorLimiterList.DataSource = RtcCore.LimiterListBindingSource;
            cbClusterLimiterList.DataSource = RtcCore.LimiterListBindingSource;

            if (RtcCore.LimiterListBindingSource.Count > 0)
            {
                UpdateVectorLimiterList(cbVectorLimiterList, null);
                UpdateVectorLimiterList(cbClusterLimiterList, null);
            }
            if (RtcCore.ValueListBindingSource.Count > 0)
            {
                UpdateVectorValueList(cbVectorValueList, null);
            }

            clusterChunkSize.ValueChanged += UpdateClusterChunkSize;
            clusterChunkModifier.ValueChanged += UpdateClusterModifier;

            for (int j = 0; j < ClusterEngine.ShuffleTypes.Length; j++)
            {
                cbClusterMethod.Items.Add(ClusterEngine.ShuffleTypes[j]);
            }
            cbClusterMethod.SelectedIndex = 0;

            for (int j = 0; j < ClusterEngine.Directions.Length; j++)
            {
                clusterDirection.Items.Add(ClusterEngine.Directions[j]);
            }
            clusterDirection.SelectedIndex = 0;
        }

        private void ResyncDistortionEngine(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearStepBlastUnits, null, true);
        }

        private void UpdateEngine(object sender, EventArgs e)
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
            nmAlignment.Maximum = RtcCore.CurrentPrecision - 1;

            //S.GET<GlitchHarvesterIntensityForm>().Visible = true;
            S.GET<GeneralParametersForm>().Show();
            S.GET<MemoryDomainsForm>().Show();
            S.GET<GlitchHarvesterIntensityForm>().Show();

            switch (cbSelectedEngine.SelectedItem.ToString())
            {
                case "Nightmare Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.NIGHTMARE;
                    gbNightmareEngine.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Hellgenie Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.HELLGENIE;
                    gbHellgenieEngine.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Distortion Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.DISTORTION;
                    gbDistortionEngine.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Freeze Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.FREEZE;
                    gbFreezeEngine.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Pipe Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.PIPE;
                    gbPipeEngine.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Vector Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.VECTOR;
                    nmAlignment.Maximum = 3;
                    gbVectorEngine.Visible = true;

                    if (cbVectorUnlockPrecision.Checked)
                    {
                        nmAlignment.Maximum = new decimal(new int[] { 0, 0, 0, 0 });
                        cbCustomPrecision.Enabled = true;
                    }
                    else
                    {
                        nmAlignment.Maximum = 3;
                        cbCustomPrecision.Enabled = false;
                    }

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Cluster Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.CLUSTER;
                    nmAlignment.Maximum = 3;
                    gbClusterEngine.Visible = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Custom Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.CUSTOM;
                    gbCustomEngine.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Blast Generator":
                    RtcCore.SelectedEngine = CorruptionEngine.BLASTGENERATORENGINE;
                    gbBlastGeneratorEngine.Visible = true;

                    S.GET<CoreForm>().AutoCorrupt = false;
                    S.GET<CoreForm>().btnAutoCorrupt.Visible = false;
                    S.GET<GeneralParametersForm>().Hide();
                    S.GET<MemoryDomainsForm>().Hide();
                    S.GET<GlitchHarvesterIntensityForm>().Hide();
                    break;

                default:
                    break;
            }

            if (cbSelectedEngine.SelectedItem.ToString() == "Blast Generator")
            {
                S.GET<GeneralParametersForm>().labelBlastRadius.Visible = false;
                S.GET<GeneralParametersForm>().multiTB_Intensity.Visible = false;
                S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Visible = false;
                S.GET<GeneralParametersForm>().cbBlastRadius.Visible = false;
                S.GET<MemoryDomainsForm>().lbMemoryDomains.Visible = false;
            }
            else
            {
                S.GET<GeneralParametersForm>().labelBlastRadius.Visible = true;
                S.GET<GeneralParametersForm>().multiTB_Intensity.Visible = true;
                S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Visible = true;
                S.GET<GeneralParametersForm>().cbBlastRadius.Visible = true;
                S.GET<MemoryDomainsForm>().lbMemoryDomains.Visible = true;
            }

            cbSelectedEngine.BringToFront();
            pnCustomPrecision.BringToFront();

            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearStepBlastUnits, null, true);
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
            S.GET<SettingsCorruptForm>().SetRewindBoxes(enabled);
            gbFreezeEngine.cbClearFreezesOnRewind.Checked = enabled;
            gbHellgenieEngine.cbClearCheatsOnRewind.Checked = enabled;
            cbClearPipesOnRewind.Checked = enabled;
            dontUpdate = false;
        }

        private bool dontUpdate = false;

        private void OnClearRewindToggle(object sender, EventArgs e)
        {
            if (dontUpdate)
            {
                return;
            }

            SetRewindBoxes(((CheckBox)sender).Checked);

            S.GET<CustomEngineConfigForm>().SetRewindBoxes(((CheckBox)sender).Checked);
            S.GET<SimpleModeForm>().SetRewindBoxes(((CheckBox)sender).Checked);

            StepActions.ClearStepActionsOnRewind = gbFreezeEngine.cbClearFreezesOnRewind.Checked;
        }

        private void ClearPipes(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearStepBlastUnits, null, true);
        }

        private void OnLockPipesToggle(object sender, EventArgs e)
        {
            S.GET<SettingsCorruptForm>().SetLockBoxes(cbLockPipes.Checked);
            StepActions.LockExecution = cbLockPipes.Checked;
        }

        private void UpdateVectorLimiterList(object sender, EventArgs e)
        {
            ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)sender).SelectedItem;
            if (item != null)
            {
                VectorEngine.LimiterListHash = item.Value;
            }
        }

        private void UpdateVectorValueList(object sender, EventArgs e)
        {
            ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)sender).SelectedItem;
            if (item != null)
            {
                VectorEngine.ValueListHash = item.Value;
            }
        }

        private void ClearCheats(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearStepBlastUnits, null, true);
        }

        private void UpdateMinMaxBoxes(int precision)
        {
            switch (precision)
            {
                case 1:
                    gbNightmareEngine.nmMinValueNightmare.Maximum = byte.MaxValue;
                    gbNightmareEngine.nmMaxValueNightmare.Maximum = byte.MaxValue;

                    gbHellgenieEngine.nmMinValueHellgenie.Maximum = byte.MaxValue;
                    gbHellgenieEngine.nmMaxValueHellgenie.Maximum = byte.MaxValue;

                    gbNightmareEngine.nmMinValueNightmare.Value = NightmareEngine.MinValue8Bit;
                    gbNightmareEngine.nmMaxValueNightmare.Value = NightmareEngine.MaxValue8Bit;

                    gbHellgenieEngine.nmMinValueHellgenie.Value = HellgenieEngine.MinValue8Bit;
                    gbHellgenieEngine.nmMaxValueHellgenie.Value = HellgenieEngine.MaxValue8Bit;

                    break;

                case 2:
                    gbNightmareEngine.nmMinValueNightmare.Maximum = ushort.MaxValue;
                    gbNightmareEngine.nmMaxValueNightmare.Maximum = ushort.MaxValue;

                    gbHellgenieEngine.nmMinValueHellgenie.Maximum = ushort.MaxValue;
                    gbHellgenieEngine.nmMaxValueHellgenie.Maximum = ushort.MaxValue;

                    gbNightmareEngine.nmMinValueNightmare.Value = NightmareEngine.MinValue16Bit;
                    gbNightmareEngine.nmMaxValueNightmare.Value = NightmareEngine.MaxValue16Bit;

                    gbHellgenieEngine.nmMinValueHellgenie.Value = HellgenieEngine.MinValue16Bit;
                    gbHellgenieEngine.nmMaxValueHellgenie.Value = HellgenieEngine.MaxValue16Bit;

                    break;
                case 4:
                    gbNightmareEngine.nmMinValueNightmare.Maximum = uint.MaxValue;
                    gbNightmareEngine.nmMaxValueNightmare.Maximum = uint.MaxValue;

                    gbHellgenieEngine.nmMinValueHellgenie.Maximum = uint.MaxValue;
                    gbHellgenieEngine.nmMaxValueHellgenie.Maximum = uint.MaxValue;

                    gbNightmareEngine.nmMinValueNightmare.Value = NightmareEngine.MinValue32Bit;
                    gbNightmareEngine.nmMaxValueNightmare.Value = NightmareEngine.MaxValue32Bit;

                    gbHellgenieEngine.nmMinValueHellgenie.Value = HellgenieEngine.MinValue32Bit;
                    gbHellgenieEngine.nmMaxValueHellgenie.Value = HellgenieEngine.MaxValue32Bit;

                    break;
                case 8:
                    gbNightmareEngine.nmMinValueNightmare.Maximum = ulong.MaxValue;
                    gbNightmareEngine.nmMaxValueNightmare.Maximum = ulong.MaxValue;

                    gbHellgenieEngine.nmMinValueHellgenie.Maximum = ulong.MaxValue;
                    gbHellgenieEngine.nmMaxValueHellgenie.Maximum = ulong.MaxValue;

                    gbNightmareEngine.nmMinValueNightmare.Value = NightmareEngine.MinValue64Bit;
                    gbNightmareEngine.nmMaxValueNightmare.Value = NightmareEngine.MaxValue64Bit;

                    gbHellgenieEngine.nmMinValueHellgenie.Value = HellgenieEngine.MinValue64Bit;
                    gbHellgenieEngine.nmMaxValueHellgenie.Value = HellgenieEngine.MaxValue64Bit;

                    break;
            }
        }

        private void UpdateCustomPrecision(object sender, EventArgs e)
        {
            cbCustomPrecision.Enabled = false;
            S.GET<CustomEngineConfigForm>().cbCustomPrecision.Enabled = false;
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
                    RtcCore.CurrentPrecision = precision;

                    UpdateMinMaxBoxes(precision);
                    nmAlignment.Maximum = precision - 1;
                    S.GET<CustomEngineConfigForm>().cbCustomPrecision.SelectedIndex = cbCustomPrecision.SelectedIndex;
                    S.GET<CustomEngineConfigForm>().UpdateMinMaxBoxes(precision);
                }
            }
            finally
            {
                cbCustomPrecision.Enabled = true;
                S.GET<CustomEngineConfigForm>().cbCustomPrecision.Enabled = true;
            }
        }

        private void OpenBlastGenerator(object sender, EventArgs e)
        {
            if (S.GET<BlastGeneratorForm>() != null)
            {
                S.GET<BlastGeneratorForm>().Close();
            }

            S.SET(new BlastGeneratorForm());
            S.GET<BlastGeneratorForm>().LoadNoStashKey();
        }

        private void UpdateBlastType(object sender, EventArgs e)
        {
            switch (gbNightmareEngine.cbBlastType.SelectedItem.ToString())
            {
                case "RANDOM":
                    NightmareEngine.Algo = NightmareAlgo.RANDOM;
                    gbNightmareEngine.nmMinValueNightmare.Enabled = true;
                    gbNightmareEngine.nmMaxValueNightmare.Enabled = true;
                    break;

                case "RANDOMTILT":
                    NightmareEngine.Algo = NightmareAlgo.RANDOMTILT;
                    gbNightmareEngine.nmMinValueNightmare.Enabled = true;
                    gbNightmareEngine.nmMaxValueNightmare.Enabled = true;
                    break;

                case "TILT":
                    NightmareEngine.Algo = NightmareAlgo.TILT;
                    gbNightmareEngine.nmMinValueNightmare.Enabled = false;
                    gbNightmareEngine.nmMaxValueNightmare.Enabled = false;
                    break;
            }
        }

        private void OpenCustomEngine(object sender, EventArgs e)
        {
            S.GET<CustomEngineConfigForm>().Show();
            S.GET<CustomEngineConfigForm>().Focus();
        }

        private void UpdateClusterLimiterList(object sender, EventArgs e)
        {
            ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)sender).SelectedItem;
            if (item != null)
            {
                ClusterEngine.LimiterListHash = item.Value;
            }
        }

        private void UpdateClusterChunkSize(object sender, EventArgs e)
        {
            ClusterEngine.ChunkSize = (int)clusterChunkSize.Value;
        }

        private void UpdateClusterModifier(object sender, EventArgs e)
        {
            ClusterEngine.Modifier = (int)clusterChunkModifier.Value;
        }

        private void UpdateClusterMethod(object sender, EventArgs e)
        {
            ClusterEngine.ShuffleType = cbClusterMethod.SelectedItem.ToString();

            if (cbClusterMethod.SelectedItem.ToString().ToLower().Contains("rotate"))
            {
                clusterChunkModifier.Enabled = true;
            }
            else
            {
                clusterChunkModifier.Enabled = false;
            }
        }

        private void UpdateClusterSplitUnits(object sender, EventArgs e)
        {
            ClusterEngine.OutputMultipleUnits = clusterSplitUnits.Checked;
        }

        private void UpdateClusterDirection(object sender, EventArgs e)
        {
            ClusterEngine.Direction = clusterDirection.SelectedItem.ToString();
        }

        private void UpdateClusterFilterAll(object sender, EventArgs e)
        {
            ClusterEngine.FilterAll = clusterFilterAll.Checked;
        }

        private void UpdateVectorUnlockPrecision(object sender, EventArgs e)
        {
            if (cbVectorUnlockPrecision.Checked)
            {
                nmAlignment.Maximum = new decimal(new int[] { 0, 0, 0, 0 });
                cbCustomPrecision.Enabled = true;
                VectorEngine.UnlockPrecision = true;
            }
            else
            {
                nmAlignment.Maximum = 3;
                cbCustomPrecision.Enabled = false;
                VectorEngine.UnlockPrecision = true;
            }
        }
    }
}
