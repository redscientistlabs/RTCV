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
        private static LinkedList<StashKey> AllBackupStates;
        public static bool HasBackedUpStates => AllBackupStates?.Count > 0;
        public static void Start(bool reset = true)
		{
            if (reset)
            {
                lock (AllBackupStates)
                {
                    AllBackupStates = new LinkedList<StashKey>();
                }
            }

            if (t == null)
			{
				t = new Timer();
				t.Tick += new EventHandler(Tick);
			}

			t.Interval = Convert.ToInt32(BackupInterval) * 1000;
			t.Start();

			isRunning = true;

		}

		public static void Stop(bool reset = true)
        {
            if (reset)
            {
                //If the states are too large this could take forever so do it async
                Task.Run(ClearAllBackups);
                lock (AllBackupStates)
                {
                    AllBackupStates = null;
                }
            }

            t?.Stop();

			isRunning = false;
		}

		public static void Reset(bool reinit)
		{
			Stop(reinit);
			Start(reinit);
		}

        public static void AddBackupState(StashKey sk)
        {
            lock (AllBackupStates)
            {
                if (AllBackupStates.Count > maxStates)
                {
                    var _sk = AllBackupStates.First.Value;
                    AllBackupStates.RemoveFirst();
                    if (_sk != null)
                        Task.Run(() => RemoveBackup(_sk)); //Do this async to prevent hangs from a slow drive
                }

                AllBackupStates.AddLast(sk);
            }
        }

        public static StashKey PopBackupState()
        {
            lock (AllBackupStates)
            {
                if (AllBackupStates.Count > 0)
                {
                    var sk = AllBackupStates.Last.Value;
                    AllBackupStates.RemoveLast();
                    return sk;
                }
            }
            return null;
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
            lock (AllBackupStates)
            {
                foreach (var sk in AllBackupStates)
                {
                    RemoveBackup(sk);
                }
            }
        }

        private static void Tick(object sender, EventArgs e)
		{
			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_BACKUPKEY_REQUEST);
		}
	}
}
