namespace RTCV.Common.CustomExtensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    // https://stackoverflow.com/a/11640700/10923568
    public static class RandomExtensions
    {
        //returns a uniformly random ulong between ulong.Min inclusive and ulong.Max inclusive
        [SuppressMessage("Microsoft.Design", "CA1062", Justification = "https://github.com/redscientistlabs/RTCV/issues/187")]
        public static ulong NextULong(this Random rng)
        {
            var buf = new byte[8];
            rng.NextBytes(buf);
            return BitConverter.ToUInt64(buf, 0);
        }

        //returns a uniformly random long between long.Min inclusive and long.Max inclusive
        [SuppressMessage("Microsoft.Design", "CA1062", Justification = "https://github.com/redscientistlabs/RTCV/issues/187")]
        public static long NextLong(this Random rng)
        {
            var buf = new byte[8];
            rng.NextBytes(buf);
            return BitConverter.ToInt64(buf, 0);
        }

        //returns a uniformly random long between Min and Max without modulo bias
        public static long NextLong(this Random rng, long min, long max, bool inclusiveUpperBound = false)
        {
            var range = (ulong)(max - min);

            if (inclusiveUpperBound)
            {
                if (range == ulong.MaxValue)
                {
                    return rng.NextLong();
                }
                range++;
            }

            if (range <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "Max must be greater than min when inclusiveUpperBound is false, and greater than or equal to when true");
            }

            var limit = ulong.MaxValue - (ulong.MaxValue % range);
            ulong r;
            do
            {
                r = rng.NextULong();
            } while (r > limit);
            return (long)((r % range) + (ulong)min);
        }

        //returns a uniformly random ulong between Min and Max without modulo bias
        public static ulong NextULong(this Random rng, ulong min, ulong max, bool inclusiveUpperBound = false)
        {
            var range = max - min;

            if (inclusiveUpperBound)
            {
                if (range == ulong.MaxValue)
                {
                    return rng.NextULong();
                }

                range++;
            }

            if (range <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "Max must be greater than min when inclusiveUpperBound is false, and greater than or equal to when true");
            }

            var limit = ulong.MaxValue - (ulong.MaxValue % range);
            ulong r;
            do
            {
                r = rng.NextULong();
            } while (r > limit);

            return (r % range) + min;
        }
    }
}
