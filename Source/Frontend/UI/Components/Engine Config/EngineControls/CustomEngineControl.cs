namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using System;
    using System.Drawing;
    using RTCV.Common;

    public partial class CustomEngineControl : EngineConfigControl
    {
        internal CustomEngineControl(Point location) : base(location)
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
