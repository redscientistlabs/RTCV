
using RTCV.CorruptCore;
using RTCV.CorruptCore.Extensions;
using RTCV.NetCore;
using RTCV.NetCore.Commands;
using RTCV.Vanguard;
using RTCV.UI;
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Threading.Timer;
using System.Linq;
using System.Windows.Threading;
using RTCV.Common;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace VanguardHook
{
    public static class EmuDirectory
    {
        public static string emuDir;
        public static string logPath;
        public static string emuEXE;
    }

    class VanguardCore
	{
        public static System.Timers.Timer focusTimer;
        public static bool FirstConnect = true;
		public static Form SyncForm;
        public static VanguardRealTimeEvents RTE_API = new VanguardRealTimeEvents();
        public static bool attached = false;
		public static string System
		{
			get => (string)AllSpec.VanguardSpec[VSPEC.SYSTEM];
			set => AllSpec.VanguardSpec.Update(VSPEC.SYSTEM, value);
		}
		public static string GameName
		{
			get => (string)AllSpec.VanguardSpec[VSPEC.GAMENAME];
			set => AllSpec.VanguardSpec.Update(VSPEC.GAMENAME, value);
		}
		public static string SystemPrefix
		{
			get => (string)AllSpec.VanguardSpec[VSPEC.SYSTEMPREFIX];
			set => AllSpec.VanguardSpec.Update(VSPEC.SYSTEMPREFIX, value);
		}
		public static string SystemCore
		{
			get => (string)AllSpec.VanguardSpec[VSPEC.SYSTEMCORE];
			set => AllSpec.VanguardSpec.Update(VSPEC.SYSTEMCORE, value);
		}
		public static string SyncSettings
		{
			get => (string)AllSpec.VanguardSpec[VSPEC.SYNCSETTINGS];
			set => AllSpec.VanguardSpec.Update(VSPEC.SYNCSETTINGS, value);
		}
		public static string OpenRomFilename
		{
			get => (string)AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME];
			set => AllSpec.VanguardSpec.Update(VSPEC.OPENROMFILENAME, value);
		}
		public static int LastLoadedRom
		{
			get => (int)AllSpec.VanguardSpec[VSPEC.CORE_LASTLOADERROM];
			set => AllSpec.VanguardSpec.Update(VSPEC.CORE_LASTLOADERROM, value);
		}
		public static string[] BlacklistedDomains
		{
			get => (string[])AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS];
			set => AllSpec.VanguardSpec.Update(VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS, value);
		}
		public static MemoryDomainProxy[] MemoryInterfaces
		{
			get => (MemoryDomainProxy[])AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_INTERFACES];
			set => AllSpec.VanguardSpec.Update(VSPEC.MEMORYDOMAINS_INTERFACES, value);
		}

		public static string RTCVHookOGLVersion = "0.0.1";

        public static PartialSpec getDefaultPartial()
        {
			//read config file and store the values
            PartialSpec partial = new PartialSpec("VanguardSpec");
			if (VanguardConfigReader.configFile == null)
				return default;
			var config = VanguardConfigReader.configFile.VSpecConfig;
            partial[VSPEC.NAME] = config.NAME;
            partial[VSPEC.SYSTEM] = String.Empty;
			partial[VSPEC.GAMENAME] = String.Empty;
			partial[VSPEC.SYSTEMPREFIX] = String.Empty;
			partial[VSPEC.OPENROMFILENAME] = String.Empty;
			partial[VSPEC.SYNCSETTINGS] = String.Empty;
            partial[VSPEC.OVERRIDE_DEFAULTMAXINTENSITY] = config.OVERRIDE_DEFAULTMAXINTENSITY;
            partial[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] = config.MEMORYDOMAINS_BLACKLISTEDDOMAINS;
			partial[VSPEC.MEMORYDOMAINS_INTERFACES] = new MemoryDomainProxy[] { };
			partial[VSPEC.CORE_LASTLOADERROM] = -1;
            partial[VSPEC.SUPPORTS_RENDERING] = config.SUPPORTS_RENDERING;
            partial[VSPEC.SUPPORTS_CONFIG_MANAGEMENT] = config.SUPPORTS_CONFIG_MANAGEMENT;
            partial[VSPEC.SUPPORTS_CONFIG_HANDOFF] = config.SUPPORTS_CONFIG_HANDOFF;
            partial[VSPEC.SUPPORTS_KILLSWITCH] = config.SUPPORTS_KILLSWITCH;
            partial[VSPEC.SUPPORTS_REALTIME] = config.SUPPORTS_REALTIME;
            partial[VSPEC.SUPPORTS_SAVESTATES] = config.SUPPORTS_SAVESTATES;
            partial[VSPEC.SUPPORTS_REFERENCES] = config.SUPPORTS_REFERENCES;
            partial[VSPEC.SUPPORTS_MIXED_STOCKPILE] = config.SUPPORTS_MIXED_STOCKPILE;
			partial[VSPEC.CORE_DISKBASED] = config.CORE_DISKBASED;
            partial[VSPEC.CONFIG_PATHS] = new[] { "" };
			partial[VSPEC.EMUDIR] = EmuDirectory.emuDir;
			EmuDirectory.emuEXE = config.EmuEXE;
            EmuDirectory.logPath = Path.Combine(EmuDirectory.emuDir, "EMU_LOG.txt");

            return partial;
			
        }
		internal static void CreateVmdText(string domain, string text)
        {
			LocalNetCoreRouter.Route(Endpoints.UI, Remote.GenerateVMDText, new object[] { domain, text }, false);
        }
		public static void RegisterVanguardSpec()
        {
			PartialSpec emuSpecTemplate = new PartialSpec("VanguardSpec");
			emuSpecTemplate.Insert(VanguardCore.getDefaultPartial());
			AllSpec.VanguardSpec = new FullSpec(emuSpecTemplate, !RtcCore.Attached);
			if (VanguardCore.attached)
				VanguardConnector.PushVanguardSpecRef(AllSpec.VanguardSpec);
            // also update the RtcCore directory for the killswitch
			// have to update it here because CorruptCore checks the AllSpec,
			// and I'm not changing that code
            RtcCore.EmuDir = EmuDirectory.emuDir;
            LocalNetCoreRouter.Route(Endpoints.CorruptCore, Remote.PushVanguardSpec, emuSpecTemplate, true);
			LocalNetCoreRouter.Route(Endpoints.UI, Remote.PushVanguardSpec, emuSpecTemplate, true);

			AllSpec.VanguardSpec.SpecUpdated += new EventHandler<SpecUpdateEventArgs>(OnVanguardSpecOnSpecUpdated);
        }
		private static void OnVanguardSpecOnSpecUpdated(object o, SpecUpdateEventArgs e)
		{
			PartialSpec partial = e.partialSpec;

			LocalNetCoreRouter.Route(RTCV.NetCore.Endpoints.CorruptCore, RTCV.NetCore.Commands.Remote.PushVanguardSpecUpdate, partial, true);
			LocalNetCoreRouter.Route(RTCV.NetCore.Endpoints.UI, RTCV.NetCore.Commands.Remote.PushVanguardSpecUpdate, partial, true);
		}
		public static void EmuThreadExecute(Action callback)
        {
			Dispatcher.CurrentDispatcher.Invoke((MethodInvoker)delegate
			{
				callback();
			}, null);

		}

		public static string GAME_TO_LOAD = "";
		public static void LOAD_GAME_START(string rompath)
        {
			StepActions.ClearStepBlastUnits();
			RtcClock.ResetCount();

			if (rompath == "EMPTY")
			{
				rompath = GAME_TO_LOAD;
				GAME_TO_LOAD = "";
			}
			AllSpec.VanguardSpec.Update(VSPEC.OPENROMFILENAME, rompath, true, true);
        }
		public static void LOAD_GAME_DONE(string gamename)
		{
            if (AllSpec.UISpec == null)
            {
				StopGame();
				string template = $"It appears you haven't connected to StandaloneRTC. Please make sure that the " +
				"RTC is running and not just {0}. If you have an antivirus, it might be " +
				"blocking the RTC from launching.\n\nIf you keep getting this message, poke " +
				"the RTC devs for help (Discord is in the launcher).";
				string message = string.Format(template, AllSpec.VanguardSpec[VSPEC.NAME]);

                MessageBox.Show(message,"RTC Not Connected");
                return;
            }
            PartialSpec gameDone = new PartialSpec("VanguardSpec");
            gameDone[VSPEC.SYSTEM] = VanguardConfigReader.configFile.VSpecConfig.NAME;
			gameDone[VSPEC.SYSTEMPREFIX] = VanguardConfigReader.configFile.VSpecConfig.NAME;

			//We need this code to be able to choose between Wii and Gamecube for Dolphin
			if (VanguardConfigReader.configFile.VSpecConfig.PROFILE == "Dolphin")
			{
                if (VanguardImplementation.Vanguard_isWii())
					gameDone[VSPEC.SYSTEMCORE] = "Wii";

				else
					gameDone[VSPEC.SYSTEMCORE] = "Gamecube";
			}
			else
				gameDone[VSPEC.SYSTEMCORE] = VanguardConfigReader.configFile.VSpecConfig.PROFILE;

            gameDone[VSPEC.SYNCSETTINGS] = "";

			// remove any invalid file characters before storing it
			string gamenameFixed = StringExtensions.MakeSafeFilename(gamename, '-');

            gameDone[VSPEC.GAMENAME] = gamenameFixed;
            //Todo: add sync settings
            AllSpec.VanguardSpec.Update(gameDone);
            VanguardImplementation.RefreshDomains();
            RtcCore.InvokeLoadGameDone();
            VanguardImplementation.Vanguard_finishLoading();

        }
        public static void GAME_CLOSED()
		{
			PartialSpec gameClosed = new PartialSpec("VanguardSpec");
			gameClosed[VSPEC.OPENROMFILENAME] = "";
			AllSpec.VanguardSpec.Update(gameClosed);
			VanguardImplementation.RefreshDomains();
			RtcCore.InvokeGameClosed(true);
		}
		public static void EMULATOR_CLOSING()
		{
			VanguardImplementation.StopClient();
			RtcCore.InvokeGameClosed(true);
		}
        public static bool RTC_OSD_ENABLED()
        {
            if (VanguardImplementation.enableRTC)
			{
				return true;
			}
            if (RTCV.NetCore.Params.IsParamSet(RTCSPEC.CORE_EMULATOROSDDISABLED))
            {
                return false;
            }
			return true;
        }
        public static void Start()
		{


            SyncForm = new AnchorForm();
			var handle = SyncForm.Handle;
            SyncObjectSingleton.SyncObject = SyncForm;

            SyncObjectSingleton.EmuThreadIsMainThread = true;
			//SyncForm.Show();
			SyncForm.Activate();
			ConsoleHelper.CreateConsole();
			ConsoleHelper.HideConsole();

            EmuDirectory.emuDir = EmuDirectory.emuDir + "\\";
            //Start everything
            VanguardImplementation.StartClient();
            VanguardCore.RegisterVanguardSpec();
            Thread.Sleep(500);
            RtcCore.StartEmuSide();

            focusTimer = new System.Timers.Timer
			{
				AutoReset = true,
				Interval = 250
			};

			//Update the focus state of the emulator
			focusTimer.Elapsed += (sender, eventArgs) =>
			{
				if (VanguardImplementation.connector.netConn.status == RTCV.NetCore.Enums.NetworkStatus.CONNECTED)
				{
					// get the current foreground window
                    IntPtr hWnd = NativeMethods.GetForegroundWindow();

					// get length of the path the emulator is running from
					int EmuWinLen = AllSpec.VanguardSpec[VSPEC.EMUDIR].ToString().Length + EmuDirectory.emuEXE.Length + 1;
                    StringBuilder EmuWinPath = new StringBuilder(EmuWinLen);

                    // get the processID of the window, then get the full path name
                    uint processID;
                    NativeMethods.GetWindowThreadProcessId(hWnd, out processID);
                    IntPtr hProcess = NativeMethods.OpenProcess(0x0410, false, processID);
                    NativeMethods.GetModuleFileNameEx(hProcess, IntPtr.Zero, EmuWinPath, EmuWinLen);
                    string exeName = EmuWinPath.ToString().Substring(EmuWinPath.ToString().LastIndexOf("\\") + 1);

					var state = (exeName == EmuDirectory.emuEXE ? true : false);

					if (((bool?)RTCV.NetCore.AllSpec.VanguardSpec?[RTCV.NetCore.Commands.Emulator.InFocus] ?? true) != state)
						RTCV.NetCore.AllSpec.VanguardSpec?.Update(RTCV.NetCore.Commands.Emulator.InFocus, state, true, false);
				}
			};
			focusTimer.Start();
		}

		public static void StopGame()
		{
            var g = new SyncObjectSingleton.GenericDelegate(VanguardImplementation.Vanguard_forceStop);
            SyncObjectSingleton.FormExecute(g);
        }
	}
}

