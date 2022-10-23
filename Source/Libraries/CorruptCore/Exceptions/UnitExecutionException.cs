namespace RTCV.CorruptCore.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class UnitExecutionException : Exception
    {
        public UnitExecutionException() : base()
        {
        }

        public UnitExecutionException(string message) : base(message)
        {
        }

        public UnitExecutionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnitExecutionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
