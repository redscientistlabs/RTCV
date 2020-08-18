namespace RTCV.CorruptCore.StockpileNS
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SaveException : Exception
    {
        public SaveException() : base()
        {
        }

        public SaveException(string message) : base(message)
        {
        }

        public SaveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SaveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
