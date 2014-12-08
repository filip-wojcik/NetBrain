using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions.Regression;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Data.Models;
using NetBrain.Defaults.MachineLearning.MathematicalFunctions.Regression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace NetBrain.Defaults.MachineLearning.MathematicalFunctions.Regression.Tests
{
    [TestClass()]
    public class PolynomialFunctionTests
    {
        internal IRegressionFunction Subject;
        internal ISingleValueDataSet<double> TestDataSet;
        
        [TestInitialize]
        public void SetUpSubject()
        {
            this.Subject = new PolynomialFunction();
            this.TestDataSet = new SingleValueDataSet<double>(
                new[] { "e1", "e2", "e3", "e4" }, 4, 3, new List<IFeatureVector<double>>
                {
                    new SingleValueFeatureVector<double>(new double[]{ 1, 2, 3, 14 }),
                    new SingleValueFeatureVector<double>(new double[]{ 4, 5, 6, 32 }),
                });
        }

        [TestMethod()]
        public void CalculateOutput_OnSingleVector_Test()
        {
            //Given
            var data = new double[] { 1, 2, 3 };
            var weights = new double[] { 1, 2, 3 };
            var expected = 14.0;

            //When
            double actual = this.Subject.CalculateOutput(data, weights);

            //Then
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CalculateOutput_OnDataSet_Test()
        {
            //Given
            var weights = new double[] { 1, 2, 3 };
            var expected = new double[] {14.0, 32.0};

            //When
            IList<double> actual =
                Subject.CalculateOutput(TestDataSet.NonValueVectorsSet as ISingleValueDataSet<double>, weights);

            //Then
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
        }

        [TestMethod()]
        public void CalculateDerivatives_OnDataSet_Test()
        {
            //Given
            var weights = new double[] { 1, 1, 1 };
            var expectedYVals = new double[] { 14.0, 32.0 };
            var actualYVals = new double[] { 3.0, 7.5 };
            var expectedDerivatives = new double[] { -54.50, -72.25, -90.00 };

            //When

            var actualDerivatives = Subject.CalculateDerivativeWithRespectToWeights(expectedYVals, actualYVals,
                TestDataSet.NonValueVectorsSet as ISingleValueDataSet<double>, 3);

            //Then
            Assert.IsTrue(expectedDerivatives.SequenceEqual(actualDerivatives));
        }
    }
}
