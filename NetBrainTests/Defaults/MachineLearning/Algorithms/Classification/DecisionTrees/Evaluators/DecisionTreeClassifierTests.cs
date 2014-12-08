using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    [TestClass()]
    public class DecisionTreeClassifierTests
    {
        public IDecisionTreeClassifier<object, bool> Subject { get; set; } 
        public IDecisionTreeBuilder<object, object> MultiFeatureDecisionTreeBuilder { get; set; }
        public IDecisionTreeBuilder<object, bool> BinaryDecisionTreeBuilder { get; set; }

        [TestInitialize]
        public void InitializeBuilders()
        {
            var entropyMeasurer = new EntropyMeasurer<object>(ChaosMeasureFunctions.ShannonEntropy,
                StatisticalFunctions.Variance);

            this.MultiFeatureDecisionTreeBuilder = new DecisionTreeBuilder<object, object>(
                new MultiValueDataSplitter<object>(StatisticalFunctions.Mean, () => true, () => false),
                new BestSplitSelector<object, object>(entropyMeasurer, false),
                () => true,
                () => false
            );

            this.BinaryDecisionTreeBuilder = new DecisionTreeBuilder<object, bool>(
                new BinaryDataSplitter<object>(StatisticalFunctions.Mean, () => true, () => false),
                new BestSplitSelector<object, bool>(entropyMeasurer, true),
                () => true,
                () => false
            );

            this.Subject = new DecisionTreeClassifier<object, bool>(() => true, () => false, (value, linkValue) => value.Equals(linkValue));
        }

        # region Binary decision trees evaluation

        [TestMethod]
        public void ClassifyWithBinaryTreeTest()
        {
            //Given
            var data = TestDataBuilder.BuildWebBrowsersDataSet();
            var decisionTree = this.BinaryDecisionTreeBuilder.BuildDecisionTree(data);
            var vectorToClassify =
                new SingleValueFeatureVector<object>(new List<object>() {"(direct)", "USA", "yes", 5, null});
            var expected = "Basic";

            //When
            object result = this.Subject.Classify(vectorToClassify, decisionTree);

            //Then
            Assert.AreEqual(expected, result);
        }

        # endregion Binary decision trees evaluation

        # region Multi split trees tests

        //TODO: add tests here

        # endregion Multi split trees tests

        # region Uncertainity handling

        [TestMethod]
        public void SelectValueBasedOnTheWeightedVectorsCount()
        {
            //Given
            var decisionTree = TestDataBuilder.BuildAnimalsExampleDecisionTree();
            var vectorToClassify = new SingleValueFeatureVector<object>(new object[] {null, null, null});

            //When
            var value = (bool)this.Subject.Classify(vectorToClassify, decisionTree);

            //(No surfacing) node (split axis 0) has 3 vectors in >>yes<< branch vs 2 vectors in >>no<< branch, so null under axis 0 will be handled by yes
            //(Flippers) node (split axis 1) has 1 vector in >>no<< branch vs 2 vectors in >>yes<< branch, so null under axis 1 will be handled by yes

            //Then
            Assert.IsTrue(value);
        }

        # endregion Uncertainity handling
    }
}
