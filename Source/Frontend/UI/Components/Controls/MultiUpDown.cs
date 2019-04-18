using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.UI.Components.Controls;

namespace RTCV.UI.Components.Controls
{

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
			set => updown.Minimum = value;
		}
		[Description("The maximum value of the NumericUpDown"), Category("Data")]
		public decimal Maximum
		{
			get => updown.Maximum;
			set => updown.Maximum = value;
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
                updown.Value = value;

				foreach (var slave in slaveComps)
					slave.UpdateAllControls(value, this);

				if (_parent != null)
					_parent.UpdateAllControls(value, setter);

			}

			GeneralUpdateFlag = false;
        }

        public void registerSlave(MultiUpDown comp)
		{
			comp.Minimum = this.Minimum;
			comp.Maximum = this.Maximum;
			base.registerSlave(comp);
		}

		private void updown_ValueChanged(object sender, EventArgs e)
		{
			if (GeneralUpdateFlag)
				return;

			PropagateValue(updown.Value, updown);
		}


	}
}