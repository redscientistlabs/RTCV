namespace RTCV.CorruptCore
{
    using System;

    public delegate void ProgressBarEventHandler(object source, ProgressBarEventArgs e);

    public class ProgressBarEventArgs : EventArgs
    {
        public string CurrentTask;
        public decimal Progress;

        public ProgressBarEventArgs(string text, decimal progress)
        {
            CurrentTask = text;
            Progress = progress;

            RTCV.Common.Logging.GlobalLogger.Log(NLog.LogLevel.Info, $"ProgressBarEventArgs: {text}");
        }
    }
}
