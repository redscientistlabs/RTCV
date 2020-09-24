namespace RTCV.UI.Components.EngineConfig.Engines
{
    using RTCV.NetCore;

    public partial class DistortionEngine : EngineConfigControl
    {
        public DistortionEngine()
        {
            InitializeComponent();
        }

        private void ResyncDistortionEngine(object sender, System.EventArgs e)
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearStepBlastUnits, null, true);
        }
    }
}
