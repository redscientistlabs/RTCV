namespace RTCV.UI
{
    using System;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class UI_SaveProgress_Form : ComponentForm, IAutoColorize, ISubForm
    {
        public UI_SaveProgress_Form()
        {
            InitializeComponent();

            RtcCore.ProgressBarHandler += StockpileProgressBarHandler;
        }

        private void StockpileProgressBarHandler(object source, ProgressBarEventArgs e)
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                lbCurrentAction.Text = e.CurrentTask;
                if ((int)e.Progress > 100)
                {
                    e.Progress = 100;
                }

                pbSave.Value = (int)e.Progress;
            });
        }

        public bool SubForm_HasLeftButton => false;
        public bool SubForm_HasRightButton => false;
        public string SubForm_LeftButtonText { get; }
        public string SubForm_RightButtonText { get; }

        public void SubForm_LeftButton_Click()
        {
            throw new NotImplementedException();
        }

        public void SubForm_RightButton_Click()
        {
            throw new NotImplementedException();
        }

        public void OnShown()
        {
            logger.Trace("Entering OnShown() {0}\n{1}", System.Threading.Thread.CurrentThread.ManagedThreadId, Environment.StackTrace);
            lbCurrentAction.Text = "Waiting";
            pbSave.Value = 0;
            try
            {
                UI_VanguardImplementation.connector?.netConn?.spec?.LockStatusEventLockout();
                logger.Trace("Thread id {0} got Mutex... (save)", System.Threading.Thread.CurrentThread.ManagedThreadId);
            }
            catch (System.Threading.AbandonedMutexException)
            {
                logger.Trace("AbandonedMutexException! Thread id {0} got Mutex... (save)", System.Threading.Thread.CurrentThread.ManagedThreadId);
            }
        }

        public void OnHidden()
        {
            logger.Trace("Entering OnHidden() {0}\n{1}", System.Threading.Thread.CurrentThread.ManagedThreadId, Environment.StackTrace);
            UI_VanguardImplementation.connector?.netConn?.spec?.UnlockLockStatusEventLockout();
            logger.Trace("Thread id {0} released Mutex... (save)", System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
    }
}
