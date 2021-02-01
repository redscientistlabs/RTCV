namespace RTCV.UI.Components.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class ValueUpdateEventArgs : EventArgs
    {
        private long _value;

        public ValueUpdateEventArgs(long value)
        {
            _value = value;
        }
    }

    #pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class MultiTrackBar : UserControl
    {
        public event EventHandler<ValueUpdateEventArgs> ValueChanged;
        public virtual void OnValueChanged(ValueUpdateEventArgs e) => ValueChanged?.Invoke(this, e);

        public event EventHandler<EventArgs> CheckChanged;
        public virtual void OnCheckChanged(object sender, EventArgs e) => CheckChanged?.Invoke(sender, e);

        private bool GeneralUpdateFlag = false; //makes other events ignore firing

        private long _Value;
        [Description("Net value of the control (displayed in numeric box)"), Category("Data")]
        public long Value
        {
            get => _Value;
            set
            {
                _Value = value;
                var tbValue = nmValueToTbValueQuadScale(value);
                if (!GeneralUpdateFlag)
                {
                    UpdateAllControls(value, tbValue, null);
                }
            }
        }

        [Description("Whether or not the NumericUpDown should use hex"), Category("Data")]
        public bool Hexadecimal
        {
            get => nmControlValue.Hexadecimal;
            set => nmControlValue.Hexadecimal = value;
        }

        private bool _DisplayCheckbox = false;
        [Description("Display a checkbox before the label"), Category("Data")]
        public bool DisplayCheckbox
        {
            get => _DisplayCheckbox;
            set
            {
                _DisplayCheckbox = value;
                if (value)
                {
                    lbControlName.Visible = false;
                    cbControlName.Visible = true;
                }
                else
                {
                    lbControlName.Visible = true;
                    cbControlName.Visible = false;
                }
            }
        }

        [Description("Value of the checkbox"), Category("Data")]
        public bool Checked
        {
            get => cbControlName.Checked;
            set => cbControlName.Checked = value;
        }

        private bool FirstLoadDone = false;

        private long _Minimum = 0;
        [Description("Minimum value of the control"), Category("Data")]
        public long Minimum
        {
            get => _Minimum;
            set
            {
                _Minimum = value;
                nmControlValue.Minimum = value;
                tbControlValue.Minimum = Convert.ToInt32(value);
                if (FirstLoadDone)
                {
                    tbControlValue_ValueChanged(null, null);
                }
            }
        }
        private long _Maximum = 65535;
        [Description("Maximum value of the control"), Category("Data")]
        public long Maximum
        {
            get => _Maximum;
            set => SetMaximum(value, true);
        }

        public void SetMaximum(long value, bool useChangeHandler = true)
        {
            _Maximum = value;
            if (FirstLoadDone && useChangeHandler)
            {
                tbControlValue_ValueChanged(null, null);
            }
        }

        private string name = "Name";
        [Description("Displayed label of the control"), Category("Data")]
        public string Label
        {
            get => name;
            set
            {
                name = value;
                lbControlName.Text = value;
                cbControlName.Text = value;
            }
        }

        [Description("Let the NumericBox override the maximum value"), Category("Data")]
        public bool UncapNumericBox { get; set; } = false;

        private Timer updater;
        private int updateThreshold = 250;

        private List<MultiTrackBar> slaveComps = new List<MultiTrackBar>();
        private MultiTrackBar _parent = null;

        private decimal A; //A value for quadratic function Y = AX² used to scale components

        public MultiTrackBar()
        {
            InitializeComponent();
            cbControlName.Location = lbControlName.Location;
            tbControlValue.MouseWheel += TbControlValue_MouseWheel;

            updater = new Timer
            {
                Interval = updateThreshold
            };
            updater.Tick += Updater_Tick;

            //Calculate A for quadratic function
            decimal max_X = Convert.ToDecimal(65536);
            decimal max_X_Squared = max_X * max_X;
            A = Maximum / max_X_Squared;
        }

        /*
        Y = AX²
        5000000 = A * 4294967296

        A = 4294967296
        A = (65536*65536)/Maximum
        */

        private long tbValueToNmValueQuadScale(long tbValue)
        {
            decimal max_X = Convert.ToDecimal(tbControlValue.Maximum);
            decimal max_X_Squared = max_X * max_X;
            decimal A = Maximum / max_X_Squared;

            decimal X = Convert.ToDecimal(tbValue);
            decimal Y = A * (X * X);

            decimal Floored_Y = Math.Floor(Y);
            return Convert.ToInt64(Floored_Y);
        }

        private int nmValueToTbValueQuadScale(decimal Y)
        {
            decimal max_X = Convert.ToDecimal(tbControlValue.Maximum);
            decimal max_X_Squared = max_X * max_X;
            decimal A = Maximum / max_X_Squared;

            decimal X = Maximum;

            if (A != 0) // fixes divisions by 0
            {
                X = DecSqrt(Y / A);
            }

            decimal Floored_X = Math.Floor(X);
            return Convert.ToInt32(Floored_X);
        }

        public static decimal DecSqrt(decimal x)
        {
            return (decimal)Math.Sqrt((double)x);
        }

        private void TbControlValue_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true; //disable default mouse wheel
            if (e.Delta > 0)
            {
                if (tbControlValue.Value + e.Delta <= tbControlValue.Maximum)
                {
                    tbControlValue.Value += e.Delta;
                }
            }
            else
            {
                //We're using a negative number so use +
                if (tbControlValue.Value + e.Delta >= tbControlValue.Minimum)
                {
                    tbControlValue.Value += e.Delta;
                }
            }
        }

        private void Updater_Tick(object sender, EventArgs e)
        {
            updater.Stop();
            OnValueChanged(new ValueUpdateEventArgs(Value));
        }

        public void registerSlave(MultiTrackBar comp)
        {
            slaveComps.Add(comp);
            comp._parent = this;
        }

        private void MultiTrackBar_Load(object sender, EventArgs e)
        {
            FirstLoadDone = true;

            if (DisplayCheckbox && !cbControlName.Checked)
            {
                nmControlValue.Enabled = false;
                tbControlValue.Enabled = false;
            }
        }

        private void UpdateAllControls(long nmValue, long tbValue, Control setter)
        {
            GeneralUpdateFlag = true;

            if (setter != this)
            {
                if (setter != tbControlValue)
                {
                    if (tbValue > 65536)
                    {
                        tbControlValue.Value = Convert.ToInt32(65536);
                    }
                    else if (tbValue < tbControlValue.Minimum)
                    {
                        tbControlValue.Value = Convert.ToInt32(Minimum);
                    }
                    else
                    {
                        tbControlValue.Value = Convert.ToInt32(tbValue);
                    }
                }

                if (setter != nmControlValue)
                {
                    if (nmValue > Maximum && !UncapNumericBox)
                    {
                        nmValue = Convert.ToInt32(Maximum);
                    }
                    else if (nmValue < Minimum)
                    {
                        nmValue = Convert.ToInt32(Minimum);
                    }

                    nmControlValue.Value = nmValue;
                }

                foreach (var slave in slaveComps)
                {
                    slave.UpdateAllControls(nmValue, tbValue, this);
                }

                if (_parent != null)
                {
                    _parent.UpdateAllControls(nmValue, tbValue, setter);
                }
            }

            GeneralUpdateFlag = false;
        }

        private void PropagateValue(long nmValue, long tbValue, Control setter)
        {
            UpdateAllControls(nmValue, tbValue, setter);

            if (nmValue > Maximum && !UncapNumericBox)
            {
                nmValue = Convert.ToInt32(Maximum);
            }

            if (nmValue < Minimum)
            {
                nmValue = Convert.ToInt32(Minimum);
            }

            Value = nmValue;
            updater.Stop();
            updater.Start();
        }

        private void tbControlValue_ValueChanged(object sender, EventArgs e)
        {
            if (GeneralUpdateFlag)
            {
                return;
            }

            int tbValue = tbControlValue.Value;
            long nmValue = tbValueToNmValueQuadScale(tbValue);
            if (nmValue < _Minimum)
            {
                nmValue = _Minimum;
            }

            PropagateValue(nmValue, tbValue, tbControlValue);
        }

        private void nmControlValue_ValueChanged(object sender, EventArgs e)
        {
            if (GeneralUpdateFlag)
            {
                return;
            }

            if (nmControlValue.Value > int.MaxValue)
            {
                GeneralUpdateFlag = true;
                nmControlValue.Value = int.MaxValue;
                GeneralUpdateFlag = false;
            }

            long nmValue = Convert.ToInt64(nmControlValue.Value);
            int tbValue = nmValueToTbValueQuadScale(nmControlValue.Value);

            PropagateValue(nmValue, tbValue, nmControlValue);
        }

        private void cbControlName_CheckedChanged(object sender, EventArgs e)
        {
            if (DisplayCheckbox)
            {
                nmControlValue.Enabled = cbControlName.Checked;
                tbControlValue.Enabled = cbControlName.Checked;
            }

            OnCheckChanged(sender, e);
        }

        private void nmControlValue_KeyUp(object sender, KeyEventArgs e)
        {
            if (GeneralUpdateFlag)
            {
                return;
            }

            long nmValue = Convert.ToInt64(nmControlValue.Value);
            int tbValue = nmValueToTbValueQuadScale(nmControlValue.Value);

            PropagateValue(nmValue, tbValue, nmControlValue);
        }

        private void tbControlValue_Scroll(object sender, EventArgs e)
        {
        }
    }

    internal class NoFocusTrackBar : TrackBar
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        private static int MakeParam(int loWord, int hiWord)
        {
            return (hiWord << 16) | (loWord & 0xffff);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            SendMessage(this.Handle, 0x0128, MakeParam(1, 0x1), 0);
        }
    }
}
