namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.UI.Components.EngineConfig;

    public partial class NightmareEngineControl : EngineConfigControl
    {
        private bool updatingMinMax = false;

        internal NightmareEngineControl(Point location) : base(location)
        {
            InitializeComponent();

            cbBlastType.SelectedIndex = 0;
            nmMinValueNightmare.ValueChanged += UpdateMinValue;
            nmMaxValueNightmare.ValueChanged += UpdateMaxValue;
        }

        private void UpdateMinValue(object sender, EventArgs e)
        {
            //We don't want to trigger this if it caps when stepping downwards
            if (updatingMinMax)
            {
                return;
            }

            ulong value = Convert.ToUInt64(nmMinValueNightmare.Value);

            switch (RtcCore.CurrentPrecision)
            {
                case 1:
                    NightmareEngine.MinValue8Bit = value;
                    break;
                case 2:
                    NightmareEngine.MinValue16Bit = value;
                    break;
                case 4:
                    NightmareEngine.MinValue32Bit = value;
                    break;
                case 8:
                    NightmareEngine.MinValue64Bit = value;
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

            ulong value = Convert.ToUInt64(nmMaxValueNightmare.Value);

            switch (RtcCore.CurrentPrecision)
            {
                case 1:
                    NightmareEngine.MaxValue8Bit = value;
                    break;
                case 2:
                    NightmareEngine.MaxValue16Bit = value;
                    break;
                case 4:
                    NightmareEngine.MaxValue32Bit = value;
                    break;
                case 8:
                    NightmareEngine.MaxValue64Bit = value;
                    break;
            }
        }

        private void UpdateBlastType(object sender, EventArgs e)
        {
            switch (cbBlastType.SelectedItem.ToString())
            {
                case "RANDOM":
                    CorruptCore.NightmareEngine.Algo = NightmareAlgo.RANDOM;
                    nmMinValueNightmare.Enabled = true;
                    nmMaxValueNightmare.Enabled = true;
                    break;

                case "RANDOMTILT":
                    CorruptCore.NightmareEngine.Algo = NightmareAlgo.RANDOMTILT;
                    nmMinValueNightmare.Enabled = true;
                    nmMaxValueNightmare.Enabled = true;
                    break;

                case "TILT":
                    CorruptCore.NightmareEngine.Algo = NightmareAlgo.TILT;
                    nmMinValueNightmare.Enabled = false;
                    nmMaxValueNightmare.Enabled = false;
                    break;
            }
        }

        internal void UpdateMinMaxBoxes(int precision)
        {
            updatingMinMax = true;
            switch (precision)
            {
                case 1:
                    nmMinValueNightmare.Maximum = byte.MaxValue;
                    nmMaxValueNightmare.Maximum = byte.MaxValue;

                    nmMinValueNightmare.Value = CorruptCore.NightmareEngine.MinValue8Bit;
                    nmMaxValueNightmare.Value = CorruptCore.NightmareEngine.MaxValue8Bit;

                    break;

                case 2:
                    nmMinValueNightmare.Maximum = ushort.MaxValue;
                    nmMaxValueNightmare.Maximum = ushort.MaxValue;

                    nmMinValueNightmare.Value = CorruptCore.NightmareEngine.MinValue16Bit;
                    nmMaxValueNightmare.Value = CorruptCore.NightmareEngine.MaxValue16Bit;

                    break;
                case 4:
                    nmMinValueNightmare.Maximum = uint.MaxValue;
                    nmMaxValueNightmare.Maximum = uint.MaxValue;

                    nmMinValueNightmare.Value = CorruptCore.NightmareEngine.MinValue32Bit;
                    nmMaxValueNightmare.Value = CorruptCore.NightmareEngine.MaxValue32Bit;

                    break;
                case 8:
                    nmMinValueNightmare.Maximum = ulong.MaxValue;
                    nmMaxValueNightmare.Maximum = ulong.MaxValue;

                    nmMinValueNightmare.Value = CorruptCore.NightmareEngine.MinValue64Bit;
                    nmMaxValueNightmare.Value = CorruptCore.NightmareEngine.MaxValue64Bit;

                    break;
            }
            updatingMinMax = false;
        }
    }
}
