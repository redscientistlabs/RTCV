﻿namespace RTCV.CorruptCore
{
    public static class PipeEngine
    {
        public static BlastUnit GenerateUnit(string domain, long address, int precision, int alignment, bool useAlignment)
        {
            // Randomly selects a memory operation according to the selected algorithm

            if (domain == null)
            {
                return null;
            }

            BlastTarget pipeStart = RtcCore.GetBlastTarget();
            MemoryInterface mi = MemoryDomains.GetInterface(domain);
            MemoryInterface startmi = MemoryDomains.GetInterface(pipeStart.Domain);

            long safeAddress = address;
            long safePipeStartAddress = pipeStart.Address;
            if (useAlignment)
            {
                safeAddress = safeAddress - (address % precision) + alignment;
                safePipeStartAddress = safePipeStartAddress - (pipeStart.Address % precision) + alignment;
            }
            
            if (safeAddress > mi.Size - precision && mi.Size > precision)
            {
                safeAddress = mi.Size - (2 * precision) + alignment; //If we're out of range, hit the last aligned address
            }
            if (safePipeStartAddress > startmi.Size - precision && startmi.Size > precision)
            {
                safePipeStartAddress = startmi.Size - (2 * precision) + alignment; //If we're out of range, hit the last aligned address
            }

            return new BlastUnit(StoreType.CONTINUOUS, StoreTime.PREEXECUTE, domain, safeAddress, pipeStart.Domain, safePipeStartAddress, precision, mi.BigEndian, 0, 0);
        }
    }
}
