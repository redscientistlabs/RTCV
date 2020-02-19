namespace RTCV.UI.Components.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class MultiUpDown : SpecControl<decimal>
    {
        [Description("Whether or not the NumericUpDown should use hex"), Category("Data")]
        public bool Hexadecimal
        {
            get => updown.Hexadecimal;
            set => updown.Hexadecimal = value;
        }

        [Description("The minimum value of the NumericUpDown"), Category("Data")]
        public decimal Minimum
        {
            get => updown.Minimum;
            set
            {
                //If the minimum is going to change the current value, we need to mark initialized as false at the end
                bool reinit = value < updown.Value;
                updown.Minimum = value;
            }
        }

        [Description("The maximum value of the NumericUpDown"), Category("Data")]
        public decimal Maximum
        {
            get => updown.Maximum;
            set
            {
                //If the minimum is going to change the current value, we need to mark initialized as false at the end
                bool reinit = value > updown.Value;
                updown.Maximum = value;
            }
        }

        public MultiUpDown()
        {
            InitializeComponent();
            ForeColorChanged += (o, a) => updown.ForeColor = base.ForeColor.A == 255 ? base.ForeColor : Color.FromArgb(base.ForeColor.R, base.ForeColor.G, base.ForeColor.B);
            BackColorChanged += (o, a) => updown.BackColor = base.BackColor.A == 255 ? base.BackColor : Color.FromArgb(base.BackColor.R, base.BackColor.G, base.BackColor.B);

            updown.Tag = base.Tag;
            updown.ValueChanged += updown_ValueChanged;
        }

        internal override void UpdateAllControls(decimal value, Control setter, bool ignore = false)
        {
            GeneralUpdateFlag = true;
            if (setter != this || ignore)
            {
                if (value > updown.Maximum)
                {
                    value = updown.Maximum;
                }
                else if (value < updown.Minimum)
                {
                    value = updown.Minimum;
                }

                updown.Value = value;
                _Value = value;

                foreach (var slave in slaveComps)
                {
                    slave.UpdateAllControls(value, this);
                }

                if (_parent != null)
                {
                    _parent.UpdateAllControls(value, setter);
                }
            }

            GeneralUpdateFlag = false;
        }

        public void registerSlave(MultiUpDown comp, EventHandler<ValueUpdateEventArgs<decimal>> valueChangedHandler = null)
        {
            comp.Minimum = this.Minimum;
            comp.Maximum = this.Maximum;
            comp.Value = this.Value;

            if (valueChangedHandler != null)
            {
                comp.ValueChanged += valueChangedHandler;
            }

            base.registerSlave(comp);
        }

        private void updown_ValueChanged(object sender, EventArgs e)
        {
            if (GeneralUpdateFlag)
            {
                return;
            }

            PropagateValue(updown.Value, updown);
        }
    }
}
