namespace RTCV.CorruptCore.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class StockpileSaveException : Exception
    {
        public StockpileSaveException() : base()
        {
        }

        public StockpileSaveException(string message) : base(message)
        {
        }

        public StockpileSaveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StockpileSaveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
