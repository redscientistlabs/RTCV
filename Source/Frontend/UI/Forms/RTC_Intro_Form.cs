namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.NetCore;
    using RTCV.Common;

    public partial class RTC_Intro_Form : Form, IAutoColorize
    {
        public IntroAction selection = IntroAction.EXIT;

        public RTC_Intro_Form()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                if (CloudDebug.ShowErrorDialog(ex, true) == DialogResult.Abort)
                {
                    throw new RTCV.NetCore.AbortEverythingException();
                }
            }
        }

        private void RTC_Intro_Form_Load(object sender, EventArgs e)
        {
            UICore.SetRTCColor(UICore.GeneralColor, this);
        }

        private void RTC_Intro_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
            {
                return;
            }

            if (selection == IntroAction.EXIT)
            {
                if (UI_VanguardImplementation.connector.netConn.status == NetworkStatus.CONNECTED)
                {
                    LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_EVENT_CLOSEEMULATOR);
                }

                Environment.Exit(0);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            selection = IntroAction.EXIT;
            this.Close();
        }

        private void btnSimpleMode_Click(object sender, EventArgs e)
        {
            selection = IntroAction.SIMPLEMODE;
            Close();
        }

        private void btnNormalMode_Click(object sender, EventArgs e)
        {
            selection = IntroAction.NORMALMODE;
            Close();
        }

        private void cbAgree_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAgree.Checked)
            {
                btnSimpleMode.Visible = true;

                if (btnNormalMode.Text != "Continue")
                {
                    lbStartupMode.Visible = true;
                    btnNormalMode.Visible = true;
                }
            }
        }

        internal void DisplayRtcvDisclaimer(string disclaimer)
        {
            cbAgree.Checked = false;
            btnSimpleMode.Visible = false;
            lbStartupMode.Visible = false;
            btnNormalMode.Visible = false;

            this.Text = "Welcome to RTCV";

            tbDisclaimerText.Text = disclaimer;

            selection = IntroAction.EXIT;
            this.ShowDialog();
        }

        internal void DisplayGenericDisclaimer(string disclaimer, string windowTitle)
        {
            cbAgree.Checked = false;
            btnSimpleMode.Visible = false;
            lbStartupMode.Visible = false;
            btnNormalMode.Visible = false;

            this.Text = windowTitle;

            tbDisclaimerText.Text = disclaimer;
            btnSimpleMode.Text = "Continue";

            selection = IntroAction.EXIT;
            this.ShowDialog();
        }
    }

    public enum IntroAction { EXIT, SIMPLEMODE, NORMALMODE }
}
