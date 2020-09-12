namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class GlitchHarvesterIntensityForm : ComponentForm, IAutoColorize, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public GlitchHarvesterIntensityForm()
        {
            InitializeComponent();
            popoutAllowed = true;

            multiTB_Intensity.ValueChanged += (sender, args) => RtcCore.Intensity = multiTB_Intensity.Value;
        }

        private void OnFormShown(object sender, EventArgs e)
        {
            object paramValue = AllSpec.VanguardSpec[VSPEC.OVERRIDE_DEFAULTMAXINTENSITY];

            if (paramValue != null && paramValue is int maxintensity)
            {
                multiTB_Intensity.SetMaximum(maxintensity, false);
            }
        }
    }
}
