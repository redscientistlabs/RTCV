using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Ceras;
using Newtonsoft.Json;
using RTCV.CorruptCore;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class MemoryDomains
	{

		public static Dictionary<string, MemoryDomainProxy> MemoryInterfaces
		{
			get => RTCV.NetCore.AllSpec.CorruptCoreSpec?["MEMORYINTERFACES"] as Dictionary<string, MemoryDomainProxy>;
			set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update("MEMORYINTERFACES", value);
		}

		public static Dictionary<string, VirtualMemoryDomain> VmdPool = new Dictionary<string, VirtualMemoryDomain>();

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");

			partial["MEMORYINTERFACES"] = new Dictionary<string, MemoryInterface>();

			return partial;
		}


		public static void RefreshDomains(bool domainsChanged = false)
		{
			var temp = new Dictionary<string, MemoryDomainProxy>();
			var mdps = (MemoryDomainProxy[])RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_INTERFACES];
			foreach (MemoryDomainProxy mdp in mdps)
				temp.Add(mdp.ToString(), mdp);
			MemoryInterfaces = temp;

			if(domainsChanged)
				LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_EVENT_DOMAINSUPDATED, true);
		}


		public static void Clear()
		{
			MemoryInterfaces?.Clear();
		}

		public static MemoryDomainProxy GetProxy(string domain, long address)
		{
			if (domain == null)
				return null;

			if (MemoryInterfaces.Count == 0)
				RefreshDomains();

			if (MemoryInterfaces.TryGetValue(domain, out MemoryDomainProxy mdp))
				return mdp;

			if (VmdPool.TryGetValue(domain, out VirtualMemoryDomain vmd))
				return GetProxy(vmd.GetRealDomain(address), vmd.GetRealAddress(address));

			return null;
		}

		public static MemoryInterface GetInterface(string _domain)
		{
			if (_domain == null)
				return null;

			if (MemoryInterfaces.Count == 0)
				RefreshDomains();
			
			if (MemoryInterfaces.TryGetValue(_domain, out MemoryDomainProxy mi))
				return mi;

			if (VmdPool.TryGetValue(_domain, out VirtualMemoryDomain vmd))
				return vmd;

			return null;
		}


		public static long GetRealAddress(string domain, long address)
		{
			if (domain.Contains("[V]"))
			{
				MemoryInterface mi = VmdPool[domain];
				VirtualMemoryDomain vmd = ((VirtualMemoryDomain)mi);
				return vmd.GetRealAddress(address);
			}
			else
				return address;
		}

		public static string GetRealDomain(string domain, long address)
		{
			if (domain.Contains("[V]"))
			{
				MemoryInterface mi = VmdPool[domain];
				VirtualMemoryDomain vmd = ((VirtualMemoryDomain)mi);
				return vmd.GetRealDomain(address);
			}
			else
				return domain;
		}

		public static void GenerateVmdFromStashkey(StashKey sk)
		{
			VmdPrototype proto = new VmdPrototype(sk.BlastLayer);
			AddVMD(proto);
		}

		/// <summary>
		/// This is one of the rare cases where a method is netcore redundant
		/// We don't use a spec for this because VMDs can get huge, and as such, we keep the dictionary synced on both sides manually.
		/// </summary>
		/// <param name="proto"></param>
		public static void AddVMD(VmdPrototype proto) => AddVMD(proto.Generate());

		/// <summary>
		/// This is one of the rare cases where a method is netcore redundant
		/// We don't use a spec for this because VMDs can get huge, and as such, we keep the dictionary synced on both sides manually.
		/// </summary>
		/// <param name="VMD"></param>
		public static void AddVMD(VirtualMemoryDomain VMD)
		{
			MemoryDomains.VmdPool[VMD.ToString()] = VMD;

			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_DOMAIN_VMD_ADD, VMD.Proto, true);
			LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_EVENT_DOMAINSUPDATED);
		}


		public static void AddVMD_NET(VmdPrototype proto) => AddVMD_NET(proto.Generate());
		public static void AddVMD_NET(VirtualMemoryDomain VMD)
		{
			MemoryDomains.VmdPool[VMD.ToString()] = VMD;
		}

		/// <summary>
		/// This is one of the rare cases where a method is netcore redundant
		/// We don't use a spec for this because VMDs can get huge, and as such, we keep the dictionary synced on both sides manually.
		/// </summary>
		/// <param name="VMD"></param>
		public static void RemoveVMD(VirtualMemoryDomain VMD) => RemoveVMD(VMD.ToString());
		/// <summary>
		/// This is one of the rare cases where a method is netcore redundant
		/// We don't use a spec for this because VMDs can get huge, and as such, we keep the dictionary synced on both sides manually.
		/// </summary>
		/// <param name="vmdName"></param>
		public static void RemoveVMD(string vmdName)
		{
			if (MemoryDomains.VmdPool.ContainsKey(vmdName))
				MemoryDomains.VmdPool.Remove(vmdName);

			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_DOMAIN_VMD_REMOVE, vmdName, true);
			LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_EVENT_DOMAINSUPDATED);
		}

		public static void RemoveVMD_NET(VirtualMemoryDomain VMD) => RemoveVMD_NET(VMD.ToString());
		public static void RemoveVMD_NET(string vmdName)
		{
			if (MemoryDomains.VmdPool.ContainsKey(vmdName))
				MemoryDomains.VmdPool.Remove(vmdName);
		}

		public static void GenerateActiveTableDump(string domain, string key)
		{
			if(!MemoryInterfaces.ContainsKey(domain))
				return;

			MemoryInterface mi = MemoryInterfaces[domain];

			byte[] dump = mi.GetDump();

			File.WriteAllBytes(CorruptCore.workingDir + Path.DirectorySeparatorChar + "MEMORYDUMPS" + Path.DirectorySeparatorChar + key + ".dmp", dump.ToArray());
		}

		public static byte[] GetDomainData(string domain)
		{
			MemoryInterface mi = domain.Contains("[V]") ? (MemoryInterface)VmdPool[domain] : MemoryInterfaces[domain];
			return mi.GetDump();
		}


		private static bool CheckNesHeader(string filename)
		{
			byte[] buffer = new byte[4];
			using (Stream fs = File.OpenRead(filename))
			{
				fs.Read(buffer, 0, buffer.Length);
			}
			if (!buffer.SequenceEqual(Encoding.ASCII.GetBytes("NES\x1A")))
				return false;
			return true;
		}

		public static RomParts GetRomParts(string thisSystem, string romFilename)
		{
			RomParts rp = new RomParts();

			switch (thisSystem.ToUpper())
			{
				case "NES":     //Nintendo Entertainment System

					//There's no easy way to discern NES from FDS so just check for the domain name
					if (MemoryDomains.MemoryInterfaces.ContainsKey("PRG ROM"))
						rp.PrimaryDomain = "PRG ROM";
					else
					{
						rp.Error = "Unfortunately, Bizhawk doesn't support editing the ROM (FDS Side) domain of FDS games. Maybe in a future version...";
						break;
					}

					if (MemoryDomains.MemoryInterfaces.ContainsKey("CHR VROM"))
						rp.SecondDomain = "CHR VROM";
					//Skip the first 16 bytes if there's an iNES header
					if (CheckNesHeader(romFilename))
						rp.SkipBytes = 16;
					break;

				case "SNES":    //Super Nintendo
					if (MemoryDomains.MemoryInterfaces.ContainsKey("SGB CARTROM")) //BSNES SGB Mode
						rp.PrimaryDomain = "SGB CARTROM";
					else
					{
						rp.PrimaryDomain = "CARTROM";

						long filesize = new System.IO.FileInfo(romFilename).Length;

						if (filesize % 1024 != 0)
							rp.SkipBytes = 512;
					}

					break;

				case "LYNX":    //Atari Lynx
					rp.PrimaryDomain = "Cart A";
					rp.SkipBytes = 64;
					break;

				case "N64":     //Nintendo 64
				case "GB":      //Gameboy
				case "GBC":     //Gameboy Color
				case "SMS":     //Sega Master System
				case "GBA":     //Game Boy Advance
				case "PCE":     //PC Engine
				case "GG":      //Game Gear
				case "SG":      //SG-1000
				case "SGX":     //PC Engine SGX
				case "WSWAN":   //Wonderswan
				case "VB":      //Virtualboy
				case "NGP":     //Neo Geo Pocket
					rp.PrimaryDomain = "ROM";
					break;

				case "GEN":     // Sega Genesis
					if (MemoryDomains.MemoryInterfaces.ContainsKey("MD CART"))  //If it's regular Genesis or 32X
					{
						rp.PrimaryDomain = "MD CART";

						if (romFilename.ToUpper().Contains(".SMD"))
							rp.SkipBytes = 512;
					}
					else
					{    //If it's in Sega CD mode
						rp.Error = "Unfortunately, Bizhawk doesn't support editing the ISOs while it is running. Maybe in a future version...";
					}
					break;

				case "PCFX":    //PCFX
				case "PCECD":   //PC Engine CD
				case "SAT":     //Sega Saturn
				case "PSX":     //Playstation
					rp.Error = "Unfortunately, Bizhawk doesn't support editing the ISOs while it is running. Maybe in a future version...";
					break;
				default:
					rp.Error = "The RTC devs haven't added support for this system. Go yell at them to make it work.";
					break;
			}

			return rp;
		}


	}

	public class RomParts
	{
		public string Error { get; set; }
		public string PrimaryDomain { get; set; }
		public string SecondDomain { get; set; }
		public int SkipBytes { get; set; }
	}

	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public abstract class MemoryInterface
	{
		public abstract long Size { get; set; }
		public int WordSize { get; set; }
		public string Name { get; set; }
		public bool BigEndian { get; set; }

		public abstract byte[] GetDump();

		public abstract byte[] PeekBytes(long startAddress, long endAddress, bool raw);

		public abstract byte PeekByte(long address);

		public abstract void PokeByte(long address, byte value);

		public MemoryInterface()
		{

		}
	}

	[XmlInclude(typeof(BlastLayer))]
	[XmlInclude(typeof(BlastUnit))]
	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public class VmdPrototype
	{
		public string VmdName { get; set; }
		public string GenDomain { get; set; }
		public bool BigEndian { get; set; }
		public int WordSize { get; set; }
		public long PointerSpacer { get; set; }

		public long Padding { get; set; }

		public List<long> AddSingles { get; set; } = new List<long>();
		public List<long> RemoveSingles { get; set; } = new List<long>();

		public List<long[]> AddRanges { get; set; } = new List<long[]>();
		public List<long[]> RemoveRanges { get; set; } = new List<long[]>();

		public BlastLayer SuppliedBlastLayer = null;

		public VmdPrototype()
		{

		}

		public VmdPrototype(BlastLayer bl)
		{
			VmdName = CorruptCore.GetRandomKey();
			GenDomain = "Hybrid";

			BlastUnit bu = bl.Layer[0];
			MemoryInterface mi = MemoryDomains.GetInterface(bu.Domain);
			BigEndian = mi.BigEndian;
			WordSize = mi.WordSize;
			SuppliedBlastLayer = bl;
		}

		public VirtualMemoryDomain Generate()
		{
			VirtualMemoryDomain VMD = new VirtualMemoryDomain
			{
				Proto = this,
				Name = VmdName,
				BigEndian = BigEndian,
				WordSize = WordSize
			};


			if (SuppliedBlastLayer != null)
			{
				VMD.AddFromBlastLayer(SuppliedBlastLayer);
				return VMD;
			}

			int addressCount = 0;
			for (int i = 0; i < Padding; i++)
			{
				VMD.PointerDomains.Add(GenDomain);
				VMD.PointerAddresses.Add(i);
			}

			foreach (long[] range in AddRanges)
			{
				long start = range[0];
				long end = range[1];
				if (end < start)
					continue;

				for (long i = start; i < end; i++)
				{
					if (!IsAddressInRanges(i, RemoveSingles, RemoveRanges))
						if (PointerSpacer == 1 || addressCount % PointerSpacer == 0)
						{
							VMD.PointerDomains.Add(GenDomain);
							VMD.PointerAddresses.Add(i);
						}
					addressCount++;
				}
			}

			foreach (long single in AddSingles)
			{
				VMD.PointerDomains.Add(GenDomain);
				VMD.PointerAddresses.Add(single);
				addressCount++;
			}

			return VMD;
		}

		public bool IsAddressInRanges(long address, List<long> singles, List<long[]> ranges)
		{
			if (singles.Contains(address))
				return true;

			foreach (long[] range in ranges)
			{
				long start = range[0];
				long end = range[1];

				if (address >= start && address < end)
					return true;
			}

			return false;
		}
	}

	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public class VirtualMemoryDomain : MemoryInterface
	{
		public List<string> PointerDomains { get; set; }  =  new List<string>();
		public List<long> PointerAddresses { get; set; } = new List<long>();
		public VmdPrototype Proto { get; set; }


		public VirtualMemoryDomain()
		{

		}

		public override long Size { get => PointerDomains.Count;
			set { } }

		public void AddFromBlastLayer(BlastLayer bl)
		{
			if (bl == null)
				return;

			foreach (BlastUnit bu in bl.Layer)
			{
				PointerDomains.Add(bu.Domain);
				for (int i = 0; i < bu.Precision; i++)
				{
					PointerAddresses.Add(bu.Address);
				}
					
			}
		}

		public string GetRealDomain(long address)
		{
			if (address < 0 || address > PointerDomains.Count)
				return "ERROR";

			return PointerDomains[(int)address];
		}

		public long GetRealAddress(long address)
		{
			if (address < 0 || address > PointerAddresses.Count || address < Proto.Padding)
				return 0;
			return PointerAddresses[(int)address];
		}

		public byte[] ToData()
		{
			VirtualMemoryDomain VMD = this;

			using (MemoryStream serialized = new MemoryStream())
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(serialized, VMD);

				using (MemoryStream input = new MemoryStream(serialized.ToArray()))
				using (MemoryStream output = new MemoryStream())
				{
					using (GZipStream zip = new GZipStream(output, CompressionMode.Compress))
					{
						input.CopyTo(zip);
					}

					return output.ToArray();
				}
			}
		}

		public static VirtualMemoryDomain FromData(byte[] data)
		{
			using (MemoryStream input = new MemoryStream(data))
			using (MemoryStream output = new MemoryStream())
			{
				using (GZipStream zip = new GZipStream(input, CompressionMode.Decompress))
				{
					zip.CopyTo(output);
				}

				var binaryFormatter = new BinaryFormatter();

				using (MemoryStream serialized = new MemoryStream(output.ToArray()))
				{
					VirtualMemoryDomain VMD = (VirtualMemoryDomain)binaryFormatter.Deserialize(serialized);
					return VMD;
				}
			}
		}

		public override string ToString()
		{
			//Virtual Memory Domains always start with [V]
			return "[V]" + Name;
		}

		public override byte[] GetDump()
		{
			return PeekBytes(0, Size);
		}

		public override byte[] PeekBytes(long startAddress, long endAddress, bool raw = true)
		{
			//endAddress is exclusive
			List<byte> data = new List<byte>();
			for (long i = startAddress; i < endAddress; i++)
				data.Add(PeekByte(i));

			if (raw || BigEndian)
				return data.ToArray();
			else
				return data.ToArray().FlipBytes();
		}

		public override byte PeekByte(long address)
		{
			if (address < this.Proto.Padding)
				return (byte)0;
			if (address > this.Size - 1)
				return 0;
			string targetDomain = GetRealDomain(address);
			long targetAddress = GetRealAddress(address);

			MemoryDomainProxy mdp = MemoryDomains.GetProxy(targetDomain, targetAddress);

			return mdp?.PeekByte(targetAddress) ?? 0;
		}

		public override void PokeByte(long address, byte value)
		{
			if (address < this.Proto.Padding)
				return;
			if (address > this.Size - 1)
				return;

			string targetDomain = GetRealDomain(address);
			long targetAddress = GetRealAddress(address);

			MemoryDomainProxy mdp = MemoryDomains.GetProxy(targetDomain, targetAddress);

			mdp?.PokeByte(targetAddress, value);
		}
	}


	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public sealed class MemoryDomainProxy : MemoryInterface
	{
		[NonSerialized , Ceras.Exclude]
		public IMemoryDomain MD = null;
		public override long Size { get; set; }
		public MemoryDomainProxy(IMemoryDomain _md)
		{
			MD = _md;
			Size = MD.Size;

			Name = MD.ToString();


			WordSize = MD.WordSize;
			Name = MD.ToString();
			BigEndian = MD.BigEndian;
		}
		public MemoryDomainProxy()
		{
		}
		public override string ToString()
		{
			return Name;
		}

		public override byte[] GetDump()
		{
			return PeekBytes(0, Size);
		}

		public override byte[] PeekBytes(long startAddress, long endAddress, bool raw = true)
		{
			//endAddress is exclusive
			List<byte> data = new List<byte>();
			for (long i = startAddress; i < endAddress; i++)
				data.Add(PeekByte(i));

			if(raw || BigEndian)
				return data.ToArray();
			else
				return data.ToArray().FlipBytes();
		}

		public override byte PeekByte(long address)
		{
			if (address > Size - 1)
				return 0;
			return MD.PeekByte(address);
		}

		public override void PokeByte(long address, byte value)
		{
			if (address > Size - 1)
				return;

			MD.PokeByte(address, value);
		}
	}

	public interface IMemoryDomain
	{
		string Name { get; }
		long Size { get; }
		int WordSize { get; }
		bool BigEndian { get; }

		byte PeekByte(long addr);
		void PokeByte(long addr, byte val);
	}

}
