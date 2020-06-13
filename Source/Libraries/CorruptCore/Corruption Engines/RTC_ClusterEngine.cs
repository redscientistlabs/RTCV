namespace RTCV.CorruptCore
{
    using System.Collections.Generic;
    using RTCV.NetCore;

    public static class RTC_ClusterEngine
    {

        public static string LimiterListHash
        {
            get => (string)AllSpec.CorruptCoreSpec["CLUSTER_LIMITERLISTHASH"];
            set => AllSpec.CorruptCoreSpec.Update("CLUSTER_LIMITERLISTHASH", value);
        }

        public static int ChunkSize
        {
            get => (int)AllSpec.CorruptCoreSpec["CLUSTER_SHUFFLEAMT"];
            set => AllSpec.CorruptCoreSpec.Update("CLUSTER_SHUFFLEAMT", value);
        }

        public static PartialSpec getDefaultPartial()
        {
            var partial = new PartialSpec("RTCSpec");
            partial["CLUSTER_LIMITERLISTHASH"] = string.Empty;
            partial["CLUSTER_SHUFFLEAMT"] = 3;
            return partial;
        }


        public static BlastUnit GenerateUnit(string domain, long address, int alignment)
        {
            if (domain == null)
            {
                return null;
            }

            long safeAddress = address - (address % 4) + alignment; //32-bit trunk

            MemoryInterface mi = MemoryDomains.GetInterface(domain);
            if (mi == null)
            {
                return null;
            }

            if (safeAddress > mi.Size - 4)
            {
                safeAddress = mi.Size - 8 + alignment; //If we're out of range, hit the last aligned address
            }

            //do not swap endianess
            byte[] GetWord(long address)
            {
                byte[] values = new byte[4];

                for (long i = 0; i < 4; i++)
                {
                    values[i] = mi.PeekByte(address + i);
                }

                return values;
            }

            void Shuffle(byte[][] list)
            {
                int n = list.Length;
                while (n > 1)
                {
                    n--;
                    int k = RtcCore.RND.Next(n + 1);
                    byte[] value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }

            if (Filtering.LimiterPeekBytes(safeAddress, safeAddress + 4, domain, LimiterListHash, mi))
            {
                int cs = ChunkSize;
                byte[][] byteArr = new byte[cs][];

                if (safeAddress + (long)(cs * 4) >= mi.Size)
                {
                    return null;
                }

                for (int j = 0; j < cs; j++)
                {
                    byteArr[j] = GetWord(safeAddress + (long)(j * 4));
                }

                Shuffle(byteArr);

                List<byte> btsOut = new List<byte>();
                for (int j = 0; j < cs; j++)
                {
                    btsOut.AddRange(byteArr[j]);
                }
                //do not swap endianess
                return new BlastUnit(btsOut.ToArray(), domain, safeAddress, (4 * cs), false, 0, 1, null, true, false, true);
            }
            return null;
        }
    }
}
