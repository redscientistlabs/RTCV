namespace RTCV.CorruptCore.Extensions
{
    using System;
    using System.Numerics;
    using System.Text;

    public static class ByteArrayExtensions
    {
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

        private static BigInteger Mod(BigInteger x, BigInteger m)
        {
            return (x % m + m) % m;
        }

        public static byte[] AddValueToByteArrayUnchecked(this byte[] input, BigInteger addValue, bool isInputBigEndian)
        {
            if (isInputBigEndian)
            {
                Array.Reverse(input);
            }

            var isAdd = addValue >= 0;
            var bigintAddValueAbs = BigInteger.Abs(addValue);

            switch (input.Length)
            {
                case 1:
                    var addByteValue = (bigintAddValueAbs > byte.MaxValue ? byte.MaxValue : (byte)bigintAddValueAbs);

                    if (isAdd)
                    {
                        unchecked { input[0] += addByteValue; }
                    }
                    else
                    {
                        unchecked { input[0] -= addByteValue; }
                    }

                    return input;

                case 2:
                    {
                        var ushortValue = BitConverter.ToUInt16(input, 0);
                        var addushortValue = (bigintAddValueAbs > ushort.MaxValue ? ushort.MaxValue : (ushort)bigintAddValueAbs);

                        if (isAdd)
                        {
                            unchecked { ushortValue += addushortValue; }
                        }
                        else
                        {
                            unchecked { ushortValue -= addushortValue; }
                        }

                        input = BitConverter.GetBytes(ushortValue);

                        if (isInputBigEndian)
                        {
                            Array.Reverse(input);
                        }

                        return input;
                    }
                case 4:
                    {
                        var uintValue = BitConverter.ToUInt32(input, 0);
                        var adduintValue = (bigintAddValueAbs > uint.MaxValue ? uint.MaxValue : (uint)bigintAddValueAbs);

                        if (isAdd)
                        {
                            unchecked { uintValue += adduintValue; }
                        }
                        else
                        {
                            unchecked { uintValue -= adduintValue; }
                        }

                        input = BitConverter.GetBytes(uintValue);

                        if (isInputBigEndian)
                        {
                            Array.Reverse(input);
                        }

                        return input;
                    }
                case 8:
                    {
                        var ulongValue = BitConverter.ToUInt64(input, 0);
                        var addulongValue = (bigintAddValueAbs > ulong.MaxValue ? ulong.MaxValue : (ulong)bigintAddValueAbs);

                        if (isAdd)
                        {
                            unchecked { ulongValue += addulongValue; }
                        }
                        else
                        {
                            unchecked { ulongValue -= addulongValue; }
                        }

                        input = BitConverter.GetBytes(ulongValue);

                        if (isInputBigEndian)
                        {
                            Array.Reverse(input);
                        }

                        return input;
                    }
                default:
                    {
                        //Gets us a positive value
                        var temp = new byte[input.Length + 1];
                        input.CopyTo(temp, 0);
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
                        BigInteger maxValue = BigInteger.Pow(2, input.Length * 8) - 1;

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
                        if (added.Length > input.Length)
                        {
                            length = input.Length;
                        }
                        else
                        {
                            length = added.Length;
                        }

                        //Don't use copyto as we actually want to copy a trimmed array out (left aligned)
                        for (var i = 0; i < length; i++)
                        {
                            input[i] = added[i];
                        }

                        if (isInputBigEndian)
                        {
                            Array.Reverse(input);
                        }

                        return input;
                    }
            }

            //return null;
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
    }
}
