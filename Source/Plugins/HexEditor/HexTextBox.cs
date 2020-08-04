namespace RTCV.Plugins.HexEditor
{
    using System;
    using System.Globalization;
    using System.Windows.Forms;
    using RTCV.Common.CustomExtensions;

    public interface INumberBox
    {
        bool Nullable { get; }
        int? ToRawInt();
        void SetFromRawInt(int? rawint);
    }

    public class HexTextBox : TextBox, INumberBox
    {
        private readonly string _addressFormatStr = "";
        private readonly long? _maxSize;
        private bool _nullable = true;

        public HexTextBox()
        {
            CharacterCasing = CharacterCasing.Upper;
        }

        public bool Nullable { get => _nullable; set => _nullable = value; }
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
