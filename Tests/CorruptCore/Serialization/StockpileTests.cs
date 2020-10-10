
namespace Tests.CorruptCore.Serialization
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using RTCV.CorruptCore;
    using FluentAssertions;
    using FluentAssertions.Common;

    [TestClass]
    public class StockpileTests
    {
        [TestMethod]
        public void TestDefaultStockpileSerialization()
        {
            var stockpile = new Stockpile();
            var tempFilePath = Path.GetTempFileName();
            using (var fileStream = new FileStream(tempFilePath, FileMode.OpenOrCreate))
            {
                JsonHelper.Serialize(stockpile, fileStream, Formatting.Indented);
            }

            using (var actualStreamReader = new StreamReader(tempFilePath))
            {
                var actualValue = actualStreamReader.ReadToEnd();

                using (var streamReader = new StreamReader("../Tests/CorruptCore/Serialization/Resources/DefaultStockpile.json"))
                {
                    var expectedValue = streamReader.ReadToEnd();
                    actualValue.Should().Be(expectedValue);
                }
            }

            File.Delete(tempFilePath);
        }
    }
}
