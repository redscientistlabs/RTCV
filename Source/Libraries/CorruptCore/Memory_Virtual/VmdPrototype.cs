using System.Threading;
using System.Threading.Tasks;

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
    [MemberConfig(TargetMember.All)]
    public class VmdPrototype
    {
        public string VmdName { get; set; }
        public string SystemCore { get; set; }
        public string GenDomain { get; set; }
        public bool BigEndian { get; set; }
        public int WordSize { get; set; }
        public long PointerSpacer { get; set; } = 1;

        public long Padding { get; set; }

        public bool UsingRPC { get; set; }

        public List<long> AddSingles { get; set; } = new List<long>();
        public List<long> RemoveSingles { get; set; } = new List<long>();

        public List<long[]> AddRanges { get; set; } = new List<long[]>();
        public List<long[]> RemoveRanges { get; set; } = new List<long[]>();

        public BlastLayer SuppliedBlastLayer { get; set; } = null;

        public VmdPrototype()
        {
        }

        public VirtualMemoryDomain Generate(IProgress<int> progress = null)
        {
            VirtualMemoryDomain VMD = new VirtualMemoryDomain
            {
                Proto = this,
                Name = VmdName,
                SystemCore = SystemCore,
                BigEndian = BigEndian,
                WordSize = WordSize,
                UsingRPC = UsingRPC,
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
                int addresses = addressCount;
                
                int threads = (int)Math.Min(Environment.ProcessorCount - 2, end - start);
                if (threads <= 0)
                {
                    threads = 1;
                }
                
                var orderedPointers = new List<long>[threads];

                object lockObject = new object();
                
                Parallel.For(0, threads, thread =>
                {
                    List<long> pointerAddresses = new List<long>();
                    long size = (end - start) / threads;
                    long beginning = (size * thread) + start;
                    long ending;
                    if (thread == threads - 1)
                    {
                        ending = end;
                    }
                    else
                    {
                        ending = (size * (thread + 1)) - 1 + start;
                    }

                    int ourAddresses = addresses + (int)beginning;

                    for (long i = beginning; i <= ending; i++)
                    {
                        if (!IsAddressInRanges(i, this.RemoveSingles, this.RemoveRanges))
                        {
                            if (this.PointerSpacer == 1 || ourAddresses % this.PointerSpacer == 0)
                            {
                                //VMD.PointerDomains.Add(GenDomain);
                                pointerAddresses.Add(i);
                            }
                        }

                        if (progress != null && ourAddresses % (1423 * threads) == 0)
                        {
                            progress.Report(threads);
                        }
                        ourAddresses++;
                    }
                    Interlocked.Add(ref addressCount, ourAddresses);
                    lock (lockObject)
                    {
                        orderedPointers[thread] = pointerAddresses;
                    }
                });
                foreach (List<long> pointers in orderedPointers)
                {
                    VMD.PointerAddresses.AddRange(pointers);
                }
                addressCount += addresses;
            }

            foreach (long single in this.AddSingles)
            {
                //VMD.PointerDomains.Add(GenDomain);
                VMD.PointerAddresses.Add(single);
                addressCount++;
            }

            VMD.CompactPointerDomains = new[] { this.GenDomain };
            VMD.CompactPointerAddresses = new[] { VMD.PointerAddresses.ToArray() };

            VMD.Compact(true);

            return VMD;
        }

        private static bool IsAddressInRanges(long address, List<long> singles, List<long[]> ranges)
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
            if (trimmedLine == null)
            {
                throw new ArgumentNullException(nameof(trimmedLine));
            }

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
