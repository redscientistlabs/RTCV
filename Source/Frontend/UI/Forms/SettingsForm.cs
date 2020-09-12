namespace RTCV.UI
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    #pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class SettingsForm : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public ListBoxForm lbForm { get; private set; }

        public SettingsForm()
        {
            InitializeComponent();

            lbForm = new ListBoxForm(new ComponentForm[] {
                S.GET<SettingsGeneralForm>(),
                S.GET<SettingsCorruptForm>(),
                S.GET<RTC_SettingsHotkeyConfig_Form>(),
                S.GET<RTC_SettingsNetCore_Form>(),
                S.GET<RTC_SettingsAbout_Form>(),
            })
            {
                popoutAllowed = false
            };

            lbForm.AnchorToPanel(pnListBoxForm);
        }

        private void FactoryClean(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "FactoryClean.bat";
            p.StartInfo.WorkingDirectory = RtcCore.EmuDir;
            p.Start();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            if (Debugger.IsAttached)
            {
                btnTestForm.Show();
            }
        }

        private void ToggleConsole(object sender, EventArgs e)
        {
            LogConsole.ToggleConsole();
        }

        private void ShowDebugInfo(object sender, EventArgs e)
        {
            S.GET<NetCore.DebugInfo_Form>().ShowDialog();
        }

        private void ShowTestForm(object sender, EventArgs e)
        {
            var testform = new TestForm();
            testform.Show();
        }
    }
}
