namespace RTCV.UI
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Windows.Forms;
    using RTCV.Common;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.UI.Components.Controls;
    using RTCV.UI.Modular;

    [SuppressMessage("Microsoft.Designer", "CA2213:Disposable types are not disposed", Justification = "Designer classes have their own Dispose method")]
    public partial class CorruptionEngineForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public readonly Components.EngineConfig.EngineControls.FreezeEngineControl FreezeEngineControl;
        public readonly Components.EngineConfig.EngineControls.NightmareEngineControl NightmareEngineControl;
        public readonly Components.EngineConfig.EngineControls.HellgenieEngineControl HellgenieEngineControl;
        public readonly Components.EngineConfig.EngineControls.DistortionEngineControl distortionEngineControl;
        public readonly Components.EngineConfig.EngineControls.CustomEngineControl customEngineControl;
        public readonly Components.EngineConfig.EngineControls.PipeEngineControl PipeEngineControl;
        public readonly Components.EngineConfig.EngineControls.BlastGeneratorEngineControl BlastGeneratorEngineControl;
        public readonly Components.EngineConfig.EngineControls.VectorEngineControl VectorEngineControl;
        public readonly Components.EngineConfig.EngineControls.ClusterEngineControl ClusterEngineControl;

        public string CurrentVectorLimiterListName
        {
            get {
                ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)VectorEngineControl.cbVectorLimiterList).SelectedItem;

                if (item == null) //this shouldn't ever happen unless the list files are missing
                {
                    {
                    MessageBox.Show("Error: No vector engine limiter list selected. Bad install?");
                }
                }

                return item?.Name;
            }
        }

        public CorruptionEngineForm()
        {
            InitializeComponent();

            this.undockedSizable = false;

            var engineControlLocation = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

            FreezeEngineControl = new Components.EngineConfig.EngineControls.FreezeEngineControl(this);
            this.Controls.Add(FreezeEngineControl);

            NightmareEngineControl = new Components.EngineConfig.EngineControls.NightmareEngineControl(engineControlLocation);
            this.Controls.Add(NightmareEngineControl);

            HellgenieEngineControl = new Components.EngineConfig.EngineControls.HellgenieEngineControl(this);
            this.Controls.Add(HellgenieEngineControl);

            distortionEngineControl = new Components.EngineConfig.EngineControls.DistortionEngineControl(engineControlLocation);
            this.Controls.Add(distortionEngineControl);

            customEngineControl = new Components.EngineConfig.EngineControls.CustomEngineControl(engineControlLocation);
            this.Controls.Add(customEngineControl);

            PipeEngineControl = new Components.EngineConfig.EngineControls.PipeEngineControl(this);
            this.Controls.Add(PipeEngineControl);

            BlastGeneratorEngineControl = new Components.EngineConfig.EngineControls.BlastGeneratorEngineControl(engineControlLocation);
            this.Controls.Add(BlastGeneratorEngineControl);

            VectorEngineControl = new Components.EngineConfig.EngineControls.VectorEngineControl(this);
            this.Controls.Add(VectorEngineControl);

            ClusterEngineControl = new Components.EngineConfig.EngineControls.ClusterEngineControl(engineControlLocation);
            this.Controls.Add(ClusterEngineControl);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {

            var handler = new EventHandler<Components.Controls.ValueUpdateEventArgs<decimal>>(HandleAlignmentChange);
            nmAlignment.ValueChanged += handler;
            nmAlignment.registerSlave(S.GET<CustomEngineConfigForm>().nmAlignment, handler);

            cbSelectedEngine.SelectedIndex = 0;
            cbCustomPrecision.SelectedIndex = 0;

            if (RtcCore.LimiterListBindingSource.Count > 0)
            {
                UpdateVectorLimiterList(VectorEngineControl.cbVectorLimiterList, null);
                UpdateVectorLimiterList(ClusterEngineControl.cbClusterLimiterList, null);
            }
            if (RtcCore.ValueListBindingSource.Count > 0)
            {
                UpdateVectorValueList(VectorEngineControl.cbVectorValueList, null);
            }
        }

        private void HandleAlignmentChange(object sender, Components.Controls.ValueUpdateEventArgs<decimal> e)
        {
            RtcCore.Alignment = Convert.ToInt32(nmAlignment.Value);
        }

        private void UpdateEngine(object sender, EventArgs e)
        {
            NightmareEngineControl.Visible = false;
            HellgenieEngineControl.Visible = false;
            distortionEngineControl.Visible = false;
            FreezeEngineControl.Visible = false;
            PipeEngineControl.Visible = false;
            VectorEngineControl.Visible = false;
            ClusterEngineControl.Visible = false;
            BlastGeneratorEngineControl.Visible = false;
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
                    NightmareEngineControl.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Hellgenie Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.HELLGENIE;
                    HellgenieEngineControl.Visible = true;
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
                    FreezeEngineControl.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Pipe Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.PIPE;
                    PipeEngineControl.Visible = true;
                    cbCustomPrecision.Enabled = true;

                    S.GET<CoreForm>().btnAutoCorrupt.Visible = AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true;
                    break;

                case "Vector Engine":
                    RtcCore.SelectedEngine = CorruptionEngine.VECTOR;
                    nmAlignment.Maximum = 3;
                    VectorEngineControl.Visible = true;

                    if (VectorEngineControl.cbVectorUnlockPrecision.Checked)
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
                    ClusterEngineControl.Visible = true;

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
                    BlastGeneratorEngineControl.Visible = true;

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
            PipeEngineControl.cbLockPipes.Checked = enabled;
            dontUpdate = false;
        }

        public void SetRewindBoxes(bool enabled)
        {
            dontUpdate = true;
            S.GET<SettingsCorruptForm>().SetRewindBoxes(enabled);
            FreezeEngineControl.cbClearFreezesOnRewind.Checked = enabled;
            HellgenieEngineControl.cbClearCheatsOnRewind.Checked = enabled;
            PipeEngineControl.cbClearPipesOnRewind.Checked = enabled;
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

            StepActions.ClearStepActionsOnRewind = FreezeEngineControl.cbClearFreezesOnRewind.Checked;
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
            NightmareEngineControl.UpdateMinMaxBoxes(precision);
            HellgenieEngineControl.UpdateMinMaxBoxes(precision);
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
            if (VectorEngineControl.cbVectorUnlockPrecision.Checked)
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
