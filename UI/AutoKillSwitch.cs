using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using RTCV.NetCore;
using RTCV.NetCore.StaticTools;
using RTCV.CorruptCore;
using RTCV.UI;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{

	public static class AutoKillSwitch
	{
		public static int MaxMissedPulses = 15;
		private static Timer killswitchSpamPreventTimer;
		public static bool ShouldKillswitchFire = true;

		public static bool Enabled
		{
			get
			{
				if (BoopMonitoringTimer == null)
					return false;
				return BoopMonitoringTimer.Enabled;
			}
			set
			{
				if (value)
					Start();
				else
					Stop();

			}
		}

		private static volatile int pulseCount = MaxMissedPulses;
		private static System.Timers.Timer BoopMonitoringTimer = null;

		public static SoundPlayer[] LoadedSounds = null;

		public static void PlayCrashSound(bool forcePlay = false)
		{
			if (LoadedSounds != null && (forcePlay || S.GET<RTC_ConnectionStatus_Form>()
				.btnStartEmuhawkDetached.Text == "Restart BizHawk"))
				LoadedSounds[CorruptCore.CorruptCore.RND.Next(LoadedSounds.Length)]
					.Play();
		}

		public static void Pulse()
		{
			pulseCount = MaxMissedPulses;
		}

		public static void KillEmulator(string str, bool forceBypass = false)
		{
			SyncObjectSingleton.FormExecute((o, ea) =>
			{

				if (!ShouldKillswitchFire || (!S.GET<RTC_Core_Form>()
					.cbUseAutoKillSwitch.Checked && !forceBypass))
					return;

				//Stop the old timer and eat any exceptions
				try
				{
					BoopMonitoringTimer?.Stop();
					BoopMonitoringTimer?.Dispose();
				}
				catch { }

				killswitchSpamPreventTimer = new Timer();
				killswitchSpamPreventTimer.Interval = Debugger.IsAttached ? 300000 : 1000;
				killswitchSpamPreventTimer.Tick += KillswitchSpamPreventTimer_Tick;
				killswitchSpamPreventTimer.Start();

				ShouldKillswitchFire = false;

				PlayCrashSound(true);
				switch (str)
				{
					case "KILL":
						Process.Start("KILLDETACHEDRTC.bat");
						break;
					case "KILL + RESTART":
						Process.Start("RESTARTDETACHEDRTC.bat");
						break;
				}
			});
		}
		private static void KillswitchSpamPreventTimer_Tick(object sender, EventArgs e)
		{
			ShouldKillswitchFire = true;
			killswitchSpamPreventTimer.Stop();
		}

		private static void Start()
		{
			pulseCount = MaxMissedPulses;

			//Stop the old timer and eat any exceptions
			try
			{
				BoopMonitoringTimer?.Stop();
				BoopMonitoringTimer?.Dispose();
			}
			catch { }

			BoopMonitoringTimer = new System.Timers.Timer();
			BoopMonitoringTimer.Interval = 500;
			BoopMonitoringTimer.Elapsed += BoopMonitoringTimer_Tick;
			BoopMonitoringTimer.Start();
		}

		private static void Stop()
		{
			BoopMonitoringTimer?.Stop();
		}

		private static void BoopMonitoringTimer_Tick(object sender, EventArgs e)
		{
			if (!Enabled || UI_VanguardImplementation.connector.netConn.status != NetCore.NetworkStatus.CONNECTED)
				return;

			pulseCount--;

			if(pulseCount < MaxMissedPulses - 1)
				SyncObjectSingleton.FormExecute((o, ea) =>
				{
					S.GET<RTC_Core_Form>().pbAutoKillSwitchTimeout.PerformStep();
				});
			else
				SyncObjectSingleton.FormExecute((o, ea) =>
				{
					S.GET<RTC_Core_Form>().pbAutoKillSwitchTimeout.Value = 0;
				});

			if (pulseCount == 0)
			{
				KillEmulator("KILL + RESTART");
			}
		}
	}
}
