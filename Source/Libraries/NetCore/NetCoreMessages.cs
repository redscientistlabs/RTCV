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
        public NetCoreSimpleMessage(string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type = type.Trim().ToUpper();
        }
    }

    [Serializable()]
    [MemberConfig(TargetMember.All)]
    public class NetCoreAdvancedMessage : NetCoreMessage
    {
        [SuppressMessage("Microsoft.Design", "CA1051", Justification = "Unknown serialization impact of making this property instead of a field")]
        public Guid? requestGuid = null;

        [SuppressMessage("Microsoft.Design", "CA1051", Justification = "Unknown serialization impact of making this property instead of a field")]
        public object objectValue = null;

        public NetCoreAdvancedMessage()
        {
        }
        public NetCoreAdvancedMessage(string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type = type.Trim().ToUpper();
        }

        public NetCoreAdvancedMessage(string type, object obj)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type = type.Trim().ToUpper();
            objectValue = obj;
        }
    }
}
