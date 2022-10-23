namespace RTCV.UI.Components
{
    using System;
    using System.Globalization;
    using System.Windows.Forms;

    // http://msdn.microsoft.com/en-us/library/ms229644%28v=vs.80%29.aspx
    public class NumericTextBox : TextBox
    {
        // Restricts the entry of characters to digits (including hex), the negative sign,
        // the decimal point, and editing keystrokes (backspace).
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            base.OnKeyPress(e);

            var numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
            var decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            var groupSeparator = numberFormatInfo.NumberGroupSeparator;
            var negativeSign = numberFormatInfo.NegativeSign;

            var keyInput = e.KeyChar.ToString();

            if (char.IsDigit(e.KeyChar))
            {
                // Digits are OK
            }
            else if (keyInput.Equals(decimalSeparator) && AllowDecimal)
            {
                // Decimal separator is OK
            }
            else if (keyInput.Equals(negativeSign) && AllowNegative)
            {
                // Negative is OK
            }
            else if (keyInput.Equals(groupSeparator))
            {
                // group seperator is ok
            }
            else if (e.KeyChar == '\b')
            {
                // Backspace key is OK
            }
            //    else if ((ModifierKeys & (Keys.Control | Keys.Alt)) != 0)
            //    {
            //     // Let the edit control handle control and alt key combinations
            //    }
            else if (AllowSpace && e.KeyChar == ' ')
            {
            }
            else
            {
                // Swallow this invalid key and beep
                e.Handled = true;
                //    MessageBeep();
            }
        }

        public int IntValue => int.Parse(Text);

        public decimal DecimalValue => decimal.Parse(Text);

        public bool AllowSpace { get; set; }

        public bool AllowDecimal { get; set; }

        public bool AllowNegative { get; set; }
    }
}
