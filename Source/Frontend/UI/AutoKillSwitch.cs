namespace RTCV.UI
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Media;
    using System.Windows.Forms;
    using RTCV.Common;
    using RTCV.NetCore;

    public static class AutoKillSwitch
    {
        public static int MaxMissedPulses = 25;
        private static Timer killswitchSpamPreventTimer;
        public static bool ShouldKillswitchFire = true;
        private static volatile object lockObject = new object();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static bool Enabled
        {
            get
            {
                if (BoopMonitoringTimer == null)
                {
                    return false;
                }

                return BoopMonitoringTimer.Enabled;
            }
            set
            {
                if (value)
                {
                    Start();
                }
                else
                {
                    Stop();
                }
            }
        }

        private static volatile int pulseCount = MaxMissedPulses;
        private static System.Windows.Forms.Timer BoopMonitoringTimer = null;

        public static SoundPlayer[] LoadedSounds = null;

        public static void PlayCrashSound(bool forcePlay = false)
        {
            if (LoadedSounds?.Length != 0)
            {
                LoadedSounds[CorruptCore.RtcCore.RND.Next(LoadedSounds.Length)].Play();
            }
        }

        public static void Pulse()
        {
            pulseCount = MaxMissedPulses;
        }

        private static string _oldEmuDir = "";
        private static string oldEmuDir
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                _oldEmuDir = value;
            }
            get => _oldEmuDir;
        }

        public static void KillEmulator(bool forceBypass = false)
        {
            logger.Trace("Entered KillEmulator {ShouldKillswitchFire} {UICore.FirstConnect} {!forceBypass} {!S.GET<UI_CoreForm>().cbUseAutoKillSwitch.Checked} {!forceBypass}", ShouldKillswitchFire, UICore.FirstConnect, !forceBypass, !S.GET<UI_CoreForm>().cbUseAutoKillSwitch.Checked, !forceBypass);
            if (!ShouldKillswitchFire || (UICore.FirstConnect && !forceBypass) || (!S.GET<UI_CoreForm>().cbUseAutoKillSwitch.Checked && !forceBypass))
            {
                logger.Trace("Exited KillEmulator {ShouldKillswitchFire} {UICore.FirstConnect} {!forceBypass} {!S.GET<UI_CoreForm>().cbUseAutoKillSwitch.Checked} {!forceBypass}", ShouldKillswitchFire, UICore.FirstConnect, !forceBypass, !S.GET<UI_CoreForm>().cbUseAutoKillSwitch.Checked, !forceBypass);
                return;
            }
            logger.Trace("Thread id {0} requesting KillEmulator lockObject...", System.Threading.Thread.CurrentThread.ManagedThreadId);
            if (System.Threading.Monitor.TryEnter(lockObject)) // No re-entrancy on the killswitch
            {
                logger.Trace("Thread id {0} got KillEmulator lockObject...", System.Threading.Thread.CurrentThread.ManagedThreadId);
                try
                {
                    ShouldKillswitchFire = false;

                    //Nuke netcore
                    logger.Trace("Nuking Netcore");
                    UI_VanguardImplementation.RestartServer();

                    SyncObjectSingleton.FormExecute(() =>
                    {
                        //Stop the old timer and eat any exceptions
                        try
                        {
                            BoopMonitoringTimer?.Stop();
                            BoopMonitoringTimer?.Dispose();
                        }
                        catch
                        {
                        }

                        killswitchSpamPreventTimer = new Timer
                        {
                            Interval = 5000
                        };
                        killswitchSpamPreventTimer.Tick += KillswitchSpamPreventTimer_Tick;
                        killswitchSpamPreventTimer.Start();

                        PlayCrashSound(true);

                        if (CorruptCore.RtcCore.EmuDir == null)
                        {
                            MessageBox.Show("Couldn't determine what emulator to start! Please start it manually.");
                            return;
                        }
                    });
                    logger.Trace("Starting the new process");
                    var info = new ProcessStartInfo();
                    oldEmuDir = CorruptCore.RtcCore.EmuDir;
                    info.WorkingDirectory = oldEmuDir;
                    info.FileName = Path.Combine(oldEmuDir, "RESTARTDETACHEDRTC.bat");
                    if (!File.Exists(info.FileName))
                    {
                        MessageBox.Show($"Couldn't find {info.FileName}! Killswitch will not work.");
                        return;
                    }

                    Process.Start(info);
                }
                finally
                {
                    logger.Trace("Thread id {0} released KillEmulator lockObject...", System.Threading.Thread.CurrentThread.ManagedThreadId);
                    System.Threading.Monitor.Exit(lockObject);
                }
            }
            else
            {
                logger.Trace("Thread id {0} did not get KillEmulator lockObject...", System.Threading.Thread.CurrentThread.ManagedThreadId);
            }
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

            BoopMonitoringTimer = new System.Windows.Forms.Timer
            {
                Interval = 500
            };
            BoopMonitoringTimer.Tick += BoopMonitoringTimer_Tick;
            BoopMonitoringTimer.Start();
        }

        private static void Stop()
        {
            BoopMonitoringTimer?.Stop();
        }

        private static void BoopMonitoringTimer_Tick(object sender, EventArgs e)
        {
            if (!Enabled || (UI_VanguardImplementation.connector?.netConn?.status != NetCore.NetworkStatus.CONNECTED))
            {
                return;
            }

            pulseCount--;

            if (pulseCount < MaxMissedPulses - 1)
            {
                S.GET<UI_CoreForm>().pbAutoKillSwitchTimeout.PerformStep();
            }
            else if (S.GET<UI_CoreForm>().pbAutoKillSwitchTimeout.Value != 0)
            {
                S.GET<UI_CoreForm>().pbAutoKillSwitchTimeout.Value = 0;
            }

            if (pulseCount == 0)
            {
                KillEmulator();
            }
        }
    }
}
