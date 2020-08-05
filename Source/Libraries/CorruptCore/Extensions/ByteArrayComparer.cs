namespace RTCV.CorruptCore.Extensions
{
    using System.Collections.Generic;

    public class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(byte[] a)
        {
            uint b = 0;
            for (int i = 0; i < a.Length; i++)
            {
                b = ((b << 23) | (b >> 9)) ^ a[i];
            }

            return unchecked((int)b);
        }

        public ByteArrayComparer()
        {
        }
    }
}
