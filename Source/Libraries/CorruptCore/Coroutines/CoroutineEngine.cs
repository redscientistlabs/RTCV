namespace RTCV.CorruptCore.Coroutines
{
    /// <summary>
    /// Holds all the coroutine runners
    /// </summary>
    public static class CoroutineEngine
    {
        public static readonly CoroutineRunner PreExecute = new CoroutineRunner();
        public static readonly CoroutineRunner Execute = new CoroutineRunner();
        public static readonly CoroutineRunner PostExecute = new CoroutineRunner();

        public static void Reset()
        {
            PreExecute.StopAndClearAll();
            Execute.StopAndClearAll();
            PostExecute.StopAndClearAll();
        }
    }
}
