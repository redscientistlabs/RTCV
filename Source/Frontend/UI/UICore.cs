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
    using RTCV.NetCore.Commands;

    public static class UICore
    {
        internal static bool FirstConnect = true;
        internal static ManualResetEvent Initialized = new ManualResetEvent(false);
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static System.Timers.Timer inputCheckTimer;

        //RTC Main Forms
        private static SelectBoxForm _mtForm = null;
        public static SelectBoxForm mtForm //keep public for autoextendability for plugins
        {
            get
            {
                if (_mtForm == null)
                    _mtForm = DefaultGrids.DefaultTools;

                return _mtForm;
            }
            set { _mtForm = value; }
        }

        public static BindingCollection HotkeyBindings = new BindingCollection();

        public static void Start(Form standaloneForm = null)
        {
            S.formRegister.FormRegistered += FormRegister_FormRegistered;
            //registerFormEvents(S.GET<RTC_Core_Form>());
            registerFormEvents(S.GET<CoreForm>());
            registerHotkeyBlacklistControls(S.GET<CoreForm>());

            if (!RtcCore.Attached)
            {
                S.SET((Forms.StandaloneForm)standaloneForm);
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

                LocalNetCoreRouter.Route(Endpoints.CorruptCore, Remote.PushUISpecUpdate, partial, e.SyncedUpdate);
            };

            RtcCore.StartUISide();

            //Loading RTC Params

            S.GET<SettingsGeneralForm>().cbDisableEmulatorOSD.Checked = Params.IsParamSet(RTCSPEC.CORE_EMULATOROSDDISABLED);
            S.GET<SettingsGeneralForm>().cbAllowCrossCoreCorruption.Checked = Params.IsParamSet("ALLOW_CROSS_CORE_CORRUPTION");
            S.GET<SettingsGeneralForm>().cbDontCleanAtQuit.Checked = Params.IsParamSet("DONT_CLEAN_SAVESTATES_AT_QUIT");
            S.GET<SettingsGeneralForm>().cbUncapIntensity.Checked = Params.IsParamSet("UNCAP_INTENSITY");

            //Initialize input code. Poll every 16ms
            Input.Input.Initialize();
            inputCheckTimer = new System.Timers.Timer();
            inputCheckTimer.Elapsed += ProcessInputCheck;
            inputCheckTimer.AutoReset = false;
            inputCheckTimer.Interval = 10;
            inputCheckTimer.Start();

            if (FirstConnect)
            {
                DefaultGrids.connectionStatus.LoadToMain();
            }

            Colors.LoadRTCColor();
            S.GET<CoreForm>().Show();
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

        internal static void registerHotkeyBlacklistControls(Control container)
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

        internal static void registerFormEvents(Form f)
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

            bool previousState = (bool?)AllSpec.UISpec[Basic.RTCInFocus] ?? false;
            //bool currentState = forceSet ?? isAnyRTCFormFocused();
            bool currentState = (Form.ActiveForm != null && forceSet == null) || (forceSet ?? false);

            if (previousState != currentState)
            {
                logger.Trace($"Swapping focus state {previousState} => {currentState}");
                //This is a non-synced spec update to prevent jittering. Shouldn't have any other noticeable impact
                AllSpec.UISpec.Update(Basic.RTCInFocus, currentState, true, false);
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
                var cf = S.GET<CoreForm>();
                cf.LockSideBar();

                S.GET<ConnectionStatusForm>().pnBlockedButtons.Show();

                if (blockMainForm)
                {
                    CanvasForm.mainForm.BlockView();
                }

                CanvasForm.extraForms.ForEach(it => it.BlockView());

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
                S.GET<CoreForm>().UnlockSideBar();

                S.GET<ConnectionStatusForm>().pnBlockedButtons.Hide();

                CanvasForm.mainForm.UnblockView();
                CanvasForm.extraForms.ForEach(it => it.UnblockView());
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

        internal static void BlockView(this IBlockable ib)
        {
            if (ib is ConnectionStatusForm)
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

        internal static void UnblockView(this IBlockable ib)
        {
            if (ib is ConnectionStatusForm)
            {
                return;
            }

            if (ib.blockPanel != null)
            {
                ib.blockPanel.Visible = false;
            }
        }

        internal static volatile bool isClosing = false;
        private static bool interfaceLocked;
        private static bool lockPending;
        private static object lockObject = new object();

        internal static object InputLock = new object();
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

        internal static bool CheckHotkey(string trigger)
        {
            logger.Info("Hotkey {trigger} pressed", trigger);
            switch (trigger)
            {
                case "Manual Blast":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<CoreForm>().ManualBlast(null, null);
                    });
                    break;

                case "Auto-Corrupt":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<CoreForm>().StartAutoCorrupt(null, null);
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
                        S.GET<GlitchHarvesterBlastForm>().loadBeforeOperation = true;
                        S.GET<GlitchHarvesterBlastForm>().Corrupt(null, null);
                    });
                    break;

                case "Just Corrupt":
                    AllSpec.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE, true);

                    SyncObjectSingleton.FormExecute(() =>
                    {
                        bool isload = S.GET<GlitchHarvesterBlastForm>().loadBeforeOperation;
                        S.GET<GlitchHarvesterBlastForm>().loadBeforeOperation = false;
                        S.GET<GlitchHarvesterBlastForm>().Corrupt(null, null);
                        S.GET<GlitchHarvesterBlastForm>().loadBeforeOperation = isload;
                    });
                    break;

                case "New Savestate":

                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<SavestateManagerForm>().savestateList.NewSavestateNow();
                    });
                    break;

                case "Load Prev State":

                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<SavestateManagerForm>().savestateList.LoadPreviousSavestateNow();
                    });
                    break;

                case "Reload Corruption":

                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var sh = S.GET<StashHistoryForm>();
                        var sm = S.GET<StockpileManagerForm>();
                        var ghb = S.GET<GlitchHarvesterBlastForm>();

                        if (sh.lbStashHistory.SelectedIndex != -1)
                        {
                            sh.HandleStashHistorySelectionChange(null, null);
                        }
                        else
                        {
                            var rows = sm.dgvStockpile.SelectedRows;
                            var mainRow = rows[0];

                            if (rows.Count > 1)
                            {
                                ghb.Corrupt(null, null);
                            }
                            else
                            {
                                sm.HandleCellClick(sm.dgvStockpile, new DataGridViewCellEventArgs(0, mainRow.Index));
                            }
                        }
                    });

                    break;

                case "Reroll":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<GlitchHarvesterBlastForm>().RerollSelected(null, null);
                    });
                    break;

                case "Load":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<SavestateManagerForm>().savestateList.btnSaveLoad.Text = "LOAD";
                        S.GET<SavestateManagerForm>().savestateList.HandleSaveLoadClick(null, null);
                    });
                    break;

                case "Save":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<SavestateManagerForm>().savestateList.btnSaveLoad.Text = "SAVE";
                        S.GET<SavestateManagerForm>().savestateList.HandleSaveLoadClick(null, null);
                    });
                    break;

                case "Stash->Stockpile":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<StashHistoryForm>().AddStashToStockpile(false);
                    });
                    break;

                case "Induce KS Crash":
                    AutoKillSwitch.KillEmulator(true);
                    break;

                case "Blast+RawStash":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        AllSpec.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE, true);
                        S.GET<CoreForm>().ManualBlast(null, null);
                        S.GET<GlitchHarvesterBlastForm>().SendRawToStash(null, null);
                    });
                    break;

                case "Send Raw to Stash":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<GlitchHarvesterBlastForm>().SendRawToStash(null, null);
                    });
                    break;

                case "BlastLayer Toggle":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<GlitchHarvesterBlastForm>().BlastLayerToggle(null, null);
                    });
                    break;

                case "BlastLayer Re-Blast":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        if (StockpileManagerUISide.CurrentStashkey == null || StockpileManagerUISide.CurrentStashkey.BlastLayer.Layer.Count == 0)
                        {
                            S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = false;
                            return;
                        }
                        S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = true;
                        StockpileManagerUISide.ApplyStashkey(StockpileManagerUISide.CurrentStashkey, false);
                    });
                    break;

                case "Game Protect Back":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var f = S.GET<CoreForm>();
                        var b = f.btnGpJumpBack;
                        if (b.Visible && b.Enabled)
                        {
                            f.OnGameProtectionBack(null, null);
                        }
                    });
                    break;

                case "Game Protect Now":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var f = S.GET<CoreForm>();
                        var b = f.btnGpJumpNow;
                        if (b.Visible && b.Enabled)
                        {
                            f.OnGameProtectionNow(null, null);
                        }
                    });
                    break;
                case "Disable 50":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<BlastEditorForm>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.Disable50(null, null);
                        }
                    });
                    break;

                case "Remove Disabled":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<BlastEditorForm>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.RemoveDisabled(null, null);
                        }
                    });
                    break;

                case "Invert Disabled":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<BlastEditorForm>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.InvertDisabled(null, null);
                        }
                    });
                    break;
                case "Shift Up":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<BlastEditorForm>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.ShiftBlastLayerUp(null, null);
                        }
                    });
                    break;
                case "Shift Down":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<BlastEditorForm>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.ShiftBlastLayerDown(null, null);
                        }
                    });
                    break;
                case "Load Corrupt":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<BlastEditorForm>();
                        if (bef != null && bef.Focused)
                        {
                            bef.LoadCorrupt(null, null);
                        }
                    });
                    break;
                case "Apply":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<BlastEditorForm>();
                        if (bef != null && bef.Focused)
                        {
                            bef.Corrupt(null, null);
                        }
                    });
                    break;
                case "Send Stash":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        var bef = S.GET<BlastEditorForm>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.SendToStash(null, null);
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
                S.GET<CoreForm>().btnManualBlast.Visible = true;
                S.GET<CoreForm>().btnAutoCorrupt.Visible = true;
            }
            else
            {
                if (AllSpec.VanguardSpec[VSPEC.REPLACE_MANUALBLAST_WITH_GHCORRUPT] == null)
                {
                    S.GET<CoreForm>().btnManualBlast.Visible = false;
                }
                else
                {
                    S.GET<CoreForm>().btnManualBlast.Visible = true;
                }

                S.GET<CoreForm>().btnAutoCorrupt.Visible = false;
            }
        }

        private static void toggleLimiterBoxSource(bool setToBindingSource)
        {
            if (setToBindingSource)
            {
                S.GET<CustomEngineConfigForm>().cbLimiterList.DisplayMember = "Name";
                S.GET<CustomEngineConfigForm>().cbLimiterList.ValueMember = "Value";
                S.GET<CustomEngineConfigForm>().cbLimiterList.DataSource = RtcCore.LimiterListBindingSource;

                S.GET<CustomEngineConfigForm>().cbValueList.DisplayMember = "Name";
                S.GET<CustomEngineConfigForm>().cbValueList.ValueMember = "Value";
                S.GET<CustomEngineConfigForm>().cbValueList.DataSource = RtcCore.ValueListBindingSource;

                S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorLimiterList.DisplayMember = "Name";
                S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorLimiterList.ValueMember = "Value";
                S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorLimiterList.DataSource = RtcCore.LimiterListBindingSource;

                S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorValueList.DisplayMember = "Name";
                S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorValueList.ValueMember = "Value";
                S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorValueList.DataSource = RtcCore.ValueListBindingSource;

                S.GET<CorruptionEngineForm>().ClusterEngineControl.cbClusterLimiterList.DisplayMember = "Name";
                S.GET<CorruptionEngineForm>().ClusterEngineControl.cbClusterLimiterList.ValueMember = "Value";
                S.GET<CorruptionEngineForm>().ClusterEngineControl.cbClusterLimiterList.DataSource = RtcCore.LimiterListBindingSource;
            }
            else
            {
                S.GET<CustomEngineConfigForm>().cbLimiterList.DataSource = null;
                S.GET<CustomEngineConfigForm>().cbValueList.DataSource = null;

                S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorLimiterList.DataSource = null;
                S.GET<CorruptionEngineForm>().VectorEngineControl.cbVectorValueList.DataSource = null;

                S.GET<CorruptionEngineForm>().ClusterEngineControl.cbClusterLimiterList.DataSource = null;
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
