namespace RTCV.UI
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI;

    public partial class FastSanitizeToolForm : Modular.ColorizedForm
    {
        private FastSanitizer _sanitizer = null;
        private int _originalSize = 0;

        public FastSanitizeToolForm()
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

        public static void OpenSanitizeTool(StashKey sk = null, bool lockUI = true)
        {
            if (!S.ISNULL<FastSanitizeToolForm>() && S.GET<FastSanitizeToolForm>().IsDisposed)
            {
                S.GET<FastSanitizeToolForm>()?.Close();
            }
            var stf = new FastSanitizeToolForm();
            S.SET(stf);

            var bl = sk?.BlastLayer;

            if (sk == null || bl == null)
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

            BlastLayer clone = new BlastLayer(bl.Layer.Where(x => !x.IsLocked).ToList());
            stf._originalSize = clone.Layer.Count;
            stf.lbOriginalLayerSize.Text = $"Original Layer size: {clone.Layer.Count}";
            stf.lbSteps.DisplayMember = "Text";
            stf.lbSteps.ValueMember = "Value";

            stf._sanitizer = new FastSanitizer(sk, clone);
            stf.UpdateSanitizeProgress();
            stf.lbSteps.Items.Add(new { Text = $"[{stf._sanitizer.OriginalLayer.Layer.Count} Units]", Value = "" });

            if (lockUI)
            {
                stf.ShowDialog();
            }
            else
            {
                stf.Show();
            }
        }

        public async void Reroll(object sender, EventArgs e)
        {
            pnBlastLayerSanitization.Visible = false;
            this.Refresh();

            _sanitizer.Disable50();
            await _sanitizer.LoadCorrupt();
            UpdateSanitizeProgress();
            lbSteps.Items.RemoveAt(lbSteps.Items.Count - 1);
            lbSteps.Items.Add(new { Text = $"[{_sanitizer.NumCurUnits} Units]", Value = "" });
            pnBlastLayerSanitization.Visible = true;
        }

        public async void YesEffect(object sender, EventArgs e)
        {
            pnBlastLayerSanitization.Visible = false;
            this.Refresh();

            _sanitizer.Yes();
            _sanitizer.Disable50();
            await _sanitizer.LoadCorrupt();

            UpdateSanitizeProgress();
            lbSteps.Items.Add(new { Text = $"[{_sanitizer.NumCurUnits} Units]", Value = "" });

            if (_sanitizer.NumCurUnits == 1)
            {
                lbSanitizationText.Text = "1 Unit remaining, sanitization complete.";
                btnYesEffect.Visible = false;
                btnNoEffect.Visible = false;
                btnReroll.Visible = false;
            }

            pnBlastLayerSanitization.Visible = true;
        }

        public async void NoEffect(object sender, EventArgs e)
        {
            pnBlastLayerSanitization.Visible = false;
            this.Refresh();

            _sanitizer.No();

            _sanitizer.Disable50();
            await _sanitizer.LoadCorrupt();

            UpdateSanitizeProgress();
            lbSteps.Items.Add(new { Text = $"[{_sanitizer.NumCurUnits} Units]", Value = "" });

            if (_sanitizer.NumCurUnits == 1)
            {
                lbSanitizationText.Text = "1 Unit remaining, sanitization complete.";
                btnYesEffect.Visible = false;
                btnNoEffect.Visible = false;
                btnReroll.Visible = false;
            }

            pnBlastLayerSanitization.Visible = true;
        }

        private async void ReplayCorruption(object sender, EventArgs e)
        {
            pnBlastLayerSanitization.Visible = false;
            this.Refresh();
            await _sanitizer.Replay();
            pnBlastLayerSanitization.Visible = true;
        }

        public void LeaveAndKeepChanges(object sender, EventArgs e)
        {
            BlastEditorForm.OpenBlastEditor(_sanitizer.GetFinalStashKey());
            this.Close();
        }

        public void LeaveAndSubtractChanges(object sender, EventArgs e)
        {
            var sk = _sanitizer.GetStashKeyMinusChanges();
            BlastEditorForm.OpenBlastEditor(sk);
            this.Close();
        }

        private void LeaveWithoutChanges(object sender, EventArgs e)
        {
            //Open blast editor with original blast layer/savestate
            BlastEditorForm.OpenBlastEditor(_sanitizer.GetOriginalStashKey());
            this.Close();
        }

        private async void GoBackToPreviousState(object sender, EventArgs e)
        {
            if (_sanitizer.stateStack.Count < 1 || _sanitizer.shownStack.Count < 1) return;

            pnBlastLayerSanitization.Visible = false;
            this.Refresh();

            _sanitizer.Undo();
            UpdateSanitizeProgress();
            lbSteps.Items.RemoveAt(lbSteps.Items.Count - 1);

            await _sanitizer.LoadCorrupt();

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
            int originalRemainder = _originalSize;
            int originalMaxsteps = 0;
            while (originalRemainder > 1)
            {
                originalRemainder = originalRemainder / 2;
                originalMaxsteps++;
            }

            int currentSize = _sanitizer.NumCurUnits;

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

        public async void StartSanitizing(object sender, EventArgs e)
        {
            btnStartSanitizing.Visible = false;

            _sanitizer.Disable50();
            await _sanitizer.LoadCorrupt();

            pnBlastLayerSanitization.Visible = true;
            lbWorkingPleaseWait.Visible = true;
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
                        //S.GET<BlastEditorForm>().LoadBlastlayer(_originalBlastLayer);
                        BlastEditorForm.OpenBlastEditor(_sanitizer.GetOriginalStashKey());
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
            StashKey newSk = _sanitizer.GetFinalStashKey();
            StockpileManagerUISide.StashHistory.Add(newSk);

            S.GET<StashHistoryForm>().RefreshStashHistory();
            S.GET<StockpileManagerForm>().dgvStockpile.ClearSelection();
            S.GET<StashHistoryForm>().lbStashHistory.ClearSelected();

            S.GET<StashHistoryForm>().DontLoadSelectedStash = true;
            S.GET<StashHistoryForm>().lbStashHistory.SelectedIndex = S.GET<StashHistoryForm>().lbStashHistory.Items.Count - 1;
            StockpileManagerUISide.CurrentStashkey = StockpileManagerUISide.StashHistory[S.GET<StashHistoryForm>().lbStashHistory.SelectedIndex];

            bool res = S.GET<StashHistoryForm>().AddStashToStockpileFromUI();

            if (res)
            {
                this.Close();
            }
        }
        private void AddToStash(object sender, EventArgs e)
        {
            StashKey oldSk = _sanitizer.GetFinalStashKey();

            StashKey newSk = new StashKey(RtcCore.GetRandomKey(), oldSk.ParentKey, null)
            {
                RomFilename = oldSk.RomFilename,
                SystemName = oldSk.SystemName,
                SystemCore = oldSk.SystemCore,
                GameName = oldSk.GameName,
                SyncSettings = oldSk.SyncSettings,
                StateLocation = oldSk.StateLocation
            };
            newSk.BlastLayer = (BlastLayer)oldSk.BlastLayer.Clone();
            StockpileManagerUISide.StashHistory.Add(newSk);

            S.GET<StashHistoryForm>().RefreshStashHistory();
            S.GET<StockpileManagerForm>().dgvStockpile.ClearSelection();
            S.GET<StashHistoryForm>().lbStashHistory.ClearSelected();

            S.GET<StashHistoryForm>().DontLoadSelectedStash = true;
            S.GET<StashHistoryForm>().lbStashHistory.SelectedIndex = S.GET<StashHistoryForm>().lbStashHistory.Items.Count - 1;
            StockpileManagerUISide.CurrentStashkey = StockpileManagerUISide.StashHistory[S.GET<StashHistoryForm>().lbStashHistory.SelectedIndex];
            S.GET<StashHistoryForm>().AddStashToStockpileFromUI();
        }
        private void LeaveWithNoChanges(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    internal class FastSanitizer
    {
        private StashKey internalSK;
        public BlastLayer OriginalLayer { get; private set; }
        public Stack<List<BlastUnit>> stateStack = new Stack<List<BlastUnit>>();
        public Stack<List<BlastUnit>> shownStack = new Stack<List<BlastUnit>>();
        public List<BlastUnit> shownHalf;
        public List<BlastUnit> otherHalf;
        Random rand = new Random();
        public int NumCurUnits => stateStack.Peek().Count;
        public FastSanitizer(StashKey originalStashkey, BlastLayer blClone)
        {
            //Create StashKey clone
            internalSK = new StashKey(RtcCore.GetRandomKey(), originalStashkey.ParentKey, null)
            {
                RomFilename = originalStashkey.RomFilename,
                SystemName = originalStashkey.SystemName,
                SystemCore = originalStashkey.SystemCore,
                GameName = originalStashkey.GameName,
                SyncSettings = originalStashkey.SyncSettings,
                StateLocation = originalStashkey.StateLocation
            };
            internalSK.BlastLayer = blClone;
            OriginalLayer = blClone;
            shownHalf = OriginalLayer.Layer;
            otherHalf = OriginalLayer.Layer;
            stateStack.Push(OriginalLayer.Layer);
        }

        internal StashKey GetFinalStashKey()
        {
            internalSK.BlastLayer = new BlastLayer(shownHalf);
            return internalSK;
        }

        internal void Clean()
        {
            stateStack.Clear();
            shownHalf = null;
            otherHalf = null;
            internalSK = null;
            OriginalLayer = null;
        }

        internal void Undo()
        {
            stateStack.Pop();
            shownHalf = shownStack.Pop();
        }

        internal void Yes()
        {
            stateStack.Push(shownHalf);
            internalSK.BlastLayer = new BlastLayer(shownHalf);
            shownStack.Push(shownHalf);
        }
        internal void No()
        {
            stateStack.Push(otherHalf);
            internalSK.BlastLayer = new BlastLayer(otherHalf);
            shownStack.Push(shownHalf);
        }

        internal void Disable50()
        {
            var lastState = stateStack.Peek();
            shownHalf = new List<BlastUnit>();
            otherHalf = new List<BlastUnit>();

            if (lastState.Count == 1)
            {
                shownHalf = lastState;
                otherHalf = lastState;
                return;
            }

            int[] allIndices = new int[lastState.Count];
            for (int i = 0; i < lastState.Count; i++)
            {
                allIndices[i] = i;
            }

            //In-place shuffle, optimized with cached lengths
            var shuffleCount = allIndices.Length;
            var shuffleEnd = shuffleCount - 1;
            for (var i = 0; i < shuffleEnd; ++i)
            {
                var r = rand.Next(i, shuffleCount);
                var tmp = allIndices[i];
                allIndices[i] = allIndices[r];
                allIndices[r] = tmp;
            }

            for (int i = 0; i < allIndices.Length - 1; i += 2)
            {
                shownHalf.Add(lastState[allIndices[i]]);
                otherHalf.Add(lastState[allIndices[i + 1]]);
            }

            if (lastState.Count % 2 == 1)
            {
                shownHalf.Add(lastState[allIndices.Length - 1]);
            }
        }

        internal async Task LoadCorrupt()
        {
            internalSK.BlastLayer = new BlastLayer(shownHalf);
            S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = internalSK.Run();
        }

        internal StashKey GetOriginalStashKey()
        {
            internalSK.BlastLayer = OriginalLayer;
            return internalSK;
        }

        internal StashKey GetStashKeyMinusChanges()
        {
            List<BlastUnit> newLayer = new List<BlastUnit>();
            for (int j = 0; j < OriginalLayer.Layer.Count; j++)
            {
                newLayer.AddRange(OriginalLayer.Layer.Where(x => !shownHalf.Contains(x)));
            }
            internalSK.BlastLayer = new BlastLayer(newLayer);
            return internalSK;
        }

        internal async Task Replay()
        {
            S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = internalSK.Run();
        }
    }

}
