using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Abstracts.MachineLearning.MathematicalFunctions
{
    [TestClass()]
    public class ChaosMeasureFunctionsTests
    {
        [TestMethod()]
        public void ShannonEntropyTest()
        {
            //Given
            var dataSet = new SingleValueDataSet<string>(new string[0], 1, 0,
                    new ISingleValueFeatureVector<string>[]{
                          new SingleValueFeatureVector<string>(new string[] { "A" }),
                          new SingleValueFeatureVector<string>(new string[] { "A" }),
                          new SingleValueFeatureVector<string>(new string[] { "B" }),
                          new SingleValueFeatureVector<string>(new string[] { "B" }),
                          new SingleValueFeatureVector<string>(new string[] { "B" })
                });

            //Then
            Assert.AreEqual(
                0.9709,
                ChaosMeasureFunctions.ShannonEntropy(dataSet, 0),
                0.0009);
        }

        [TestMethod()]
        public void GiniImpurityTest()
        {
             //Given
            var dataSet = new SingleValueDataSet<string>(new string[0], 1, 0,
                    new ISingleValueFeatureVector<string>[]{
                          new SingleValueFeatureVector<string>(new string[] { "A" }),
                          new SingleValueFeatureVector<string>(new string[] { "A" }),
                          new SingleValueFeatureVector<string>(new string[] { "A" }),
                          new SingleValueFeatureVector<string>(new string[] { "A" }),
                          new SingleValueFeatureVector<string>(new string[] { "B" }),
                          new SingleValueFeatureVector<string>(new string[] { "B" }),
                          new SingleValueFeatureVector<string>(new string[] { "B" }),
                          new SingleValueFeatureVector<string>(new string[] { "C" }),
                          new SingleValueFeatureVector<string>(new string[] { "C" }),
                          new SingleValueFeatureVector<string>(new string[] { "C" })
                });
            var expected = 0.66;
            var allowedDifference = 0.009;

            //When
            var actual = ChaosMeasureFunctions.GiniImpurity(dataSet, 0);


            //Then
            Assert.AreEqual(
                expected,
                actual,
                allowedDifference
                );
        }

        [TestMethod()]
        public void GiniImpurityTest_uniformData()
        {
            //Given
            var dataSet = new SingleValueDataSet<string>(new string[0], 1, 0,
                new ISingleValueFeatureVector<string>[]
                {
                    new SingleValueFeatureVector<string>(new string[] {"A"}),
                    new SingleValueFeatureVector<string>(new string[] {"A"}),
                    new SingleValueFeatureVector<string>(new string[] {"A"}),
                    new SingleValueFeatureVector<string>(new string[] {"A"})
                });

            //Then
            double entropy1 = ChaosMeasureFunctions.GiniImpurity(dataSet, 0);
            double entropy2 = ChaosMeasureFunctions.ShannonEntropy(dataSet, 0);
            Assert.AreEqual(entropy1, entropy2);
        }
    }
}
