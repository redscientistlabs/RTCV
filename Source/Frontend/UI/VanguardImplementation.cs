namespace RTCV.UI
{
    using Newtonsoft.Json;
    using RTCV.Common;
    using RTCV.CorruptCore;
    using RTCV.CorruptCore.Extensions;
    using RTCV.NetCore;
    using RTCV.NetCore.Commands;
    using RTCV.UI.Components.Controls;
    using RTCV.UI.Modular;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using System.Windows.Forms;
    using static RTCV.CorruptCore.Stockpile;

    public static class VanguardImplementation
    {
        internal static UIConnector connector = null;
        private static string lastVanguardClient = "";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static bool isSwapping = false;
        public static CanvasForm windowSelect = null;

        public static void StartServer()
        {
            logger.Trace("Starting UI Vanguard Implementation");

            var spec = new NetCoreReceiver();
            spec.MessageReceived += OnMessageReceived;

            spec.Attached = RtcCore.Attached;

            connector = new UIConnector(spec);
        }

        public static void RestartServer()
        {
            logger.Info("Restarting NetCore");
            connector?.Restart();
        }

        public static void Shutdown()
        {
            logger.Info("Shutting down Netcore");
            connector?.Kill();
        }

        private static List<string> GetCueTracks(string cueFile)
        {
            string cueFolder = Path.GetDirectoryName(cueFile);
            string[] cueLines = File.ReadAllLines(cueFile);
            List<string> binFiles = new List<string>();

            binFiles.Add(Path.GetFileName(cueFile));

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
                    }
                    else
                    {
                        //Just copy the old cue into the new one
                        startFilename = cueLines[i].IndexOf('"') + 1;
                    }

                    binFiles.Add(cueLines[i].Substring(startFilename, endFilename - startFilename));
                }
            }

            return binFiles.Select(file => Path.Combine(cueFolder, file)).ToList();
        }

        private static List<string> GetGdiTracks(string gdiFile)
        {
            string gdiFolder = Path.GetDirectoryName(gdiFile);
            string[] gdiLines = File.ReadAllLines(gdiFile);
            List<string> binFiles = new List<string>();

            binFiles.Add(Path.GetFileName(gdiFile));

            for (int i = 0; i < gdiLines.Length; i++)
            {
                if (gdiLines[i].Contains(".bin"))
                {
                    int startFilename;
                    int endFilename = gdiLines[i].LastIndexOf('"');

                    //Just copy the old gdi into the new one
                    startFilename = gdiLines[i].IndexOf('"') + 1;

                    binFiles.Add(gdiLines[i].Substring(startFilename, endFilename - startFilename));
                }
            }
            return binFiles.Select(file => Path.Combine(gdiFolder, file)).ToList();
        }

        private static List<string> GetCcdTracks(string ccdFile)
        {
            List<string> binFiles = new List<string>();

            binFiles.Add(Path.GetFileName(ccdFile));

            if (File.Exists(Path.GetFileNameWithoutExtension(ccdFile) + ".sub"))
            {
                binFiles.Add(Path.GetFileNameWithoutExtension(ccdFile) + ".sub");
            }

            if (File.Exists(Path.GetFileNameWithoutExtension(ccdFile) + ".img"))
            {
                binFiles.Add(Path.GetFileNameWithoutExtension(ccdFile) + ".img");
            }

            return binFiles;
        }

        // Custom Crc32 hash generator since we don't have access to System.IO.Hashing
        private static class Crc32
        {
            private const uint Polynomial = 0xEDB88320;
            private static readonly uint[] Table;

            static Crc32()
            {
                Table = new uint[256];
                for (uint i = 0; i < 256; i++)
                {
                    uint entry = i;
                    for (int j = 0; j < 8; j++)
                        entry = (entry & 1) == 1 ? (entry >> 1) ^ Polynomial : entry >> 1;
                    Table[i] = entry;
                }
            }

            public static uint Calculate(uint currentCrc, byte[] buffer, int offset, int count)
            {
                uint crc = ~currentCrc;
                for (int i = offset; i < offset + count; i++)
                {
                    crc = (crc >> 8) ^ Table[(crc ^ buffer[i]) & 0xFF];
                }
                return ~crc;
            }
        }

        private static string lastLoadedGameName = "";
        // Checks to see if we need to generate metadata for a game that's been opened for the first time
        public static void GenerateHashes(object sender, EventArgs e)
        {
            string romFilePath = AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME]?.ToString();
            string gameName = AllSpec.VanguardSpec[VSPEC.GAMENAME]?.ToString();
            string ext = Path.GetExtension(romFilePath);

            bool sameGameName = string.Equals(gameName, lastLoadedGameName);
            lastLoadedGameName = gameName;
            if (sameGameName)
                return;

            if (String.IsNullOrEmpty(romFilePath) || String.IsNullOrEmpty(gameName))
                return;

            if (!File.Exists(romFilePath))
                return;

            List<string> romFilePaths = new List<string>();
            if (romFilePath.IndexOf(".CUE", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                romFilePaths.AddRange(GetCueTracks(romFilePath));
            }
            else if (romFilePath.IndexOf(".GDI", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                romFilePaths.AddRange(GetGdiTracks(romFilePath));
            }
            else if (romFilePath.IndexOf(".CCD", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                romFilePaths.AddRange(GetCcdTracks(romFilePath));
            }
            else
            {
                romFilePaths.Add(romFilePath);
            }

            string multiFileFolder = Path.GetFileNameWithoutExtension(romFilePath);
            string finalDir = "";
            if (romFilePaths.Count > 1)
            {
                finalDir = Path.Combine(RtcCore.RtcDir, "ROMHASHES", multiFileFolder);
                Directory.CreateDirectory(finalDir);
            }
            else
            {
                finalDir = Path.Combine(RtcCore.RtcDir, "ROMHASHES");
            }

            Dictionary<string, string> requireHashFiles = new Dictionary<string, string>();
            Dictionary<string, RomMetadata> requireComparison = new Dictionary<string, RomMetadata>();
            foreach (string filePath in romFilePaths)
            {
                string metadataFilepath = Path.Combine(finalDir, $"{gameName}{ext}.metadata");
                if (File.Exists(metadataFilepath))
                {
                    var metadata = JsonHelper.Deserialize<RomMetadata>(File.ReadAllText(metadataFilepath));
                    if (metadata != null)
                    {
                        if (!Stockpile.runtimeMetadata.Exists(m => m.Name == gameName))
                            Stockpile.runtimeMetadata.Add(metadata);
                        requireComparison[filePath] = metadata;
                    }
                }
                else
                {
                    requireHashFiles[filePath] = gameName;
                    // Create the metadata file right away so that additional spec updates don't begin new threads
                    File.Create(metadataFilepath).Dispose();
                }
            }

            foreach (RomMetadata metadata in requireComparison.Values)
            {
                CompareHashes(metadata);
            }

            if (requireHashFiles.Count == 0)
                return;

            int toastID = lastToastID;
            if (StockpileManagerUISide.totalHashFilesInQueue == 0)
            {
                var stockpileForm = S.GET<StockpileManagerForm>();
                Toast toast = Toast.GetInstance();
                toastID = toast.AddToastEntry("Generating hashes...");
                lastToastID = toastID;

                var openForms = Application.OpenForms.Cast<Form>();
                var activeForm = openForms.Where(form => form is CanvasForm && form.Visible).First() as CanvasForm;
                activeForm?.ShowToast(toast);
            }

            Interlocked.Add(ref StockpileManagerUISide.totalHashFilesInQueue, requireHashFiles.Count);
            hashGenerationQueue.Post((requireHashFiles, finalDir, toastID));
        }


        private static int lastToastID = -1;
        public static ActionBlock<(Dictionary<string, string>, string, int)> hashGenerationQueue = new ActionBlock<(Dictionary<string, string>, string, int)>(async data =>
        {
            await ProcessFileHashes(data.Item1, data.Item2, data.Item3);

            if (hashGenerationQueue.InputCount == 0)
            {
                StockpileManagerUISide.waitingForHashes = false;
            }
        });

        private static async Task ProcessFileHashes(Dictionary<string, string> files, string dir, int toastID)
        {
            var currentToastID = toastID;
            var toast = Toast.GetInstance();
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            await Task.Run(async () =>
            {
                var stockpileForm = S.GET<StockpileManagerForm>();
                foreach (string filePath in files.Keys)
                {
                    string gameName = files[filePath];
                    string ext = Path.GetExtension(filePath);

                    RomMetadata metadata = new RomMetadata();
                    metadata.Name = gameName;
                    metadata.Size = new FileInfo(filePath).Length;

                    using (SHA256 sHA256 = SHA256.Create())
                    {
                        byte[] input = Encoding.UTF8.GetBytes(metadata.Name + metadata.Size);
                        byte[] hash = sHA256.ComputeHash(input);

                        StringBuilder builder = new StringBuilder();
                        for (int k = 0; k < hash.Length; k++)
                        {
                            builder.Append(hash[k].ToString("x2"));
                        }

                        metadata.Checksum = builder.ToString();
                    }

                    logger.Trace(metadata.Name);


                    // Open the file
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 2 * 1024 * 1024, useAsync: true))
                    {
                        long totalBytes = fs.Length;
                        byte[] buffer = new byte[2 * 1024 * 1024];
                        int bytesRead;
                        long totalBytesRead = 0;
                        long totalBytesReadLast = 0;
                        int progressLast = 0;

                        uint crc = 0;
                        using (SHA1 sha1 = SHA1.Create())
                        using (MD5 md5 = MD5.Create())
                        {
                            try
                            {
                                // Instead of using ComputeHash(), we can read the bytes with ReadAsync() and then use TransformBlock(). This lets us generate
                                // all hashes at the same time, instead of having to restart the filestream after each
                                while ((bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length, cts.Token)) > 0)
                                {
                                    crc = Crc32.Calculate(crc, buffer, 0, bytesRead);
                                    sha1.TransformBlock(buffer, 0, bytesRead, null, 0);
                                    md5.TransformBlock(buffer, 0, bytesRead, null, 0);

                                    totalBytesRead += bytesRead;

                                    int progress = (int)((decimal)totalBytesRead / totalBytes * 100);

                                    if (progressLast != progress)
                                    {
                                        if (StockpileManagerUISide.waitingForHashes)
                                            currentToastID = -1;

                                        RtcCore.OnProgressBarUpdate(null, new ProgressBarEventArgs($"Generating hashes...({StockpileManagerUISide.totalHashFilesInQueue} files left)", progress, currentToastID));
                                    }

                                    if (totalBytesReadLast != totalBytesRead)
                                    {
                                        cts.CancelAfter(5000);
                                    }
                                    else
                                    {
                                        cts.Token.ThrowIfCancellationRequested();
                                    }

                                    totalBytesReadLast = totalBytesRead;
                                    progressLast = progress;
                                }

                                sha1.TransformFinalBlock(buffer, 0, 0);
                                md5.TransformFinalBlock(buffer, 0, 0);

                                string crc32HashString = crc.ToString("x8");
                                string sha1HashString = sha1.Hash.BytesToHexString().ToLowerInvariant();
                                string md5HashString = md5.Hash.BytesToHexString().ToLowerInvariant();

                                metadata.Crc32 = crc32HashString;
                                metadata.Sha1 = sha1HashString;
                                metadata.Md5 = md5HashString;

                                using (FileStream fs2 = File.Open(
                                    Path.Combine(dir, $"{gameName}{ext}.metadata"),
                                                FileMode.OpenOrCreate))
                                {
                                    JsonHelper.Serialize(metadata, fs2, Formatting.Indented);
                                }
                            }
                            catch (OperationCanceledException)
                            {
                                MessageBox.Show("Failed to generate metadata! This should never happen." +
                                    " \n\nPoke the RTC devs for help (Discord is in the launcher).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error,
                                    MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                                File.Delete(Path.Combine(dir, $"{metadata.Name}.metadata"));
                            }
                            finally
                            {
                                Interlocked.Decrement(ref StockpileManagerUISide.totalHashFilesInQueue);
                                string fileName = Path.GetFileNameWithoutExtension(filePath);
                                Stockpile.runtimeMetadata.Add(metadata);
                                CompareHashes(metadata);
                            }
                        }
                    }
                }
            });

            // Close the toast on the UI thread
            if (StockpileManagerUISide.totalHashFilesInQueue == 0)
            {
                toast.RemoveToastEntry(toastID);
            }
            logger.Trace($"Finished generating hashes");
        }

        private static void CompareHashes(RomMetadata metadata)
        {
            if (Stockpile.disableMetadataCheck)
                return;

            foreach (RomMetadata stockpileMetadata in Stockpile.stockpileMetadata)
            {
                if (stockpileMetadata.Name != metadata.Name)
                    continue;

                if (stockpileMetadata.Crc32 != metadata.Crc32 ||
                    stockpileMetadata.Sha1  != metadata.Sha1  ||
                    stockpileMetadata.Md5   != metadata.Md5)
                {
                    DialogResult disableMismatchMessage = MessageBox.Show($"the metadata for this game does not match the stockpile's version. " +
                        $"This is most likely caused by a bad dump or a different file extension. " +
                        $"Entries may not work as intended.\n" +
                        $"If you believe you have a good dump and have verified the entries work correctly, " +
                        $"you can save the stockpile to overwrite it with your metadata.\n\n" +
                        $"This message will not show again until this game is reopened. If you would like to " +
                        $"temporarily disable all similar messages for this stockpile, click yes.", "Metadata Mismatch",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, 
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                    if (disableMismatchMessage == DialogResult.Yes)
                    {
                        Stockpile.disableMetadataCheck = true;
                    }
                }
            }
        }

        private static async void OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            try
            {
                var message = e.message;
                var advancedMessage = message as NetCoreAdvancedMessage;

                switch (message.Type) //Handle received messages here
                {
                    case Remote.PushVanguardSpec:
                        PushVanguardSpec(advancedMessage, ref e);
                        break;
                    case Remote.AllSpecSent:
                        AllSpecSent();
                        if (isSwapping)
                            StockpileManagerUISide.finishedSwapping.TrySetResult(true);
                        break;
                    case Remote.PushVanguardSpecUpdate:
                        PushVanguardSpecUpdate(advancedMessage, ref e);
                        break;
                    case Remote.PushCorruptCoreSpecUpdate:
                        PushCorruptCoreSpecUpdate(advancedMessage, ref e);
                        break;
                    case Remote.GenerateVMDText:
                        GenerateVmdText(advancedMessage, ref e);
                        break;
                    case Remote.EventDomainsUpdated:
                        var eventArgs = (object[])(advancedMessage.objectValue as object[]);
                        var domainsChanged = (bool)eventArgs[0];
                        DomainsUpdated(domainsChanged);
                        break;
                    case Remote.GetBlastGeneratorLayer:
                        GetBlastGeneratorLayer(ref e);
                        break;
                    case Basic.ErrorDisableAutoCorrupt:
                        DisableAutoCorrupt();
                        break;
                    case Remote.RenderDisplay:
                        RenderDisplay();
                        break;
                    case Remote.BackupKeyStash:
                        BackupKeyStash(advancedMessage);
                        break;
                    case Basic.KillswitchPulse:
                        KillSwitchPulse();
                        break;
                    case Basic.ResetGameProtectionIfRunning:
                        ResetGameProtectionIfRunning();
                        break;
                    case Remote.DisableSavestateSupport:
                        DisableSavestateSupport();
                        break;

                    case Remote.DisableGameProtectionSupport:
                        DisableGameProtectionSupport();
                        break;

                    case Remote.DisableRealtimeSupport:
                        DisableRealTimeSupport();
                        break;
                    case Remote.DisableKillSwitchSupport:
                        DisableKillSwitchSupport();
                        break;
                    case Remote.DomainVMDAdd:
                        AddVMD(advancedMessage.objectValue as VmdPrototype);
                        break;
                    case Remote.BlastEditorStartSanitizeTool:
                        StartSanitizeTool();
                        break;

                    case Remote.BlastEditorLoadCorrupt:
                        LoadCorrupt();
                        break;

                    case Remote.BlastEditorLoadOriginal:
                        LoadOriginal();
                        break;

                    case Remote.BlastEditorGetLayerSizeUnlockedUnits:
                        GetLayerSizeUnlockedUnits(ref e);
                        break;

                    case Remote.BlastEditorGetLayerSize:
                        GetLayerSize(ref e);
                        break;

                    case Remote.SanitizeToolStartSanitizing:
                        StartSanitizing();
                        break;

                    case Remote.SanitizeToolLeaveWithChanges:
                        LeaveWithChanges();
                        break;

                    case Remote.SanitizeToolLeaveSubtractChanges:
                        LeaveSubtractChanges();
                        break;

                    case Remote.SanitizeToolYesEffect:
                        YesEffect();
                        break;

                    case Remote.SanitizeToolNoEffect:
                        NoEffect();
                        break;

                    case Remote.SanitizeToolReroll:
                        Reroll();
                        break;
                    case Remote.TriggerHotkey:
                        {
                            string hotkey = (advancedMessage.objectValue as string);
                            UICore.CheckHotkey(hotkey);
                        }
                        break;
                    case Remote.SwapImplementation:
                        {
                            var args = (object[])(advancedMessage.objectValue as object[]);
                            var newEmu = (string)args[0];
                            var unlockAfterSwap = false;
                            if (args.Length > 1)
                                unlockAfterSwap = (bool)args[1];

                            bool result = await SwapImplementation(newEmu, unlockAfterSwap);

                            e.setReturnValue(result);
                        }
                        break;
                    case Remote.UnlockInterface:
                        {
                            if (windowSelect != null)
                                SyncObjectSingleton.FormExecute(() => { windowSelect.CloseSubForm(); });

                            logger.Trace("Unlocking UI");
                            SyncObjectSingleton.FormExecute(() => { UICore.UnlockInterface(); });
                            logger.Trace("UI Unlocked");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                if (CloudDebug.ShowErrorDialog(ex) == DialogResult.Abort)
                {
                    throw new AbortEverythingException();
                }

                return;
            }
        }

        private static void SwapTimeout(CanvasForm form)
        {
            Task.Run(() => MessageBox.Show($"Failed to swap emulators. Please save your work, then close and reopen RTC. If you are able to reproduce this issue," +
                $"poke the RTC devs for help (Discord is in the launcher).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly));

            SyncObjectSingleton.FormExecute(() => { form.CloseSubForm(); });
            logger.Trace("Unlocking Interface");
            SyncObjectSingleton.FormExecute(() => { UICore.UnlockInterface(); });
            logger.Trace("Load cancelled");

            AutoKillSwitch.Enabled = true;
            VanguardImplementation.isSwapping = false;
            return;
        }
        
        public static async Task<bool> SwapImplementation(string newEmu, bool unlockAfterSwap = false)
        {
            // Get the currently active form for displaying the loading bar. If there isn't any (a.k.a we're loading an implementation from the launcher),
            // then find the first visible CanvasForm and display it on that instead.
            var openForms = Application.OpenForms.Cast<Form>();
            var activeForm = openForms.Where(form => form == Form.ActiveForm).FirstOrDefault() as CanvasForm;
            windowSelect = activeForm ?? openForms.Where(form => form is CanvasForm && form.Visible).First() as CanvasForm;

            Task completedTask = null;
            CancellationTokenSource cts = new CancellationTokenSource();

            Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(StockpileManagerUISide.timeout), cts.Token);

            isSwapping = true;
            StockpileManagerUISide.finishedClosing = new TaskCompletionSource<bool>(false);
            StockpileManagerUISide.finishedSwapping = new TaskCompletionSource<bool>(false);
            
            logger.Trace("different emulator found, switching");

            AutoKillSwitch.Enabled = false;

            // If we were focused on anything other than the core form (aka we're swapping emulators from the launcher), don't focus it
            logger.Trace("Blocking UI");
            var stayFocusedForms = new List<string> { "Glitch Harvester", "Blast Editor", "Blast Generator" };
            bool focusCoreForm = stayFocusedForms.Contains(windowSelect.Text, StringComparer.OrdinalIgnoreCase) ? false : true;
            SyncObjectSingleton.FormExecute(() => { UICore.LockInterface(focusCoreForm, true); });
            logger.Trace("UI Blocked");

            string oldEmuDir = CorruptCore.RtcCore.EmuDir;
            var newEmuDir = Path.Combine(Path.Combine(new DirectoryInfo(RtcCore.RtcDir).Parent.Parent.FullName, newEmu));
            CorruptCore.RtcCore.EmuDir = newEmuDir;

            // Load the progress form
            S.GET<SaveProgressForm>().Dock = DockStyle.Fill;
            SyncObjectSingleton.FormExecute(() => { windowSelect.OpenSubForm(S.GET<SaveProgressForm>()); });
            RtcCore.OnProgressBarUpdate(null, new ProgressBarEventArgs($"Switching from " + new DirectoryInfo(oldEmuDir).Name +
                                            " to " + newEmu, 0));

            LocalNetCoreRouter.Route(NetCore.Endpoints.Vanguard, NetCore.Commands.Remote.EventCloseEmulator);

            // Wait until the UI thread has confirmed the emulator has finished closing
            // This will be when RTC has lost the TCP connection with the emulator
            completedTask = await Task.WhenAny(StockpileManagerUISide.finishedClosing.Task, timeoutTask).ConfigureAwait(false);
            if (completedTask == timeoutTask && !completedTask.IsCanceled)
            {
                SwapTimeout(windowSelect);
                return false;
            }

            // Open the new emulator
            var info = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                WorkingDirectory = newEmuDir,
                FileName = Path.Combine(newEmuDir, "RESTARTDETACHEDRTC.bat"),
            };

            // If we couldn't open the emulator for some reason, fall back to the old emulator
            if (!File.Exists(info.FileName))
            {
                MessageBox.Show($"Couldn't find {info.FileName}! Killswitch will not work.");

                SyncObjectSingleton.FormExecute(() => { windowSelect.CloseSubForm(); });
                logger.Trace("Unlocking Interface");
                SyncObjectSingleton.FormExecute(UICore.UnlockInterface);
                logger.Trace("Load cancelled");

                CorruptCore.RtcCore.EmuDir = oldEmuDir;

                // Re-open the old emulator
                var infoOldEmu = new System.Diagnostics.ProcessStartInfo()
                {
                    UseShellExecute = false,
                    WorkingDirectory = oldEmuDir,
                    FileName = Path.Combine(oldEmuDir, "RESTARTDETACHEDRTC.bat"),
                };
                RtcCore.OnProgressBarUpdate(null, new ProgressBarEventArgs($"Swapping back to " + new DirectoryInfo(oldEmuDir).Name, 50));
                System.Diagnostics.Process.Start(infoOldEmu);

                AutoKillSwitch.Enabled = true;
                VanguardImplementation.isSwapping = false;

                return false;
            }

            logger.Trace("Starting the new process");
            RtcCore.OnProgressBarUpdate(null, new ProgressBarEventArgs($"Starting " + newEmu, 50));

            System.Diagnostics.Process.Start(info);


            // Wait until the UI thread has confirmed the emulator has finished opening
            // This will be once AllSpecSent() has finished
            completedTask = await Task.WhenAny(StockpileManagerUISide.finishedSwapping.Task, timeoutTask).ConfigureAwait(false);
            if (completedTask == timeoutTask && !completedTask.IsCanceled)
            {
                SwapTimeout(windowSelect);

                // Re-open the old emulator
                var infoOldEmu = new System.Diagnostics.ProcessStartInfo()
                {
                    UseShellExecute = false,
                    WorkingDirectory = oldEmuDir,
                    FileName = Path.Combine(oldEmuDir, "RESTARTDETACHEDRTC.bat"),
                };
                RtcCore.OnProgressBarUpdate(null, new ProgressBarEventArgs($"Swapping back to " + new DirectoryInfo(oldEmuDir).Name, 50));
                System.Diagnostics.Process.Start(infoOldEmu);


                return false;
            }

            // We need to make sure to send the name to the connection status and about form again since we couldn't get it before reconnecting
            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<ConnectionStatusForm>().lbConnectionStatus.Text =
                    $"Connected to {(string)AllSpec.VanguardSpec?[VSPEC.NAME] ?? "Vanguard"}";
                S.GET<SettingsAboutForm>().lbConnectedTo.Text =
                    $"Connected to: {(string)AllSpec.VanguardSpec?[VSPEC.NAME] ?? "Vanguard"}";

            });

            // Also make sure to update the swap emulator form
            S.GET<SwapEmulatorForm>().UpdateSelectedEmulator(newEmu);

            RtcCore.OnProgressBarUpdate(null, new ProgressBarEventArgs($"Loading game", 100));

            VanguardImplementation.isSwapping = false;
            cts.Cancel();

            if (unlockAfterSwap)
                LocalNetCoreRouter.Route(Endpoints.UI, NetCore.Commands.Remote.UnlockInterface, true);

            return true;
        }

        private static void PushVanguardSpec(NetCoreAdvancedMessage advancedMessage, ref NetCoreEventArgs e)
        {
            if (!RtcCore.Attached)
            {
                AllSpec.VanguardSpec = new FullSpec((PartialSpec)advancedMessage.objectValue, !RtcCore.Attached);
                AllSpec.VanguardSpec.SpecUpdated += new EventHandler<SpecUpdateEventArgs>(GenerateHashes);
            }

            e.setReturnValue(true);

            //Push the UI, Plugin and CorruptCore spec (since we're master)
            LocalNetCoreRouter.Route(Endpoints.CorruptCore, Remote.PushUISpec, AllSpec.UISpec.GetPartialSpec(), true);
            LocalNetCoreRouter.Route(Endpoints.CorruptCore, Remote.PushCorruptCoreSpec, AllSpec.CorruptCoreSpec.GetPartialSpec(), true);
            LocalNetCoreRouter.Route(Endpoints.CorruptCore, Remote.PushPluginSpec, AllSpec.PluginSpec.GetPartialSpec(), true);

            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<CoreForm>().pnAutoKillSwitch.Visible = true;
                S.GET<CoreForm>().pnCrashProtection.Visible = true;
            });
            //Specs are all set up so UI is clear.
            LocalNetCoreRouter.Route(Endpoints.Vanguard, Remote.AllSpecSent, true);
        }
        private static void AddVMD(VmdPrototype proto)
        {
            RTCV.CorruptCore.MemoryDomains.AddVMD(proto);
        }
        internal static void AllSpecSent()
        {
            if (UICore.FirstConnect)
            {
                UICore.Initialized.WaitOne(10000);
            }

            SyncObjectSingleton.FormExecute(() =>
            {
                var coreForm = S.GET<CoreForm>();
                if (UICore.FirstConnect)
                {
                    lastVanguardClient = (string)AllSpec.VanguardSpec?[VSPEC.NAME] ?? "VANGUARD";
                    UICore.FirstConnect = false;

                    //Load plugins on both sides
                    RtcCore.LoadPlugins();
                    LocalNetCoreRouter.Route(Endpoints.CorruptCore, Remote.LoadPlugins, true);

                    //Configure the UI based on the vanguard spec
                    UICore.ConfigureUIFromVanguardSpec();

                    coreForm.Show();

                    //Pull any lists from the vanguard implementation
                    List<string> dirs = new List<string>();
                    dirs.Add(RtcCore.ListsDir);
                    if (RtcCore.EmuDir != null)
                    {
                        dirs.Add(Path.Combine(RtcCore.EmuDir, "LISTS"));
                    }

                    UICore.LoadLists(dirs);

                    Panel sidebar = coreForm.pnSideBar;
                    foreach (Control c in sidebar.Controls)
                    {
                        if (c is Button b)
                        {
                            if (!b.Text.Contains("Test") && b.ForeColor != Color.OrangeRed)
                            {
                                b.Visible = true;
                            }
                        }
                    }

                    DefaultGrids.engineConfig.LoadToMain();

                    DefaultGrids.glitchHarvester.LoadToNewWindow("Glitch Harvester", true);
                }
                else if (VanguardImplementation.isSwapping)
                {
                    lastVanguardClient = (string)AllSpec.VanguardSpec?[VSPEC.NAME] ?? "VANGUARD";

                    //make sure the other side reloads the plugins
                    LocalNetCoreRouter.Route(Endpoints.CorruptCore, Remote.LoadPlugins, true);

                    //Configure the UI based on the vanguard spec
                    UICore.ConfigureUIFromVanguardSpec();

                    Panel sidebar = coreForm.pnSideBar;
                    foreach (Control c in sidebar.Controls)
                    {
                        if (c is Button b)
                        {
                            if (!b.Text.Contains("Test") && b.ForeColor != Color.OrangeRed)
                            {
                                b.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    LocalNetCoreRouter.Route(Endpoints.CorruptCore, Remote.LoadPlugins, true);
                    //make sure the other side reloads the plugins

                    var clientName = (string)AllSpec.VanguardSpec?[VSPEC.NAME] ?? "VANGUARD";
                    // Disabled with the addition of cross-emulator stockpiles
                    /*if (clientName != lastVanguardClient)
                    {
                        MessageBox.Show($"Error: Found {clientName} when previously connected to {lastVanguardClient}.\nPlease restart the RTC to swap clients.");
                        return;
                    }*/

                    //Push the VMDs since we store them out of spec
                    var vmdProtos = MemoryDomains.VmdPool.Values.Select(x => x.Proto).ToArray();
                    LocalNetCoreRouter.Route(Endpoints.CorruptCore, Remote.PushVMDProtos, vmdProtos, true);

                    coreForm.Show();

                    //Configure the UI based on the vanguard spec
                    UICore.ConfigureUIFromVanguardSpec();

                    //Unblock the controls in the GH
                    S.GET<GlitchHarvesterBlastForm>().SetBlastButtonVisibility(true);

                    //Return to the main form. If the form is null for some reason, default to engineconfig
                    coreForm.PreviousGrids[1] ??= DefaultGrids.engineConfig;

                    UICore.UnlockInterface();

                    switch (coreForm.ExternalIndex)
                    {
                        case 1:
                            coreForm.PreviousGrids[0].LoadToMain(true);
                            coreForm.PreviousGrids[1].LoadToNewWindow("External", false, true);
                            break;
                        case 0:
                            coreForm.PreviousGrids[0].LoadToNewWindow("External", false, true);
                            coreForm.PreviousGrids[1].LoadToMain(true);
                            break;
                        default:
                            coreForm.PreviousGrids[1].LoadToMain(true);
                            break;
                    }
                }

                coreForm.pbAutoKillSwitchTimeout.Value = 0; //remove this once core form is dead

                if (!RtcCore.Attached)
                {
                    //AutoKillSwitch.Enabled = true;
                }

                //Restart game protection
                if (coreForm.cbUseGameProtection.Checked)
                {
                    if (StockpileManagerUISide.BackupedState != null)
                    {
                        StockpileManagerUISide.BackupedState.Run();
                    }

                    if (StockpileManagerUISide.BackupedState != null)
                    {
                        S.GET<MemoryDomainsForm>().RefreshDomainsAndKeepSelected(StockpileManagerUISide.BackupedState.SelectedDomains.Distinct().ToArray());
                    }

                    GameProtection.Start();
                    if (GameProtection.WasAutoCorruptRunning)
                    {
                        coreForm.AutoCorrupt = true;
                    }
                }

                coreForm.Show();


                //UI LOAD FINISHED

                //Fetch Settings Form reference to make sure it is instancied on unlock
                //Forces-load submodules of the Settings form on the first boot
                //required for the infinite units maximum to get bound
                S.GET<SettingsForm>();
            });
        }

        private static void PushVanguardSpecUpdate(NetCoreAdvancedMessage advancedMessage, ref NetCoreEventArgs e)
        {
            SyncObjectSingleton.FormExecute(() =>
                        {
                            AllSpec.VanguardSpec?.Update((PartialSpec)advancedMessage.objectValue);
                        });
            e.setReturnValue(true);
        }

        //CorruptCore pushed its spec. Note the false on propogate (since we don't want a recursive loop)
        private static void PushCorruptCoreSpecUpdate(NetCoreAdvancedMessage advancedMessage, ref NetCoreEventArgs e)
        {
            SyncObjectSingleton.FormExecute(() =>
                        {
                            AllSpec.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
                        });
            e.setReturnValue(true);
        }

        private static void GenerateVmdText(NetCoreAdvancedMessage advancedMessage, ref NetCoreEventArgs e)
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                object[] objs = (object[])advancedMessage.objectValue;
                string domain = (string)objs[0];
                string text = (string)objs[1];

                var vmdgenerator = S.GET<VmdGenForm>();

                vmdgenerator.btnLoadDomains_Click(null, null);

                var cbitems = vmdgenerator.cbSelectedMemoryDomain.Items;
                object domainFound = null;
                for (int i = 0; i < cbitems.Count; i++)
                {
                    var item = cbitems[i];

                    if (item.ToString() == domain)
                    {
                        domainFound = item;
                        vmdgenerator.cbSelectedMemoryDomain.SelectedIndex = i;
                        break;
                    }
                }

                if (domainFound == null)
                {
                    throw new Exception($"Domain {domain} could not be selected in the VMD Generator. Aborting procedure.");
                                            //return;
                                        }

                vmdgenerator.tbCustomAddresses.Text = text;

                string value = "";

                if (Forms.InputBox.ShowDialog("VMD Generation", "Enter the new VMD name:", ref value) == DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        vmdgenerator.tbVmdName.Text = value.Trim();
                    }

                    vmdgenerator.GenerateVMD(null, null);
                }
            });
            e.setReturnValue(true);
        }

        private static void DomainsUpdated(bool domainsChanged = false)
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<MemoryDomainsForm>().RefreshDomains();

                string systemCore = AllSpec.VanguardSpec[VSPEC.SYSTEMCORE]?.ToString() ?? null;

                // If we have a game running, update the domain config
                if (!string.IsNullOrEmpty(systemCore) && !systemCore.Contains("ProcessStub") && !systemCore.Contains("FileStub"))
                {
                    string configFileName = new DirectoryInfo(CorruptCore.RtcCore.EmuDir).Name + "_DOMAINS_CONFIG";

                    // If we have a config file already, we only want to keep the settings if the domains have changed (i.e. we're loading them)
                    // or if it's another core that isn't currently being used (so we save it back to the json file)
                    DomainConfigRoot config = GetDomainConfigParam(configFileName, systemCore, domainsChanged);
                    DomainConfigRoot defaultConfig = GetDomainConfigParam("DEFAULT_" + configFileName, systemCore, true);
                    DomainConfigRoot vmdConfig = GetDomainConfigParam("VMD_DOMAINS_CONFIG", systemCore, true);

                    // If the vmd config is for a different core, throw it away
                    if (!vmdConfig.DomainConfigSystem.ContainsKey(systemCore))
                        Params.RemoveParam("VMD_DOMAINS_CONFIG");

                    // Grab the default blacklisted domains
                    string[] defaultBlacklistedDomains = (string[])AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS];

                    // Store the defaults if they haven't been yet
                    if (!Params.IsParamSet("DEFAULT_" + configFileName) || (!defaultConfig.DomainConfigSystem.ContainsKey(systemCore)))
                    {
                        DomainConfigSystem defaultConfigSystem = new DomainConfigSystem();
                        foreach (string domain in MemoryDomains.MemoryInterfaces.Keys)
                        {
                            bool isBlacklisted = defaultBlacklistedDomains.Contains(domain);
                            defaultConfigSystem.DomainConfig[domain] = new DomainConfig(true, !isBlacklisted);
                        }

                        defaultConfig.DomainConfigSystem[systemCore] = defaultConfigSystem;
                        string defaultJsonString = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented);
                        Params.SetParam("DEFAULT_" + configFileName, defaultJsonString);
                    }

                    DomainConfigSystem configSystem = new DomainConfigSystem();
                    foreach (KeyValuePair<string, MemoryDomainProxy> domain in MemoryDomains.MemoryInterfaces)
                    {
                        configSystem.DomainConfig[domain.Key] = new DomainConfig(domain.Value.Visible, domain.Value.AutoDomainSelect);
                    }

                    // If we don't have a config file yet or we're updating the domains from the settings form, save to the json config file
                    if (!Params.IsParamSet(configFileName) || (domainsChanged && !config.DomainConfigSystem.ContainsKey(systemCore)) || !domainsChanged)
                    {
                        config.DomainConfigSystem[systemCore] = config.DomainConfigSystem.ContainsKey(systemCore) ? configSystem : defaultConfig.DomainConfigSystem[systemCore];
                        string jsonString = JsonConvert.SerializeObject(config, Formatting.Indented);

                        Params.SetParam(configFileName, jsonString);
                    }

                    DomainConfigSystem vmdConfigSystem = new DomainConfigSystem();
                    foreach (KeyValuePair<string, VirtualMemoryDomain> vmd in MemoryDomains.VmdPool)
                    {
                        vmdConfigSystem.DomainConfig[vmd.Key] = new DomainConfig(vmd.Value.Visible, vmd.Value.AutoDomainSelect);
                    }
                    if ((!Params.IsParamSet("VMD_DOMAINS_CONFIG") && vmdConfigSystem.DomainConfig.Count > 0))
                    {
                        vmdConfig.DomainConfigSystem[systemCore] = vmdConfigSystem;
                        string jsonString = JsonConvert.SerializeObject(vmdConfig, Formatting.Indented);

                        Params.SetParam("VMD_DOMAINS_CONFIG", jsonString);
                    }

                    // If the domains changed, update them with the latest settings from the config file then refresh the domains
                    if (domainsChanged)
                    {
                        foreach (KeyValuePair<string, MemoryDomainProxy> domain in MemoryDomains.MemoryInterfaces)
                        {
                            domain.Value.Visible = config.DomainConfigSystem[systemCore].DomainConfig[domain.Key].VISIBLE;
                            domain.Value.AutoDomainSelect = config.DomainConfigSystem[systemCore].DomainConfig[domain.Key].AUTOSELECT;
                        }
                    }

                    var blacklistedDomains = MemoryDomains.MemoryInterfaces.Keys.Where(key => MemoryDomains.MemoryInterfaces[key].AutoDomainSelect == false);
                    blacklistedDomains = blacklistedDomains.Concat(MemoryDomains.VmdPool.Keys.Where(key => MemoryDomains.VmdPool[key].AutoDomainSelect == false));
                    AllSpec.VanguardSpec.Update(VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS, blacklistedDomains.ToArray());

                    S.GET<MemoryDomainsForm>().RefreshDomains();

                    if (S.GET<DomainSelectionConfigForm>().Visible)
                        S.GET<DomainSelectionConfigForm>().UpdateDomainsList();
                }
                S.GET<MemoryDomainsForm>().SetMemoryDomainsAllButSelectedDomains(AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] as string[] ?? new string[] { });
            });
        }

        private static DomainConfigRoot GetDomainConfigParam(string configFileName, string systemCore, bool domainsChanged)
        {
            var config = new DomainConfigRoot();
            if (Params.IsParamSet(configFileName))
            {
                var configFile = File.ReadAllText(Path.Combine(Params.ParamsDir, configFileName));
                var jsonString = JsonConvert.DeserializeObject<DomainConfigRoot>(configFile);

                foreach (string system in jsonString.DomainConfigSystem.Keys)
                {
                    config.DomainConfigSystem[system] = new DomainConfigSystem();
                    config.DomainConfigSystem[system].DomainConfig = jsonString.DomainConfigSystem[system].DomainConfig;
                }
            }
            return config;
        }

        private static void GetBlastGeneratorLayer(ref NetCoreEventArgs e)
        {
            BlastLayer bl = null;
            SyncObjectSingleton.FormExecute(() =>
            {
                bl = S.GET<BlastGeneratorForm>().GenerateBlastLayers(true, true, false);
            });
            e.setReturnValue(bl);
        }

        private static void DisableAutoCorrupt()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<CoreForm>().AutoCorrupt = false;
            });
        }

        private static void RenderDisplay()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<GlitchHarvesterBlastForm>().refreshRenderOutputButton();
            });
        }

        private static void BackupKeyStash(NetCoreAdvancedMessage advancedMessage)
        {
            if (advancedMessage?.objectValue is StashKey sk)
            {
                StockpileManagerUISide.BackupedState = sk;
                GameProtection.AddBackupState(sk);
                SyncObjectSingleton.FormExecute(() =>
                {
                    S.GET<CoreForm>().btnGpJumpBack.Visible = true;
                    S.GET<CoreForm>().btnGpJumpNow.Visible = true;
                });
            }
        }

        private static void ResetGameProtectionIfRunning()
        {
            if (GameProtection.isRunning)
            {
                SyncObjectSingleton.FormExecute(() =>
                {
                    S.GET<CoreForm>().cbUseGameProtection.Checked = false;
                    S.GET<CoreForm>().cbUseGameProtection.Checked = true;
                });
            }
        }

        private static void DisableSavestateSupport()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<SavestateManagerForm>().DisableFeature();
                S.GET<CoreForm>().pnCrashProtection.Visible = false;
            });
        }

        private static void DisableGameProtectionSupport()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<CoreForm>().pnCrashProtection.Visible = false;
            });
        }

        private static void DisableRealTimeSupport()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                Button btnManual = S.GET<CoreForm>().btnManualBlast;
                if (AllSpec.VanguardSpec[VSPEC.REPLACE_MANUALBLAST_WITH_GHCORRUPT] != null)
                {
                    btnManual.Text = "  Corrupt";
                }
                else
                {
                    btnManual.Visible = false;
                }

                S.GET<CoreForm>().btnAutoCorrupt.Enabled = false;
                S.GET<CoreForm>().btnAutoCorrupt.Visible = false;
                S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Enabled = false;
                S.GET<GlitchHarvesterBlastForm>().btnSendRaw.Enabled = false;
                S.GET<GlitchHarvesterBlastForm>().btnBlastToggle.Enabled = false;

                S.GET<CorruptionEngineForm>().cbSelectedEngine.Items.Remove("Distortion Engine");
                S.GET<CorruptionEngineForm>().cbSelectedEngine.Items.Remove("Pipe Engine");
                S.GET<CorruptionEngineForm>().cbSelectedEngine.Items.Remove("Freeze Engine");
            });
        }

        private static void DisableKillSwitchSupport()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<CoreForm>().pnAutoKillSwitch.Visible = false;
                S.GET<CoreForm>().cbUseAutoKillSwitch.Checked = false;
                AutoKillSwitch.Enabled = false;
            });
        }

        private static void StartSanitizeTool()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var blastEditor = S.GET<BlastEditorForm>();
                blastEditor.OpenSanitizeTool(false);
            });
        }

        private static void LoadCorrupt()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var blastEditor = S.GET<BlastEditorForm>();
                blastEditor.LoadCorrupt(null, null);
            });
        }

        private static void LoadOriginal()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var blastEditor = S.GET<BlastEditorForm>();
                blastEditor.LoadOriginal();
            });
        }

        private static void GetLayerSizeUnlockedUnits(ref NetCoreEventArgs e)
        {
            var units = 0;
            SyncObjectSingleton.FormExecute(() =>
            {   // this is what the sanitize tool uses to judge how many units there are left to sanitize.
                var blastEditor = S.GET<BlastEditorForm>();
                units = blastEditor.currentSK?.BlastLayer?.Layer.Count(x => !x.IsLocked) ?? -1;
            });

            e.setReturnValue(units);
        }

        private static void GetLayerSize(ref NetCoreEventArgs e)
        {
            var layerSize = 0;
            SyncObjectSingleton.FormExecute(() =>
            {
                layerSize = S.GET<BlastEditorForm>().currentSK?.BlastLayer?.Layer?.Count ?? -1;
            });

            e.setReturnValue(layerSize);
        }

        private static void StartSanitizing()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var sanitizeTool = S.GET<SanitizeToolForm>();
                sanitizeTool.StartSanitizing(null, null);
            });
        }

        private static void LeaveWithChanges()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var sanitizeTool = S.GET<SanitizeToolForm>();
                sanitizeTool.lbSteps.Items.Clear(); //this is a hack for leaving in automation
                sanitizeTool.LeaveAndKeepChanges(null, null);
            });
        }

        private static void LeaveSubtractChanges()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var sanitizeTool = S.GET<SanitizeToolForm>();
                sanitizeTool.lbSteps.Items.Clear(); //this is a hack for leaving in automation
                sanitizeTool.LeaveAndSubtractChanges(null, null);
            });
        }

        private static void YesEffect()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var sanitizeTool = S.GET<SanitizeToolForm>();
                sanitizeTool.YesEffect(null, null);
            });
        }

        private static void NoEffect()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var sanitizeTool = S.GET<SanitizeToolForm>();
                sanitizeTool.NoEffect(null, null);
            });
        }

        private static void Reroll()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var sanitizeTool = S.GET<SanitizeToolForm>();
                sanitizeTool.Reroll(null, null);
            });
        }

        private static void KillSwitchPulse()
        {
            AutoKillSwitch.Pulse();
        }
    }
}
