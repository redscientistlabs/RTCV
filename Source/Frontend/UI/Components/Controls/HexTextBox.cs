namespace RTCV.UI.Components.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    public interface INumberBox
    {
        bool Nullable { get; }
        int? ToRawInt();
        void SetFromRawInt(int? rawint);
    }

    //From Bizhawk
    public static class NumberExtensions
    {
        public static string ToHexString(this int n, int numdigits)
        {
            return string.Format("{0:X" + numdigits + "}", n);
        }

        public static string ToHexString(this uint n, int numdigits)
        {
            return string.Format("{0:X" + numdigits + "}", n);
        }

        public static string ToHexString(this byte n, int numdigits)
        {
            return string.Format("{0:X" + numdigits + "}", n);
        }

        public static string ToHexString(this ushort n, int numdigits)
        {
            return string.Format("{0:X" + numdigits + "}", n);
        }

        public static string ToHexString(this long n, int numdigits)
        {
            return string.Format("{0:X" + numdigits + "}", n);
        }

        public static string ToHexString(this long n)
        {
            return $"{n:X}";
        }

        public static string ToHexString(this ulong n, int numdigits)
        {
            return string.Format("{0:X" + numdigits + "}", n);
        }

        public static bool Bit(this byte b, int index)
        {
            return (b & (1 << index)) != 0;
        }

        public static bool Bit(this int b, int index)
        {
            return (b & (1 << index)) != 0;
        }

        public static bool Bit(this ushort b, int index)
        {
            return (b & (1 << index)) != 0;
        }

        public static bool In(this int i, params int[] options)
        {
            return options.Any(j => i == j);
        }

        public static byte BinToBCD(this byte v)
        {
            return (byte)(((v / 10) * 16) + (v % 10));
        }

        public static byte BCDtoBin(this byte v)
        {
            return (byte)(((v / 16) * 10) + (v % 16));
        }

        /// <summary>
        /// Receives a number and returns the number of hexadecimal digits it is
        /// Note: currently only returns 2, 4, 6, or 8
        /// </summary>
        public static int NumHexDigits(this long i)
        {
            // now this is a bit of a trick. if it was less than 0, it mustve been >= 0x80000000 and so takes all 8 digits
            if (i < 0)
            {
                return 8;
            }

            if (i < 0x100)
            {
                return 2;
            }

            if (i < 0x10000)
            {
                return 4;
            }

            if (i < 0x1000000)
            {
                return 6;
            }

            if (i < 0x100000000)
            {
                return 8;
            }

            return 16;
        }

        /// <summary>
        /// The % operator is a remainder operator. (e.g. -1 mod 4 returns -1, not 3.)
        /// </summary>
        public static int Mod(this int a, int b)
        {
            return a - (b * (int)Math.Floor((float)a / b));
        }

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

    public static class StringExtensions
    {
        #region String and Char validation extensions

        /// <summary>
        /// Validates all chars are 0-9, A-F or a-f
        /// </summary>
        public static bool IsHex(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            return str.All(IsHex);
        }

        /// <summary>
        /// Validates the char is 0-9, A-F or a-f
        /// </summary>
        public static bool IsHex(this char c)
        {
            if (char.IsDigit(c))
            {
                return true;
            }

            return char.ToUpperInvariant(c) >= 'A' && char.ToUpperInvariant(c) <= 'F';
        }

        #endregion
    }

    public class HexTextBox : TextBox, INumberBox
    {
        private string _addressFormatStr = "";
        private long? _maxSize;
        private bool _nullable = true;

        public HexTextBox()
        {
            CharacterCasing = CharacterCasing.Upper;
        }

        public bool Nullable { get => _nullable; set => _nullable = value; }

        public void SetHexProperties(long domainSize)
        {
            bool wasMaxSizeSet = _maxSize.HasValue;
            int currMaxLength = MaxLength;

            _maxSize = domainSize - 1;

            MaxLength = _maxSize.Value.NumHexDigits();
            _addressFormatStr = "{0:X" + MaxLength + "}";

            //try to preserve the old value, as best we can
            if (!wasMaxSizeSet)
            {
                ResetText();
            }
            else if (_nullable)
            {
                Text = "";
            }
            else if (MaxLength != currMaxLength)
            {
                long? value = ToLong();
                if (value.HasValue)
                {
                    value = value.Value & ((1L << (MaxLength * 4)) - 1);
                }
                else
                {
                    value = 0;
                }

                Text = string.Format(_addressFormatStr, value.Value);
            }
        }

        public long GetMax()
        {
            if (_maxSize.HasValue)
            {
                return _maxSize.Value;
            }

            return ((long)1 << (4 * MaxLength)) - 1;
        }

        public override void ResetText()
        {
            Text = _nullable ? "" : string.Format(_addressFormatStr, 0);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == 22 || e.KeyChar == 1 || e.KeyChar == 3)
            {
                return;
            }

            if (!e.KeyChar.IsHex())
            {
                e.Handled = true;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (Text.IsHex() && !string.IsNullOrEmpty(_addressFormatStr))
                {
                    var val = (uint)ToRawInt();

                    if (val == GetMax())
                    {
                        val = 0;
                    }
                    else
                    {
                        val++;
                    }

                    Text = string.Format(_addressFormatStr, val);
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (Text.IsHex() && !string.IsNullOrEmpty(_addressFormatStr))
                {
                    var val = (uint)ToRawInt();
                    if (val == 0)
                    {
                        val = (uint)GetMax(); // int to long todo
                    }
                    else
                    {
                        val--;
                    }

                    Text = string.Format(_addressFormatStr, val);
                }
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                ResetText();
                SelectAll();
                return;
            }

            base.OnTextChanged(e);
        }

        public int? ToRawInt()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                if (Nullable)
                {
                    return null;
                }

                return 0;
            }

            return int.Parse(Text, NumberStyles.HexNumber);
        }

        public void SetFromRawInt(int? val)
        {
            Text = val.HasValue ? string.Format(_addressFormatStr, val) : "";
        }

        public void SetFromLong(long val)
        {
            Text = string.Format(_addressFormatStr, val);
        }

        public long? ToLong()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                if (Nullable)
                {
                    return null;
                }

                return 0;
            }

            return long.Parse(Text, NumberStyles.HexNumber);
        }
    }
}
