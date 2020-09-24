namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    internal partial class HellgenieEngine : EngineConfigControl
    {
        internal HellgenieEngine(CorruptionEngineForm parent)
        {
            InitializeComponent();

            cbClearCheatsOnRewind.CheckedChanged += parent.OnClearRewindToggle;
            btnClearCheats.Click += parent.ClearCheats;
        }

        internal void UpdateMinMaxBoxes(int precision)
        {
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
        }
    }
}
