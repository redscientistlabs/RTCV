using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;

namespace RTCV.UI
{
	//Todo, rebuild this?
	public static class GameProtection
	{
		static Timer t;
		public static int BackupInterval = 5;
		public static bool isRunning = false;
        public static bool WasAutoCorruptRunning = false;
        private const int maxStates = 20;
        private static ConcurrentQueue<StashKey> AllBackupStates;

        public static void Start()
		{
            AllBackupStates = new ConcurrentQueue<StashKey>();

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
            AllBackupStates = null;
            //If the states are too large this could take forever so do it async
            Task.Run(ClearAllBackups);

            t?.Stop();

			isRunning = false;
		}

		public static void Reset()
		{
			Stop();
			Start();
		}

        public static void AddBackupState(StashKey sk)
        {
            if (AllBackupStates.Count > maxStates)
            {
                AllBackupStates.TryDequeue(out sk);
                if(sk != null)
                    Task.Run(() => RemoveBackup(sk)); //Do this async to prevent hangs from a slow drive
            }
            AllBackupStates.Enqueue(sk);
        }

        private static void RemoveBackup(StashKey sk)
        {
            try
            {
                if (File.Exists(sk.StateFilename))
                    File.Delete(sk.StateFilename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to remove backup " + sk + " from queue!");
            }
        }
        public static void ClearAllBackups()
        {
            StockpileManager_UISide.BackupedState = null;
            foreach (var sk in AllBackupStates.ToArray())
            {
                RemoveBackup(sk);
            }
        }

        private static void Tick(object sender, EventArgs e)
		{
			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_BACKUPKEY_REQUEST);
		}
	}
}
