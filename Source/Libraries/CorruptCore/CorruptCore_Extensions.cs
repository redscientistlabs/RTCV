namespace RTCV.CorruptCore
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Numerics;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using Ceras;

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
            foreach (var path in paths)
            {
                DirectoryRequired(path);
            }
        }

        #region STRING EXTENSIONS

        //taken from https://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string
        public static string CreateMD5(this string input)
        {
            // Use input string to calculate MD5 hash
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
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
        public static byte[] StringToByteArray(string hex)
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
        public static byte?[] StringToNullableByteArray(string hex)
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
            foreach (var c in Path.GetInvalidFileNameChars())
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
            var carryFlag = ShiftLeft(bytes);

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
            var carryFlag = ShiftRight(bytes);

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
            var leftMostCarryFlag = false;

            // Iterate through the elements of the array from left to right.
            for (var index = 0; index < bytes.Length; index++)
            {
                // If the leftmost bit of the current byte is 1 then we have a carry.
                var carryFlag = (bytes[index] & 0x80) > 0;

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
            var rightMostCarryFlag = false;
            var rightEnd = bytes.Length - 1;

            // Iterate through the elements of the array right to left.
            for (var index = rightEnd; index >= 0; index--)
            {
                // If the rightmost bit of the current byte is 1 then we have a carry.
                var carryFlag = (bytes[index] & 0x01) > 0;

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

        public static decimal GetDecimalValue(byte[] value, bool needsBytesFlipped)
        {
            var valueClone = (byte[])value.Clone();

            if (needsBytesFlipped)
            {
                Array.Reverse(valueClone);
            }

            return value.Length switch
            {
                1 => valueClone[0],
                2 => BitConverter.ToUInt16(valueClone, 0),
                4 => BitConverter.ToUInt32(valueClone, 0),
                8 => BitConverter.ToUInt64(valueClone, 0),
                _ => 0
            };
        }

        public static byte[] AddValueToByteArrayUnchecked(ref byte[] value, BigInteger addValue, bool isInputBigEndian)
        {
            if (isInputBigEndian)
            {
                Array.Reverse(value);
            }

            var isAdd = addValue >= 0;
            var bigintAddValueAbs = BigInteger.Abs(addValue);

            switch (value.Length)
            {
                case 1:
                    var addByteValue = (bigintAddValueAbs > byte.MaxValue ? byte.MaxValue : (byte)bigintAddValueAbs);

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
                        var ushortValue = BitConverter.ToUInt16(value, 0);
                        var addushortValue = (bigintAddValueAbs > ushort.MaxValue ? ushort.MaxValue : (ushort)bigintAddValueAbs);

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
                        var uintValue = BitConverter.ToUInt32(value, 0);
                        var adduintValue = (bigintAddValueAbs > uint.MaxValue ? uint.MaxValue : (uint)bigintAddValueAbs);

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
                        var ulongValue = BitConverter.ToUInt64(value, 0);
                        var addulongValue = (bigintAddValueAbs > ulong.MaxValue ? ulong.MaxValue : (ulong)bigintAddValueAbs);

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
                        var temp = new byte[value.Length + 1];
                        value.CopyTo(temp, 0);
                        var bigIntValue = new BigInteger(temp);

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

                        var added = bigIntValue.ToByteArray();

                        int length;
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
                        for (var i = 0; i < length; i++)
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

        private static BigInteger Mod(BigInteger x, BigInteger m)
        {
            return (x % m + m) % m;
        }

        public static byte[] GetByteArrayValue(int precision, ulong newValue, bool needsBytesFlipped = false)
        {
            switch (precision)
            {
                case 1:
                    return new byte[] { (byte)newValue };
                case 2:
                    {
                        var value = BitConverter.GetBytes(Convert.ToUInt16(newValue));
                        if (needsBytesFlipped)
                        {
                            Array.Reverse(value);
                        }

                        return value;
                    }
                case 4:
                    {
                        var value = BitConverter.GetBytes(Convert.ToUInt32(newValue));
                        if (needsBytesFlipped)
                        {
                            Array.Reverse(value);
                        }

                        return value;
                    }
                case 8:
                    {
                        var value = BitConverter.GetBytes(newValue);
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
            var arrayClone = (byte[])array.Clone();

            for (var i = 0; i < arrayClone.Length; i++)
            {
                array[i] = arrayClone[(arrayClone.Length - 1) - i];
            }

            return array;
        }

        public static byte?[] FlipBytes(this byte?[] array)
        {
            var arrayClone = (byte?[])array.Clone();

            for (var i = 0; i < arrayClone.Length; i++)
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

            for (var i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == null)
                    newArray[i] = 69;
                else
                    newArray[i] = bytes[i].Value;
            }

            return newArray;
        }

        #endregion BYTE ARRAY EXTENSIONS

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
            var rootPathAsUri = new Uri(rootPath + "\\");
            var fullPathAsUri = new Uri(fullPath);
            Uri diff = rootPathAsUri.MakeRelativeUri(fullPathAsUri);
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
    }

    public static class ObjectCopierCeras
    {
        public static T Clone<T>(T source)
        {
            var ser = new CerasSerializer(new SerializerConfig()
            {
                DefaultTargets = TargetMember.All
            });
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            //Return default of a null object
            if (source == null)
            {
                return default;
            }

            return ser.Deserialize<T>(ser.Serialize(source));
        }
    }
    //Export dgv to csv
    public static class CSVGenerator
    {
        public static string GenerateFromDGV(DataGridView dgv)
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

    //Lifted from Bizhawk https://github.com/TASVideos/BizHawk
#pragma warning disable 162
    public static class LogConsole
    {
        public static bool ConsoleVisible
        {
            get;
            private set;
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
        public static extern IntPtr GetSystemMenu(IntPtr hWndValue, bool isRevert);

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
