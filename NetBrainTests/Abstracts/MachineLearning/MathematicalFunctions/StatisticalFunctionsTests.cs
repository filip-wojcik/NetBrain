using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Abstracts.MachineLearning.MathematicalFunctions
{
    [TestClass()]
    public class StatisticalFunctionsTests
    {
        [TestMethod]
        public void VarianceTest()
        {
            var data = new double[] { 2, 5, 1, 3 };
            Assert.AreEqual(2.19, StatisticalFunctions.Variance(data), 0.9);
        }

        [TestMethod()]
        public void VarianceTest_FeatureVectorsSet()
        {
            //Given
            var dataSet = new SingleValueDataSet<double>(new string[0], 2, 0,
                    new ISingleValueFeatureVector<double>[]{
                          new SingleValueFeatureVector<double>(new double[] { 2, 2}),
                          new SingleValueFeatureVector<double>(new double[] { 5, 5 }),
                          new SingleValueFeatureVector<double>(new double[] { 1, 1}),
                          new SingleValueFeatureVector<double>(new double[] { 3, 3 })
                });

            //When
            double variance = StatisticalFunctions.Variance(dataSet);

            //Then
            Assert.AreEqual(2.1875, variance);
        }

        [TestMethod()]
        public void MeanTest_FeatureVectorsSet()
        {
            //Given
            var dataSet = new SingleValueDataSet<double>(new string[0], 2, 1,
                    new ISingleValueFeatureVector<double>[]{
                          new SingleValueFeatureVector<double>(new double[] { 1, 4}),
                          new SingleValueFeatureVector<double>(new double[] { 2, 5 }),
                          new SingleValueFeatureVector<double>(new double[] { 3, 6})
                });

            //When
            double mean = StatisticalFunctions.Mean(dataSet);

            //Then
            Assert.AreEqual(5, mean);
        }
    }
}
