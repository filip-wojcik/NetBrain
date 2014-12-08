using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    [TestClass()]
    public class BinaryDataSplitterTests
    {
        public IDataSplitter<object, bool> Subject { get; set; }

        [TestInitialize]
        public void SetupSubject()
        {
            this.Subject = new BinaryDataSplitter<object>(StatisticalFunctions.Mean, () => true, () => false);
        }

        [TestMethod()]
        public void SplitFeatureVectorsTest()
        {
            //Given
            var dataSet = TestDataBuilder.BuildMixedDomain_TransportationExample();
            var splitOption = new SplitOption<object>(0, "Gender", false, true, true, "Male");

            //When
            IList<ISplittingResult<object, bool>> splittedData = this.Subject.SplitFeatureVectors(dataSet,
                splitOption).ToList();

            //Then
            Assert.AreEqual(2, splittedData.Count);
            Assert.IsTrue(
                splittedData.Any(
                    result => result.Value.Equals(true) &&
                    result.SingleValuesDataSet.Count().Equals(5) &&
                    result.SingleValuesDataSet.All(vector => vector[0].Equals("Male")))
                );
            Assert.IsTrue(
                splittedData.Any(
                    result => result.Value.Equals(false) &&
                    result.SingleValuesDataSet.Count().Equals(5) &&
                    result.SingleValuesDataSet.All(vector => vector[0].Equals("Female")))
            );

        }

        [TestMethod()]
        public void SplitOnNumbericDataTest()
        {
            //Given
            var dataSet = TestDataBuilder.BuildMixedDomain_TransportationExample();
            var splitOption = new SplitOption<object>(1, "Car ownership", true, false, true);

            //When
            IList<ISplittingResult<object, bool>> splittedData = this.Subject.SplitFeatureVectors(dataSet,
                splitOption).ToList();

            //Then
            Assert.AreEqual(2, splittedData.Count);
            Assert.IsTrue(
               splittedData.Any(
                   result => result.Value.Equals(true) &&
                   result.SingleValuesDataSet.Count().Equals(7) &&
                   result.SingleValuesDataSet.All(vector => Convert.ToDouble(vector[1]) > 0.9))

               );
            Assert.IsTrue(
                splittedData.Any(
                    result => result.Value.Equals(false) &&
                              result.SingleValuesDataSet.Count().Equals(3) &&
                              result.SingleValuesDataSet.All(vector => Convert.ToDouble(vector[1]) < 0.9))
                );
        }


        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SplitFeatureVectorsTest_NoConcreteSplitValueSet_ExpectedException()
        {
            //Given
            var dataSet = TestDataBuilder.BuildMixedDomain_TransportationExample();
            var splitOption = new SplitOption<object>(0, "Gender", false, true, true, null);

            //When
            IList<ISplittingResult<object, bool>> splittedData = this.Subject.SplitFeatureVectors(dataSet,
                splitOption).ToList();
        }

        [TestMethod()]
        public void GenerateSplitOptionsForAxis_DiscreteValuesTest()
        {
            //Given
            var dataSet = TestDataBuilder.BuildMixedDomain_TransportationExample();
            var expectedSplitOptions = new ISplitOption<object>[]
            {
                new SplitOption<object>(
                    splitAxis: 2, 
                    splitLabel: "Travel cost ($/km)", 
                    isDataNumberic: false, 
                    splitOnConcreteValue: true, 
                    isSplitBinary: true, 
                    concreteValueToSplit: "Expensive"), 

                new SplitOption<object>(
                    splitAxis: 2, 
                    splitLabel: "Travel cost ($/km)", 
                    isDataNumberic: false, 
                    splitOnConcreteValue: true, 
                    isSplitBinary: true, 
                    concreteValueToSplit: "Expensive"), 
                new SplitOption<object>(
                    splitAxis: 2, 
                    splitLabel: "Travel cost ($/km)", 
                    isDataNumberic: false, 
                    splitOnConcreteValue: true, 
                    isSplitBinary: true, 
                    concreteValueToSplit: "Expensive")
            };

            //When
            var results = this.Subject.GenerateSplitOptionsForAxis(dataSet, 2);

            //Then
            Assert.AreEqual(3, results.Count());
            Assert.IsTrue(expectedSplitOptions.All(option => results.Contains(option)));
        }

        [TestMethod()]
        public void GenerateSplitOptionsForAxis_NumbericValuesTest()
        {
            //Given
            var dataSet = TestDataBuilder.BuildMixedDomain_TransportationExample();
            var expectedSplitOptions = new ISplitOption<object>[]
            {
                new SplitOption<object>(
                    splitAxis: 1,
                    splitLabel: "Car ownership",
                    isDataNumberic: true,
                    splitOnConcreteValue: true,
                    isSplitBinary: true,
                    concreteNumbericValueToSplit: 0.9
                    )
            };

            //When
            var splitOptions = this.Subject.GenerateSplitOptionsForAxis(dataSet, 1);

            //Then
            Assert.IsTrue(expectedSplitOptions.SequenceEqual(splitOptions));
        }
    }
}
