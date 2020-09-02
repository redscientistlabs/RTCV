namespace RTCV.UI.Components.Controls
{
    using System;
    using System.Globalization;
    using System.Windows.Forms;
    using System.ComponentModel;
    using RTCV.Common.CustomExtensions;

    /// <summary>
    /// Provides an analog to NumericUpDown without all the overhead
    /// </summary>
    public class NumericTextBox : TextBox
    {
        private readonly string _addressFormatStr;

        [Category("Data")]
        [Description("Indicates the minimum value for the numeric up-down cells.")]
        [RefreshProperties(RefreshProperties.All)]
        public long Minimum
        {
            get;
            set;
        } = 0;

        [Category("Data")] [Description("Indicates the Maximum value for the numeric up-down cells.")] [RefreshProperties(RefreshProperties.All)]
        public long Maximum = long.MaxValue;

        [Category("Data")] [Description("How much to increment on scroll/arrow key")] [RefreshProperties(RefreshProperties.All)]
        public long Increment = 1;

        [Category("Data")] [Description("Indicates if the text box uses hexadecimal instead of decimal")] [RefreshProperties(RefreshProperties.All)]
        public bool Hexadecimal = false;

        public long Value
        {
            get => ToLong();
            set => SetFromLong(value);
        }

        public NumericTextBox()
        {
            CharacterCasing = CharacterCasing.Upper;
            //MaxLength = NumHexDigits(Maximum);
            if (Hexadecimal)
            {
                _addressFormatStr = "{0:X2}";
            }
            else
            {
                _addressFormatStr = "{0:N0}";
            }
        }

        public override void ResetText()
        {
            Text = string.Format(_addressFormatStr, 0);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || e.KeyChar == 22 || e.KeyChar == 1 || e.KeyChar == 3)
            {
                return;
            }

            if (Hexadecimal && !e.KeyChar.IsHex())
            {
                e.Handled = true;
            }

            if (!Hexadecimal && !e.KeyChar.IsDigit())
            {
                e.Handled = true;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!ClientRectangle.Contains(e.Location))
            {
                return;
            }

            var scrollBy = Increment;
            if (e.Delta < 0)
            {
                scrollBy = -1 * Increment;
            }

            var val = ToLong();

            if (val + scrollBy > Maximum)
            {
                val = Maximum;
            }
            else if (val + scrollBy < Minimum)
            {
                val = Minimum;
            }
            else
            {
                val += scrollBy;
            }

            Text = string.Format(_addressFormatStr, val);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (!string.IsNullOrEmpty(_addressFormatStr))
                {
                    var val = ToLong();

                    if (val + Increment <= Maximum)
                    {
                        val += Increment;
                    }
                    else
                    {
                        val = Maximum;
                    }

                    Text = string.Format(_addressFormatStr, val);
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (!string.IsNullOrEmpty(_addressFormatStr))
                {
                    var val = ToLong();

                    if (val - Increment >= Minimum)
                    {
                        val -= Increment;
                    }
                    else
                    {
                        val = Minimum;
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

        public void SetFromLong(long val)
        {
            if (val > Maximum)
            {
                Text = string.Format(_addressFormatStr, Maximum);
            }
            else if (val < Minimum)
            {
                Text = string.Format(_addressFormatStr, Minimum);
            }
            else
            {
                Text = string.Format(_addressFormatStr, val);
            }
        }

        public long ToLong()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                return 0;
            }

            var r = long.Parse(Text, Hexadecimal ? NumberStyles.HexNumber : NumberStyles.Integer);
            return r > Maximum ? Maximum : r < Minimum ? Minimum : r;
        }
    }
}
