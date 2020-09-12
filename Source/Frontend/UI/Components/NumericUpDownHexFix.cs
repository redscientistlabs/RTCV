namespace RTCV.UI.Components
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    //Fixes microsoft's numericupdown hex issues. Thanks microsoft
    public class NumericUpDownHexFix : NumericUpDown
    {
        private bool currentValueChanged = false;

        public NumericUpDownHexFix()
        {
            base.Minimum = 0;
            base.Maximum = ulong.MaxValue;
            this.ValueChanged += OnValueChanged;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new decimal Maximum
        {    // Doesn't serialize properly
            get => base.Maximum;
            set => base.Maximum = value;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            currentValueChanged = true;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            var hme = e as HandledMouseEventArgs;
            if (hme != null)
            {
                hme.Handled = true;
            }

            if (e.Delta > 0 && this.Value < this.Maximum)
            {
                this.Value += this.Increment;
            }
            else if (e.Delta < 0 && this.Value > this.Minimum)
            {
                this.Value -= this.Increment;
            }
        }

        protected override void UpdateEditText()
        {
            if (UserEdit)
            {
                if (base.Hexadecimal)
                {
                    HexParseEditText();
                }
                else
                {
                    ParseEditText();
                }
            }

            if (currentValueChanged || (!string.IsNullOrEmpty(Text) && !(Text.Length == 1 && Text == "-")))
            {
                currentValueChanged = false;
                ChangingText = true;
                Text = GetNumberText(Value);
                ChangingText = false;
            }
        }

        private string GetNumberText(decimal num)
        {
            string text;

            if (Hexadecimal)
            {
                text = ((ulong)num).ToString("X", CultureInfo.InvariantCulture);
                Debug.Assert(text == text.ToUpper(CultureInfo.InvariantCulture), "GetPreferredSize assumes hex digits to be uppercase.");
            }
            else
            {
                text = num.ToString((ThousandsSeparator ? "N" : "F") + DecimalPlaces.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
            }
            return text;
        }

        private void HexParseEditText()
        {
            try
            {
                if (!string.IsNullOrEmpty(Text) && !(Text.Length == 1 && Text == "-"))
                {
                    var val = Convert.ToDecimal(Convert.ToUInt64(base.Text, 16));

                    if (val > Maximum)
                    {
                        base.Text = string.Format("{0:X}", (uint)Maximum);
                        //    val = (uint)Maximum;
                    }

                    if (!string.IsNullOrEmpty(base.Text))
                    {
                        this.Value = val;
                    }
                }
            }
            catch { }
            finally
            {
                base.UserEdit = false;
            }
        }
    }
}
