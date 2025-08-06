namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class GlitchHarvesterIntensityForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public GlitchHarvesterIntensityForm()
        {
            InitializeComponent();
            PopoutAllowed = true;

            multiTB_Intensity.ValueChanged += (sender, args) => RtcCore.Intensity = multiTB_Intensity.Value;
        }

        public void UpdateMaxIntensity()
        {
            object paramValue = AllSpec.VanguardSpec[VSPEC.OVERRIDE_DEFAULTMAXINTENSITY];

            if (paramValue is int maxIntensity)
                multiTB_Intensity.SetMaximum(maxIntensity, false);
            else
                multiTB_Intensity.SetMaximum(65535, false);

            if (multiTB_Intensity.Value > multiTB_Intensity.Maximum)
                multiTB_Intensity.Value = multiTB_Intensity.Maximum;
        }

        private void OnFormShown(object sender, EventArgs e)
        {
            UpdateMaxIntensity();
        }
    }
}
