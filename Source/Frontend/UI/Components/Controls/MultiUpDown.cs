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
	public partial class MultiUpDown : NumericUpDown
	{
		private bool GeneralUpdateFlag = false; //makes other events ignore firing

		private Timer updater;
		private int updateThreshold = 250;
		private bool FirstLoadDone = false;


		private List<MultiUpDown> slaveComps = new List<MultiUpDown>();
		private MultiUpDown _parent = null;


		public MultiUpDown()
		{
			updater = new Timer();
			updater.Interval = updateThreshold;
			updater.Tick += Updater_Tick;
			this.ValueChanged += nmControlValue_ValueChanged;
		}

		private void Updater_Tick(object sender, EventArgs e)
		{
			updater.Stop();
		}

		public void registerSlave(MultiUpDown comp)
		{
			//Sync the slave's settings
			comp.Value = this.Value;
			comp.Minimum = this.Minimum;
			comp.Maximum = this.Maximum;
			slaveComps.Add(comp);
			comp._parent = this;
		}

		private void UpdateAllControls(decimal value, Control setter, bool ignore = false)
		{
			GeneralUpdateFlag = true;
			if (setter != this || ignore)
			{
				this.Value = value;

				foreach (var slave in slaveComps)
					slave.UpdateAllControls(value, this);

				if (_parent != null)
					_parent.UpdateAllControls(value, setter);

			}

			GeneralUpdateFlag = false;
		}

		private void PropagateValue(decimal value, Control setter)
		{
			UpdateAllControls(value, setter, true);

			updater.Stop();
			updater.Start();
		}

		private void nmControlValue_ValueChanged(object sender, EventArgs e)
		{
			if (GeneralUpdateFlag)
				return;
			PropagateValue(this.Value, this);
		}

	}

}
