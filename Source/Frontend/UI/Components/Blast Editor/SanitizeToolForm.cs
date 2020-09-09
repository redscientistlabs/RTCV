namespace RTCV.UI
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;

    public partial class SanitizeToolForm : Form, IAutoColorize
    {
        private BlastLayer originalBlastLayer = null;
        private BlastLayer workBlastLayer = null;

        public SanitizeToolForm()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                if (CloudDebug.ShowErrorDialog(ex, true) == DialogResult.Abort)
                {
                    throw new AbortEverythingException();
                }
            }
        }

        public static void OpenSanitizeTool(BlastLayer bl = null, bool lockUI = true)
        {
            S.GET<SanitizeToolForm>().Close();
            var stf = new SanitizeToolForm();
            S.SET(stf);

            if (bl == null)
            {
                return;
            }

            if (!bl.Layer.Any(x => !x.IsLocked))
            {
                MessageBox.Show("Sanitize Tool cannot sanitize BlastLayers that don't have any units.");
                return;
            }

            if (bl.Layer.Count(x => !x.IsLocked) == 1)
            {
                MessageBox.Show("Sanitize Tool cannot sanitize BlastLayers that only have one unit.");
                return;
            }

            BlastLayer clone = (BlastLayer)bl.Clone();

            stf.lbOriginalLayerSize.Text = $"Original Layer size: {clone.Layer.Count(x => !x.IsLocked)}";


            stf.lbSteps.DisplayMember = "Text";
            stf.lbSteps.ValueMember = "Value";
            stf.lbSteps.Items.Add(new { Text = $"Original Layer [{clone.Layer.Count(x => !x.IsLocked)} Units]", Value = clone });

            stf.originalBlastLayer = clone;
            stf.workBlastLayer = bl;

            stf.UpdateSanitizeProgress();

            if (lockUI)
                stf.ShowDialog();
            else
                stf.Show();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            Colors.SetRTCColor(Colors.GeneralColor, this);
        }

        public void Reroll(object sender, EventArgs e)
        {
            pnBlastLayerSanitization.Visible = false;
            this.Refresh();

            S.GET<NewBlastEditorForm>().dgvBlastEditor.ClearSelection();
            S.GET<NewBlastEditorForm>().Disable50(null, null);
            S.GET<NewBlastEditorForm>().LoadCorrupt(null, null);

            BlastLayer bl = (BlastLayer)S.GET<NewBlastEditorForm>().currentSK.BlastLayer.Clone();

            UpdateSanitizeProgress();

            pnBlastLayerSanitization.Visible = true;
        }

        public void YesEffect(object sender, EventArgs e)
        {
            pnBlastLayerSanitization.Visible = false;
            this.Refresh();

            S.GET<NewBlastEditorForm>().RemoveDisabled(null, null);

            S.GET<NewBlastEditorForm>().dgvBlastEditor.ClearSelection();
            S.GET<NewBlastEditorForm>().Disable50(null, null);
            S.GET<NewBlastEditorForm>().LoadCorrupt(null, null);

            BlastLayer bl = (BlastLayer)S.GET<NewBlastEditorForm>().currentSK.BlastLayer.Clone();
            lbSteps.Items.Add(new { Text = $"[{bl.Layer.Count(x => !x.IsLocked)} Units]", Value = bl });

            UpdateSanitizeProgress();

            if (bl.Layer.Count(x => !x.IsLocked) == 1)
            {
                lbSanitizationText.Text = "1 Unit remaining, sanitization complete.";
                btnYesEffect.Visible = false;
                btnNoEffect.Visible = false;
                btnReroll.Visible = false;
            }

            pnBlastLayerSanitization.Visible = true;
        }

        public void NoEffect(object sender, EventArgs e)
        {
            pnBlastLayerSanitization.Visible = false;
            this.Refresh();

            S.GET<NewBlastEditorForm>().InvertDisabled(null, null);
            S.GET<NewBlastEditorForm>().RemoveDisabled(null, null);

            RunSanitizeAlgo();

            BlastLayer bl = (BlastLayer)S.GET<NewBlastEditorForm>().currentSK.BlastLayer.Clone();
            lbSteps.Items.Add(new { Text = $"[{bl.Layer.Count(x => !x.IsLocked)} Units]", Value = bl });

            UpdateSanitizeProgress();

            if (bl.Layer.Count(x => !x.IsLocked) == 1)
            {
                lbSanitizationText.Text = "1 Unit remaining, sanitization complete.";
                btnYesEffect.Visible = false;
                btnNoEffect.Visible = false;
                btnReroll.Visible = false;
            }

            pnBlastLayerSanitization.Visible = true;
        }

        private void ReplayCorruption(object sender, EventArgs e)
        {
            pnBlastLayerSanitization.Visible = false;
            this.Refresh();

            S.GET<NewBlastEditorForm>().LoadCorrupt(null, null);

            BlastLayer changes = (BlastLayer)S.GET<NewBlastEditorForm>().currentSK.BlastLayer.Clone();

            pnBlastLayerSanitization.Visible = true;
        }

        public void LeaveAndKeepChanges(object sender, EventArgs e)
        {
            ReopenBlastEditor();
            this.Close();
        }

        private static void ReopenBlastEditor()
        {
            var be = S.GET<NewBlastEditorForm>();
            be.RefreshAllNoteIcons();
            be.WindowState = FormWindowState.Minimized;
            be.Show();
            be.WindowState = FormWindowState.Normal;
            be.BringToFront();
        }

        public void LeaveAndSubtractChanges(object sender, EventArgs e)
        {
            BlastLayer changes = (BlastLayer)S.GET<NewBlastEditorForm>().currentSK.BlastLayer.Clone();
            BlastLayer modified = (BlastLayer)originalBlastLayer.Clone();

            foreach (var unit in changes.Layer.Where(it => it.IsEnabled))
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
                it.ValueString == unit.ValueString &&
                it.LoopTiming == unit.LoopTiming
                );

                if (TargetUnit != null && !TargetUnit.IsLocked)
                    modified.Layer.Remove(TargetUnit);

                foreach (var bu in modified.Layer)
                    bu.IsEnabled = true;
            }

            S.GET<NewBlastEditorForm>().LoadBlastlayer(modified);
            ReopenBlastEditor();
            this.Close();
        }

        private void LeaveWithoutChanges(object sender, EventArgs e)
        {
            S.GET<NewBlastEditorForm>().LoadBlastlayer(originalBlastLayer);
            ReopenBlastEditor();

            this.Close();
        }

        private void GoBackToPreviousState(object sender, EventArgs e)
        {
            pnBlastLayerSanitization.Visible = false;
            this.Refresh();

            var lastItem = lbSteps.Items[lbSteps.Items.Count - 1];

            if (lbSteps.Items.Count > 1)
            {
                lastItem = lbSteps.Items[lbSteps.Items.Count - 2];
            }

            #pragma warning disable CA1801,IDE0060 //type is used for templating
            static T Cast<T>(object obj, T type) { return (T)obj; }
            var modified = Cast(lastItem, new { Text = "", Value = new BlastLayer() });

            BlastLayer bl = (BlastLayer)modified.Value.Clone();
            S.GET<NewBlastEditorForm>().LoadBlastlayer(bl);
            workBlastLayer = bl;

            UpdateSanitizeProgress();

            if (lbSteps.Items.Count > 1)
            {
                lbSteps.Items.RemoveAt(lbSteps.Items.Count - 1);
            }

            S.GET<NewBlastEditorForm>().LoadCorrupt(null, null);

            lbSanitizationText.Text = "Is the effect you are looking for still present?";
            btnYesEffect.Visible = true;
            btnNoEffect.Visible = true;
            btnReroll.Visible = true;

            if (lbSteps.Items.Count == 1)
            {
                lbWorkingPleaseWait.Visible = false;
                pnBlastLayerSanitization.Visible = false;
                btnStartSanitizing.Visible = true;
            }
            else
            {
                pnBlastLayerSanitization.Visible = true;
            }
        }

        public void UpdateSanitizeProgress()
        {
            int originalSize = originalBlastLayer.Layer.Count(x => !x.IsLocked);

            int originalRemainder = originalSize;
            int originalMaxsteps = 0;
            while (originalRemainder > 1)
            {
                originalRemainder = originalRemainder / 2;
                originalMaxsteps++;
            }


            int currentSize = S.GET<NewBlastEditorForm>().currentSK.BlastLayer.Layer.Count(x => !x.IsLocked);
            //int currentSize = workBlastLayer

            int currentRemainder = currentSize;
            int currentMaxsteps = 0;
            while (currentRemainder > 1)
            {
                currentRemainder = currentRemainder / 2;
                currentMaxsteps++;
            }

            lbCurrentLayerSize.Text = $"Current Layer size: {currentSize}";
            pbProgress.Maximum = originalMaxsteps;
            pbProgress.Value = originalMaxsteps - currentMaxsteps;
        }

        public void StartSanitizing(object sender, EventArgs e)
        {
            btnStartSanitizing.Visible = false;

            S.GET<NewBlastEditorForm>().dgvBlastEditor.ClearSelection();
            S.GET<NewBlastEditorForm>().Disable50(null, null);
            S.GET<NewBlastEditorForm>().LoadCorrupt(null, null);

            pnBlastLayerSanitization.Visible = true;
            lbWorkingPleaseWait.Visible = true;
        }

        public static void RunSanitizeAlgo()
        {
            S.GET<NewBlastEditorForm>().dgvBlastEditor.ClearSelection();
            S.GET<NewBlastEditorForm>().Disable50(null, null);
            S.GET<NewBlastEditorForm>().LoadCorrupt(null, null);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
            {
                return;
            }

            Form frm = (sender as Form);
            Button check = (frm?.ActiveControl as Button);

            if (check == null && lbSteps.Items.Count > 1)
            {
                DialogResult dr = MessageBox.Show("Would you like to restore the Original BlastLayer?", "Leaving Sanitize Tool", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (dr)
                {
                    case DialogResult.Yes:
                        S.GET<NewBlastEditorForm>().LoadBlastlayer(originalBlastLayer);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void AddToStockpile(object sender, EventArgs e)
        {
            if (S.GET<NewBlastEditorForm>().AddStashToStockpile())
                this.Close();
        }
        private void AddToStash(object sender, EventArgs e)
        {
            S.GET<NewBlastEditorForm>().SendToStash(null, null);
            this.Close();
        }
        private void LeaveWithNoChanges(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
