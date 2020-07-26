namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Numerics;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using Ceras;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
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

    [Serializable]
    [Ceras.MemberConfig(TargetMember.AllPublic)]
    public class StashKey : ICloneable, INote
    {
        [NonSerialized]
        [Ceras.Exclude]
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string RomFilename { get; set; }
        public string RomShortFilename { get; set; }
        public byte[] RomData { get; set; }

        public string StateShortFilename { get; set; }
        public string StateFilename { get; set; }
        public byte[] StateData { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public StashKeySavestateLocation StateLocation { get; set; } = StashKeySavestateLocation.SESSION;

        public Dictionary<string, string> KnownLists { get; set; } = new Dictionary<string, string>();

        public string SystemName { get; set; }
        public string SystemDeepName { get; set; }
        public string SystemCore { get; set; }
        public List<string> SelectedDomains { get; set; } = new List<string>();
        public string GameName { get; set; }
        public string SyncSettings { get; set; }
        public string Note { get; set; }

        public string Key { get; set; }
        public string ParentKey { get; set; }
        public BlastLayer BlastLayer { get; set; }

        private string alias;
        public string Alias
        {
            get => alias ?? Key;
            set => alias = value;
        }

        public StashKey()
        {
            string key = RtcCore.GetRandomKey();
            string parentkey = null;
            BlastLayer blastlayer = new BlastLayer();
            StashKeyConstructor(key, parentkey, blastlayer);
        }

        public StashKey(string key, string parentkey, BlastLayer blastlayer)
        {
            StashKeyConstructor(key, parentkey, blastlayer);
        }

        private void StashKeyConstructor(string key, string parentkey, BlastLayer blastlayer)
        {
            Key = key;
            ParentKey = parentkey;
            BlastLayer = blastlayer;

            RomFilename = (string)RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.OPENROMFILENAME] ?? "ERROR";
            SystemName = (string)RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.SYSTEM] ?? "ERROR";
            SystemCore = (string)RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.SYSTEMCORE] ?? "ERROR";
            GameName = (string)RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.GAMENAME] ?? "ERROR";
            SyncSettings = (string)RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.SYNCSETTINGS] ?? "";

            this.SelectedDomains = ((string[])RTCV.NetCore.AllSpec.UISpec["SELECTEDDOMAINS"]).ToList();
        }

        public object Clone()
        {
            object sk = ObjectCopierCeras.Clone(this);
            ((StashKey)sk).Key = RtcCore.GetRandomKey();
            ((StashKey)sk).Alias = null;
            return sk;
        }

        public static void SetCore(StashKey sk)
        {
            SetCore(sk.SystemName, sk.SystemCore);
        }

        public static void SetCore(string systemName, string systemCore)
        {
            LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_KEY_SETSYSTEMCORE, new object[] { systemName, systemCore }, true);
        }

        public override string ToString()
        {
            return Alias;
        }

        /// <summary>
        /// Can be called from UI Side
        /// </summary>
        public bool Run()
        {
            StockpileManager_UISide.CurrentStashkey = this;
            return StockpileManager_UISide.ApplyStashkey(this);
        }

        /// <summary>
        /// Can be called from UI Side
        /// </summary>
        public void RunOriginal()
        {
            StockpileManager_UISide.CurrentStashkey = this;
            StockpileManager_UISide.OriginalFromStashkey(this);
        }

        public byte[] EmbedState()
        {
            if (StateFilename == null)
            {
                return null;
            }

            if (this.StateData != null)
            {
                return this.StateData;
            }

            byte[] stateData = File.ReadAllBytes(StateFilename);
            this.StateData = stateData;

            return stateData;
        }

        public bool DeployState()
        {
            if (StateShortFilename == null || this.StateData == null)
            {
                return false;
            }

            string deployedStatePath = GetSavestateFullPath();

            if (File.Exists(deployedStatePath))
            {
                return true;
            }

            File.WriteAllBytes(deployedStatePath, this.StateData);

            return true;
        }

        public string GetSavestateFullPath()
        {
            return Path.Combine(RtcCore.workingDir, this.StateLocation.ToString(), this.GameName + "." + this.ParentKey + ".timejump.State"); // get savestate name
        }

        //Todo - Replace this when compat is broken
        public void PopulateKnownLists()
        {
            if (BlastLayer.Layer == null) {
                MessageBox.Show($"Something went really wrong. Stashkey {Alias}.\nThere doesn't appear to be a linked blastlayer.\nWill attempt to continue saving. If save fails, remove {Alias} from your stockpile and save again.\nSend this stockpile and any info on how you got into this state to the devs.");
                return;
            }
            List<string> knownListKeys = new List<string>();
            foreach (var bu in BlastLayer.Layer.Where(x => x.LimiterListHash != null))
            {
                logger.Trace("Looking for knownlist {bu.LimiterListHash}", bu.LimiterListHash);
                if (knownListKeys.Contains(bu.LimiterListHash))
                {
                    logger.Trace("knownListKeys already contains {bu.LimiterListHash}", bu.LimiterListHash);
                    logger.Trace("Done\n");
                    continue;
                }

                logger.Trace("Adding {bu.LimiterListHash} to knownListKeys", bu.LimiterListHash);
                knownListKeys.Add(bu.LimiterListHash);

                logger.Trace("Getting name of {bu.LimiterListHash} from Hash2NameDico", bu.LimiterListHash);
                Filtering.Hash2NameDico.TryGetValue(bu.LimiterListHash, out string name);

                if (name == null)
                {
                    name = "UNKNOWN_" + Filtering.StockpileListCount++;
                }
                else
                {
                    name = Path.GetFileNameWithoutExtension(name);
                }

                logger.Trace("Setting KnownLists[{bu.LimiterListHash}] to {name}", bu.LimiterListHash, name);
                this.KnownLists[bu.LimiterListHash] = name;
                logger.Trace("Done");
            }
        }
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class SaveStateKey
    {
        public StashKey StashKey = null;
        public string Text = "";

        public SaveStateKey(StashKey stashKey, string text)
        {
            StashKey = stashKey;
            Text = text;
        }
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class SaveStateKeys
    {
        public string VanguardImplementation { get; set; }
        public List<StashKey> StashKeys = new List<StashKey>();
        public List<string> Text = new List<string>();

        public SaveStateKeys()
        {
            VanguardImplementation = (string)RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.NAME] ?? "ERROR";
        }
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class BlastTarget
    {
        public string Domain = null;
        public long Address = 0;

        public BlastTarget(string _domain, long _address)
        {
            Domain = _domain;
            Address = _address;
        }
    }

    [Ceras.MemberConfig(TargetMember.All)]
    [Serializable]
    [XmlInclude(typeof(BlastUnit))]
    public class BlastLayer : ICloneable, INote
    {
        public List<BlastUnit> Layer;

        public BlastLayer()
        {
            Layer = new List<BlastUnit>();
        }

        public BlastLayer(List<BlastUnit> _layer)
        {
            Layer = _layer;
        }

        public object Clone()
        {
            return ObjectCopierCeras.Clone(this);
        }

        public void Apply(bool storeUncorruptBackup, bool followMaximums = false, bool mergeWithPrevious = false)
        {
            if (storeUncorruptBackup && this != StockpileManager_EmuSide.UnCorruptBL)
            {
                BlastLayer UnCorruptBL_Backup = null;
                BlastLayer CorruptBL_Backup = null;

                if (mergeWithPrevious)
                {
                    //UnCorruptBL_Backup = (BlastLayer)StockpileManager_EmuSide.UnCorruptBL?.Clone();
                    //CorruptBL_Backup = (BlastLayer)StockpileManager_EmuSide.CorruptBL?.Clone();

                    UnCorruptBL_Backup = StockpileManager_EmuSide.UnCorruptBL;
                    CorruptBL_Backup = StockpileManager_EmuSide.CorruptBL;
                }

                StockpileManager_EmuSide.UnCorruptBL = GetBackup();
                StockpileManager_EmuSide.CorruptBL = this;

                if (mergeWithPrevious)
                {
                    if (UnCorruptBL_Backup.Layer != null)
                    {
                        if (StockpileManager_EmuSide.UnCorruptBL.Layer == null)
                            StockpileManager_EmuSide.UnCorruptBL.Layer = new List<BlastUnit>();

                        StockpileManager_EmuSide.UnCorruptBL.Layer.AddRange(UnCorruptBL_Backup.Layer);
                    }

                    if (CorruptBL_Backup.Layer != null)
                    {
                        if (StockpileManager_EmuSide.CorruptBL.Layer == null)
                            StockpileManager_EmuSide.CorruptBL.Layer = new List<BlastUnit>();

                        StockpileManager_EmuSide.CorruptBL.Layer.AddRange(CorruptBL_Backup.Layer);
                    }
                }
            }

            bool success;
            bool UseRealtime = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_REALTIME];

            try
            {
                foreach (BlastUnit bb in Layer)
                {
                    if (bb == null) //BlastCheat getBackup() always returns null so they can happen and they are valid
                    {
                        success = true;
                    }
                    else
                    {
                        success = bb.Apply(true);
                    }

                    if (!success)
                    {
                        throw new Exception(
                        "One of the BlastUnits in the BlastLayer failed to Apply().\n\n" +
                        "The operation was cancelled");
                    }
                }

                //Only filter if there are actually enabled units
                if (Layer.Any(x => x.IsEnabled))
                {
                    StepActions.FilterBuListCollection();

                    //If we're not using realtime, we execute right away.
                    if (!UseRealtime)
                    {
                        StepActions.Execute();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                            "An error occurred in RTC while applying a BlastLayer to the game.\n\n" +
                            "The operation was cancelled\n\n" + ex.Message
                            );
            }
            finally
            {
                if (followMaximums)
                {
                    StepActions.RemoveExcessInfiniteStepUnits();
                }
            }
        }

        public BlastLayer GetBakedLayer()
        {
            List<BlastUnit> BackupLayer = new List<BlastUnit>();

            BackupLayer.AddRange(Layer.Select(it => it.GetBakedUnit()));

            return new BlastLayer(BackupLayer);
        }

        public BlastLayer GetBackup()
        {
            List<BlastUnit> BackupLayer = new List<BlastUnit>();

            BackupLayer.AddRange(Layer.Select(it => it.GetBackup()).Where(it => it != null));

            return new BlastLayer(BackupLayer);
        }

        public void Reroll()
        {
            foreach (BlastUnit bu in Layer.Where(x => x.IsLocked == false))
            {
                bu.Reroll();
            }
        }

        public void RasterizeVMDs(string vmdToRasterize = null)
        {
            List<BlastUnit> l = new List<BlastUnit>();
            //Inserting turned out to be WAY too cpu intensive, so just sacrifice the ram and rebuild the layer
            foreach (var bu in Layer)
            {
                var u = bu.GetRasterizedUnits(vmdToRasterize);
                l.AddRange(u);
            }
            this.Layer = l;
        }

        private string shared = "[DIFFERENT]";

        [JsonIgnore]
        public string Note
        {
            get
            {
                if (Layer.All(x => x.Note == Layer.First().Note))
                {
                    return Layer.FirstOrDefault()?.Note;
                }
                return shared;
            }
            set
            {
                if (value == shared)
                {
                    return;
                }
                foreach (BlastUnit bu in Layer)
                {
                    bu.Note = value;
                }
            }
        }

        public void SanitizeDuplicates()
        {
            /*
            Layer = Layer.GroupBy(x => new { x.Address, x.Domain })
              .Where(g => g.Count() > 1)
              .Select(y => y.First())
              .ToList();
              */

            List<BlastUnit> bul = new List<BlastUnit>(Layer.ToArray().Reverse());
            List<ValueTuple<string, long>> usedAddresses = new List<ValueTuple<string, long>>();

            foreach (BlastUnit bu in bul)
            {
                if (!usedAddresses.Contains(new ValueTuple<string, long>(bu.Domain, bu.Address)) && !bu.IsLocked)
                {
                    usedAddresses.Add(new ValueTuple<string, long>(bu.Domain, bu.Address));
                }
                else if (!bu.IsLocked)
                {
                    Layer.Remove(bu);
                }
            }
        }
    }

    /// <summary>
    /// Working data for BlastUnits.
    /// Not serialized
    /// </summary>
    [Ceras.MemberConfig(TargetMember.None)]
    public class BlastUnitWorkingData
    {
        //We Calculate a LastFrame at the beginning of execute
        [NonSerialized]
        public int LastFrame = -1;
        //We calculate ExecuteFrameQueued which is the ExecuteFrame + the currentframe that was calculated at the time of it entering the execution pool
        [NonSerialized]
        public int ExecuteFrameQueued = 0;

        //We use ApplyValue so we don't need to keep re-calculating the tiled value every execute if we don't have to.
        [NonSerialized]
        public byte[] ApplyValue = null;

        //The data that has been backed up. This is a list of bytes so if they start backing up at IMMEDIATE, they can have historical backups
        [NonSerialized]
        public Queue<byte[]> StoreData = new Queue<byte[]>();
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class BlastUnit : INote
    {
        public object Clone()
        {
            return ObjectCopierCeras.Clone(this);
        }

        [Category("Settings")]
        [Description("Whether or not the BlastUnit will apply if the stashkey is run")]
        [DisplayName("Enabled")]
        public bool IsEnabled { get; set; } = true;

        [Category("Settings")]
        [Description("Whether or not this unit will be affected by batch operations (disable 50, invert, etc)")]
        [DisplayName("Locked")]
        public bool IsLocked { get; set; } = false;

        [Category("Data")]
        [Description("Whether or not the unit's values need to be flipped due to endianess")]
        [DisplayName("Big Endian")]
        public bool BigEndian { get; set; }

        [Category("Data")]
        [Description("The domain this unit will target")]
        [DisplayName("Domain")]
        public string Domain { get; set; }

        [Category("Data")]
        [Description("The address this unit will target")]
        [DisplayName("Address")]
        public long Address { get; set; }

        private int precision;

        [Category("Data")]
        [Description("The precision of this unit")]
        [DisplayName("Precision")]
        public int Precision
        {
            get => precision;
            set
            {
                int max = 16348; //The textbox breaks if I go over 20k
                if (value > max)
                {
                    value = max;
                }
                //Cache the old precision
                int oldPrecision = precision;
                //Update the precision
                precision = value;

                //If the user is changing the precision and already has a Value set, we need to update that array
                if (Value != null && oldPrecision != precision && Value.Length != precision)
                {
                    //If the precision is being set to 0, force it back to 1 and fill in an empty value
                    if (precision < 1)
                    {
                        Value = new byte[1];
                        precision = 1;
                    }
                    //If Value was 0 bytes long for some reason (deserialization?), just make a new byte of the correct length
                    else if (Value.Length == 0)
                    {
                        Value = new byte[value];
                    }
                    //Figure out the new length
                    else
                    {
                        //If there was no precision set, set it to 1
                        if (oldPrecision == 0)
                        {
                            oldPrecision = 1;
                        }

                        byte[] temp = new byte[precision];
                        //If the new unit is larger, copy it over left padded
                        if (precision > oldPrecision)
                        {
                            Value.CopyTo(temp, precision - oldPrecision);
                        }
                        //If the new unit is smaller, truncate it (first X bytes cut off)
                        else
                        {
                            int j = 0;
                            for (int i = oldPrecision - precision; i < oldPrecision; i++)
                            {
                                temp[j] = Value[i];
                                j++;
                            }
                        }
                        Value = temp;
                    }
                }
            }
        }

        private BlastUnitSource source;

        [Category("Source")]
        [Description("The source for the value for this unit for STORE mode")]
        [DisplayName("Source")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BlastUnitSource Source
        {
            get => source;
            set
            {
                //Cleanup from other types of units
                switch (value)
                {
                    case BlastUnitSource.STORE:
                        {
                            Value = null;
                            break;
                        }

                    case BlastUnitSource.VALUE:
                        {
                            if (Value == null)
                            {
                                Value = new byte[Precision];
                            }

                            break;
                        }
                }
                source = value;
            }
        }

        [Category("Store")]
        [Description("The time when the store will take place")]
        [DisplayName("Store Time")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StoreTime StoreTime { get; set; }

        [Category("Store")]
        [Description("The type of store that when the store will take place")]
        [DisplayName("Store Type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StoreType StoreType { get; set; }

        [JsonIgnore]
        [Category("Value")]
        [Description("The value used for the BlastUnit in VALUE mode")]
        [DisplayName("Value")]
        public byte[] Value { get; set; }

        [Category("Value")]
        [Description("Gets and sets Value[] through a string. Used for Textboxes")]
        [DisplayName("ValueString")]
        [Ceras.Exclude]
        public string ValueString
        {
            get
            {
                if (Value == null)
                {
                    return string.Empty;
                }

                return BitConverter.ToString(this.Value).Replace("-", string.Empty);
            }
            set
            {
                //If there's no precision, use the length of the string rounded up
                int p = this.Precision;
                if (p == 0 && value.Length != 0)
                {
                    p = (value.Length / 2) + (Value.Length % 2);
                }
                var temp = CorruptCore_Extensions.StringToByteArrayPadLeft(value, p);
                if (temp != null)
                {
                    this.Value = temp;
                }
            }
        }

        [Category("Store")]
        [Description("The domain used for the STORE operation")]
        [DisplayName("Source Domain")]
        public string SourceDomain { get; set; }

        [Category("Store")]
        [Description("The address used for the STORE operation")]
        [DisplayName("Source Address")]
        public long SourceAddress { get; set; }

        [Category("Modifiers")]
        [Description("How much to tilt the value before poking memory")]
        [DisplayName("Tilt Value")]
        public BigInteger TiltValue { get; set; }

        public int ExecuteFrame { get; set; }
        public int Lifetime { get; set; }
        public bool Loop { get; set; } = false;

        public int? LoopTiming { get; set; } = null;

        [Category("Limiter")]
        [Description("What mode to use for the limiter in STORE mode")]
        [DisplayName("Store Limiter Source")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StoreLimiterSource StoreLimiterSource { get; set; }

        [Category("Limiter")]
        [Description("When to apply the limiter list")]
        [DisplayName("Limiter List")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LimiterTime LimiterTime { get; set; }

        [Category("Limiter")]
        [Description("The hash of the Limiter List in use")]
        [DisplayName("Limiter List Hash")]
        public string LimiterListHash { get; set; }

        [Category("Limiter")]
        [Description("Invert the limiter so the unit only applies if the value doesn't match the limiter")]
        [DisplayName("Invert Limiter")]
        public bool InvertLimiter { get; set; }

        [Category("Data")]
        [Description("Whether or not the unit was originally seeded with a value list")]
        [DisplayName("Generated Using Value List")]
        public bool GeneratedUsingValueList { get; set; }

        [Category("Misc")]
        [Description("Note associated with this unit")]
        public string Note { get; set; }

        //Don't serialize this
        [NonSerialized, XmlIgnore, JsonIgnore, Ceras.Exclude]
        public BlastUnitWorkingData Working;

        /// <summary>
        /// Creates a Blastunit that utilizes a backup.
        /// </summary>
        /// <param name="storeType">The type of store</param>
        /// <param name="storeTime">The time of the store</param>
        /// <param name="domain">The domain of the blastunit</param>
        /// <param name="address">The address of the blastunit</param>
        /// <param name="bigEndian">If the Blastunit is being applied to a big endian system. Results in the bytes being flipped before apply</param>
        /// <param name="applyFrame">The frame on which the BlastUnit will start executing</param>
        /// <param name="lifetime">How many frames the BlastUnit will execute for. 0 for infinite</param>
        /// <param name="note"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isLocked"></param>
        public BlastUnit(StoreType storeType, StoreTime storeTime,
            string domain, long address, string sourceDomain, long sourceAddress, int precision, bool bigEndian, int executeFrame = 0, int lifetime = 1,
            string note = null, bool isEnabled = true, bool isLocked = false, int? loopTiming = null)
        {
            Source = BlastUnitSource.STORE;
            StoreTime = storeTime;
            StoreType = storeType;

            Domain = domain;
            Address = address;
            SourceDomain = sourceDomain;
            SourceAddress = sourceAddress;
            Precision = precision;
            BigEndian = bigEndian;
            ExecuteFrame = executeFrame;
            Lifetime = lifetime;
            Note = note;
            IsEnabled = isEnabled;
            IsLocked = isLocked;
            LoopTiming = loopTiming;
        }

        /// <summary>
        /// Creates a BlastUnit that uses a byte array value as the value
        /// </summary>
        /// <param name="value">The value of the BlastUnit</param>
        /// <param name="domain">The domain the blastunit lies in</param>
        /// <param name="address"></param>
        /// <param name="bigEndian"></param>
        /// <param name="executeFrame"></param>
        /// <param name="lifetime"></param>
        /// <param name="note"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isLocked"></param>
        public BlastUnit(byte[] value,
            string domain, long address, int precision, bool bigEndian, int executeFrame = 0, int lifetime = 1,
            string note = null, bool isEnabled = true, bool isLocked = false, bool generatedUsingValueList = false, int? loopTiming = null)
        {
            Source = BlastUnitSource.VALUE;
            //Precision has to be set before value
            Precision = precision;
            Value = value;

            Domain = domain;
            Address = address;
            ExecuteFrame = executeFrame;
            Lifetime = lifetime;
            Note = note;
            IsEnabled = isEnabled;
            IsLocked = isLocked;
            GeneratedUsingValueList = generatedUsingValueList;
            BigEndian = bigEndian;
            LoopTiming = loopTiming;
        }

        public BlastUnit()
        {
        }

        /// <summary>
        /// Returns a blastunit that's a subunit of the current unit
        /// </summary>
        /// <param name="start">Where to start in the unit</param>
        /// <param name="end">Where to end (INCLUSIVE)</param>
        /// <returns></returns>
        public BlastUnit GetSubUnit(int start, int end)
        {
            BlastUnit bu = new BlastUnit()
            {
                Precision = end - start,
                Address = this.Address + start,
                Domain = this.Domain,
                SourceAddress = this.SourceAddress,
                SourceDomain = this.SourceDomain,
                Source = this.Source,
                ExecuteFrame = this.ExecuteFrame,
                Lifetime = this.Lifetime,
                LimiterTime = this.LimiterTime,
                Loop = this.Loop,
                InvertLimiter = this.InvertLimiter,
                StoreLimiterSource = this.StoreLimiterSource,
                GeneratedUsingValueList = this.GeneratedUsingValueList,
                BigEndian = this.BigEndian,
                IsLocked = this.IsLocked,
                Note = this.Note,
                StoreTime = this.StoreTime,
                StoreType = this.StoreType,
                IsEnabled = this.IsEnabled,
                LimiterListHash = this.LimiterListHash,
                LoopTiming = this.LoopTiming,
            };

            if (bu.Source == BlastUnitSource.STORE)
            {
                bu.SourceAddress += start;

                if (BigEndian && start == (precision - 1))
                {
                    bu.TiltValue = TiltValue;
                }
                else if (!BigEndian && start == 0)
                {
                    bu.TiltValue = TiltValue;
                }
                else
                {
                    bu.TiltValue = 0;
                }
            }
            else
            {
                bu.Value = new byte[bu.precision];
                for (int i = 0; i < bu.precision; i++)
                {
                    if (BigEndian)
                    {
                        bu.Value[i] = Value[end - (i + 1)];
                    }
                    else
                    {
                        bu.Value[i] = Value[start + i];
                    }

                    //If we have a tilt, calculate it and bake it into the value
                    if (this.TiltValue != 0)
                    {
                        unchecked
                        {
                            if (BigEndian)
                            {
                                bu.Value[i] += (TiltValue.ToByteArray().PadLeft(this.precision))[end - (i + 1)];
                            }
                            else
                            {
                                bu.Value[i] += (TiltValue.ToByteArray().PadLeft(this.precision).FlipBytes())[start + i];
                            }
                        }
                    }
                    else
                    {
                        bu.TiltValue = 0;
                    }
                }
            }

            return bu;
        }

        /// <summary>
        /// Rasterizes VMDs to their underlying domain
        /// This returns a blastunit[] because if we have a non-contiguous vmd, we need to return multiple units
        /// </summary>
        public List<BlastUnit> GetRasterizedUnits(string vmdToRasterize = null)
        {
            if (vmdToRasterize == null)
            {
                vmdToRasterize = "[V]";
            }

            bool breakDown = false;
            BlastLayer l = new BlastLayer();
            //Todo - Change this to a more unique marker than [V]?
            if (Domain.Contains(vmdToRasterize))
            {
                string domain = (string)Domain.Clone();
                long address = Address;

                if (MemoryDomains.VmdPool[domain] is VirtualMemoryDomain vmd)
                {
                    long lastAddress = vmd.GetRealAddress(address);
                    string lastDomain = vmd.GetRealDomain(address);
                    for (int i = 1; i < this.Precision; i++)
                    {
                        var a = vmd.GetRealAddress(address + i);
                        var d = vmd.GetRealDomain(address + i);
                        if (a != lastAddress + 1 || d != lastDomain)
                        {
                            breakDown = true;
                            break;
                        }

                        lastAddress = a;
                        lastDomain = d;
                    }
                    if (!breakDown)
                    {
                        Domain = vmd.GetRealDomain(address);
                        Address = vmd.GetRealAddress(address);
                    }
                }
                else
                {
                    Domain = "ERROR";
                    Address = -1;
                }
            }
            if (SourceDomain?.Contains(vmdToRasterize) ?? false)
            {
                string sourceDomain = (string)SourceDomain.Clone();
                long sourceAddress = SourceAddress;

                if (MemoryDomains.VmdPool[sourceDomain] is VirtualMemoryDomain vmd)
                {
                    long lastAddress = vmd.GetRealAddress(sourceAddress);
                    string lastDomain = vmd.GetRealDomain(sourceAddress);
                    for (int i = 1; i < this.Precision; i++)
                    {
                        var a = vmd.GetRealAddress(sourceAddress + i);
                        var d = vmd.GetRealDomain(sourceAddress + i);
                        if (a != lastAddress + 1 || d != lastDomain)
                        {
                            breakDown = true;
                            break;
                        }
                        lastAddress = a;
                        lastDomain = d;
                    }

                    if (!breakDown)
                    {
                        SourceDomain = vmd.GetRealDomain(sourceAddress);
                        SourceAddress = vmd.GetRealAddress(sourceAddress);
                    }
                }
                else
                {
                    Domain = "ERROR";
                    Address = -1;
                }
            }

            if (breakDown)
            {
                for (int i = 0; i < this.Precision; i++)
                {
                    var bu = this.GetSubUnit(i, i + 1);
                    l.Layer.Add(bu);
                }
                l.RasterizeVMDs(); //recursively do this
            }
            else
            {
                l.Layer.Add(this);
            }

            return l.Layer;
        }

        /// <summary>
        /// Adds a blastunit to the execution pool
        /// </summary>
        /// <returns></returns>
        public bool Apply(bool dontFilter, bool overrideExecuteFrame = false)
        {
            if (!IsEnabled)
            {
                return true;
            }
            //Create our working data object
            this.Working = new BlastUnitWorkingData();

            //We need to grab the value to freeze
            if (Source == BlastUnitSource.STORE && StoreTime == StoreTime.IMMEDIATE)
            {
                //If it's one time, store the backup. Otherwise add it to the backup pool
                if (StoreType == StoreType.ONCE)
                {
                    StoreBackup();
                }
                else
                {
                    StepActions.StoreDataPool.Add(this);
                }
            }
            //Add it to the execution pool
            StepActions.AddBlastUnit(this, overrideExecuteFrame);

            if (dontFilter)
            {
                return true;
            }

            StepActions.FilterBuListCollection();

            return true;
        }

        /// <summary>
        /// Executes (applies) a blastunit. This shouldn't be called manually.
        /// If you want to execute a blastunit, add it to the execution pool using Apply()
        /// Returns false
        /// </summary>
        public ExecuteState Execute(bool UseRealtime = true)
        {
            if (!IsEnabled)
            {
                return ExecuteState.NOTEXECUTED;
            }

            try
            {
                //Get our memory interface
                MemoryInterface mi = MemoryDomains.GetInterface(Domain);
                if (mi == null)
                {
                    return ExecuteState.NOTEXECUTED;
                }

                //Limiter handling
                if (LimiterListHash != null && LimiterTime == LimiterTime.EXECUTE)
                {
                    if (!LimiterCheck(mi))
                    {
                        return ExecuteState.SILENTERROR;
                    }
                }

                if (Working == null)
                {
                    if (Debugger.IsAttached)
                    {
                        throw new Exception("wtf");
                    }

                    RTCV.Common.Logging.GlobalLogger.Error("Blastunit: WORKING WAS NULL {this}", this);
                    return ExecuteState.SILENTERROR;
                }
                switch (Source)
                {
                    case (BlastUnitSource.STORE):
                        {
                            if (Working.StoreData == null)
                            {
                                RTCV.Common.Logging.GlobalLogger.Error("Blastunit: STOREDATA WAS NULL {this}", this);
                                return ExecuteState.SILENTERROR;
                            }

                            //If there's no stored data, return out.
                            if (Working.StoreData.Count == 0)
                            {
                                return ExecuteState.NOTEXECUTED;
                            }

                            //Apply the value we have stored
                            Working.ApplyValue = Working.StoreData.Peek();

                            //Remove it from the store pool if it's a continuous backup
                            if (StoreType == StoreType.CONTINUOUS)
                            {
                                Working.StoreData.Dequeue();
                            }

                            //All the data is already handled by GetStoreBackup, so we can just poke
                            for (int i = 0; i < Precision; i++)
                            {
                                mi.PokeByte(Address + i, Working.ApplyValue[i]);
                            }
                            break;
                        }
                    case (BlastUnitSource.VALUE):
                        {
                            //We only calculate it once for Value and then store it in ApplyValue.
                            //If the length has changed (blast editor) we gotta recalc it
                            if (Working.ApplyValue == null)
                            {
                                //We don't want to modify the original array
                                Working.ApplyValue = (byte[])Value.Clone();

                                //Calculate the actual value to apply
                                CorruptCore_Extensions.AddValueToByteArrayUnchecked(ref Working.ApplyValue, TiltValue, false); //We don't use the endianess toggle here as we always store value units as little endian

                                //Flip it if it's big endian
                                if (this.BigEndian)
                                {
                                    Working.ApplyValue.FlipBytes();
                                }
                            }

                            //Poke the memory
                            for (int i = 0; i < Precision; i++)
                            {
                                mi.PokeByte(Address + i, Working.ApplyValue[i]);
                            }
                            break;
                        }
                }
            }
            catch (IOException)
            {
                var dr = MessageBox.Show(
                    "An IOException occured during Execute().\nThis probably means whatever is being corrupted can't be accessed.\nIf you're corrupting a file, close any program that might be using it.\n\nAborting corrupt.\nSend this error to the devs?",
                    "IOException during Execute()", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    throw;
                }
                return ExecuteState.HANDLEDERROR;
            }
            return ExecuteState.EXECUTED;
        }

        /// <summary>
        /// Adds a backup to the end of the StoreData queue
        /// </summary>
        public void StoreBackup()
        {
            if (SourceDomain == null)
            {
                return;
            }

            //Snag our memory interface
            MemoryInterface mi = MemoryDomains.GetInterface(SourceDomain);

            if (mi == null)
            {
                throw new Exception(
                    $"Memory Domain error. Mi was null. If you know how to reproduce this, let the devs know");
            }

            //Get the value
            byte[] value = new byte[Precision];
            for (int i = 0; i < Precision; i++)
            {
                value[i] = mi.PeekByte(SourceAddress + i);
            }

            //Calculate the final value after adding the tilt value
            if (TiltValue != 0)
            {
                CorruptCore_Extensions.AddValueToByteArrayUnchecked(ref value, TiltValue, this.BigEndian);
            }

            //Enqueue it
            Working.StoreData.Enqueue(value);
        }

        /// <summary>
        /// Returns a unit baked to VALUE with a lifetime of 1
        /// </summary>
        /// <returns></returns>
        public BlastUnit GetBakedUnit()
        {
            if (!IsEnabled)
            {
                return null;
            }

            //Grab our mi
            MemoryInterface mi = MemoryDomains.GetInterface(Domain);
            if (mi == null)
            {
                return null;
            }

            //Grab the value
            byte[] _value = new byte[Precision];
            for (int i = 0; i < Precision; i++)
            {
                _value[i] = mi.PeekByte(Address + i);
            }
            //Return a new unit
            //Note the false on bigEndian. That's because when reading from memory we're always reading from left to right and we don't want to flip the bytes twice
            return new BlastUnit(_value, Domain, Address, Precision, false, 0, 1, Note, IsEnabled, IsLocked);
        }

        private bool ReturnFalseAndDequeueIfContinuousStore()
        {
            if (this.Source == BlastUnitSource.STORE && this.StoreType == StoreType.CONTINUOUS && this.LimiterTime != LimiterTime.GENERATE)
            {
                if (this.Working.StoreData.Count > 0)
                {
                    this.Working.StoreData.Dequeue();
                }
            }

            return false;
        }

        public bool LimiterCheck(MemoryInterface destMI)
        {
            if (Source == BlastUnitSource.STORE)
            {
                if (StoreLimiterSource == StoreLimiterSource.ADDRESS || StoreLimiterSource == StoreLimiterSource.BOTH)
                {
                    if (Filtering.LimiterPeekBytes(Address,
                        Address + Precision, Domain, LimiterListHash, destMI))
                    {
                        if (InvertLimiter)
                        {
                            return ReturnFalseAndDequeueIfContinuousStore();
                        }

                        return true;
                    }
                }
                if (StoreLimiterSource == StoreLimiterSource.SOURCEADDRESS || StoreLimiterSource == StoreLimiterSource.BOTH)
                {
                    //We need an MI for the source domain. We pass a normal one around and pull this when needed
                    MemoryInterface sourceMI = MemoryDomains.GetInterface(SourceDomain);
                    if (sourceMI == null)
                    {
                        return false;
                    }

                    if (Filtering.LimiterPeekBytes(SourceAddress,
                        SourceAddress + Precision, Domain, LimiterListHash, sourceMI))
                    {
                        if (InvertLimiter)
                        {
                            return ReturnFalseAndDequeueIfContinuousStore();
                        }

                        return true;
                    }
                }
            }
            else
            {
                if (Filtering.LimiterPeekBytes(Address,
                    Address + Precision, Domain, LimiterListHash, destMI))
                {
                    if (InvertLimiter)
                    {
                        return ReturnFalseAndDequeueIfContinuousStore();
                    }

                    return true;
                }
            }
            //Note the flipped logic here
            if (InvertLimiter)
            {
                return true;
            }

            return ReturnFalseAndDequeueIfContinuousStore();
        }

        public BlastUnit GetBackup()
        {
            //TODO
            //There's a todo here but I didn't leave a note please help someone tell me why there's a todo here oh god I'm the only one working on this code
            return GetBakedUnit();
        }

        /// <summary>
        /// Rerolls a blastunit and generates new values based on various params
        /// </summary>
        public void Reroll()
        {
            //Don't reroll locked units
            if (this.IsLocked)
            {
                return;
            }

            if (Source == BlastUnitSource.VALUE)
            {
                if (RtcCore.RerollFollowsCustomEngine)
                {
                    if (this.GeneratedUsingValueList && !RtcCore.RerollIgnoresOriginalSource)
                    {
                        Value = Filtering.GetRandomConstant(RTC_CustomEngine.ValueListHash, Precision);
                    }
                    else
                    {
                        if (RTC_CustomEngine.ValueSource == CustomValueSource.VALUELIST)
                        {
                            Value = Filtering.GetRandomConstant(RTC_CustomEngine.ValueListHash, Precision);
                            return;
                        }

                        //Generate a random value based on our precision.
                        //We use a BigInteger as we support arbitrary length, but we do use built in methods for 8,16,32 bit for performance reasons
                        BigInteger randomValue = 0;
                        if (RTC_CustomEngine.ValueSource == CustomValueSource.RANGE)
                        {
                            switch (Precision)
                            {
                                case (1):
                                    randomValue = RtcCore.RND.NextULong(RTC_CustomEngine.MinValue8Bit, RTC_CustomEngine.MaxValue8Bit, true);
                                    break;
                                case (2):
                                    randomValue = RtcCore.RND.NextULong(RTC_CustomEngine.MinValue16Bit, RTC_CustomEngine.MaxValue16Bit, true);
                                    break;
                                case (4):
                                    randomValue = RtcCore.RND.NextULong(RTC_CustomEngine.MinValue32Bit, RTC_CustomEngine.MaxValue32Bit, true);
                                    break;
                                case (8):
                                    randomValue = RtcCore.RND.NextULong(RTC_CustomEngine.MinValue64Bit, RTC_CustomEngine.MaxValue64Bit, true);
                                    break;
                                //No limits if out of normal range
                                default:
                                    byte[] _randomValue = new byte[Precision];
                                    RtcCore.RND.NextBytes(_randomValue);
                                    randomValue = new BigInteger(_randomValue);
                                    break;
                            }
                        }
                        else if (RTC_CustomEngine.ValueSource == CustomValueSource.RANDOM)
                        {
                            switch (this.Precision)
                            {
                                case (1):
                                    randomValue = RtcCore.RND.NextULong(0, 0xFF, true);
                                    break;
                                case (2):
                                    randomValue = RtcCore.RND.NextULong(0, 0xFFFF, true);
                                    break;
                                case (4):
                                    randomValue = RtcCore.RND.NextULong(0, 0xFFFFFFFF, true);
                                    break;
                                case (8):
                                    randomValue = RtcCore.RND.NextULong(0, 0xFFFFFFFFFFFFFFFF, true);
                                    break;
                                //No limits if out of normal range
                                default:
                                    byte[] _randomValue = new byte[Precision];
                                    RtcCore.RND.NextBytes(_randomValue);
                                    randomValue = new BigInteger(_randomValue);
                                    break;
                            }
                        }
                        byte[] temp = new byte[Precision];
                        //We use this as it properly handles the length for us
                        CorruptCore_Extensions.AddValueToByteArrayUnchecked(ref temp, randomValue, false);
                        Value = temp;
                    }
                }
                else
                {
                    if (this.GeneratedUsingValueList && !RtcCore.RerollIgnoresOriginalSource)
                    {
                        Value = Filtering.GetRandomConstant(RTC_VectorEngine.ValueListHash, Precision);
                    }
                    else
                    {
                        //Generate a random value based on our precision.
                        //We use a BigInteger as we support arbitrary length, but we do use built in methods for 8,16,32 bit for performance reasons
                        BigInteger randomValue;
                        switch (Precision)
                        {
                            case (1):
                                randomValue = RtcCore.RND.NextULong(0, 0xFF, true);
                                break;
                            case (2):
                                randomValue = RtcCore.RND.NextULong(0, 0xFFFF, true);
                                break;
                            case (4):
                                randomValue = RtcCore.RND.NextULong(0, 0xFFFFFFFF, true);
                                break;
                            case (8):
                                randomValue = RtcCore.RND.NextULong(0, 0xFFFFFFFFFFFFFFFF, true);
                                break;
                            //No limits if out of normal range
                            default:
                                byte[] _randomValue = new byte[Precision];
                                RtcCore.RND.NextBytes(_randomValue);
                                randomValue = new BigInteger(_randomValue);
                                break;
                        }

                        byte[] temp = new byte[Precision];
                        //We use this as it properly handles the length for us
                        CorruptCore_Extensions.AddValueToByteArrayUnchecked(ref temp, randomValue, false);
                        Value = temp;
                    }
                }
            }
            else if (Source == BlastUnitSource.STORE)
            {
                string[] _selectedDomains = (string[])RTCV.NetCore.AllSpec.UISpec["SELECTEDDOMAINS"];

                //Always reroll domain before address
                if (RtcCore.RerollSourceDomain)
                {
                    SourceDomain = _selectedDomains[RtcCore.RND.Next(_selectedDomains.Length)];
                }
                if (RtcCore.RerollSourceAddress)
                {
                    long maxAddress = MemoryDomains.GetInterface(SourceDomain)?.Size ?? 1;
                    SourceAddress = RtcCore.RND.NextLong(0, maxAddress - 1);
                }

                if (RtcCore.RerollDomain)
                {
                    Domain = _selectedDomains[RtcCore.RND.Next(_selectedDomains.Length)];
                }
                if (RtcCore.RerollAddress)
                {
                    long maxAddress = MemoryDomains.GetInterface(Domain)?.Size ?? 1;
                    Address = RtcCore.RND.NextLong(0, maxAddress - 1);
                }
            }
        }

        public override string ToString()
        {
            string enabledString = "[ ] BlastUnit -> ";
            if (IsEnabled)
            {
                enabledString = "[x] BlastUnit -> ";
            }

            string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
            return (enabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString("X") + ")." + Source.ToString() + "(" + ValueString + ")");
        }

        /// <summary>
        /// Called when a unit is moved from the queue into the execution pool
        /// </summary>
        /// <returns></returns>
        public bool EnteringExecution()
        {
            //Snag our MI
            MemoryInterface mi = MemoryDomains.GetInterface(Domain);
            if (mi == null)
            {
                return false;
            }

            //If it's a store unit, store the backup
            if (Source == BlastUnitSource.STORE && StoreTime == StoreTime.PREEXECUTE)
            {
                //One off store vs execution pool
                if (StoreType == StoreType.ONCE)
                {
                    StoreBackup();
                }
                else
                {
                    StepActions.StoreDataPool.Add(this);
                }
            }
            //Limiter handling. Normal operation is to not do anything if it doesn't match the limiter. Inverted is to only continue if it doesn't match
            if (LimiterTime == LimiterTime.PREEXECUTE)
            {
                if (!LimiterCheck(mi))
                {
                    return false;
                }
            }

            return true;
        }

        public BlastUnit[] GetBreakdown()
        {
            BlastUnit[] brokenUnits = new BlastUnit[precision];

            if (precision == 1)
            {
                brokenUnits[0] = this;
                return brokenUnits;
            }

            for (int i = 0; i < this.Precision; i++)
            {
                var bu = this.GetSubUnit(i, i + 1);
                brokenUnits[i] = bu;
            }

            return brokenUnits;
        }
    }

    [Serializable]
    public class ActiveTableObject
    {
        public long[] Data { get; set; }

        public ActiveTableObject()
        {
        }

        public ActiveTableObject(long[] data)
        {
            Data = data;
        }
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.AllPublic)]
    public class BlastGeneratorProto : INote
    {
        public string BlastType { get; set; }
        public string Domain { get; set; }
        public int Precision { get; set; }
        public long StepSize { get; set; }
        public long StartAddress { get; set; }
        public long EndAddress { get; set; }
        public ulong Param1 { get; set; }
        public ulong Param2 { get; set; }
        public string Mode { get; set; }
        public string Note { get; set; }
        public int Lifetime { get; set; }
        public int ExecuteFrame { get; set; }
        public bool Loop { get; set; }
        public int Seed { get; set; }
        public BlastLayer bl { get; set; }

        public BlastGeneratorProto()
        {
        }

        public BlastGeneratorProto(string _note, string _blastType, string _domain, string _mode, int _precision, long _stepSize, long _startAddress, long _endAddress, ulong _param1, ulong _param2, int lifetime, int executeframe, bool loop, int _seed)
        {
            Note = _note;
            BlastType = _blastType;
            Domain = _domain;
            Precision = _precision;
            StartAddress = _startAddress;
            EndAddress = _endAddress;
            Param1 = _param1;
            Param2 = _param2;
            Mode = _mode;
            StepSize = _stepSize;
            Lifetime = lifetime;
            ExecuteFrame = executeframe;
            Loop = loop;
            Seed = _seed;
        }

        public BlastLayer GenerateBlastLayer()
        {
            switch (BlastType)
            {
                case "Value":
                    bl = RTC_ValueGenerator.GenerateLayer(Note, Domain, StepSize, StartAddress, EndAddress, Param1, Param2, Precision, Lifetime, ExecuteFrame, Loop, Seed, (BGValueMode)Enum.Parse(typeof(BGValueMode), Mode, true));
                    break;
                case "Store":
                    bl = RTC_StoreGenerator.GenerateLayer(Note, Domain, StepSize, StartAddress, EndAddress, Param1, Param2, Precision, Lifetime, ExecuteFrame, Loop, Seed, (BGStoreMode)Enum.Parse(typeof(BGStoreMode), Mode, true));
                    break;
                default:
                    return null;
            }

            return bl;
        }
    }

    public class ProblematicProcess
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }

    public interface INote
    {
        string Note { get; set; }
    }

    /// <summary>
    /// A generic object for combobox purposes.
    /// Has a name and a value of type T for storing any object.
    /// </summary>
    /// <typeparam name="T">The type of object you want the comboxbox value to be</typeparam>
    public class ComboBoxItem<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }

        public ComboBoxItem(string name, T value)
        {
            Name = name;
            Value = value;
        }

        public ComboBoxItem()
        {
        }
    }

    public delegate void ProgressBarEventHandler(object source, ProgressBarEventArgs e);

    public class ProgressBarEventArgs : EventArgs
    {
        public string CurrentTask;
        public decimal Progress;

        public ProgressBarEventArgs(string text, decimal progress)
        {
            CurrentTask = text;
            Progress = progress;

            RTCV.Common.Logging.GlobalLogger.Log(NLog.LogLevel.Info, $"ProgressBarEventArgs: {text}");
        }
    }
}
