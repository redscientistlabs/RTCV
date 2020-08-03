namespace RTCV.CorruptCore
{
    using System;
    using Ceras;

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class BlastTarget
    {
        public string Domain = null;
        public long Address = 0;

        public BlastTarget(string _domain, long _address)
        {
            Domain = _domain;
            Address = _address;
        }
    }
}
