
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.NetCore.Commands;
using RTCV.Vanguard;
using System;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using RTCV.Common;
using System.Runtime.InteropServices;

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
		public static bool connected = false;
		public static PartialSpec emuSpecTemplate = new PartialSpec("VanguardSpec");

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

		// Loads config file data and returns a partially built spec for when Vanguard first connects
        public static PartialSpec getDefaultPartial()
        {
            PartialSpec partial = new PartialSpec("VanguardSpec");
            // Read config and blacklisted domains files and store their values
            var config = VanguardConfigReader.configFile.VSpecConfig;
			var blacklistedDomainsConfig = VanguardBlacklistedDomains.domains;
			partial[VSPEC.NAME] = config.NAME;
			partial[VSPEC.SYSTEM] = String.Empty;
			partial[VSPEC.GAMENAME] = String.Empty;
			partial[VSPEC.SYSTEMPREFIX] = String.Empty;
			partial[VSPEC.OPENROMFILENAME] = String.Empty;
			partial[VSPEC.SYNCSETTINGS] = String.Empty;
			partial[VSPEC.OVERRIDE_DEFAULTMAXINTENSITY] = config.OVERRIDE_DEFAULTMAXINTENSITY;
			partial[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] = blacklistedDomainsConfig.MEMORYDOMAINS_BLACKLISTEDDOMAINS;
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

		// Registers the initial spec and pushes it to the CorruptCore and UI
		public static void RegisterVanguardSpec()
        {
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

			//Create the FullSpec template for the AllSpec before starting the client connection
            emuSpecTemplate.Insert(VanguardCore.getDefaultPartial());
            AllSpec.VanguardSpec = new FullSpec(emuSpecTemplate, !RtcCore.Attached);
            if (VanguardCore.attached)
                VanguardConnector.PushVanguardSpecRef(AllSpec.VanguardSpec);
            RtcCore.EmuDir = EmuDirectory.emuDir;

            VanguardImplementation.StartClient();
			RegisterVanguardSpec();

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
			MethodImports.Vanguard_forceStop();
        }

		// Stops the connection to the RTC client and terminates the hook
        public static void StopVanguard()
        {
            VanguardImplementation.StopClient();
            RtcCore.InvokeGameClosed(true);


            Environment.Exit(-1);
        }

		public static void SaveEmuSettings()
		{
            var defaultSettingsPath = Path.Combine(RtcCore.workingDir, "SESSION", (string)AllSpec.VanguardSpec[VSPEC.NAME] + "VanguardDefaultSettings");

            //Get the settings from the emulator and save them to a file
            PartialSpec storeDefaultSettings = new PartialSpec("VanguardSpec");
            IntPtr settingsPtr = MethodImports.Vanguard_saveEmuSettings();

            string default_settings = Marshal.PtrToStringAnsi(settingsPtr);
            //Make sure to free the pointer after using it
            Marshal.FreeHGlobal(settingsPtr);

            using (StreamWriter writetext = new StreamWriter(defaultSettingsPath))
            {
                writetext.WriteLine(default_settings);
                ConsoleEx.WriteLine("default settings stored: \n" + default_settings);
            }

            AllSpec.VanguardSpec.Update(storeDefaultSettings);
        }

		public static void LoadEmuSettings()
		{
            var defaultSettingsPath = Path.Combine(RtcCore.workingDir, "SESSION", (string)AllSpec.VanguardSpec[VSPEC.NAME] + "VanguardDefaultSettings");
			ConsoleEx.WriteLine("checking for " + defaultSettingsPath);
			if (File.Exists(defaultSettingsPath))
			{
				string default_settings;
				using (StreamReader readtext = new StreamReader(defaultSettingsPath))
				{
					default_settings = readtext.ReadToEnd();
					ConsoleEx.WriteLine("loading default settings: \n" + default_settings);
					MethodImports.Vanguard_loadEmuSettings(default_settings);

				}

				//Remove the file after we're done with it 
				File.Delete(defaultSettingsPath);
				ConsoleEx.WriteLine("file deleted");
			}
			else
				ConsoleEx.WriteLine("file not found");
        }
    }
}

