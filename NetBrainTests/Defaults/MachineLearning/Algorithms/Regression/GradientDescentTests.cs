using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Algorithms.Regression;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Abstracts.MachineLearning.QualityCheckers;
using NetBrain.Defaults.MachineLearning.Data.Models;
using NetBrain.Defaults.MachineLearning.MathematicalFunctions.Regression;
using NetBrainTests.Defaults.MachineLearning.Algorithms.Regression;
using NetBrainTests.Defaults.MachineLearning.QualityCheckers;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Regression.Tests
{
    [TestClass()]
    public class GradientDescentTests
    {
        internal IRegressor Subject;
        internal SingleValueDataSet<double> TrivialTestDataSet;
        internal RegressionTestDataBuilder TestDataBuilder;

        [TestInitialize]
        public void Setup()
        {
            this.TrivialTestDataSet = new SingleValueDataSet<double>(
                new[] { "e1", "e2", "e3", "e4" }, 4, 3, new List<IFeatureVector<double>>
                {
                    new SingleValueFeatureVector<double>(new double[]{ 1, 2, 3, 14 }),
                    new SingleValueFeatureVector<double>(new double[]{ 4, 5, 6, 32 }),
                });
            this.TestDataBuilder = new RegressionTestDataBuilder();
        }

        [TestMethod()]
        public void GradientDescent_TrivialDataSet_Test()
        {
            // Given
            var logger = new QualityCheckerLoggerStub();
            var expectedWeights = new double[] { 1.032, 2.00, 2.97 };
            this.Subject = new GradientDescent(new PolynomialFunction(), StatisticalFunctions.RootMeanSquareErrorFunction, logger: logger);

            // When
            var actualWeights = this.Subject.PredictWeights(this.TrivialTestDataSet);

            // Then
            for (int weightIdx = 0; weightIdx < expectedWeights.Length; weightIdx++)
            {
                Assert.AreEqual(expectedWeights[weightIdx], actualWeights[weightIdx], 0.009);
            }
            double error = double.MaxValue;
            for (int iterationNo = 0; iterationNo < logger.Results.Count; iterationNo++)
            {
                IQualityData qualityData = logger.Results[iterationNo];
                double currentIterationError = qualityData.ErrorRate;
                Assert.IsTrue(currentIterationError < error);
                error = currentIterationError;
            }
        }

        [TestMethod]
        public void GradientDescent_BiggerDataSet_Test()
        {
            // Given
            var logger = new QualityCheckerLoggerStub();
            this.Subject = new GradientDescent(new PolynomialFunction(), StatisticalFunctions.RootMeanSquareErrorFunction, logger: logger, learningRate: 0.02);
            
            // When
            var actualWeights = this.Subject.PredictWeights(TestDataBuilder.TrainingDataSet);

            // Then
            IList<double> actualOutputs =
                this.Subject.RegressionFunction.CalculateOutput(TestDataBuilder.TestDataSet, actualWeights);
            var queryError = StatisticalFunctions.RootMeanSquareErrorFunction(TestDataBuilder.IdealTestOutputs, actualOutputs);
            Assert.IsTrue(queryError < 10);

            double error = double.MaxValue;
            for (int iterationNo = 0; iterationNo < logger.Results.Count; iterationNo++)
            {
                IQualityData qualityData = logger.Results[iterationNo];
                double currentIterationError = qualityData.ErrorRate;
                Assert.IsTrue(currentIterationError < error);
                error = currentIterationError;
            }
        }
    }
}
