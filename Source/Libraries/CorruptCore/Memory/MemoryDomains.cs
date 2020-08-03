namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using RTCV.NetCore;

    public static class MemoryDomains
    {
        private static object miLock = new object();

        public static Dictionary<string, MemoryDomainProxy> MemoryInterfaces
        {
            get
            {
                lock (miLock)
                {
                    return RTCV.NetCore.AllSpec.CorruptCoreSpec?["MEMORYINTERFACES"] as Dictionary<string, MemoryDomainProxy>;
                }
            }
            set
            {
                lock (miLock)
                {
                    RTCV.NetCore.AllSpec.CorruptCoreSpec.Update("MEMORYINTERFACES", value);
                }
            }
        }

        private static Dictionary<string, VirtualMemoryDomain> vmdPool = new Dictionary<string, VirtualMemoryDomain>();

        public static Dictionary<string, VirtualMemoryDomain> VmdPool
        {
            get
            {
                lock (miLock)
                {
                    return vmdPool;
                }
            }
            set
            {
                lock (miLock)
                {
                    vmdPool = value;
                }
            }
        }

        public static Dictionary<string, MemoryInterface> AllMemoryInterfaces
        {
            get
            {
                lock (miLock)
                {
                    var d = new Dictionary<string, MemoryInterface>();
                    if (MemoryInterfaces != null)
                    {
                        foreach (var item in MemoryInterfaces)
                        {
                            d[item.Key] = item.Value;
                        }
                    }

                    if (VmdPool != null)
                    {
                        foreach (var item in VmdPool)
                        {
                            d[item.Key] = item.Value;
                        }
                    }

                    return d;
                }
            }
        }

        public static PartialSpec getDefaultPartial()
        {
            var partial = new PartialSpec("RTCSpec");

            partial["MEMORYINTERFACES"] = new Dictionary<string, MemoryInterface>();

            return partial;
        }

        public static void RefreshDomains(bool domainsChanged = false)
        {
            var mdps = RTCV.NetCore.AllSpec.VanguardSpec?[VSPEC.MEMORYDOMAINS_INTERFACES] as MemoryDomainProxy[];
            if (mdps == null)
            {
                return;
            }

            var temp = new Dictionary<string, MemoryDomainProxy>();

            foreach (MemoryDomainProxy mdp in mdps)
            {
                temp.Add(mdp.ToString(), mdp);
            }

            MemoryInterfaces = temp;

            if (domainsChanged)
            {
                LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_EVENT_DOMAINSUPDATED, true);
            }
        }

        public static void Clear()
        {
            MemoryInterfaces?.Clear();
        }

        public static MemoryDomainProxy GetProxy(string domain, long address)
        {
            if (domain == null)
            {
                return null;
            }

            if (MemoryInterfaces.Count == 0)
            {
                RefreshDomains();
            }

            if (MemoryInterfaces.TryGetValue(domain, out MemoryDomainProxy mdp))
            {
                return mdp;
            }

            if (VmdPool.TryGetValue(domain, out VirtualMemoryDomain vmd))
            {
                return GetProxy(vmd.GetRealDomain(address), vmd.GetRealAddress(address));
            }

            return null;
        }

        public static MemoryInterface GetInterface(string _domain)
        {
            if (_domain == null)
            {
                return null;
            }

            if (MemoryInterfaces.Count == 0)
            {
                RefreshDomains();
            }

            if (MemoryInterfaces.TryGetValue(_domain, out MemoryDomainProxy mi))
            {
                return mi;
            }

            if (VmdPool.TryGetValue(_domain, out VirtualMemoryDomain vmd))
            {
                return vmd;
            }

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
            {
                return address;
            }
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
            {
                return domain;
            }
        }

        public static VmdPrototype GetVmdPrototypeFromBlastlayer(BlastLayer bl)
        {
            //If the BL references a VMD that doesn't exist, return null
            if (bl.Layer.Any(x => MemoryDomains.GetInterface(x.Domain) == null))
            {
                return null;
            }

            VmdPrototype proto = new VmdPrototype
            {
                VmdName = RtcCore.GetRandomKey(),
                GenDomain = "Hybrid"
            };

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
            {
                MessageBox.Show("The resulting layer was empty or contained invalid data (unloaded VMD?)");
            }

            AddVMD(proto);
        }

        /// <summary>
        /// This is one of the rare cases where a method is netcore redundant
        /// We don't use a spec for this because VMDs can get huge, and as such, we keep the dictionary synced on both sides manually.
        /// </summary>
        /// <param name="proto"></param>
        public static void AddVMD(VmdPrototype proto)
        {
            AddVMD(proto.Generate());
        }

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

        public static void AddVMD_NET(VmdPrototype proto)
        {
            AddVMD_NET(proto.Generate());
        }

        public static void AddVMD_NET(VirtualMemoryDomain VMD)
        {
            MemoryDomains.VmdPool[VMD.ToString()] = VMD;
        }

        /// <summary>
        /// This is one of the rare cases where a method is netcore redundant
        /// We don't use a spec for this because VMDs can get huge, and as such, we keep the dictionary synced on both sides manually.
        /// </summary>
        /// <param name="VMD"></param>
        public static void RemoveVMD(VirtualMemoryDomain VMD)
        {
            RemoveVMD(VMD.ToString());
        }

        /// <summary>
        /// This is one of the rare cases where a method is netcore redundant
        /// We don't use a spec for this because VMDs can get huge, and as such, we keep the dictionary synced on both sides manually.
        /// </summary>
        /// <param name="vmdName"></param>
        public static void RemoveVMD(string vmdName)
        {
            if (MemoryDomains.VmdPool.ContainsKey(vmdName))
            {
                MemoryDomains.VmdPool.Remove(vmdName);
            }

            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_DOMAIN_VMD_REMOVE, vmdName, true);
            LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_EVENT_DOMAINSUPDATED);
        }

        public static void RemoveVMD_NET(VirtualMemoryDomain VMD)
        {
            RemoveVMD_NET(VMD.ToString());
        }

        public static void RemoveVMD_NET(string vmdName)
        {
            if (MemoryDomains.VmdPool.ContainsKey(vmdName))
            {
                MemoryDomains.VmdPool.Remove(vmdName);
            }
        }

        public static void GenerateActiveTableDump(string domain, string key)
        {
            if (!MemoryInterfaces.ContainsKey(domain))
            {
                return;
            }

            MemoryInterface mi = MemoryInterfaces[domain];

            byte[] dump = mi.GetDump();

            File.WriteAllBytes(Path.Combine(RtcCore.workingDir, "MEMORYDUMPS", key + ".dmp"), dump.ToArray());
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
            {
                return false;
            }

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
                    {
                        rp.PrimaryDomain = "PRG ROM";
                    }
                    else
                    {
                        rp.Error = "Unfortunately, Bizhawk doesn't support editing the ROM (FDS Side) domain of FDS games. Maybe in a future version...";
                        break;
                    }

                    if (MemoryDomains.MemoryInterfaces.ContainsKey("CHR VROM"))
                    {
                        rp.SecondDomain = "CHR VROM";
                    }
                    //Skip the first 16 bytes if there's an iNES header
                    if (CheckNesHeader(romFilename))
                    {
                        rp.SkipBytes = 16;
                    }

                    break;

                case "SNES":    //Super Nintendo
                    if (MemoryDomains.MemoryInterfaces.ContainsKey("SGB CARTROM")) //BSNES SGB Mode
                    {
                        rp.PrimaryDomain = "SGB CARTROM";
                    }
                    else
                    {
                        rp.PrimaryDomain = "CARTROM";

                        long filesize = new FileInfo(romFilename).Length;

                        if (filesize % 1024 != 0)
                        {
                            rp.SkipBytes = 512;
                        }
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

                        if (romFilename.IndexOf(".SMD", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            rp.SkipBytes = 512;
                        }
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
                    {
                        /*
                        //just take the first domain
                        MemoryDomainProxy[] mdps = (AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_INTERFACES] as MemoryDomainProxy[]);

                        if(mdps.Length == 0)
                        {
                            rp.Error = "No domains could be hooked onto";
                            break;
                        }

                        string selectedDomain = mdps.First().Name;

                        rp.PrimaryDomain = selectedDomain;

                        if (mdps.Length > 0)
                            MessageBox.Show($"More than one domain is loaded, the first one ({selectedDomain}) was selected.");
                        */

                        rp.Error = "Domain has no preset";
                        break;
                    }
            }

            return rp;
        }
    }
}
