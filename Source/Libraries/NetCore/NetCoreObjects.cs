namespace RTCV.NetCore
{
    using System;

    public enum NetworkSide
    {
        NONE,
        CLIENT,
        SERVER
    }

    public enum NetworkStatus
    {
        DISCONNECTED,
        CONNECTIONLOST,
        CONNECTING,
        CONNECTED,
        LISTENING
    }

    public class NetCoreReceiver
    {
        public bool Attached { get; set; } = false;
        public event EventHandler<NetCoreEventArgs> MessageReceived;
        public virtual void OnMessageReceived(NetCoreEventArgs e)
        {
            if (MessageReceived == null)
            {
                throw new Exception("No registered handler for MessageReceived!");
            }

            MessageReceived.Invoke(this, e);
        }
    }
}
