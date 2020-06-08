namespace RTCV.CorruptCore.EventWarlock
{
    using System;

    /// <summary>
    /// All subclasses must be serializable
    /// </summary>
    [Serializable]
    public abstract class WarlockAction
    {
        //Add param data in subclasses
        public abstract void DoAction(Grimoire grimoire);
    }
}
