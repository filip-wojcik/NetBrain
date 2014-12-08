using System;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Data.Models;
using NetBrain.Utils;

namespace NetBrainTests.Defaults.MachineLearning.Data.Models
{
    [TestClass()]
    public class SingleValueFeatureVectorTests
    {
        [TestMethod()]
        public void EqualsTest_ResultTrue()
        {
            //Given
            var vector1 = new SingleValueFeatureVector<string>(
                new string[]{ "a", "b", "c", "d" },
                1
                );

            var vector2 = new SingleValueFeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                1
                );

            //Then
            Assert.AreEqual(vector1, vector2);
        }

        [TestMethod()]
        public void EqualsTest_ResultFalse_FeaturesDiffer()
        {
            //Given
            var vector1 = new SingleValueFeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                1
                );

            var vector2 = new SingleValueFeatureVector<string>(
                new string[] { "a", "b", "e", "d" },
                1
                );

            //Then
            Assert.AreNotEqual(vector1, vector2);
        }

        [TestMethod()]
        public void EqualsTest_ResultFalse_ValuesDiffer()
        {
            //Given
            var vector1 = new SingleValueFeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                2
                );

            var vector2 = new SingleValueFeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                1
                );

            //Then
            Assert.AreNotEqual(vector1, vector2);
        }

        [TestMethod()]
        public void ValuesVectorTest_ValuesArePresent()
        {
            //Given
            var vector1 = new SingleValueFeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                1
                );

            var expectedValue = "b";

            //When
            var value = vector1.Value;

            //Then
            Assert.AreEqual(expectedValue, value);
        }

        [TestMethod()]
        public void ValuesVectorTest_ValuesAreNotPresent()
        {
            //Given
            var vector1 = new SingleValueFeatureVector<string>(
                new string[] { "a", "b", "c", "d" }
                );

            //When
            var value = vector1.Value;

            //Then
            Assert.IsNull(value);
        }

        [TestMethod()]
        public void NonValuesVectorTest_ValuesArePresent()
        {
            //Given
            var vector1 = new SingleValueFeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                1
                );

            var expectedFeatures = new string[] { "a", "c", "d" };

            //When
            ISingleValueFeatureVector<string> valuesVector = vector1.NonValuesVector;

            //Then
            Assert.IsTrue(expectedFeatures.SequenceEqual(valuesVector.Features));
            Assert.IsNull(valuesVector.Value);
            Assert.AreEqual(-1, valuesVector.ValueIndex);
        }

        [TestMethod()]
        public void NonValuesVectorTest_ValuesAreNotPresent()
        {
            //Given
            var vector1 = new SingleValueFeatureVector<string>(
                new string[] { "a", "b", "c", "d" }
                );

            var expectedFeatures = new string[] { "a", "b", "c", "d" };

            //When
            ISingleValueFeatureVector<string> valuesVector = vector1.NonValuesVector;

            //Then
            Assert.IsTrue(expectedFeatures.SequenceEqual(valuesVector.Features));
            Assert.IsNull(valuesVector.Value);
            Assert.AreEqual(-1, vector1.ValueIndex);
        }
    }
}
