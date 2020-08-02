namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml.Serialization;
    using Ceras;

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
        public long PointerSpacer { get; set; } = 1;

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
                {
                    continue;
                }

                for (long i = start; i < end; i++)
                {
                    if (!IsAddressInRanges(i, RemoveSingles, RemoveRanges))
                    {
                        if (PointerSpacer == 1 || addressCount % PointerSpacer == 0)
                        {
                            //VMD.PointerDomains.Add(GenDomain);
                            VMD.PointerAddresses.Add(i);
                        }
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
            {
                return true;
            }

            foreach (long[] range in ranges)
            {
                long start = range[0];
                long end = range[1];

                if (address >= start && address < end)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddFromTrimmedLine(string trimmedLine, long currentDomainSize, bool remove)
        {
            var lineParts = trimmedLine.Split('-');

            if (lineParts.Length > 1)
            {
                var start = SafeStringToLong(lineParts[0]);
                var end = SafeStringToLong(lineParts[1]);

                if (end < start)
                {
                    return;
                }

                if (end >= currentDomainSize)
                {
                    end = Convert.ToInt64(currentDomainSize - 1);
                }

                if (remove)
                {
                    RemoveRanges.Add(new long[] { start, end });
                }
                else
                {
                    AddRanges.Add(new long[] { start, end });
                }
            }
            else
            {
                var address = SafeStringToLong(lineParts[0]);

                if (address > 0 && address < currentDomainSize)
                {
                    if (remove)
                    {
                        RemoveSingles.Add(address);
                    }
                    else
                    {
                        AddSingles.Add(address);
                    }
                }
            }
        }

        private static long SafeStringToLong(string input)
        {
            try
            {
                if (input.IndexOf("0X", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return long.Parse(input.Substring(2), NumberStyles.HexNumber);
                }
                else
                {
                    return long.Parse(input, NumberStyles.HexNumber);
                }
            }
            catch (FormatException e)
            {
                Console.Write(e);
                return -1;
            }
        }
    }
}
