namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using RTCV.NetCore;

    public partial class DistortionEngineControl : EngineConfigControl
    {
        public DistortionEngineControl()
        {
            InitializeComponent();
        }

        private void ResyncDistortionEngine(object sender, System.EventArgs e)
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearStepBlastUnits, null, true);
        }
    }
}
