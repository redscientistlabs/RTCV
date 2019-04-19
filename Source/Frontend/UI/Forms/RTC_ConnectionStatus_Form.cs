using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_ConnectionStatus_Form : ComponentForm, IAutoColorize
	{
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_ConnectionStatus_Form()
		{
			InitializeComponent();
		}

        private readonly string[] _flavorText = {
            "Imagine if we had actual flavor text",
            "Fun flavor text goes here",
        };

		private void RTC_ConnectionStatus_Form_Load(object sender, EventArgs e)
		{
			int crashSound = 0;

			if (NetCore.Params.IsParamSet("CRASHSOUND"))
				crashSound = Convert.ToInt32(NetCore.Params.ReadParam("CRASHSOUND"));

			S.GET<RTC_SettingsNetCore_Form>().cbCrashSoundEffect.SelectedIndex = crashSound;
            lbFlavorText.Text = _flavorText[CorruptCore.CorruptCore.RND.Next(0, _flavorText.Length)];
        }

		public void btnStartEmuhawkDetached_Click(object sender, EventArgs e)
		{

			S.GET<UI_CoreForm>().pbAutoKillSwitchTimeout.Value = S.GET<UI_CoreForm>().pbAutoKillSwitchTimeout.Maximum;

			//RTC_NetCoreSettings.PlayCrashSound();

			Process.Start("RESTARTDETACHEDRTC.bat");
		}

		private void btnStopGameProtection_Click(object sender, EventArgs e)
		{
			S.GET<UI_CoreForm>().cbUseGameProtection.Checked = false;
		}

	}
}
