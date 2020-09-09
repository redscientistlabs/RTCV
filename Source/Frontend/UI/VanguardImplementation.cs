namespace RTCV.UI
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;
    using static RTCV.NetCore.NetcoreCommands;

    public static class VanguardImplementation
    {
        public static UIConnector connector = null;
        private static string lastVanguardClient = "";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

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

        private static void OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            try
            {
                var message = e.message;
                var advancedMessage = message as NetCoreAdvancedMessage;

                switch (message.Type) //Handle received messages here
                {
                    case REMOTE_PUSHVANGUARDSPEC:
                        PushVanguardSpec(advancedMessage, ref e);
                        break;
                    case REMOTE_ALLSPECSSENT:
                        AllSpecSent();
                        break;
                    case REMOTE_PUSHVANGUARDSPECUPDATE:
                        PushVanguardSpecUpdate(advancedMessage, ref e);
                        break;
                    case REMOTE_PUSHCORRUPTCORESPECUPDATE:
                        PushCorruptCoreSpecUpdate(advancedMessage, ref e);
                        break;
                    case REMOTE_GENERATEVMDTEXT:
                        GenerateVmdText(advancedMessage, ref e);
                        break;
                    case REMOTE_EVENT_DOMAINSUPDATED:
                        DomainsUpdated();
                        break;
                    case REMOTE_GETBLASTGENERATOR_LAYER:
                        GetBlastGeneratorLayer(ref e);
                        break;
                    case ERROR_DISABLE_AUTOCORRUPT:
                        DisableAutoCorrupt();
                        break;
                    case REMOTE_RENDER_DISPLAY:
                        RenderDisplay();
                        break;
                    case REMOTE_BACKUPKEY_STASH:
                        BackupKeyStash(advancedMessage);
                        break;
                    case KILLSWITCH_PULSE:
                        KillSwitchPulse();
                        break;
                    case RESET_GAME_PROTECTION_IF_RUNNING:
                        ResetGameProtectionIfRunning();
                        break;
                    case REMOTE_DISABLESAVESTATESUPPORT:
                        DisableSavestateSupport();
                        break;

                    case REMOTE_DISABLEGAMEPROTECTIONSUPPORT:
                        DisableGameProtectionSupport();
                        break;

                    case REMOTE_DISABLEREALTIMESUPPORT:
                        DisableRealTimeSupport();
                        break;
                    case REMOTE_DISABLEKILLSWITCHSUPPORT:
                        DisableKillSwitchSupport();
                        break;

                    case REMOTE_BLASTEDITOR_STARTSANITIZETOOL:
                        StartSanitizeTool();
                        break;

                    case REMOTE_BLASTEDITOR_LOADCORRUPT:
                        LoadCorrupt();
                        break;

                    case REMOTE_BLASTEDITOR_LOADORIGINAL:
                        LoadOriginal();
                        break;

                    case REMOTE_BLASTEDITOR_GETLAYERSIZE_UNLOCKEDUNITS:
                        GetLayerSizeUnlockedUnits(ref e);
                        break;

                    case REMOTE_BLASTEDITOR_GETLAYERSIZE:
                        GetLayerSize(ref e);
                        break;

                    case REMOTE_SANITIZETOOL_STARTSANITIZING:
                        StartSanitizing();
                        break;

                    case REMOTE_SANITIZETOOL_LEAVEWITHCHANGES:
                        LeaveWithChanges();
                        break;

                    case REMOTE_SANITIZETOOL_LEAVESUBTRACTCHANGES:
                        LeaveSubtractChanges();
                        break;

                    case REMOTE_SANITIZETOOL_YESEFFECT:
                        YesEffect();
                        break;

                    case REMOTE_SANITIZETOOL_NOEFFECT:
                        NoEffect();
                        break;

                    case REMOTE_SANITIZETOOL_REROLL:
                        Reroll();
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

        private static void PushVanguardSpec(NetCoreAdvancedMessage advancedMessage, ref NetCoreEventArgs e)
        {
            if (!RtcCore.Attached)
            {
                AllSpec.VanguardSpec = new FullSpec((PartialSpec)advancedMessage.objectValue, !RtcCore.Attached);
            }

            e.setReturnValue(true);

            //Push the UI and CorruptCore spec (since we're master)
            LocalNetCoreRouter.Route(CORRUPTCORE, REMOTE_PUSHUISPEC, AllSpec.UISpec.GetPartialSpec(), true);
            LocalNetCoreRouter.Route(CORRUPTCORE, REMOTE_PUSHCORRUPTCORESPEC, AllSpec.CorruptCoreSpec.GetPartialSpec(), true);

            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<CoreForm>().pnAutoKillSwitch.Visible = true;
                S.GET<CoreForm>().pnCrashProtection.Visible = true;
            });
            //Specs are all set up so UI is clear.
            LocalNetCoreRouter.Route(VANGUARD, REMOTE_ALLSPECSSENT, true);
        }

        private static void AllSpecSent()
        {
            if (UICore.FirstConnect)
            {
                UICore.Initialized.WaitOne(10000);
            }

            SyncObjectSingleton.FormExecute(() =>
            {
                if (UICore.FirstConnect)
                {
                    lastVanguardClient = (string)AllSpec.VanguardSpec?[VSPEC.NAME] ?? "VANGUARD";
                    UICore.FirstConnect = false;

                    //Load plugins on both sides
                    RtcCore.LoadPlugins();
                    LocalNetCoreRouter.Route(CORRUPTCORE, REMOTE_LOADPLUGINS, true);

                    //Configure the UI based on the vanguard spec
                    UICore.ConfigureUIFromVanguardSpec();

                    S.GET<CoreForm>().Show();

                    //Pull any lists from the vanguard implementation
                    if (RtcCore.EmuDir != null)
                    {
                        UICore.LoadLists(Path.Combine(RtcCore.EmuDir, "LISTS"));
                    }

                    UICore.LoadLists(RtcCore.ListsDir);

                    Panel sidebar = S.GET<CoreForm>().pnSideBar;
                    foreach (Control c in sidebar.Controls)
                    {
                        if (c is Button b)
                        {
                            if (!b.Text.Contains("Test") && !b.Text.Contains("Custom Layout") && b.ForeColor != Color.OrangeRed)
                            {
                                b.Visible = true;
                            }
                        }
                    }

                    string customLayoutPath = Path.Combine(RtcCore.RtcDir, "CustomLayout.txt");
                    if (File.Exists(customLayoutPath))
                    {
                        S.GET<CoreForm>().SetCustomLayoutName(customLayoutPath);
                        S.GET<CoreForm>().btnOpenCustomLayout.Visible = true;
                    }

                    DefaultGrids.engineConfig.LoadToMain();

                    DefaultGrids.glitchHarvester.LoadToNewWindow("Glitch Harvester", true);
                }
                else
                {
                    LocalNetCoreRouter.Route(CORRUPTCORE, REMOTE_LOADPLUGINS, true);
                    //make sure the other side reloads the plugins

                    var clientName = (string)AllSpec.VanguardSpec?[VSPEC.NAME] ?? "VANGUARD";
                    if (clientName != lastVanguardClient)
                    {
                        MessageBox.Show($"Error: Found {clientName} when previously connected to {lastVanguardClient}.\nPlease restart the RTC to swap clients.");
                        return;
                    }

                    //Push the VMDs since we store them out of spec
                    var vmdProtos = MemoryDomains.VmdPool.Values.Cast<VirtualMemoryDomain>().Select(x => x.Proto).ToArray();
                    LocalNetCoreRouter.Route(CORRUPTCORE, REMOTE_PUSHVMDPROTOS, vmdProtos, true);

                    S.GET<CoreForm>().Show();

                    //Configure the UI based on the vanguard spec
                    UICore.ConfigureUIFromVanguardSpec();

                    //Unblock the controls in the GH
                    S.GET<GlitchHarvesterBlastForm>().SetBlastButtonVisibility(true);

                    //Return to the main form. If the form is null for some reason, default to engineconfig
                    if (S.GET<CoreForm>().previousGrid == null)
                    {
                        S.GET<CoreForm>().previousGrid = DefaultGrids.engineConfig;
                    }

                    UICore.UnlockInterface();
                    S.GET<CoreForm>().previousGrid.LoadToMain();
                }

                S.GET<CoreForm>().pbAutoKillSwitchTimeout.Value = 0; //remove this once core form is dead

                if (!RtcCore.Attached)
                {
                    AutoKillSwitch.Enabled = true;
                }

                //Restart game protection
                if (S.GET<CoreForm>().cbUseGameProtection.Checked)
                {
                    if (StockpileManager_UISide.BackupedState != null)
                    {
                        StockpileManager_UISide.BackupedState.Run();
                    }

                    if (StockpileManager_UISide.BackupedState != null)
                    {
                        S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected(StockpileManager_UISide.BackupedState.SelectedDomains.ToArray());
                    }

                    GameProtection.Start();
                    if (GameProtection.WasAutoCorruptRunning)
                    {
                        S.GET<CoreForm>().AutoCorrupt = true;
                    }
                }

                S.GET<CoreForm>().Show();

                if (Params.IsParamSet("SIMPLE_MODE"))
                {
                    bool isSpec = (AllSpec.VanguardSpec[VSPEC.NAME] as string)?.ToUpper().Contains("SPEC") ?? false;

                    if (isSpec) //Simple Mode cannot run on Stubs
                    {
                        MessageBox.Show("Unfortunately, Simple Mode is not compatible with Stubs. RTC will now switch to Normal Mode.");
                        Params.RemoveParam("SIMPLE_MODE");
                    }
                    else
                    {
                        DefaultGrids.simpleMode.LoadToMain();
                        SimpleModeForm smForm = S.GET<SimpleModeForm>();
                        smForm.EnteringSimpleMode();
                    }
                }
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

                var vmdgenerator = S.GET<RTC_VmdGen_Form>();

                vmdgenerator.btnSelectAll_Click(null, null);

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
                        vmdgenerator.tbVmdName.Text = value.Trim();
                    vmdgenerator.btnGenerateVMD_Click(null, null);
                }
            });
            e.setReturnValue(true);
        }

        private static void DomainsUpdated()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<RTC_MemoryDomains_Form>().RefreshDomains();
                S.GET<RTC_MemoryDomains_Form>().SetMemoryDomainsAllButSelectedDomains(AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] as string[] ?? new string[] { });
            });
        }

        private static void GetBlastGeneratorLayer(ref NetCoreEventArgs e)
        {
            BlastLayer bl = null;
            SyncObjectSingleton.FormExecute(() =>
            {
                bl = S.GET<RTC_BlastGenerator_Form>().GenerateBlastLayers(true, true, false);
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
                StockpileManager_UISide.BackupedState = sk;
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

                S.GET<CorruptionEngineForm>().cbSelectedEngine.Items.Remove("Hellgenie Engine");
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
            });
        }

        private static void StartSanitizeTool()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var blastEditor = S.GET<RTC_NewBlastEditor_Form>();
                blastEditor.OpenSanitizeTool(false);
            });
        }

        private static void LoadCorrupt()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var blastEditor = S.GET<RTC_NewBlastEditor_Form>();
                blastEditor.btnLoadCorrupt_Click(null, null);
            });
        }

        private static void LoadOriginal()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var blastEditor = S.GET<RTC_NewBlastEditor_Form>();
                blastEditor.LoadOriginal();
            });
        }

        private static void GetLayerSizeUnlockedUnits(ref NetCoreEventArgs e)
        {
            var units = 0;
            SyncObjectSingleton.FormExecute(() =>
            {   // this is what the sanitize tool uses to judge how many units there are left to sanitize.
                var blastEditor = S.GET<RTC_NewBlastEditor_Form>();
                units = blastEditor.currentSK?.BlastLayer?.Layer.Count(x => !x.IsLocked) ?? -1;
            });

            e.setReturnValue(units);
        }

        private static void GetLayerSize(ref NetCoreEventArgs e)
        {
            var layerSize = 0;
            SyncObjectSingleton.FormExecute(() =>
            {
                layerSize = S.GET<RTC_NewBlastEditor_Form>().currentSK?.BlastLayer?.Layer?.Count ?? -1;
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
