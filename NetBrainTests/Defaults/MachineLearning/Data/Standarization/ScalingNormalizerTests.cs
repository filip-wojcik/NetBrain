using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Defaults.MachineLearning.Data.Standarization;

namespace NetBrainTests.Defaults.MachineLearning.Data.Standarization
{
    [TestClass()]
    public class ScalingNormalizerTests
    {
        [TestMethod()]
        public void Normalize_FromKnownValues_Test()
        {
            //Given
            var values = Enumerable.Range(5, 11).Select(num => (double) num);
            Func<double, object> mappingFunction = num => num as object;
            var subject = new ScalingNormalizer<object>(mappingFunction, values);

            var expectedNormalizedValue1 = 0.5;
            var expectedNormalizedValue2 = 0.9;

            var value1 = 10.0;
            var value2 = 14.0;
            //When

            var normalizedValue1 = subject.Normalize(value1);
            var normalizedValue2 = subject.Normalize(value2);

            //Then
            Assert.AreEqual(expectedNormalizedValue1, normalizedValue1);
            Assert.AreEqual(expectedNormalizedValue2, normalizedValue2);
        }

        [TestMethod()]
        public void Normalize_FromMinMax_Test()
        {
            //Given
            Func<double, object> mappingFunction = num => num as object;
            var subject = new ScalingNormalizer<object>(mappingFunction, 5, 15);

            var expectedNormalizedValue1 = 0.5;
            var expectedNormalizedValue2 = 0.9;

            var value1 = 10.0;
            var value2 = 14.0;
            //When

            var normalizedValue1 = subject.Normalize(value1);
            var normalizedValue2 = subject.Normalize(value2);

            //Then
            Assert.AreEqual(expectedNormalizedValue1, normalizedValue1);
            Assert.AreEqual(expectedNormalizedValue2, normalizedValue2);
        }

        [TestMethod()]
        public void DenormalizeTest()
        {
            //Given
            var values = Enumerable.Range(5, 11).Select(num => (double)num);
            Func<double, object> mappingFunction = num => num as object;
            var subject = new ScalingNormalizer<object>(mappingFunction, values);

            var normalizedValue1 = 0.5;
            var normalizedValue2 = 0.9;

            var expectedValue1 = 10;
            var expectedValue2 = 14;
            //When

            var denormalizedValue1 = subject.Denormalize(normalizedValue1);
            var denormalizedValue2 = subject.Denormalize(normalizedValue2);

            //Then
            Assert.AreEqual(expectedValue1, denormalizedValue1);
            Assert.AreEqual(expectedValue2, denormalizedValue2);
        }
    }
}
