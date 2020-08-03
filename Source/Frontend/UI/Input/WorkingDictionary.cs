namespace RTCV.UI.Input
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    //From bizhawk
    /// <summary>
    /// A dictionary that creates new values on the fly as necessary so that any key you need will be defined.
    /// </summary>
    /// <typeparam name="TKey">dictionary keys</typeparam>
    /// <typeparam name="TValue">dictionary values</typeparam>
    [Serializable]
    public class WorkingDictionary<TKey, TValue> : Dictionary<TKey, TValue>
        where TValue : new()
    {
        public new TValue this[TKey key]
        {
            get
            {
                if (!TryGetValue(key, out TValue temp))
                {
                    temp = this[key] = new TValue();
                }

                return temp;
            }

            set => base[key] = value;
        }

        public WorkingDictionary() { }

        protected WorkingDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
