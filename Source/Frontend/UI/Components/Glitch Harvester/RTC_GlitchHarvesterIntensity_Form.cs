namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using static RTCV.UI.UI_Extensions;

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
                var prevState = multiTB_Intensity.FirstLoadDone;
                multiTB_Intensity.FirstLoadDone = false;
                multiTB_Intensity.Maximum = maxintensity;
                multiTB_Intensity.FirstLoadDone = prevState;
            }
        }
    }
}
