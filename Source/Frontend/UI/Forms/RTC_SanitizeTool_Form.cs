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
	public partial class RTC_SanitizeTool_Form : Form, IAutoColorize
	{

        public BlastLayer originalBlastLayer = null;


		public RTC_SanitizeTool_Form()
		{
			try
			{
				InitializeComponent();

			}
			catch(Exception ex)
			{
				string additionalInfo = "An error occurred while opening the SanitizeTool Form\n\n";

				var ex2 = new CustomException(ex.Message, additionalInfo + ex.StackTrace);

				if (CloudDebug.ShowErrorDialog(ex2, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

			}
		}

        public static void OpenSanitizeTool(BlastLayer bl = null)
        {
            S.GET<RTC_SanitizeTool_Form>().Close();
            var stf = new RTC_SanitizeTool_Form();
            S.SET(stf);

            if (bl == null)
                return;

            BlastLayer clone = (BlastLayer)bl.Clone();

            stf.lbOriginalLayerSize.Text = $"Original Layer size: {clone.Layer.Count}"; 
            stf.lbCurrentLayerSize.Text = $"Current Layer size: {clone.Layer.Count}"; 

            stf.lbSteps.DisplayMember = "Text";
            stf.lbSteps.ValueMember = "Value";
            stf.lbSteps.Items.Add(new { Text = $"Original Layer [{clone.Layer.Count} Units]", Value = clone });

            stf.originalBlastLayer = clone;
            stf.ShowDialog();
        }

        private void RTC_NewBlastEditorForm_Load(object sender, EventArgs e)
		{
			UICore.SetRTCColor(UICore.GeneralColor, this);

		}

        private void btnYesEffect_Click(object sender, EventArgs e)
        {
            pnBlastLayerSanitization.Visible = false;

            S.GET<RTC_NewBlastEditor_Form>().btnRemoveDisabled_Click(null, null);

            S.GET<RTC_NewBlastEditor_Form>().dgvBlastEditor.ClearSelection();
            S.GET<RTC_NewBlastEditor_Form>().btnDisable50_Click(null, null);
            S.GET<RTC_NewBlastEditor_Form>().btnLoadCorrupt_Click(null, null);

            BlastLayer bl = (BlastLayer)S.GET<RTC_NewBlastEditor_Form>().currentSK.BlastLayer.Clone();
            lbSteps.Items.Add(new { Text = $"[{bl.Layer.Count} Units]", Value = bl });

            lbCurrentLayerSize.Text = $"Current Layer size: {bl.Layer.Count}";

            if(bl.Layer.Count == 1)
            {
                lbSanitizationText.Text = "1 Unit remaining, sanitization complete.";
                btnYesEffect.Visible = false;
                btnNoEffect.Visible = false;
            }

            pnBlastLayerSanitization.Visible = true;
        }

        private void btnNoEffect_Click(object sender, EventArgs e)
        {
            pnBlastLayerSanitization.Visible = false;

            S.GET<RTC_NewBlastEditor_Form>().btnInvertDisabled_Click(null, null);
            S.GET<RTC_NewBlastEditor_Form>().btnRemoveDisabled_Click(null, null);

            RunSanitizeAlgo();

            BlastLayer bl = (BlastLayer)S.GET<RTC_NewBlastEditor_Form>().currentSK.BlastLayer.Clone();
            lbSteps.Items.Add(new { Text = $"[{bl.Layer.Count} Units]", Value = bl });

            lbCurrentLayerSize.Text = $"Current Layer size: {bl.Layer.Count}";

            if (bl.Layer.Count == 1)
            {
                lbSanitizationText.Text = "1 Unit remaining, sanitization complete.";
                btnYesEffect.Visible = false;
                btnNoEffect.Visible = false;
            }

            pnBlastLayerSanitization.Visible = true;
        }

        private void btnReplayLast_Click(object sender, EventArgs e)
        {
            pnBlastLayerSanitization.Visible = false;

            S.GET<RTC_NewBlastEditor_Form>().btnLoadCorrupt_Click(null, null);

            BlastLayer changes = (BlastLayer)S.GET<RTC_NewBlastEditor_Form>().currentSK.BlastLayer.Clone();

            pnBlastLayerSanitization.Visible = true;
        }

        private void btnLeaveWithChanges_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLeaveSubstractChanges_Click(object sender, EventArgs e)
        {
            BlastLayer changes = (BlastLayer)S.GET<RTC_NewBlastEditor_Form>().currentSK.BlastLayer.Clone();
            BlastLayer modified = (BlastLayer)originalBlastLayer.Clone();

            foreach (var unit in changes.Layer)
            {
                var TargetUnit = modified.Layer.FirstOrDefault(it =>
                it.Address == unit.Address &&
                it.Domain == unit.Domain &&
                it.ExecuteFrame == unit.ExecuteFrame &&
                it.GeneratedUsingValueList == unit.GeneratedUsingValueList &&
                it.InvertLimiter == unit.InvertLimiter &&
                it.SourceAddress == unit.SourceAddress &&
                it.SourceDomain == unit.SourceDomain &&
                it.StoreLimiterSource == unit.StoreLimiterSource &&
                it.StoreTime == unit.StoreTime &&
                it.StoreType == unit.StoreType &&
                it.TiltValue == unit.TiltValue &&
                it.ValueString == unit.ValueString 
                );


                if (TargetUnit != null)
                    modified.Layer.Remove(TargetUnit);
            }

            S.GET<RTC_NewBlastEditor_Form>().LoadBlastlayer(modified);

            this.Close();
        }

        private void btnLeaveWithoutChanges_Click(object sender, EventArgs e)
        {
            S.GET<RTC_NewBlastEditor_Form>().LoadBlastlayer(originalBlastLayer);
            this.Close();
        }

        private void btnBackPrevState_Click(object sender, EventArgs e)
        {
            var lastItem = lbSteps.Items[lbSteps.Items.Count -1];

            if(lbSteps.Items.Count > 1)
                lastItem = lbSteps.Items[lbSteps.Items.Count - 2];

            T Cast<T>(object obj, T type) { return (T)obj; }
            var modified = Cast(lastItem, new { Text = "", Value = new BlastLayer() }); ;

            BlastLayer bl = (BlastLayer)modified.Value.Clone();
            S.GET<RTC_NewBlastEditor_Form>().LoadBlastlayer(bl);

            lbCurrentLayerSize.Text = $"Current Layer size: {bl.Layer.Count}";

            if(lbSteps.Items.Count>1)
                lbSteps.Items.RemoveAt(lbSteps.Items.Count - 1);

            S.GET<RTC_NewBlastEditor_Form>().btnLoadCorrupt_Click(null, null);

            lbSanitizationText.Text = "Is the effect you are looking for still present?";
            btnYesEffect.Visible = true;
            btnNoEffect.Visible = true;

            if(lbSteps.Items.Count == 1)
            {
                pnBlastLayerSanitization.Visible = false;
                btnStartSanitizing.Visible = true;
            }

        }

        private void btnStartSanitizing_Click(object sender, EventArgs e)
        {
            btnStartSanitizing.Visible = false;

            S.GET<RTC_NewBlastEditor_Form>().dgvBlastEditor.ClearSelection();
            S.GET<RTC_NewBlastEditor_Form>().btnDisable50_Click(null, null);
            S.GET<RTC_NewBlastEditor_Form>().btnLoadCorrupt_Click(null, null);

            pnBlastLayerSanitization.Visible = true;
        }

        public void RunSanitizeAlgo()
        {
            S.GET<RTC_NewBlastEditor_Form>().dgvBlastEditor.ClearSelection();
            S.GET<RTC_NewBlastEditor_Form>().btnDisable50_Click(null, null);
            S.GET<RTC_NewBlastEditor_Form>().btnLoadCorrupt_Click(null, null);
        }

        private void lbSteps_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbSteps.SelectedIndex = -1;
        }
    }
}
