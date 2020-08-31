namespace RTCV.CorruptCore
{
    using System;

    public class ProgressBarEventArgs : EventArgs
    {
        public string CurrentTask { get; private set; }
        public decimal Progress { get; set; }

        public ProgressBarEventArgs(string text, decimal progress)
        {
            CurrentTask = text;
            Progress = progress;

            Common.Logging.GlobalLogger.Log(NLog.LogLevel.Info, $"ProgressBarEventArgs: {text}");
        }
    }
}
