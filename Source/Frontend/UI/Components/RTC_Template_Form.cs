namespace RTCV.UI
{
    using System.Windows.Forms;
    using RTCV.Common;
    using static RTCV.UI.UI_Extensions;

    public partial class RTC_Template_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_Template_Form()
        {
            InitializeComponent();

            this.undockedSizable = false;
        }
    }
}
