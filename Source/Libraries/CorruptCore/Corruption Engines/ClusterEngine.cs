namespace RTCV.CorruptCore
{
    using System.Collections.Generic;
    using RTCV.NetCore;

    public static class ClusterEngine
    {
        const string rand = "Random";
        const string reverse = "Reverse";
        const string rotFW = "Rotate Forwards";
        const string rotBW = "Rotate Backwards";
        const string overWrite = "Overwrite";

        public static string[] ShuffleTypes { get; private set; } = new string[] { rand, reverse, rotFW, rotBW, overWrite };


        const string forwards = "Forwards";
        const string backwards = "Backwards";
        public static string[] Directions { get; private set; } = new string[] { forwards, backwards };

        public static string LimiterListHash
        {
            get => (string)AllSpec.CorruptCoreSpec[RTCSPEC.CLUSTER_LIMITERLISTHASH];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CLUSTER_LIMITERLISTHASH, value);
        }

        public static int ChunkSize
        {
            get => (int)AllSpec.CorruptCoreSpec[RTCSPEC.CLUSTER_SHUFFLEAMT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CLUSTER_SHUFFLEAMT, value);
        }

        public static string ShuffleType
        {
            get => (string)AllSpec.CorruptCoreSpec[RTCSPEC.CLUSTER_SHUFFLETYPE];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CLUSTER_SHUFFLETYPE, value);
        }

        public static int Modifier
        {
            get => (int)AllSpec.CorruptCoreSpec[RTCSPEC.CLUSTER_MODIFIER];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CLUSTER_MODIFIER, value);
        }


        public static bool OutputMultipleUnits
        {
            get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CLUSTER_MULTIOUT];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CLUSTER_MULTIOUT, value);
        }

        public static bool FilterAll
        {
            get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.CLUSTER_FILTERALL];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CLUSTER_FILTERALL, value);
        }

        public static string Direction
        {
            get => (string)AllSpec.CorruptCoreSpec[RTCSPEC.CLUSTER_DIR];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.CLUSTER_DIR, value);
        }

        public static PartialSpec getDefaultPartial()
        {
            var partial = new PartialSpec("RTCSpec");
            partial[RTCSPEC.CLUSTER_LIMITERLISTHASH] = string.Empty;
            partial[RTCSPEC.CLUSTER_SHUFFLETYPE] = rand;
            partial[RTCSPEC.CLUSTER_SHUFFLEAMT] = 3;
            partial[RTCSPEC.CLUSTER_MODIFIER] = 1;
            partial[RTCSPEC.CLUSTER_MULTIOUT] = true;
            partial[RTCSPEC.CLUSTER_FILTERALL] = false;
            partial[RTCSPEC.CLUSTER_DIR] = forwards;
            return partial;
        }


        public static BlastUnit[] GenerateUnit(string domain, long address, int alignment, bool useAlignment)
        {
            if (domain == null)
            {
                return null;
            }

            int precision = 4;
            if (Filtering.Hash2LimiterDico.TryGetValue(LimiterListHash, out IListFilter list))
            {
                precision = list.GetPrecision();
            }
            else
            {
                return null;
            }

            MemoryInterface mi = MemoryDomains.GetInterface(domain);
            if (mi == null)
            {
                return null;
            }
            //Query once
            int chunkSize = ChunkSize;

            int srcUnit = 0;

            long safeAddress = address;
            if (useAlignment)
                safeAddress = safeAddress - (address % precision) + alignment;



            if (safeAddress > mi.Size - precision)
            {
                safeAddress = mi.Size - (precision * 2) + alignment; //If we're out of range, hit the last aligned address
            }

            //if chunk size is still too big then abort, could be optimized for forwards
            if (safeAddress + (chunkSize * precision) >= mi.Size)
            {
                return null;
            }
            long filterAddress = safeAddress;

            if (Direction == backwards)
            {
                srcUnit = chunkSize - 1;
                filterAddress = safeAddress + (precision * (chunkSize - 1));
            }

            //do not swap endianess
            byte[] GetSegment(long address)
            {
                byte[] values = new byte[precision];

                for (long i = 0; i < precision; i++)
                {
                    values[i] = mi.PeekByte(address + i);
                }

                return values;
            }

            void ShuffleRandom(List<byte[]> list)
            {
                int n = list.Count;
                while (n > 1)
                {
                    n--;
                    int k = RtcCore.RND.Next(n + 1);
                    byte[] value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }

            void RotateForward(List<byte[]> list)
            {
                var x = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                list.Insert(0, x);
            }

            void RotateBackward(List<byte[]> list)
            {
                var x = list[0];
                list.RemoveAt(0);
                list.Add(x);
            }

            void OverWrite(List<byte[]> list)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    list[j] = list[srcUnit];
                }
            }

            //filter
            if (FilterAll)
            {
                for (int j = 0; j < chunkSize; j++)
                {
                    if (!Filtering.LimiterPeekBytes(safeAddress + (j * precision), safeAddress + (j * precision) + precision, LimiterListHash, mi))
                    {
                        return null;
                    }
                }
            }
            else if (!Filtering.LimiterPeekBytes(filterAddress, filterAddress + precision, LimiterListHash, mi))
            {
                return null;
            }


            List<byte[]> byteArr = new List<byte[]>();

            for (int j = 0; j < chunkSize; j++)
            {
                byteArr.Add(GetSegment(safeAddress + (long)(j * precision)));
            }

            int modifier = Modifier;
            switch (ShuffleType)
            {
                case rotFW:
                    for (int j = 0; j < modifier; j++)
                    {
                        RotateForward(byteArr);
                    }
                    break;
                case rotBW:
                    for (int j = 0; j < modifier; j++)
                    {
                        RotateBackward(byteArr);
                    }
                    break;
                case reverse:
                    byteArr.Reverse();
                    break;
                case overWrite:
                    OverWrite(byteArr);
                    break;
                case rand:
                default:
                    ShuffleRandom(byteArr);
                    break;
            }

            if (OutputMultipleUnits)
            {
                BlastUnit[] ret = new BlastUnit[chunkSize];

                for (int j = 0; j < chunkSize; j++)
                {
                    //do not swap endianess
                    ret[j] = new BlastUnit(byteArr[j], domain, safeAddress + (j * precision), precision, false, 0, 1, null, true, false, true);
                }

                return ret;
            }
            else
            {
                List<byte> btsOut = new List<byte>();
                for (int j = 0; j < chunkSize; j++)
                {
                    btsOut.AddRange(byteArr[j]);
                }
                //do not swap endianess
                return new BlastUnit[] { new BlastUnit(btsOut.ToArray(), domain, safeAddress, (precision * chunkSize), false, 0, 1, null, true, false, true) };
            }
        }
    }
}
