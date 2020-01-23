using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.Coroutines
{
    static class CoroutineEngine
    {
        public static CoroutineRunner Load = new CoroutineRunner();
        public static CoroutineRunner PreExecute = new CoroutineRunner();
        public static CoroutineRunner PostExecute = new CoroutineRunner();

        public static void StopAll()
        {
            Load.StopAll();
            PreExecute.StopAll();
            PostExecute.StopAll();
        }

        
    }
}
