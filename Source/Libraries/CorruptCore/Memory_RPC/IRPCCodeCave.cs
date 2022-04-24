namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public interface IRPCCodeCave : ICodeCave
    {
        void DumpMemory();
        void UpdateMemory();
    }
    public interface IRPCCodeCavesDomain : ICodeCavesDomain, IRPCMemoryDomain
    {
    }
}
