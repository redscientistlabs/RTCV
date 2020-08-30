namespace RTCV.NetCore
{
    using System;

    public class NetCoreReceiver
    {
        public bool Attached = false;
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
