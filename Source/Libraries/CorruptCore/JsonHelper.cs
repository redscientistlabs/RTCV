namespace RTCV.CorruptCore
{
    using System.IO;
    using Newtonsoft.Json;
    using RTCV.NetCore.SafeJsonTypeSerialization;

    public static class JsonHelper
    {
        public static void Serialize(object value, Stream s, Formatting f = Formatting.None, JsonKnownTypesBinder binder = null)
        {
            using (var writer = new StreamWriter(s))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                var ser = new JsonSerializer
                {
                    Formatting = f,
                    SerializationBinder = binder ?? new JsonKnownTypesBinder()
                };
                ser.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                ser.Serialize(jsonWriter, value);
                jsonWriter.Flush();
            }
        }

        public static T Deserialize<T>(Stream s, JsonKnownTypesBinder binder = null)
        {
            using (var reader = new StreamReader(s))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var ser = new JsonSerializer()
                {
                    SerializationBinder = binder ?? new JsonKnownTypesBinder()
                };
                return ser.Deserialize<T>(jsonReader);
            }
        }

        //Wrap JsonConvert so we can access this in vanguard implementations without importing json.net directly
        public static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
