namespace RTCV.CorruptCore
{
    using RTCV.NetCore;

    public static class VectorEngine
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

        public static bool UnlockPrecision
        {
            get => (bool)AllSpec.CorruptCoreSpec[RTCSPEC.VECTOR_UNLOCKPRECISION];
            set => AllSpec.CorruptCoreSpec.Update(RTCSPEC.VECTOR_UNLOCKPRECISION, value);
        }

        public static PartialSpec getDefaultPartial()
        {
            var partial = new PartialSpec("RTCSpec");

            partial[RTCSPEC.VECTOR_LIMITERLISTHASH] = string.Empty;
            partial[RTCSPEC.VECTOR_VALUELISTHASH] = string.Empty;
            partial[RTCSPEC.VECTOR_UNLOCKPRECISION] = false;

            return partial;
        }

        public static BlastUnit GenerateUnit(string domain, long address, int alignment)
        {
            if (domain == null)
            {
                return null;
            }

            int precision;

            //Behavior: Will use the selected limiter list's precision by default
            //And if the precision was unlocked, use what was set in the precision box

            if (Filtering.Hash2LimiterDico.TryGetValue(LimiterListHash, out IListFilter list))
            {
                if (UnlockPrecision)
                {
                    precision = RtcCore.CachedPrecision;
                }
                else
                {
                    precision = list.GetPrecision();
                }
            }
            else
            {
                return null;
            }

            long safeAddress = address - (address % precision) + alignment; //32-bit trunk

            MemoryInterface mi = MemoryDomains.GetInterface(domain);
            if (mi == null)
            {
                return null;
            }

            if (safeAddress >= mi.Size - precision)
            {
                safeAddress = mi.Size - (2 * precision) + alignment; //If we're out of range, hit the last aligned address
            }

            //Enforce the safeaddress at generation
            var matchBytes = Filtering.LimiterPeekAndGetBytes(safeAddress, safeAddress + precision, LimiterListHash, mi);
            if (matchBytes != null)
            {
                return new BlastUnit(Filtering.GetRandomConstant(ValueListHash, precision, matchBytes), domain, safeAddress, precision,
                    mi.BigEndian, 0, 1, null, true, false, true);
            }

            return null;
        }
    }
}
