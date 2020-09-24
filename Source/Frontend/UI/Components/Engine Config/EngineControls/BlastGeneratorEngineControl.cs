namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using System;
    using RTCV.Common;

    internal partial class BlastGeneratorEngineControl : EngineConfigControl
    {
        internal BlastGeneratorEngineControl()
        {
            InitializeComponent();
        }

        private void OpenBlastGenerator(object sender, EventArgs e)
        {
            if (S.GET<BlastGeneratorForm>() != null)
            {
                S.GET<BlastGeneratorForm>().Close();
            }

            S.SET(new BlastGeneratorForm());
            S.GET<BlastGeneratorForm>().LoadNoStashKey();
        }
    }
}
