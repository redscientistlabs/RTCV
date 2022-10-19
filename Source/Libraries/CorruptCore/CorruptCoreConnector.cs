namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using RTCV.NetCore;
    using RTCV.NetCore.Commands;

    public class CorruptCoreConnector : IRoutable
    {
        private static volatile object loadLock = new object();
        private static object LoadLock => loadLock;

        public object OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            try
            { //Use setReturnValue to handle returns
                var message = e.message;
                var advancedMessage = message as NetCoreAdvancedMessage;

                switch (e.message.Type)
                {
                    case "GETSPECDUMPS":
                        GetSpecDumps(ref e);
                        break;
                    //UI sent its spec
                    case Remote.PushUISpec:
                        {
                            SyncObjectSingleton.FormExecute(() => AllSpec.UISpec = new FullSpec((PartialSpec)advancedMessage.objectValue, !RtcCore.Attached));
                            break;
                        }

                    //UI sent a spec update
                    case Remote.PushUISpecUpdate:
                        SyncObjectSingleton.FormExecute(() => AllSpec.UISpec?.Update((PartialSpec)advancedMessage.objectValue));
                        break;

                    //Vanguard sent a copy of its spec
                    case Remote.PushVanguardSpec:

                        SyncObjectSingleton.FormExecute(() =>
                        {
                            if (!RtcCore.Attached)
                            {
                                AllSpec.VanguardSpec = new FullSpec((PartialSpec)advancedMessage.objectValue, !RtcCore.Attached);
                            }
                        });
                        break;

                    //Vanguard sent a spec update
                    case Remote.PushVanguardSpecUpdate:
                        AllSpec.VanguardSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
                        break;

                    //UI sent a copy of the CorruptCore spec
                    case Remote.PushCorruptCoreSpec:
                        PushCorruptCoreSpec((PartialSpec)advancedMessage.objectValue, ref e);
                        break;

                    //UI sent an update of the CorruptCore spec
                    case Remote.PushCorruptCoreSpecUpdate:
                        SyncObjectSingleton.FormExecute(() => AllSpec.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false));
                        break;


                    //UI sent a copy of the Plugin spec
                    case Remote.PushPluginSpec:
                        PushPluginSpec((PartialSpec)advancedMessage.objectValue, ref e);
                        break;

                    //UI sent an update of the CorruptCore spec
                    case Remote.PushPluginSpecUpdate:
                        SyncObjectSingleton.FormExecute(() => AllSpec.PluginSpec?.Update((PartialSpec)advancedMessage.objectValue, false));
                        break;

                    case Remote.EventDomainsUpdated:
                        var domainsChanged = (bool)advancedMessage.objectValue;
                        MemoryDomains.RefreshDomains(domainsChanged);
                        break;

                    case Remote.EventRestrictFeatures:
                        RestrictFeatures();
                        break;

                    case Remote.EventShutdown:
                        RtcCore.Shutdown();
                        break;

                    case Remote.OpenHexEditor:
                        OpenHexEditor();
                        break;

                    case Emulator.OpenHexEditorAddress:
                        OpenHexEditorAddress(advancedMessage.objectValue);
                        break;

                    case Basic.ManualBlast:
                        RtcCore.GenerateAndBlast();
                        break;

                    case Basic.GenerateBlastLayer:
                        GenerateBlastLayer(advancedMessage, ref e);
                        break;

                    case Basic.ApplyBlastLayer:
                        ApplyBlastLayer(advancedMessage);
                        break;

                    case Remote.PushRTCSpec:
                        AllSpec.CorruptCoreSpec = new FullSpec((PartialSpec)advancedMessage.objectValue, !RtcCore.Attached);
                        e.setReturnValue(true);
                        break;

                    case Remote.UpdatedSelectedPluginEngine:
                        CorruptCore.RtcCore.SelectedPluginEngine = (ICorruptionEngine)advancedMessage.objectValue;
                        e.setReturnValue(true);
                        break;

                    case Remote.PushRTCSpecUpdate:
                        AllSpec.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
                        break;

                    case Basic.BlastGeneratorBlast:
                        {
                            var valueAsObjectArr = advancedMessage.objectValue as object[];
                            BlastGeneratorBlast(valueAsObjectArr, ref e);
                        }
                        break;

                    case Remote.LoadState:
                        {
                            var valueAsObjectArr = advancedMessage.objectValue as object[];
                            LoadState(valueAsObjectArr, ref e);
                        }
                        break;
                    case Remote.SaveState:
                        {
                            StashKey sk = null;
                            void a()
                            {
                                sk = StockpileManagerEmuSide.SaveStateNET(advancedMessage.objectValue as StashKey); //Has to be nullable cast
                            }
                            SyncObjectSingleton.EmuThreadExecute(a, false);
                            e.setReturnValue(sk);
                        }
                        break;
                    case Remote.SaveStateless:
                        {
                            StashKey sk = null;
                            void a()
                            {
                                sk = StockpileManagerEmuSide.SaveStateLessNet(advancedMessage.objectValue as StashKey); //Has to be nullable cast
                            }
                            SyncObjectSingleton.EmuThreadExecute(a, false);
                            e.setReturnValue(sk);
                        }
                        break;

                    case Remote.BackupKeyRequest:
                        {
                            //We don't store this in the spec as it'd be horrible to push it to the UI and it doesn't care
                            //if (!LocalNetCoreRouter.QueryRoute<bool>(NetCore.Endpoints.Vanguard, NetcoreCommands.REMOTE_ISNORMALADVANCE))
                            //break;

                            StashKey sk = null;
                            //We send an unsynced command back
                            SyncObjectSingleton.FormExecute(() => sk = StockpileManagerEmuSide.SaveStateNET());

                            if (sk != null)
                            {
                                LocalNetCoreRouter.Route(Endpoints.UI, Remote.BackupKeyStash, sk, false);
                            }

                            break;
                        }
                    case Remote.DomainGetDomains:
                        e.setReturnValue(LocalNetCoreRouter.Route(Endpoints.Vanguard, Remote.DomainGetDomains, true));
                        break;
                    case Remote.PushVMDProtos:
                        MemoryDomains.VmdPool.Clear();
                        foreach (var proto in (advancedMessage.objectValue as VmdPrototype[]))
                        {
                            MemoryDomains.AddVMD(proto);
                        }

                        break;

                    case Remote.DomainPeekByte:
                        {
                            //ObjectValue -­> Object[] -­> string DomainName, long Address
                            //returns: byte Value

                            var obj = advancedMessage.objectValue as object[];
                            string DomainName = (string)obj[0];
                            long Address = (long)obj[1];

                            var MD = MemoryDomains.GetInterface(DomainName);
                            e.setReturnValue(MD.PeekByte(Address));
                            break;
                        }
                    case Remote.DomainPokeByte:
                        {
                            //ObjectValue -­> Object[] -­> string DomainName, long Address, byte Value
                            //no return

                            var obj = advancedMessage.objectValue as object[];
                            string DomainName = (string)obj[0];
                            long Address = (long)obj[1];
                            byte Value = (byte)obj[2];

                            var MD = MemoryDomains.GetInterface(DomainName);
                            MD.PokeByte(Address, Value);
                            break;
                        }
                    case Remote.DomainPeekBytes:
                        {
                            //ObjectValue -­> Object[] -­> string DomainName, long StartAddress, long EndAddress, bool raw
                            //returns: byte[] Value

                            var obj = advancedMessage.objectValue as object[];
                            string DomainName = (string)obj[0];
                            long StartAddress = (long)obj[1];
                            long EndAddress = (long)obj[2];
                            bool raw = (bool)obj[3];

                            var MD = MemoryDomains.GetInterface(DomainName);
                            e.setReturnValue(MD.PeekBytes(StartAddress, EndAddress, raw));
                            break;
                        }
                    case Remote.DomainPokeBytes:
                        {
                            //ObjectValue -­> Object[] -­> string DomainName, long Address, byte[] Value, bool raw
                            //no return

                            var obj = advancedMessage.objectValue as object[];
                            string DomainName = (string)obj[0];
                            long Address = (long)obj[1];
                            byte[] Value = (byte[])obj[2];
                            bool raw = (bool)obj[3];

                            var MD = MemoryDomains.GetInterface(DomainName);
                            MD.PokeBytes(Address, Value, true);
                            break;
                        }

                    case Remote.RerollBlastLayer:
                        {
                            //ObjectValue -­> Object -­> BlastLayer bl
                            //returns BlastLayer

                            var bl = (BlastLayer)advancedMessage.objectValue;
                            bl.Reroll();
                            e.setReturnValue(bl);
                            break;
                        }


                    case Remote.DomainVMDAdd:
                        MemoryDomains.AddVMDFromRemote((advancedMessage.objectValue as VmdPrototype));
                        break;

                    case Remote.DomainVMDRemove:
                        {
                            StepActions.ClearStepBlastUnits();
                            MemoryDomains.RemoveVMDFromRemote((advancedMessage.objectValue as string));
                        }
                        break;

                    case Remote.DomainActiveTableMakeDump:
                        {
                            void a()
                            {
                                MemoryDomains.GenerateActiveTableDump(
                                    (string)(advancedMessage.objectValue as object[])[0],
                                    (string)(advancedMessage.objectValue as object[])[1]);
                            }

                            SyncObjectSingleton.EmuThreadExecute(a, false);
                        }
                        break;

                    case Remote.BlastToolsGetAppliedBackupLayer:
                        {
                            var bl = (BlastLayer)(advancedMessage.objectValue as object[])[0];
                            var sk = (StashKey)(advancedMessage.objectValue as object[])[1];

                            void a()
                            {
                                e.setReturnValue(BlastTools.GetAppliedBackupLayer(bl, sk));
                            }

                            SyncObjectSingleton.EmuThreadExecute(a, false);
                            break;
                        }

                    case Remote.LongArrayFilterDomain:
                        {
                            var objValues = (advancedMessage.objectValue as object[]);
                            FilterDomain(objValues, ref e);
                        }
                        break;

                    case Remote.KeyGetRawBlastLayer:
                        {
                            void a()
                            { e.setReturnValue(StockpileManagerEmuSide.GetRawBlastlayer()); }
                            SyncObjectSingleton.EmuThreadExecute(a, false);
                        }
                        break;

                    case Remote.BLGetDiffBlastLayer:
                        {
                            var filename = advancedMessage.objectValue as string;
                            void a()
                            { e.setReturnValue(BlastDiff.GetBlastLayer(filename)); }
                            SyncObjectSingleton.EmuThreadExecute(a, false);
                        }
                        break;

                    case Remote.ClearBlastlayerCache:
                        {
                            void a()
                            {
                                StockpileManagerEmuSide.UnCorruptBL = null;
                                StockpileManagerEmuSide.CorruptBL = null;
                            }
                            SyncObjectSingleton.EmuThreadExecute(a, false);
                        }
                        break;

                    case Remote.SetApplyUncorruptBL:
                        {
                            void a()
                            {
                                StockpileManagerEmuSide.UnCorruptBL?.Apply(true);
                            }
                            SyncObjectSingleton.EmuThreadExecute(a, false);
                        }
                        break;

                    case Remote.SetApplyCorruptBL:
                        {
                            void a()
                            {
                                var autoUncorrupt = RtcCore.AutoUncorrupt;
                                if (autoUncorrupt && RtcCore.prevAutoUncorruptBlastLayer != null)
                                    RtcCore.prevAutoUncorruptBlastLayer.Apply(false);

                                StockpileManagerEmuSide.CorruptBL?.Apply(autoUncorrupt);

                                if (autoUncorrupt)
                                    RtcCore.prevAutoUncorruptBlastLayer = StockpileManagerEmuSide.CorruptBL?.GetBackup();

                            }
                            SyncObjectSingleton.EmuThreadExecute(a, false);
                        }
                        break;

                    case Remote.ClearStepBlastUnits:
                        SyncObjectSingleton.FormExecute(() => StepActions.ClearStepBlastUnits());
                        break;

                    case Remote.LoadPlugins:
                        LoadPlugins();
                        break;
                    case Remote.RemoveExcessInfiniteStepUnits:
                        SyncObjectSingleton.FormExecute(() => StepActions.RemoveExcessInfiniteStepUnits());
                        break;

                    default:
                        new object();
                        break;
                }

                return e.returnMessage;
            }
            catch (Exception ex)
            {
                if (CloudDebug.ShowErrorDialog(ex, true) == DialogResult.Abort)
                {
                    throw new AbortEverythingException();
                }

                return e.returnMessage;
            }
        }

        private static void GetSpecDumps(ref NetCoreEventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Spec Dump from CorruptCore");
            sb.AppendLine();
            sb.AppendLine("UISpec");
            AllSpec.UISpec?.GetDump().ForEach(x => sb.AppendLine(x));
            sb.AppendLine("CorruptCoreSpec");
            AllSpec.CorruptCoreSpec?.GetDump().ForEach(x => sb.AppendLine(x));
            sb.AppendLine("VanguardSpec");
            AllSpec.VanguardSpec?.GetDump().ForEach(x => sb.AppendLine(x));
            e.setReturnValue(sb.ToString());
        }

        private static void PushCorruptCoreSpec(PartialSpec partialSpec, ref NetCoreEventArgs e)
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                //So here's the deal. The UI doesn't actually have the full memory domains (md isn't sent across) so if we take them from it, it results in them going null
                //Instead, we stick with what we have, then tell the UI to use that.

                var temp = new FullSpec(partialSpec, !RtcCore.Attached);

                //Stick with what we have if it exists to prevent any exceptions if autocorrupt was on or something, then call refresh
                temp.Update("MEMORYINTERFACES", AllSpec.CorruptCoreSpec?["MEMORYINTERFACES"] ?? new Dictionary<string, MemoryDomainProxy>());

                AllSpec.CorruptCoreSpec = new FullSpec(temp.GetPartialSpec(), !RtcCore.Attached);
                AllSpec.CorruptCoreSpec.SpecUpdated += (ob, eas) =>
                {
                    PartialSpec partial = eas.partialSpec;
                    LocalNetCoreRouter.Route(Endpoints.Default, Remote.PushCorruptCoreSpecUpdate, partial, true);
                };
                MemoryDomains.RefreshDomains();
            });
            e.setReturnValue(true);
        }

        private static void PushPluginSpec(PartialSpec partialSpec, ref NetCoreEventArgs e)
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                AllSpec.PluginSpec = new FullSpec(partialSpec, !RtcCore.Attached);
                AllSpec.PluginSpec.SpecUpdated += (ob, eas) =>
                {
                    PartialSpec partial = eas.partialSpec;
                    LocalNetCoreRouter.Route(Endpoints.Default, Remote.PushPluginSpecUpdate, partial, true);
                };
            });
            e.setReturnValue(true);
        }

        private static void RestrictFeatures()
        {
            if (!AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_SAVESTATES) ?? true)
            {
                LocalNetCoreRouter.Route(Endpoints.UI, Remote.DisableSavestateSupport);
            }

            if (!AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true)
            {
                LocalNetCoreRouter.Route(Endpoints.UI, Remote.DisableRealtimeSupport);
            }

            if (!AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_KILLSWITCH) ?? true)
            {
                LocalNetCoreRouter.Route(Endpoints.UI, Remote.DisableKillSwitchSupport);
            }

            if (!AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_GAMEPROTECTION) ?? true)
            {
                LocalNetCoreRouter.Route(Endpoints.UI, Remote.DisableGameProtectionSupport);
            }
        }

        private static void LoadState(object[] valueAsObjectArr, ref NetCoreEventArgs e)
        {
            lock (LoadLock)
            {
                var sk = (StashKey)valueAsObjectArr[0];
                var reloadRom = (bool)valueAsObjectArr[1];
                var runBlastLayer = (bool)valueAsObjectArr[2];

                var returnValue = false;

                //Load the game from the main thread
                if (reloadRom)
                {
                    SyncObjectSingleton.FormExecute(() => StockpileManagerEmuSide.LoadRomNet(sk));
                }
                void a()
                {
                    returnValue = StockpileManagerEmuSide.LoadStateNet(sk, runBlastLayer);
                }
                //If the emulator uses callbacks, we do everything on the main thread and once we're done, we unpause emulation
                if ((bool?)AllSpec.VanguardSpec[VSPEC.LOADSTATE_USES_CALLBACKS] ?? false)
                {
                    SyncObjectSingleton.FormExecute(a);
                    e.setReturnValue(LocalNetCoreRouter.Route(Endpoints.Vanguard, Remote.ResumeEmulation, true));
                }
                else //We're loading on the emulator thread which'll block
                {
                    SyncObjectSingleton.EmuThreadExecute(a, false);
                }
                e.setReturnValue(returnValue);
            }
        }

        private static void OpenHexEditor()
        {
            if ((bool?)AllSpec.VanguardSpec[VSPEC.USE_INTEGRATED_HEXEDITOR] ?? false)
            {
                LocalNetCoreRouter.Route(Endpoints.Vanguard, Remote.OpenHexEditor, true);
            }
            else
            {
                //Route it to the plugin if loaded
                if (RtcCore.PluginHost.LoadedPlugins.Any(x => x.Name == "Hex Editor"))
                {
                    LocalNetCoreRouter.Route("HEXEDITOR", Remote.OpenHexEditor, true);
                }
                else
                {
                    MessageBox.Show("The current Vanguard implementation does not include a\n hex editor & the hex editor plugin isn't loaded. Aborting.");
                }
            }
        }

        private static void OpenHexEditorAddress(object objectValue)
        {
            if ((bool?)AllSpec.VanguardSpec[VSPEC.USE_INTEGRATED_HEXEDITOR] ?? false)
            {
                LocalNetCoreRouter.Route(Endpoints.Vanguard, Emulator.OpenHexEditorAddress, objectValue, true);
            }
            else
            {
                //Route it to the plugin if loaded
                if (RtcCore.PluginHost.LoadedPlugins.Any(x => x.Name == "Hex Editor"))
                {
                    LocalNetCoreRouter.Route("HEXEDITOR", Emulator.OpenHexEditorAddress, objectValue, true);
                }
                else
                {
                    MessageBox.Show("The current Vanguard implementation does not include a\n hex editor & the hex editor plugin isn't loaded. Aborting.");
                }
            }
        }

        private static void BlastGeneratorBlast(object[] valueAsObjectArr, ref NetCoreEventArgs e)
        {
            List<BlastGeneratorProto> returnList = null;
            var sk = (StashKey)valueAsObjectArr[0];
            var blastGeneratorProtos = (List<BlastGeneratorProto>)valueAsObjectArr[1];
            var loadBeforeCorrupt = (bool)valueAsObjectArr[2];
            var applyAfterCorrupt = (bool)valueAsObjectArr[3];
            var resumeAfter = (bool)valueAsObjectArr[4];
            void a()
            {
                //Load the game from the main thread
                if (loadBeforeCorrupt)
                {
                    SyncObjectSingleton.FormExecute(() => StockpileManagerEmuSide.LoadRomNet(sk));
                }

                if (loadBeforeCorrupt)
                {
                    StockpileManagerEmuSide.LoadStateNet(sk, false);
                }

                returnList = BlastTools.GenerateBlastLayersFromBlastGeneratorProtos(blastGeneratorProtos);
                if (applyAfterCorrupt)
                {
                    var bl = new BlastLayer();
                    foreach (var p in returnList.Where(x => x != null))
                    {
                        bl.Layer.AddRange(p.bl.Layer);
                    }


                    var autoUncorrupt = RtcCore.AutoUncorrupt;
                    if (autoUncorrupt && RtcCore.prevAutoUncorruptBlastLayer != null)
                        RtcCore.prevAutoUncorruptBlastLayer.Apply(false);

                    bl?.Apply(true);

                    if (autoUncorrupt)
                        RtcCore.prevAutoUncorruptBlastLayer = bl?.GetBackup();

                }
            }
            //If the emulator uses callbacks, we do everything on the main thread and once we're done, we unpause emulation
            if ((bool?)AllSpec.VanguardSpec[VSPEC.LOADSTATE_USES_CALLBACKS] ?? false)
            {
                SyncObjectSingleton.FormExecute(a);
                if (resumeAfter)
                {
                    e.setReturnValue(LocalNetCoreRouter.Route(Endpoints.Vanguard, Remote.ResumeEmulation, true));
                }
            }
            else
            {
                SyncObjectSingleton.EmuThreadExecute(a, false);
            }

            e.setReturnValue(returnList);
        }

        private static void GenerateBlastLayer(NetCoreAdvancedMessage advancedMessage, ref NetCoreEventArgs e)
        {
            var val = advancedMessage.objectValue as object[];
            var sk = val[0] as StashKey;
            var loadBeforeCorrupt = (bool)val[1];
            var applyBlastLayer = (bool)val[2];
            var backup = (bool)val[3];

            BlastLayer bl = null;

            var useSavestates = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES];

            void a()
            {
                lock (LoadLock)
                {
                    //Load the game from the main thread
                    if (useSavestates && loadBeforeCorrupt)
                    {
                        SyncObjectSingleton.FormExecute(() => StockpileManagerEmuSide.LoadRomNet(sk));
                    }

                    if (useSavestates && loadBeforeCorrupt)
                    {
                        StockpileManagerEmuSide.LoadStateNet(sk, false);
                    }

                    bl = RtcCore.GenerateBlastLayerOnAllThreads();

                    if (applyBlastLayer)
                    {

                        var autoUncorrupt = RtcCore.AutoUncorrupt;
                        if (autoUncorrupt && RtcCore.prevAutoUncorruptBlastLayer != null)
                            RtcCore.prevAutoUncorruptBlastLayer.Apply(false);

                        bl?.Apply(backup || autoUncorrupt);

                        if (autoUncorrupt)
                            RtcCore.prevAutoUncorruptBlastLayer = bl?.GetBackup();

                    }
                }
            }

            //If the emulator uses callbacks, we do everything on the main thread and once we're done, we unpause emulation
            if ((bool?)AllSpec.VanguardSpec[VSPEC.LOADSTATE_USES_CALLBACKS] ?? false)
            {
                SyncObjectSingleton.FormExecute(a);
                e.setReturnValue(LocalNetCoreRouter.Route(Endpoints.Vanguard, Remote.ResumeEmulation, true));
            }
            else //We can just do everything on the emulation thread as it'll block
            {
                SyncObjectSingleton.EmuThreadExecute(a, true);
            }

            if (advancedMessage.requestGuid != null)
            {
                e.setReturnValue(bl);
            }
        }

        private static void ApplyBlastLayer(NetCoreAdvancedMessage advancedMessage)
        {
            var temp = advancedMessage.objectValue as object[];
            var bl = (BlastLayer)temp[0];
            var storeUncorruptBackup = (bool)temp[1];
            var merge = (temp.Length > 2) && (bool)temp[2];
            void a()
            {
                var autoUncorrupt = RtcCore.AutoUncorrupt;
                if (autoUncorrupt && RtcCore.prevAutoUncorruptBlastLayer != null)
                    RtcCore.prevAutoUncorruptBlastLayer.Apply(false);

                bl?.Apply(storeUncorruptBackup || autoUncorrupt, true, merge);

                if (autoUncorrupt)
                    RtcCore.prevAutoUncorruptBlastLayer = bl?.GetBackup();
            }

            SyncObjectSingleton.EmuThreadExecute(a, true);
        }

        private static void FilterDomain(object[] objValues, ref NetCoreEventArgs e)
        {
            lock (LoadLock)
            {
                var domain = (string)objValues[0];
                var limiterListHash = (string)objValues[1];
                var sk = objValues[2] as StashKey; //Intentionally nullable
                var allLegalAdresses = new List<long>();

                void a()
                {
                    if (sk != null) //If a stashkey was passed in, we want to load then profile
                    {
                        StockpileManagerEmuSide.LoadStateNet(sk, false);
                    }

                    MemoryInterface mi = MemoryDomains.MemoryInterfaces[domain];

                    var listItemSize = Filtering.GetPrecisionFromHash(limiterListHash);

                    for (long i = 0; i < mi.Size; i += listItemSize)
                    {
                        if (Filtering.LimiterPeekBytes(i, i + listItemSize, limiterListHash, mi))
                        {
                            for (var j = 0; j < listItemSize; j++)
                            {
                                allLegalAdresses.Add(i + j);
                            }
                        }
                    }
                }

                //If the emulator uses callbacks and we're loading a state, we do everything on the main thread and once we're done, we unpause emulation
                if (sk != null && ((bool?)AllSpec.VanguardSpec[VSPEC.LOADSTATE_USES_CALLBACKS] ?? false))
                {
                    SyncObjectSingleton.FormExecute(a);
                    LocalNetCoreRouter.Route(Endpoints.Vanguard, Remote.ResumeEmulation, true);
                }
                else //We can just do everything on the emulation thread as it'll block
                {
                    SyncObjectSingleton.EmuThreadExecute(a, true);
                }

                e.setReturnValue(allLegalAdresses.ToArray());
            }
        }

        private static void LoadPlugins()
        {
            SyncObjectSingleton.FormExecute(() =>
                {
                    var emuPluginDir = string.Empty;
                    try
                    {
                        emuPluginDir = System.IO.Path.Combine(RtcCore.EmuDir, "RTC", "PLUGINS");
                    }
                    catch (Exception e)
                    {
                        Common.Logging.GlobalLogger.Error(e, "Unable to find plugin dir in {dir}", RtcCore.EmuDir + "\\RTC" + "\\PLUGINS");
                    }
                    RtcCore.LoadPlugins(new[] { RtcCore.PluginDir,  emuPluginDir });
                });
        }

        public static void Kill()
        {
        }
    }
}
