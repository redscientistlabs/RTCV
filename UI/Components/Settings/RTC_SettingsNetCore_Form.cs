using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Media;
using System.Diagnostics;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_SettingsNetCore_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		public RTC_SettingsNetCore_Form()
		{
			InitializeComponent();
		}

		private void cbCrashSoundEffect_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cbCrashSoundEffect.SelectedIndex)
			{
				case 0:
					var PlatesHdFiles = Directory.GetFiles(CorruptCore.CorruptCore.assetsDir + Path.DirectorySeparatorChar + "PLATESHD" + Path.DirectorySeparatorChar);
					AutoKillSwitch.LoadedSounds = PlatesHdFiles.Select(it => new SoundPlayer(it)).ToArray();
					break;
				case 1:
					AutoKillSwitch.LoadedSounds = new SoundPlayer[] { new SoundPlayer(CorruptCore.CorruptCore.assetsDir + Path.DirectorySeparatorChar + "crash.wav") };
					break;

				case 2:
					AutoKillSwitch.LoadedSounds = null;
					break;
				case 3:
					var CrashSoundsFiles = Directory.GetFiles(CorruptCore.CorruptCore.assetsDir + Path.DirectorySeparatorChar + "CRASHSOUNDS");
					AutoKillSwitch.LoadedSounds = CrashSoundsFiles.Select(it => new SoundPlayer(it)).ToArray();
					break;
			}

			RTCV.NetCore.Params.SetParam("CRASHSOUND", cbCrashSoundEffect.SelectedIndex.ToString());
		}
		
		private void nmGameProtectionDelay_ValueChanged(object sender, KeyPressEventArgs e) => UpdateGameProtectionDelay();

		private void nmGameProtectionDelay_ValueChanged(object sender, KeyEventArgs e) => UpdateGameProtectionDelay();

		private void nmGameProtectionDelay_ValueChanged(object sender, EventArgs e) => UpdateGameProtectionDelay();

		public void UpdateGameProtectionDelay()
		{
			GameProtection.BackupInterval = Convert.ToInt32(S.GET<RTC_SettingsNetCore_Form>().nmGameProtectionDelay.Value);
			if (GameProtection.isRunning)
				GameProtection.Reset();
		}

		private void RTC_SettingsNetCore_Form_Load(object sender, EventArgs e)
		{

		}
	}
}
