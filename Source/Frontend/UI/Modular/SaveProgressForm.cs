namespace RTCV.UI
{
    using System;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class SaveProgressForm : ComponentForm, ISubForm
    {
        public SaveProgressForm()
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

        public bool HasLeftButton => false;
        public bool HasRightButton => false;
        public string LeftButtonText { get; }
        public string RightButtonText { get; }

        public void LeftButtonClick()
        {
            throw new NotImplementedException();
        }

        public void RightButtonClick()
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
                VanguardImplementation.connector?.netConn?.Spec?.LockStatusEventLockout();
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
            VanguardImplementation.connector?.netConn?.Spec?.UnlockLockStatusEventLockout();
            logger.Trace("Thread id {0} released Mutex... (save)", System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
    }
}
