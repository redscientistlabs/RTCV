using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using RTCV.CorruptCore;
using System.Linq;
using System.Threading.Tasks;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;
using RTCV.NetCore;

namespace RTCV.UI
{
	public partial class RTC_SimpleMode_Form : ComponentForm, IAutoColorize
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public bool DontLoadSelectedStockpile = false;
		private bool currentlyLoading = false;

		public RTC_SimpleMode_Form()
		{
			InitializeComponent();
		}

		private void RTC_SimpleMode_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				e.Cancel = true;
				this.Hide();
			}
		}

		private void btnBlastToggle_Click(object sender, EventArgs e)
		{
			S.GET<RTC_GlitchHarvesterBlast_Form>().btnBlastToggle_Click(null, null);
		}

    }
}
