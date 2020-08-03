namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Components.Controls;
    using RTCV.UI.Modular;

    public partial class RTC_OpenTools_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_OpenTools_Form()
        {
            InitializeComponent();

            RegisterTool("Hex Editor", "Open Hex Editor", () => btnOpenHexEditor_Click(this, EventArgs.Empty));
        }

        private void btnOpenHexEditor_Click(object sender, EventArgs e)
        {
            bool UseRealtime = (bool?)AllSpec.VanguardSpec[VSPEC.SUPPORTS_REALTIME] ?? false;
            if (UseRealtime)
            {
                LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_OPENHEXEDITOR);
            }
            else
            {
                MessageBox.Show("Hex editor only works with real-time systems");
            }
        }

        public void RegisterTool(string name, string buttonText, Action clickAction)
        {
            Control c = new OpenToolButton(name, buttonText, clickAction);
            flpTools.Controls.Add(c);
        }
    }
}
