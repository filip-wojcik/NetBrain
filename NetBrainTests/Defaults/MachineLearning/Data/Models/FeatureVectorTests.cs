using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Data.Models
{
    [TestClass()]
    public class FeatureVectorTests
    {
        [TestMethod()]
        public void EqualsTest_ResultTrue()
        {
            //Given
            var vector1 = new FeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                new int[] { 1, 3 }
                );

            var vector2 = new FeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                new int[] { 1, 3 }
                );

            //Then
            Assert.AreEqual(vector1, vector2);
        }

        [TestMethod()]
        public void EqualsTest_ResultFalse_FeaturesDiffer()
        {
            //Given
            var vector1 = new FeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                new int[] { 1, 3 }
                );

            var vector2 = new FeatureVector<string>(
                new string[] { "a", "b", "e", "d" },
                new int[] { 1, 3 }
                );

            //Then
            Assert.AreNotEqual(vector1, vector2);
        }

        [TestMethod()]
        public void EqualsTest_ResultFalse_ValuesDiffer()
        {
            //Given
            var vector1 = new FeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                new int[] { 1, 3 }
                );

            var vector2 = new FeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                new int[] { 1, 2 }
                );

            //Then
            Assert.AreNotEqual(vector1, vector2);
        }

        [TestMethod()]
        public void ValuesVectorTest_ValuesArePresent()
        {
            //Given
            var vector1 = new FeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                new int[] { 1, 3 }
                );

            var expectedFeatures = new string[] { "b", "d" };
            var expectedValuesIndexes = new int[0];

            //When
            IFeatureVector<string> valuesVector = vector1.ValuesVector;

            //Then
            Assert.IsTrue(expectedFeatures.SequenceEqual(valuesVector.Features));
            Assert.IsTrue(expectedValuesIndexes.SequenceEqual(valuesVector.ValueIndexes));
        }

        [TestMethod()]
        public void ValuesVectorTest_ValuesAreNotPresent()
        {
            //Given
            var vector1 = new FeatureVector<string>(
                new string[] { "a", "b", "c", "d" }
                );

            var expectedFeatures = new string[0];
            var expectedValuesIndexes = new int[0];

            //When
            IFeatureVector<string> valuesVector = vector1.ValuesVector;

            //Then
            Assert.IsTrue(expectedFeatures.SequenceEqual(valuesVector.Features));
            Assert.IsTrue(expectedValuesIndexes.SequenceEqual(valuesVector.ValueIndexes));
        }

        [TestMethod()]
        public void NonValuesVectorTest_ValuesArePresent()
        {
            //Given
            var vector1 = new FeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                new int[] { 1, 3 }
                );

            var expectedFeatures = new string[] { "a", "c" };
            var expectedValuesIndexes = new int[0];

            //When
            IFeatureVector<string> valuesVector = vector1.NonValuesVector;

            //Then
            Assert.IsTrue(expectedFeatures.SequenceEqual(valuesVector.Features));
            Assert.IsTrue(expectedValuesIndexes.SequenceEqual(valuesVector.ValueIndexes));
        }

        [TestMethod()]
        public void NonValuesVectorTest_ValuesAreNotPresent()
        {
            //Given
            var vector1 = new FeatureVector<string>(
                new string[] { "a", "b", "c", "d" }
                );

            var expectedFeatures = new string[] { "a", "b", "c", "d" };
            var expectedValuesIndexes = new int[0];

            //When
            IFeatureVector<string> valuesVector = vector1.NonValuesVector;

            //Then
            Assert.IsTrue(expectedFeatures.SequenceEqual(valuesVector.Features));
            Assert.IsTrue(expectedValuesIndexes.SequenceEqual(valuesVector.ValueIndexes));
        }
    }
}
