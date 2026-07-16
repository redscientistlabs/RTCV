namespace RTCV.CorruptCore
{
    using System;

    public class ProgressBarEventArgs : EventArgs
    {
        public string CurrentTask { get; private set; }
        public decimal Progress { get; set; }
        public int ToastID { get; set; }

        public ProgressBarEventArgs(string text, decimal progress, int toastID = -1)
        {
            CurrentTask = text;
            Progress = progress;
            ToastID = toastID;

            Common.Logging.GlobalLogger.Log(NLog.LogLevel.Info, $"ProgressBarEventArgs: {text}");
        }
    }
}
