using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.QualityCheckers;
using NetBrain.Defaults.MachineLearning.QualityCheckers;

namespace NetBrainTests.Defaults.MachineLearning.QualityCheckers
{
    [TestClass()]
    public class ClassificationQualityCheckerTests
    {
        [TestMethod()]
        public void AccuracyRateTest_CalculateAccuracyFromRawData()
        {
            //Given
            var subject = new ClassificationQualityChecker<double>();
            double expectedAccuracy = 0.75;
            double expectedErrorRate = 0.25;

            //When
            double actualAccuracy = subject.AccuracyRate(this.TestData());
            double actualErrorRate = subject.ErrorRate(this.TestData());

            //Then
            Assert.AreEqual(expectedAccuracy, actualAccuracy);
            Assert.AreEqual(expectedErrorRate, actualErrorRate);
        }

        [TestMethod()]
        public void ContingencyTableTest()
        {
            //Given
            var subject = new ClassificationQualityChecker<double>();

            uint firstClassRealCountExpected = 75;
            uint firstClassTruePositivesExpected = 60;
            uint firstClassFalsePositivesExpected = 15;

            uint secondClassRealCountExpected = 25;
            uint secondClassTruePositivesExpected = 15;
            uint secondClassFalsePositivesExpected = 10;

            //When
            IContingencyTable<double> contingencyTable = subject.BuildContingencyTable(this.TestData());

            //Then
            Assert.AreEqual(firstClassRealCountExpected, contingencyTable.RealClassesCounts[0]);
            Assert.AreEqual(firstClassTruePositivesExpected, contingencyTable.ContingencyValues[0, 0]);
            Assert.AreEqual(firstClassFalsePositivesExpected, contingencyTable.ContingencyValues[0, 1]);

            Assert.AreEqual(secondClassRealCountExpected, contingencyTable.RealClassesCounts[1]);
            Assert.AreEqual(secondClassTruePositivesExpected, contingencyTable.ContingencyValues[1, 1]);
            Assert.AreEqual(secondClassFalsePositivesExpected, contingencyTable.ContingencyValues[1, 0]);
        }

        [TestMethod()]
        public void AccuracyFromContingencyTableTest()
        {
            //Given
            var subject = new ClassificationQualityChecker<double>();
            double expectedAccuracy = 0.75;

            //When
            IContingencyTable<double> contingencyTable = subject.BuildContingencyTable(this.TestData());
            double actualAccuracy = subject.AccuracyRate(contingencyTable);

            //Then
            Assert.AreEqual(expectedAccuracy, actualAccuracy);
        }

        /// <summary>
        /// Examples of classification results based on the book "Machine Learning" Peter Flach, Cambridge University Press 2012
        /// page 56
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<IExpectedActualPair<double>> TestData()
        {
            var results = new List<IExpectedActualPair<double>>();

            //True positives for class 1
            for (int i = 0; i < 60; i++)
            {
                results.Add(new ExpectedActualPair<double>(
                    new List<double>(){ 1.0, 0.0 }, 
                    new List<double>(){ 1.0, 0.0 } 
                    ));
            }

            //False positives for class 1
            for (int i = 0; i < 15; i++)
            {
                results.Add(new ExpectedActualPair<double>(
                    new List<double>() { 1.0, 0.0 },
                    new List<double>() { 0.0, 1.0 }
                    ));
            }

            //True positives for class 2
            for (int i = 0; i < 15; i++)
            {
                results.Add(new ExpectedActualPair<double>(
                    new List<double>() { 0.0, 1.0 },
                    new List<double>() { 0.0, 1.0 }
                    ));
            }

            //False positives for class 2
            for (int i = 0; i < 10; i++)
            {
                results.Add(new ExpectedActualPair<double>(
                    new List<double>() { 0.0, 1.0 },
                    new List<double>() { 1.0, 0.0 }
                    ));
            }

            return results;
        }
    }
}
