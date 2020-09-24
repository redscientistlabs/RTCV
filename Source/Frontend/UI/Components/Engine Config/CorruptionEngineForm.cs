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

        internal readonly Components.EngineConfig.EngineControls.FreezeEngineControl gbFreezeEngine;
        internal Components.EngineConfig.EngineControls.NightmareEngineControl gbNightmareEngine = new Components.EngineConfig.EngineControls.NightmareEngineControl();
        internal readonly Components.EngineConfig.EngineControls.HellgenieEngineControl gbHellgenieEngine;
        private Components.EngineConfig.EngineControls.DistortionEngineControl gbDistortionEngine = new Components.EngineConfig.EngineControls.DistortionEngineControl();
        private Components.EngineConfig.EngineControls.CustomEngineControl gbCustomEngine = new Components.EngineConfig.EngineControls.CustomEngineControl();
        internal Components.EngineConfig.EngineControls.PipeEngineControl gbPipeEngine = new Components.EngineConfig.EngineControls.PipeEngineControl();
        internal Components.EngineConfig.EngineControls.BlastGeneratorEngineControl gbBlastGeneratorEngine = new Components.EngineConfig.EngineControls.BlastGeneratorEngineControl();
        internal readonly Components.EngineConfig.EngineControls.VectorEngineControl gbVectorEngine;
        internal Components.EngineConfig.EngineControls.ClusterEngineControl gbClusterEngine = new Components.EngineConfig.EngineControls.ClusterEngineControl();

        public string CurrentVectorLimiterListName
        {
            get {
                ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)gbVectorEngine.cbVectorLimiterList).SelectedItem;

                if (item == null) //this shouldn't ever happen unless the list files are missing
                    MessageBox.Show("Error: No vector engine limiter list selected. Bad install?");

                return item?.Name;
            }
        }

        public CorruptionEngineForm()
        {
            InitializeComponent();

            this.undockedSizable = false;

            gbFreezeEngine = new Components.EngineConfig.EngineControls.FreezeEngineControl(this);
            this.Controls.Add(gbFreezeEngine);

            this.Controls.Add(gbNightmareEngine);

            gbHellgenieEngine = new Components.EngineConfig.EngineControls.HellgenieEngineControl(this);
            this.Controls.Add(gbHellgenieEngine);

            this.Controls.Add(gbDistortionEngine);
            this.Controls.Add(gbCustomEngine);

            this.Controls.Add(gbPipeEngine);
            gbPipeEngine.cbClearPipesOnRewind.CheckedChanged += OnClearRewindToggle;

            this.Controls.Add(gbBlastGeneratorEngine);

            gbVectorEngine = new Components.EngineConfig.EngineControls.VectorEngineControl(this);
            this.Controls.Add(gbVectorEngine);

            this.Controls.Add(gbClusterEngine);
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

            gbVectorEngine.cbVectorValueList.DataSource = null;
            gbVectorEngine.cbVectorLimiterList.DataSource = null;
            gbClusterEngine.cbClusterLimiterList.DataSource = null;
            gbVectorEngine.cbVectorValueList.DisplayMember = "Name";
            gbVectorEngine.cbVectorLimiterList.DisplayMember = "Name";
            gbClusterEngine.cbClusterLimiterList.DisplayMember = "Name";

            gbVectorEngine.cbVectorValueList.ValueMember = "Value";
            gbVectorEngine.cbVectorLimiterList.ValueMember = "Value";
            gbClusterEngine.cbClusterLimiterList.ValueMember = "Value";

            //Do this here as if it's stuck into the designer, it keeps defaulting out
            gbVectorEngine.cbVectorValueList.DataSource = RtcCore.ValueListBindingSource;
            gbVectorEngine.cbVectorLimiterList.DataSource = RtcCore.LimiterListBindingSource;
            gbClusterEngine.cbClusterLimiterList.DataSource = RtcCore.LimiterListBindingSource;

            if (RtcCore.LimiterListBindingSource.Count > 0)
            {
                UpdateVectorLimiterList(gbVectorEngine.cbVectorLimiterList, null);
                UpdateVectorLimiterList(gbClusterEngine.cbClusterLimiterList, null);
            }
            if (RtcCore.ValueListBindingSource.Count > 0)
            {
                UpdateVectorValueList(gbVectorEngine.cbVectorValueList, null);
            }

            for (int j = 0; j < ClusterEngine.ShuffleTypes.Length; j++)
            {
                gbClusterEngine.cbClusterMethod.Items.Add(ClusterEngine.ShuffleTypes[j]);
            }
            gbClusterEngine.cbClusterMethod.SelectedIndex = 0;

            for (int j = 0; j < ClusterEngine.Directions.Length; j++)
            {
                gbClusterEngine.clusterDirection.Items.Add(ClusterEngine.Directions[j]);
            }
            gbClusterEngine.clusterDirection.SelectedIndex = 0;
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

                    if (gbVectorEngine.cbVectorUnlockPrecision.Checked)
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
            gbPipeEngine.cbLockPipes.Checked = enabled;
            dontUpdate = false;
        }

        public void SetRewindBoxes(bool enabled)
        {
            dontUpdate = true;
            S.GET<SettingsCorruptForm>().SetRewindBoxes(enabled);
            gbFreezeEngine.cbClearFreezesOnRewind.Checked = enabled;
            gbHellgenieEngine.cbClearCheatsOnRewind.Checked = enabled;
            gbPipeEngine.cbClearPipesOnRewind.Checked = enabled;
            dontUpdate = false;
        }

        private bool dontUpdate = false;

        internal void OnClearRewindToggle(object sender, EventArgs e)
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

        internal void UpdateVectorLimiterList(object sender, EventArgs e)
        {
            ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)sender).SelectedItem;
            if (item != null)
            {
                VectorEngine.LimiterListHash = item.Value;
            }
        }

        internal void UpdateVectorValueList(object sender, EventArgs e)
        {
            ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)sender).SelectedItem;
            if (item != null)
            {
                VectorEngine.ValueListHash = item.Value;
            }
        }

        internal void ClearCheats(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearStepBlastUnits, null, true);
        }

        private void UpdateMinMaxBoxes(int precision)
        {
            gbNightmareEngine.UpdateMinMaxBoxes(precision);
            gbHellgenieEngine.UpdateMinMaxBoxes(precision);
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

        internal void UpdateVectorUnlockPrecision(object sender, EventArgs e)
        {
            if (gbVectorEngine.cbVectorUnlockPrecision.Checked)
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
