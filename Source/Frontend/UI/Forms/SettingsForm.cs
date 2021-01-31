namespace RTCV.UI
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using RTCV.UI.Modular;
    using System.Collections.Generic;

#pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class SettingsForm : ComponentForm, IBlockable
    {
        public ListBoxForm lbForm { get; private set; }

        public SettingsForm()
        {
            InitializeComponent();

            var forms = new List<ComponentForm>(new ComponentForm[] {
                S.GET<SettingsGeneralForm>(),
                S.GET<MyListsForm>(),
                S.GET<MyVMDsForm>(),
                S.GET<MyPluginsForm>(),
                S.GET<SettingsCorruptForm>(),
                S.GET<SettingsHotkeyConfigForm>(),
                S.GET<SettingsNetCoreForm>(),
                S.GET<SettingsAboutForm>(),
            });

            if (Debugger.IsAttached)
                forms.Add(S.GET<SettingsTestForm>());

            lbForm = new ListBoxForm(forms.ToArray())
            {
                popoutAllowed = false
            };

            lbForm.AnchorToPanel(pnListBoxForm);
            lbForm.Size = pnListBoxForm.Size;
        }

        public void SwitchToComponentForm(ComponentForm form) => lbForm.SetFocusedForm(form);


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
            S.GET<NetCore.DebugInfoForm>().ShowDialog();
        }

        private void ShowTestForm(object sender, EventArgs e)
        {
            var testform = new TestForm();
            testform.Show();
        }
    }
}
