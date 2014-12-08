using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Defaults.MachineLearning.Algorithms.Classification.Knn;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Classification.Knn
{
    [TestClass()]
    public class WeightedKnnClassifierTests
    {
        [TestMethod()]
        public void ClassifyTest()
        {
            //Given
            var fetauresSet = new SingleValueDataSet<double>(
                new[] {"data elem1", "data elem2", "data elem3", "value elem4", "value elem5"}, 5, 4,
                new SingleValueFeatureVector<double>[]
                {
                    new SingleValueFeatureVector<double>(new double[]{ 1.0, 1.0, 1.0, 10.0, 100.0 }),
                    new SingleValueFeatureVector<double>(new double[]{ 1.7, 1.7, 1.7, 17.0, 170.0 }),
                    new SingleValueFeatureVector<double>(new double[]{ 1.3, 1.3, 1.3, 13.0, 130.0 }),
                    new SingleValueFeatureVector<double>(new double[]{ 1.2, 1.2, 1.2, 12.0, 120.0 }),
                    new SingleValueFeatureVector<double>(new double[]{ 1.6, 1.6, 1.6, 16.0, 160.0 }),
                    new SingleValueFeatureVector<double>(new double[]{ 3.3, 3.3, 3.3, 33.0, 330.0 }),
                    new SingleValueFeatureVector<double>(new double[]{ 3.1, 3.1, 3.1, 31.0, 310.0 }),
                    new SingleValueFeatureVector<double>(new double[]{ 3.8, 3.8, 3.8, 38.0, 380.0 }),
                    new SingleValueFeatureVector<double>(new double[]{ 3.5, 3.5, 3.5, 35.0, 350.0 }),
                    new SingleValueFeatureVector<double>(new double[]{ 3.9, 3.9, 3.9, 39.0, 390.0 }),
                    new SingleValueFeatureVector<double>(new double[]{ 6.1, 6.1, 6.1, 61, 610}),
                    new SingleValueFeatureVector<double>(new double[]{ 6.2, 6.2, 6.2, 62, 620}),
                    new SingleValueFeatureVector<double>(new double[]{ 6.3, 6.3, 6.3, 63, 630}),
                    new SingleValueFeatureVector<double>(new double[]{ 6.5, 6.5, 6.5, 65, 650}),
                    new SingleValueFeatureVector<double>(new double[]{ 6.8, 6.8, 6.8, 68, 680})
                });

            var vectorToClassify = new SingleValueFeatureVector<double>(new double[] { 3.65, 3.65, 3.65, 36.5, 0 }, 4);

            Func<double, double> weightFunction = (distance) => StatisticalFunctions.GaussianCurve(distance);
            var subject = new WeightedKnnClassifier(DistanceFunctions.EuclideanVectorsDistance, 3, weightFunction);
            var expected = new double[] {37.33, 373.33};

            //When
            double result = subject.Classify(vectorToClassify, fetauresSet);

            //Then
            Assert.AreEqual(373.44, result, 0.009);

        }
    }
}
