namespace RTCV.CorruptCore
{
    using System;

    [Serializable]
    public class ActiveTableObject
    {
        public long[] Data { get; set; }

        public ActiveTableObject()
        {
        }

        public ActiveTableObject(long[] data)
        {
            Data = data;
        }
    }
}
