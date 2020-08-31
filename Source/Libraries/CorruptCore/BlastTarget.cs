namespace RTCV.CorruptCore
{
    using System;
    using Ceras;

    [Serializable]
    [MemberConfig(TargetMember.All)]
    public class BlastTarget
    {
        public string Domain { get; private set; } = null;
        public long Address { get; private set; } = 0;

        public BlastTarget(string _domain, long _address)
        {
            Domain = _domain;
            Address = _address;
        }
    }
}
