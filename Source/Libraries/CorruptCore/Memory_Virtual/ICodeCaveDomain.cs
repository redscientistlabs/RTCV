namespace RTCV.CorruptCore
{
    using System.Collections.Generic;
    public interface ICodeCave
    {
        ulong RealAddress { get; set;  }
        int AllocatedSize { get; set; }
        byte[] Data { get; set; }
    }

    public interface ICodeCavesDomain : IMemoryDomain
    {
        Dictionary<long, ICodeCave> Caves { get; set; }
        new long Size { get; set; }
        (long, ulong) AllocateMemory(int size);
    }

    public interface ICodeCavable
    {
        ICodeCavesDomain CodeCaves { get; set; }
        byte[] GetMemory();
    }
}
