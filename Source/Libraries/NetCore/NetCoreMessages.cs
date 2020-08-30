namespace RTCV.NetCore
{
    using System;
    using Ceras;

    [Serializable()]
    [MemberConfig(TargetMember.All)]
    public abstract class NetCoreMessage
    {
        public string Type;
    }

    [Serializable()]
    [MemberConfig(TargetMember.All)]
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
    [MemberConfig(TargetMember.All)]
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
