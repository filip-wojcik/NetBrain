using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    [TestClass()]
    public class DecisionTreeBuilderTests
    {
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
        }

        # region Tests with multi feature splitter

        [TestMethod()]
        public void ChooseBestOptionToSplitTest()
        {
            //Given
            var dataSet = TestDataBuilder.BuildMixedDomain_TransportationExample();
            var subject = this.MultiFeatureDecisionTreeBuilder;
            var expectedSplitOption = new SplitOption<object>(
                splitAxis: 2,
                splitLabel: "Travel cost ($/km)",
                isDataNumberic: false,
                isSplitBinary: false,
                splitOnConcreteValue: false
                );

            //When
            ISplitOption<object> bestSplitOption = subject.BestSplitSelector.ChooseBestSplitOption(dataSet, subject.DataSplitter);

            //Then
            Assert.AreEqual(expectedSplitOption, bestSplitOption);
            
        }

        [TestMethod]
        public void BuildDecisionTreeTest_TransportationExamples()
        {
            //Given
            var dataSet = TestDataBuilder.BuildMixedDomain_TransportationExample();
            var subject = this.MultiFeatureDecisionTreeBuilder;
            
            //When
            var times = new List<long>();
            for (int i = 0; i < 100; i++)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                IDecisionTree<object, object> dt = subject.BuildDecisionTree(dataSet);
                stopWatch.Stop();
                long time = stopWatch.ElapsedMilliseconds;
                times.Add(time);
            }
            var avg = times.Sum()/times.Count;

            IDecisionTree<object, object> decisionTree = subject.BuildDecisionTree(dataSet);

            //Then
            Assert.AreEqual("Travel cost ($/km)", decisionTree.SplitOption.SplitLabel);
            Assert.AreEqual(3, decisionTree.Children.Count());
            
            //Leaf children
            Assert.IsTrue(decisionTree.ChildrenWithValues.Any(
                childWithValue =>
                    childWithValue.ChildValue.Equals("Expensive") && childWithValue.ChildTree.IsLeaf &&
                    childWithValue.ChildTree.Value.Equals("Car")));

            Assert.IsTrue(decisionTree.ChildrenWithValues.Any(
                childWithValue =>
                    childWithValue.ChildValue.Equals("Standard") && childWithValue.ChildTree.IsLeaf &&
                    childWithValue.ChildTree.Value.Equals("Train")));

            //Decision tree child
            var genderChild =
                decisionTree.ChildrenWithValues.FirstOrDefault(childWithValue => childWithValue.ChildValue.Equals("Cheap")).ChildTree as IDecisionTree<object, object>;
            Assert.IsNotNull(genderChild);
            Assert.IsFalse(genderChild.SplitOption.SplitOnConcreteValue);
            Assert.AreEqual("Gender", genderChild.SplitOption.SplitLabel);

            //Gender node first child
            Assert.IsTrue(genderChild.ChildrenWithValues.Any(
                childWithValue => 
                    childWithValue.ChildValue.Equals("Male") && childWithValue.ChildTree.IsLeaf &&
                    childWithValue.ChildTree.Value.Equals("Bus")
                ));

            //Gender node second child
            var carOwnershipNode =
                genderChild.ChildrenWithValues.FirstOrDefault(childWithValue => childWithValue.ChildValue.Equals("Female"))
                    .ChildTree as IDecisionTree<object, object>;
            Assert.IsNotNull(carOwnershipNode);
            Assert.IsTrue(carOwnershipNode.SplitOption.SplitOnConcreteValue);
            Assert.AreEqual(0.5, carOwnershipNode.SplitOption.ConcreteNumbericValueToSplit);

            //Car ownership node children
            carOwnershipNode.ChildrenWithValues.Any(
                childWithValue =>
                    childWithValue.ChildValue.Equals(0) && childWithValue.ChildTree.IsLeaf &&
                    childWithValue.ChildTree.Value.Equals("Bus"));
            carOwnershipNode.ChildrenWithValues.Any(
                childWithValue =>
                    childWithValue.ChildValue.Equals(1) && childWithValue.ChildTree.IsLeaf &&
                    childWithValue.ChildTree.Value.Equals("Train"));
        }

        # endregion Tests with multi feature splitter

        # region Tests with binary decision tree splitter

        [TestMethod]
        public void ChooseBestFeatureToSplitTest_WebBrowsersExample_BinarySplit()
        {
            //Given
            var dataSet = TestDataBuilder.BuildWebBrowsersDataSet();
            var subject = this.BinaryDecisionTreeBuilder;

            //When
            ISplitOption<object> splitOption = subject.BestSplitSelector.ChooseBestSplitOption(dataSet, subject.DataSplitter);

            //Then
            Assert.AreEqual("google", splitOption.ConcreteValueToSplit);
        }

        [TestMethod]
        public void BuildDecisionTreeAnimalsExample()
        {
            //Given
            var dataSet = TestDataBuilder.BuildAnimalsTestData();
            var subject = this.BinaryDecisionTreeBuilder;
            
            //When
            IDecisionTree<object, bool> decisionTree = subject.BuildDecisionTree(dataSet);

            //Then
            var expectedRootSplitOption = new SplitOption<object>(
                splitAxis: 0,
                splitLabel: "can survive without coming on the surface",
                isDataNumberic: false,
                splitOnConcreteValue: true,
                isSplitBinary: true,
                concreteValueToSplit: true);
            Assert.AreEqual(expectedRootSplitOption, decisionTree.SplitOption);
            var children = decisionTree.ChildrenWithValues.ToList();

            //False child
            var falseChild = children.FirstOrDefault(child => child.ChildValue == false).ChildTree as IDecisionTree<object, bool>;
            var firstChildVectors = new HashSet<IFeatureVector<object>> { dataSet[3], dataSet[4] };
            Assert.IsTrue(falseChild.IsLeaf);
            Assert.IsTrue(firstChildVectors.SetEquals(falseChild.FeatureVectors));

            //True child
            var trueChild = children.FirstOrDefault(child => child.ChildValue == true).ChildTree as IDecisionTree<object, bool>;
            var trueChildSplit = new SplitOption<object>(
                splitAxis: 1,
                splitLabel: "has flippers",
                isDataNumberic: false,
                splitOnConcreteValue: true,
                isSplitBinary: true,
                concreteValueToSplit: true);
            Assert.IsFalse(trueChild.IsLeaf);
            Assert.AreEqual(trueChildSplit, trueChild.SplitOption);
            var trueChildChildren = trueChild.ChildrenWithValues.ToList();

            //False child of true child
            var falseChildOfTrueChild =
                trueChildChildren.FirstOrDefault(child => child.ChildValue == false).ChildTree as IDecisionTree<object, bool>;
            var falseChildOfTrueChildVectors = new HashSet<IFeatureVector<object>> { dataSet[2] };
            Assert.IsTrue(falseChildOfTrueChild.IsLeaf);
            Assert.IsTrue(falseChildOfTrueChildVectors.SetEquals(falseChildOfTrueChild.FeatureVectors));

            //True child of true child
            var trueChildOfTrueChild =
                trueChildChildren.FirstOrDefault(child => child.ChildValue == true).ChildTree as IDecisionTree<object, bool>;
            var trueChildOfTrueChildVectors = new HashSet<IFeatureVector<object>> { dataSet[0], dataSet[1] };
            Assert.IsTrue(trueChildOfTrueChild.IsLeaf);
            Assert.IsTrue(trueChildOfTrueChildVectors.SetEquals(trueChildOfTrueChild.FeatureVectors));
        }

        # endregion Tests with binary decision tree splitter

    }
}
