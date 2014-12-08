using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    [TestClass()]
    public class EntropyMeasurerTests
    {
        public IEntopyMeasurer<object> Subject { get; set; }

        [TestInitialize]
        public void InitializeSubject()
        {
            this.Subject = new EntropyMeasurer<object>(
                ChaosMeasureFunctions.GiniImpurity,
                StatisticalFunctions.Variance,
                false
                );
        }
           
        [TestMethod()]
        public void MeasureEntropyOnAxisTest_NumbericTypeConcludedFromData()
        {
            //Given
            var dataSet = TestDataBuilder.AbstractTestData_NumbericValues();
            
            //When
            var entropy = this.Subject.MeasureEntropyOnAxis(dataSet, 3);

            //Then
            Assert.AreEqual(2.29, entropy, 0.009);
        }

        [TestMethod()]
        public void MeasureEntropyOnAxisTest_DiscreteData()
        {
            //Given
            var dataSet = TestDataBuilder.AbstractTestData_DiscreteValues();

            //When
            var entropy = this.Subject.MeasureEntropyOnAxis(dataSet, 2);

            //Then
            Assert.AreEqual(0.66, entropy, 0.009);
        }
    }
}
