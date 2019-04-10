using System.Windows.Forms;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

//using RTC.Shortcuts;

namespace RTCV.UI
{
	public partial class RTC_HotkeyConfig_Form : Form, IAutoColorize
	{
		public RTC_HotkeyConfig_Form()
		{
			InitializeComponent();
		}
	}
}
