namespace RTCV.NetCore
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Ceras;

    [Serializable()]
    [MemberConfig(TargetMember.All)]
    public abstract class NetCoreMessage
    {
        [SuppressMessage("Microsoft.Design", "CA1051", Justification = "Unknown serialization impact of making this property instead of a field")]
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
