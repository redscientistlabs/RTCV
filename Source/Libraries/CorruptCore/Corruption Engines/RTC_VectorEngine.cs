namespace RTCV.CorruptCore
{
    using RTCV.NetCore;

    public static class RTC_VectorEngine
    {
        public static string LimiterListHash
        {
            get => (string)AllSpec.CorruptCoreSpec[RTCSPEC.VECTOR_LIMITERLISTHASH];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.VECTOR_LIMITERLISTHASH, value);
        }

        public static string ValueListHash
        {
            get => (string)AllSpec.CorruptCoreSpec[RTCSPEC.VECTOR_VALUELISTHASH];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.VECTOR_VALUELISTHASH, value);
        }

        public static PartialSpec getDefaultPartial()
        {
            var partial = new PartialSpec("RTCSpec");

            partial[RTCSPEC.VECTOR_LIMITERLISTHASH] = string.Empty;
            partial[RTCSPEC.VECTOR_VALUELISTHASH] = string.Empty;

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
            //Enforce the safeaddress at generation
            var matchBytes = Filtering.LimiterPeekAndGetBytes(safeAddress, safeAddress + 4, domain, LimiterListHash, mi);
            if (matchBytes != null)
            {
                return new BlastUnit(Filtering.GetRandomConstant(ValueListHash, 4, matchBytes), domain, safeAddress, 4,
                    mi.BigEndian, 0, 1, null, true, false, true);
            }

            return null;
        }
    }
}
