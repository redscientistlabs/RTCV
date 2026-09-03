namespace RTCV.UI
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;
    using System.Collections.Generic;
    using RTCV.CorruptCore;
    using RTCV.Common;
    using RTCV.UI.Modular;


#pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class SettingsForm : ComponentForm, IBlockable
    {
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);
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
                PopoutAllowed = false
            };

            lbForm.AnchorToPanel(pnListBoxForm);
            lbForm.Size = pnListBoxForm.Size;
        }

        public void SwitchToComponentForm(ComponentForm form) => lbForm.SetFocusedForm(form);


        private void OnFactoryCleanSelect(object sender, EventArgs e)
        {
            var result = MessageBox.Show("This will close and reset RTC and the currently open emulator to factory default settings. Are you sure you want to continue?", "Factory Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
                VanguardImplementation.FactoryReset();
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
