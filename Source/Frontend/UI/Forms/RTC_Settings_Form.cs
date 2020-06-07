namespace RTCV.UI
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using static RTCV.UI.UI_Extensions;

    public partial class RTC_Settings_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_ListBox_Form lbForm;

        public RTC_Settings_Form()
        {
            InitializeComponent();

            lbForm = new RTC_ListBox_Form(new ComponentForm[] {
                S.GET<RTC_SettingsGeneral_Form>(),
                S.GET<RTC_SettingsCorrupt_Form>(),
                S.GET<RTC_SettingsHotkeyConfig_Form>(),
                S.GET<RTC_SettingsNetCore_Form>(),
                S.GET<RTC_SettingsAbout_Form>(),
            })
            {
                popoutAllowed = false
            };

            lbForm.AnchorToPanel(pnListBoxForm);
        }

        private void btnRtcFactoryClean_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "FactoryClean.bat";
            p.StartInfo.WorkingDirectory = CorruptCore.RtcCore.EmuDir;
            p.Start();
        }

        private void RTC_Settings_Form_Load(object sender, EventArgs e)
        {
            if (Debugger.IsAttached)
            {
                btnTestForm.Show();
            }
        }

        private void btnCloseSettings_Click(object sender, EventArgs e)
        {
            //If we're not connected, go to connectionstatus
            /*
            if (UI_VanguardImplementation.connector.netConn.status != NetCore.NetworkStatus.CONNECTED)
                S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>());
            else
                S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_Core_Form>().previousForm, false);
             */

            MessageBox.Show("is this even needed anymore?");
        }

        private void btnToggleConsole_Click(object sender, EventArgs e)
        {
            LogConsole.ToggleConsole();
        }

        private void btnDebugInfo_Click(object sender, EventArgs e)
        {
            S.GET<RTCV.NetCore.DebugInfo_Form>().ShowDialog();
        }

        private void BtnTestForm_Click(object sender, EventArgs e)
        {
            var testform = new RTC_Test_Form();
            testform.Show();
        }
    }
}
