using System.Windows.Forms;
using RTCV.NetCore.StaticTools;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{
    public partial class RTC_VmdNoTool_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_VmdNoTool_Form()
        {
            InitializeComponent();

            popoutAllowed = false;
        }

    }
}
