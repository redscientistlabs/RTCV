using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class StockpileManager_UISide
	{
		//Object references
		public static Stockpile CurrentStockpile { get; set; }
		public static StashKey CurrentStashkey { get; set; }
        public static StashKey CurrentSavestateStashKey { get; set; }
		public static volatile StashKey BackupedState;
		public static bool StashAfterOperation = true;
		public static volatile List<StashKey> StashHistory = new List<StashKey>();

		private static void PreApplyStashkey()
		{
			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);

            bool UseSavestates = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES];

            LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_PRECORRUPTACTION, null,true);
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

		public static bool ApplyStashkey(StashKey sk, bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();

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
				LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.APPLYBLASTLAYER, new object[] {sk?.BlastLayer, true}, true);
			}

			PostApplyStashkey();
			return isCorruptionApplied;
		}

		public static bool Corrupt(bool _loadBeforeOperation = true)
		{
            string saveStateWord = "Savestate";

            object renameSaveStateWord = AllSpec.VanguardSpec[VSPEC.RENAME_SAVESTATE];
            if (renameSaveStateWord != null && renameSaveStateWord is String s)
                saveStateWord = s;

            PreApplyStashkey();
            StashKey psk = CurrentSavestateStashKey;

            bool UseSavestates = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES];
            if (!UseSavestates)
                psk = SaveState();

            if (psk == null && UseSavestates)
			{
				MessageBox.Show($"The Glitch Harvester could not perform the CORRUPT action\n\nEither no {saveStateWord} Box was selected in the {saveStateWord} Manager\nor the {saveStateWord} Box itself is empty.");
				return false;
			}

			string currentGame = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.GAMENAME.ToString()];
			if (UseSavestates && (currentGame == null || psk.GameName != currentGame)) 
			{
				LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_LOADROM, psk.RomFilename, true);
			}

			//We make it without the blastlayer so we can send it across and use the cached version without needing a prototype
			CurrentStashkey = new StashKey(CorruptCore.GetRandomKey(), psk.ParentKey, null)
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

		public static bool InjectFromStashkey(StashKey sk, bool _loadBeforeOperation = true)
		{
            string saveStateWord = "Savestate";

            object renameSaveStateWord = AllSpec.VanguardSpec[VSPEC.RENAME_SAVESTATE];
            if (renameSaveStateWord != null && renameSaveStateWord is String s)
                saveStateWord = s;


            PreApplyStashkey();

            StashKey psk = CurrentSavestateStashKey;

            if (psk == null)
			{
				MessageBox.Show($"The Glitch Harvester could not perform the INJECT action\n\nEither no {saveStateWord} Box was selected in the {saveStateWord} Manager\nor the {saveStateWord} Box itself is empty.");
				return false;
			}

			if (psk.SystemCore != sk.SystemCore && !CorruptCore.AllowCrossCoreCorruption)
			{
				MessageBox.Show("Merge attempt failed: Core mismatch\n\n" + $"{psk.GameName} -> {psk.SystemName} -> {psk.SystemCore}\n{sk.GameName} -> {sk.SystemName} -> {sk.SystemCore}");
				return false;
			}

			CurrentStashkey = new StashKey(CorruptCore.GetRandomKey(), psk.ParentKey, sk.BlastLayer)
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
					return false;
			}
			else
				LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.APPLYBLASTLAYER, new object[] { CurrentStashkey.BlastLayer, true }, true);

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
				return isCorruptionApplied;

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
					if (item.SystemCore != master.SystemCore)
					{
						allCoresIdentical = false;
						break;
					}

				if (!allCoresIdentical && !CorruptCore.AllowCrossCoreCorruption)
				{
					MessageBox.Show("Merge attempt failed: Core mismatch\n\n" + string.Join("\n", sks.Select(it => $"{it.GameName} -> {it.SystemName} -> {it.SystemCore}")));


					return false;
				}

				foreach (StashKey item in sks)
					if (item.GameName != master.GameName)
					{
						MessageBox.Show("Merge attempt failed: game mismatch\n\n" + string.Join("\n", sks.Select(it => $"{it.GameName} -> {it.SystemName} -> {it.SystemCore}")));

						return false;
					}


				BlastLayer bl = new BlastLayer();

				foreach (StashKey item in sks)
					bl.Layer.AddRange(item.BlastLayer.Layer);

				bl.Layer = bl.Layer.Distinct().ToList();

				CurrentStashkey = new StashKey(CorruptCore.GetRandomKey(), master.ParentKey, bl)
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
			bool success = LocalNetCoreRouter.QueryRoute<bool>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_LOADSTATE, new object[] {sk, true, applyBlastLayer}, true);
			return success;
		}

		public static StashKey SaveState(StashKey sk = null, bool threadSave = false)
		{
            bool UseSavestates = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES];

            if (UseSavestates)
                return LocalNetCoreRouter.QueryRoute<StashKey>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_SAVESTATE, sk, true);
            else
                return LocalNetCoreRouter.QueryRoute<StashKey>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_SAVESTATELESS, sk, true);

        }


        public static void StockpileChanged()
		{
			//S.GET<RTC_StockpileBlastBoard_Form>().RefreshButtons();
		}


		public static bool AddCurrentStashkeyToStash(bool _stashAfterOperation = true)
		{
			bool isCorruptionApplied = CurrentStashkey?.BlastLayer?.Layer?.Count > 0;

			if (StashAfterOperation && _stashAfterOperation)
			{
				StashHistory.Add(CurrentStashkey);
			}

			return isCorruptionApplied;
		}

        /// <summary>
        /// Takes a stashkey and a list of keys, fixing the path and if a list of keys is provided, it'll look for all shared references and update them
        /// </summary>
        /// <param name="psk"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static bool CheckAndFixMissingReference(StashKey psk, bool force = false, List<StashKey> keys = null, string customTitle = null, string customMessage = null)
		{
			string message = customMessage ?? $"Can't find file {psk.RomFilename}\nGame name: {psk.GameName}\nSystem name: {psk.SystemName}\n\n To continue loading, provide a new file for replacement.";
			string title = customTitle ?? "Error: File not found";

            if (force || !File.Exists(psk.RomFilename))
                if (DialogResult.OK == MessageBox.Show(message, title, MessageBoxButtons.OKCancel))
                {
                    OpenFileDialog ofd = new OpenFileDialog
                    {
                        DefaultExt = "*",
                        Title = "Select Replacement File",
                        Filter = "Any file|*.*",
                        RestoreDirectory = true
                    };
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string filename = ofd.FileName.ToString();
                        string oldFilename = psk.RomFilename;
                        foreach (var sk in keys.Where(x => x.RomFilename == oldFilename))
                        {
                            sk.RomFilename = filename;
                            sk.RomShortFilename = Path.GetFileName(sk.RomFilename);
                        }
                    }
                    else
                        return false;
                }
                else
                    return false;
			return true;
        }


    }
}
