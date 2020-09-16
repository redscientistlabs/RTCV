namespace RTCV.UI
{
    using System;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Components.Controls;
    using RTCV.UI.Modular;

    public partial class OpenToolsForm : ComponentForm, IAutoColorize, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public OpenToolsForm()
        {
            InitializeComponent();

            RegisterTool("Hex Editor", "Open Hex Editor", () => TryOpenHexEditor(this, EventArgs.Empty));
        }

        private void TryOpenHexEditor(object sender, EventArgs e)
        {
            bool UseRealtime = (bool?)AllSpec.VanguardSpec[VSPEC.SUPPORTS_REALTIME] ?? false;
            if (UseRealtime)
            {
                LocalNetCoreRouter.Route(NetCore.Commands.Basic.CorruptCore, NetCore.Commands.Remote.OpenHexEditor);
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
