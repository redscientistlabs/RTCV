namespace RTCV.UI
{
    using System;
    using System.Drawing;
    using System.Numerics;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;

    public partial class CustomEngineConfigForm : Form, IAutoColorize
    {
        private bool updatingMinMax = false;
        private bool DontUpdateSpec = false;

        public CustomEngineConfigForm()
        {
            InitializeComponent();
            RTC_CustomEngine.InitTemplates();

            this.GotFocus += (o, e) => this.Refresh();

            foreach (var k in RTC_CustomEngine.Name2TemplateDico.Keys)
            {
                cbSelectedTemplate.Items.Add(k);
            }

            cbSelectedTemplate.SelectedIndex = 0;

            cbCustomPrecision.SelectedIndexChanged += HandleCustomPrecisionSelectionChange;
            nmAlignment.ValueChanged += HandleAlignmentChange;
        }

        private void HandleAlignmentChange(object sender, Components.Controls.ValueUpdateEventArgs<decimal> e)
        {
            RtcCore.Alignment = Convert.ToInt32(nmAlignment.Value);
            S.GET<CorruptionEngineForm>().nmAlignment.Value = nmAlignment.Value;
        }

        private void HandleCustomPrecisionSelectionChange(object sender, EventArgs e)
        {
            cbCustomPrecision.Enabled = false;
            S.GET<CorruptionEngineForm>().cbCustomPrecision.Enabled = false;
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
                    if (!DontUpdateSpec)
                    {
                        RtcCore.CurrentPrecision = precision;
                    }

                    UpdateMinMaxBoxes(precision);
                    nmAlignment.Maximum = precision - 1;
                    S.GET<CorruptionEngineForm>().cbCustomPrecision.SelectedIndex = cbCustomPrecision.SelectedIndex;
                }
            }
            finally
            {
                cbCustomPrecision.Enabled = true;
                S.GET<CorruptionEngineForm>().cbCustomPrecision.Enabled = true;
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            cbValueList.DisplayMember = "Name";
            cbLimiterList.DisplayMember = "Name";

            cbValueList.ValueMember = "Value";
            cbLimiterList.ValueMember = "Value";

            //Do this here as if it's stuck into the designer, it keeps defaulting out
            cbValueList.DataSource = RtcCore.ValueListBindingSource;
            cbLimiterList.DataSource = RtcCore.LimiterListBindingSource;

            if (RtcCore.ValueListBindingSource.Count > 0)
            {
                HandleValueListSelectionChange(cbValueList, null);
            }
            if (RtcCore.LimiterListBindingSource.Count > 0)
            {
                HandleLimiterListSelectionChange(cbLimiterList, null);
            }

            cbCustomPrecision.SelectedIndex = 0;
            setFlavorText();
        }

        private void setFlavorText()
        {
            var text = new[]
            {
                "Make your own engine",
                "Yes it probably works",
                "Never ask me for anything ever again",
                "Imagine using default engines",
                "I just needed to fill this empty space",
                "I've run out of ideas for flavor text",
            };

            Random rnd = new Random();
            lbFlavorText.Text = text[rnd.Next(0, text.Length)];
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                this.Hide();
                return;
            }
        }

        //I'm using if-else's rather than switch statements on purpose.
        //The switch statements required more lines and were harder to read.
        private void HandleUnitSourceChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            updateUILock();
        }

        private void updateUILock()
        {
            if (rbUnitSourceStore.Checked)
            {
                RTC_CustomEngine.Source = BlastUnitSource.STORE;
                gbValueSettings.Enabled = false;
                gbStoreSettings.Enabled = true;
                gbStoreCompare.Enabled = true;
            }
            else if (rbUnitSourceValue.Checked)
            {
                RTC_CustomEngine.Source = BlastUnitSource.VALUE;
                gbValueSettings.Enabled = true;
                gbStoreSettings.Enabled = false;
                gbStoreCompare.Enabled = false;
            }
        }

        private void HandleValueSourceChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            if (rbRandom.Checked)
            {
                RTC_CustomEngine.ValueSource = CustomValueSource.RANDOM;
            }
            else if (rbValueList.Checked)
            {
                RTC_CustomEngine.ValueSource = CustomValueSource.VALUELIST;
            }
            else if (rbRange.Checked)
            {
                RTC_CustomEngine.ValueSource = CustomValueSource.RANGE;
            }
        }

        private void HandleStoreTimeChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            if (rbStoreImmediate.Checked)
            {
                RTC_CustomEngine.StoreTime = StoreTime.IMMEDIATE;
            }
            else if (rbStoreFirstExecute.Checked)
            {
                RTC_CustomEngine.StoreTime = StoreTime.PREEXECUTE;
            }
        }

        private void HandleStoreAddressChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            if (rbStoreRandom.Checked)
            {
                RTC_CustomEngine.StoreAddress = CustomStoreAddress.RANDOM;
            }
            else if (rbStoreSame.Checked)
            {
                RTC_CustomEngine.StoreAddress = CustomStoreAddress.SAME;
            }
        }

        private void HandleStoreTypeChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            if (rbStoreOnce.Checked)
            {
                RTC_CustomEngine.StoreType = StoreType.ONCE;
            }

            if (rbStoreStep.Checked)
            {
                RTC_CustomEngine.StoreType = StoreType.CONTINUOUS;
            }
        }

        private void HandleMinValueChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            //We don't want to trigger this if it caps when stepping downwards
            if (updatingMinMax)
            {
                return;
            }

            ulong value = Convert.ToUInt64(nmMinValue.Value);

            switch (RtcCore.CurrentPrecision)
            {
                case 1:
                    RTC_CustomEngine.MinValue8Bit = value;
                    break;
                case 2:
                    RTC_CustomEngine.MinValue16Bit = value;
                    break;
                case 4:
                    RTC_CustomEngine.MinValue32Bit = value;
                    break;
                case 8:
                    RTC_CustomEngine.MinValue64Bit = value;
                    break;
            }
        }

        private void HandleMaxValueChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            //We don't want to trigger this if it caps when stepping downwards
            if (updatingMinMax)
            {
                return;
            }

            ulong value = Convert.ToUInt64(nmMaxValue.Value);

            switch (RtcCore.CurrentPrecision)
            {
                case 1:
                    RTC_CustomEngine.MaxValue8Bit = value;
                    break;
                case 2:
                    RTC_CustomEngine.MaxValue16Bit = value;
                    break;
                case 4:
                    RTC_CustomEngine.MaxValue32Bit = value;
                    break;
                case 8:
                    RTC_CustomEngine.MaxValue64Bit = value;
                    break;
            }
        }

        public void SetRewindBoxes(bool enabled)
        {
            DontUpdateSpec = true;
            cbClearRewind.Checked = enabled;
            DontUpdateSpec = false;
        }

        private void HandleClearRewindChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            S.GET<CorruptionEngineForm>().SetRewindBoxes(cbClearRewind.Checked);
            S.GET<SimpleModeForm>().SetRewindBoxes(cbClearRewind.Checked);

            StepActions.ClearStepActionsOnRewind = cbClearRewind.Checked;
        }

        private void HandleLoopUnitChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            RTC_CustomEngine.Loop = cbLoopUnit.Checked;
        }

        private void HandleValueListSelectionChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            RTC_CustomEngine.ValueListHash = (string)cbValueList.SelectedValue;
        }

        private void HandleLimiterListSelectionChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            RTC_CustomEngine.LimiterListHash = (string)cbLimiterList.SelectedValue;
        }

        private void HandleLimiterTimeSelectionChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            if (rbLimiterNone.Checked)
            {
                RTC_CustomEngine.LimiterTime = LimiterTime.NONE;
            }
            else if (rbLimiterGenerate.Checked)
            {
                RTC_CustomEngine.LimiterTime = LimiterTime.GENERATE;
            }
            else if (rbLimiterFirstExecute.Checked)
            {
                RTC_CustomEngine.LimiterTime = LimiterTime.PREEXECUTE;
            }
            else if (rbLimiterExecute.Checked)
            {
                RTC_CustomEngine.LimiterTime = LimiterTime.EXECUTE;
            }
        }

        private void HandleStoreLimiterModeChange(object sender, EventArgs e)
        {
            if (rbStoreModeAddress.Checked)
            {
                RTC_CustomEngine.StoreLimiterSource = StoreLimiterSource.ADDRESS;
            }
            else if (rbStoreModeSource.Checked)
            {
                RTC_CustomEngine.StoreLimiterSource = StoreLimiterSource.SOURCEADDRESS;
            }
            else if (rbStoreModeBoth.Checked)
            {
                RTC_CustomEngine.StoreLimiterSource = StoreLimiterSource.BOTH;
            }
        }

        private void ClearActive(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
        }

        private void HandleLifetimeChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            RTC_CustomEngine.Lifetime = Convert.ToInt32(nmLifetime.Value);
        }

        private void HandleDelayChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            RTC_CustomEngine.Delay = Convert.ToInt32(nmDelay.Value);
        }

        private void HandleTiltChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            RTC_CustomEngine.TiltValue = (BigInteger)nmTilt.Value;
        }

        public void UpdateMinMaxBoxes(int precision)
        {
            updatingMinMax = true;
            switch (precision)
            {
                case 1:
                    nmMinValue.Maximum = byte.MaxValue;
                    nmMaxValue.Maximum = byte.MaxValue;

                    nmMinValue.Value = RTC_CustomEngine.MinValue8Bit;
                    nmMaxValue.Value = RTC_CustomEngine.MaxValue8Bit;
                    break;

                case 2:
                    nmMinValue.Maximum = ushort.MaxValue;
                    nmMaxValue.Maximum = ushort.MaxValue;

                    nmMinValue.Value = RTC_CustomEngine.MinValue16Bit;
                    nmMaxValue.Value = RTC_CustomEngine.MaxValue16Bit;
                    break;
                case 4:
                    nmMinValue.Maximum = uint.MaxValue;
                    nmMaxValue.Maximum = uint.MaxValue;

                    nmMinValue.Value = RTC_CustomEngine.MinValue32Bit;
                    nmMaxValue.Value = RTC_CustomEngine.MaxValue32Bit;
                    break;
                case 8:
                    nmMinValue.Maximum = ulong.MaxValue;
                    nmMaxValue.Maximum = ulong.MaxValue;

                    nmMinValue.Value = RTC_CustomEngine.MinValue64Bit;
                    nmMaxValue.Value = RTC_CustomEngine.MaxValue64Bit;
                    break;
            }
            updatingMinMax = false;
        }

        private void HandleLimiterInvertedChange(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            RTC_CustomEngine.LimiterInverted = cbLimiterInverted.Checked;
        }

        private void HandleSelectedTemplateChange(object sender, EventArgs e)
        {
            PartialSpec spec = new PartialSpec("RTCSpec");

            bool readOnlyTemplate = false;

            switch (cbSelectedTemplate.SelectedItem.ToString())
            {
                case "Nightmare Engine":
                case "Hellgenie Engine":
                case "Distortion Engine":
                case "Freeze Engine":
                case "Pipe Engine":
                case "Vector Engine":
                    readOnlyTemplate = true;
                    break;
            }

            if (readOnlyTemplate)
            {
                btnCustomTemplateSave.Enabled = false;
                btnCustomTemplateSave.BackColor = Color.LightGray;
                btnCustomTemplateSave.ForeColor = Color.DimGray;
            }
            else
            {
                btnCustomTemplateSave.Enabled = true;
                btnCustomTemplateSave.BackColor = Color.Tomato;
                btnCustomTemplateSave.ForeColor = Color.Black;
            }

            if (RTC_CustomEngine.LoadTemplate(cbSelectedTemplate.SelectedItem.ToString()))
            {
                AllSpec.CorruptCoreSpec.Update(spec);
                RestoreUIStateFromSpec();
                updateUILock();
                Refresh();
            }
        }

        private void LoadCustomTemplate(object sender, EventArgs e)
        {
            PartialSpec spec = RTC_CustomEngine.LoadTemplateFile();

            if (spec == null)
            {
                return;
            }

            RTC_CustomEngine.Name2TemplateDico[spec[RTCSPEC.CUSTOM_NAME].ToString()] = spec;
            AllSpec.CorruptCoreSpec.Update(spec);
            RestoreUIStateFromSpec();
            Refresh();
            if (!cbSelectedTemplate.Items.Contains(spec[RTCSPEC.CUSTOM_NAME].ToString()))
            {
                cbSelectedTemplate.Items.Add(spec[RTCSPEC.CUSTOM_NAME].ToString());
            }

            cbSelectedTemplate.SelectedItem = spec[RTCSPEC.CUSTOM_NAME].ToString();
        }

        private void SaveAsCustomTemplate(object sender, EventArgs e)
        {
            string TemplateName = RTC_CustomEngine.SaveTemplateFile(true);

            if (string.IsNullOrWhiteSpace(TemplateName))
            {
                return;
            }

            if (!cbSelectedTemplate.Items.Contains(TemplateName))
            {
                cbSelectedTemplate.Items.Add(TemplateName);
            }

            cbSelectedTemplate.SelectedItem = TemplateName;

            btnCustomTemplateSave.Enabled = true;
            btnCustomTemplateSave.BackColor = Color.Tomato;
            btnCustomTemplateSave.ForeColor = Color.Black;
        }

        private void SaveCustomTemplate(object sender, EventArgs e)
        {
            RTC_CustomEngine.SaveTemplateFile(false);
        }

        private void RestoreUIStateFromSpec()
        {
            try
            {
                DontUpdateSpec = true;

                switch (RTC_CustomEngine.Source)
                {
                    case (BlastUnitSource.STORE):
                        rbUnitSourceStore.Checked = true;
                        break;
                    case (BlastUnitSource.VALUE):
                        rbUnitSourceValue.Checked = true;
                        break;
                }

                switch (RTC_CustomEngine.ValueSource)
                {
                    case (CustomValueSource.RANDOM):
                        rbRandom.Checked = true;
                        break;
                    case (CustomValueSource.VALUELIST):
                        rbValueList.Checked = true;
                        break;
                    case (CustomValueSource.RANGE):
                        rbRange.Checked = true;
                        break;
                }

                switch (RTC_CustomEngine.StoreTime)
                {
                    case (StoreTime.IMMEDIATE):
                        rbStoreImmediate.Checked = true;
                        break;
                    case (StoreTime.PREEXECUTE):
                        rbStoreFirstExecute.Checked = true;
                        break;
                }

                switch (RTC_CustomEngine.StoreAddress)
                {
                    case (CustomStoreAddress.RANDOM):
                        rbStoreRandom.Checked = true;
                        break;
                    case (CustomStoreAddress.SAME):
                        rbStoreSame.Checked = true;
                        break;
                }

                switch (RTC_CustomEngine.StoreType)
                {
                    case (StoreType.ONCE):
                        rbStoreOnce.Checked = true;
                        break;
                    case (StoreType.CONTINUOUS):
                        rbStoreStep.Checked = true;
                        break;
                }

                switch (RTC_CustomEngine.LimiterTime)
                {
                    case (LimiterTime.NONE):
                        rbLimiterNone.Checked = true;
                        break;
                    case (LimiterTime.GENERATE):
                        rbLimiterGenerate.Checked = true;
                        break;
                    case (LimiterTime.PREEXECUTE):
                        rbLimiterFirstExecute.Checked = true;
                        break;
                    case (LimiterTime.EXECUTE):
                        rbLimiterExecute.Checked = true;
                        break;
                }

                switch (RTC_CustomEngine.StoreLimiterSource)
                {
                    case (StoreLimiterSource.ADDRESS):
                        rbStoreModeAddress.Checked = true;
                        break;
                    case (StoreLimiterSource.SOURCEADDRESS):
                        rbStoreModeSource.Checked = true;
                        break;
                    case (StoreLimiterSource.BOTH):
                        rbStoreModeBoth.Checked = true;
                        break;
                }

                cbClearRewind.Checked = StepActions.ClearStepActionsOnRewind;

                cbLoopUnit.Checked = RTC_CustomEngine.Loop;
                cbLimiterInverted.Checked = RTC_CustomEngine.LimiterInverted;

                cbValueList.SelectedValue = RTC_CustomEngine.ValueListHash;
                cbLimiterList.SelectedValue = RTC_CustomEngine.LimiterListHash;

                if (RTC_CustomEngine.TiltValue > (BigInteger)decimal.MaxValue)
                {
                    RTC_CustomEngine.TiltValue = (BigInteger)decimal.MaxValue;
                }

                nmTilt.Value = (decimal)RTC_CustomEngine.TiltValue;
                nmDelay.Value = RTC_CustomEngine.Delay;
                nmLifetime.Value = RTC_CustomEngine.Lifetime;

                UpdateMinMaxBoxes(RtcCore.CurrentPrecision);

                nmAlignment.Maximum = RtcCore.CurrentPrecision - 1;
                nmAlignment.Value = RtcCore.Alignment;

                //Todo - replace this and data-bind it
                switch (RtcCore.CurrentPrecision)
                {
                    case 1:
                        S.GET<CorruptionEngineForm>().cbCustomPrecision.SelectedIndex = 0;
                        break;
                    case 2:
                        S.GET<CorruptionEngineForm>().cbCustomPrecision.SelectedIndex = 1;
                        break;
                    case 4:
                        S.GET<CorruptionEngineForm>().cbCustomPrecision.SelectedIndex = 2;
                        break;
                    case 8:
                        S.GET<CorruptionEngineForm>().cbCustomPrecision.SelectedIndex = 3;
                        break;
                }

                switch (RtcCore.CurrentPrecision)
                {
                    case 1:
                        nmMinValue.Value = RTC_CustomEngine.MinValue8Bit;
                        break;
                    case 2:
                        nmMinValue.Value = RTC_CustomEngine.MinValue16Bit;
                        break;
                    case 4:
                        nmMinValue.Value = RTC_CustomEngine.MinValue32Bit;
                        break;
                    case 8:
                        nmMinValue.Value = RTC_CustomEngine.MinValue64Bit;
                        break;
                }

                switch (RtcCore.CurrentPrecision)
                {
                    case 1:
                        nmMaxValue.Value = RTC_CustomEngine.MaxValue8Bit;
                        break;
                    case 2:
                        nmMaxValue.Value = RTC_CustomEngine.MaxValue16Bit;
                        break;
                    case 4:
                        nmMaxValue.Value = RTC_CustomEngine.MaxValue32Bit;
                        break;
                    case 8:
                        nmMaxValue.Value = RTC_CustomEngine.MaxValue64Bit;
                        break;
                }
            }
            finally
            {
                DontUpdateSpec = false;
                this.Focus();
            }
        }
    }
}
