namespace RTCV.CorruptCore
{
    using RTCV.NetCore;

    public static class RTC_DistortionEngine
    {
        public static int Delay
        {
            get => (int)RTCV.NetCore.AllSpec.CorruptCoreSpec[RTCSPEC.DISTORTION_DELAY];
            set => RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.DISTORTION_DELAY, value);
        }

        public static PartialSpec getDefaultPartial()
        {
            var partial = new PartialSpec("RTCSpec");
            partial[RTCSPEC.DISTORTION_DELAY] = 50;

            return partial;
        }

        public static BlastUnit GenerateUnit(string domain, long address, int precision, int alignment)
        {
            // Randomly selects a memory operation according to the selected algorithm

            if (domain == null)
            {
                return null;
            }

            MemoryInterface mi = MemoryDomains.GetInterface(domain);
            long safeAddress = address - (address % precision) + alignment;
            if (safeAddress > mi.Size - precision && mi.Size > precision)
            {
                safeAddress = mi.Size - (2 * precision) + alignment; //If we're out of range, hit the last aligned address
            }

            return new BlastUnit(StoreType.ONCE, StoreTime.IMMEDIATE, domain, safeAddress, domain, safeAddress, precision, mi.BigEndian, Delay, 1);
        }
    }
}
