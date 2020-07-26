namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using RTCV.NetCore;
    using static RTCV.NetCore.NetcoreCommands;

    public class CorruptCoreConnector : IRoutable
    {
        private static volatile object _loadLock = new object();
        private static object loadLock => _loadLock;

        public CorruptCoreConnector()
        {
            //spec.Side = RTCV.NetCore.NetworkSide.CLIENT;
        }

        public object OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            try
            { //Use setReturnValue to handle returns
                var message = e.message;
                var advancedMessage = message as NetCoreAdvancedMessage;

                switch (e.message.Type)
                {
                    case "GETSPECDUMPS":
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Spec Dump from CorruptCore");
                        sb.AppendLine();
                        sb.AppendLine("UISpec");
                        RTCV.NetCore.AllSpec.UISpec?.GetDump().ForEach(x => sb.AppendLine(x));
                        sb.AppendLine("CorruptCoreSpec");
                        RTCV.NetCore.AllSpec.CorruptCoreSpec?.GetDump().ForEach(x => sb.AppendLine(x));
                        sb.AppendLine("VanguardSpec");
                        RTCV.NetCore.AllSpec.VanguardSpec?.GetDump().ForEach(x => sb.AppendLine(x));
                        e.setReturnValue(sb.ToString());
                        break;
                    //UI sent its spec
                    case REMOTE_PUSHUISPEC:
                        {
                            SyncObjectSingleton.FormExecute(() =>
                            {
                                RTCV.NetCore.AllSpec.UISpec = new FullSpec((PartialSpec)advancedMessage.objectValue, !RtcCore.Attached);
                            });
                            break;
                        }

                    //UI sent a spec update
                    case REMOTE_PUSHUISPECUPDATE:
                        SyncObjectSingleton.FormExecute(() =>
                        {
                            RTCV.NetCore.AllSpec.UISpec?.Update((PartialSpec)advancedMessage.objectValue);
                        });
                        break;

                    //Vanguard sent a copy of its spec
                    case REMOTE_PUSHVANGUARDSPEC:

                        SyncObjectSingleton.FormExecute(() =>
                        {
                            if (!RtcCore.Attached)
                            {
                                RTCV.NetCore.AllSpec.VanguardSpec = new FullSpec((PartialSpec)advancedMessage.objectValue, !RtcCore.Attached);
                            }
                        });
                        break;

                    //Vanguard sent a spec update
                    case REMOTE_PUSHVANGUARDSPECUPDATE:
                        RTCV.NetCore.AllSpec.VanguardSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
                        break;

                    //UI sent a copy of the CorruptCore spec
                    case REMOTE_PUSHCORRUPTCORESPEC:
                        SyncObjectSingleton.FormExecute(() =>
                        {
                            //So here's the deal. The UI doesn't actually have the full memory domains (md isn't sent across) so if we take them from it, it results in them going null
                            //Instead, we stick with what we have, then tell the UI to use that.

                            var temp = new FullSpec((PartialSpec)advancedMessage.objectValue, !RtcCore.Attached);

                            //Stick with what we have if it exists to prevent any exceptions if autocorrupt was on or something, then call refresh
                            temp.Update("MEMORYINTERFACES", AllSpec.CorruptCoreSpec?["MEMORYINTERFACES"] ?? new Dictionary<string, MemoryDomainProxy>());

                            RTCV.NetCore.AllSpec.CorruptCoreSpec = new FullSpec(temp.GetPartialSpec(), !RtcCore.Attached);
                            RTCV.NetCore.AllSpec.CorruptCoreSpec.SpecUpdated += (ob, eas) =>
                            {
                                PartialSpec partial = eas.partialSpec;
                                LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_PUSHCORRUPTCORESPECUPDATE, partial, true);
                            };
                            RTCV.CorruptCore.MemoryDomains.RefreshDomains();
                        });
                        e.setReturnValue(true);
                        break;

                    //UI sent an update of the CorruptCore spec
                    case REMOTE_PUSHCORRUPTCORESPECUPDATE:
                        SyncObjectSingleton.FormExecute(() =>
                        {
                            RTCV.NetCore.AllSpec.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
                        });
                        break;

                    case REMOTE_EVENT_DOMAINSUPDATED:
                        var domainsChanged = (bool)advancedMessage.objectValue;
                        MemoryDomains.RefreshDomains(domainsChanged);
                        break;

                    case REMOTE_EVENT_RESTRICTFEATURES:
                        {
                            if (!RTCV.NetCore.AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_SAVESTATES) ?? true)
                            {
                                LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_DISABLESAVESTATESUPPORT);
                            }

                            if (!RTCV.NetCore.AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_REALTIME) ?? true)
                            {
                                LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_DISABLEREALTIMESUPPORT);
                            }

                            if (!RTCV.NetCore.AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_KILLSWITCH) ?? true)
                            {
                                LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_DISABLEKILLSWITCHSUPPORT);
                            }

                            if (!RTCV.NetCore.AllSpec.VanguardSpec?.Get<bool>(VSPEC.SUPPORTS_GAMEPROTECTION) ?? true)
                            {
                                LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_DISABLEGAMEPROTECTIONSUPPORT);
                            }

                            break;
                        }
                    case REMOTE_EVENT_SHUTDOWN:
                        {
                            RtcCore.Shutdown();
                            break;
                        }

                    case REMOTE_OPENHEXEDITOR:
                        {
                            if ((bool?)AllSpec.VanguardSpec[VSPEC.USE_INTEGRATED_HEXEDITOR] ?? false)
                            {
                              LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_OPENHEXEDITOR, true);
                            }
                            else
                            {
                                //Route it to the plugin if loaded
                                if (RtcCore.PluginHost.LoadedPlugins.Any(x => x.Name == "Hex Editor"))
                                    LocalNetCoreRouter.Route("HEXEDITOR", NetcoreCommands.REMOTE_OPENHEXEDITOR, true);
                                else
                                {
                                    MessageBox.Show("The current Vanguard implementation does not include a\n hex editor & the hex editor plugin isn't loaded. Aborting.");
                                }
                            }
                        }
                        break;

                    case EMU_OPEN_HEXEDITOR_ADDRESS:
                        {
                            if ((bool?)AllSpec.VanguardSpec[VSPEC.USE_INTEGRATED_HEXEDITOR] ?? false)
                            {
                                LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.EMU_OPEN_HEXEDITOR_ADDRESS, advancedMessage.objectValue, true);
                            }
                            else
                            {
                                //Route it to the plugin if loaded
                                if (RtcCore.PluginHost.LoadedPlugins.Any(x => x.Name == "Hex Editor"))
                                    LocalNetCoreRouter.Route("HEXEDITOR", NetcoreCommands.EMU_OPEN_HEXEDITOR_ADDRESS, advancedMessage.objectValue, true);
                                else
                                {
                                    MessageBox.Show("The current Vanguard implementation does not include a\n hex editor & the hex editor plugin isn't loaded. Aborting.");
                                }
                            }
                            break;
                        }

                    case MANUALBLAST:
                        {
                            RtcCore.GenerateAndBlast();
                        }
                        break;

                    case GENERATEBLASTLAYER:
                        {
                            var val = advancedMessage.objectValue as object[];
                            StashKey sk = val[0] as StashKey;
                            bool loadBeforeCorrupt = (bool)val[1];
                            bool applyBlastLayer = (bool)val[2];
                            bool backup = (bool)val[3];

                            BlastLayer bl = null;

                            bool UseSavestates = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES];

                            void a()
                            {
                                lock (loadLock)
                                {
                                    //Load the game from the main thread
                                    if (UseSavestates && loadBeforeCorrupt)
                                    {
                                        SyncObjectSingleton.FormExecute(() =>
                                        {
                                            StockpileManager_EmuSide.LoadRom_NET(sk);
                                        });
                                    }

                                    if (UseSavestates && loadBeforeCorrupt)
                                    {
                                        StockpileManager_EmuSide.LoadState_NET(sk, false);
                                    }

                                    //We pull the domains here because if the syncsettings changed, there's a chance the domains changed
                                    string[] domains = (string[])AllSpec.UISpec["SELECTEDDOMAINS"];

                                    var cpus = Environment.ProcessorCount;

                                    if (cpus == 1 || AllSpec.VanguardSpec[VSPEC.SUPPORTS_MULTITHREAD] == null)
                                    {
                                        bl = RtcCore.GenerateBlastLayer(domains);
                                    }
                                    else
                                    {
                                        //if emulator supports multithreaded access of the domains, disregard the emulation thread and just span threads...

                                        long reminder = RtcCore.Intensity % (cpus - 1);

                                        long splitintensity = (RtcCore.Intensity - reminder) / (cpus - 1);

                                        Task<BlastLayer>[] tasks = new Task<BlastLayer>[cpus];
                                        for (int i = 0; i < cpus; i++)
                                        {
                                            long requestedIntensity = splitintensity;

                                            if (i == 0 && reminder != 0)
                                            {
                                                requestedIntensity = reminder;
                                            }

                                            tasks[i] = Task.Factory.StartNew(() =>
                                                RtcCore.GenerateBlastLayer(domains, requestedIntensity));
                                        }

                                        Task.WaitAll(tasks);

                                        bl = tasks[0]
                                            .Result ?? new BlastLayer();

                                        if (tasks.Length > 1)
                                        {
                                            for (int i = 1; i < tasks.Length; i++)
                                            {
                                                if (tasks[i]
                                                    .Result != null)
                                                {
                                                    bl.Layer.AddRange(tasks[i]
                                                        .Result.Layer);
                                                }
                                            }
                                        }

                                        if (bl.Layer.Count == 0)
                                        {
                                            bl = null;
                                        }
                                    }

                                    if (applyBlastLayer)
                                    {
                                        bl?.Apply(backup);
                                    }
                                }
                            }
                            //If the emulator uses callbacks, we do everything on the main thread and once we're done, we unpause emulation
                            if ((bool?)AllSpec.VanguardSpec[VSPEC.LOADSTATE_USES_CALLBACKS] ?? false)
                            {
                                SyncObjectSingleton.FormExecute(a);
                                e.setReturnValue(LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_RESUMEEMULATION, true));
                            }
                            else //We can just do everything on the emulation thread as it'll block
                            {
                                SyncObjectSingleton.EmuThreadExecute(a, true);
                            }

                            if (advancedMessage.requestGuid != null)
                            {
                                e.setReturnValue(bl);
                            }
                            break;
                        }
                    case APPLYBLASTLAYER:
                        {
                            var temp = advancedMessage.objectValue as object[];
                            BlastLayer bl = (BlastLayer)temp[0];
                            bool storeUncorruptBackup = (bool)temp[1];

                            bool merge = false;

                            if (temp.Length > 2)
                                 merge = (bool)temp[2];


                            void a()
                            {
                                bl.Apply(storeUncorruptBackup, true, merge);
                            }

                            SyncObjectSingleton.EmuThreadExecute(a, true);
                            break;
                        }

                    /*
                    case STASHKEY:
                        {
                            var temp = advancedMessage.objectValue as object[];

                            var sk = temp[0] as StashKey;
                            var romFilename = temp[1] as String;
                            var romData = temp[2] as Byte[];

                            if (!File.Exists(CorruptCore.rtcDir + Path.DirectorySeparatorChar + "WORKING" + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + romFilename))
                                File.WriteAllBytes(CorruptCore.rtcDir + Path.DirectorySeparatorChar + "WORKING" + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + romFilename, romData);

                            sk.RomFilename = CorruptCore.rtcDir + Path.DirectorySeparatorChar + "WORKING" + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + CorruptCore_Extensions.getShortFilenameFromPath(romFilename);
                            sk.DeployState();
                            sk.Run();
                        }
                        break;
                        */

                    case REMOTE_PUSHRTCSPEC:
                        RTCV.NetCore.AllSpec.CorruptCoreSpec = new FullSpec((PartialSpec)advancedMessage.objectValue, !RtcCore.Attached);
                        e.setReturnValue(true);
                        break;

                    case REMOTE_PUSHRTCSPECUPDATE:
                        RTCV.NetCore.AllSpec.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
                        break;

                    case BLASTGENERATOR_BLAST:
                        {
                            List<BlastGeneratorProto> returnList = null;
                            StashKey sk = (StashKey)(advancedMessage.objectValue as object[])[0];
                            List<BlastGeneratorProto> blastGeneratorProtos = (List<BlastGeneratorProto>)(advancedMessage.objectValue as object[])[1];
                            bool loadBeforeCorrupt = (bool)(advancedMessage.objectValue as object[])[2];
                            bool applyAfterCorrupt = (bool)(advancedMessage.objectValue as object[])[3];
                            bool resumeAfter = (bool)(advancedMessage.objectValue as object[])[4];
                            void a()
                            {
                                //Load the game from the main thread
                                if (loadBeforeCorrupt)
                                {
                                    SyncObjectSingleton.FormExecute(() =>
                                    {
                                        StockpileManager_EmuSide.LoadRom_NET(sk);
                                    });
                                }

                                if (loadBeforeCorrupt)
                                {
                                    StockpileManager_EmuSide.LoadState_NET(sk, false);
                                }

                                returnList = BlastTools.GenerateBlastLayersFromBlastGeneratorProtos(blastGeneratorProtos, sk);
                                if (applyAfterCorrupt)
                                {
                                    BlastLayer bl = new BlastLayer();
                                    foreach (var p in returnList.Where(x => x != null))
                                    {
                                        bl.Layer.AddRange(p.bl.Layer);
                                    }
                                    bl.Apply(true);
                                }
                            }
                            //If the emulator uses callbacks, we do everything on the main thread and once we're done, we unpause emulation
                            if ((bool?)AllSpec.VanguardSpec[VSPEC.LOADSTATE_USES_CALLBACKS] ?? false)
                            {
                                SyncObjectSingleton.FormExecute(a);
                                if (resumeAfter)
                                {
                                    e.setReturnValue(LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_RESUMEEMULATION, true));
                                }
                            }
                            else
                            {
                                SyncObjectSingleton.EmuThreadExecute(a, false);
                            }

                            e.setReturnValue(returnList);

                            break;
                        }

                    case REMOTE_LOADSTATE:
                        {
                            lock (loadLock)
                            {
                                StashKey sk = (StashKey)(advancedMessage.objectValue as object[])[0];
                                bool reloadRom = (bool)(advancedMessage.objectValue as object[])[1];
                                bool runBlastLayer = (bool)(advancedMessage.objectValue as object[])[2];

                                bool returnValue = false;

                                //Load the game from the main thread
                                if (reloadRom)
                                {
                                    SyncObjectSingleton.FormExecute(() =>
                                    {
                                        StockpileManager_EmuSide.LoadRom_NET(sk);
                                    });
                                }
                                void a()
                                {
                                    returnValue = StockpileManager_EmuSide.LoadState_NET(sk, runBlastLayer);
                                }
                                //If the emulator uses callbacks, we do everything on the main thread and once we're done, we unpause emulation
                                if ((bool?)AllSpec.VanguardSpec[VSPEC.LOADSTATE_USES_CALLBACKS] ?? false)
                                {
                                    SyncObjectSingleton.FormExecute(a);
                                    e.setReturnValue(LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_RESUMEEMULATION, true));
                                }
                                else //We're loading on the emulator thread which'll block
                                {
                                    SyncObjectSingleton.EmuThreadExecute(a, false);
                                }
                                e.setReturnValue(returnValue);
                            }
                        }
                        break;
                    case REMOTE_SAVESTATE:
                        {
                            StashKey sk = null;
                            void a()
                            {
                                sk = StockpileManager_EmuSide.SaveState_NET(advancedMessage.objectValue as StashKey); //Has to be nullable cast
                            }
                            SyncObjectSingleton.EmuThreadExecute(a, false);
                            e.setReturnValue(sk);
                        }
                        break;
                    case REMOTE_SAVESTATELESS:
                        {
                            StashKey sk = null;
                            void a()
                            {
                                sk = StockpileManager_EmuSide.SaveStateLess_NET(advancedMessage.objectValue as StashKey); //Has to be nullable cast
                            }
                            SyncObjectSingleton.EmuThreadExecute(a, false);
                            e.setReturnValue(sk);
                        }
                        break;

                    case REMOTE_BACKUPKEY_REQUEST:
                        {
                            //We don't store this in the spec as it'd be horrible to push it to the UI and it doesn't care
                            //if (!LocalNetCoreRouter.QueryRoute<bool>(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_ISNORMALADVANCE))
                            //break;

                            StashKey sk = null;
                            //We send an unsynced command back
                            SyncObjectSingleton.FormExecute(() =>
                            {
                                sk = StockpileManager_EmuSide.SaveState_NET();
                            });

                            if (sk != null)
                            {
                                LocalNetCoreRouter.Route(NetcoreCommands.UI, REMOTE_BACKUPKEY_STASH, sk, false);
                            }

                            break;
                        }
                    case REMOTE_DOMAIN_GETDOMAINS:
                        e.setReturnValue(LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_DOMAIN_GETDOMAINS, true));
                        break;
                    case REMOTE_PUSHVMDPROTOS:
                        MemoryDomains.VmdPool.Clear();
                        foreach (var proto in (advancedMessage.objectValue as VmdPrototype[]))
                        {
                            MemoryDomains.AddVMD(proto);
                        }

                        break;

                    case REMOTE_DOMAIN_VMD_ADD:
                        MemoryDomains.AddVMD_NET((advancedMessage.objectValue as VmdPrototype));
                        break;

                    case REMOTE_DOMAIN_VMD_REMOVE:
                        {
                            StepActions.ClearStepBlastUnits();
                            MemoryDomains.RemoveVMD_NET((advancedMessage.objectValue as string));
                        }
                        break;

                    case REMOTE_DOMAIN_ACTIVETABLE_MAKEDUMP:
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

                    case REMOTE_BLASTTOOLS_GETAPPLIEDBACKUPLAYER:
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

                    case REMOTE_LONGARRAY_FILTERDOMAIN:
                        {
                            lock (loadLock)
                            {
                                var objValues = (advancedMessage.objectValue as object[]);
                                string domain = (string)objValues[0];
                                string LimiterListHash = (string)objValues[1];
                                StashKey sk = objValues[2] as StashKey; //Intentionally nullable

                                void a()
                                {
                                    if (sk != null) //If a stashkey was passed in, we want to load then profile
                                    {
                                        StockpileManager_EmuSide.LoadState_NET(sk, false);
                                    }

                                    MemoryInterface mi = MemoryDomains.MemoryInterfaces[domain];
                                    List<long> allLegalAdresses = new List<long>();

                                    int listItemSize = Filtering.GetPrecisionFromHash(LimiterListHash);

                                    for (long i = 0; i < mi.Size; i += listItemSize)
                                    {
                                        if (Filtering.LimiterPeekBytes(i, i + listItemSize, mi.Name, LimiterListHash, mi))
                                        {
                                            for (int j = 0; j < listItemSize; j++)
                                            {
                                                allLegalAdresses.Add(i + j);
                                            }
                                        }
                                    }

                                    e.setReturnValue(allLegalAdresses.ToArray());
                                }

                                //If the emulator uses callbacks and we're loading a state, we do everything on the main thread and once we're done, we unpause emulation
                                if (sk != null && ((bool?)AllSpec.VanguardSpec[VSPEC.LOADSTATE_USES_CALLBACKS] ?? false))
                                {
                                    SyncObjectSingleton.FormExecute(a);
                                    LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_RESUMEEMULATION, true);
                                }
                                else //We can just do everything on the emulation thread as it'll block
                                {
                                    SyncObjectSingleton.EmuThreadExecute(a, true);
                                }
                            }
                        }
                        break;

                    case REMOTE_KEY_GETRAWBLASTLAYER:
                        {
                            void a()
                            { e.setReturnValue(StockpileManager_EmuSide.GetRawBlastlayer()); }
                            SyncObjectSingleton.EmuThreadExecute(a, false);
                        }
                        break;

                    case REMOTE_BL_GETDIFFBLASTLAYER:
                        {
                            string filename = (advancedMessage.objectValue as string);
                            void a()
                            { e.setReturnValue(BlastDiff.GetBlastLayer(filename)); }
                            SyncObjectSingleton.EmuThreadExecute(a, false);
                        }
                        break;

                    case REMOTE_SET_APPLYUNCORRUPTBL:
                        {
                            void a()
                            {
                                if (StockpileManager_EmuSide.UnCorruptBL != null)
                                {
                                    StockpileManager_EmuSide.UnCorruptBL.Apply(true);
                                }
                            }
                            SyncObjectSingleton.EmuThreadExecute(a, false);
                        }
                        break;

                    case REMOTE_SET_APPLYCORRUPTBL:
                        {
                            void a()
                            {
                                if (StockpileManager_EmuSide.CorruptBL != null)
                                {
                                    StockpileManager_EmuSide.CorruptBL.Apply(false);
                                }
                            }
                            SyncObjectSingleton.EmuThreadExecute(a, false);
                        }
                        break;

                    case REMOTE_CLEARSTEPBLASTUNITS:
                        SyncObjectSingleton.FormExecute(() =>
                        {
                            StepActions.ClearStepBlastUnits();
                        });
                        break;

                    case REMOTE_LOADPLUGINS:
                        SyncObjectSingleton.FormExecute(() =>
                        {
                            string emuPluginDir = "";
                            try
                            {
                                emuPluginDir = System.IO.Path.Combine(RtcCore.EmuDir, "RTC", "PLUGINS");
                            }
                            catch (Exception e)
                            {
                                RTCV.Common.Logging.GlobalLogger.Error(e, "Unable to find plugin dir in {dir}", RtcCore.EmuDir + "\\RTC" + "\\PLUGINS");
                            }
                            RtcCore.LoadPlugins(new[] { RtcCore.pluginDir,  emuPluginDir });
                        });

                        break;
                    case REMOTE_REMOVEEXCESSINFINITESTEPUNITS:
                        SyncObjectSingleton.FormExecute(() =>
                        {
                            StepActions.RemoveExcessInfiniteStepUnits();
                        });
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
                    throw new RTCV.NetCore.AbortEverythingException();
                }

                return e.returnMessage;
            }
        }

        public void Kill()
        {
        }
    }
}
