namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using System;
    using System.Drawing;
    using RTCV.CorruptCore;
    using RTCV.NetCore;

    public partial class DistortionEngineControl : EngineConfigControl
    {
        private bool updatingDelay = false;
        public DistortionEngineControl(Point location) : base(location)
        {
            InitializeComponent();
            nmDistortionDelay.ValueChanged += UpdateDistortionDelay;
        }

        private void UpdateDistortionDelay(object sender, Controls.ValueUpdateEventArgs<decimal> e)
        {
            if (updatingDelay) return;
            DistortionEngine.Delay = (int)nmDistortionDelay.Value;
        }

        private void ResyncDistortionEngine(object sender, System.EventArgs e)
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearStepBlastUnits, null, true);
        }

        public void ResyncEngineUI()
        {
            updatingDelay = true;
            nmDistortionDelay.Value = Math.Max(nmDistortionDelay.Minimum, Math.Min(nmDistortionDelay.Maximum, DistortionEngine.Delay));
            updatingDelay = false;
            //throw new NotImplementedException();
        }
    }
}
