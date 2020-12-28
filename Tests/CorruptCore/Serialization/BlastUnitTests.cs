namespace Tests.CorruptCore.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using RTCV.CorruptCore;

    [TestClass]
    public class BlastUnitTests
    {
        private const string TestResourceFolder = "../Tests/CorruptCore/Serialization/Resources";
        private static readonly string defaultBlastUnitPath = Path.Combine(TestResourceFolder, "DefaultBlastUnit.json");

        [TestMethod]
        public void TestDefaultBlastUnitSerialization()
        {
            var blastUnit = new BlastUnit();
            var tempFilePath = Path.GetTempFileName();
            using (var fileStream = new FileStream(tempFilePath, FileMode.OpenOrCreate))
            {
                JsonHelper.Serialize(blastUnit, fileStream, Formatting.Indented);
            }

            using (var actualStreamReader = new StreamReader(tempFilePath))
            {
                var actualValue = actualStreamReader.ReadToEnd();

                using (var streamReader = new StreamReader(defaultBlastUnitPath))
                {
                    var expectedValue = streamReader.ReadToEnd();
                    actualValue.Should().Be(expectedValue);
                }
            }

            File.Delete(tempFilePath);
        }
    }
}
