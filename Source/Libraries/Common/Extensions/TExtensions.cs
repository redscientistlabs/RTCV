namespace RTCV.Common
{
    using System;

    public static class TExtensions
    {
        /// <summary>
        /// Force the value to be strictly between min and max (both exclued)
        /// </summary>
        /// <typeparam name="T">Anything that implements <see cref="IComparable{T}"/></typeparam>
        /// <param name="val">Value that will be clamped</param>
        /// <param name="min">Minimum allowed</param>
        /// <param name="max">Maximum allowed</param>
        /// <returns>The value if strictly between min and max; otherwise min (or max depending of what is passed)</returns>
        public static T Clamp<T>(this T val, T min, T max)
            where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0)
            {
                return min;
            }

            if (val.CompareTo(max) > 0)
            {
                return max;
            }

            return val;
        }
    }
}
