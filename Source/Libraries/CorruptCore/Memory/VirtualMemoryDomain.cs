namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using Ceras;

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class VirtualMemoryDomain : MemoryInterface
    {
        public List<string> PointerDomains { get; set; } = new List<string>();
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
            {
                return;
            }

            List<string> domains = new List<string>();
            List<List<long>> domainAdresses = new List<List<long>>();

            for (int i = 0; i < PointerAddresses.Count; i++)
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
                {
                    return CompactPointerAddresses.Sum(it => it.Length);
                }
                else
                {
                    return PointerAddresses.Count;
                }
            }
            set { }
        }

        private string name;
        public override string Name
        {
            get => "[V]" + name;
            set => name = value;
        }

        public void AddFromBlastLayer(BlastLayer bl)
        {
            if (bl == null)
            {
                return;
            }

            bl.SanitizeDuplicates();

            foreach (BlastUnit bu in bl.Layer)
            {
                for (int i = 0; i < bu.Precision; i++)
                {
                    PointerDomains.Add(bu.Domain);
                    PointerAddresses.Add(bu.Address + i);
                }
            }
        }

        private int GetCompactedDomainIndexFromAddress(long address)
        {
            long currentBankStartAddress = 0;
            for (var i = 0; i < CompactPointerAddresses.Length; i++)
            {
                long[] addressBank = CompactPointerAddresses[i];
                if (address < (currentBankStartAddress + addressBank.Length)) // are we in the right bank?
                {
                    return i;
                }

                currentBankStartAddress += addressBank.Length;
            }
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
                {
                    return "ERROR";
                }

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
                    {
                        return addressBank[address - currentBankStartAddress];
                    }

                    currentBankStartAddress += addressBank.Length;
                }
                return 0; //failure
            }
            else
            {
                if (address < 0 || address > PointerAddresses.Count || address < Proto.Padding)
                {
                    return 0;
                }

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
            {
                data.Add(PeekByte(i));
            }

            if (raw || BigEndian)
            {
                return data.ToArray();
            }
            else
            {
                return data.ToArray().FlipBytes();
            }
        }

        public override void PokeBytes(long startAddress, byte[] value, bool raw = true)
        {
            if (!raw || !BigEndian)
            {
                value.FlipBytes();
            }

            for (long i = 0; i < value.Length; i++)
            {
                PokeByte(startAddress + i, value[i]);
            }
        }

        public override byte PeekByte(long address)
        {
            if (address < this.Proto.Padding)
            {
                return 0;
            }

            if (address > this.Size - 1)
            {
                return 0;
            }

            string targetDomain = GetRealDomain(address);
            long targetAddress = GetRealAddress(address);

            MemoryDomainProxy mdp = MemoryDomains.GetProxy(targetDomain, targetAddress);

            return mdp?.PeekByte(targetAddress) ?? 0;
        }

        public override void PokeByte(long address, byte value)
        {
            if (address < this.Proto.Padding)
            {
                return;
            }

            if (address > this.Size - 1)
            {
                return;
            }

            string targetDomain = GetRealDomain(address);
            long targetAddress = GetRealAddress(address);

            MemoryDomainProxy mdp = MemoryDomains.GetProxy(targetDomain, targetAddress);

            mdp?.PokeByte(targetAddress, value);
        }
    }
}
