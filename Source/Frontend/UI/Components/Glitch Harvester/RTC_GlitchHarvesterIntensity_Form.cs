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
using RTCV.NetCore;

namespace RTCV.UI
{
    public partial class RTC_GlitchHarvesterIntensity_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);



        public RTC_GlitchHarvesterIntensity_Form()
        {
            InitializeComponent();
            popoutAllowed = true;

            multiTB_Intensity.ValueChanged += (sender, args) => CorruptCore.RtcCore.Intensity = multiTB_Intensity.Value;

        }

        private void RTC_GlitchHarvesterIntensity_Form_Shown(object sender, EventArgs e)
        {
            object paramValue = AllSpec.VanguardSpec[VSPEC.OVERRIDE_DEFAULTMAXINTENSITY];

            if (paramValue != null && paramValue is int maxintensity)
            {
                var prevState = multiTB_Intensity.FirstLoadDone;
                multiTB_Intensity.FirstLoadDone = false;
                multiTB_Intensity.Maximum = maxintensity;
                multiTB_Intensity.FirstLoadDone = prevState;
            }

        }
    }
}
