namespace RTCV.CorruptCore
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Newtonsoft.Json;

    [Serializable]
    public class ActiveTableObject : IEquatable<ActiveTableObject>
    {
        public long[] Data { get; set; }

        public ActiveTableObject()
        {
        }

        public ActiveTableObject(long[] data)
        {
            Data = data;
        }

        public bool Equals(ActiveTableObject other)
        {
            if (other == null)
            {
                return this == null;
            }

            return Enumerable.SequenceEqual(Data, other.Data);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ActiveTableObject);
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }
    }
}
