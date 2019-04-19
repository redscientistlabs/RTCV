using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;
using RTCV.CorruptCore;
using RTCV.NetCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;
using System.Linq;

namespace RTCV.UI
{
	public partial class RTC_GlitchHarvester_Form : Form, IAutoColorize
	{


		public RTC_GlitchHarvester_Form()
		{
			InitializeComponent();

		}


		private void RTC_GH_Form_Load(object sender, EventArgs e)
		{
			/*
			foreach (Control ctrl in pnSavestateHolder.Controls)
				if (ctrl is Button)
					ctrl.Size = new Size(29, 25);
            */




		}




		private void RTC_GH_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				//S.GET<RTC_Core_Form>().btnGlitchHarvester.Text = S.GET<RTC_Core_Form>().btnGlitchHarvester.Text.Replace("○ ", "");
                S.GET<UI_CoreForm>().pnGlitchHarvesterOpen.Visible = false;
                e.Cancel = true;
				this.Hide();
			}
		}







    }
}
