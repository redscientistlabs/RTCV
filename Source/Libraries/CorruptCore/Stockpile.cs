namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Windows.Forms;
    using Ceras;
    using Newtonsoft.Json;
    using RTCV.Common.Objects;
    using RTCV.NetCore;
    using Exception = System.Exception;

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class Stockpile
    {
        [NonSerialized]
        [Ceras.Exclude]
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public List<StashKey> StashKeys = new List<StashKey>();

        public string Name;
        public string Filename;
        public string ShortFilename;
        public string RtcVersion;
        public string VanguardImplementation;
        public bool MissingLimiter;

        public Stockpile(DataGridView dgvStockpile)
        {
            foreach (DataGridViewRow row in dgvStockpile.Rows)
            {
                StashKeys.Add((StashKey)row.Cells[0].Value);
            }
        }

        public Stockpile()
        {
        }

        public override string ToString()
        {
            return Name ?? string.Empty;
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

            if ((AllSpec.VanguardSpec[VSPEC.CORE_DISKBASED] as bool? ?? false) && ((AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME] as string ?? "DEFAULT").Length != 0))
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
            if (includeReferencedFiles && ((bool?)RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.SUPPORTS_REFERENCES] ?? false))
            {
                RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs("Prepping referenced files", saveProgress += 2));
                //populating Allroms array
                foreach (StashKey key in sks.StashKeys)
                {
                    if (!allRoms.Contains(key.RomFilename))
                    {
                        allRoms.Add(key.RomFilename);

                        //If it's a cue file, find the bins and fix the cue to be relative
                        if (key.RomFilename.IndexOf(".CUE", StringComparison.OrdinalIgnoreCase) >= 0)
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
                                {
                                    fixedCue[i] = cueLines[i];
                                }
                            }
                            //Write our new cue
                            File.WriteAllLines(key.RomFilename, fixedCue);

                            allRoms.AddRange(binFiles.Select(file => Path.Combine(cueFolder, file)));
                        }

                        if (key.RomFilename.IndexOf(".CCD", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            List<string> binFiles = new List<string>();

                            if (File.Exists(Path.GetFileNameWithoutExtension(key.RomFilename) + ".sub"))
                            {
                                binFiles.Add(Path.GetFileNameWithoutExtension(key.RomFilename) + ".sub");
                            }

                            if (File.Exists(Path.GetFileNameWithoutExtension(key.RomFilename) + ".img"))
                            {
                                binFiles.Add(Path.GetFileNameWithoutExtension(key.RomFilename) + ".img");
                            }

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
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //If the file already exists, overwrite it.
                        if (File.Exists(romTempfilename))
                        {
                            //Whack the attributes in case a rom is readonly
                            File.SetAttributes(romTempfilename, FileAttributes.Normal);
                            File.Delete(romTempfilename);
                            File.Copy(rom, romTempfilename);
                        }
                        else
                        {
                            File.Copy(rom, romTempfilename);
                        }
                    }
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
                        while ((!string.IsNullOrEmpty(key.RomFilename)) && (CorruptCore_Extensions.IsOrIsSubDirectoryOf(Path.GetDirectoryName(key.RomFilename), RtcCore.workingDir))) // Make sure they don't give a new file within working
                        {
                            if (!StockpileManager_UISide.CheckAndFixMissingReference(key, true, sks.StashKeys, title, message))
                            {
                                failure = true;
                                return;
                            }
                        }
                    }
                });

                if (failure)
                {
                    return false;
                }
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
                {
                    if (File.Exists(path))
                    {
                        Directory.CreateDirectory(Path.Combine(RtcCore.workingDir, "TEMP", "CONFIGS"));
                        File.Copy(path, Path.Combine(RtcCore.workingDir, "TEMP", "CONFIGS", Path.GetFileName(path)));
                    }
                }
            }

            //Get all the limiter lists
            RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Finding limiter lists to copy", saveProgress += 5));
            var limiterLists = Filtering.GetAllLimiterListsFromStockpile(sks);
            if (limiterLists == null)
            {
                return false;
            }

            //Write them to a file
            foreach (var l in limiterLists.Keys)
            {
                RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Copying limiter lists to stockpile", saveProgress += 2));
                File.WriteAllLines(Path.Combine(RtcCore.workingDir, "TEMP", l + ".limiter"), limiterLists[l]);
            }
            //Create stockpile.json to temp folder from stockpile object
            using (FileStream fs = File.Open(Path.Combine(RtcCore.workingDir, "TEMP", "stockpile.json"), FileMode.OpenOrCreate))
            {
                RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Creating stockpile.json", saveProgress += 2));
                JsonHelper.Serialize(sks, fs, Formatting.Indented);
            }

            string tempFilename = sks.Filename + ".temp";
            //If there's already a temp file from a previous failed save, delete it
            try
            {
                if (File.Exists(tempFilename))
                {
                    File.Delete(tempFilename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            CompressionLevel comp = CompressionLevel.Fastest;
            if (!compress)
            {
                comp = CompressionLevel.NoCompression;
            }

            RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Creating SKS", saveProgress += 10));
            //Create the file into temp
            ZipFile.CreateFromDirectory(Path.Combine(RtcCore.workingDir, "TEMP"), tempFilename, comp, false);

            //Remove the old stockpile
            try
            {
                RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Removing old stockpile", saveProgress += 2));
                if (File.Exists(sks.Filename))
                {
                    File.Delete(sks.Filename);
                }
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

            RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Done", saveProgress = 100));
            return true;
        }

        public static OperationResults<Stockpile> Load(string filename, bool import = false)
        {
            var results = new OperationResults<Stockpile>();
            Stockpile sks;

            decimal loadProgress = 0;
            decimal percentPerFile = 0;
            if ((AllSpec.VanguardSpec[VSPEC.CORE_DISKBASED] as bool? ?? false) && ((AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME] as string ?? "DEFAULT").Length != 0))
            {
                var dr = MessageBox.Show($"The currently loaded game is disk based and needs to be closed before {(import ? "importing" : "loading")}. Press OK to close the game and continue loading.", "Loading requires closing game",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (dr == DialogResult.OK)
                {
                    LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_CLOSEGAME, true);
                }
                else
                {
                    results.AddError("Operation cancelled by user");
                    return results;
                }
            }

            if (!File.Exists(filename))
            {
                results.AddError("The selected stockpile was not found.");
                return results;
            }

            var extractFolder = import ? "TEMP" : "SKS";

            //Extract the stockpile
            RtcCore.OnProgressBarUpdate(null, new ProgressBarEventArgs("Extracting Stockpile (progress not reported during extraction)", loadProgress += 5));
            if (Extract(filename, Path.Combine("WORKING", extractFolder), "stockpile.json") is OperationResults<bool> r && r.Failed)
            {
                results.AddResults(r);
                return results;
            }

            //Read in the stockpile
            try
            {
                RtcCore.OnProgressBarUpdate(null, new ProgressBarEventArgs("Reading Stockpile", loadProgress += 45));
                using (FileStream fs = File.Open(Path.Combine(RtcCore.workingDir, extractFolder, "stockpile.json"), FileMode.OpenOrCreate))
                {
                    sks = JsonHelper.Deserialize<Stockpile>(fs);
                }
            }
            catch (Exception e)
            {
                results.AddError("Failed to read the stockpile", e);
                return results;
            }

            RtcCore.OnProgressBarUpdate(null, new ProgressBarEventArgs("Checking Compatibility", loadProgress += 5));
            //Check version/implementation compatibility

            var c = CheckCompatibility(sks);
            results.AddResults(c);
            if (c.Failed)
            {
                return results;
            }

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
                            try
                            {
                                foreach (var f in allCopied)
                                {
                                    File.Delete(f);
                                }
                            }
                            catch { }

                            results.AddError($"Unable to copy a file from temp to sks. The culprit is " + file + ".\nCancelling operation.\n ", ex);
                            return results;
                        }
                    }
                }
                EmptyFolder(Path.Combine("WORKING", "TEMP"));
            }
            else
            {
                //Update the filename in case they renamed it
                sks.Filename = filename;
            }

            //Set up the correct paths
            percentPerFile = 20m / (sks.StashKeys.Count + 1);
            foreach (StashKey t in sks.StashKeys)
            {
                RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Fixing up paths for {t.Alias}", loadProgress += percentPerFile));
                //If we have the file, update the path
                if (!string.IsNullOrEmpty(t.RomShortFilename))
                {
                    var newFilename = Path.Combine(RtcCore.workingDir, "SKS", t.RomShortFilename);
                    if (File.Exists(newFilename))
                    {
                        t.RomFilename = newFilename;
                    }
                }

                t.StateLocation = StashKeySavestateLocation.SKS;
            }

            RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Loading limiter lists", loadProgress += 5));
            //Load the limiter lists into the dictionary and UI
            Filtering.LoadStockpileLists(sks);

            //If there's a limiter missing, pop a message
            if (sks.MissingLimiter)
            {
                results.AddWarning(
                    "This stockpile is missing a limiter list used by some blastunits.\n" +
                    "Some corruptions probably won't work properly.\n" +
                    "If the limiter list is found next time you save, it'll automatically be packed in.");
            }

            RtcCore.OnProgressBarUpdate(sks, new ProgressBarEventArgs($"Done", 100));

            results.Result = sks;
            return results;
        }

        public static OperationResults<Stockpile> Import(string filename)
        {
            return Load(filename, true);
        }

        /// <summary>
        /// Checks a stockpile for compatibility with the current version of the RTC
        /// </summary>
        /// <param name="sks"></param>
        public static OperationResults CheckCompatibility(Stockpile sks)
        {
            var results = new OperationResults();

            var s = (string)RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.NAME] ?? "ERROR";
            if (!string.IsNullOrEmpty(sks.VanguardImplementation) && !sks.VanguardImplementation.Equals(s, StringComparison.OrdinalIgnoreCase) && sks.VanguardImplementation != "ERROR")
            {
                results.AddError($"The stockpile you loaded is for a different Vanguard implementation.\nThe Stockpile reported {sks.VanguardImplementation} but you're connected to {s}.\nThis is a fatal error. Aborting load.");
            }
            if (sks.RtcVersion != RtcCore.RtcVersion)
            {
                if (sks.RtcVersion == null)
                {
                    results.AddError("You have loaded a broken stockpile that didn't contain an RTC Version number\n. There is no reason to believe that these items will work.");
                }
                else
                {
                    results.AddWarning("You have loaded a stockpile created with RTC " + sks.RtcVersion + " using RTC " + RtcCore.RtcVersion + "\n" + "Items might not appear identical to how they when they were created or it is possible that they won't work.");
                }
            }
            return results;
        }

        /// <summary>
        /// Recursively deletes all files and folders within a directory
        /// </summary>
        /// <param name="baseDir"></param>
        public static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
            {
                return;
            }

            foreach (DirectoryInfo dir in baseDir.EnumerateDirectories())
            {
                RecursiveDelete(dir);
            }
            baseDir.Delete(true);
        }

        public static void EmptyFolder(string folder)
        {
            try
            {
                string targetFolder = Path.Combine(RtcCore.RtcDir, folder);

                foreach (string file in Directory.GetFiles(targetFolder))
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in Directory.GetDirectories(targetFolder))
                {
                    RecursiveDelete(new DirectoryInfo(dir));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to empty a temp folder! If your stockpile has any CD based games, close them before saving the stockpile! If this isn't the case, report this bug to the RTC developers." + ex.Message);
            }
        }

        /// <summary>
        /// Extracts a stockpile into a folder and ensures a master file exists
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="folder"></param>
        /// <param name="masterFile"></param>
        /// <returns></returns>
        public static OperationResults<bool> Extract(string filename, string folder, string masterFile)
        {
            var r = new OperationResults<bool>();
            try
            {
                EmptyFolder(folder);
                ZipFile.ExtractToDirectory(filename, Path.Combine(RtcCore.RtcDir, folder));

                if (!File.Exists(Path.Combine(RtcCore.RtcDir, folder, masterFile)))
                {
                    if (File.Exists(Path.Combine(RtcCore.RtcDir, folder, "stockpile.xml")))
                    {
                        r.AddError("Legacy stockpile found. This stockpile isn't supported by this version of the RTC.");
                    }
                    else if (File.Exists(Path.Combine(RtcCore.RtcDir, folder, "keys.xml")))
                    {
                        r.AddError("Legacy SSK found. This SSK isn't supported by this version of the RTC.");
                    }
                    else
                    {
                        r.AddError("The file could not be read properly. Master file missing");
                    }

                    EmptyFolder(folder);
                    return r;
                }

                return r;
            }
            catch (Exception e)
            {
                //If it errors out, empty the folder
                EmptyFolder(folder);
                r.AddError($"The file could not be read properly (an error occurred, check the log file for more details)", logger, e);
                return r;
            }
        }

        public static void LoadConfigFromStockpile()
        {
            if (((bool?)AllSpec.VanguardSpec[VSPEC.SUPPORTS_CONFIG_MANAGEMENT] ?? false) == false)
            {
                MessageBox.Show("The currently selected emulator doesn't support config management");
                return;
            }

            string[] configPaths = AllSpec.VanguardSpec[VSPEC.CONFIG_PATHS] as string[];
            if (configPaths == null)
            {
                throw new Exception("ConfigMode was set but ConfigPath was null!");
            }

            string filename;
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "sks",
                Title = "Open Stockpile File",
                Filter = "SKS files|*.sks",
                RestoreDirectory = true
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.FileName;
            }
            else
            {
                return;
            }

            foreach (var path in configPaths)
            {
                var dir = Path.GetDirectoryName(path);
                var backupFilename = Path.Combine(dir, "backup_" + Path.GetFileName(path));
                if (File.Exists(backupFilename) && MessageBox.Show("Do you want to overwrite the previous config backup with the current config?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                File.Copy(path, backupFilename, true);
            }

            if (Extract(filename, Path.Combine("WORKING", "TEMP"), "stockpile.json") is OperationResults<bool> r && r.Failed)
            {
                return;
            }

            //Configs are stored in the "configs" folder within the stockpile
            //Build up a filename map and use that when copying back in

            Dictionary<string, string> name2filedico = new Dictionary<string, string>();
            foreach (var str in configPaths)
            {
                var relPath = CorruptCore_Extensions.GetRelativePath(RtcCore.EmuDir, str);

                name2filedico.Add(Path.GetFileName(str), relPath);
            }

            //Parse the configs folder and then if the emulator is looking for that file, copy it over.
            //Otherwise ignore it (version change, wrong emulator, etc)
            var newConfigPath = Path.Combine(RtcCore.workingDir, "TEMP", "CONFIGS");
            if (!Directory.Exists(newConfigPath))
            {
                MessageBox.Show("No configs found in stockpile");
                return;
            }

            //There's no prmomise that the implementation will always have every file
            //We delete existing files that line up with what the stockpile COULD contain as a non-existent config counts as a valid config.
            foreach (var file in Directory.GetFiles(newConfigPath))
            {
                var name = Path.GetFileName(file);
                if (name2filedico.ContainsKey(name))
                {
                    //Nuke the old config if it exists
                    if (File.Exists(RtcCore.EmuDir + name2filedico[name]))
                    {
                        File.Delete(RtcCore.EmuDir + name2filedico[name]);
                    }

                    //Bring in our new config.
                    if (File.Exists(file))
                    {
                        File.Copy(file, RtcCore.EmuDir + name2filedico[name], true);
                    }
                }
            }

            ProcessStartInfo p = new ProcessStartInfo
            {
                WorkingDirectory = RtcCore.EmuDir,
                FileName = Path.Combine(RtcCore.EmuDir, "RESTARTDETACHEDRTC.bat")
            };
            Process.Start(p);
        }

        public static void RestoreEmuConfig()
        {
            if (((bool?)AllSpec.VanguardSpec[VSPEC.SUPPORTS_CONFIG_MANAGEMENT] ?? false) == false)
            {
                MessageBox.Show("The currently selected emulator doesn't support config management");
                return;
            }

            string[] configPaths = AllSpec.VanguardSpec[VSPEC.CONFIG_PATHS] as string[];
            if (configPaths == null)
            {
                throw new Exception("ConfigMode was set but ConfigPath was null!");
            }

            Dictionary<string, string> name2filedico = new Dictionary<string, string>();
            foreach (var str in configPaths)
            {
                var path = Path.Combine(Path.GetDirectoryName(str), "backup_" + Path.GetFileName(str));
                if (File.Exists(path))
                {
                    File.Copy(path, str, true);
                }
            }

            ProcessStartInfo p = new ProcessStartInfo
            {
                WorkingDirectory = RtcCore.EmuDir,
                FileName = Path.Combine(RtcCore.EmuDir, "RESTARTDETACHEDRTC.bat")
            };
            Process.Start(p);
        }
    }
}
