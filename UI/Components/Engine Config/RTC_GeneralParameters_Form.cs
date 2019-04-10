using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_GeneralParameters_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);


		public RTC_GeneralParameters_Form()
		{
			InitializeComponent();
			multiTB_Intensity.ValueChanged += (sender, args) => CorruptCore.CorruptCore.Intensity = multiTB_Intensity.Value;
			multiTB_Intensity.registerSlave(S.GET<RTC_GlitchHarvester_Form>().multiTB_Intensity);

			multiTB_ErrorDelay.ValueChanged += (sender, args) => CorruptCore.CorruptCore.ErrorDelay = multiTB_ErrorDelay.Value;
		}

		private void RTC_GeneralParameters_Form_Load(object sender, EventArgs e)
		{
			cbBlastRadius.SelectedIndex = 0;
		}


		
		Guid? errorDelayToken = null;
		Guid? intensityToken = null;


		private void cbBlastRadius_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cbBlastRadius.SelectedItem.ToString())
			{
				case "SPREAD":
					CorruptCore.CorruptCore.Radius = BlastRadius.SPREAD;
					break;

				case "CHUNK":
					CorruptCore.CorruptCore.Radius = BlastRadius.CHUNK;
					break;

				case "BURST":
					CorruptCore.CorruptCore.Radius = BlastRadius.BURST;
					break;

				case "NORMALIZED":
					CorruptCore.CorruptCore.Radius = BlastRadius.NORMALIZED;
					break;

				case "PROPORTIONAL":
					CorruptCore.CorruptCore.Radius = BlastRadius.PROPORTIONAL;
					break;

				case "EVEN":
					CorruptCore.CorruptCore.Radius = BlastRadius.EVEN;
					break;
			}
		}



		private void RTC_GeneralParameters_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				e.Cancel = true;
				this.RestoreToPreviousPanel();
				return;
			}
		}

		private void nmErrorDelay_ValueChanged(object sender, KeyPressEventArgs e)
		{

		}

		private void nmErrorDelay_ValueChanged(object sender, KeyEventArgs e)
		{

		}

		private void nmIntensity_KeyDown(object sender, KeyEventArgs e)
		{

		}

		private void nmIntensity_KeyUp(object sender, KeyEventArgs e)
		{

		}

		private void track_Intensity_MouseUp(object sender, KeyPressEventArgs e)
		{

		}

		private void track_Intensity_MouseUp(object sender, MouseEventArgs e)
		{

		}
	}
}
