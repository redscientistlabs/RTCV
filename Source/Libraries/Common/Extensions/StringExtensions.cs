namespace RTCV.Common.CustomExtensions
{
    using System.Linq;
    using System.Text;

    public static class StringExtensions
    {
        #region String and Char validation extensions

        /// <summary>
        /// Validates the char is 0-9
        /// </summary>
        public static bool IsUnsigned(this char c)
        {
            return char.IsDigit(c);
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

        #endregion
    }
}