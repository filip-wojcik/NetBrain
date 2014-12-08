using System.Collections.Generic;
using Moq;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Utils;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Defaults.MachineLearning.Data.Models;
using NetBrain.Abstracts.Graphs.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees
{
    public static class TestDataBuilder
    {
        /// <summary>
        /// Builds test data basing on examples from "Machine Learning in Action"
        /// </summary>
        /// <returns></returns>
        public static ISingleValueDataSet<object> BuildAnimalsTestData()
        {
            //Given
            var vectorsSet = new SingleValueDataSet<object>(
                new string[]
                {
                    "can survive without coming on the surface", 
                    "has flippers", 
                    "is a fish"
                },
                3, 2);

            vectorsSet.AddVector(
                new SingleValueFeatureVector<object>(
                    new object[] { true, true, true }
                    )
                );

            vectorsSet.AddVector(
                new SingleValueFeatureVector<object>(
                    new object[] { true, true, true }
                ));


            vectorsSet.AddVector(
                new SingleValueFeatureVector<object>(
                    new object[] { true, false, false }
            ));


            vectorsSet.AddVector(
                new SingleValueFeatureVector<object>(
                    new object[] { false, true, false }
            ));


            vectorsSet.AddVector(
                new SingleValueFeatureVector<object>(
                    new object[] { false, true, false }
            ));

            return vectorsSet;
        }

        public static ISingleValueDataSet<object> AbstractTestData_NumbericValues()
        {
            var dataSet = new SingleValueDataSet<object>(new string[0], 4, 3,
                    new ISingleValueFeatureVector<object>[]{
                          new SingleValueFeatureVector<object>(new object[] { true, "X", "A", 2 }),
                          new SingleValueFeatureVector<object>(new object[] { false, "Y", "A", 5 }),
                          new SingleValueFeatureVector<object>(new object[] { false, "Y", "A", 1 }),
                          new SingleValueFeatureVector<object>(new object[] {true, "X", "A", 3 }),
                          new SingleValueFeatureVector<object>(new object[] { true, "X", "B", 2 }),
                          new SingleValueFeatureVector<object>(new object[] { false, "Y", "B", 5 }),
                          new SingleValueFeatureVector<object>(new object[] { true, "X", "B", 1 }),
                          new SingleValueFeatureVector<object>(new object[] { false, "Y", "C", 3 }),
                          new SingleValueFeatureVector<object>(new object[] { false, "Y", "C", 2 }),
                          new SingleValueFeatureVector<object>(new object[] { true, "Y", "C", 5 })
                });

            return dataSet;
        }

        public static ISingleValueDataSet<object> AbstractTestData_DiscreteValues()
        {
            var dataSet = new SingleValueDataSet<object>(new string[0], 4, 2,
                    new ISingleValueFeatureVector<object>[]{
                          new SingleValueFeatureVector<object>(new object[] { true, "X", "A", 2 }),
                          new SingleValueFeatureVector<object>(new object[] { false, "Y", "A", 5 }),
                          new SingleValueFeatureVector<object>(new object[] { false, "Y", "A", 1 }),
                          new SingleValueFeatureVector<object>(new object[] {true, "X", "A", 3 }),
                          new SingleValueFeatureVector<object>(new object[] { true, "X", "B", 2 }),
                          new SingleValueFeatureVector<object>(new object[] { false, "Y", "B", 5 }),
                          new SingleValueFeatureVector<object>(new object[] { true, "X", "B", 1 }),
                          new SingleValueFeatureVector<object>(new object[] { false, "Y", "C", 3 }),
                          new SingleValueFeatureVector<object>(new object[] { false, "Y", "C", 2 }),
                          new SingleValueFeatureVector<object>(new object[] { true, "Y", "C", 5 })
                });

            return dataSet;
        }


        /// <summary>
        /// Example data taken form http://people.revoledu.com/kardi/tutorial/DecisionTree/how-decision-tree-algorithm-work.htm,
        /// great tutorial about machine learning and decision trees
        /// </summary>
        public static ISingleValueDataSet<object> BuildMixedDomain_TransportationExample()
        {
            var dataset = new SingleValueDataSet<object>(
                new string[] {"Gender", "Car ownership", "Travel cost ($/km)", "Income level", "Transportation mode"},
                5, 4, new List<ISingleValueFeatureVector<object>>()
                {
                    new SingleValueFeatureVector<object>(new object[] {"Male", 0, "Cheap", "Low", "Bus"}),
                    new SingleValueFeatureVector<object>(new object[] {"Male", 1, "Cheap", "Medium", "Bus"}),
                    new SingleValueFeatureVector<object>(new object[] {"Female", 1, "Cheap", "Medium", "Train"}),
                    new SingleValueFeatureVector<object>(new object[] {"Female", 0, "Cheap", "Low", "Bus"}),
                    new SingleValueFeatureVector<object>(new object[] {"Male", 1, "Cheap", "Medium", "Bus"}),
                    new SingleValueFeatureVector<object>(new object[] {"Male", 0, "Standard", "Medium", "Train"}),
                    new SingleValueFeatureVector<object>(new object[] {"Female", 1, "Standard", "Medium", "Train"}),
                    new SingleValueFeatureVector<object>(new object[] {"Female", 1, "Expensive", "High", "Car"}),
                    new SingleValueFeatureVector<object>(new object[] {"Male", 2, "Expensive", "Medium", "Car"}),
                    new SingleValueFeatureVector<object>(new object[] {"Female", 2, "Expensive", "High", "Car"})
                });
            return dataset;
        }

        /// <summary>
        /// Example taken from Progamming collective intelligence: Building smart Web 2.0 applications"
        /// </summary>
        /// <returns></returns>
        public static ISingleValueDataSet<object> BuildWebBrowsersDataSet()
        {
            var dataSet =
                new SingleValueDataSet<object>(new string[]
                    {"Referer", "Country", "Have read FAQ", "Pages visited", "Service selected"}, 5, 4,
                    new SingleValueFeatureVector<object>[]
                    {
                        new SingleValueFeatureVector<object>(new List<object>() {"slashdot","USA","yes",18,"None" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "google", "France", "yes", 23, "Premium" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "digg", "USA", "yes", 24, "Basic" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "kiwitobes", "France", "yes", 23, "Basic" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "google", "UK", "no", 21, "Premium" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "(direct)", "New Zealand", "no", 12, "None" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "(direct)", "UK", "no", 21, "Basic" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "google", "USA", "no", 24, "Premium" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "slashdot", "France", "yes", 19, "None" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "digg", "USA", "no", 18, "None" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "google", "UK", "no", 18, "None" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "kiwitobes", "UK", "no", 19, "None" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "digg", "New Zealand", "yes", 12, "Basic" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "slashdot", "UK", "no", 21, "None" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "google", "UK", "yes", 18, "Basic" }),
                        new SingleValueFeatureVector<object>(new List<object>() { "kiwitobes", "France", "yes", 19, "Basic" })
                    }
                );

            return dataSet;
        }

        public static IDecisionTree<object, object> BuildMockTree()
        {
            //Given
            var treeRoot = new Mock<IDecisionTree<object, object>>();
            treeRoot.Setup(root => root.IsLeaf).Returns(false);

            var child1 = new Mock<IDecisionTree<object, object>>();
            child1.Setup(child => child.IsLeaf).Returns(false);

            var child2 = new Mock<IDecisionTree<object, object>>();
            child2.Setup(child => child.IsLeaf).Returns(false);

            var child3 = new Mock<IDecisionTree<object, object>>();
            child3.Setup(child => child.IsLeaf).Returns(true);
            child3.Setup(child => child.FeatureVectors).Returns(
                new List<ISingleValueFeatureVector<object>>()
                {
                    new SingleValueFeatureVector<object>(new object[]{"a", "b", "c"}),
                    new SingleValueFeatureVector<object>(new object[] { "a", "b", "c" }),
                    new SingleValueFeatureVector<object>(new object[] { "a", "b", "c" })
                });

            //Child 1 setup
            var grandChild11 = new Mock<IDecisionTree<object, object>>();
            grandChild11.Setup(child => child.IsLeaf).Returns(true);
            grandChild11.Setup(child => child.FeatureVectors).Returns(
                new List<ISingleValueFeatureVector<object>>()
                {
                    new SingleValueFeatureVector<object>(new object[]{"a", "b", "c"}),
                    new SingleValueFeatureVector<object>(new object[] { "a", "b", "c" })
                });

            var grandChild12 = new Mock<IDecisionTree<object, object>>();
            grandChild12.Setup(child => child.IsLeaf).Returns(true);
            grandChild12.Setup(child => child.FeatureVectors).Returns(
                new List<ISingleValueFeatureVector<object>>()
                {
                    new SingleValueFeatureVector<object>(new object[]{"a", "b", "c"})
                });
            child1.Setup(child => child.Children).Returns(new List<ITree<object, object>>() { grandChild11.Object, grandChild12.Object });

            //Child2 setup
            var grandChild21 = new Mock<IDecisionTree<object, object>>();
            grandChild21.Setup(child => child.IsLeaf).Returns(true);
            grandChild21.Setup(child => child.FeatureVectors).Returns(
                new List<ISingleValueFeatureVector<object>>()
                {
                    new SingleValueFeatureVector<object>(new object[]{"a", "b", "c"}),
                    new SingleValueFeatureVector<object>(new object[] { "a", "b", "c" }),
                    new SingleValueFeatureVector<object>(new object[] { "a", "b", "c" }),
                    new SingleValueFeatureVector<object>(new object[] { "a", "b", "c" })
                });

            var grandChild22 = new Mock<IDecisionTree<object, object>>();
            grandChild22.Setup(child => child.IsLeaf).Returns(true);
            grandChild22.Setup(child => child.FeatureVectors).Returns(
                new List<ISingleValueFeatureVector<object>>()
                {
                    new SingleValueFeatureVector<object>(new object[]{"a", "b", "c"}),
                    new SingleValueFeatureVector<object>(new object[] { "a", "b", "c" })
                });
            child2.Setup(child => child.Children).Returns(new List<ITree<object, object>>() { grandChild21.Object, grandChild22.Object });

            //Tree root setup
            treeRoot.Setup(root => root.Children)
                .Returns(new List<ITree<object, object>>() {child1.Object, child2.Object, child3.Object});

            return treeRoot.Object;
        }

        /// <summary>
        /// Builds decision tree basing on examples from "Machine Learning in Action"
        /// </summary>
        /// <returns></returns>
        public static IDecisionTree<object, bool> BuildAnimalsExampleDecisionTree()
        {
            //Leaves
            var surfacingLeaf = new DecisionTree<object, bool>(false, new List<ISingleValueFeatureVector<object>>()
            {
                new SingleValueFeatureVector<object>(new object[]{ false, true, false }),
                new SingleValueFeatureVector<object>(new object[]{ false, true, false })
            });

            var noSurfacingNoFlippersLeaf = new DecisionTree<object, bool>(false, new List<ISingleValueFeatureVector<object>>()
            {
                new SingleValueFeatureVector<object>(new object[]{ true, false, false })
            });

            var noSurfacingFlippersLeaf = new DecisionTree<object, bool>(true, new List<ISingleValueFeatureVector<object>>()
            {
                new SingleValueFeatureVector<object>(new object[]{ true, false, true }),
                new SingleValueFeatureVector<object>(new object[]{ true, false, true })
            });

            var flippersDecisionNode = new DecisionTree<object, bool>(
                new SplitOption<object>(1, "has flippers?", false, true, true, true)
                );
            flippersDecisionNode.AddWeightedChild(noSurfacingFlippersLeaf, true, 0.66);
            flippersDecisionNode.AddWeightedChild(noSurfacingNoFlippersLeaf, false, 0.33);

            var treeRoot = new DecisionTree<object, bool>(
                new SplitOption<object>(0, "no surfacing?", false, true, true, "true")
                );
            treeRoot.AddWeightedChild(surfacingLeaf, false, 0.4);
            treeRoot.AddWeightedChild(flippersDecisionNode, true, 0.6);

            return treeRoot;
        } 
    }
}