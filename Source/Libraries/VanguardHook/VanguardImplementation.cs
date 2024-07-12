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
using RTCV.Common.CustomExtensions;
using RTCV.CorruptCore.Extensions;
using System.Reflection;
using Newtonsoft.Json;
using static VanguardHook.VanguardCore;
using System.Runtime.InteropServices.ComTypes;

namespace VanguardHook
{
	public class MemoryDomainSRAMDomain : IMemoryDomain
	{
		public string Name => "SRAM";
		public bool BigEndian => true;
		public long Size => 0x017FFFFF;
		public int WordSize => 4;
		public override string ToString() => Name;
		public MemoryDomainSRAMDomain()
		{

		}
		public void PokeByte(long address, byte val)
		{
			if (address > Size)
			{
				return;
			}
			VanguardImplementation.VM_WRITEB(address + 0x80000000, (byte)val);
		}
		public byte PeekByte(long addr)
		{
			if (addr > Size)
			{
				return 0;
			}
			uint buffer = 0;
			return (byte)VanguardImplementation.VM_READB(addr + 0x80000000, buffer);
		}
		public byte[] PeekBytes(long address, int length)
		{

			var returnArray = new byte[length];
			for (var i = 0; i < length; i++)
				returnArray[i] = PeekByte(address + i);
			return returnArray;
		}
	}
	public class MemoryDomainEXRAMDomain : IMemoryDomain
	{
		public string Name => "EXRAM";
		public bool BigEndian => true;
		public long Size => 0x03FFFFFF;
		public int WordSize => 4;
		public override string ToString() => Name;
		public MemoryDomainEXRAMDomain()
		{

		}
		public void PokeByte(long address, byte val)
		{
			if (address > Size)
			{
				return;
			}
			VanguardImplementation.VM_WRITEB(address + 0x90000000, val);
		}
		public byte PeekByte(long addr)
		{
			if (addr > Size)
			{
				return 0;
			}
			uint buffer = 0;
			return (byte)VanguardImplementation.VM_READB(addr + 0x90000000, buffer);
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

		public delegate void Vsavesavestate([MarshalAs(UnmanagedType.BStr)] string filename, bool wait);
		public static Vsavesavestate Vanguard_savesavestate = GetMethod<Vsavesavestate>("Vanguard_savesavestate");

        public delegate void Vloadsavestate([MarshalAs(UnmanagedType.BStr)] string filename);
		public static Vloadsavestate Vanguard_loadsavestate = GetMethod<Vloadsavestate>("Vanguard_loadsavestate");

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
            IntPtr pDll = NativeMethods.LoadLibrary(VSpecConfig.config.EmuEXE);
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
				Vanguard_savesavestate(path, false);
				Vanguard_loadsavestate(path);
			}, true);
		}

		public static byte VM_READB(long addr, uint buf)
        {
			return Vanguard_peekbyte(addr);
			
		}
		public static void VM_WRITEB(long addr, byte buf)
		{
			Vanguard_pokebyte(addr, buf);
		}
		public static void SaveVMState(string path)
        {
            Vanguard_savesavestate(path, false);
        }
		public static void LoadVMState(string filename)
		{
            Vanguard_loadsavestate(filename);
		}
		public static string GetStateName()
        {
			return "";
        }
		public static string SaveSavestate(string Key, bool threadSave = false)
		{
            string quickSlotName = Key + ".timejump";
			string prefix = VanguardCore.GameName;
			string path = Path.Combine(RtcCore.workingDir, "SESSION", prefix + "." + quickSlotName + ".State");

			//Todo: readd MakeSafeFilename

			FileInfo file = new FileInfo(path);
			if (file.Directory != null && file.Directory.Exists == false)
				file.Directory.Create();
			
			VanguardImplementation.SaveVMState(path);
			return path;
		}

        public static bool LoadSavestate(string path, StashKeySavestateLocation stateLocation = StashKeySavestateLocation.DEFAULTVALUE)
		{
			StepActions.ClearStepBlastUnits();
			RtcClock.ResetCount();
			LoadVMState(path);
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
		public static void LoadROM(string rompath)
		{
			//vanguard_setDVDPath(dvd);
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
		public static MemoryDomainProxy[] GetInterfaces()
        {
			Console.WriteLine($"getInterfaces()");
			List<MemoryDomainProxy> interfaces = new List<MemoryDomainProxy>();
			MemoryDomainSRAMDomain SRAMdomn = new MemoryDomainSRAMDomain();
			interfaces.Add(new MemoryDomainProxy(SRAMdomn));
            MemoryDomainEXRAMDomain EXRAMdomn = new MemoryDomainEXRAMDomain();
            interfaces.Add(new MemoryDomainProxy(EXRAMdomn));
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
						RefreshDomains();
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
						var fileName = advancedMessage.objectValue as string;
						//VanguardCore.LOAD_GAME_START(fileName);
						
					}
					break;
				case RTCV.NetCore.Commands.Remote.PreCorruptAction:
					SyncObjectSingleton.EmuThreadExecute(Vanguard_pause(), true);
					break;

				case RTCV.NetCore.Commands.Remote.PostCorruptAction:
                    {
						//var path = Path.Combine(RtcCore.workingDir, "Dolphintmp.savestat");
						SyncObjectSingleton.EmuThreadExecute(() =>
						{
							Vanguard_resume();
							//Vanguard_savesavestate(path);
							//Vanguard_loadsavestate(path);
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
