using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_Settings_Form : Form, IAutoColorize
	{

		public RTC_ListBox_Form lbForm;

		public RTC_Settings_Form()
		{
			InitializeComponent();

			lbForm = new RTC_ListBox_Form(new ComponentForm[]{
				S.GET<RTC_SettingsGeneral_Form>(),
				S.GET<RTC_SettingsCorrupt_Form>(),
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
			p.StartInfo.WorkingDirectory = CorruptCore.CorruptCore.bizhawkDir;
			p.Start();
		}

		private void RTC_Settings_Form_Load(object sender, EventArgs e)
		{
			if (Debugger.IsAttached)
				btnTestForm.Show();

		}

		private void btnCloseSettings_Click(object sender, EventArgs e)
		{
			//If we're not connected, go to connectionstatus
			if (UI_VanguardImplementation.connector.netConn.status != NetCore.NetworkStatus.CONNECTED)
				S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>());
			else
				S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_Core_Form>().previousForm, false);
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
