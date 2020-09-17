namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;

    //Todo, rebuild this?
    public static class GameProtection
    {
        private static Timer t;
        internal static int BackupInterval = 5;
        internal static bool isRunning = false;
        internal static bool WasAutoCorruptRunning = false;
        private const int maxStates = 20;

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly LinkedList<StashKey> AllBackupStates = new LinkedList<StashKey>();
        public static bool HasBackedUpStates => AllBackupStates?.Count > 0;

        public static void Start(bool reset = true)
        {
            if (reset)
            {
                ClearAllBackups();
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
                Task.Run(() => ClearAllBackups());
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
                    {
                        Task.Run(() => RemoveBackup(_sk)); //Do this async to prevent hangs from a slow drive
                    }
                }

                AllBackupStates.AddLast(sk);
            }
        }

        public static void PopAndRunBackupState()
        {
            StashKey sk = null;
            lock (AllBackupStates)
            {
                if (AllBackupStates.Count > 0)
                {
                    sk = AllBackupStates.Last.Value;
                    AllBackupStates.RemoveLast();
                }
            }
            sk?.Run();
            //Don't delete it if it's also our "current" state
            if (sk != StockpileManagerUISide.BackupedState)
            {
                Task.Run(() => RemoveBackup(sk)); //Don't wait on the hdd operations
            }
        }

        private static void RemoveBackup(StashKey sk)
        {
            try
            {
                if (File.Exists(sk.StateFilename))
                {
                    File.Delete(sk.StateFilename);
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Unable to remove backup {stashkey} from queue!", sk);
            }
        }

        public static void ClearAllBackups()
        {
            StockpileManagerUISide.BackupedState = null;
            StashKey[] states = new StashKey[0];

            //Grab a copy then clear it out
            lock (AllBackupStates)
            {
                states = AllBackupStates?.ToArray();
                AllBackupStates.Clear();
            }

            //Do this async
            Task.Run(() =>
            {
                foreach (var sk in states)
                {
                    RemoveBackup(sk);
                }
            });
        }

        private static void Tick(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.BackupKeyRequest);
        }
    }
}
