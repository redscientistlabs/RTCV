namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using RTCV.UI.Modular;
    using RTCV.NetCore;

    public partial class SettingsTestForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public SettingsTestForm()
        {
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
        }

        private void OpenRTCVRepo(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/redscientistlabs/RTCV");
        }

        private void OpenRTCHome(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://redscientist.com/rtc");
        }

        private void btnNetcoreTest_Click(object sender, EventArgs e)
        {
                LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, "TEST");
        }

        private void btnTestSubform_Click(object sender, EventArgs e)
        {
            //test button, loads a dummy form in SubForm mode

            var f = S.GET<ComponentFormSubForm>();
            CoreForm.cfForm.OpenSubForm(f, true);
        }

        private void btnTestLockdown_Click(object sender, EventArgs e)
        {
            UICore.LockInterface();
            DefaultGrids.connectionStatus.LoadToMain();
        }

        private void btnOpenCloudDebug_Click(object sender, EventArgs e)
        {
            CoreForm.ForceCloudDebug();
        }
    }
}
