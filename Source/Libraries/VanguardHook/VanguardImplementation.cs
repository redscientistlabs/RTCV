using RTCV.CorruptCore;
using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VanguardHook
{
    public class MemoryDomain : IMemoryDomain
	{
		private string VName;
		private bool VBigEndian;
		private long VSize;
		private int VWordSize;
		private long VOffset;
		private int VPeekPokeSel;
		public MemoryDomain(MemoryDomainConfig config)
		{
			VName = config.Name;
			VBigEndian = config.BigEndian;
			VSize = Int32.Parse(config.Size.Substring(2), NumberStyles.HexNumber);
			VWordSize = config.WordSize;
			VOffset = Int32.Parse(config.Offset.Substring(2), NumberStyles.HexNumber);
			VPeekPokeSel = config.PeekPokeSel;
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
			if (address > VSize)
			{
				return;
			}
			MethodImports.Vanguard_pokebyte(address + VOffset, (byte)val, VPeekPokeSel);
		}
		public byte PeekByte(long addr)
		{
            if (addr > VSize)
            {
                return 0;
            }
            return (byte)MethodImports.Vanguard_peekbyte(addr + VOffset, VPeekPokeSel);
		}
		public byte[] PeekBytes(long address, int length)
		{
			var returnArray = new byte[length];
			for (var i = 0; i < length; i++)
				returnArray[i] = PeekByte(address + i);
			return returnArray;
		}
	}

	public class VanguardImplementation
	{
		public static bool enableRTC = true;
		public static bool waitForEmulatorClose = false;
		public static bool reloadingRom = false;

        public static void ReloadState()
        {
			var path = Path.Combine(RtcCore.workingDir, "SESSION", "Dolphintmp.savestat");
			SyncObjectSingleton.EmuThreadExecute(() =>
			{
				// Call emulator functions
				MethodImports.Vanguard_savesavestate(path, false);
				MethodImports.Vanguard_loadsavestate(path);
			}, true);
		}

		public static string SaveSavestate(string Key, bool threadSave = false)
		{
            string quickSlotName = Key + ".timejump";
            // remove any invalid file characters before storing it
            string prefix = RTCV.CorruptCore.Extensions.StringExtensions.MakeSafeFilename(VanguardCore.GameName, '-');

            string path = Path.Combine(RtcCore.workingDir, "SESSION", prefix + "." + quickSlotName + ".State");

            FileInfo file = new FileInfo(path);
			if (file.Directory != null && file.Directory.Exists == false)
				file.Directory.Create();

            // Call emulator function
            MethodImports.Vanguard_savesavestate(path);
			return path;
		}

        public static bool LoadSavestate(string path, StashKeySavestateLocation stateLocation = StashKeySavestateLocation.DEFAULTVALUE)
		{
			StepActions.ClearStepBlastUnits();
			RtcClock.ResetCount();
			// Call emulator function
            MethodImports.Vanguard_loadsavestate(path);
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

			if (filename == currentOpenRom)
				reloadingRom = true;

             MethodImports.Vanguard_loadROM(filename);
        }
		public static string GetROM()
        {
			return "";
        }

        public static RTCV.Vanguard.VanguardConnector connector = null;
        public static void StartClient()
        {
			VanguardCore.connected = false;

            try
            {
                ConsoleEx.WriteLine("Starting Vanguard Client");
                var spec = new NetCoreReceiver();
                spec.Attached = VanguardCore.attached;
                spec.MessageReceived += OnMessageRecieved;

                connector = new RTCV.Vanguard.VanguardConnector(spec);
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

			var memoryInterfaces = GetInterfaces();
			bool domainsChanged = !memoryInterfaces.Select(p => p.Name)
				.SequenceEqual(((MemoryDomainProxy[])AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_INTERFACES]).Select(p => p.Name));
            
			AllSpec.VanguardSpec.Update(VSPEC.MEMORYDOMAINS_INTERFACES, memoryInterfaces);
            LocalNetCoreRouter.Route(RTCV.NetCore.Endpoints.CorruptCore, RTCV.NetCore.Commands.Remote.EventDomainsUpdated, domainsChanged, true);
        }
		// finds all domains to be used by the system
		public static MemoryDomainProxy[] GetInterfaces()
        {
			Console.WriteLine($"getInterfaces()");

            if (String.IsNullOrWhiteSpace((string)AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME]))
				return new List<MemoryDomainProxy>(0).ToArray();

            List<MemoryDomainProxy> interfaces = new List<MemoryDomainProxy>();
            List<MemoryDomainConfig> memDomainList = VanguardConfigReader.configFile.MemoryDomainConfig;
            List<string> domainsThatRequireReload = new List<string>();

            for (var i = 0; i < memDomainList.Count; i++)
			{
				for (var j = 0; j < memDomainList[i].Profiles.Count(); j++)
				{
					if (memDomainList[i].Profiles[j] == (string)AllSpec.VanguardSpec[VSPEC.SYSTEMCORE])
					{
						MemoryDomain memDomain = new MemoryDomain(memDomainList[i]);
						interfaces.Add(new MemoryDomainProxy(memDomain));

                        if (memDomainList[i].RequiresReload)
                            domainsThatRequireReload.Add(memDomainList[i].Name);

                        break;
					}
				}
			}

            string[] arr = domainsThatRequireReload.Count > 0 ? domainsThatRequireReload.ToArray() : new string[0];
			AllSpec.VanguardSpec.Update(VSPEC.RELOAD_IF_DOMAIN_SELECTED, arr);

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
						VanguardCore.connected = true;
                        SyncObjectSingleton.FormExecute(() => {; });
                        RefreshDomains();
                    }
					break;
				case RTCV.NetCore.Commands.Basic.SaveSavestate:
					{
						SyncObjectSingleton.EmuThreadExecute(() => { e.setReturnValue(SaveSavestate(advancedMessage.objectValue as string)); }, true);
                    }
					break;

                case RTCV.NetCore.Commands.Basic.LoadSavestate:
					{
                        var cmd = advancedMessage.objectValue as object[];
						var path = cmd[0] as string;
						var location = (StashKeySavestateLocation)cmd[1];
                        SyncObjectSingleton.EmuThreadExecute(() => { e.setReturnValue(LoadSavestate(path, location)); }, true);

						ConsoleEx.WriteLine("finished loading savestate");
                    }
					break;

                case RTCV.NetCore.Commands.Remote.LoadROM:
					{
						string fileName = advancedMessage.objectValue as string;
						// load game on the main thread
						Action<string> a = new Action<string>(LoadROM);
						SyncObjectSingleton.FormExecute<string>(a, fileName);
					}
					break;

				case RTCV.NetCore.Commands.Remote.PreCorruptAction:
					{ 
						SyncObjectSingleton.EmuThreadExecute(() => { MethodImports.Vanguard_pause(true); }, true);
					}
					break;

				case RTCV.NetCore.Commands.Remote.PostCorruptAction:
                    {
                        SyncObjectSingleton.EmuThreadExecute(() => { MethodImports.Vanguard_resume(); }, true);
                    }
					break;

				case RTCV.NetCore.Commands.Remote.CloseGame:
					{
                        SyncObjectSingleton.EmuThreadExecute(() => { MethodImports.Vanguard_closeGame(); }, true);
                    }
					break;

				case RTCV.NetCore.Commands.Remote.DomainGetDomains:
					SyncObjectSingleton.FormExecute(() =>
					{
                        e.setReturnValue(GetInterfaces());
					});
					break;

				case RTCV.NetCore.Commands.Remote.DomainRefreshDomains:
					{
						RefreshDomains(); 
					}
					break;

                case RTCV.NetCore.Commands.Remote.KeySetSyncSettings:
					{
						ConsoleEx.WriteLine("Setting Sync Settings");
						String settings = advancedMessage.objectValue as string;

                        MethodImports.Vanguard_loadEmuSettings(settings);
                        AllSpec.VanguardSpec.Update(VSPEC.SYNCSETTINGS, settings);

                        ConsoleEx.WriteLine("Loaded settings: \n" + settings);
                    }
					break;

				case RTCV.NetCore.Commands.Emulator.GetRealtimeAPI:
					{ 
						e.setReturnValue(VanguardCore.RTE_API); 
					}
					break;

				case RTCV.NetCore.Commands.Remote.EventEmuStarted:
					break;

				case RTCV.NetCore.Commands.Remote.OpenHexEditor:
					break;

				case RTCV.NetCore.Commands.Remote.EventCloseEmulator:
                    {
                        // Close the hex editor if it's open
                        RtcCore.InvokeKillHexEditor();

                        // Since some emulators close the game first when receiving a close emulator command, we need
						// to make sure that RTC doesn't try to refresh domains when the GAMECLOSED signal is sent
                        if (AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME].ToString() != "")
						{
							waitForEmulatorClose = true;
                        }

                        // Close the emulator
                        MethodImports.Vanguard_forceStop();


                        if (!waitForEmulatorClose)
						{
							VanguardCore.StopVanguard();
                        }
					}
                    break;
				case RTCV.NetCore.Commands.Remote.LoadFailed:
					{
                        PartialSpec gameClosed = new PartialSpec("VanguardSpec");
                        gameClosed[VSPEC.OPENROMFILENAME] = "";
                        gameClosed[VSPEC.GAMENAME] = "";
                        AllSpec.VanguardSpec.Update(gameClosed);
                        RtcCore.InvokeGameClosed(true);
                        VanguardImplementation.RefreshDomains();
                    }
					break;
			}
		}
    }
}
