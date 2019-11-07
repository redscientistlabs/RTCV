using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_OpenTools_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		public RTC_OpenTools_Form()
		{
			InitializeComponent();
		}

		private void btnOpenHexEditor_Click(object sender, EventArgs e)
		{
			bool UseRealtime = (bool?)AllSpec.VanguardSpec[VSPEC.SUPPORTS_REALTIME] ?? false;
            if(UseRealtime)
				LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_OPENHEXEDITOR);
			else
			{
				MessageBox.Show("Hex editor only works with real-time systems");
			}
        }
	}
}
