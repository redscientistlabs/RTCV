namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class SettingsAboutForm : ComponentForm, IAutoColorize, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public SettingsAboutForm()
        {
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            lbVersion.Text += RtcCore.RtcVersion;
            lbProcess.Text += (RtcCore.Attached ? "Attached mode" : "Detached mode");
            lbConnectedTo.Text += (string)NetCore.AllSpec.VanguardSpec?[VSPEC.NAME] ?? "Not Connected";
        }

        private void OpenRTCVRepo(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/redscientistlabs/RTCV");
        }

        private void OpenRTCHome(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://redscientist.com/rtc");
        }
    }
}
