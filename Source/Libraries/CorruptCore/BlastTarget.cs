namespace RTCV.CorruptCore
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Ceras;

    [Serializable]
    [MemberConfig(TargetMember.All)]
    public class BlastTarget
    {
        [SuppressMessage("Microsoft.Design", "CA1051", Justification = "Unknown serialization impact of making this property instead of a field")]
        public string Domain = null;

        [SuppressMessage("Microsoft.Design", "CA1051", Justification = "Unknown serialization impact of making this property instead of a field")]
        public long Address = 0;

        public BlastTarget(string domain, long address)
        {
            Domain = domain;
            Address = address;
        }
    }
}
