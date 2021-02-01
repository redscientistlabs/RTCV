namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using System;
    using System.Drawing;
    using RTCV.Common;
    using RTCV.CorruptCore;
    using RTCV.NetCore;

    public partial class PipeEngineControl : EngineConfigControl
    {
        internal PipeEngineControl(CorruptionEngineForm parent) : base(new Point(parent.gbSelectedEngine.Location.X, parent.gbSelectedEngine.Location.Y))
        {
            InitializeComponent();

            cbClearPipesOnRewind.CheckedChanged += parent.OnClearRewindToggle;
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
