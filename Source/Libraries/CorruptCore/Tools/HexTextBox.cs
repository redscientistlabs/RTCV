using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.CorruptCore.Tools
{
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
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
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
        /// <summary>
        /// Gets the string before a specific string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetPrecedingString(this string str, string value)
        {
            var index = str.IndexOf(value);

            if (index < 0)
            {
                return null;
            }

            if (index == 0)
            {
                return "";
            }

            return str.Substring(0, index);
        }

        public static bool IsValidRomExtentsion(this string str, params string[] romExtensions)
        {
            var strUpper = str.ToUpper();
            return romExtensions.Any(ext => strUpper.EndsWith(ext.ToUpper()));
        }

        public static bool In(this string str, params string[] options)
        {
            return options.Any(opt => opt.Equals(str, StringComparison.CurrentCultureIgnoreCase));
        }

        public static bool In(this string str, IEnumerable<string> options)
        {
            return options.Any(opt => opt.Equals(str, StringComparison.CurrentCultureIgnoreCase));
        }

        public static bool In<T>(this string str, IEnumerable<T> options, Func<T, string, bool> eval)
        {
            return options.Any(opt => eval(opt, str));
        }

        public static bool NotIn(this string str, params string[] options)
        {
            return options.All(opt => opt.ToLower() != str.ToLower());
        }

        public static bool NotIn(this string str, IEnumerable<string> options)
        {
            return options.All(opt => opt.ToLower() != str.ToLower());
        }

        public static int HowMany(this string str, char c)
        {
            return !string.IsNullOrEmpty(str) ? str.Count(t => t == c) : 0;
        }

        public static int HowMany(this string str, string s)
        {
            if (str == null)
            {
                return 0;
            }

            var count = 0;
            for (var i = 0; i < (str.Length - s.Length); i++)
            {
                if (str.Substring(i, s.Length) == s)
                {
                    count++;
                }
            }

            return count;
        }

        #region String and Char validation extensions

        /// <summary>
        /// Validates all chars are 0-9
        /// </summary>
        public static bool IsUnsigned(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            return str.All(IsUnsigned);
        }

        /// <summary>
        /// Validates the char is 0-9
        /// </summary>
        public static bool IsUnsigned(this char c)
        {
            return char.IsDigit(c);
        }

        /// <summary>
        /// Validates all chars are 0-9, or a dash as the first value
        /// </summary>
        public static bool IsSigned(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            return str[0].IsSigned() && str.Substring(1).All(IsUnsigned);
        }

        /// <summary>
        /// Validates the char is 0-9 or a dash
        /// </summary>
        public static bool IsSigned(this char c)
        {
            return char.IsDigit(c) || c == '-';
        }

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

        /// <summary>
        /// Validates all chars are 0 or 1
        /// </summary>
        public static bool IsBinary(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            return str.All(IsBinary);
        }

        /// <summary>
        /// Validates the char is 0 or 1
        /// </summary>
        public static bool IsBinary(this char c)
        {
            return c == '0' || c == '1';
        }

        /// <summary>
        /// Validates all chars are 0-9, a decimal point, and that there is no more than 1 decimal point, can not be signed
        /// </summary>
        public static bool IsFixedPoint(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            return str.HowMany('.') <= 1
                && str.All(IsFixedPoint);
        }

        /// <summary>
        /// Validates the char is 0-9, a dash, or a decimal
        /// </summary>
        public static bool IsFixedPoint(this char c)
        {
            return c.IsUnsigned() || c == '.';
        }

        /// <summary>
        /// Validates all chars are 0-9 or decimal, and that there is no more than 1 decimal point, a dash can be the first character
        /// </summary>
        public static bool IsFloat(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            return str.HowMany('.') <= 1
                && str[0].IsFloat()
                && str.Substring(1).All(IsFixedPoint);
        }

        /// <summary>
        /// Validates that the char is 0-9, a dash, or a decimal point
        /// </summary>
        public static bool IsFloat(this char c)
        {
            return c.IsFixedPoint() || c == '-';
        }

        /// <summary>
        /// Takes any string and removes any value that is not a valid binary value (0 or 1)
        /// </summary>
        public static string OnlyBinary(this string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return "";
            }

            var output = new StringBuilder();

            foreach (var chr in raw)
            {
                if (IsBinary(chr))
                {
                    output.Append(chr);
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Takes any string and removes any value that is not a valid unsigned integer value (0-9)
        /// </summary>
        public static string OnlyUnsigned(this string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return "";
            }

            var output = new StringBuilder();

            foreach (var chr in raw)
            {
                if (IsUnsigned(chr))
                {
                    output.Append(chr);
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Takes any string and removes any value that is not a valid unsigned integer value (0-9 or -)
        /// Note: a "-" will only be kept if it is the first digit
        /// </summary>
        public static string OnlySigned(this string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return "";
            }

            var output = new StringBuilder();

            int count = 0;
            foreach (var chr in raw)
            {
                if (count == 0 && chr == '-')
                {
                    output.Append(chr);
                }
                else if (IsUnsigned(chr))
                {
                    output.Append(chr);
                }

                count++;
            }

            return output.ToString();
        }

        /// <summary>
        /// Takes any string and removes any value that is not a valid hex value (0-9, a-f, A-F), returns the remaining characters in uppercase
        /// </summary>
        public static string OnlyHex(this string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return "";
            }

            var output = new StringBuilder(raw.Length);

            foreach (var chr in raw)
            {
                if (IsHex(chr))
                {
                    output.Append(char.ToUpper(chr));
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Takes any string and removes any value that is not a fixed point value (0-9 or .)
        /// Note: only the first occurrence of a . will be kept
        /// </summary>
        public static string OnlyFixedPoint(this string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return "";
            }

            var output = new StringBuilder();

            var usedDot = false;
            foreach (var chr in raw)
            {
                if (chr == '.')
                {
                    if (usedDot)
                    {
                        continue;
                    }

                    usedDot = true;
                }

                if (IsFixedPoint(chr))
                {
                    output.Append(chr);
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Takes any string and removes any value that is not a float point value (0-9, -, or .)
        /// Note: - is only valid as the first character, and only the first occurrence of a . will be kept
        /// </summary>
        public static string OnlyFloat(this string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return "";
            }

            var output = new StringBuilder();

            var usedDot = false;
            var count = 0;
            foreach (var chr in raw)
            {
                if (count == 0 && chr == '-')
                {
                    output.Append(chr);
                }
                else
                {
                    if (chr == '.')
                    {
                        if (usedDot)
                        {
                            continue;
                        }

                        usedDot = true;
                    }

                    if (IsFixedPoint(chr))
                    {
                        output.Append(chr);
                    }
                }

                count++;
            }

            return output.ToString();
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

        public bool Nullable { get { return _nullable; } set { _nullable = value; } }

        public void SetHexProperties(long domainSize)
        {
            bool wasMaxSizeSet = _maxSize.HasValue;
            int currMaxLength = MaxLength;

            _maxSize = domainSize - 1;

            MaxLength = _maxSize.Value.NumHexDigits();
            _addressFormatStr = "{0:X" + MaxLength + "}";

            //try to preserve the old value, as best we can
            if (!wasMaxSizeSet)
                ResetText();
            else if (_nullable)
                Text = "";
            else if (MaxLength != currMaxLength)
            {
                long? value = ToLong();
                if (value.HasValue)
                    value = value.Value & ((1L << (MaxLength * 4)) - 1);
                else value = 0;
                Text = String.Format(_addressFormatStr, value.Value);
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
            Text = _nullable ? "" : String.Format(_addressFormatStr, 0);
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
                if (Text.IsHex() && !String.IsNullOrEmpty(_addressFormatStr))
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

                    Text = String.Format(_addressFormatStr, val);
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (Text.IsHex() && !String.IsNullOrEmpty(_addressFormatStr))
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

                    Text = String.Format(_addressFormatStr, val);
                }
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Text))
            {
                ResetText();
                SelectAll();
                return;
            }

            base.OnTextChanged(e);
        }

        public int? ToRawInt()
        {
            if (String.IsNullOrWhiteSpace(Text))
            {
                if (Nullable)
                {
                    return null;
                }

                return 0;
            }

            return Int32.Parse(Text, NumberStyles.HexNumber);
        }

        public void SetFromRawInt(int? val)
        {
            Text = val.HasValue ? String.Format(_addressFormatStr, val) : "";
        }

        public void SetFromLong(long val)
        {
            Text = String.Format(_addressFormatStr, val);
        }

        public long? ToLong()
        {
            if (String.IsNullOrWhiteSpace(Text))
            {
                if (Nullable)
                {
                    return null;
                }

                return 0;
            }

            return Int64.Parse(Text, NumberStyles.HexNumber);
        }
    }
}
