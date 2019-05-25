using Ceras;
using Newtonsoft.Json;
using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

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


        public static VmdPrototype GetVmdPrototypeFromBlastlayer(BlastLayer bl)
        {
            //If the BL references a VMD that doesn't exist, return null
            if (bl.Layer.Any(x => MemoryDomains.GetInterface(x.Domain) == null))
                return null;

            VmdPrototype proto = new VmdPrototype();
            proto.VmdName = CorruptCore.GetRandomKey();
            proto.GenDomain = "Hybrid";

            BlastUnit bu = bl.Layer[0];
            MemoryInterface mi = MemoryDomains.GetInterface(bu.Domain);
            proto.BigEndian = mi.BigEndian;
            proto.WordSize = mi.WordSize;
            proto.SuppliedBlastLayer = bl;
            return proto;
        }
        public static void GenerateVmdFromStashkey(StashKey sk)
		{
			VmdPrototype proto = GetVmdPrototypeFromBlastlayer(sk.BlastLayer);
            if (proto == null)
                MessageBox.Show("The resulting layer was empty or contained invalid data (unloaded VMD?)");
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

			File.WriteAllBytes(Path.Combine(CorruptCore.workingDir,"MEMORYDUMPS",key + ".dmp"), dump.ToArray());
		}

		public static byte[] GetDomainData(string domain)
		{
			MemoryInterface mi = domain.Contains("[V]") ? (MemoryInterface)VmdPool[domain] : MemoryInterfaces[domain];
			return mi.GetDump();
		}


		private static bool CheckNesHeader(string filename)
		{
			byte[] buffer = new byte[4];
			using (Stream fs = File.Open(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
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
                VMD.Compact();
				return VMD;
			}

			int addressCount = 0;
			for (int i = 0; i < Padding; i++)
			{
				//VMD.PointerDomains.Add(GenDomain);
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
							//VMD.PointerDomains.Add(GenDomain);
							VMD.PointerAddresses.Add(i);
						}
					addressCount++;
				}
			}

			foreach (long single in AddSingles)
			{
				//VMD.PointerDomains.Add(GenDomain);
				VMD.PointerAddresses.Add(single);
				addressCount++;
			}

            VMD.CompactPointerDomains = new string[] { GenDomain };
            VMD.CompactPointerAddresses = new long[][] { VMD.PointerAddresses.ToArray() };

            VMD.Compact(true);

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

        public string[] CompactPointerDomains { get; set; } = null;

        public long[][] CompactPointerAddresses { get; set; } = null;

        public VmdPrototype Proto { get; set; }

        public bool Compacted = false;



        public VirtualMemoryDomain()
		{

		}

        public void Compact(bool preCompacted = false)
        {
            if (Compacted || preCompacted)
            {
                PointerAddresses.Clear();
                PointerDomains.Clear();

                Compacted = true;

                GC.Collect();
                GC.WaitForFullGCComplete();

                return;
            }

            if (PointerDomains.Count == 0 && PointerAddresses.Count == 0)
                return;

            List<string> domains = new List<string>();
            List<List<long>> domainAdresses = new List<List<long>>();

            for(int i= 0; i< PointerAddresses.Count();i++)
            {
                var dom = PointerDomains[i];
                if (!domains.Contains(dom))
                {
                    domains.Add(dom);
                    domainAdresses.Add(new List<long>());
                }

                int domainIndex = domains.FindIndex(it => it == dom);
                domainAdresses[domainIndex].Add(PointerAddresses[i]);
            }

            CompactPointerDomains = domains.ToArray();
            CompactPointerAddresses = domainAdresses.Select(addressArray => addressArray.OrderBy(address => address).ToArray()).ToArray();

            PointerAddresses.Clear();
            PointerDomains.Clear();

            Compacted = true;

            GC.Collect();
            GC.WaitForFullGCComplete();

        }

		public override long Size
        {
            get
            {
                if (Compacted)
                    return CompactPointerAddresses.Sum(it => it.Length);
                else
                    return PointerAddresses.Count;
            }
            set{ }
        }

		public void AddFromBlastLayer(BlastLayer bl)
		{
			if (bl == null)
				return;

            bl.SanitizeDuplicates();


            foreach (BlastUnit bu in bl.Layer)
			{
				for (int i = 0; i < bu.Precision; i++)
				{ 
                    PointerDomains.Add(bu.Domain);
                    PointerAddresses.Add(bu.Address);
				}
					
			}
		}

        private int GetCompactedDomainIndexFromAddress(long address)
        {
            return 0;
        }
		public string GetRealDomain(long address)
		{
            if (Compacted)
            {
                return CompactPointerDomains[GetCompactedDomainIndexFromAddress(address)];
            }
            else
            {
                if (address < 0 || address > PointerDomains.Count)
                    return "ERROR";

                return PointerDomains[(int)address];
            }
		}

        public long GetRealAddress(long address)
        {
            if (Compacted)
            {
                long currentBankStartAddress = 0;


                foreach (long[] addressBank in CompactPointerAddresses)
                {

                    if (address < (currentBankStartAddress + addressBank.Length)) // are we in the right bank?
                            return addressBank[address - currentBankStartAddress];
                        else
                            currentBankStartAddress += addressBank.Length;


                }
                return 0; //failure
            }
            else
            {
                if (address < 0 || address > PointerAddresses.Count || address < Proto.Padding)
                    return 0;
                return PointerAddresses[(int)address];
            }
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


    [Serializable()]
    public abstract class FileMemoryInterface : IMemoryDomain
    {
        public abstract string Name { get; }

        public abstract long Size { get; }

        public abstract int WordSize { get;  }
        public abstract bool BigEndian { get;}

        public abstract void CloseStream();
        public abstract void getMemoryDump();
        public abstract void wipeMemoryDump();
        public abstract bool dolphinSavestateVersion();
        public abstract byte[][] lastMemoryDump { get; set; }
        public abstract bool cacheEnabled { get; }

        public abstract long getMemorySize();
        public abstract long? lastMemorySize { get; set; }

        //public abstract Dictionary<String, String> CompositeFilenameDico { get; set; }

        public abstract void PokeByte(long address, byte data);
        public abstract void PokeBytes(long address, byte[] data);
        public abstract byte PeekByte(long address);
        public abstract byte[] PeekBytes(long address, int length);

        public abstract void SetBackup();
        public abstract void ResetBackup(bool askConfirmation = true);
        public abstract void RestoreBackup(bool announce = true);
        public abstract void ResetWorkingFile();
        public abstract void ApplyWorkingFile();

        public volatile System.IO.Stream stream = null;
    }

    [Serializable()]
    public class FileInterface : FileMemoryInterface
    {
        //File management
        public static Dictionary<String, String> CompositeFilenameDico { get; set; }

        public static FileInterfaceIdentity identity = FileInterfaceIdentity.SELF_DESCRIBE;
        public override string Name => ShortFilename;
        public override long Size => lastMemorySize.GetValueOrDefault(0);

        public override bool BigEndian { get; }
        public override int WordSize => 4;

        public string Filename;
        public string ShortFilename = null;

        public MultipleFileInterface parent = null;
        public override byte[][] lastMemoryDump { get; set; } = null;
        public override bool cacheEnabled { get { return lastMemoryDump != null; } }

        //lastMemorySize gets rounded up to a multiplier of 4 to make the vector engine work on multiple files
        //lastRealMemorySize is used in peek/poke to cancel out non-existing adresses
        public override long? lastMemorySize { get; set; }
        public long? lastRealMemorySize { get; set; }
        public bool useAutomaticFileBackups { get; set; } = false;

        public long MultiFilePosition = 0;
        public long MultiFilePositionCeiling = 0;
        private static readonly bool writeCopyMode = false;

        public string InterfaceUniquePrefix = "";

        public override string ToString()
        {
            switch(identity)
            {
                case FileInterfaceIdentity.HASHED_PREFIX:
                    return InterfaceUniquePrefix + ":" + ShortFilename;
                case FileInterfaceIdentity.FULL_PATH:
                    return Filename;
                case FileInterfaceIdentity.SELF_DESCRIBE:
                default:
                    return ShortFilename;
            }

        }

        public FileInterface(string _targetId, bool _bigEndian, bool _useAutomaticFileBackups = false)
        {
            try
            {
                string[] targetId = _targetId.Split('|');
                Filename = targetId[1];
                var fi = new FileInfo(Filename);
                ShortFilename = fi.Name;
                BigEndian = _bigEndian;
                InterfaceUniquePrefix = Filename.CreateMD5().Substring(0, 4).ToUpper();
                useAutomaticFileBackups = _useAutomaticFileBackups;

                if (!File.Exists(Filename))
                    throw new FileNotFoundException("The file " + Filename + " doesn't exist! Cancelling load");

                FileInfo info = new System.IO.FileInfo(Filename);

                if (info.IsReadOnly)
                {
                    throw new Exception("The file " + Filename + " is read - only! Cancelling load");
                }
                try
                {
                    using (Stream stream = new FileStream(Filename, FileMode.Open))
                    {

                        Console.Write(stream.Length);
                    }
                }
                catch (IOException ex)
                {
                    if (ex is FileLoadException)
                    {
                        throw new Exception($"FileInterface failed to load something because the file is (probably) in use \n" + "Culprit file: " + Filename + "\n" + ex.Message);
                    }
                    if (ex is PathTooLongException)
                    {
                        throw new Exception($"FileInterface failed to load something because the path is too long. Try moving it closer to root \n" + "Culprit file: " + Filename + "\n" + ex.Message);
                    }
                }


                if(useAutomaticFileBackups)
                    SetBackup();

                //getMemoryDump();
                getMemorySize();

            }
            catch (Exception ex)
            {
                if(parent != null && !MultipleFileInterface.LoadAnything)
                    MessageBox.Show($"FileInterface failed to load something \n\n" + "Culprit file: " + Filename + "\n\n" + ex.ToString());

                throw;
            }
            finally
            {
                CloseStream();
            }
        }

        public string getCompositeFilename(string prefix)
        {
            if (CompositeFilenameDico.ContainsKey(Filename))
            {
                return CompositeFilenameDico[Filename];
            }
            //Add it to the dico
            string name = (CompositeFilenameDico.Keys.Count + 1).ToString();
            CompositeFilenameDico[Filename] = name;
            //Flush to disk
            SaveCompositeFilenameDico();
            return name;
        }


        public static bool LoadCompositeFilenameDico(string jsonBaseDir = null)
        {
            if (jsonBaseDir == null)
                jsonBaseDir = CorruptCore.EmuDir;

            JsonSerializer serializer = new JsonSerializer();
            var path = Path.Combine(jsonBaseDir, "TEMP","filemap.json");
            if (!File.Exists(path))
            {
                CompositeFilenameDico = new Dictionary<string, string>();
                return true;
            }
            try
            {

                using (StreamReader sw = new StreamReader(path))
                using (JsonTextReader reader = new JsonTextReader(sw))
                {
                    CompositeFilenameDico = serializer.Deserialize<Dictionary<string, string>>(reader);
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Unable to access the filemap! Figure out what's locking it and then restart the WGH.\n" + e.ToString());
                return false;
            }
            return true;
        }


        public static bool SaveCompositeFilenameDico(string jsonFilePath = null)
        {
            if (jsonFilePath == null)
                jsonFilePath = CorruptCore.EmuDir;

            JsonSerializer serializer = new JsonSerializer();
            var path = Path.Combine(jsonFilePath, "TEMP", "filemap.json");
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, CompositeFilenameDico);
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Unable to access the filemap!\n" + e.ToString());
                return false;
            }
            return true;
        }



        public string getCorruptFilename(bool overrideWriteCopyMode = false)
        {
            if (overrideWriteCopyMode || FileInterface.writeCopyMode)
                return Path.Combine(CorruptCore.EmuDir, "TEMP", getCompositeFilename("CORRUPT"));
            else
                return Filename;
        }
        public string getBackupFilename()
        {
            return Path.Combine(CorruptCore.EmuDir, "TEMP", getCompositeFilename("BACKUP"));
        }

        public override void ResetWorkingFile()
        {

            try
            {
                if (File.Exists(getCorruptFilename()))
                    File.Delete(getCorruptFilename());
            }
            catch
            {
                MessageBox.Show($"Could not get access to {getCorruptFilename()}\n\nClose the file then try whatever you were doing again", "WARNING");
            }


            SetWorkingFile();
        }

        public string SetWorkingFile()
        {

            string corruptFilename = getCorruptFilename();


            if (!File.Exists(corruptFilename))
                File.Copy(getBackupFilename(), corruptFilename, true);

            return corruptFilename;
        }

        public override void ApplyWorkingFile()
        {

            CloseStream();

            if (FileInterface.writeCopyMode)
            {

                try
                {
                    if (File.Exists(Filename))
                        File.Delete(Filename);

                    if (File.Exists(getCorruptFilename()))
                        File.Move(getCorruptFilename(), Filename);
                }
                catch
                {
                    MessageBox.Show($"Could not get access to {Filename} because some other program is probably using it. \n\nClose the file then press OK to try again", "WARNING");
                }

            }
        }

        public override void SetBackup()
        {
            if (!File.Exists(getBackupFilename()))
                File.Copy(Filename, getBackupFilename(), true);
        }

        public override void ResetBackup(bool askConfirmation = true)
        {
            if (askConfirmation && MessageBox.Show("Are you sure you want to reset the backup using the target file?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            if (File.Exists(getBackupFilename()))
                File.Delete(getBackupFilename());

            SetBackup();

        }

        public override void RestoreBackup(bool announce = true)
        {

            if (File.Exists(getBackupFilename()))
            {
                File.Delete(Filename);
                File.Copy(getBackupFilename(), Filename, true);

                if (announce)
                    MessageBox.Show("Backup of " + ShortFilename + " was restored");
            }
            else
            {
                MessageBox.Show("Couldn't find backup of " + ShortFilename);
            }
        }

        public override void wipeMemoryDump()
        {
            lastMemoryDump = null;
            //GC.Collect();
            //GC.WaitForFullGCComplete();
        }

        public override void getMemoryDump()
        {
            if(useAutomaticFileBackups)
                lastMemoryDump = MemoryBanks.ReadFile(getBackupFilename());
            else
                lastMemoryDump = MemoryBanks.ReadFile(Filename);

        }

        public override long getMemorySize()
        {
            if (lastMemorySize != null)
                return (long)lastMemorySize;

            lastRealMemorySize = new FileInfo(Filename).Length;

            long Alignment32bitReminder = lastRealMemorySize.Value % 4;

            if (Alignment32bitReminder != 0)
            {
                lastMemorySize = lastRealMemorySize.Value + (4 - Alignment32bitReminder);
            }
            else
            {
                lastMemorySize = lastRealMemorySize;
            }




            return (long)lastMemorySize;

        }

        public override bool dolphinSavestateVersion()
        {
            /*
             * 0x20-0x32 = "Dolphin Narry's Mod"
             * 0x35-0x39 = "X.Y.Z" - This is the version number
             */

            string a = "Dolphin Narry's Mod";
            string b = Encoding.Default.GetString(PeekBytes(32, 19));

            if (a == b)
            {
                //Change this if there's a new version that breaks things!!!
                string earliestSupportedVersion = "0.1.3";
                string savestateVersion = Encoding.Default.GetString(PeekBytes(53, 5));
                string earliestSupportedVersionTrimmed = earliestSupportedVersion.Replace(".", "");
                string savestateVersionTrimmed = savestateVersion.Replace(".", "");

                if (Convert.ToInt32(savestateVersionTrimmed) >= Convert.ToInt32(earliestSupportedVersionTrimmed))
                    return true;
                else
                {
                    MessageBox.Show("You are trying to load a savestate from an old version of Dolphin Narry's Mod. The earliest supported version is version " + earliestSupportedVersion + ". This will not work properly.");
                    return false;
                }

            }
            else
            {
                MessageBox.Show("The currently loaded file is not a Dolphin Narry's Mod savestate");
                return false;
            }

        }

        public override void PokeBytes(long address, byte[] data)
        {
            if (address + data.Length >= lastRealMemorySize)
                return;

            if (stream == null)
                stream = File.Open(SetWorkingFile(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

            stream.Position = address;
            stream.Write(data, 0, data.Length);

            if (cacheEnabled)
                MemoryBanks.PokeBytes(lastMemoryDump, address, data);

            /*
            if (lastMemoryDump != null)
                for (int i = 0; i < data.Length; i++)
                    lastMemoryDump[address + i] = data[i];
            */

        }

        public override void PokeByte(long address, byte data)
        {
            if (address >= lastRealMemorySize)
                return;

            if (stream == null)
                stream = File.Open(SetWorkingFile(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

            stream.Position = address;
            stream.WriteByte(data);

            if (cacheEnabled)
                MemoryBanks.PokeByte(lastMemoryDump, address, data);
            //lastMemoryDump[address] = data;
        }

        public override byte PeekByte(long address)
        {
            if (address >= lastRealMemorySize)
                return 0;


            if (cacheEnabled)
                return MemoryBanks.PeekByte(lastMemoryDump, address);
            //return lastMemoryDump[address];

            byte[] readBytes = new byte[1];

            if (stream == null)
                stream = File.Open(SetWorkingFile(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);


            stream.Position = address;
            stream.Read(readBytes, 0, 1);

            //fs.Close();

            return readBytes[0];

        }

        public override byte[] PeekBytes(long address, int length)
        {
            if (address + length >= lastRealMemorySize)
                return new byte[length];

            if (cacheEnabled)
                return MemoryBanks.PeekBytes(lastMemoryDump, address, length);
            //return lastMemoryDump.SubArray(address, range);

            byte[] readBytes = new byte[length];


            if (stream == null)
                stream = File.Open(SetWorkingFile(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);


            stream.Position = address;
            stream.Read(readBytes, 0, length);

            //fs.Close();

            return readBytes;

        }

        public override void CloseStream()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }

        }


    }

    [Serializable()]
    public class MultipleFileInterface : FileMemoryInterface, IMemoryDomain
    {
        public static Dictionary<String, String> CompositeFilenameDico { get; set; }


        public override string Name => ShortFilename;
        public override long Size => lastMemorySize.GetValueOrDefault(0);

        public override bool BigEndian { get; }
        public override int WordSize => 4;


        public string Filename;
        public string ShortFilename;

        public List<FileInterface> FileInterfaces = new List<FileInterface>();

        public MultipleFileInterface(string _targetId, bool _bigEndian, bool _useAutomaticFileBackups = false)
        {
            try
            {
                BigEndian = _bigEndian;
                string[] targetId = _targetId.Split('|');
                foreach (string t in targetId)
                {
                    try
                    {
                        var fi = new FileInterface("File|" + t, _bigEndian, _useAutomaticFileBackups);
                        fi.parent = this;
                        FileInterfaces.Add(fi);
                    }
                    catch
                    {
                        if (MultipleFileInterface.LoadAnything)
                            break;
                        else
                            throw;
                    }
                }

                Filename = "MultipleFiles";
                ShortFilename = "MultipleFiles";

                
                if(_useAutomaticFileBackups)
                    SetBackup();
                

                //getMemoryDump();
                getMemorySize();

                setFilePositions();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"MultipleFileInterface failed to load something \n\n" + ex.ToString());
            }
        }

        public override void CloseStream()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }

            foreach (var fi in FileInterfaces)
                if (fi.stream != null)
                {
                    fi.stream.Close();
                    fi.stream = null;
                }

        }

        public override string ToString()
        {
            return "Multiple Files";
        }
        public string getCompositeFilename(string prefix)
        {
            return string.Join("|", FileInterfaces.Select(it => it.getCompositeFilename(prefix)));
        }

        public string getCorruptFilename(bool overrideWriteCopyMode = false)
        {
            return string.Join("|", FileInterfaces.Select(it => it.getCorruptFilename(overrideWriteCopyMode)));

        }

        public string getBackupFilename()
        {
            return string.Join("|", FileInterfaces.Select(it => it.getBackupFilename()));
        }

        public override void ResetWorkingFile()
        {
            foreach (var fi in FileInterfaces)
                fi.ResetWorkingFile();

        }

        public string SetWorkingFile()
        {
            return string.Join("|", FileInterfaces.Select(it => it.SetWorkingFile()));

        }

        public override void ApplyWorkingFile()
        {
            foreach (var fi in FileInterfaces)
                fi.ApplyWorkingFile();

        }

        public override void SetBackup()
        {
            foreach (var fi in FileInterfaces)
                fi.SetBackup();

        }

        public override void ResetBackup(bool askConfirmation = true)
        {
            if (askConfirmation && MessageBox.Show("Are you sure you want to reset the backup using the target files?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            foreach (var fi in FileInterfaces)
                fi.ResetBackup(false);

        }

        public override void RestoreBackup(bool announce = true)
        {

            foreach (var fi in FileInterfaces)
                fi.RestoreBackup(false);

            if (announce)
                MessageBox.Show("Backups of " + string.Join(",", FileInterfaces.Select(it => (it as FileInterface).ShortFilename)) + " were restored");

        }

        public void setFilePositions()
        {

            long addressPad = 0;

            //find which fileInterface contains the file we want
            foreach (var fi in FileInterfaces)
            {
                fi.MultiFilePosition = addressPad;
                addressPad += fi.getMemorySize();
                fi.MultiFilePositionCeiling = addressPad;

            }

        }

        public override void wipeMemoryDump()
        {
            for (int i = 0; i < FileInterfaces.Count; i++)
            {
                FileInterfaces[i].wipeMemoryDump();
                //GC.Collect();
                //GC.WaitForFullGCComplete();
            }
        }

        public override void getMemoryDump()
        {
            long totalDumpSize = 0;

            for (int i = 0; i < FileInterfaces.Count; i++)
            {
                totalDumpSize += FileInterfaces[i].lastMemorySize.Value;
                FileInterfaces[i].getMemoryDump();
            }

            //lastMemoryDump = new byte[totalDumpSize];

            //long targetAddress = 0;

            //for (int i = 0; i < FileInterfaces.Count; i++)
            //{


            //Removed copying of the memory in a local big file because
            //it's smarter to actually use the FileInterfaces themselves
            /*
            long targetLength = FileInterfaces[i].lastMemorySize.Value;
            Array.Copy(FileInterfaces[i].lastMemoryDump, 0, lastMemoryDump, targetAddress, targetLength);
            targetAddress += targetLength;
            FileInterfaces[i].lastMemoryDump = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            */
            //}
            /*
            List<byte> allBytes = new List<byte>();

            foreach (var fi in FileInterfaces)
            {
                allBytes.AddRange(fi.getMemoryDump());
                fi.lastMemoryDump = null;
            }

        lastMemoryDump = allBytes.ToArray();
        */

            //return lastMemoryDump;

        }
        public override byte[][] lastMemoryDump
        {
            get { throw new Exception("FORBIDDEN USE OF LASTMEMORYDUMP ON MULTIPLEFILEINTERFACE"); }
            set { throw new Exception("FORBIDDEN USE OF LASTMEMORYDUMP ON MULTIPLEFILEINTERFACE"); }
        }

        public override bool cacheEnabled
        {
            get { return FileInterfaces.Count > 0 && FileInterfaces[0].lastMemoryDump != null; }
        }

        public override long getMemorySize()
        {
            long size = 0;

            foreach (var fi in FileInterfaces)
                size += fi.getMemorySize();

            lastMemorySize = size;
            return (long)lastMemorySize;

        }

        public override bool dolphinSavestateVersion()
        {
            //Not supported for multiple files
            return false;
        }

        public override long? lastMemorySize { get; set; }
        public static bool LoadAnything { get; set; } = false;

        public override void PokeBytes(long address, byte[] data)
        {

            //find which fileInterface contains the file we want
            for (int i = 0; i < FileInterfaces.Count; i++)
            {
                var fi = FileInterfaces[i];

                if (fi.MultiFilePositionCeiling > address)
                {
                    fi.PokeBytes(address - fi.MultiFilePosition, data);
                    return;
                }
            }

        }

        public override void PokeByte(long address, byte data)
        {

            //find which fileInterface contains the file we want
            for (int i = 0; i < FileInterfaces.Count; i++)
            {
                var fi = FileInterfaces[i];

                if (fi.MultiFilePositionCeiling > address)
                {
                    fi.PokeByte(address - fi.MultiFilePosition, data);
                    return;
                }
            }

        }

        public override byte PeekByte(long address)
        {

            //find which fileInterface contains the file we want
            for (int i = 0; i < FileInterfaces.Count; i++)
            {
                var fi = FileInterfaces[i];

                if (fi.MultiFilePositionCeiling > address)
                    return fi.PeekByte(address - fi.MultiFilePosition);
            }

            //if wasn't found
            return 0;


        }

        public override byte[] PeekBytes(long address, int range)
        {

            //find which fileInterface contains the file we want
            for (int i = 0; i < FileInterfaces.Count; i++)
            {
                var fi = FileInterfaces[i];

                if (fi.MultiFilePositionCeiling > address)
                    return fi.PeekBytes(address - fi.MultiFilePosition, range);
            }

            //if wasn't found
            return null;

        }

    }

    public enum FileInterfaceIdentity
    {
        SELF_DESCRIBE,
        HASHED_PREFIX,
        FULL_PATH,
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
