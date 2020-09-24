namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    internal partial class VectorEngine : EngineConfigControl
    {
        internal VectorEngine(CorruptionEngineForm parent)
        {
            InitializeComponent();

            cbVectorLimiterList.SelectedIndexChanged += parent.UpdateVectorLimiterList;
            cbVectorValueList.SelectedIndexChanged += parent.UpdateVectorValueList;
        }
    }
}
