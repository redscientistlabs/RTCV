namespace RTCV.CorruptCore.Extensions
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class NullableByteArrayComparer : IEqualityComparer<byte?[]>
    {
        public bool Equals(byte?[] a, byte?[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == null || b[i] == null) //wildcards
                {
                    return true;
                }

                if (a[i].Value != b[i].Value)
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(byte?[] a)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            uint b = 0;
            for (int i = 0; i < a.Length; i++)
            {
                b = ((b << 23) | (b >> 9)) ^ (a[i] ?? 69);
            }

            return unchecked((int)b);
        }

        public NullableByteArrayComparer()
        {
        }
    }
}
