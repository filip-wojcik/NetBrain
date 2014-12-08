using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Defaults.MachineLearning.Data.Standarization;

namespace NetBrainTests.Defaults.MachineLearning.Data.Standarization
{
    [TestClass()]
    public class BinaryEncoderTests
    {
        [TestMethod()]
        public void EncodeTest()
        {
            //Given
            var data = new string[] { "male", "female" };
            var subject = new BinaryEncoder<string>(data, string.Empty);
            var expectedResult = new List<IList<double>>()
            {
                new double[]{ 1.0 },
                new double[]{ -1.0 }
            };

            //When
            var encodedData = new List<IList<double>>();
            foreach (var element in data)
            {
                encodedData.Add(subject.Encode(element));
            }

            //Then
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.IsTrue(expectedResult[i].SequenceEqual(encodedData[i]));
            }
        }

        [TestMethod()]
        public void DecodeTest()
        {
            //Given
            var data = new string[] { "male", "female" };
            var subject = new BinaryEncoder<string>(data, string.Empty);
            var expectedResults = new string[] { "male", "female" };
            var queries = new List<IList<double>>()
            {
                new double[]{ 1.0 },
                new double[]{ -1.0 }
            };

            //When
            var decodedData = new List<string>();
            foreach (var query in queries)
            {
                decodedData.Add(subject.Decode(query));
            }

            //Then
            for (int i = 0; i < expectedResults.Length; i++)
            {
                Assert.AreEqual(expectedResults[i], decodedData[i]);
            }
        }
    }
}
