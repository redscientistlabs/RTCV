namespace RTCV.UI.Components.EngineConfig.Engines
{
    public partial class HellgenieEngine : EngineConfigControl
    {
        public HellgenieEngine(CorruptionEngineForm parent)
        {
            InitializeComponent();

            cbClearCheatsOnRewind.CheckedChanged += parent.OnClearRewindToggle;
            btnClearCheats.Click += parent.ClearCheats;
        }
    }
}
