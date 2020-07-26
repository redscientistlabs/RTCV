namespace RTCV.CorruptCore.Coroutines
{
    using System;
    using System.Collections.Generic;

    //Can probably simplify logic or refactor

    /// <summary>
    /// Class form of a coroutine. Call StartCoroutine() on a <see cref="CoroutineRunner"/> in <see cref="CoroutineEngine"/> to start one
    /// </summary>
    public class Coroutine : IDisposable
    {
        public bool IsComplete { get; protected set; } = false;
        IEnumerator<Yielder> coroutine;
        Yielder currentConditional;
        /// <summary>
        /// Used to create a coroutine, must be created
        /// </summary>
        /// <param name="coroutine"></param>
        internal Coroutine(IEnumerator<Yielder> coroutine)
        {
            this.coroutine = coroutine;
            currentConditional = coroutine.Current;
        }

        /// <summary>
        /// Stops the coroutine and disposes it
        /// </summary>
        public void Stop()
        {
            IsComplete = true;
            Dispose();
        }

        public void Dispose()
        {
            if (coroutine != null)
            {
                coroutine.Dispose();
            }
        }

        public void DoCycle()
        {
            if (IsComplete)
            {
                //Do nothing
            }
            else if (currentConditional == null || currentConditional.Process())
            {
                //current conditional can be null if you use yield return null; basically acts as a single cycle skip
                IsComplete = !coroutine.MoveNext();
                currentConditional = coroutine.Current;
            }
        }
    }
}
