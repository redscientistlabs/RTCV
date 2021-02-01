namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using System;
    using System.Drawing;
    using RTCV.Common;

    public partial class BlastGeneratorEngineControl : EngineConfigControl
    {
        internal BlastGeneratorEngineControl(Point location) : base(location)
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
