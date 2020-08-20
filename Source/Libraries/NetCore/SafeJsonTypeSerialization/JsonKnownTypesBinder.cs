namespace RTCV.NetCore.SafeJsonTypeSerialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Serialization;

    public class JsonKnownTypesBinder : ISerializationBinder
    {
        public IList<Type> KnownTypes { get; set; }

        public JsonKnownTypesBinder()
        {
            KnownTypes = new List<Type>();
        }

        public Type BindToType(string assemblyName, string typeName) => KnownTypes.SingleOrDefault(t => t.Name == typeName);

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }
    }
}
