namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Extensions;
    using RTCV.UI.Input;
    using RTCV.UI.Modular;
    using static RTCV.NetCore.NetcoreCommands;

    public static class UICore
    {
        //Note Box Settings
        public static Point NoteBoxPosition;
        public static Size NoteBoxSize;

        public static bool FirstConnect = true;
        public static ManualResetEvent Initialized = new ManualResetEvent(false);
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static System.Timers.Timer inputCheckTimer;

        //RTC Main Forms
        public static SelectBoxForm mtForm = null;

        public static BindingCollection HotkeyBindings = new BindingCollection();

        public static void Start(Form standaloneForm = null)
        {
            S.formRegister.FormRegistered += FormRegister_FormRegistered;
            //registerFormEvents(S.GET<RTC_Core_Form>());
            registerFormEvents(S.GET<UI_CoreForm>());
            registerHotkeyBlacklistControls(S.GET<UI_CoreForm>());

            if (!RtcCore.Attached)
            {
                S.SET((Forms.RTC_Standalone_Form)standaloneForm);
            }

            Form dummy = new Form();
            IntPtr Handle = dummy.Handle;

            SyncObjectSingleton.SyncObject = dummy;

            VanguardImplementation.StartServer();

            PartialSpec p = new PartialSpec("UISpec");

            p["SELECTEDDOMAINS"] = new string[] { };

            AllSpec.UISpec = new FullSpec(p, !RtcCore.Attached);
            AllSpec.UISpec.SpecUpdated += (o, e) =>
            {
                PartialSpec partial = e.partialSpec;

                LocalNetCoreRouter.Route(CORRUPTCORE, REMOTE_PUSHUISPECUPDATE, partial, e.syncedUpdate);
            };

            RtcCore.StartUISide();

            //Loading RTC Params

            S.GET<RTC_SettingsGeneral_Form>().cbDisableEmulatorOSD.Checked = Params.IsParamSet(RTCSPEC.CORE_EMULATOROSDDISABLED);
            S.GET<RTC_SettingsGeneral_Form>().cbAllowCrossCoreCorruption.Checked = Params.IsParamSet("ALLOW_CROSS_CORE_CORRUPTION");
            S.GET<RTC_SettingsGeneral_Form>().cbDontCleanAtQuit.Checked = Params.IsParamSet("DONT_CLEAN_SAVESTATES_AT_QUIT");
            S.GET<RTC_SettingsGeneral_Form>().cbUncapIntensity.Checked = Params.IsParamSet("UNCAP_INTENSITY");

            //Initialize input code. Poll every 16ms
            Input.Input.Initialize();
            inputCheckTimer = new System.Timers.Timer();
            inputCheckTimer.Elapsed += ProcessInputCheck;
            inputCheckTimer.AutoReset = false;
            inputCheckTimer.Interval = 10;
            inputCheckTimer.Start();

            if (FirstConnect)
            {
                UI_DefaultGrids.connectionStatus.LoadToMain();
            }

            Colors.LoadRTCColor();
            S.GET<UI_CoreForm>().Show();
            Initialized.Set();
            Colors.LoadRTCColor();
        }

        private static void FormRegister_FormRegistered(object sender, FormRegisteredEventArgs e)
        {
            Form newForm = e.Form;

            if (newForm != null)
            {
                registerFormEvents(newForm);
                registerHotkeyBlacklistControls(newForm);
            }
        }

        public static void registerHotkeyBlacklistControls(Control container)
        {
            foreach (Control c in container.Controls)
            {
                registerHotkeyBlacklistControls(c);
                if (c is NumericUpDown || c is TextBox)
                {
                    c.Enter -= ControlFocusObtained;
                    c.Enter += ControlFocusObtained;

                    c.Leave -= ControlFocusLost;
                    c.Leave += ControlFocusLost;
                }
                else if (c is DataGridView dgv)
                {
                    dgv.CellBeginEdit -= ControlFocusObtained;
                    dgv.CellBeginEdit += ControlFocusObtained;

                    dgv.CellEndEdit -= ControlFocusLost;
                    dgv.CellEndEdit += ControlFocusLost;
                }
            }
        }

        public static void registerFormEvents(Form f)
        {
            f.Deactivate -= NewForm_FocusChanged;
            f.Deactivate += NewForm_FocusChanged;

            f.Activated -= NewForm_FocusChanged;
            f.Activated += NewForm_FocusChanged;

            f.GotFocus -= NewForm_FocusChanged;
            f.GotFocus += NewForm_FocusChanged;

            f.LostFocus -= NewForm_FocusChanged;
            f.LostFocus += NewForm_FocusChanged;

            //There's a chance that the form may already be visible by the time this fires
            if (f.Focused)
            {
                UpdateFormFocusStatus(true);
            }
        }

        private static void ControlFocusObtained(object sender, EventArgs e) => UpdateFormFocusStatus(false);
        private static void ControlFocusLost(object sender, EventArgs e) => UpdateFormFocusStatus(true);

        private static void NewForm_FocusChanged(object sender, EventArgs e)
        {
            ((Control)sender).TabIndex = 0;
            UpdateFormFocusStatus(null);
        }

        public static void UpdateFormFocusStatus(bool? forceSet = null)
        {
            if (AllSpec.UISpec == null)
            {
                return;
            }

            bool previousState = (bool?)AllSpec.UISpec[RTC_INFOCUS] ?? false;
            //bool currentState = forceSet ?? isAnyRTCFormFocused();
            bool currentState = (Form.ActiveForm != null && forceSet == null) || (forceSet ?? false);

            if (previousState != currentState)
            {
                logger.Trace($"Swapping focus state {previousState} => {currentState}");
                //This is a non-synced spec update to prevent jittering. Shouldn't have any other noticeable impact
                AllSpec.UISpec.Update(RTC_INFOCUS, currentState, true, false);
            }
        }

        public static void LockInterface(bool focusCoreForm = true, bool blockMainForm = false)
        {
            if (interfaceLocked || lockPending)
            {
                return;
            }

            lockPending = true;
            lock (lockObject)
            {
                //Kill hotkeys while locked
                SetHotkeyTimer(false);

                interfaceLocked = true;
                var cf = S.GET<UI_CoreForm>();
                cf.LockSideBar();

                S.GET<RTC_ConnectionStatus_Form>().pnBlockedButtons.Show();

                if (blockMainForm)
                {
                    UI_CanvasForm.mainForm.BlockView();
                }

                UI_CanvasForm.extraForms.ForEach(it => it.BlockView());

                var ifs = S.GETINTERFACES<IBlockable>();

                foreach (var i in ifs)
                {
                    i.BlockView();
                }

                if (focusCoreForm)
                {
                    cf.Focus();
                }
            }
            lockPending = false;
        }

        public static void UnlockInterface()
        {
            if (lockPending)
            {
                lockPending = false;
            }

            lock (lockObject)
            {
                interfaceLocked = false;
                S.GET<UI_CoreForm>().UnlockSideBar();

                S.GET<RTC_ConnectionStatus_Form>().pnBlockedButtons.Hide();

                UI_CanvasForm.mainForm.UnblockView();
                UI_CanvasForm.extraForms.ForEach(it => it.UnblockView());
                var ifs = S.GETINTERFACES<IBlockable>();
                foreach (var i in ifs)
                {
                    i.UnblockView();
                }

                //Resume hotkeys
                SetHotkeyTimer(true);
            }
        }

        public static void SetHotkeyTimer(bool enable)
        {
            inputCheckTimer.Enabled = enable;
            Input.Input.Instance.ClearEvents();
        }

        public static void BlockView(this IBlockable ib)
        {
            if (ib is RTC_ConnectionStatus_Form)
            {
                return;
            }

            if (ib.blockPanel == null)
            {
                ib.blockPanel = new Panel();
            }

            if (ib is Control c)
            {
                c.Controls.Add(ib.blockPanel);
                ib.blockPanel.Location = new Point(0, 0);
                ib.blockPanel.Size = c.Size;
                ib.blockPanel.BringToFront();

                var bmp = c.getFormScreenShot();
                bmp.Tint(Color.FromArgb(0xCC, Colors.Dark4Color));

                ib.blockPanel.BackgroundImage = bmp;
            }

            ib.blockPanel.Visible = true;
        }

        public static void UnblockView(this IBlockable ib)
        {
            if (ib is RTC_ConnectionStatus_Form)
            {
                return;
            }

            if (ib.blockPanel != null)
            {
                ib.blockPanel.Visible = false;
            }
        }

        //All RTC forms
        public static Form[] AllColorizedSingletons(Type baseType = null)
        {
            if (baseType == null)
            {
                baseType = typeof(UI_CoreForm);
            }
            //This fetches all singletons interface IAutoColorized

            List<Form> all = new List<Form>();
            foreach (Type t in Assembly.GetAssembly(baseType).GetTypes())
            {
                if (typeof(IAutoColorize).IsAssignableFrom(t) && t != typeof(IAutoColorize))
                {
                    all.Add((Form)S.GET(Type.GetType(t.ToString())));
                }
            }

            return all.ToArray();
        }

        public static volatile bool isClosing = false;

        public static void CloseAllRtcForms() //This allows every form to get closed to prevent RTC from hanging
        {
            if (isClosing)
            {
                return;
            }

            isClosing = true;

            foreach (Form frm in AllColorizedSingletons())
            {
                if (frm != null)
                {
                    frm.Close();
                }
            }

            if (S.GET<Forms.RTC_Standalone_Form>() != null)
            {
                S.GET<Forms.RTC_Standalone_Form>().Close();
            }

            //Clean out the working folders
            if (!RtcCore.DontCleanSavestatesOnQuit)
            {
                Stockpile.EmptyFolder("WORKING");
            }

            Environment.Exit(0);
        }

        private static bool interfaceLocked;
        private static bool lockPending;
        private static object lockObject = new object();

        public static object InputLock = new object();
        //Borrowed from Bizhawk. Thanks guys
        private static void ProcessInputCheck(object o, ElapsedEventArgs e)
        {
            try
            {
                lock (InputLock)
                {
                    while (true)
                    {
                        Input.Input.Update();
                        // loop through all available events
                        var ie = Input.Input.Instance.DequeueEvent();
                        if (ie == null)
                        {
                            break;
                        }

                        // useful debugging:
                        //Console.WriteLine(ie);

                        // look for hotkey bindings for this key
                        var triggers = Bindings.SearchBindings(ie.LogicalButton.ToString());

                        bool handled = false;
                        if (ie.EventType == InputEventType.Press)
                        {
                            triggers.Aggregate(handled, (current, trigger) => current | CheckHotkey(trigger));
                        }
                    } // foreach event
                }
            }
            finally
            {
                inputCheckTimer.Start();
            }
        }

        private static bool CheckHotkey(string trigger)
        {
            logger.Info("Hotkey {trigger} pressed", trigger);
            switch (trigger)
            {
                case "Manual Blast":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<UI_CoreForm>().btnManualBlast_Click(null, null);
                    });
                    break;

                case "Auto-Corrupt":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<UI_CoreForm>().btnAutoCorrupt_Click(null, null);
                    });
                    break;

                case "Error Delay--":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        if (S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Value > 1)
                        {
                            S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Value--;
                        }
                    });
                    break;

                case "Error Delay++":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        if (S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Value < S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Maximum)
                        {
                            S.GET<GeneralParametersForm>().multiTB_ErrorDelay.Value++;
                        }
                    });
                    break;

                case "Intensity--":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        if (S.GET<GeneralParametersForm>().multiTB_Intensity.Value > 1)
                        {
                            S.GET<GeneralParametersForm>().multiTB_Intensity.Value--;
                        }
                    });
                    break;

                case "Intensity++":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        if (S.GET<GeneralParametersForm>().multiTB_Intensity.Value < S.GET<GeneralParametersForm>().multiTB_Intensity.Maximum)
                        {
                            S.GET<GeneralParametersForm>().multiTB_Intensity.Value++;
                        }
                    });
                    break;

                case "Load and Corrupt":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<RTC_GlitchHarvesterBlast_Form>().loadBeforeOperation = true;
                        S.GET<RTC_GlitchHarvesterBlast_Form>().btnCorrupt_Click(null, null);
                    });
                    break;

                case "Just Corrupt":
                    AllSpec.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE, true);

                    SyncObjectSingleton.FormExecute(() =>
                    {
                        bool isload = S.GET<RTC_GlitchHarvesterBlast_Form>().loadBeforeOperation;
                        S.GET<RTC_GlitchHarvesterBlast_Form>().loadBeforeOperation = false;
                        S.GET<RTC_GlitchHarvesterBlast_Form>().btnCorrupt_Click(null, null);
                        S.GET<RTC_GlitchHarvesterBlast_Form>().loadBeforeOperation = isload;
                    });
                    break;

                case "Reload Corruption":

                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var sh = S.GET<RTC_StashHistory_Form>();
                        var sm = S.GET<RTC_StockpileManager_Form>();
                        var ghb = S.GET<RTC_GlitchHarvesterBlast_Form>();

                        if (sh.lbStashHistory.SelectedIndex != -1)
                            sh.lbStashHistory_SelectedIndexChanged(null, null);
                        else
                        {
                            var rows = sm.dgvStockpile.SelectedRows;
                            var mainRow = rows[0];

                            if (rows.Count > 1)
                            {
                                ghb.btnCorrupt_Click(null, null);
                            }
                            else
                            {
                                sm.dgvStockpile_CellClick(sm.dgvStockpile, new DataGridViewCellEventArgs(0, mainRow.Index));
                            }
                        }
                    });

                    break;

                case "Reroll":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<RTC_GlitchHarvesterBlast_Form>().btnRerollSelected_Click(null, null);
                    });
                    break;

                case "Load":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<SavestateManagerForm>().savestateList.btnSaveLoad.Text = "LOAD";
                        S.GET<SavestateManagerForm>().savestateList.btnSaveLoad_Click(null, null);
                    });
                    break;

                case "Save":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<SavestateManagerForm>().savestateList.btnSaveLoad.Text = "SAVE";
                        S.GET<SavestateManagerForm>().savestateList.btnSaveLoad_Click(null, null);
                    });
                    break;

                case "Stash->Stockpile":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<RTC_StashHistory_Form>().AddStashToStockpile(false);
                    });
                    break;

                case "Induce KS Crash":
                    AutoKillSwitch.KillEmulator(true);
                    break;

                case "Blast+RawStash":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        AllSpec.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE, true);
                        S.GET<UI_CoreForm>().btnManualBlast_Click(null, null);
                        S.GET<RTC_GlitchHarvesterBlast_Form>().btnSendRaw_Click(null, null);
                    });
                    break;

                case "Send Raw to Stash":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<RTC_GlitchHarvesterBlast_Form>().btnSendRaw_Click(null, null);
                    });
                    break;

                case "BlastLayer Toggle":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<RTC_GlitchHarvesterBlast_Form>().btnBlastToggle_Click(null, null);
                    });
                    break;

                case "BlastLayer Re-Blast":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        if (StockpileManager_UISide.CurrentStashkey == null || StockpileManager_UISide.CurrentStashkey.BlastLayer.Layer.Count == 0)
                        {
                            S.GET<RTC_GlitchHarvesterBlast_Form>().IsCorruptionApplied = false;
                            return;
                        }
                        S.GET<RTC_GlitchHarvesterBlast_Form>().IsCorruptionApplied = true;
                        StockpileManager_UISide.ApplyStashkey(StockpileManager_UISide.CurrentStashkey, false);
                    });
                    break;

                case "Game Protect Back":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var f = S.GET<UI_CoreForm>();
                        var b = f.btnGpJumpBack;
                        if (b.Visible && b.Enabled)
                        {
                            f.btnGpJumpBack_Click(null, null);
                        }
                    });
                    break;

                case "Game Protect Now":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var f = S.GET<UI_CoreForm>();
                        var b = f.btnGpJumpNow;
                        if (b.Visible && b.Enabled)
                        {
                            f.btnGpJumpNow_Click(null, null);
                        }
                    });
                    break;
                case "Disable 50":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.btnDisable50_Click(null, null);
                        }
                    });
                    break;

                case "Remove Disabled":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.btnRemoveDisabled_Click(null, null);
                        }
                    });
                    break;

                case "Invert Disabled":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.btnInvertDisabled_Click(null, null);
                        }
                    });
                    break;
                case "Shift Up":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.btnShiftBlastLayerUp_Click(null, null);
                        }
                    });
                    break;
                case "Shift Down":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.btnShiftBlastLayerDown_Click(null, null);
                        }
                    });
                    break;
                case "Load Corrupt":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && bef.Focused)
                        {
                            bef.btnLoadCorrupt_Click(null, null);
                        }
                    });
                    break;
                case "Apply":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && bef.Focused)
                        {
                            bef.btnCorrupt_Click(null, null);
                        }
                    });
                    break;
                case "Send Stash":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.btnSendToStash_Click(null, null);
                        }
                    });
                    break;
            }

            return true;
        }

        /// <summary>
        /// Disables things in the UI that aren't supported by the current spec.
        /// </summary>
        public static void ConfigureUIFromVanguardSpec()
        {
            if ((AllSpec.VanguardSpec[VSPEC.SUPPORTS_REALTIME] as bool?) ?? false)
            {
                S.GET<UI_CoreForm>().btnManualBlast.Visible = true;
                S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = true;
            }
            else
            {
                if (AllSpec.VanguardSpec[VSPEC.REPLACE_MANUALBLAST_WITH_GHCORRUPT] == null)
                {
                    S.GET<UI_CoreForm>().btnManualBlast.Visible = false;
                }
                else
                {
                    S.GET<UI_CoreForm>().btnManualBlast.Visible = true;
                }

                S.GET<UI_CoreForm>().btnAutoCorrupt.Visible = false;
            }
        }

        private static void toggleLimiterBoxSource(bool setToBindingSource)
        {
            if (setToBindingSource)
            {
                S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.DisplayMember = "Name";
                S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.ValueMember = "Value";
                S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.DataSource = RtcCore.LimiterListBindingSource;

                S.GET<RTC_CustomEngineConfig_Form>().cbValueList.DisplayMember = "Name";
                S.GET<RTC_CustomEngineConfig_Form>().cbValueList.ValueMember = "Value";
                S.GET<RTC_CustomEngineConfig_Form>().cbValueList.DataSource = RtcCore.ValueListBindingSource;

                S.GET<CorruptionEngineForm>().cbVectorLimiterList.DisplayMember = "Name";
                S.GET<CorruptionEngineForm>().cbVectorLimiterList.ValueMember = "Value";
                S.GET<CorruptionEngineForm>().cbVectorLimiterList.DataSource = RtcCore.LimiterListBindingSource;

                S.GET<CorruptionEngineForm>().cbVectorValueList.DisplayMember = "Name";
                S.GET<CorruptionEngineForm>().cbVectorValueList.ValueMember = "Value";
                S.GET<CorruptionEngineForm>().cbVectorValueList.DataSource = RtcCore.ValueListBindingSource;
            }
            else
            {
                S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.DataSource = null;
                S.GET<RTC_CustomEngineConfig_Form>().cbValueList.DataSource = null;

                S.GET<CorruptionEngineForm>().cbVectorLimiterList.DataSource = null;
                S.GET<CorruptionEngineForm>().cbVectorValueList.DataSource = null;
            }
        }

        public static void LoadLists(string dir)
        {
            if (!Directory.Exists(dir))
            {
                return;
            }


            //x.Substring(x.LastIndexOf('\\')+1)[0] != '$'
            //checks if first char is $

            string[] paths = Directory.GetFiles(dir).Where(x => x.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) && x.Substring(x.LastIndexOf('\\') + 1)[0] != '$').ToArray();
            paths = paths.OrderBy(x => x).ToArray();

            List<string> hashes = Filtering.LoadListsFromPaths(paths);
            toggleLimiterBoxSource(false);
            foreach (var hash in hashes)
            {
                Filtering.RegisterListInUI(Filtering.Hash2NameDico[hash], hash);
            }

            toggleLimiterBoxSource(true);
        }
    }
}
