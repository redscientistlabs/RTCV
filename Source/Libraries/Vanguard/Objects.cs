using System;
using RTCV.NetCore;

namespace RTCV.Vanguard
{
    public class TargetSpec
    {
        public event EventHandler<NetCoreEventArgs> MessageReceived;
        public virtual void OnMessageReceived(NetCoreEventArgs e) => MessageReceived?.Invoke(this, e);

        public FullSpec specDetails;
    }
}
