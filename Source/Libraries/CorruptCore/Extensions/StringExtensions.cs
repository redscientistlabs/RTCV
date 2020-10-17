namespace RTCV.CorruptCore.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public static class StringExtensions
    {
        //taken from https://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string
        public static string CreateMD5(this string input)
        {
            // Use input string to calculate MD5 hash
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                for (var i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets you a byte array from the CONTENTS of a string
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1062", Justification = "https://github.com/redscientistlabs/RTCV/issues/187")]
        public static byte[] ToByteArray(this string hex)
        {
            var lengthPadded = (hex.Length / 2) + (hex.Length % 2);
            var bytes = new byte[lengthPadded];
            if (hex == null)
            {
                return null;
            }

            var temp = hex.PadLeft(lengthPadded * 2, '0'); //*2 since a byte is two characters

            var j = 0;
            for (var i = 0; i < lengthPadded * 2; i += 2)
            {
                try
                {
                    var chars = temp.Substring(i, 2);

                    bytes[j] = (byte)Convert.ToUInt32(chars, 16);
                }
                catch (FormatException e)
                {
                    Console.Write(e);
                    return null;
                }

                j++;
            }
            return bytes;
        }

        /// <summary>
        /// Gets you a byte array from the CONTENTS of a string
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1062", Justification = "https://github.com/redscientistlabs/RTCV/issues/187")]
        public static byte?[] ToNullableByteArray(this string hex)
        {
            var lengthPadded = (hex.Length / 2) + (hex.Length % 2);
            var bytes = new byte?[lengthPadded];
            if (hex == null)
            {
                return null;
            }

            var temp = hex.PadLeft(lengthPadded * 2, '0'); //*2 since a byte is two characters

            var j = 0;
            for (var i = 0; i < lengthPadded * 2; i += 2)
            {
                try
                {
                    var chars = temp.Substring(i, 2);

                    if (chars == "??")
                    {
                        bytes[j] = null;
                    }
                    else
                    {
                        bytes[j] = (byte)Convert.ToUInt32(chars, 16);
                    }
                }
                catch (FormatException e)
                {
                    Console.Write(e);
                    return null;
                }

                j++;
            }
            return bytes;
        }


        /// <summary>
        /// Gets you a byte array from the CONTENTS of a string 0 padded on the left to a specific length
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] ToByteArrayPadLeft(this string hex, int precision)
        {
            var bytes = new byte[precision];
            if (hex == null)
            {
                return null;
            }

            var temp = hex.PadLeft(precision * 2, '0'); //*2 since a byte is two characters

            var j = 0;
            for (var i = 0; i < precision * 2; i += 2)
            {
                try
                {
                    if (!byte.TryParse(temp.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture,
                        out var b))
                    {
                        return null;
                    }

                    bytes[j] = b;
                }
                catch (FormatException e)
                {
                    Console.Write(e);
                    return null;
                }

                j++;
            }
            return bytes;
        }

        public static string MakeSafeFilename(string filename, char replaceChar)
        {
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }

            foreach (var c in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, replaceChar);
            }
            return filename;
        }
    }
}
