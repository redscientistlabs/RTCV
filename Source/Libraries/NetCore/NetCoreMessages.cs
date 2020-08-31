namespace RTCV.NetCore
{
    using System;
    using Ceras;

    [Serializable()]
    [MemberConfig(TargetMember.All)]
    public abstract class NetCoreMessage
    {
        public string Type { get; set; }
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
        public Guid? requestGuid { get; set; } = null;
        public object objectValue { get; set; } = null;

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
