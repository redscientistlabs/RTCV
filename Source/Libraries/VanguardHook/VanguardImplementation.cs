using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RTCV.Common;
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.Vanguard;
using RTCV.UI;
using RTCV.NetCore.Commands;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Globalization;
using RTCV.Common.CustomExtensions;
using RTCV.CorruptCore.Extensions;
using System.Reflection;
using Newtonsoft.Json;
using static VanguardHook.VanguardCore;
using System.Runtime.InteropServices.ComTypes;

namespace VanguardHook
{
    public class MemoryDomain : IMemoryDomain
	{
		private string VName;
		private bool VBigEndian;
		private long VSize;
		private int VWordSize;
		private long VOffset;
		public MemoryDomain(MemoryDomainConfig config)
		{
			VName = config.Name;
			VBigEndian = config.BigEndian;
			VSize = Int32.Parse(config.Size.Substring(2), NumberStyles.HexNumber);
			VWordSize = config.WordSize;
			VOffset = Int32.Parse(config.Offset.Substring(2), NumberStyles.HexNumber);
		}

		// set the interface variables
		public string Name => VName;
        public bool BigEndian => VBigEndian;
        public long Size => VSize;
        public int WordSize => VWordSize;

        public override string ToString() => VName;
		public MemoryDomain()
		{

		}
		public void PokeByte(long address, byte val)
		{
			if (address > Size)
			{
				return;
			}
			VanguardImplementation.Vanguard_pokebyte(address + VOffset, (byte)val);
		}
		public byte PeekByte(long addr)
		{
			if (addr > Size)
			{
				return 0;
			}
			return (byte)VanguardImplementation.Vanguard_peekbyte(addr + VOffset);
		}
		public byte[] PeekBytes(long address, int length)
		{

			var returnArray = new byte[length];
			for (var i = 0; i < length; i++)
				returnArray[i] = PeekByte(address + i);
			return returnArray;
		}
	}

	//Import Windows functions required for importing emulator functions
	public static class NativeMethods
	{
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

    }

	public class VanguardImplementation
	{

		public static bool enableRTC = true;

		//load the emulator exe and creates a pointer to it
		public static IntPtr pEXE = LoadEmuPointer();

		//
		//Import all supported functions from the emulator
		//
		public delegate byte Vpeekbyte(long addr);
		public static Vpeekbyte Vanguard_peekbyte = GetMethod<Vpeekbyte>("Vanguard_peekbyte");

		public delegate void Vpokebyte(long addr, byte buf);
		public static Vpokebyte Vanguard_pokebyte = GetMethod<Vpokebyte>("Vanguard_pokebyte");

		public delegate void Vsavesavestate([MarshalAs(UnmanagedType.BStr)] string filename, bool wait = false);
		public static Vsavesavestate Vanguard_savesavestate = GetMethod<Vsavesavestate>("Vanguard_savesavestate");

		public delegate void Vloadsavestate([MarshalAs(UnmanagedType.BStr)] string filename);
		public static Vloadsavestate Vanguard_loadsavestate = GetMethod<Vloadsavestate>("Vanguard_loadsavestate");

		public delegate void VLoadROM([MarshalAs(UnmanagedType.BStr)] string filename);
		public static VLoadROM Vanguard_loadROM = GetMethod<VLoadROM>("Vanguard_loadROM");

		public delegate void VfinishLoading();
		public static VfinishLoading Vanguard_finishLoading = GetMethod<VfinishLoading>("Vanguard_finishLoading");

		public delegate bool VisWii();
		public static VisWii Vanguard_isWii = GetMethod<VisWii>("Vanguard_isWii");


		//These two aren't programmed in the emulator right now, I'll have to figure out how to decide which methods are imported
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Action Vpause();
        //public static Vpause Vanguard_pause = GetMethod<Vpause>("Vanguard_pause");
        public static Vpause Vanguard_pause;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Vresume();
        //public static Vresume Vanguard_resume = GetMethod<Vresume>("Vanguard_resume");
        public static Vresume Vanguard_resume;

		public static IntPtr LoadEmuPointer()
		{
            IntPtr pDll = NativeMethods.LoadLibrary(VanguardCore.emuEXE);
			return pDll;
        }

		public static T GetMethod<T>(string MethodName)
		{
			IntPtr procAddr = NativeMethods.GetProcAddress(pEXE, MethodName);
			T Method = Marshal.GetDelegateForFunctionPointer<T>(procAddr);
            return Method;
		}

        public static void ReloadState()
        {
			var path = Path.Combine(RtcCore.workingDir, "SESSION", "Dolphintmp.savestat");
			SyncObjectSingleton.EmuThreadExecute(() =>
			{
				// Call emulator functions
				Vanguard_savesavestate(path, false);
				Vanguard_loadsavestate(path);
			}, true);
		}

		public static string SaveSavestate(string Key, bool threadSave = false)
		{
            string quickSlotName = Key + ".timejump";
			string prefix = VanguardCore.GameName;
			string path = Path.Combine(RtcCore.workingDir, "SESSION", prefix + "." + quickSlotName + ".State");

			// Todo: readd MakeSafeFilename

			FileInfo file = new FileInfo(path);
			if (file.Directory != null && file.Directory.Exists == false)
				file.Directory.Create();
			
			// Call emulator function
			Vanguard_savesavestate(path);
			return path;
		}

        public static bool LoadSavestate(string path, StashKeySavestateLocation stateLocation = StashKeySavestateLocation.DEFAULTVALUE)
		{
			StepActions.ClearStepBlastUnits();
			RtcClock.ResetCount();

			// Call emulator function
            Vanguard_loadsavestate(path);
			return true;
		}
		
		public static string GetGameName()
		{
			string gamename = "IGNORE";
			
			return gamename;
		}
		public static string GetShortGameName()
        {
			string gamename = GetGameName();
			string shortgamename = gamename.Trim().Replace(" ", "").Substring(0, 8);
			return shortgamename;
        }
		public static void LoadROM(string filename)
		{
			string currentOpenRom = "";
			if ((string)AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME] != "")
				currentOpenRom = (string)AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME];

			// Game is not running
			if (currentOpenRom != filename)
			{
				// Clear out any old settings
				//Config.ClearCurrentVanguardLayer();

				// Call emulator function
				Vanguard_loadROM(filename);
			}
        }
		public static string GetROM()
        {
			return "";
        }

        //[UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        //delegate
        public static RTCV.Vanguard.VanguardConnector connector = null;
        public static void StartClient()
        {

            try
            {
                ConsoleEx.WriteLine("Starting Vanguard Client");
                Thread.Sleep(500);
                var spec = new NetCoreReceiver();
                spec.Attached = VanguardCore.attached;
                spec.MessageReceived += OnMessageRecieved;
                connector = new RTCV.Vanguard.VanguardConnector(spec);
                //while(true)
                //            {
                //            }
            }
            catch (Exception ex)
            {
                ConsoleEx.WriteLine("ERROR");
                throw new RTCV.NetCore.AbortEverythingException();
            }
        }
        public static void RestartClient()
        {
            connector?.Kill();
            connector = null;
            StartClient();
        }
        public static void StopClient()
        {
            connector?.Kill();
            connector = null;
        }
        public static void RefreshDomains()
        {
			if (connector.netcoreStatus != RTCV.NetCore.Enums.NetworkStatus.CONNECTED)
                return;
			PartialSpec gameDone = new PartialSpec("VanguardSpec");
			gameDone[VSPEC.MEMORYDOMAINS_INTERFACES] = GetInterfaces();
			//gameDone[VSPEC.GAMENAME] = GetGameName();
			AllSpec.VanguardSpec.Update(gameDone);
			LocalNetCoreRouter.Route(RTCV.NetCore.Endpoints.CorruptCore, RTCV.NetCore.Commands.Remote.EventDomainsUpdated, true, true);
		}

		// finds all domains to be used by the system
		public static MemoryDomainProxy[] GetInterfaces()
        {
			Console.WriteLine($"getInterfaces()");

            if (String.IsNullOrWhiteSpace((string)AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME]))
				return new List<MemoryDomainProxy>(0).ToArray();

            List<MemoryDomainProxy> interfaces = new List<MemoryDomainProxy>();
			List<MemoryDomainConfig> memDomainList = VanguardConfigReader.configFile.MemoryDomainConfig;
            for (var i = 0; i < memDomainList.Count; i++)
            {
                for (var j = 0; j < memDomainList[i].Profiles.Count; j++)
				{
                    if (memDomainList[i].Profiles[j] == (string)AllSpec.VanguardSpec[VSPEC.SYSTEMCORE])
					{
                        MemoryDomain memDomain = new MemoryDomain(memDomainList[i]);
                        interfaces.Add(new MemoryDomainProxy(memDomain));
						break;
                    }
				}
            }
            return interfaces.ToArray();
			
        }
        public static void OnMessageRecieved(object sender, NetCoreEventArgs e)
        {
            var message = e.message;
            var simpleMessage = message as NetCoreSimpleMessage;
            var advancedMessage = message as NetCoreAdvancedMessage;

            ConsoleEx.WriteLine(message.Type);
			switch (message.Type) //Handle received messages here
			{
				case RTCV.NetCore.Commands.Remote.AllSpecSent:
					{
						if (VanguardCore.FirstConnect)
						{
							SyncObjectSingleton.FormExecute(() => { ; });
							
							VanguardCore.FirstConnect = false;
						}
						//VanguardCore.LOAD_GAME_DONE();
						//RefreshDomains();
					}
					break;
				case RTCV.NetCore.Commands.Basic.SaveSavestate:
					{
                        SyncObjectSingleton.EmuThreadExecute(() => { e.setReturnValue(SaveSavestate(advancedMessage.objectValue as string)); }, true);
						break;
					}
				case RTCV.NetCore.Commands.Basic.LoadSavestate:
					{
						var cmd = advancedMessage.objectValue as object[];
						var path = cmd[0] as string;
						var location = (StashKeySavestateLocation)cmd[1];
						SyncObjectSingleton.EmuThreadExecute(() => { e.setReturnValue(LoadSavestate(path, location)); }, true);
						break;
					}
                case RTCV.NetCore.Commands.Remote.LoadROM:
					{
						string fileName = advancedMessage.objectValue as string;
						// Dolphin DEMANDS the rom is loaded from the main thread
                        Action<string> a = new Action <string> (LoadROM);
                        SyncObjectSingleton.FormExecute<string>(a, fileName);
					}
					break;
				case RTCV.NetCore.Commands.Remote.PreCorruptAction:
					SyncObjectSingleton.EmuThreadExecute(Vanguard_pause(), true);
					break;

				case RTCV.NetCore.Commands.Remote.PostCorruptAction:
                    {
						SyncObjectSingleton.EmuThreadExecute(() =>
						{
							Vanguard_resume();
						}, true);
					}
					break;
				case RTCV.NetCore.Commands.Remote.CloseGame:
					{
						//SyncObjectSingleton.FormExecute(() => { Hooks.CLOSE_GAME(true); });
					}
					break;

				case RTCV.NetCore.Commands.Remote.DomainGetDomains:
					SyncObjectSingleton.FormExecute(() =>
					{
						e.setReturnValue(GetInterfaces());
					});
					break;

				case RTCV.NetCore.Commands.Remote.DomainRefreshDomains:
					RefreshDomains();
					break;

				case RTCV.NetCore.Commands.Remote.KeySetSyncSettings:
					//SyncObjectSingleton.FormExecute(() => { Hooks.BIZHAWK_GETSET_SYNCSETTINGS = (string)advancedMessage.objectValue; });
					break;

				case RTCV.NetCore.Commands.Emulator.GetRealtimeAPI:
					e.setReturnValue(VanguardCore.RTE_API);
					break;


				case RTCV.NetCore.Commands.Remote.EventEmuStarted:
					//if (RTC_StockpileManager.BackupedState == null)
					//S.GET<RTC_Core_Form>().AutoCorrupt = false;


					//Todo
					//RTC_NetcoreImplementation.SendCommandToBizhawk(new RTC_Command("REMOTE_PUSHVMDS) { objectValue = MemoryDomains.VmdPool.Values.Select(it => (it as VirtualMemoryDomain).Proto).ToArray() }, true, true);

					//Thread.Sleep(100);

					//		if (RTC_StockpileManager.BackupedState != null)
					//			S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected(RTC_StockpileManager.BackupedState.SelectedDomains.ToArray());

					//		if (S.GET<RTC_Core_Form>().cbUseGameProtection.Checked)
					//			RTC_GameProtection.Start();

					break;
				case RTCV.NetCore.Commands.Remote.OpenHexEditor:
					SyncObjectSingleton.FormExecute(() => LocalNetCoreRouter.Route("HEXEDITOR", Remote.OpenHexEditor, true));
					break;
				case RTCV.NetCore.Commands.Remote.EventCloseEmulator:
					Environment.Exit(-1);
					break;
			}
		}
    }
}
