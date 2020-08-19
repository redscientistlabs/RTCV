namespace RTCV.NetCore.SafeJsonTypeSerialization
{
    using System;
    using Newtonsoft.Json;

    public abstract class TypeWrapper
    {
        protected TypeWrapper() { }

        [JsonIgnore]
        public abstract object ObjectValue { get; }

        public static TypeWrapper CreateWrapper<T>(T value)
        {
            if (value == null)
            {
                return new TypeWrapper<T>();
            }

            var type = value.GetType();
            if (type == typeof(T))
            {
                return new TypeWrapper<T>(value);
            }
            // Return actual type of subclass
            return (TypeWrapper)Activator.CreateInstance(typeof(TypeWrapper<>).MakeGenericType(type), value);
        }
    }

    public sealed class TypeWrapper<T> : TypeWrapper
    {
        public TypeWrapper() : base() { }

        public TypeWrapper(T value)
            : base()
        {
            this.Value = value;
        }

        public override object ObjectValue => Value;

        public T Value { get; set; }
    }
}
