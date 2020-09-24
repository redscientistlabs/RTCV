namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using System;
    using RTCV.Common;
    using RTCV.CorruptCore;
    using RTCV.NetCore;

    public partial class PipeEngine : EngineConfigControl
    {
        public PipeEngine()
        {
            InitializeComponent();
        }

        private void ClearPipes(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearStepBlastUnits, null, true);
        }

        private void OnLockPipesToggle(object sender, EventArgs e)
        {
            S.GET<SettingsCorruptForm>().SetLockBoxes(cbLockPipes.Checked);
            StepActions.LockExecution = cbLockPipes.Checked;
        }
    }
}
