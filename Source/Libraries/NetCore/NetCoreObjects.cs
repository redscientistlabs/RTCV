namespace RTCV.NetCore
{
    using System;
    using Ceras;

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

    [Serializable()]
    [Ceras.MemberConfig(TargetMember.All)]
    public abstract class NetCoreMessage
    {
        public string Type;
    }

    [Serializable()]
    [Ceras.MemberConfig(TargetMember.All)]
    public class NetCoreSimpleMessage : NetCoreMessage
    {
        public NetCoreSimpleMessage()
        {
        }
        public NetCoreSimpleMessage(string _Type)
        {
            Type = _Type.Trim().ToUpper();
        }
    }

    [Serializable()]
    [Ceras.MemberConfig(TargetMember.All)]
    public class NetCoreAdvancedMessage : NetCoreMessage
    {
        public string ReturnedFrom;
        public bool Priority = false;
        public Guid? requestGuid = null;
        public object objectValue = null;

        public NetCoreAdvancedMessage()
        {
        }
        public NetCoreAdvancedMessage(string _Type)
        {
            Type = _Type.Trim().ToUpper();
        }

        public NetCoreAdvancedMessage(string _Type, object _Obj)
        {
            Type = _Type.Trim().ToUpper();
            objectValue = _Obj;
        }
    }
}
