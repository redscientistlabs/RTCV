using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Newtonsoft.Json;
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.UI;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;
using RTCV.UI.Input;
using static RTCV.NetCore.NetcoreCommands;
using RTCV.UI.Modular;

namespace RTCV.UI
{
	public static class UICore
	{


		//Note Box Settings
		public static Point NoteBoxPosition;
		public static Size NoteBoxSize;

		public static bool FirstConnect = true;

        public static System.Timers.Timer inputCheckTimer;

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

			S.SET<RTC_Standalone_Form>((RTC_Standalone_Form)standaloneForm);

			Form dummy = new Form();
			IntPtr Handle = dummy.Handle;

			SyncObjectSingleton.SyncObject = dummy;

			UI_VanguardImplementation.StartServer();


			PartialSpec p = new PartialSpec("UISpec");

			p["SELECTEDDOMAINS"] = new string[]{};

			RTCV.NetCore.AllSpec.UISpec = new FullSpec(p, !CorruptCore.CorruptCore.Attached);
			RTCV.NetCore.AllSpec.UISpec.SpecUpdated += (o, e) =>
			{
				PartialSpec partial = e.partialSpec;

				LocalNetCoreRouter.Route(CORRUPTCORE, REMOTE_PUSHUISPECUPDATE, partial, e.syncedUpdate);
			};

            CorruptCore.CorruptCore.StartUISide();


            //Loading RTC Params
            
			S.GET<RTC_SettingsGeneral_Form>().cbDisableBizhawkOSD.Checked = !RTCV.NetCore.Params.IsParamSet("ENABLE_BIZHAWK_OSD");
			S.GET<RTC_SettingsGeneral_Form>().cbAllowCrossCoreCorruption.Checked = RTCV.NetCore.Params.IsParamSet("ALLOW_CROSS_CORE_CORRUPTION");
			S.GET<RTC_SettingsGeneral_Form>().cbDontCleanAtQuit.Checked = RTCV.NetCore.Params.IsParamSet("DONT_CLEAN_SAVESTATES_AT_QUIT");
			S.GET<RTC_SettingsGeneral_Form>().cbUncapIntensity.Checked = RTCV.NetCore.Params.IsParamSet("UNCAP_INTENSITY");


            //Initialize input code. Poll every 16ms
            Input.Input.Initialize();
            inputCheckTimer = new System.Timers.Timer();
            inputCheckTimer.Elapsed += ProcessInputCheck;
            inputCheckTimer.Interval = 16;
            inputCheckTimer.Start();


            if (FirstConnect)
            {
                UI_DefaultGrids.connectionStatus.LoadToMain();
            }

            LoadRTCColor();
            S.GET<UI_CoreForm>().Show();
        }

		private static void FormRegister_FormRegistered(object sender, NetCoreEventArgs e)
		{
			Form newForm = ((e.message as NetCoreAdvancedMessage)?.objectValue as Form);

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
			((Control) sender).TabIndex = 0;
			UpdateFormFocusStatus(null);
		}

		public static void UpdateFormFocusStatus(bool? forceSet = null)
		{
			if (RTCV.NetCore.AllSpec.UISpec == null)
				return;

			bool previousState = (bool?)RTCV.NetCore.AllSpec.UISpec[RTC_INFOCUS] ?? false;
            //bool currentState = forceSet ?? isAnyRTCFormFocused();
            bool currentState = forceSet ?? true;
            Console.WriteLine("Setting state to " + currentState);

			if (previousState != currentState)
			{	//This is a non-synced spec update to prevent jittering. Shouldn't have any other noticeable impact
				RTCV.NetCore.AllSpec.UISpec.Update(RTC_INFOCUS, currentState,true,false);
			}

		}

		private static bool isAnyRTCFormFocused()
		{
			bool ExternalForm = Form.ActiveForm == null;

			if (ExternalForm)
				return false;

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


		//All RTC forms
		public static Form[] AllRtcForms
		{
			get
			{
				//This fetches all singletons of interface IAutoColorized

				List<Form> all = new List<Form>();


				foreach (Type t in Assembly.GetAssembly(typeof(RTCV.UI.UI_CoreForm)).GetTypes())
					if (typeof(IAutoColorize).IsAssignableFrom(t) && t != typeof(IAutoColorize))
						all.Add((Form)S.GET(Type.GetType(t.ToString())));

				return all.ToArray();

			}
		}

		public static volatile bool isClosing = false;
		private static bool focus;

		public static void CloseAllRtcForms() //This allows every form to get closed to prevent RTC from hanging
		{
			if (isClosing)
				return;

			isClosing = true;

			foreach (Form frm in UICore.AllRtcForms)
			{
				if (frm != null)
					frm.Close();
			}

			if (S.GET<RTC_Standalone_Form>() != null)
				S.GET<RTC_Standalone_Form>().Close();

			//Clean out the working folders
			if (!CorruptCore.CorruptCore.DontCleanSavestatesOnQuit)
			{
				Stockpile.EmptyFolder(Path.DirectorySeparatorChar + "WORKING" + Path.DirectorySeparatorChar);
			}

			Environment.Exit(-1);
		}

		public static void SetRTCHexadecimal(bool useHex, Form form = null)
		{
			//Sets the interface to use Hex across the board

			List<Control> allControls = new List<Control>();

			if (form == null)
			{
				foreach (Form targetForm in UICore.AllRtcForms)
				{
					if (targetForm != null)
					{
						allControls.AddRange(targetForm.Controls.getControlsWithTag());
						allControls.Add(targetForm);
					}
				}
			}
			else
			{
				allControls.AddRange(form.Controls.getControlsWithTag());
				allControls.Add(form);
			}

			var hexadecimal = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("hex"));

			foreach (NumericUpDownHexFix updown in hexadecimal)
				updown.Hexadecimal = true;

			foreach (DataGridView dgv in hexadecimal)
			foreach (DataGridViewColumn column in dgv.Columns)
			{
				if (column.CellType.Name == "DataGridViewNumericUpDownCell")
				{
					DataGridViewNumericUpDownColumn _column = column as DataGridViewNumericUpDownColumn;
					_column.Hexadecimal = useHex;
				}
			}
		}

		public static void SetRTCColor(Color color, Control ctr = null)
		{
			List<Control> allControls = new List<Control>();

			if (ctr == null)
			{
				foreach (Form targetForm in UICore.AllRtcForms)
				{
					if (targetForm != null)
					{
						allControls.AddRange(targetForm.Controls.getControlsWithTag());
						allControls.Add(targetForm);
					}
				}
			}
			else if (ctr is Form)
			{
				allControls.AddRange(ctr.Controls.getControlsWithTag());
				allControls.Add(ctr);
			}
            else
            {
                allControls.Add(ctr);
            }

            //this needs refactoring. the string contains method is broken as color:dark2 is also color:dark1.
            //at least the priority of the foreach loops makes it so it works like expected.

            var light2ColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:light2"));
            var light1ColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:light1"));
			var normalColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:normal"));
			var dark1ColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:dark1"));
			var dark2ColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:dark2"));
			var dark3ColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:dark3"));

            float generalDarken = -0.50f;
            float light1 = 0.10f;
            float light2 = 0.45f;
            float dark1 = -0.20f;
            float dark2 = -0.35f;
            float dark3 = -0.50f;

            color = color.ChangeColorBrightness(generalDarken);

            Color Light1Color = color.ChangeColorBrightness(light1);
            Color Light2Color = color.ChangeColorBrightness(light2);
            Color NormalColor = color;
            Color Dark1Color = color.ChangeColorBrightness(dark1);
            Color Dark2Color = color.ChangeColorBrightness(dark2);
            Color Dark3Color = color.ChangeColorBrightness(dark3);

            foreach (Control c in light1ColorControls)
            {
                if (c is Label)
                    c.ForeColor = Light1Color;
                else
                    c.BackColor = Light1Color;

                if (c is Button)
                    (c as Button).FlatAppearance.BorderColor = Light1Color;
            }

            foreach (Control c in light2ColorControls)
            {
                
                if (c is Label)
                    c.ForeColor = Light2Color;
                else
                    c.BackColor = Light2Color;

                if (c is Button)
                    (c as Button).FlatAppearance.BorderColor = Light2Color;

            }


            foreach (Control c in normalColorControls)
            {
                if (c is Label)
                    c.ForeColor = NormalColor;
                else
                    c.BackColor = NormalColor;

                if (c is Button)
                    (c as Button).FlatAppearance.BorderColor = NormalColor;
            }

            if(ctr == null)
            {
                S.GET<RTC_StockpilePlayer_Form>().dgvStockpile.BackgroundColor = NormalColor;
                S.GET<RTC_StockpileManager_Form>().dgvStockpile.BackgroundColor = NormalColor;

                S.GET<RTC_NewBlastEditor_Form>().dgvBlastEditor.BackgroundColor = NormalColor;
                S.GET<RTC_BlastGenerator_Form>().dgvBlastGenerator.BackgroundColor = NormalColor;
            }

            foreach (Control c in dark1ColorControls)
            {
                if (c is Label)
                    c.ForeColor = Dark1Color;
                else
                    c.BackColor = Dark1Color;

                if(c is Button)
                    (c as Button).FlatAppearance.BorderColor = Dark1Color;


            }

            foreach (Control c in dark2ColorControls)
            {
                if (c is Label)
                    c.ForeColor = Dark2Color;
                else
                    c.BackColor = Dark2Color;

                if (c is Button)
                    (c as Button).FlatAppearance.BorderColor = Dark2Color;
            }

            foreach (Control c in dark3ColorControls)
            {
                if (c is Label)
                    c.ForeColor = Dark3Color;
                else
                    c.BackColor = Dark3Color;

                if (c is Button)
                    (c as Button).FlatAppearance.BorderColor = Dark3Color;
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
				return;

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
					UICore.GeneralColor = Color.FromArgb(110, 150, 193);

				UICore.SetRTCColor(UICore.GeneralColor);
		}

		public static void SaveRTCColor(Color color)
		{
			RTCV.NetCore.Params.SetParam("COLOR", color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString());
		}

        //Borrowed from Bizhawk. Thanks guys
        private static void ProcessInputCheck(Object o, ElapsedEventArgs e)
        {
            for (; ; )
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

                /*
                if (triggers.Count == 0)
                {
                    // Maybe it is a system alt-key which hasnt been overridden
                    if (ie.EventType == Input.InputEventType.Press)
                    {
                        if (ie.LogicalButton.Alt && ie.LogicalButton.Button.Length == 1)
                        {
                            var c = ie.LogicalButton.Button.ToLower()[0];
                            if ((c >= 'a' && c <= 'z') || c == ' ')
                            {
                                SendAltKeyChar(c);
                            }
                        }

                        if (ie.LogicalButton.Alt && ie.LogicalButton.Button == "Space")
                        {
                            SendPlainAltKey(32);
                        }
                    }*/

                // TODO - wonder what happens if we pop up something interactive as a response to one of these hotkeys? may need to purge further processing


            } // foreach event

        }


        private static bool CheckHotkey(string trigger)
        {
            switch (trigger)
            {

                case "Manual Blast":
                    LocalNetCoreRouter.Route(CORRUPTCORE, ASYNCBLAST);
                    break;

                case "Auto-Corrupt":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        S.GET<UI_CoreForm>().btnAutoCorrupt_Click(null, null);
                    });
                    break;

                case "Error Delay--":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        	if (S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value > 1)
                        		S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value--;
                    });
                    break;

                case "Error Delay++":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        	if (S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value < S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Maximum)
                        		S.GET<RTC_GeneralParameters_Form>().multiTB_ErrorDelay.Value++;
                    });
                    break;

                case "Intensity--":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        	if (S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value > 1)
                        		S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value--;
                    });
                    break;

                case "Intensity++":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        	if (S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value < S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Maximum)
                        		S.GET<RTC_GeneralParameters_Form>().multiTB_Intensity.Value++;
                    });
                    break;

                case "Load and Corrupt":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        S.GET<RTC_GlitchHarvesterBlast_Form>().loadBeforeOperation = true;
                        S.GET<RTC_GlitchHarvesterBlast_Form>().btnCorrupt_Click(null, null);
                    });
                    break;

                case "Just Corrupt":
                    RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE, true);

                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        bool isload = S.GET<RTC_GlitchHarvesterBlast_Form>().loadBeforeOperation;
                        S.GET<RTC_GlitchHarvesterBlast_Form>().loadBeforeOperation = false;
                        S.GET<RTC_GlitchHarvesterBlast_Form>().btnCorrupt_Click(null, null);
                        S.GET<RTC_GlitchHarvesterBlast_Form>().loadBeforeOperation = isload;
                    });
                    break;

                case "Reroll":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        S.GET<RTC_GlitchHarvesterBlast_Form>().btnRerollSelected_Click(null, null);
                    });
                    break;

                case "Load":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad.Text = "LOAD";
                        S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad_Click(null, null);
                    });
                    break;

                case "Save":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad.Text = "SAVE";
                        S.GET<RTC_SavestateManager_Form>().savestateList.btnSaveLoad_Click(null, null);
                    });
                    break;

                case "Stash->Stockpile":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        S.GET<RTC_StashHistory_Form>().AddStashToStockpile(false);
                    });
                    break;

                case "Induce KS Crash":
                    //
                    break;

                case "Blast+RawStash":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE, true);
                        LocalNetCoreRouter.Route(CORRUPTCORE, ASYNCBLAST, null, true);

                        S.GET<RTC_GlitchHarvesterBlast_Form>().btnSendRaw_Click(null, null);
                    });
                    break;

                case "Send Raw to Stash":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        S.GET<RTC_GlitchHarvesterBlast_Form>().btnSendRaw_Click(null, null);
                    });
                    break;

                case "BlastLayer Toggle":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        S.GET<RTC_GlitchHarvesterBlast_Form>().btnBlastToggle_Click(null, null);
                    });
                    break;

                case "BlastLayer Re-Blast":
                    SyncObjectSingleton.FormExecute((o, ea) =>
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
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        var f = S.GET<UI_CoreForm>();
                        var b = f.btnGpJumpBack;
                        if (b.Visible && b.Enabled)
                            f.btnGpJumpBack_Click(null, null);
                    });
                    break;

                case "Game Protect Now":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        var f = S.GET<UI_CoreForm>();
                        var b = f.btnGpJumpNow;
                        if (b.Visible && b.Enabled)
                            f.btnGpJumpNow_Click(null, null);
                    });
                    break;
                case "Disable 50":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.btnDisable50_Click(null, null);
                        }
                    });
                    break;

                case "Remove Disabled":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.btnRemoveDisabled_Click(null, null);
                        }
                    });
                    break;

                case "Invert Disabled":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.btnInvertDisabled_Click(null, null);
                        }
                    });
                    break;
                case "Shift Up":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.btnShiftBlastLayerUp_Click(null, null);
                        }
                    });
                    break;
                case "Shift Down":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && Form.ActiveForm == bef)
                        {
                            bef.btnShiftBlastLayerDown_Click(null, null);
                        }
                    });
                    break;
                case "Load Corrupt":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && bef.Focused)
                        {
                            bef.btnLoadCorrupt_Click(null, null);
                        }
                    });
                    break;
                case "Apply":
                    SyncObjectSingleton.FormExecute((o, ea) =>
                    {
                        var bef = S.GET<RTC_NewBlastEditor_Form>();
                        if (bef != null && bef.Focused)
                        {
                            bef.btnCorrupt_Click(null, null);
                        }
                    });
                    break;
                case "Send Stash":
                    SyncObjectSingleton.FormExecute((o, ea) =>
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
            //S.GET<RTC_GlitchHarvester_Form>().pnRender.Visible = false;

        }


        private static void toggleLimiterBoxSource(bool setToBindingSource)
        {
            if (setToBindingSource)
            {
                S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.DisplayMember = "Name";
                S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.ValueMember = "Value";
                S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.DataSource = CorruptCore.CorruptCore.LimiterListBindingSource;


                S.GET<RTC_CustomEngineConfig_Form>().cbValueList.DisplayMember = "Name";
                S.GET<RTC_CustomEngineConfig_Form>().cbValueList.ValueMember = "Value";
                S.GET<RTC_CustomEngineConfig_Form>().cbValueList.DataSource = CorruptCore.CorruptCore.ValueListBindingSource;



                S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.DisplayMember = "Name";
                S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.ValueMember = "Value";
                S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.DataSource = CorruptCore.CorruptCore.LimiterListBindingSource;

                S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.DisplayMember = "Name";
                S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.ValueMember = "Value";
                S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.DataSource = CorruptCore.CorruptCore.ValueListBindingSource;
            }
            else
            {
                S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.DataSource = null;
                S.GET<RTC_CustomEngineConfig_Form>().cbValueList.DataSource = null;

                S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.DataSource = null;
                S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.DataSource = null;
            }
        }

        public static void LoadLists()
        {
            toggleLimiterBoxSource(false);

            string[] paths = System.IO.Directory.GetFiles(CorruptCore.CorruptCore.listsDir);

            paths = paths.OrderBy(x => x).ToArray();

            List<string> hashes = Filtering.LoadListsFromPaths(paths);
            for (int i = 0; i < hashes.Count; i++)
            {
                string[] _paths = paths[i].Split('\\', '.');
                CorruptCore.Filtering.RegisterListInUI(_paths[_paths.Length - 2], hashes[i]);
            }
            toggleLimiterBoxSource(true);
        }

    }
}
