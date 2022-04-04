namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRPCMemoryDomain : IMemoryDomain
    {
        void DumpMemory();
        void UpdateMemory();
        (MemoryInterface, ulong, long) AllocateMemory(int size);
        void FreeMemory(ulong addr, int size);
        byte[] NopInstruction(long instructionAddress);
        byte[] GetMemory();
    }
}
