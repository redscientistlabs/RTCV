namespace RTCV.Vanguard
{
    using System;
    using RTCV.NetCore;

    public class TargetSpec
    {
        public event EventHandler<NetCoreEventArgs> MessageReceived;
        public virtual void OnMessageReceived(NetCoreEventArgs e) => MessageReceived?.Invoke(this, e);

        public FullSpec specDetails;
    }
}
