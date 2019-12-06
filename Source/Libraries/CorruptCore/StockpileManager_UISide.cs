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
		private static Stockpile CurrentStockpile { get; set; }
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
            if (renameSaveStateWord != null && renameSaveStateWord is String s)
                saveStateWord = s;


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

				if (!allCoresIdentical && !RtcCore.AllowCrossCoreCorruption)
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
                        Filter = $"Any file|*.*",
                        RestoreDirectory = true
                    };
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string filename = ofd.FileName.ToString();
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
                        return false;
                }
                else
                    return false;
			return true;
        }

		public static bool Save(Stockpile sks, string filename, bool includeReferencedFiles = false, bool compress = true)
		{
			decimal saveProgress = 0;
			decimal percentPerFile = 0;
			if (sks.StashKeys.Count == 0)
			{
				MessageBox.Show("Can't save because the Current Stockpile is empty");
				return false;
			}

			if ((AllSpec.VanguardSpec[VSPEC.CORE_DISKBASED] as bool? ?? false) && ((AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME] as string ?? "DEFAULT") != ""))
			{
				var dr = MessageBox.Show("The currently loaded game is disk based and needs to be closed before saving. Press OK to close the game and continue saving.", "Saving requires closing game", MessageBoxButtons.OKCancel,
					MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
				if (dr == DialogResult.OK)
				{
					LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_CLOSEGAME, true);
				}
				else
				{
					return false;
				}
			}


			sks.Filename = filename;
			sks.ShortFilename = Path.GetFileName(sks.Filename);


			//clean temp folder
			try
			{
				RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs("Emptying TEMP", saveProgress += 2));
				EmptyFolder(Path.Combine("WORKING", "TEMP"));
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				return false;
			}

			//Watermarking RTC Version
			sks.RtcVersion = RtcCore.RtcVersion;
			sks.VanguardImplementation = (string)RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.NAME] ?? "ERROR";


			List<string> allRoms = new List<string>();
			if (includeReferencedFiles)
			{
				RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs("Prepping referenced files", saveProgress += 2));
				//populating Allroms array
				foreach (StashKey key in sks.StashKeys)
				{
					if (!allRoms.Contains(key.RomFilename))
					{
						allRoms.Add(key.RomFilename);

						//If it's a cue file, find the bins and fix the cue to be relative
						if (key.RomFilename.ToUpper().Contains(".CUE"))
						{
							string cueFolder = Path.GetDirectoryName(key.RomFilename);
							string[] cueLines = File.ReadAllLines(key.RomFilename);
							List<string> binFiles = new List<string>();

							string[] fixedCue = new string[cueLines.Length];
							for (int i = 0; i < cueLines.Length; i++)
							{
								if (cueLines[i].Contains("FILE") && cueLines[i].Contains("BINARY"))
								{
									int startFilename;
									int endFilename = cueLines[i].LastIndexOf('"');

									//If it's an absolute path, convert it to a relative path then fix the cue as well
									if (cueLines[i].Contains(':'))
									{
										startFilename = cueLines[i].LastIndexOfAny(new char[] { '\\', '/' }) + 1;
										fixedCue[i] = "FILE \"" + cueLines[i].Substring(startFilename, endFilename - startFilename) + "\" BINARY";
									}
									else
									{
										//Just copy the old cue into the new one
										startFilename = cueLines[i].IndexOf('"') + 1;
										fixedCue[i] = cueLines[i];
									}

									binFiles.Add(cueLines[i].Substring(startFilename, endFilename - startFilename));
								}
								else
									fixedCue[i] = cueLines[i];
							}
							//Write our new cue
							File.WriteAllLines(key.RomFilename, fixedCue);

							allRoms.AddRange(binFiles.Select(file => Path.Combine(cueFolder, file)));
						}

						if (key.RomFilename.ToUpper().Contains(".CCD"))
						{
							List<string> binFiles = new List<string>();

							if (File.Exists(Path.GetFileNameWithoutExtension(key.RomFilename) + ".sub"))
								binFiles.Add(Path.GetFileNameWithoutExtension(key.RomFilename) + ".sub");

							if (File.Exists(Path.GetFileNameWithoutExtension(key.RomFilename) + ".img"))
								binFiles.Add(Path.GetFileNameWithoutExtension(key.RomFilename) + ".img");

							allRoms.AddRange(binFiles);
						}
					}
				}

				percentPerFile = 20m / (allRoms.Count + 1);
				//populating temp folder with roms
				foreach (string str in allRoms)
				{
					RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Copying {Path.GetFileNameWithoutExtension(str)} to stockpile", saveProgress += percentPerFile));
					string rom = str;
					string romTempfilename = Path.Combine(RtcCore.workingDir, "TEMP", Path.GetFileName(rom));

					if (!File.Exists(rom))
					{
						if (MessageBox.Show($"Include referenced files was set but we couldn't find {rom}. Continue saving? (You'll need to reassociate the file at runtime)", "Couldn't find file.", MessageBoxButtons.YesNo) == DialogResult.No)
							return false;
					}


					//If the file already exists, overwrite it.
					if (File.Exists(romTempfilename))
					{
						//Whack the attributes in case a rom is readonly 
						File.SetAttributes(romTempfilename, FileAttributes.Normal);
						File.Delete(romTempfilename);
						File.Copy(rom, romTempfilename);
					}
					else
						File.Copy(rom, romTempfilename);
				}

				//Update the paths
				RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Fixing paths", saveProgress += 2));
				foreach (var sk in sks.StashKeys)
				{
					sk.RomShortFilename = Path.GetFileName(sk.RomFilename);
					sk.RomFilename = Path.Combine(RtcCore.workingDir, "SKS", sk.RomShortFilename);
				}
			}
			else
			{
				bool failure = false;
				//Gotta do this on the UI thread.
				SyncObjectSingleton.FormExecute(() =>
				{
					// We need to handle if they aren't including referenced files but the file is within the working dir where they'll get deleted (temp, sks, etc)
					foreach (StashKey key in sks.StashKeys)
					{
						string title = "Reference found in RTC dir";
						string message = $"Can't save with file {key.RomFilename}\nGame name: {key.GameName}\n\nThis file appears to be in temporary storage (e.g. loaded from a stockpile).\nTo save without references, you will need to provide a replacement from outside the RTC's working directory.\n\nPlease provide a new path to the file in question.";
						while (CorruptCore_Extensions.IsOrIsSubDirectoryOf(Path.GetDirectoryName(key.RomFilename), RtcCore.workingDir)) // Make sure they don't give a new file within working
							if (!StockpileManager_UISide.CheckAndFixMissingReference(key, true, sks.StashKeys, title, message))
							{
								failure = true;
								return;
							}
					}
				});

				if (failure)
					return false;
			}

			if ((bool?)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES] ?? false)
			{
				percentPerFile = (20m) / (sks.StashKeys.Count + 1);
				//Copy all the savestates
				foreach (StashKey key in sks.StashKeys)
				{
					// get savestate name
					string stateFilename = key.GameName + "." + key.ParentKey + ".timejump.State";
					RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Copying {stateFilename} to stockpile", saveProgress += percentPerFile));
					File.Copy(
						Path.Combine(RtcCore.workingDir, key.StateLocation.ToString(), stateFilename),
						Path.Combine(RtcCore.workingDir, "TEMP", stateFilename), true); // copy savestates to temp folder
				}
			}

			if ((bool?)AllSpec.VanguardSpec[VSPEC.SUPPORTS_CONFIG_MANAGEMENT] ?? false)
			{
				RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Copying configs to stockpile", saveProgress += 2));
				string[] configPaths = AllSpec.VanguardSpec[VSPEC.CONFIG_PATHS] as string[] ?? new string[] { };
				foreach (var path in configPaths)
					if (File.Exists(path))
					{
						Directory.CreateDirectory(Path.Combine(RtcCore.workingDir, "TEMP", "CONFIGS"));
						File.Copy(path, Path.Combine(RtcCore.workingDir, "TEMP", "CONFIGS", Path.GetFileName(path)));
					}
			}
			//Get all the limiter lists
			RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Finding limiter lists to copy", saveProgress += 5));
			var limiterLists = Filtering.GetAllLimiterListsFromStockpile(sks);
			if (limiterLists == null)
				return false;

			//Write them to a file
			foreach (var l in limiterLists.Keys)
			{
				RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Copying limiter lists to stockpile", saveProgress += 2));
				File.WriteAllLines(Path.Combine(RtcCore.workingDir, "TEMP", l + ".limiter"), limiterLists[l]);
			}
			//Create stockpile.xml to temp folder from stockpile object
			using (FileStream fs = File.Open(Path.Combine(RtcCore.workingDir, "TEMP", "stockpile.json"), FileMode.OpenOrCreate))
			{
				RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Creating stockpile.json", saveProgress += 2));
				JsonHelper.Serialize(sks, fs, Formatting.Indented);
				fs.Close();
			}

			string tempFilename = sks.Filename + ".temp";
			//If there's already a temp file from a previous failed save, delete it
			try
			{
				if (File.Exists(tempFilename))
					File.Delete(tempFilename);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return false;
			}

			CompressionLevel comp = CompressionLevel.Fastest;
			if (!compress)
				comp = CompressionLevel.NoCompression;

			RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Creating SKS", saveProgress += 10));
			//Create the file into temp
			ZipFile.CreateFromDirectory(Path.Combine(RtcCore.workingDir, "TEMP"), tempFilename, comp, false);

			//Remove the old stockpile
			try
			{
				RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Removing old stockpile", saveProgress += 2));
				if (File.Exists(sks.Filename))
					File.Delete(sks.Filename);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return false;
			}

			//Move us to the destination
			RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Moving SKS to destination", saveProgress += 2));
			File.Move(tempFilename, sks.Filename);

			//Clean out SKS
			try
			{
				RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Emptying SKS", saveProgress += 2));
				EmptyFolder(Path.Combine("WORKING", "SKS"));
			}
			catch (Exception e)
			{
				Console.Write(e);
				MessageBox.Show("Unable to empty the stockpile folder. There's probably something locking a file inside it (iso based game loaded?)\n. Your stockpile is saved, but your current session is bunk.\nRe-load the file");
			}

			var files = Directory.GetFiles(Path.Combine(RtcCore.workingDir, "TEMP"));
			percentPerFile = (10m) / (files.Length + 1);
			foreach (var file in files)
			{
				RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Copying limiter lists to stockpile", saveProgress += percentPerFile));
				try
				{
					File.Move(file, Path.Combine(RtcCore.workingDir, "SKS", Path.GetFileName(file)));
				}
				catch (Exception e)
				{
					Console.Write(e);
					MessageBox.Show("Unable to move " + Path.GetFileName(file) +
									"to SKS. Your stockpile should be saved.\n" +
									"If you're seeing this error, that means the file is probably in use. If it is, everything should technically be fine assuming it's the same file.\n" +
									"If the file you're seeing here has changed since the stockpile was last saved (rom edited manually), you should probably reload your stockpile from the file.");
				}

			}

			//Update savestate location info 
			percentPerFile = (5m) / (sks.StashKeys.Count + 1);
			foreach (StashKey sk in sks.StashKeys)
			{
				RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Updating StashKeySaveState Location for {sk.Alias}", saveProgress += percentPerFile));
				sk.StateLocation = StashKeySavestateLocation.SKS;
			}

			StockpileManager_UISide.CurrentStockpile = sks;
			RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Done", saveProgress = 100));
			return true;
		}

		public static Stockpile Load(string filename, bool import = false)
		{
			decimal loadProgress = 0;
			decimal percentPerFile = 0;
			if ((AllSpec.VanguardSpec[VSPEC.CORE_DISKBASED] as bool? ?? false) && ((AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME] as string ?? "DEFAULT") != ""))
			{
				var dr = MessageBox.Show($"The currently loaded game is disk based and needs to be closed before {(import ? "importing" : "loading")}. Press OK to close the game and continue loading.", "Loading requires closing game",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
				if (dr == DialogResult.OK)
				{
					LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_CLOSEGAME, true);
				}
				else
				{
					throw new OperationCanceledException("Operation cancelled by user");
				}
			}

			if (!File.Exists(filename))
			{
				throw new Exception("The stockpile wasn't found");
			}


			Stockpile sks;
			var extractFolder = import ? "TEMP" : "SKS";

			//Extract the stockpile
			RtcCore.OnProgressBarUpdate(null, new ProgressBarEventArgs("Extracting Stockpile (progress not reported during extraction)", loadProgress += 5));
			if (!Extract(filename, Path.Combine("WORKING", extractFolder), "stockpile.json"))
				return false;

			//Read in the stockpile
			try
			{
				RtcCore.OnProgressBarUpdate(null, new ProgressBarEventArgs("Reading Stockpile", loadProgress += 45));
				using (FileStream fs = File.Open(Path.Combine(RtcCore.workingDir, extractFolder, "stockpile.json")
					, FileMode.OpenOrCreate))
				{
					sks = JsonHelper.Deserialize<Stockpile>(fs);
					fs.Close();
				}
			}
			catch (Exception e)
			{
				MessageBox.Show("The Stockpile file could not be loaded" + e);
				return false;
			}


			RtcCore.OnProgressBarUpdate(null, new ProgressBarEventArgs("Checking Compatibility", loadProgress += 5));
			//Check version/implementation compatibility
			if (CheckCompatibility(sks))
				return false;

			if (import)
			{
				var allCopied = new List<string>();
				//Copy from temp to sks
				var files = Directory.GetFiles(Path.Combine(RtcCore.workingDir, "TEMP"));
				percentPerFile = 20m / (files.Length + 1);
				foreach (string file in files)
				{
					RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Merging {Path.GetFileNameWithoutExtension(file)} to stockpile", loadProgress += percentPerFile));
					if (!file.Contains(".sk"))
					{
						try
						{
							string dest = Path.Combine(RtcCore.workingDir, "SKS", Path.GetFileName(file));

							//Only copy if a version doesn't exist
							//This prevents copying over keys
							if (!File.Exists(dest))
							{
								File.Copy(file, dest); // copy roms/stockpile/whatever to sks folder
								allCopied.Add(dest);
							}

						}
						catch (Exception ex)
						{
							MessageBox.Show("Unable to copy a file from temp to sks. The culprit is " + file + ".\nCancelling operation.\n " + ex.ToString());
							//Attempt to cleanup
							foreach (var f in allCopied)
								File.Delete(f);
							return false;

						}
					}

				}
				EmptyFolder(Path.Combine("WORKING", "TEMP"));
			}
			else
			{
				//Update the current stockpile to this one
				StockpileManager_UISide.CurrentStockpile = sks;

				//fill list controls
				SyncObjectSingleton.FormExecute(() => dgvStockpile.Rows.Clear());


				//Update the filename in case they renamed it
				sks.Filename = filename;
			}

			//Set up the correct paths
			percentPerFile = 20m / (sks.StashKeys.Count + 1);
			foreach (StashKey t in sks.StashKeys)
			{
				RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Fixing up paths for {t.Alias}", loadProgress += percentPerFile));
				//If we have the file, update the path
				if (!String.IsNullOrEmpty(t.RomShortFilename))
				{
					var newFilename = Path.Combine(RtcCore.workingDir, "SKS", t.RomShortFilename);
					if (File.Exists(newFilename))
						t.RomFilename = newFilename;
				}

				t.StateLocation = StashKeySavestateLocation.SKS;
			}


			//Todo - Refactor this to get it out of the object
			//Populate the dgv
			RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Populating UI", loadProgress += 5));
			SyncObjectSingleton.FormExecute(() =>
			{
				foreach (StashKey key in sks.StashKeys)
					dgvStockpile?.Rows.Add(key, key.GameName, key.SystemName, key.SystemCore, key.Note);
			});


			RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Loading limiter lists", loadProgress += 5));
			//Load the limiter lists into the dictionary and UI
			Filtering.LoadStockpileLists(sks);


			//If there's a limiter missing, pop a message
			if (sks.MissingLimiter)
				MessageBox.Show(
					"This stockpile is missing a limiter list used by some blastunits.\n" +
					"Some corruptions probably won't work properly.\n" +
					"If the limiter list is found next time you save, it'll automatically be packed in.");

			RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Done", loadProgress = 100));
			return true;
		}

		public static bool Import(string filename, DataGridView dgvStockpile)
		{
			return Load(dgvStockpile, filename, true);

		}

	}
}
