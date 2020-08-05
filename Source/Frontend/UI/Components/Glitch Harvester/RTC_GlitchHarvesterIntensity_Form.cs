namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class RTC_GlitchHarvesterIntensity_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_GlitchHarvesterIntensity_Form()
        {
            InitializeComponent();
            popoutAllowed = true;

            multiTB_Intensity.ValueChanged += (sender, args) => CorruptCore.RtcCore.Intensity = multiTB_Intensity.Value;
        }

        private void RTC_GlitchHarvesterIntensity_Form_Shown(object sender, EventArgs e)
        {
            object paramValue = AllSpec.VanguardSpec[VSPEC.OVERRIDE_DEFAULTMAXINTENSITY];

            if (paramValue != null && paramValue is int maxintensity)
            {
                multiTB_Intensity.SetMaximum(maxintensity, false);
            }
        }
    }
}
