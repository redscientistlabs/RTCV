using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.Coroutines
{
    /// <summary>
    /// Holds all the coroutine runners
    /// </summary>
    public static class CoroutineEngine
    {
        public static CoroutineRunner PreExecute = new CoroutineRunner();
        public static CoroutineRunner Execute = new CoroutineRunner();
        public static CoroutineRunner PostExecute = new CoroutineRunner();

        public static void Reset()
        {
            PreExecute.StopAndClearAll();
            Execute.StopAndClearAll();
            PostExecute.StopAndClearAll();
        }
    }
}
