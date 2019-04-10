using System;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;

namespace RTCV.UI
{
	//Todo, rebuild this?
	public static class GameProtection
	{
		static Timer t;
		public static int BackupInterval = 30;
		public static bool isRunning = false;

		public static void Start()
		{
			if (t == null)
			{
				t = new Timer();
				t.Tick += new EventHandler(Tick);
			}

			t.Interval = Convert.ToInt32(BackupInterval) * 1000;
			t.Start();

			isRunning = true;

		}

		public static void Stop()
		{
			t?.Stop();

			isRunning = false;
		}

		public static void Reset()
		{
			Stop();
			Start();
		}

		private static void Tick(object sender, EventArgs e)
		{
			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_BACKUPKEY_REQUEST);
		}
	}
}
