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

    public static class UI_VanguardImplementation
    {
        public static UIConnector connector = null;
        private static string lastVanguardClient = "";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void StartServer()
        {
            logger.Trace("Starting UI Vanguard Implementation");

            var spec = new NetCoreReceiver();
            spec.MessageReceived += OnMessageReceived;

            spec.Attached = CorruptCore.RtcCore.Attached;

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
                var simpleMessage = message as NetCoreSimpleMessage;
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
                    throw new RTCV.NetCore.AbortEverythingException();
                }

                return;
            }
        }

        private static void PushVanguardSpec(NetCoreAdvancedMessage advancedMessage, ref NetCoreEventArgs e)
        {
            if (!CorruptCore.RtcCore.Attached)
            {
                RTCV.NetCore.AllSpec.VanguardSpec = new FullSpec((PartialSpec)advancedMessage.objectValue, !CorruptCore.RtcCore.Attached);
            }

            e.setReturnValue(true);

            //Push the UI and CorruptCore spec (since we're master)
            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHUISPEC, RTCV.NetCore.AllSpec.UISpec.GetPartialSpec(), true);
            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHCORRUPTCORESPEC, RTCV.NetCore.AllSpec.CorruptCoreSpec.GetPartialSpec(), true);

            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<UI_CoreForm>().pnAutoKillSwitch.Visible = true;
                S.GET<UI_CoreForm>().pnCrashProtection.Visible = true;
            });
            //Specs are all set up so UI is clear.
            LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_ALLSPECSSENT, true);
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
                    lastVanguardClient = (string)RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.NAME] ?? "VANGUARD";
                    UICore.FirstConnect = false;

                    //Load plugins on both sides
                    CorruptCore.RtcCore.LoadPlugins();
                    LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_LOADPLUGINS, true);

                    //Configure the UI based on the vanguard spec
                    UICore.ConfigureUIFromVanguardSpec();

                    S.GET<UI_CoreForm>().Show();

                    //Pull any lists from the vanguard implementation
                    if (RtcCore.EmuDir != null)
                    {
                        UICore.LoadLists(Path.Combine(RtcCore.EmuDir, "LISTS"));
                    }

                    UICore.LoadLists(CorruptCore.RtcCore.ListsDir);

                    Panel sidebar = S.GET<UI_CoreForm>().pnSideBar;
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

                    string customLayoutPath = Path.Combine(RTCV.CorruptCore.RtcCore.RtcDir, "CustomLayout.txt");
                    if (File.Exists(customLayoutPath))
                    {
                        S.GET<UI_CoreForm>().SetCustomLayoutName(customLayoutPath);
                        S.GET<UI_CoreForm>().btnOpenCustomLayout.Visible = true;
                    }

                    UI_DefaultGrids.engineConfig.LoadToMain();

                    UI_DefaultGrids.glitchHarvester.LoadToNewWindow("Glitch Harvester", true);
                }
                else
                {
                    LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_LOADPLUGINS, true);
                    //make sure the other side reloads the plugins

                    var clientName = (string)RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.NAME] ?? "VANGUARD";
                    if (clientName != lastVanguardClient)
                    {
                        MessageBox.Show($"Error: Found {clientName} when previously connected to {lastVanguardClient}.\nPlease restart the RTC to swap clients.");
                        return;
                    }

                    //Push the VMDs since we store them out of spec
                    var vmdProtos = MemoryDomains.VmdPool.Values.Cast<VirtualMemoryDomain>().Select(x => x.Proto).ToArray();
                    LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHVMDPROTOS, vmdProtos, true);

                    S.GET<UI_CoreForm>().Show();

                    //Configure the UI based on the vanguard spec
                    UICore.ConfigureUIFromVanguardSpec();

                    //Unblock the controls in the GH
                    S.GET<RTC_GlitchHarvesterBlast_Form>().SetBlastButtonVisibility(true);

                    //Return to the main form. If the form is null for some reason, default to engineconfig
                    if (S.GET<UI_CoreForm>().previousGrid == null)
                    {
                        S.GET<UI_CoreForm>().previousGrid = UI_DefaultGrids.engineConfig;
                    }

                    UICore.UnlockInterface();
                    S.GET<UI_CoreForm>().previousGrid.LoadToMain();
                }

                S.GET<UI_CoreForm>().pbAutoKillSwitchTimeout.Value = 0; //remove this once core form is dead

                if (!CorruptCore.RtcCore.Attached)
                {
                    AutoKillSwitch.Enabled = true;
                }

                //Restart game protection
                if (S.GET<UI_CoreForm>().cbUseGameProtection.Checked)
                {
                    if (CorruptCore.StockpileManager_UISide.BackupedState != null)
                    {
                        CorruptCore.StockpileManager_UISide.BackupedState.Run();
                    }

                    if (CorruptCore.StockpileManager_UISide.BackupedState != null)
                    {
                        S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected(CorruptCore.StockpileManager_UISide.BackupedState.SelectedDomains.ToArray());
                    }

                    GameProtection.Start();
                    if (GameProtection.WasAutoCorruptRunning)
                    {
                        S.GET<UI_CoreForm>().AutoCorrupt = true;
                    }
                }

                S.GET<UI_CoreForm>().Show();

                if (NetCore.Params.IsParamSet("SIMPLE_MODE"))
                {
                    bool isSpec = (AllSpec.VanguardSpec[VSPEC.NAME] as string)?.ToUpper().Contains("SPEC") ?? false;

                    if (isSpec) //Simple Mode cannot run on Stubs
                    {
                        MessageBox.Show("Unfortunately, Simple Mode is not compatible with Stubs. RTC will now switch to Normal Mode.");
                        NetCore.Params.RemoveParam("SIMPLE_MODE");
                    }
                    else
                    {
                        UI_DefaultGrids.simpleMode.LoadToMain();
                        RTC_SimpleMode_Form smForm = S.GET<RTC_SimpleMode_Form>();
                        smForm.EnteringSimpleMode();
                    }
                }
            });
        }

        private static void PushVanguardSpecUpdate(NetCoreAdvancedMessage advancedMessage, ref NetCoreEventArgs e)
        {
            SyncObjectSingleton.FormExecute(() =>
                        {
                            RTCV.NetCore.AllSpec.VanguardSpec?.Update((PartialSpec)advancedMessage.objectValue);
                        });
            e.setReturnValue(true);
        }

        //CorruptCore pushed its spec. Note the false on propogate (since we don't want a recursive loop)
        private static void PushCorruptCoreSpecUpdate(NetCoreAdvancedMessage advancedMessage, ref NetCoreEventArgs e)
        {
            SyncObjectSingleton.FormExecute(() =>
                        {
                            RTCV.NetCore.AllSpec.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
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

                if (RTCV.UI.UI_Extensions.GetInputBox("VMD Generation", "Enter the new VMD name:", ref value) == DialogResult.OK)
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
                S.GET<RTC_MemoryDomains_Form>().SetMemoryDomainsAllButSelectedDomains(RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] as string[] ?? new string[] { });
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
                S.GET<UI_CoreForm>().AutoCorrupt = false;
            });
        }

        private static void RenderDisplay()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<RTC_GlitchHarvesterBlast_Form>().refreshRenderOutputButton();
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
                    S.GET<UI_CoreForm>().btnGpJumpBack.Visible = true;
                    S.GET<UI_CoreForm>().btnGpJumpNow.Visible = true;
                });
            }
        }

        private static void ResetGameProtectionIfRunning()
        {
            if (GameProtection.isRunning)
            {
                SyncObjectSingleton.FormExecute(() =>
                {
                    S.GET<UI_CoreForm>().cbUseGameProtection.Checked = false;
                    S.GET<UI_CoreForm>().cbUseGameProtection.Checked = true;
                });
            }
        }

        private static void DisableSavestateSupport()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<RTC_SavestateManager_Form>().DisableFeature();
                S.GET<UI_CoreForm>().pnCrashProtection.Visible = false;
            });
        }

        private static void DisableGameProtectionSupport()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<UI_CoreForm>().pnCrashProtection.Visible = false;
            });
        }

        private static void DisableRealTimeSupport()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                Button btnManual = S.GET<UI_CoreForm>().btnManualBlast;
                if (AllSpec.VanguardSpec[VSPEC.REPLACE_MANUALBLAST_WITH_GHCORRUPT] != null)
                {
                    btnManual.Text = "  Corrupt";
                }
                else
                {
                    btnManual.Visible = false;
                }

                S.GET<UI_CoreForm>().btnAutoCorrupt.Enabled = false;
                S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = false;
                S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Enabled = false;
                S.GET<RTC_GlitchHarvesterBlast_Form>().btnSendRaw.Enabled = false;
                S.GET<RTC_GlitchHarvesterBlast_Form>().btnBlastToggle.Enabled = false;

                S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.Items.Remove("Hellgenie Engine");
                S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.Items.Remove("Distortion Engine");
                S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.Items.Remove("Pipe Engine");
                S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.Items.Remove("Freeze Engine");
            });
        }

        private static void DisableKillSwitchSupport()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                S.GET<UI_CoreForm>().pnAutoKillSwitch.Visible = false;
                S.GET<UI_CoreForm>().cbUseAutoKillSwitch.Checked = false;
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
                var sanitizeTool = S.GET<RTC_SanitizeTool_Form>();
                sanitizeTool.btnStartSanitizing_Click(null, null);
            });
        }

        private static void LeaveWithChanges()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var sanitizeTool = S.GET<RTC_SanitizeTool_Form>();
                sanitizeTool.lbSteps.Items.Clear(); //this is a hack for leaving in automation
                sanitizeTool.btnLeaveWithChanges_Click(null, null);
            });
        }

        private static void LeaveSubtractChanges()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var sanitizeTool = S.GET<RTC_SanitizeTool_Form>();
                sanitizeTool.lbSteps.Items.Clear(); //this is a hack for leaving in automation
                sanitizeTool.btnLeaveSubstractChanges_Click(null, null);
            });
        }

        private static void YesEffect()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var sanitizeTool = S.GET<RTC_SanitizeTool_Form>();
                sanitizeTool.btnYesEffect_Click(null, null);
            });
        }

        private static void NoEffect()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var sanitizeTool = S.GET<RTC_SanitizeTool_Form>();
                sanitizeTool.btnNoEffect_Click(null, null);
            });
        }

        private static void Reroll()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                var sanitizeTool = S.GET<RTC_SanitizeTool_Form>();
                sanitizeTool.btnReroll_Click(null, null);
            });
        }

        private static void KillSwitchPulse()
        {
            AutoKillSwitch.Pulse();
        }
    }
}
