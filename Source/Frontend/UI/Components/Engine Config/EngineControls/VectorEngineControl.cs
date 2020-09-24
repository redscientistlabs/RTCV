namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    internal partial class VectorEngineControl : EngineConfigControl
    {
        internal VectorEngineControl(CorruptionEngineForm parent)
        {
            InitializeComponent();

            cbVectorValueList.DataSource = null;
            cbVectorLimiterList.DataSource = null;

            cbVectorValueList.DisplayMember = "Name";
            cbVectorLimiterList.DisplayMember = "Name";
            cbVectorValueList.ValueMember = "Value";
            cbVectorLimiterList.ValueMember = "Value";

            cbVectorLimiterList.SelectedIndexChanged += parent.UpdateVectorLimiterList;
            cbVectorValueList.SelectedIndexChanged += parent.UpdateVectorValueList;
            cbVectorUnlockPrecision.CheckedChanged += parent.UpdateVectorUnlockPrecision;
        }
    }
}
