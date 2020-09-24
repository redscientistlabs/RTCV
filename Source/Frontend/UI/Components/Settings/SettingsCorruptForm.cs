namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class SettingsCorruptForm : ComponentForm, IBlockable
    {
        public SettingsCorruptForm()
        {
            InitializeComponent();

            var handler = new EventHandler<Components.Controls.ValueUpdateEventArgs<decimal>>(UpdateMaxInfiniteStepUnits);
            nmMaxInfiniteStepUnits.ValueChanged += handler;
            nmMaxInfiniteStepUnits.registerSlave(S.GET<CorruptionEngineForm>().updownMaxCheats, handler);
            nmMaxInfiniteStepUnits.registerSlave(S.GET<CorruptionEngineForm>().gbFreezeEngine.updownMaxFreeze, handler);
            nmMaxInfiniteStepUnits.registerSlave(S.GET<CorruptionEngineForm>().updownMaxPipes, handler);
            nmMaxInfiniteStepUnits.registerSlave(S.GET<CustomEngineConfigForm>().updownMaxInfiniteUnits, handler);
            nmMaxInfiniteStepUnits.registerSlave(S.GET<SimpleModeForm>().updownMaxInfiniteUnits, handler);

            cbRerollAddress.Checked = RtcCore.RerollAddress;
            cbRerollSourceAddress.Checked = RtcCore.RerollSourceAddress;

            cbRerollDomain.Checked = RtcCore.RerollDomain;
            cbRerollSourceDomain.Checked = RtcCore.RerollSourceDomain;

            cbRerollFollowsCustom.Checked = RtcCore.RerollFollowsCustomEngine;
            cbIgnoreUnitOrigin.Checked = RtcCore.RerollIgnoresOriginalSource;
        }

        private void UpdateMaxInfiniteStepUnits(object sender, EventArgs e)
        {
            StepActions.MaxInfiniteBlastUnits = Convert.ToInt32(nmMaxInfiniteStepUnits.Value);
        }

        private void UpdateRerollSourceAddress(object sender, EventArgs e)
        {
            RtcCore.RerollSourceAddress = cbRerollSourceAddress.Checked;
            if (!cbRerollSourceAddress.Checked)
            {
                cbRerollSourceDomain.Checked = false;
                cbRerollSourceDomain.Enabled = false;
            }
            else
            {
                cbRerollSourceDomain.Enabled = true;
            }
        }

        private void UpdateRerollDomain(object sender, EventArgs e)
        {
            RtcCore.RerollDomain = cbRerollDomain.Checked;
        }

        private void UpdateRerollSourceDomain(object sender, EventArgs e)
        {
            RtcCore.RerollSourceDomain = cbRerollSourceDomain.Checked;
        }

        private void UpdateRerollAddress(object sender, EventArgs e)
        {
            RtcCore.RerollAddress = cbRerollAddress.Checked;
            if (!cbRerollAddress.Checked)
            {
                cbRerollDomain.Checked = false;
                cbRerollDomain.Enabled = false;
            }
            else
            {
                cbRerollDomain.Enabled = true;
            }
        }

        private void UpdateRerollFollowsCustom(object sender, EventArgs e)
        {
            RtcCore.RerollFollowsCustomEngine = cbRerollFollowsCustom.Checked;
        }

        private void CBRerollIgnoresOriginalSource(object sender, EventArgs e)
        {
            RtcCore.RerollIgnoresOriginalSource = cbIgnoreUnitOrigin.Checked;
        }

        public void SetRewindBoxes(bool enabled)
        {
            DontUpdateSpec = true;
            cbClearStepUnitsOnRewind.Checked = enabled;
            DontUpdateSpec = false;
        }

        public void SetLockBoxes(bool enabled)
        {
            DontUpdateSpec = true;
            cbLockUnits.Checked = enabled;
            DontUpdateSpec = false;
        }

        private bool DontUpdateSpec;

        private void UpdateClearStepUnitsOnRewind(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            S.GET<CorruptionEngineForm>().SetRewindBoxes(cbClearStepUnitsOnRewind.Checked);
            S.GET<CustomEngineConfigForm>().SetRewindBoxes(cbClearStepUnitsOnRewind.Checked);
            S.GET<SimpleModeForm>().SetRewindBoxes(cbClearStepUnitsOnRewind.Checked);

            StepActions.ClearStepActionsOnRewind = cbClearStepUnitsOnRewind.Checked;
        }

        private void UpdateLockUnits(object sender, EventArgs e)
        {
            if (DontUpdateSpec)
            {
                return;
            }

            S.GET<CorruptionEngineForm>().SetLockBoxes(cbLockUnits.Checked);

            StepActions.LockExecution = cbLockUnits.Checked;
        }
    }
}
