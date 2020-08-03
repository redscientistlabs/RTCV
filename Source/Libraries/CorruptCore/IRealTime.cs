namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRealTime
    {
        event EventHandler<RealTimeEventArgs> Step;
        event EventHandler GameLoaded;
        event EventHandler GameClosed;

        public bool SupportsRewind { get; set; }
        public bool SupportsForwarding { get; set; }
        public bool SupportsFastForwarding { get; set; }
    }

    public class RealTimeEventArgs : EventArgs
    {
        public bool isForwarding = false;
        public bool isRewinding = false;
        public bool isFastForwarding = false;
    }
}
