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
using System.Windows.Forms;
using Newtonsoft.Json;
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.UI;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public static class UICore
	{


		//Note Box Settings
		public static Point NoteBoxPosition;
		public static Size NoteBoxSize;

		public static bool FirstConnect = true;
		public static bool HideStartButton = false;

		//RTC Main Forms
		//public static Color generalColor = Color.FromArgb(60, 45, 70);
		public static Color GeneralColor = Color.LightSteelBlue;

		public static void Start(Form standaloneForm = null)
		{
			S.formRegister.FormRegistered += FormRegister_FormRegistered;
			registerFormEvents(S.GET<RTC_Core_Form>());

			CorruptCore_Extensions.DirectoryRequired(paths: new string[] {
				CorruptCore.CorruptCore.workingDir, CorruptCore.CorruptCore.workingDir + "\\TEMP\\"
				, CorruptCore.CorruptCore.workingDir + "\\SKS\\", CorruptCore.CorruptCore.workingDir + "\\SSK\\"
				, CorruptCore.CorruptCore.workingDir + "\\SESSION\\", CorruptCore.CorruptCore.workingDir + "\\MEMORYDUMPS\\"
				, CorruptCore.CorruptCore.workingDir + "\\MP\\", CorruptCore.CorruptCore.assetsDir + "\\CRASHSOUNDS\\"
				, CorruptCore.CorruptCore.rtcDir + "\\PARAMS\\", CorruptCore.CorruptCore.rtcDir + "\\LISTS\\"
				, CorruptCore.CorruptCore.rtcDir + "\\RENDEROUTPUT\\",
			});

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

				  LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHUISPECUPDATE, partial, e.syncedUpdate);
			};

			CorruptCore.CorruptCore.StartUISide();

			//Loading RTC Params
			LoadRTCColor();
			S.GET<RTC_SettingsGeneral_Form>().cbDisableBizhawkOSD.Checked = !RTCV.NetCore.Params.IsParamSet("ENABLE_BIZHAWK_OSD");
			S.GET<RTC_SettingsGeneral_Form>().cbAllowCrossCoreCorruption.Checked = RTCV.NetCore.Params.IsParamSet("ALLOW_CROSS_CORE_CORRUPTION");
			S.GET<RTC_SettingsGeneral_Form>().cbDontCleanAtQuit.Checked = RTCV.NetCore.Params.IsParamSet("DONT_CLEAN_SAVESTATES_AT_QUIT");
			S.GET<RTC_SettingsGeneral_Form>().cbUncapIntensity.Checked = RTCV.NetCore.Params.IsParamSet("UNCAP_INTENSITY");


			S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>());
			if (HideStartButton)
			{
				S.GET<RTC_ConnectionStatus_Form>().btnStartEmuhawkDetached.Visible = false;
			}
			S.GET<RTC_Core_Form>().Show();


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

			bool previousState = (bool?)RTCV.NetCore.AllSpec.UISpec[NetcoreCommands.RTC_INFOCUS] ?? false;
			bool currentState = forceSet ?? isAnyRTCFormFocused();
			Console.WriteLine("Setting state to " + currentState);

			if (previousState != currentState)
			{	//This is a non-synced spec update to prevent jittering. Shouldn't have any other noticeable impact
				RTCV.NetCore.AllSpec.UISpec.Update(NetcoreCommands.RTC_INFOCUS, currentState,true,false);
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


				foreach (Type t in Assembly.GetAssembly(typeof(RTCV.UI.RTC_Core_Form)).GetTypes())
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


		public static void SetRTCColor(Color color, Form form = null)
		{
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

			var lightColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:light"));
			var normalColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:normal"));
			var darkColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:dark"));
			var darkerColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:darker"));
			var darkererColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:darkerer"));

			foreach (Control c in lightColorControls)
				if (c is Label)
					c.ForeColor = color.ChangeColorBrightness(0.30f);
				else
					c.BackColor = color.ChangeColorBrightness(0.30f);

			foreach (Control c in normalColorControls)
				if (c is Label)
					c.ForeColor = color;
				else
					c.BackColor = color;

			S.GET<RTC_StockpilePlayer_Form>().dgvStockpile.BackgroundColor = color;
			S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.BackgroundColor = color;



			S.GET<RTC_NewBlastEditor_Form>().dgvBlastEditor.BackgroundColor = color;
			S.GET<RTC_BlastGenerator_Form>().dgvBlastGenerator.BackgroundColor = color;

			foreach (Control c in darkColorControls)
				if (c is Label)
					c.ForeColor = color.ChangeColorBrightness(-0.30f);
				else
					c.BackColor = color.ChangeColorBrightness(-0.30f);

			foreach (Control c in darkerColorControls)
				if (c is Label)
					c.ForeColor = color.ChangeColorBrightness(-0.55f);
				else
					c.BackColor = color.ChangeColorBrightness(-0.55f);

			foreach (Control c in darkererColorControls)
				if (c is Label)
					c.ForeColor = color.ChangeColorBrightness(-0.75f);
				else
					c.BackColor = color.ChangeColorBrightness(-0.75f);
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


	}
}
