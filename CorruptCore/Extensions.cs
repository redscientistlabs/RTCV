using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTCV.CorruptCore
{
    public static class RTCV_Extensions
    {

        #region RNG EXTENSIONS

        public static Random rnd   //RND Singulatiry
        {
            get
            {
                if (_rnd == null)
                    _rnd = new Random(Convert.ToInt32(DateTime.Now.ToFileTimeUtc()));

                return _rnd;
            }
        }
        private static Random _rnd = null;


        public static T PickRandom<T>(this T[] source) //Allows to pick a random item from any array type
        {
            return source[rnd.Next(source.Count())];
        }

        #endregion

        #region ARRAY EXTENSIONS
        public static T[] SubArray<T>(this T[] data, long index, long length)
        {
            T[] result = new T[length];

            if (data == null)
                return null;

            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static T[] FlipWords<T>(this T[] data, int wordSize)
        {
            //2 : 16-bit
            //4 : 32-bit
            //8 : 64-bit

            T[] result = new T[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                int wordPos = i % wordSize;
                int wordAddress = i - wordPos;
                int newPos = wordAddress + (wordSize - (wordPos + 1));

                result[newPos] = data[i];
            }

            return result;
        }

        #endregion

        #region STRING EXTENSIONS
        public static string ToBase64(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64(this string base64)
        {
            var data = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(data);
        }

        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        #endregion

        #region BYTE ARRAY EXTENSIONS

        public static long getNumericMaxValue(byte[] Value)
        {
            switch (Value.Length)
            {
                case 1:
                    return byte.MaxValue;
                case 2:
                    return UInt16.MaxValue;
                case 4:
                    return UInt32.MaxValue;
            }

            return 0;
        }

        public static decimal getDecimalValue(byte[] Value)
        {
            switch (Value.Length)
            {
                case 1:
                    return (int)Value[0];
                case 2:
                    return BitConverter.ToUInt16(Value, 0);
                case 4:
                    return BitConverter.ToUInt32(Value, 0);
            }

            return 0;
        }

        public static byte[] getByteArrayValue(byte[] originalValue, decimal newValue)
        {
            switch (originalValue.Length)
            {
                case 1:
                    return new byte[] { (byte)newValue };
                case 2:
                    return BitConverter.GetBytes(Convert.ToUInt16(newValue));
                case 4:
                    return BitConverter.GetBytes(Convert.ToUInt32(newValue));
            }

            return null;
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).ToLower();
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        #endregion




    }
}
