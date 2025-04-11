using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using RTCV.Common;
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.UI.Components.Controls;
using Timer = System.Timers.Timer;

namespace RTCV.UI
{
    public static class AutoSave
    {
        private static readonly Timer AutoSaveTimer;
        private static bool _inProgress;
        private static decimal _maxSizeGiB = 2.5m;

        private static readonly object InProgressLock = new object();
        public static readonly object SavingLock = new object();
        public static readonly object UnsavedLock = new object();

        private static bool _saveStatesUnsaved;
        public static bool SaveStatesUnsaved
        {
            get
            {
                lock (UnsavedLock)
                {
                    return _saveStatesUnsaved;
                }
            }
            set
            {
                lock (UnsavedLock)
                {
                    _saveStatesUnsaved = value;
                }
            }
        }

        private static bool _stockpileUnsaved;
        public static bool StockpileUnsaved
        {
            get
            {
                lock (UnsavedLock)
                {
                    return _stockpileUnsaved;
                }
            }
            set
            {
                lock (UnsavedLock)
                {
                    _stockpileUnsaved = value;
                }
            }
        }

        static AutoSave()
        {
            AutoSaveTimer = new Timer { AutoReset = true, Interval = 5 * 60 * 1000 };
            AutoSaveTimer.Elapsed += PerformAutoSave;
        }

        public static void SetInterval(int seconds)
        {
            AutoSaveTimer.Interval = seconds * 1000;
        }
        public static void SetMaxSize(decimal gibibytes)
        {
            _maxSizeGiB = gibibytes;
            RemoveExcessBackups();
        }

        public static void Start()
        {
            AutoSaveTimer.Start();
        }
        public static void Stop()
        {
            AutoSaveTimer.Stop();
        }

        private static async void PerformAutoSave(object sender, ElapsedEventArgs e)
        {
            Stop();
            lock (InProgressLock)
            {
                if (_inProgress || S.ISNULL<SavestateManagerForm>() || S.ISNULL<StockpileManagerForm>())
                {
                    Start();
                    return;
                }
                _inProgress = true;
            }
    
            if (SaveStatesUnsaved)
            {
                await SaveStates();
            }
                
            // if stockpile is unsaved and not using a core that must be restarted to save the stockpile
            if (StockpileUnsaved && !(AllSpec.VanguardSpec[VSPEC.CORE_DISKBASED] as bool? ?? false))
            {
                await SaveStockpile();
            }
            
            if (SaveStatesUnsaved || (StockpileUnsaved && !(AllSpec.VanguardSpec[VSPEC.CORE_DISKBASED] as bool? ?? false)))
            {
                RemoveExcessBackups();
            }
            SaveStatesUnsaved = false;
            StockpileUnsaved = false;

            lock (InProgressLock)
            {
                _inProgress = false;
            }
            Start();
        }

        private static async Task SaveStates()
        {
            string savePath = Path.Combine(RtcCore.AutoSaveDir, $"autosave_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.ssk");

            var tcs = new TaskCompletionSource<bool>();
                
            var statesForm = S.GET<SavestateManagerForm>();
            
            SyncObjectSingleton.FormBeginExecute(() =>
            {
                Toast toast = new Toast("Auto-saving save states...", "");
                statesForm.ParentCanvas?.ShowToast(toast);

                Task.Run(() =>
                {
                    lock (SavingLock)
                    {
                        statesForm.SaveSSK(savePath);
                    }
                    tcs.SetResult(true);
                }).ContinueWith(_ =>
                {
                    // Close the toast on the UI thread
                    toast.Close();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            });

            await tcs.Task;
        }

        private static async Task SaveStockpile()
        {
            string savePath = Path.Combine(RtcCore.AutoSaveDir, $"autosave_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.sks");

            var tcs = new TaskCompletionSource<bool>();
                
            var stockpileForm = S.GET<StockpileManagerForm>();
            
            SyncObjectSingleton.FormBeginExecute(() =>
            {
                Stockpile sks = new Stockpile(stockpileForm.dgvStockpile);

                if (sks.StashKeys.Count <= 0)
                {
                    return;
                }

                Toast toast = new Toast("Auto-saving stockpile...", "");
                stockpileForm.ParentCanvas?.ShowToast(toast);
                
                Task.Run(() =>
                {
                    lock (SavingLock)
                    {
                        Stockpile.Save(sks, savePath, false, Params.IsParamSet("COMPRESS_STOCKPILE"));
                    }
                    tcs.SetResult(true);
                }).ContinueWith(_ =>
                {
                    // Close the toast on the UI thread
                    toast.Close();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            });
            
            await tcs.Task;
        }

        private static void RemoveExcessBackups()
        {
            string[] allSSKs = Directory.GetFiles(RtcCore.AutoSaveDir, "*.ssk");
            while (allSSKs.Sum(s => new FileInfo(s).Length) > _maxSizeGiB * 1024 * 1024 * 1024)
            {
                string oldestSSK = allSSKs.OrderBy(f => new FileInfo(f).CreationTime).First();
                File.Delete(oldestSSK);
                allSSKs = Directory.GetFiles(RtcCore.AutoSaveDir, "*.ssk");
            }

            string[] allStockpiles = Directory.GetFiles(RtcCore.AutoSaveDir, "*.sks");
            while (allStockpiles.Sum(s => new FileInfo(s).Length) > _maxSizeGiB * 1024 * 1024 * 1024)
            {
                string oldestStockpile = allStockpiles.OrderBy(f => new FileInfo(f).CreationTime).First();
                File.Delete(oldestStockpile);
                allStockpiles = Directory.GetFiles(RtcCore.AutoSaveDir, "*.sks");
            }
        }
    }
}