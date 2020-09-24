namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using System;
    using RTCV.Common;

    internal partial class CustomEngine : EngineConfigControl
    {
        internal CustomEngine()
        {
            InitializeComponent();
        }
        private void OpenCustomEngine(object sender, EventArgs e)
        {
            S.GET<CustomEngineConfigForm>().Show();
            S.GET<CustomEngineConfigForm>().Focus();
        }
    }
}
