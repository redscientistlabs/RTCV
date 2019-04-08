using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV;
using RTCV.CorruptCore;
using static RTCV.NetCore.NetcoreCommands;

namespace TestVanguardImplemented
{
	public static class TestVanguardImplementation
	{
		public static RTCV.Vanguard.VanguardConnector connector = null;


		public static void StartClient()
		{
			try
			{
				ConsoleEx.WriteLine("Starting Vanguard Client");
				Thread.Sleep(500); //When starting in Multiple Startup Project, the first try will be uncessful since
								   //the server takes a bit more time to start then the client.

				var spec = new NetCoreReceiver();
				spec.Attached = VanguardCore.attached;
				spec.MessageReceived += OnMessageReceived;

				connector = new RTCV.Vanguard.VanguardConnector(spec);

			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();
			}
		}

		public static void RestartClient()
		{
			connector?.Kill();
			connector = null;
			StartClient();
		}

		private static void OnMessageReceived(object sender, NetCoreEventArgs e)
		{
			try
			{
				// This is where you implement interaction.
				// Warning: Any error thrown in here will be caught by NetCore and handled by being displayed in the console.

				var message = e.message;
				var simpleMessage = message as NetCoreSimpleMessage;
				var advancedMessage = message as NetCoreAdvancedMessage;

				ConsoleEx.WriteLine(message.Type);
				switch (message.Type) //Handle received messages here
				{
					case "INFINITELOOP":
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							while (true)
							{
								Thread.Sleep(10);
							}
						});
						break;


					case REMOTE_ALLSPECSSENT:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							VanguardCore.LoadDefaultRom();
							Program.SpecsSent = true; 

						});
						break;

					case SAVESAVESTATE:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							e.setReturnValue(VanguardCore.SaveSavestate_NET(advancedMessage.objectValue as string));
						});
						break;

					case LOADSAVESTATE:
						{
							var cmd = advancedMessage.objectValue as object[];
							var path = cmd[0] as string;
							var location = (StashKeySavestateLocation)cmd[1];
							SyncObjectSingleton.FormExecute((o, ea) =>
							{
								e.setReturnValue(VanguardCore.LoadSavestate_NET(path, location));
							});
							break;
						}

					case REMOTE_LOADROM:
						{
							var fileName = advancedMessage.objectValue as String;
							SyncObjectSingleton.FormExecute((o, ea) =>
							{
								VanguardCore.LoadRom_NET(fileName);
							});

						}
						break;

					case REMOTE_DOMAIN_GETDOMAINS:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							e.setReturnValue(Hooks.GetInterfaces());
						});
						break;

					case REMOTE_KEY_SETSYNCSETTINGS:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							Hooks.EMU_GETSET_SYNCSETTINGS = (string)advancedMessage.objectValue;
						});
						break;

					case REMOTE_KEY_SETSYSTEMCORE:
						{
							var cmd = advancedMessage.objectValue as object[];
							var systemName = (string)cmd[0];
							var systemCore = (string)cmd[1];
							SyncObjectSingleton.FormExecute((o, ea) =>
							{
								Hooks.EMU_SET_SYSTEMCORE(systemName, systemCore);
							});
						}
						break;

					case EMU_OPEN_HEXEDITOR_ADDRESS:
						{
							var temp = advancedMessage.objectValue as object[];
							string domain = (string)temp[0];
							long address = (long)temp[1];

							MemoryDomainProxy mdp = MemoryDomains.GetProxy(domain, address);
							long realAddress = MemoryDomains.GetRealAddress(domain, address);

							SyncObjectSingleton.FormExecute((o, ea) =>
							{
								Hooks.EMU_OPEN_HEXEDITOR_ADDRESS(mdp, realAddress);
							});

							break;
						}
					case REMOTE_EVENT_EMU_MAINFORM_CLOSE:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							Hooks.EMU_MAINFORM_CLOSE();
						});
						break;

					case REMOTE_EVENT_SAVEBIZHAWKCONFIG:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							Hooks.EMU_MAINFORM_SAVECONFIG();
						});
						break;


					case REMOTE_IMPORTKEYBINDS:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							Hooks.EMU_IMPORTCONFIGINI(CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "import_config.ini", CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "stockpile_config.ini");
						});
						break;

					case REMOTE_MERGECONFIG:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							Hooks.EMU_MERGECONFIGINI(CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "backup_config.ini", CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "stockpile_config.ini");
						});
						break;

					case REMOTE_RESTOREBIZHAWKCONFIG:
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							Process.Start(CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + $"RestoreConfigDETACHED.bat");
						});
						break;

					case REMOTE_ISNORMALADVANCE:
						e.setReturnValue(Hooks.isNormalAdvance);
						break;
				}
			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();
			}
		}


		public class TestMemoryDomain : IMemoryDomain
		{
			public Byte[] MD;
			public string Name { get; set; }
			public long Size { get; set; }
			public int WordSize { get; set; }
			public bool BigEndian { get; set; }

			public TestMemoryDomain(byte[] md, string name, long size, int wordsize, bool bigendian)
			{
				MD = md;
				Name = name;
				Size = size;
				WordSize = wordsize;
				BigEndian = bigendian;

			}

			public byte PeekByte(long addr)
			{
				return MD[addr];
			}

			public void PokeByte(long addr, byte val)
			{
				MD[addr] = val;
			}

			public override string ToString()
			{
				return Name;
			}

		}
	}
}
