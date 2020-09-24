namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    internal partial class VectorEngineControl : EngineConfigControl
    {
        internal VectorEngineControl(CorruptionEngineForm parent)
        {
            InitializeComponent();

            cbVectorLimiterList.SelectedIndexChanged += parent.UpdateVectorLimiterList;
            cbVectorValueList.SelectedIndexChanged += parent.UpdateVectorValueList;
        }
    }
}
