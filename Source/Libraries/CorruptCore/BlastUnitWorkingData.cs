namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using Ceras;

    /// <summary>
    /// Working data for BlastUnits.
    /// Not serialized
    /// </summary>
    [MemberConfig(TargetMember.None)]
    public class BlastUnitWorkingData
    {
        //We Calculate a LastFrame at the beginning of execute
        public int LastFrame { get; set; } = -1;

        //We calculate ExecuteFrameQueued which is the ExecuteFrame + the currentframe that was calculated at the time of it entering the execution pool
        public int ExecuteFrameQueued { get; set; } = 0;

        //We use ApplyValue so we don't need to keep re-calculating the tiled value every execute if we don't have to.
        public byte[] ApplyValue { get; set; } = null;

        //The data that has been backed up. This is a list of bytes so if they start backing up at IMMEDIATE, they can have historical backups
        public Queue<byte[]> StoreData { get; private set; } = new Queue<byte[]>();
    }
}
