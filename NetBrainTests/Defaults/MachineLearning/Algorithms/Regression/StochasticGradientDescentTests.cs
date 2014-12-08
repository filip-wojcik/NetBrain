using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Algorithms.Regression;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Defaults.MachineLearning.MathematicalFunctions.Regression;
using NetBrainTests.Defaults.MachineLearning.Algorithms.Regression;
using NetBrainTests.Defaults.MachineLearning.QualityCheckers;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Regression.Tests
{
    [TestClass()]
    public class StochasticGradientDescentTests
    {
        internal IRegressor Subject;
        internal RegressionTestDataBuilder TestDataBuilder;

        [TestInitialize]
        public void Setup()
        {
            this.TestDataBuilder = new RegressionTestDataBuilder();
        }

        [TestMethod()]
        public void StochasticGradientDescentTest()
        {
            // Given
            var logger = new QualityCheckerLoggerStub();
            this.Subject = new StochasticGradientDescent(new PolynomialFunction(), StatisticalFunctions.RootMeanSquareErrorFunction, logger: logger, iterations: 500);

            // When
            var actualWeights = this.Subject.PredictWeights(TestDataBuilder.TrainingDataSet);

            // Then
            List<double> results = logger.Results.Select(result => result.ErrorRate).ToList();
            int elementsInQuartiles = results.Count / 4;
            List<double> quartilesValues = Enumerable.Range(0, 4).Select(idx => Math.Abs(results[idx*elementsInQuartiles])).ToList();
            double lastError = double.MaxValue;
            for(int quartileIdx = 0; quartileIdx < quartilesValues.Count; quartileIdx++)
            {
                double quartileValue = quartilesValues[quartileIdx];
                Assert.IsTrue(quartileValue < lastError);
                lastError = quartileValue;
            }
        }
    }
}
