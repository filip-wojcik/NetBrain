using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Regression;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Abstracts.MachineLearning.QualityCheckers;
using NetBrain.Defaults.MachineLearning.Algorithms.Regression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Defaults.MachineLearning.Data.Models;
using NetBrain.Defaults.MachineLearning.MathematicalFunctions.Regression;
using NetBrainTests.Defaults.MachineLearning.Algorithms.Regression;
using NetBrainTests.Defaults.MachineLearning.QualityCheckers;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Regression.Tests
{
    [TestClass()]
    public class RegularizedGradientDescentTests
    {
        internal IRegressor Subject;
        internal RegressionTestDataBuilder TestDataBuilder;

        [TestInitialize]
        public void Setup()
        {
            this.TestDataBuilder = new RegressionTestDataBuilder();
        }

        [TestMethod]
        public void RegularizedGradientDescent_Test()
        {
            // Given
            var logger = new QualityCheckerLoggerStub();
            this.Subject = new RegularizedGradientDescent(new PolynomialFunction(), StatisticalFunctions.RootMeanSquareErrorFunction, logger: logger, learningRate: 0.01, lambda: 0.001);

            // When
            var actualWeights = this.Subject.PredictWeights(TestDataBuilder.TrainingDataSet);

            // Then
            IList<double> actualOutputs =
                this.Subject.RegressionFunction.CalculateOutput(TestDataBuilder.TestDataSet, actualWeights);
            var queryError = StatisticalFunctions.RootMeanSquareErrorFunction(TestDataBuilder.IdealTestOutputs, actualOutputs);
            Assert.IsTrue(queryError < 9);

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
