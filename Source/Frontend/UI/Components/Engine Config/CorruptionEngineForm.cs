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

        internal readonly Components.EngineConfig.EngineControls.FreezeEngineControl freezeEngineControl;
        internal Components.EngineConfig.EngineControls.NightmareEngineControl nightmareEngineControl = new Components.EngineConfig.EngineControls.NightmareEngineControl();
        internal readonly Components.EngineConfig.EngineControls.HellgenieEngineControl hellgenieEngineControl;
        private Components.EngineConfig.EngineControls.DistortionEngineControl distortionEngineControl = new Components.EngineConfig.EngineControls.DistortionEngineControl();
        private Components.EngineConfig.EngineControls.CustomEngineControl customEngineControl = new Components.EngineConfig.EngineControls.CustomEngineControl();
        internal Components.EngineConfig.EngineControls.PipeEngineControl pipeEngineControl = new Components.EngineConfig.EngineControls.PipeEngineControl();
        internal Components.EngineConfig.EngineControls.BlastGeneratorEngineControl blastGeneratorEngineControl = new Components.EngineConfig.EngineControls.BlastGeneratorEngineControl();
        internal readonly Components.EngineConfig.EngineControls.VectorEngineControl vectorEngineControl;
        internal Components.EngineConfig.EngineControls.ClusterEngineControl clusterEngineControl = new Components.EngineConfig.EngineControls.ClusterEngineControl();

        public string CurrentVectorLimiterListName
        {
            get {
                ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)vectorEngineControl.cbVectorLimiterList).SelectedItem;

                if (item == null) //this shouldn't ever happen unless the list files are missing
                    MessageBox.Show("Error: No vector engine limiter list selected. Bad install?");

                return item?.Name;
            }
        }

        public CorruptionEngineForm()
        {
            InitializeComponent();

            this.undockedSizable = false;

            freezeEngineControl = new Components.EngineConfig.EngineControls.FreezeEngineControl(this);
            this.Controls.Add(freezeEngineControl);

            this.Controls.Add(nightmareEngineControl);

            hellgenieEngineControl = new Components.EngineConfig.EngineControls.HellgenieEngineControl(this);
            this.Controls.Add(hellgenieEngineControl);

            this.Controls.Add(distortionEngineControl);
            this.Controls.Add(customEngineControl);

            this.Controls.Add(pipeEngineControl);
            pipeEngineControl.cbClearPipesOnRewind.CheckedChanged += OnClearRewindToggle;

            this.Controls.Add(blastGeneratorEngineControl);

            vectorEngineControl = new Components.EngineConfig.EngineControls.VectorEngineControl(this);
            this.Controls.Add(vectorEngineControl);

            this.Controls.Add(clusterEngineControl);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            nmAlignment.registerSlave(S.GET<CustomEngineConfigForm>().nmAlignment);
            nightmareEngineControl.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            hellgenieEngineControl.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            distortionEngineControl.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            freezeEngineControl.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            pipeEngineControl.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            vectorEngineControl.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            clusterEngineControl.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            blastGeneratorEngineControl.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
            customEngineControl.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

            cbSelectedEngine.SelectedIndex = 0;
            cbCustomPrecision.SelectedIndex = 0;

            if (RtcCore.LimiterListBindingSource.Count > 0)
            {
                UpdateVectorLimiterList(vectorEngineControl.cbVectorLimiterList, null);
                UpdateVectorLimiterList(clusterEngineControl.cbClusterLimiterList, null);
            }
            if (RtcCore.ValueListBindingSource.Count > 0)
            {
                UpdateVectorValueList(vectorEngineControl.cbVectorValueList, null);
            }

            for (int j = 0; j < ClusterEngine.ShuffleTypes.Length; j++)
            {
                clusterEngineControl.cbClusterMethod.Items.Add(ClusterEngine.ShuffleTypes[j]);
            }
            clusterEngineControl.cbClusterMethod.SelectedIndex = 0;

            for (int j = 0; j < ClusterEngine.Directions.Length; j++)
            {
                clusterEngineControl.clusterDirection.Items.Add(ClusterEngine.Directions[j]);
            }
            clusterEngineControl.clusterDirection.SelectedIndex = 0;
        }

        private void UpdateEngine(object sender, EventArgs e)
        {
            nightmareEngineControl.Visible = false;
            hellgenieEngineControl.Visible = false;
            distortionEngineControl.Visible = false;
            freezeEngineControl.Visible = false;
            pipeEngineControl.Visible = false;
            vectorEngineControl.Visible = false;
            clusterEngineControl.Visible = false;
            blastGeneratorEngineControl.Visible = false;
            customEngineControl.Visible = false;
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
                    nightmareEngineControl.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Hellgenie Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.HELLGENIE;
                    hellgenieEngineControl.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Distortion Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.DISTORTION;
                    distortionEngineControl.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Freeze Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.FREEZE;
                    freezeEngineControl.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Pipe Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.PIPE;
                    pipeEngineControl.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Vector Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.VECTOR;
                    nmAlignment.Maximum = 3;
                    vectorEngineControl.Visible = true;

                    if (vectorEngineControl.cbVectorUnlockPrecision.Checked)
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
                    clusterEngineControl.Visible = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Custom Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.CUSTOM;
                    customEngineControl.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Blast Generator":
                    RtcCore.SelectedEngine = CorruptionEngine.BLASTGENERATORENGINE;
                    blastGeneratorEngineControl.Visible = true;

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
            pipeEngineControl.cbLockPipes.Checked = enabled;
            dontUpdate = false;
        }

        public void SetRewindBoxes(bool enabled)
        {
            dontUpdate = true;
            S.GET<SettingsCorruptForm>().SetRewindBoxes(enabled);
            freezeEngineControl.cbClearFreezesOnRewind.Checked = enabled;
            hellgenieEngineControl.cbClearCheatsOnRewind.Checked = enabled;
            pipeEngineControl.cbClearPipesOnRewind.Checked = enabled;
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

            StepActions.ClearStepActionsOnRewind = freezeEngineControl.cbClearFreezesOnRewind.Checked;
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
            nightmareEngineControl.UpdateMinMaxBoxes(precision);
            hellgenieEngineControl.UpdateMinMaxBoxes(precision);
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
            if (vectorEngineControl.cbVectorUnlockPrecision.Checked)
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
