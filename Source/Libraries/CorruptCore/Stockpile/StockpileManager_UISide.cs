namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.NetCore;

    public static class StockpileManager_UISide
    {
        //Object references
        private static Stockpile CurrentStockpile { get; set; }

        private static StashKey _lastStashKey = null;
        public static StashKey LastStashkey => _lastStashKey;
        private static StashKey _currentStashKey = null;
        public static StashKey CurrentStashkey
        {
            get
            {
                return _currentStashKey;
            }
            set
            {
                _lastStashKey = CurrentStashkey;
                _currentStashKey = value;
            }
        }
        public static StashKey CurrentSavestateStashKey { get; set; }
        public static volatile StashKey BackupedState;
        public static bool StashAfterOperation = true;
        public static volatile List<StashKey> StashHistory = new List<StashKey>();

        private static void PreApplyStashkey(bool _clearUnitsBeforeApply = true)
        {
            if (_clearUnitsBeforeApply)
                LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);


            bool UseSavestates = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES];
            LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_PRECORRUPTACTION, null, true);
        }

        private static void PostApplyStashkey()
        {
            bool UseSavestates = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES];
            bool UseRealtime = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_REALTIME];

            if (Render.RenderAtLoad && UseRealtime)
            {
                Render.StartRender();
            }

            LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_POSTCORRUPTACTION);
        }

        public static bool ApplyStashkey(StashKey sk, bool _loadBeforeOperation = true, bool _clearUnitsBeforeApply = true)
        {
            PreApplyStashkey(_clearUnitsBeforeApply);

            bool isCorruptionApplied = sk?.BlastLayer?.Layer?.Count > 0;

            if (_loadBeforeOperation)
            {
                if (!LoadState(sk))
                {
                    return isCorruptionApplied;
                }
            }
            else
            {
                bool mergeWithCurrent = !_clearUnitsBeforeApply;

                //APPLYBLASTLAYER
                //Param 0 is BlastLayer
                //Param 1 is storeUncorruptBackup
                //Param 2 is MergeWithCurrent (for fixing blast toggle with inject)
                LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.APPLYBLASTLAYER, new object[] { sk?.BlastLayer, true, mergeWithCurrent }, true);
            }

            PostApplyStashkey();
            return isCorruptionApplied;
        }

        public static void Import(BlastLayer _importedBlastLayer)
        {
            string saveStateWord = "Savestate";

            object renameSaveStateWord = AllSpec.VanguardSpec[VSPEC.RENAME_SAVESTATE];
            if (renameSaveStateWord != null && renameSaveStateWord is string s)
            {
                saveStateWord = s;
            }

            StashKey psk = CurrentSavestateStashKey;

            bool UseSavestates = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES];
            if (!UseSavestates)
            {
                psk = SaveState();
            }

            if (psk == null && UseSavestates)
            {
                MessageBox.Show($"The Glitch Harvester could not perform the IMPORT action\n\nEither no {saveStateWord} Box was selected in the {saveStateWord} Manager\nor the {saveStateWord} Box itself is empty.");
                return;
            }


            //We make it without the blastlayer so we can send it across and use the cached version without needing a prototype
            CurrentStashkey = new StashKey(RtcCore.GetRandomKey(), psk.ParentKey, null)
            {
                RomFilename = psk.RomFilename,
                SystemName = psk.SystemName,
                SystemCore = psk.SystemCore,
                GameName = psk.GameName,
                SyncSettings = psk.SyncSettings,
                StateLocation = psk.StateLocation
            };


            BlastLayer bl = _importedBlastLayer;

            CurrentStashkey.BlastLayer = bl;
            StashHistory.Add(CurrentStashkey);
        }

        public static bool Corrupt(bool _loadBeforeOperation = true)
        {
            string saveStateWord = "Savestate";

            object renameSaveStateWord = AllSpec.VanguardSpec[VSPEC.RENAME_SAVESTATE];
            if (renameSaveStateWord != null && renameSaveStateWord is string s)
            {
                saveStateWord = s;
            }

            PreApplyStashkey();
            StashKey psk = CurrentSavestateStashKey;

            bool UseSavestates = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES];
            if (!UseSavestates)
            {
                psk = SaveState();
            }

            if (psk == null && UseSavestates)
            {
                MessageBox.Show($"The Glitch Harvester could not perform the CORRUPT action\n\nEither no {saveStateWord} Box was selected in the {saveStateWord} Manager\nor the {saveStateWord} Box itself is empty.");
                return false;
            }

            string currentGame = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.GAMENAME];
            string currentCore = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.SYSTEMCORE];
            if (UseSavestates && (currentGame == null || psk.GameName != currentGame || psk.SystemCore != currentCore))
            {
                LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_LOADROM, psk.RomFilename, true);
            }

            //We make it without the blastlayer so we can send it across and use the cached version without needing a prototype
            CurrentStashkey = new StashKey(RtcCore.GetRandomKey(), psk.ParentKey, null)
            {
                RomFilename = psk.RomFilename,
                SystemName = psk.SystemName,
                SystemCore = psk.SystemCore,
                GameName = psk.GameName,
                SyncSettings = psk.SyncSettings,
                StateLocation = psk.StateLocation
            };


            BlastLayer bl = LocalNetCoreRouter.QueryRoute<BlastLayer>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.GENERATEBLASTLAYER,
                    new object[]
                    {
                    CurrentStashkey,
                    _loadBeforeOperation,
                    true,
                    true
                    }, true);
            bool isCorruptionApplied = bl?.Layer?.Count > 0;

            CurrentStashkey.BlastLayer = bl;

            if (StashAfterOperation && bl != null)
            {
                StashHistory.Add(CurrentStashkey);
            }

            PostApplyStashkey();
            return isCorruptionApplied;
        }

        public static void RemoveFirstStashItem()
        {
            StashHistory.RemoveAt(0);
        }

        public static bool InjectFromStashkey(StashKey sk, bool _loadBeforeOperation = true)
        {
            string saveStateWord = "Savestate";

            object renameSaveStateWord = AllSpec.VanguardSpec[VSPEC.RENAME_SAVESTATE];
            if (renameSaveStateWord != null && renameSaveStateWord is string s)
            {
                saveStateWord = s;
            }

            PreApplyStashkey();

            StashKey psk = CurrentSavestateStashKey;

            if (psk == null)
            {
                MessageBox.Show($"The Glitch Harvester could not perform the INJECT action\n\nEither no {saveStateWord} Box was selected in the {saveStateWord} Manager\nor the {saveStateWord} Box itself is empty.");
                return false;
            }

            if (psk.SystemCore != sk.SystemCore && !RtcCore.AllowCrossCoreCorruption)
            {
                MessageBox.Show("Merge attempt failed: Core mismatch\n\n" + $"{psk.GameName} -> {psk.SystemName} -> {psk.SystemCore}\n{sk.GameName} -> {sk.SystemName} -> {sk.SystemCore}");
                return false;
            }

            CurrentStashkey = new StashKey(RtcCore.GetRandomKey(), psk.ParentKey, sk.BlastLayer)
            {
                RomFilename = psk.RomFilename,
                SystemName = psk.SystemName,
                SystemCore = psk.SystemCore,
                GameName = psk.GameName,
                SyncSettings = psk.SyncSettings,
                StateLocation = psk.StateLocation
            };

            if (_loadBeforeOperation)
            {
                if (!LoadState(CurrentStashkey))
                {
                    return false;
                }
            }
            else
            {
                LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.APPLYBLASTLAYER, new object[] { CurrentStashkey.BlastLayer, true }, true);
            }

            bool isCorruptionApplied = CurrentStashkey?.BlastLayer?.Layer?.Count > 0;

            if (StashAfterOperation)
            {
                StashHistory.Add(CurrentStashkey);
            }

            PostApplyStashkey();
            return isCorruptionApplied;
        }

        public static bool OriginalFromStashkey(StashKey sk)
        {
            PreApplyStashkey();

            if (sk == null)
            {
                MessageBox.Show("No StashKey could be loaded");
                return false;
            }

            bool isCorruptionApplied = false;

            if (!LoadState(sk, true, false))
            {
                return isCorruptionApplied;
            }

            PostApplyStashkey();
            return isCorruptionApplied;
        }

        public static bool MergeStashkeys(List<StashKey> sks, bool _loadBeforeOperation = true)
        {
            PreApplyStashkey();

            if (sks != null && sks.Count > 1)
            {
                StashKey master = sks[0];

                string masterSystemCore = master.SystemCore;
                bool allCoresIdentical = true;

                foreach (StashKey item in sks)
                {
                    if (item.SystemCore != master.SystemCore)
                    {
                        allCoresIdentical = false;
                        break;
                    }
                }

                if (!allCoresIdentical && !RtcCore.AllowCrossCoreCorruption)
                {
                    MessageBox.Show("Merge attempt failed: Core mismatch\n\n" + string.Join("\n", sks.Select(it => $"{it.GameName} -> {it.SystemName} -> {it.SystemCore}")));


                    return false;
                }

                if (!RtcCore.AllowCrossCoreCorruption)
                    foreach (StashKey item in sks)
                    {
                        if (item.GameName != master.GameName)
                        {
                            MessageBox.Show("Merge attempt failed: game mismatch\n\n" + string.Join("\n", sks.Select(it => $"{it.GameName} -> {it.SystemName} -> {it.SystemCore}")));

                            return false;
                        }
                    }

                BlastLayer bl = new BlastLayer();

                foreach (StashKey item in sks)
                {
                    bl.Layer.AddRange(item.BlastLayer.Layer);
                }

                bl.Layer = bl.Layer.Distinct().ToList();

                CurrentStashkey = new StashKey(RtcCore.GetRandomKey(), master.ParentKey, bl)
                {
                    RomFilename = master.RomFilename,
                    SystemName = master.SystemName,
                    SystemCore = master.SystemCore,
                    GameName = master.GameName,
                    SyncSettings = master.SyncSettings,
                    StateLocation = master.StateLocation
                };


                bool isCorruptionApplied = CurrentStashkey?.BlastLayer?.Layer?.Count > 0;

                if (_loadBeforeOperation)
                {
                    if (!LoadState(CurrentStashkey))
                    {
                        return isCorruptionApplied;
                    }
                }
                else
                {
                    LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.APPLYBLASTLAYER, new object[] { CurrentStashkey.BlastLayer, true }, true);
                }


                if (StashAfterOperation)
                {
                    StashHistory.Add(CurrentStashkey);
                }


                PostApplyStashkey();
                return true;
            }
            MessageBox.Show("You need 2 or more items for Merging");
            return false;
        }

        public static bool LoadState(StashKey sk, bool reloadRom = true, bool applyBlastLayer = true)
        {
            bool success = LocalNetCoreRouter.QueryRoute<bool>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_LOADSTATE, new object[] { sk, reloadRom, applyBlastLayer }, true);
            return success;
        }

        public static StashKey SaveState(StashKey sk = null)
        {
            bool UseSavestates = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES];

            if (UseSavestates)
            {
                return LocalNetCoreRouter.QueryRoute<StashKey>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_SAVESTATE, sk, true);
            }
            else
            {
                return LocalNetCoreRouter.QueryRoute<StashKey>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_SAVESTATELESS, sk, true);
            }
        }


        public static void StockpileChanged()
        {
            //S.GET<RTC_StockpileBlastBoard_Form>().RefreshButtons();
        }


        public static bool AddCurrentStashkeyToStash(bool _stashAfterOperation = true)
        {
            bool isCorruptionApplied = CurrentStashkey?.BlastLayer?.Layer?.Count > 0;

            if (isCorruptionApplied && StashAfterOperation && _stashAfterOperation)
            {
                StashHistory.Add(CurrentStashkey);
            }

            return isCorruptionApplied;
        }

        /// <summary>
        /// Takes a stashkey and a list of keys, fixing the path and if a list of keys is provided, it'll look for all shared references and update them
        /// </summary>
        /// <param name="psk"></param>
        /// <param name="force"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static bool CheckAndFixMissingReference(StashKey psk, bool force = false, List<StashKey> keys = null, string customTitle = null, string customMessage = null)
        {
            if (!(bool?)AllSpec.VanguardSpec[VSPEC.SUPPORTS_REFERENCES] ?? false)
            {
                //Hack hack hack
                //In pre-504, some stubs would save references. This results in a fun infinite loop
                //As such, delete the referenced file because it doesn't matter as the implementation doesn't support references
                //Only do this if we explicitly know that the references are not supported. If there's missing spec info, don't do it.
                if (!(bool?)AllSpec.VanguardSpec[VSPEC.SUPPORTS_REFERENCES] == true)
                {
                    try
                    {
                        File.Delete(psk.RomFilename);
                    }
                    catch (Exception ex)
                    {
                        Common.Logging.GlobalLogger.Error(ex,
                            "Som-ething went terribly wrong when fixing missing references\n" +
                            "Your stockpile should be fine (might prompt you to fix it on load)" +
                            "Report this to the devs.");
                    }
                    psk.RomFilename = "";
                    return true;
                }
            }

            string message = customMessage ?? $"Can't find file {psk.RomFilename}\nGame name: {psk.GameName}\nSystem name: {psk.SystemName}\n\n To continue loading, provide a new file for replacement.";
            string title = customTitle ?? "Error: File not found";

            if (force || !File.Exists(psk.RomFilename))
            {
                if (DialogResult.OK == MessageBox.Show(message, title, MessageBoxButtons.OKCancel))
                {
                    OpenFileDialog ofd = new OpenFileDialog
                    {
                        DefaultExt = "*",
                        Title = "Select Replacement File",
                        Filter = $"Any file|*.*",
                        RestoreDirectory = true
                    };
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string filename = ofd.FileName;
                        string oldFilename = psk.RomFilename;
                        if (Path.GetFileName(psk.RomFilename) != Path.GetFileName(filename))
                        {
                            if (DialogResult.Cancel == MessageBox.Show($"Selected file {Path.GetFileName(filename)} has a different name than the old file {Path.GetFileName(psk.RomFilename)}.\nIf you know this file is correct, you can ignore this warning.\nContinue?", title,
                                    MessageBoxButtons.OKCancel))
                            {
                                return false;
                            }
                        }

                        foreach (var sk in keys.Where(x => x.RomFilename == oldFilename))
                        {
                            sk.RomFilename = filename;
                            sk.RomShortFilename = Path.GetFileName(sk.RomFilename);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static void ClearCurrentStockpile()
        {
            CurrentStockpile = new Stockpile();
            StockpileChanged();
        }

        public static string GetCurrentStockpilePath()
        {
            return CurrentStockpile?.Filename ?? "";
        }

        public static void SetCurrentStockpile(Stockpile sks)
        {
            CurrentStockpile = sks;
            StockpileChanged();
        }
    }
}
