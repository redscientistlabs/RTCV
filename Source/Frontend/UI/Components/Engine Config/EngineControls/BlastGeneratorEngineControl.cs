namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using System;
    using RTCV.Common;

    internal partial class BlastGeneratorEngine : EngineConfigControl
    {
        internal BlastGeneratorEngine()
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
