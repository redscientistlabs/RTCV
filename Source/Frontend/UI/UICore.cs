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
    using RTCV.UI.Input;
    using RTCV.UI.Modular;
    using static RTCV.NetCore.NetcoreCommands;
    using static RTCV.UI.UI_Extensions;

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
        //public static Color generalColor = Color.FromArgb(60, 45, 70);
        public static Color GeneralColor = Color.LightSteelBlue;

        public static RTC_SelectBox_Form mtForm = null;
        public static BindingCollection HotkeyBindings = new BindingCollection();

        public static void Start(Form standaloneForm = null)
        {
            S.formRegister.FormRegistered += FormRegister_FormRegistered;
            //registerFormEvents(S.GET<RTC_Core_Form>());
            registerFormEvents(S.GET<UI_CoreForm>());
            registerHotkeyBlacklistControls(S.GET<UI_CoreForm>());

            if (!RtcCore.Attached)
            {
                S.SET((RTC_Standalone_Form)standaloneForm);
            }

            Form dummy = new Form();
            IntPtr Handle = dummy.Handle;

            SyncObjectSingleton.SyncObject = dummy;

            UI_VanguardImplementation.StartServer();

            PartialSpec p = new PartialSpec("UISpec");

            p["SELECTEDDOMAINS"] = new string[] { };

            RTCV.NetCore.AllSpec.UISpec = new FullSpec(p, !CorruptCore.RtcCore.Attached);
            RTCV.NetCore.AllSpec.UISpec.SpecUpdated += (o, e) =>
            {
                PartialSpec partial = e.partialSpec;

                LocalNetCoreRouter.Route(CORRUPTCORE, REMOTE_PUSHUISPECUPDATE, partial, e.syncedUpdate);
            };

            CorruptCore.RtcCore.StartUISide();

            //Loading RTC Params

            S.GET<RTC_SettingsGeneral_Form>().cbDisableEmulatorOSD.Checked = RTCV.NetCore.Params.IsParamSet(RTCSPEC.CORE_EMULATOROSDDISABLED);
            S.GET<RTC_SettingsGeneral_Form>().cbAllowCrossCoreCorruption.Checked = RTCV.NetCore.Params.IsParamSet("ALLOW_CROSS_CORE_CORRUPTION");
            S.GET<RTC_SettingsGeneral_Form>().cbDontCleanAtQuit.Checked = RTCV.NetCore.Params.IsParamSet("DONT_CLEAN_SAVESTATES_AT_QUIT");
            S.GET<RTC_SettingsGeneral_Form>().cbUncapIntensity.Checked = RTCV.NetCore.Params.IsParamSet("UNCAP_INTENSITY");

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

            LoadRTCColor();
            S.GET<UI_CoreForm>().Show();
            Initialized.Set();
            LoadRTCColor();
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
            if (RTCV.NetCore.AllSpec.UISpec == null)
            {
                return;
            }

            bool previousState = (bool?)RTCV.NetCore.AllSpec.UISpec[RTC_INFOCUS] ?? false;
            //bool currentState = forceSet ?? isAnyRTCFormFocused();
            bool currentState = (Form.ActiveForm != null && forceSet == null) || (forceSet ?? false);

            if (previousState != currentState)
            {
                logger.Trace($"Swapping focus state {previousState} => {currentState}");
                //This is a non-synced spec update to prevent jittering. Shouldn't have any other noticeable impact
                RTCV.NetCore.AllSpec.UISpec.Update(RTC_INFOCUS, currentState, true, false);
            }
        }

        private static bool isAnyRTCFormFocused()
        {
            bool ExternalForm = Form.ActiveForm == null;

            if (ExternalForm)
            {
                return false;
            }

            var form = Form.ActiveForm;
            var t = form.GetType();

            focus = (
                typeof(IAutoColorize).IsAssignableFrom(t) ||
                typeof(CloudDebug).IsAssignableFrom(t) ||
                typeof(DebugInfo_Form).IsAssignableFrom(t)
            );

            bool isAllowedForm = focus;

            return isAllowedForm;
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
                bmp.Tint(Color.FromArgb(0xCC, UICore.Dark4Color));

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
                baseType = typeof(RTCV.UI.UI_CoreForm);
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
        private static bool focus;

        public static void CloseAllRtcForms() //This allows every form to get closed to prevent RTC from hanging
        {
            if (isClosing)
            {
                return;
            }

            isClosing = true;

            foreach (Form frm in UICore.AllColorizedSingletons())
            {
                if (frm != null)
                {
                    frm.Close();
                }
            }

            if (S.GET<RTC_Standalone_Form>() != null)
            {
                S.GET<RTC_Standalone_Form>().Close();
            }

            //Clean out the working folders
            if (!CorruptCore.RtcCore.DontCleanSavestatesOnQuit)
            {
                Stockpile.EmptyFolder("WORKING");
            }

            Environment.Exit(0);
        }

        public static Color Light1Color;
        public static Color Light2Color;
        public static Color NormalColor;
        public static Color Dark1Color;
        public static Color Dark2Color;
        public static Color Dark3Color;
        public static Color Dark4Color;
        private static bool interfaceLocked;
        private static bool lockPending;
        private static object lockObject = new object();


        public static void SetRTCColor(Color color, Control ctr = null)
        {
            HashSet<Control> allControls = new HashSet<Control>();

            if (ctr == null)
            {
                foreach (Form targetForm in UICore.AllColorizedSingletons())
                {
                    if (targetForm != null)
                    {
                        foreach (var c in targetForm.Controls.getControlsWithTag())
                            allControls.Add(c);
                        allControls.Add(targetForm);
                    }
                }

                //Get the extraforms
                foreach (UI_CanvasForm targetForm in UI_CanvasForm.extraForms)
                {
                    foreach (var c in targetForm.Controls.getControlsWithTag())
                        allControls.Add(c);
                    allControls.Add(targetForm);
                }

                //We have to manually add the etform because it's not singleton, not an extraForm, and not owned by any specific form
                //Todo - Refactor this so we don't need to add it separately
                if (mtForm != null)
                {
                    foreach (var c in mtForm.Controls.getControlsWithTag())
                        allControls.Add(c);
                    allControls.Add(mtForm);
                }
            }
            else if (ctr is Form || ctr is UserControl)
            {
                foreach (var c in ctr.Controls.getControlsWithTag())
                    allControls.Add(c);
                allControls.Add(ctr);
            }
            else if (ctr is Form)
            {
                allControls.Add(ctr);
            }

            float generalDarken = -0.50f;
            float light1 = 0.10f;
            float light2 = 0.45f;
            float dark1 = -0.20f;
            float dark2 = -0.35f;
            float dark3 = -0.50f;
            float dark4 = -0.85f;

            color = color.ChangeColorBrightness(generalDarken);

            Light1Color = color.ChangeColorBrightness(light1);
            Light2Color = color.ChangeColorBrightness(light2);
            NormalColor = color;
            Dark1Color = color.ChangeColorBrightness(dark1);
            Dark2Color = color.ChangeColorBrightness(dark2);
            Dark3Color = color.ChangeColorBrightness(dark3);
            Dark4Color = color.ChangeColorBrightness(dark4);

            var tag2ColorDico = new Dictionary<string, Color>
            {
                { "color:light2", Light2Color },
                { "color:light1", Light1Color },
                { "color:normal", NormalColor },
                { "color:dark1", Dark1Color },
                { "color:dark2", Dark2Color },
                { "color:dark3", Dark3Color },
                { "color:dark4", Dark4Color }
            };

            foreach (var c in allControls)
            {
                var tag = c.Tag?.ToString().Split(' ');

                if (tag == null || tag.Length == 0)
                {
                    continue;
                }

                //Snag the tag and look for the color.
                var ctag = tag.FirstOrDefault(x => x.Contains("color:"));

                //We didn't find a valid color
                if (ctag == null || !tag2ColorDico.TryGetValue(ctag, out Color _color))
                {
                    continue;
                }

                if (c is Label l && l.BackColor != Color.FromArgb(30, 31, 32))
                {
                    c.ForeColor = _color;
                }
                else
                {
                    c.BackColor = _color;
                }

                if (c is Button btn)
                {
                    btn.FlatAppearance.BorderColor = _color;
                }

                if (c is DataGridView dgv)
                {
                    dgv.BackgroundColor = _color;
                }

                c.Invalidate();
            }
        }

        public static void SelectRTCColor()
        {
            // Show the color dialog.
            Color color;
            ColorDialog cd = new ColorDialog();
            DialogResult result = cd.ShowDialog();
            // See if user pressed ok.
            if (result == DialogResult.OK)
            {
                // Set form background to the selected color.
                color = cd.Color;
            }
            else
            {
                return;
            }

            GeneralColor = color;
            SetRTCColor(color);

            SaveRTCColor(color);
        }

        public static void LoadRTCColor()
        {
            if (RTCV.NetCore.Params.IsParamSet("COLOR"))
            {
                string[] bytes = RTCV.NetCore.Params.ReadParam("COLOR").Split(',');
                UICore.GeneralColor = Color.FromArgb(Convert.ToByte(bytes[0]), Convert.ToByte(bytes[1]), Convert.ToByte(bytes[2]));
            }
            else
            {
                UICore.GeneralColor = Color.FromArgb(110, 150, 193);
            }

            UICore.SetRTCColor(UICore.GeneralColor);
        }

        public static void SaveRTCColor(Color color)
        {
            RTCV.NetCore.Params.SetParam("COLOR", color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString());
        }

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
                        Input.Input.Instance.Update();
                        // loop through all available events
                        var ie = Input.Input.Instance.DequeueEvent();
                        if (ie == null)
                        {
                            break;
                        }

                        // useful debugging:
                        //Console.WriteLine(ie);

                        // look for hotkey bindings for this key
                        var triggers = Input.Bindings.SearchBindings(ie.LogicalButton.ToString());

                        bool handled = false;
                        if (ie.EventType == RTCV.UI.Input.Input.InputEventType.Press)
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
                        if (S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value > 1)
                        {
                            S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value--;
                        }
                    });
                    break;

                case "Error Delay++":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        if (S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value < S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Maximum)
                        {
                            S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value++;
                        }
                    });
                    break;

                case "Intensity--":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        if (S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value > 1)
                        {
                            S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value--;
                        }
                    });
                    break;

                case "Intensity++":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        if (S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value < S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Maximum)
                        {
                            S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value++;
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
                    RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE, true);

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
                        S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad.Text = "LOAD";
                        S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad_Click(null, null);
                    });
                    break;

                case "Save":
                    SyncObjectSingleton.FormExecute(() =>
                    {
                        S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad.Text = "SAVE";
                        S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad_Click(null, null);
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
                        RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE, true);
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
                S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.DataSource = CorruptCore.RtcCore.LimiterListBindingSource;

                S.GET<RTC_CustomEngineConfig_Form>().cbValueList.DisplayMember = "Name";
                S.GET<RTC_CustomEngineConfig_Form>().cbValueList.ValueMember = "Value";
                S.GET<RTC_CustomEngineConfig_Form>().cbValueList.DataSource = CorruptCore.RtcCore.ValueListBindingSource;

                S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.DisplayMember = "Name";
                S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.ValueMember = "Value";
                S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.DataSource = CorruptCore.RtcCore.LimiterListBindingSource;

                S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.DisplayMember = "Name";
                S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.ValueMember = "Value";
                S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.DataSource = CorruptCore.RtcCore.ValueListBindingSource;
            }
            else
            {
                S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.DataSource = null;
                S.GET<RTC_CustomEngineConfig_Form>().cbValueList.DataSource = null;

                S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.DataSource = null;
                S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.DataSource = null;
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

            string[] paths = System.IO.Directory.GetFiles(dir).Where(x => x.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) && x.Substring(x.LastIndexOf('\\') + 1)[0] != '$').ToArray();
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
