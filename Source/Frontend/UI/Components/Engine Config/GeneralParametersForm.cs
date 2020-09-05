namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.Common;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.UI.Modular;

    public partial class GeneralParametersForm : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public GeneralParametersForm()
        {
            InitializeComponent();
            multiTB_Intensity.ValueChanged += (sender, args) => RtcCore.Intensity = multiTB_Intensity.Value;
            multiTB_Intensity.registerSlave(S.GET<RTC_GlitchHarvesterIntensity_Form>().multiTB_Intensity);

            multiTB_ErrorDelay.ValueChanged += (sender, args) => RtcCore.ErrorDelay = multiTB_ErrorDelay.Value;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            cbBlastRadius.SelectedIndex = 0;
        }

        private void OnBlastRadiusSelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbBlastRadius.SelectedItem.ToString())
            {
                case "SPREAD":
                    RtcCore.Radius = BlastRadius.SPREAD;
                    break;

                case "CHUNK":
                    RtcCore.Radius = BlastRadius.CHUNK;
                    break;

                case "BURST":
                    RtcCore.Radius = BlastRadius.BURST;
                    break;

                case "NORMALIZED":
                    RtcCore.Radius = BlastRadius.NORMALIZED;
                    break;

                case "PROPORTIONAL":
                    RtcCore.Radius = BlastRadius.PROPORTIONAL;
                    break;

                case "EVEN":
                    RtcCore.Radius = BlastRadius.EVEN;
                    break;
            }
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
