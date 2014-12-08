using System.Collections.Generic;
using System.Diagnostics;
using NetBrain.Abstracts.MachineLearning.QualityCheckers;
using NetBrain.Defaults.MachineLearning.QualityCheckers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetBrain.Defaults.MachineLearning.QualityCheckers.Tests
{
    [TestClass()]
    public class RootMeanSquareErrorCheckerTests
    {
        [TestMethod()]
        public void ErrorRateTest()
        {
            // Given
            var outcomes = new List<IExpectedActualPair<double>>
            {
                new ExpectedActualPair<double>(new double[] { 2 }, new double[] { 9 }),
                new ExpectedActualPair<double>(new double[] { 3 }, new double[] { 8 }),
                new ExpectedActualPair<double>(new double[] { 4 }, new double[] { 7 }),
                new ExpectedActualPair<double>(new double[] { 5 }, new double[] { 6 })
            };
            var subeject = new RootMeanSquareErrorChecker();
            double expectedError = 4.582;

            //When
            double error = subeject.ErrorRate(outcomes);

            //Then
            Assert.AreEqual(expectedError, error, 0.001);
        }
    }
}
