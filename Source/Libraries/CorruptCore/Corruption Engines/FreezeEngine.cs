﻿namespace RTCV.CorruptCore
{
    public static class FreezeEngine
    {
        public static BlastUnit GenerateUnit(string domain, long address, int precision, int alignment, bool useAlignment)
        {
            if (domain == null)
            {
                return null;
            }

            MemoryInterface mi = MemoryDomains.GetInterface(domain);
            long safeAddress = address;
            if (useAlignment)
                safeAddress = safeAddress - (address % precision) + alignment;
            if (safeAddress > mi.Size - precision && mi.Size > precision)
            {
                safeAddress = mi.Size - (2 * precision) + alignment; //If we're out of range, hit the last aligned address
            }

            return new BlastUnit(StoreType.ONCE, StoreTime.PREEXECUTE, domain, safeAddress, domain, safeAddress, precision, mi.BigEndian, 0, 0);
        }
    }
}
