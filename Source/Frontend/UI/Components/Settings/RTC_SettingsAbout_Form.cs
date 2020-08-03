namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class RTC_SettingsAbout_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_SettingsAbout_Form()
        {
            InitializeComponent();

            this.undockedSizable = false;
        }

        private void RTC_SettingsAbout_Form_Load(object sender, EventArgs e)
        {
            lbVersion.Text += CorruptCore.RtcCore.RtcVersion;
            lbProcess.Text += (CorruptCore.RtcCore.Attached ? "Attached mode" : "Detached mode");
            lbConnectedTo.Text += (string)NetCore.AllSpec.VanguardSpec?[VSPEC.NAME] ?? "Not Connected";
        }

        private void LbSourceCode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ircluzar/RTCV");
        }

        private void LbRTCHome_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://redscientist.com/rtc");
        }
    }
}
