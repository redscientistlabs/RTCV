namespace RTCV.CorruptCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Numerics;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Windows.Forms;
    using Ceras;
    using Newtonsoft.Json;
    using RTCV.NetCore.NetCore_Extensions;

    public static class CorruptCore_Extensions
    {
        public static void DirectoryRequired(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void DirectoryRequired(string[] paths)
        {
            foreach (string path in paths)
            {
                DirectoryRequired(path);
            }
        }

        #region ARRAY EXTENSIONS

        public static int IndexOf<T>(this T[] haystack, T[] needle)
        {
            if ((needle != null) && (haystack.Length >= needle.Length))
            {
                for (int l = 0; l < haystack.Length - needle.Length + 1; l++)
                {
                    if (!needle.Where((data, index) => !haystack[l + index].Equals(data)).Any())
                    {
                        return l;
                    }
                }
            }

            return -1;
        }

        public static T[] SubArray<T>(this T[] data, long index, long length)
        {
            T[] result = new T[length];

            if (data == null)
            {
                return null;
            }

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

        #endregion ARRAY EXTENSIONS

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

        //taken from https://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string
        public static string CreateMD5(this string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets you a byte array representing the characters in a string.
        /// THIS DOES NOT CONVERT A STRING TO A BYTE ARRAY CONTAINING THE SAME CHARACTERS
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Gets you a byte array from the CONTENTS of a string
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string hex)
        {
            var lengthPadded = (hex.Length / 2) + (hex.Length % 2);
            var bytes = new byte[lengthPadded];
            if (hex == null)
            {
                return null;
            }

            string temp = hex.PadLeft(lengthPadded * 2, '0'); //*2 since a byte is two characters

            int j = 0;
            for (var i = 0; i < lengthPadded * 2; i += 2)
            {
                try
                {
                    string chars = temp.Substring(i, 2);

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
        public static byte?[] StringToNullableByteArray(string hex)
        {
            var lengthPadded = (hex.Length / 2) + (hex.Length % 2);
            var bytes = new byte?[lengthPadded];
            if (hex == null)
            {
                return null;
            }

            string temp = hex.PadLeft(lengthPadded * 2, '0'); //*2 since a byte is two characters

            int j = 0;
            for (var i = 0; i < lengthPadded * 2; i += 2)
            {
                try
                {
                    string chars = temp.Substring(i, 2);

                    if (chars == "??")
                        bytes[j] = null;
                    else
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
        /// Gets you a byte array from the CONTENTS of a string 0 padded on the left to a specific length
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] StringToByteArrayPadLeft(string hex, int precision)
        {
            var bytes = new byte[precision];
            if (hex == null)
            {
                return null;
            }

            string temp = hex.PadLeft(precision * 2, '0'); //*2 since a byte is two characters

            int j = 0;
            for (var i = 0; i < precision * 2; i += 2)
            {
                try
                {
                    if (!byte.TryParse(temp.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture,
                        out byte b))
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
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, replaceChar);
            }
            return filename;
        }

        #endregion STRING EXTENSIONS

        #region BYTE ARRAY EXTENSIONS

        //Thanks to JamieSee https://stackoverflow.com/questions/8440938/c-sharp-left-shift-an-entire-byte-array
        /// <summary>
        /// Rotates the bits in an array of bytes to the left.
        /// </summary>
        /// <param name="bytes">The byte array to rotate.</param>
        public static void RotateLeft(byte[] bytes)
        {
            bool carryFlag = ShiftLeft(bytes);

            if (carryFlag == true)
            {
                bytes[bytes.Length - 1] = (byte)(bytes[bytes.Length - 1] | 0x01);
            }
        }

        /// <summary>
        /// Rotates the bits in an array of bytes to the right.
        /// </summary>
        /// <param name="bytes">The byte array to rotate.</param>
        public static void RotateRight(byte[] bytes)
        {
            bool carryFlag = ShiftRight(bytes);

            if (carryFlag == true)
            {
                bytes[0] = (byte)(bytes[0] | 0x80);
            }
        }

        /// <summary>
        /// Shifts the bits in an array of bytes to the left.
        /// </summary>
        /// <param name="bytes">The byte array to shift.</param>
        public static bool ShiftLeft(byte[] bytes)
        {
            bool leftMostCarryFlag = false;

            // Iterate through the elements of the array from left to right.
            for (int index = 0; index < bytes.Length; index++)
            {
                // If the leftmost bit of the current byte is 1 then we have a carry.
                bool carryFlag = (bytes[index] & 0x80) > 0;

                if (index > 0)
                {
                    if (carryFlag == true)
                    {
                        // Apply the carry to the rightmost bit of the current bytes neighbor to the left.
                        bytes[index - 1] = (byte)(bytes[index - 1] | 0x01);
                    }
                }
                else
                {
                    leftMostCarryFlag = carryFlag;
                }

                bytes[index] = (byte)(bytes[index] << 1);
            }

            return leftMostCarryFlag;
        }

        /// <summary>
        /// Shifts the bits in an array of bytes to the right.
        /// </summary>
        /// <param name="bytes">The byte array to shift.</param>
        public static bool ShiftRight(byte[] bytes)
        {
            bool rightMostCarryFlag = false;
            int rightEnd = bytes.Length - 1;

            // Iterate through the elements of the array right to left.
            for (int index = rightEnd; index >= 0; index--)
            {
                // If the rightmost bit of the current byte is 1 then we have a carry.
                bool carryFlag = (bytes[index] & 0x01) > 0;

                if (index < rightEnd)
                {
                    if (carryFlag == true)
                    {
                        // Apply the carry to the leftmost bit of the current bytes neighbor to the right.
                        bytes[index + 1] = (byte)(bytes[index + 1] | 0x80);
                    }
                }
                else
                {
                    rightMostCarryFlag = carryFlag;
                }

                bytes[index] = (byte)(bytes[index] >> 1);
            }

            return rightMostCarryFlag;
        }

        public static ulong GetNumericMaxValue(byte[] Value)
        {
            switch (Value.Length)
            {
                case 1:
                    return byte.MaxValue;
                case 2:
                    return ushort.MaxValue;
                case 4:
                    return uint.MaxValue;
                case 8:
                    return ulong.MaxValue;
            }

            return 0;
        }

        public static decimal GetDecimalValue(byte[] value, bool needsBytesFlipped)
        {
            byte[] _value = (byte[])value.Clone();

            if (needsBytesFlipped)
            {
                Array.Reverse(_value);
            }

            switch (value.Length)
            {
                case 1:
                    return _value[0];
                case 2:
                    return BitConverter.ToUInt16(_value, 0);
                case 4:
                    return BitConverter.ToUInt32(_value, 0);
                case 8:
                    return BitConverter.ToUInt64(_value, 0);
            }

            return 0;
        }

        public static byte[] AddValueToByteArrayUnchecked(ref byte[] value, BigInteger addValue, bool isInputBigEndian)
        {
            if (isInputBigEndian)
            {
                Array.Reverse(value);
            }

            bool isAdd = addValue >= 0;
            BigInteger bigintAddValueAbs = BigInteger.Abs(addValue);

            switch (value.Length)
            {
                case 1:
                    byte addByteValue = (bigintAddValueAbs > byte.MaxValue ? byte.MaxValue : (byte)bigintAddValueAbs);

                    if (isAdd)
                    {
                        unchecked { value[0] += addByteValue; }
                    }
                    else
                    {
                        unchecked { value[0] -= addByteValue; }
                    }

                    return value;

                case 2:
                    {
                        ushort ushortValue = BitConverter.ToUInt16(value, 0);
                        ushort addushortValue = (bigintAddValueAbs > ushort.MaxValue ? ushort.MaxValue : (ushort)bigintAddValueAbs);

                        if (isAdd)
                        {
                            unchecked { ushortValue += addushortValue; }
                        }
                        else
                        {
                            unchecked { ushortValue -= addushortValue; }
                        }

                        value = BitConverter.GetBytes(ushortValue);

                        if (isInputBigEndian)
                        {
                            Array.Reverse(value);
                        }

                        return value;
                    }
                case 4:
                    {
                        uint uintValue = BitConverter.ToUInt32(value, 0);
                        uint adduintValue = (bigintAddValueAbs > uint.MaxValue ? uint.MaxValue : (uint)bigintAddValueAbs);

                        if (isAdd)
                        {
                            unchecked { uintValue += adduintValue; }
                        }
                        else
                        {
                            unchecked { uintValue -= adduintValue; }
                        }

                        value = BitConverter.GetBytes(uintValue);

                        if (isInputBigEndian)
                        {
                            Array.Reverse(value);
                        }

                        return value;
                    }
                case 8:
                    {
                        ulong ulongValue = BitConverter.ToUInt64(value, 0);
                        ulong addulongValue = (bigintAddValueAbs > ulong.MaxValue ? ulong.MaxValue : (ulong)bigintAddValueAbs);

                        if (isAdd)
                        {
                            unchecked { ulongValue += addulongValue; }
                        }
                        else
                        {
                            unchecked { ulongValue -= addulongValue; }
                        }

                        value = BitConverter.GetBytes(ulongValue);

                        if (isInputBigEndian)
                        {
                            Array.Reverse(value);
                        }

                        return value;
                    }
                default:
                    {
                        //Gets us a positive value
                        byte[] temp = new byte[value.Length + 1];
                        value.CopyTo(temp, 0);
                        BigInteger bigIntValue = new BigInteger(temp);

                        if (isAdd)
                        {
                            bigIntValue += bigintAddValueAbs;
                        }
                        else
                        {
                            bigIntValue -= bigintAddValueAbs;
                        }

                        //Calculate the max value you can store in this many bits
                        BigInteger maxValue = BigInteger.Pow(2, value.Length * 8) - 1;

                        if (bigIntValue > maxValue)
                        {
                            bigIntValue = bigIntValue % maxValue - 1; //Works fine for positive
                        }
                        else if (bigIntValue < 0)
                        {
                            bigIntValue = Mod(maxValue, bigIntValue); //% means remainder in c#
                        }

                        byte[] added = bigIntValue.ToByteArray();

                        var length = 0;
                        //So with BigInteger, it returns a signed value. That means there's a chance we get a fun 0 appended at the end of added[]
                        //There's also a chance we get a value with less bytes than we put in. If this is the case, we want to copy it over left to right still
                        //So that means if added is larger we want that & if added is smaller we want added's Length
                        if (added.Length > value.Length)
                        {
                            length = value.Length;
                        }
                        else
                        {
                            length = added.Length;
                        }

                        //Don't use copyto as we actually want to copy a trimmed array out (left aligned)
                        for (int i = 0; i < length; i++)
                        {
                            value[i] = added[i];
                        }

                        if (isInputBigEndian)
                        {
                            Array.Reverse(value);
                        }

                        return value;
                    }
            }

            //return null;
        }

        private static decimal Mod(decimal x, long m)
        {
            return (x % m + m) % m;
        }

        private static BigInteger Mod(BigInteger x, BigInteger m)
        {
            return (x % m + m) % m;
        }

        public static byte[] GetByteArrayValue(int precision, decimal newValue, bool needsBytesFlipped = false)
        {
            switch (precision)
            {
                case 1:
                    return new byte[] { (byte)newValue };
                case 2:
                    {
                        byte[] value = BitConverter.GetBytes(Convert.ToUInt16(newValue));
                        if (needsBytesFlipped)
                        {
                            Array.Reverse(value);
                        }

                        return value;
                    }
                case 4:
                    {
                        byte[] value = BitConverter.GetBytes(Convert.ToUInt32(newValue));
                        if (needsBytesFlipped)
                        {
                            Array.Reverse(value);
                        }

                        return value;
                    }
                case 8:
                    {
                        byte[] value = BitConverter.GetBytes(Convert.ToUInt64(newValue));
                        if (needsBytesFlipped)
                        {
                            Array.Reverse(value);
                        }

                        return value;
                    }
            }

            return null;
        }

        public static byte[] GetByteArrayValue(int precision, ulong newValue, bool needsBytesFlipped = false)
        {
            switch (precision)
            {
                case 1:
                    return new byte[] { (byte)newValue };
                case 2:
                    {
                        byte[] value = BitConverter.GetBytes(Convert.ToUInt16(newValue));
                        if (needsBytesFlipped)
                        {
                            Array.Reverse(value);
                        }

                        return value;
                    }
                case 4:
                    {
                        byte[] value = BitConverter.GetBytes(Convert.ToUInt32(newValue));
                        if (needsBytesFlipped)
                        {
                            Array.Reverse(value);
                        }

                        return value;
                    }
                case 8:
                    {
                        byte[] value = BitConverter.GetBytes(newValue);
                        if (needsBytesFlipped)
                        {
                            Array.Reverse(value);
                        }

                        return value;
                    }
            }

            return null;
        }

        public static byte[] FlipBytes(this byte[] array)
        {
            byte[] arrayClone = (byte[])array.Clone();

            for (int i = 0; i < arrayClone.Length; i++)
            {
                array[i] = arrayClone[(arrayClone.Length - 1) - i];
            }

            return array;
        }

        public static byte?[] FlipBytes(this byte?[] array)
        {
            byte?[] arrayClone = (byte?[])array.Clone();

            for (int i = 0; i < arrayClone.Length; i++)
            {
                array[i] = arrayClone[(arrayClone.Length - 1) - i];
            }

            return array;
        }

        public static byte[] PadLeft(this byte[] input, int length)
        {
            var newArray = new byte[length];

            var startAt = newArray.Length - input.Length;
            Buffer.BlockCopy(input, 0, newArray, startAt, input.Length);
            return newArray;
        }

        /// <summary>
        /// Converts bytes to an uppercase string of hex numbers in upper case without any spacing or anything
        /// </summary>
        public static string BytesToHexString(this byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.AppendFormat("{0:X2}", b);
            }

            return sb.ToString();
        }

        public static byte[] Flatten69(this byte?[] bytes)
        {
            if (bytes == null)
                return null;

            var newArray = new byte[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == null)
                    newArray[i] = 69;
                else
                    newArray[i] = bytes[i].Value;
            }

            return newArray;
        }

        #endregion BYTE ARRAY EXTENSIONS

        #region BITARRAY EXTENSIONS

        public static byte[] ToByteArray(this BitArray bits)
        {
            int numBytes = bits.Count / 8;
            if (bits.Count % 8 != 0)
            {
                numBytes++;
            }

            byte[] bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                {
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));
                }

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return bytes;
        }

        #endregion BITARRAY EXTENSIONS

        #region COLOR EXTENSIONS

        /// <summary>
        /// Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1.
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="Color"/> structure.
        /// </returns>
        public static Color ChangeColorBrightness(this Color color, float correctionFactor)
        {
            float red = color.R;
            float green = color.G;
            float blue = color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        #endregion COLOR EXTENSIONS

        #region PATH EXTENSIONS

        public static string GetRelativePath(string rootPath, string fullPath)
        {
            Uri _rootPath = new Uri(rootPath + "\\");
            Uri _fullPath = new Uri(fullPath);
            Uri diff = _rootPath.MakeRelativeUri(_fullPath);
            return Uri.UnescapeDataString(diff.OriginalString).Replace('/', '\\');
        }
        //https://stackoverflow.com/a/23354773
        public static bool IsOrIsSubDirectoryOf(string candidate, string other)
        {
            var isChild = false;
            try
            {
                var candidateInfo = new DirectoryInfo(candidate);
                var otherInfo = new DirectoryInfo(other);

                while (true)
                {
                    if (Path.GetFullPath(candidateInfo.FullName + Path.DirectorySeparatorChar) == Path.GetFullPath(otherInfo.FullName + Path.DirectorySeparatorChar))
                    {
                        isChild = true;
                        break;
                    }
                    if (candidateInfo.Parent == null)
                    {
                        break;
                    }

                    candidateInfo = candidateInfo.Parent;
                }
            }
            catch (Exception error)
            {
                var message = string.Format("Unable to check directories {0} and {1}: {2}", candidate, other, error);
                Trace.WriteLine(message);
            }

            return isChild;
        }
        #endregion PATH EXTENSIONS

        #region STREAM EXTENSIONS
        //Thanks! https://stackoverflow.com/a/13021983
        public static long CopyBytes(long bytesRequired, Stream inStream, Stream outStream)
        {
            long readSoFar = 0L;
            var buffer = new byte[64 * 1024];
            do
            {
                var toRead = Math.Min(bytesRequired - readSoFar, buffer.Length);
                var readNow = inStream.Read(buffer, 0, (int)toRead);
                if (readNow == 0)
                {
                    break; // End of stream
                }

                outStream.Write(buffer, 0, readNow);
                readSoFar += readNow;
            } while (readSoFar < bytesRequired);
            return readSoFar;
        }
        #endregion

        #region LIST EXTENSIONS
        //https://stackoverflow.com/a/29119974
        public static System.Data.DataTable ToDataTable<T>(this List<T> items)
        {
            var tb = new System.Data.DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (prop.PropertyType.IsEnum)
                {
                    tb.Columns.Add(prop.Name, typeof(string));
                }
                else if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    tb.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                }
                else
                {
                    tb.Columns.Add(prop.Name, prop.PropertyType);
                }
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    if (props[i].PropertyType.IsEnum)
                    {
                        object val = props[i].GetValue(item, null);
                        values[i] = Enum.GetName(props[i].PropertyType, val);
                    }
                    else
                    {
                        values[i] = props[i].GetValue(item, null);
                    }
                }

                tb.Rows.Add(values);
            }

            return tb;
        }
        #endregion
        #region BINDINGLIST EXTENSIONS

        public static BindingList<T> AddRange<T>(this BindingList<T> input, IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                input.Add(item);
            }

            return input;
        }

        public static BindingList<T> ToBindingList<T>(this IEnumerable<T> collection)
        {
            return new BindingList<T>(collection.ToList());
        }
        #endregion

        #region Image CorruptCore_Extensions

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, imageFormat);
            return ms.ToArray();
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        #endregion
    }

    public static class ObjectCopier
    {
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            //Return default of a null object
            if (object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    public static class ObjectCopierCeras
    {
        public static T Clone<T>(T source)
        {
            CerasSerializer ser = new CerasSerializer(new SerializerConfig()
            {
                DefaultTargets = TargetMember.All
            });
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            //Return default of a null object
            if (object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            return ser.Deserialize<T>(ser.Serialize(source));
        }
    }
    //Export dgv to csv
    public class CSVGenerator
    {
        public string GenerateFromDGV(DataGridView dgv)
        {
            var sb = new StringBuilder();
            var headers = dgv.Columns.Cast<DataGridViewColumn>();

            sb.AppendLine(string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, headers.Select(column => "\"" + column.HeaderText + "\"").ToArray()));
            foreach (DataGridViewRow row in dgv.Rows)
            {
                var cells = row.Cells.Cast<DataGridViewCell>();
                sb.AppendLine(string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, cells.Select(cell => "\"" + cell.Value + "\"").ToArray()));
            }
            return sb.ToString();
        }
    }

    public static class JsonHelper
    {
        public static void Serialize(object value, Stream s, Formatting f = Formatting.None, SafeJsonTypeSerialization.JsonKnownTypesBinder binder = null)
        {
            using (StreamWriter writer = new StreamWriter(s))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                JsonSerializer ser = new JsonSerializer
                {
                    Formatting = f,
                    SerializationBinder = binder ?? new SafeJsonTypeSerialization.JsonKnownTypesBinder()
                };
                ser.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                ser.Serialize(jsonWriter, value);
                jsonWriter.Flush();
            }
        }

        public static T Deserialize<T>(Stream s, SafeJsonTypeSerialization.JsonKnownTypesBinder binder = null)
        {
            using (StreamReader reader = new StreamReader(s))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer ser = new JsonSerializer()
                {
                    SerializationBinder = binder ?? new SafeJsonTypeSerialization.JsonKnownTypesBinder()
                };
                return ser.Deserialize<T>(jsonReader);
            }
        }

        //Wrap JsonConvert so we can access this in vanguard implementations without importing json.net directly
        public static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }

    //Lifted from Bizhawk https://github.com/TASVideos/BizHawk
#pragma warning disable 162
    public static class LogConsole
    {
        public static bool ConsoleVisible
        {
            get;
            private set;
        }

        //static bool NeedToRelease;
        private static string SkipEverythingButProgramInCommandLine(string cmdLine)
        {
            //skip past the program name. can anyone think of a better way to do this?
            //we could use CommandLineToArgvW (commented out below) but then we would just have to re-assemble and potentially re-quote it
            int childCmdLine = 0;
            int lastSlash = 0;
            int lastGood = 0;
            bool quote = false;
            for (; ; )
            {
                char cur = cmdLine[childCmdLine];
                childCmdLine++;
                if (childCmdLine == cmdLine.Length)
                {
                    break;
                }

                bool thisIsQuote = (cur == '\"');
                if (cur == '\\' || cur == '/')
                {
                    lastSlash = childCmdLine;
                }

                if (quote)
                {
                    if (thisIsQuote)
                    {
                        quote = false;
                    }
                    else
                    {
                        lastGood = childCmdLine;
                    }
                }
                else
                {
                    if (cur == ' ' || cur == '\t')
                    {
                        break;
                    }

                    if (thisIsQuote)
                    {
                        quote = true;
                    }

                    lastGood = childCmdLine;
                }
            }
            string remainder = cmdLine.Substring(childCmdLine);
            string path = cmdLine.Substring(lastSlash, lastGood - lastSlash);
            return path + " " + remainder;
        }

        private static IntPtr oldOut;
        private static IntPtr conOut;
        private static bool hasConsole;
        private static bool attachedConsole;
        private static bool shouldRedirectStdout;

        public static void CreateConsole()
        {
            //(see desmume for the basis of some of this logic)

            if (hasConsole)
            {
                return;
            }

            if (oldOut == IntPtr.Zero)
            {
                oldOut = Win32.GetStdHandle(-11); //STD_OUTPUT_HANDLE
            }

            Win32.FileType fileType = Win32.GetFileType(oldOut);

            //stdout is already connected to something. keep using it and dont let the console interfere
            shouldRedirectStdout = (fileType == Win32.FileType.FileTypeUnknown || fileType == Win32.FileType.FileTypePipe);

            //attach to an existing console
            attachedConsole = false;

            //ever since a recent KB, XP-based systems glitch out when attachconsole is called and theres no console to attach to.
            if (Environment.OSVersion.Version.Major != 5)
            {
                if (Win32.AttachConsole(-1))
                {
                    hasConsole = true;
                    attachedConsole = true;
                }
            }

            if (!attachedConsole)
            {
                Win32.FreeConsole();
                if (Win32.AllocConsole())
                {
                    //set icons for the console so we can tell them apart from the main window
                    //    Win32.SendMessage(Win32.GetConsoleWindow(), 0x0080/*WM_SETICON*/, (IntPtr)0/*ICON_SMALL*/, Properties.Resources.console16x16.GetHicon());
                    //    Win32.SendMessage(Win32.GetConsoleWindow(), 0x0080/*WM_SETICON*/, (IntPtr)1/*ICON_LARGE*/, Properties.Resources.console32x32.GetHicon());
                    hasConsole = true;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Couldn't allocate win32 console: {0}", Marshal.GetLastWin32Error()));
                }
            }

            if (hasConsole)
            {
                IntPtr ptr = Win32.GetCommandLine();
                string commandLine = Marshal.PtrToStringAuto(ptr);
                Console.Title = SkipEverythingButProgramInCommandLine(commandLine);
            }

            if (shouldRedirectStdout)
            {
                conOut = Win32.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);

                if (!Win32.SetStdHandle(-11, conOut))
                {
                    throw new Exception("SetStdHandle() failed");
                }
            }

            //DotNetRewireConout();
            hasConsole = true;

            if (attachedConsole)
            {
                Console.WriteLine();
            }
            //Disable the X button on the console window
            Win32.EnableMenuItem(Win32.GetSystemMenu(Win32.GetConsoleWindow(), false), Win32.SC_CLOSE, Win32.MF_DISABLED);

            ConsoleVisible = true;
        }

        public static void ReleaseConsole()
        {
            if (!hasConsole)
            {
                return;
            }

            if (shouldRedirectStdout)
            {
                Win32.CloseHandle(conOut);
            }

            if (!attachedConsole)
            {
                Win32.FreeConsole();
            }

            Win32.SetStdHandle(-11, oldOut);

            conOut = IntPtr.Zero;
            hasConsole = false;
        }

        public static void ShowConsole()
        {
            var handle = Win32.GetConsoleWindow();
            Win32.ShowWindow(handle, Win32.SW_SHOW);
            ConsoleVisible = true;
        }

        public static void HideConsole()
        {
            var handle = Win32.GetConsoleWindow();
            Win32.ShowWindow(handle, Win32.SW_HIDE);
            ConsoleVisible = false;
        }

        public static void ToggleConsole()
        {
            if (ConsoleVisible)
            {
                HideConsole();
            }
            else
            {
                ShowConsole();
            }
        }
    }
    //Lifted from Bizhawk https://github.com/TASVideos/BizHawk
    public static class Win32
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern FileType GetFileType(IntPtr hFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetCommandLine();

        public enum FileType : int
        {
            FileTypeChar = 0x0002,
            FileTypeDisk = 0x0001,
            FileTypePipe = 0x0003,
            FileTypeRemote = 0x8000,
            FileTypeUnknown = 0x0000,
        }

        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;

        internal const int SC_CLOSE = 0xF060;           //close button's code in Windows API
        internal const int MF_ENABLED = 0x00000000;     //enabled button status
        internal const int MF_GRAYED = 0x1;             //disabled button status (enabled = false)
        internal const int MF_DISABLED = 0x00000002;    //disabled button status

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr HWNDValue, bool isRevert);

        [DllImport("user32.dll")]
        public static extern int EnableMenuItem(IntPtr tMenu, int targetItem, int targetStatus);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = false)]
        public static extern bool FreeConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetStdHandle(int nStdHandle, IntPtr hConsoleOutput);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateFile(
            string fileName,
            int desiredAccess,
            int shareMode,
            IntPtr securityAttributes,
            int creationDisposition,
            int flagsAndAttributes,
            IntPtr templateFile);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);
    }
}
