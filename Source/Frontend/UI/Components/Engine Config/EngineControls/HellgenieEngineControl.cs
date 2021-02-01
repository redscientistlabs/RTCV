namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using System;
    using System.Drawing;
    using RTCV.CorruptCore;

    public partial class HellgenieEngineControl : EngineConfigControl
    {
        private bool updatingMinMax = false;

        internal HellgenieEngineControl(CorruptionEngineForm parent) : base(new Point(parent.gbSelectedEngine.Location.X, parent.gbSelectedEngine.Location.Y))
        {
            InitializeComponent();

            cbClearCheatsOnRewind.CheckedChanged += parent.OnClearRewindToggle;
            btnClearCheats.Click += parent.ClearCheats;

            nmMinValueHellgenie.ValueChanged += UpdateMinValue;
            nmMinValueHellgenie.ValueChanged += UpdateMaxValue;
        }

        private void UpdateMinValue(object sender, EventArgs e)
        {
            //We don't want to trigger this if it caps when stepping downwards
            if (updatingMinMax)
            {
                return;
            }

            ulong value = Convert.ToUInt64(nmMinValueHellgenie.Value);

            switch (RtcCore.CurrentPrecision)
            {
                case 1:
                    HellgenieEngine.MinValue8Bit = value;
                    break;
                case 2:
                    HellgenieEngine.MinValue16Bit = value;
                    break;
                case 4:
                    HellgenieEngine.MinValue32Bit = value;
                    break;
                case 8:
                    HellgenieEngine.MinValue64Bit = value;
                    break;
            }
        }

        private void UpdateMaxValue(object sender, EventArgs e)
        {
            //We don't want to trigger this if it caps when stepping downwards
            if (updatingMinMax)
            {
                return;
            }

            ulong value = Convert.ToUInt64(nmMaxValueHellgenie.Value);

            switch (RtcCore.CurrentPrecision)
            {
                case 1:
                    HellgenieEngine.MaxValue8Bit = value;
                    break;
                case 2:
                    HellgenieEngine.MaxValue16Bit = value;
                    break;
                case 4:
                    HellgenieEngine.MaxValue32Bit = value;
                    break;
                case 8:
                    HellgenieEngine.MaxValue64Bit = value;
                    break;
            }
        }

        internal void UpdateMinMaxBoxes(int precision)
        {
            updatingMinMax = true;

            switch (precision)
            {
                case 1:
                    nmMinValueHellgenie.Maximum = byte.MaxValue;
                    nmMaxValueHellgenie.Maximum = byte.MaxValue;

                    nmMinValueHellgenie.Value = CorruptCore.HellgenieEngine.MinValue8Bit;
                    nmMaxValueHellgenie.Value = CorruptCore.HellgenieEngine.MaxValue8Bit;

                    break;

                case 2:
                    nmMinValueHellgenie.Maximum = ushort.MaxValue;
                    nmMaxValueHellgenie.Maximum = ushort.MaxValue;

                    nmMinValueHellgenie.Value = CorruptCore.HellgenieEngine.MinValue16Bit;
                    nmMaxValueHellgenie.Value = CorruptCore.HellgenieEngine.MaxValue16Bit;

                    break;
                case 4:
                    nmMinValueHellgenie.Maximum = uint.MaxValue;
                    nmMaxValueHellgenie.Maximum = uint.MaxValue;

                    nmMinValueHellgenie.Value = CorruptCore.HellgenieEngine.MinValue32Bit;
                    nmMaxValueHellgenie.Value = CorruptCore.HellgenieEngine.MaxValue32Bit;

                    break;
                case 8:
                    nmMinValueHellgenie.Maximum = ulong.MaxValue;
                    nmMaxValueHellgenie.Maximum = ulong.MaxValue;

                    nmMinValueHellgenie.Value = CorruptCore.HellgenieEngine.MinValue64Bit;
                    nmMaxValueHellgenie.Value = CorruptCore.HellgenieEngine.MaxValue64Bit;

                    break;
            }

            updatingMinMax = false;
        }
    }
}
