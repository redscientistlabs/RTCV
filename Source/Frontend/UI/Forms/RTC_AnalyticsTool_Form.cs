using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Serialization;
using RTCV.CorruptCore;
using RTCV.NetCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;


namespace RTCV.UI
{
    public partial class RTC_AnalyticsTool_Form : Form, IAutoColorize
    {

        public BlastLayer originalBlastLayer = null;


        public RTC_AnalyticsTool_Form()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {
                string additionalInfo = "An error occurred while opening the SanitizeTool Form\n\n";

                var ex2 = new CustomException(ex.Message, additionalInfo + ex.StackTrace);

                if (CloudDebug.ShowErrorDialog(ex2, true) == DialogResult.Abort)
                    throw new RTCV.NetCore.AbortEverythingException();

            }
        }

        public static void OpenAnalyticsTool(BlastLayer bl = null)
        {
            S.GET<RTC_AnalyticsTool_Form>().Close();
            var stf = new RTC_AnalyticsTool_Form();
            S.SET(stf);

            if (bl == null)
                return;

            if (bl.Layer.Count == 0)
            {
                MessageBox.Show("Sanitize Tool cannot sanitize BlastLayers that don't have any units.");
                return;
            }

            if (bl.Layer.Count == 1)
            {
                MessageBox.Show("Sanitize Tool cannot sanitize BlastLayers that only have one unit.");
                return;
            }

            BlastLayer clone = (BlastLayer)bl.Clone();

            stf.lbDumps.DisplayMember = "Text";
            stf.lbDumps.ValueMember = "Value";
            stf.lbDumps.Items.Add(new { Text = $"Original Layer [{clone.Layer.Count} Units]", Value = clone });

            stf.originalBlastLayer = clone;

            stf.ShowDialog();
        }

        private void RTC_AnalyticsToolForm_Load(object sender, EventArgs e)
        {
            UICore.SetRTCColor(UICore.GeneralColor, this);

        }

        private void RTC_SanitizeTool_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            Form frm = (sender as Form);
            Button check = (frm?.ActiveControl as Button);

            if (check == null && lbDumps.Items.Count > 1)
            {

            }

        }
    }
}
