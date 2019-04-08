using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class StockpileManager_EmuSide
	{


		public static bool RenderAtLoad
		{
			get => (bool)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.RENDER_AT_LOAD.ToString()];
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.RENDER_AT_LOAD.ToString(), value);
		}


		public static BlastLayer CorruptBL = null;
		public static BlastLayer UnCorruptBL = null;


		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");
			partial[RTCSPEC.RENDER_AT_LOAD.ToString()] = false;
			return partial;
		}

		public static bool LoadState_NET(StashKey sk, bool reloadRom = true, bool applyBlastLayer = true)
		{
			if (sk == null)
				return false;

			StashKey.SetCore(sk);
			string gameSystem = sk.SystemName;
			string gameName = sk.GameName;
			string key = sk.ParentKey;
			StashKeySavestateLocation stateLocation = sk.StateLocation;

			if (reloadRom)
			{
				LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_LOADROM, sk.RomFilename, true);

				string ss = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.SYNCSETTINGS.ToString()];
				//If the syncsettings are different, update them and load it again. Otheriwse, leave as is
				if (sk.SyncSettings != ss && sk.SyncSettings != null)
				{
					LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_KEY_SETSYNCSETTINGS, sk.SyncSettings, true);
					LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_LOADROM, sk.RomFilename, true);
				}
			}

			string theoreticalSaveStateFilename = CorruptCore.workingDir + Path.DirectorySeparatorChar + stateLocation.ToString() + Path.DirectorySeparatorChar + gameName + "." + key + ".timejump.State";

			if (File.Exists(theoreticalSaveStateFilename))
			{
				if (!LocalNetCoreRouter.QueryRoute<bool>(NetcoreCommands.VANGUARD, NetcoreCommands.LOADSAVESTATE, new object[] { theoreticalSaveStateFilename, stateLocation }, true))
				{
					MessageBox.Show($"Error loading savestate : An internal Bizhawk error has occurred.\n Are you sure your savestate matches the game, your syncsettings match, and the savestate is supported by this version of Bizhawk?");
					return false;
				}
			}
			else
			{
				MessageBox.Show($"Error loading savestate : (File {theoreticalSaveStateFilename} not found)");
				return false;
			}

			if (applyBlastLayer && sk?.BlastLayer?.Layer?.Count > 0)
			{
				CorruptBL = sk.BlastLayer;
				sk.BlastLayer.Apply(true);
			}

			return true;
		}


		public static StashKey SaveState_NET(StashKey _sk = null, bool threadSave = false)
		{
			string Key = CorruptCore.GetRandomKey();
			string statePath;

			StashKey sk;

			if (_sk == null)
			{
				Key = CorruptCore.GetRandomKey();
				statePath = LocalNetCoreRouter.QueryRoute<String>(NetcoreCommands.VANGUARD, NetcoreCommands.SAVESAVESTATE, Key, true);
				sk = new StashKey(Key, Key, null);
			}
			else
			{
				Key = _sk.Key;
				statePath = _sk.StateFilename;
				sk = _sk;
			}

			if (statePath == null)
				return null;

			//sk.StateShortFilename = statePath.Substring(statePath.LastIndexOf(Path.DirectorySeparatorChar) + 1, statePath.Length - (statePath.LastIndexOf(Path.DirectorySeparatorChar) + 1));
			sk.StateShortFilename = Path.GetFileName(statePath);
			sk.StateFilename = statePath;

			return sk;
		}


		public static StashKey GetRawBlastlayer()
		{
			StashKey sk = SaveState_NET();

			BlastLayer bl = new BlastLayer();

			bl.Layer.AddRange(StepActions.GetRawBlastLayer().Layer);

			string thisSystem = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.SYSTEM.ToString()];
			string romFilename = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME.ToString()];

			var rp = MemoryDomains.GetRomParts(thisSystem, romFilename);

			if (rp.Error == null)
			{
				if (rp.PrimaryDomain != null)
				{
					List<byte> addData = new List<byte>();

					if (rp.SkipBytes != 0)
					{
						byte[] padding = new byte[rp.SkipBytes];
						for (int i = 0; i < rp.SkipBytes; i++)
							padding[i] = 0;

						addData.AddRange(padding);
					}

					addData.AddRange(MemoryDomains.GetDomainData(rp.PrimaryDomain));
					if (rp.SecondDomain != null)
						addData.AddRange(MemoryDomains.GetDomainData(rp.SecondDomain));

					byte[] corrupted = addData.ToArray();
					byte[] original = File.ReadAllBytes(romFilename);

					if (MemoryDomains.MemoryInterfaces.ContainsKey("32X FB")) //Flip 16-bit words on 32X rom
						original = original.FlipWords(2);
					else if (thisSystem.ToUpper() == "N64")
						original = MutateSwapN64(original);
					else if (romFilename.ToUpper().Contains(".SMD"))
						original = DeInterleaveSMD(original);

					for (int i = 0; i < rp.SkipBytes; i++)
						original[i] = 0;

					BlastLayer romBlast = BlastTools.GetBlastLayerFromDiff(original, corrupted);

					if (romBlast != null && romBlast.Layer.Count > 0)
						bl.Layer.AddRange(romBlast.Layer);
				}
			}

			sk.BlastLayer = bl;

			return sk;
		}


		//From Bizhawk
		public static byte[] DeInterleaveSMD(byte[] source)
		{
			// SMD files are interleaved in pages of 16k, with the first 8k containing all 
			// odd bytes and the second 8k containing all even bytes.
			int size = source.Length;
			if (size > 0x400000)
			{
				size = 0x400000;
			}

			int pages = size / 0x4000;
			var output = new byte[size];

			for (int page = 0; page < pages; page++)
			{
				for (int i = 0; i < 0x2000; i++)
				{
					output[(page * 0x4000) + (i * 2) + 0] = source[(page * 0x4000) + 0x2000 + i];
					output[(page * 0x4000) + (i * 2) + 1] = source[(page * 0x4000) + 0x0000 + i];
				}
			}

			return output;
		}
		//From Bizhawk
		public static unsafe byte[] MutateSwapN64(byte[] source)
		{
			// N64 roms are in one of the following formats:
			//  .Z64 = No swapping
			//  .N64 = Word Swapped
			//  .V64 = Byte Swapped

			// File extension does not always match the format
			int size = source.Length;

			// V64 format
			fixed (byte* pSource = &source[0])
			{
				if (pSource[0] == 0x37)
				{
					for (int i = 0; i < size; i += 2)
					{
						byte temp = pSource[i];
						pSource[i] = pSource[i + 1];
						pSource[i + 1] = temp;
					}
				}

				// N64 format
				else if (pSource[0] == 0x40)
				{
					for (int i = 0; i < size; i += 4)
					{
						// output[i] = source[i + 3];
						// output[i + 3] = source[i];
						// output[i + 1] = source[i + 2];
						// output[i + 2] = source[i + 1];
						byte temp = pSource[i];
						pSource[i] = source[i + 3];
						pSource[i + 3] = temp;

						temp = pSource[i + 1];
						pSource[i + 1] = pSource[i + 2];
						pSource[i + 2] = temp;
					}
				}
				else // Z64 format (or some other unknown format)
				{
				}
			}

			return source;
		}
	}
}
