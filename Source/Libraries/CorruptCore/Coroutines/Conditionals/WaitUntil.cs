namespace RTCV.CorruptCore.Coroutines
{
    using System;

    public class WaitUntil : Yielder
    {
        Func<bool> pred;

        public WaitUntil(Func<bool> predicate)
        {
            this.pred = predicate;
        }

        public override bool Process()
        {
            return pred();
        }
    }
}
