namespace SerializationTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using RTCV.CorruptCore;

    [TestClass]
    public class CorruptCoreSerializationTest
    {
        [TestMethod]
        public void TestActiveTableObjectSerialization()
        {
            long[] data = { 0L, 1L };
            var activeTableObject = new ActiveTableObject(data);
            var serialized = JsonConvert.SerializeObject(activeTableObject);
            var deserializedActiveTableObject = JsonConvert.DeserializeObject<ActiveTableObject>(serialized);

            Assert.IsTrue(condition: activeTableObject.Equals(deserializedActiveTableObject));
        }
    }
}
