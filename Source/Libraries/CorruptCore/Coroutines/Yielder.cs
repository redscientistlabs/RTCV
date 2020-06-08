namespace RTCV.CorruptCore.Coroutines
{
    public abstract class Yielder
    {
        /// <summary>
        /// Implement a yielder. Return true to continue the coroutine.
        /// </summary>
        /// <returns>whether or not to continue</returns>
        public abstract bool Process();
    }
}
