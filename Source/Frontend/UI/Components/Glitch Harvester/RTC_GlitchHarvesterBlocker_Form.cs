using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_GlitchHarvesterBlocker_Form : Form, IAutoColorize
	{
		public RTC_GlitchHarvesterBlocker_Form()
		{
			InitializeComponent();
		}

        private void BtnEmergencySave_Click(object sender, EventArgs e)
        {
            S.GET<RTC_StockpileManager_Form>().btnSaveStockpileAs_Click(null, null);
        }
    }
}
