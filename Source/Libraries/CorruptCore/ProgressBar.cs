namespace RTCV.CorruptCore
{
    using System;

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
