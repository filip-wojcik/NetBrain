using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Algorithms.FrequentItems.Models;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Algorithms.FrequentItems;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.FrequentItems
{
    [TestClass()]
    public class AprioriAlgorithmTests
    {
        # region Frequent items finding methods

        [TestMethod()]
        public void FindInitialFrequentItems_AbstractDataSet_Test()
        {
            //Given
            int longestTrasactionFound = 0;
            IList<IEnumerable<int>> testData = AprioriAlgorithmTestDataBuilder.BuildAbstratTestData();
            var subject = new AprioriAlgorithm<int>();
            var expectedFrequentItems = new List<IFrequentItemsSet<int>>()
            {
                new FrequentItemsSet<int>(0.5, 1),
                new FrequentItemsSet<int>(0.75, 3),
                new FrequentItemsSet<int>(0.75, 2),
                new FrequentItemsSet<int>(0.75, 5),
            };

            //When
            IEnumerable<IFrequentItemsSet<int>> actualItemsSets = subject.FindInitialFrequentItems(testData, out longestTrasactionFound);

            //Then
            Assert.AreEqual(4, actualItemsSets.Count());
            Assert.IsTrue(
                expectedFrequentItems.All(actualItemsSets.Contains)
                );
            Assert.AreEqual(4, longestTrasactionFound);
        }

        [TestMethod()]
        public void FindInitialFrequentItems_FeatureVectorsDataSet_Test()
        {
            //Given
            int longestTrasactionFound = 0;
            IDataSet<string> testData = AprioriAlgorithmTestDataBuilder.BuildGamesTestData();
            var subject = new AprioriAlgorithm<ILabeledFeature<string>>();
            var expectedFrequentItems = new List<IFrequentItemsSet<ILabeledFeature<string>>>()
            {
                new FrequentItemsSet<ILabeledFeature<string>>(0.6, new LabeledFeature<string>("SF", "theme")),
                new FrequentItemsSet<ILabeledFeature<string>>(0.6, new LabeledFeature<string>("PC", "platform"))
                
            };

            //When
            IEnumerable<IFrequentItemsSet<ILabeledFeature<string>>> actualItemsSets = subject.FindInitialFrequentItems(testData.LabeledDataSet, out longestTrasactionFound);

            //Then
            Assert.AreEqual(2, actualItemsSets.Count());
            Assert.IsTrue(
                expectedFrequentItems.All(actualItemsSets.Contains)
                );
            Assert.AreEqual(4, longestTrasactionFound);
        }

        [TestMethod()]
        public void FindFrequentItems_AbstractDataSet_Test()
        {
            //Given
            int longestTrasactionFound = 0;
            IList<IEnumerable<int>> testData = AprioriAlgorithmTestDataBuilder.BuildAbstratTestData();
            var subject = new AprioriAlgorithm<int>();
            var expectedFrequentItems = new List<IFrequentItemsSet<int>>()
            {
                new FrequentItemsSet<int>(0.5, 1),
                new FrequentItemsSet<int>(0.75, 3),
                new FrequentItemsSet<int>(0.75, 2),
                new FrequentItemsSet<int>(0.75, 5),
                new FrequentItemsSet<int>(0.5, 1, 3),
                new FrequentItemsSet<int>(0.5, 3, 2),
                new FrequentItemsSet<int>(0.5, 3, 5),
                new FrequentItemsSet<int>(0.75, 2, 5),
                new FrequentItemsSet<int>(0.5, 3, 2, 5)

            };

            //When
            IEnumerable<IFrequentItemsSet<int>> actualItemsSets = subject.FindFrequentItemsSets(testData);

            //Then
            Assert.AreEqual(9, actualItemsSets.Count());
            Assert.IsTrue(
               expectedFrequentItems.All(actualItemsSets.Contains)
               );
        }

        # endregion Frequent items finding methods

        # region Association rules finding methods

        [TestMethod]
        public void CalculateConfidenceTest()
        {
            //Given
            var antecedent = new HashSet<string>() { "b" };
            var consequent = new HashSet<string>() { "a" };
            var frequentItems = new Dictionary<HashSet<string>, double>(HashSet<string>.CreateSetComparer())
            {
                {antecedent, 0.375},
                {consequent, 0.5},
                {new HashSet<string>(antecedent.Union(consequent)), 0.25},
            };
            var subject = new AprioriAlgorithm<string>();
            var expectedConfidence = 0.666;

            //When
            double confidence = subject.CalculateConfidence(antecedent, consequent, frequentItems);

            //Then
            Assert.AreEqual(expectedConfidence, confidence, 0.0009);
        }

        [TestMethod]
        public void FindAssociationRulesTest()
        {
            //Given
            var frequentItems = new List<IFrequentItemsSet<int>>()
            {
                new FrequentItemsSet<int>(0.5, 1),
                new FrequentItemsSet<int>(0.75, 3),
                new FrequentItemsSet<int>(0.75, 2),
                new FrequentItemsSet<int>(0.75, 5),
                new FrequentItemsSet<int>(0.5, 1, 3),
                new FrequentItemsSet<int>(0.5, 3, 2),
                new FrequentItemsSet<int>(0.5, 3, 5),
                new FrequentItemsSet<int>(0.75, 2, 5),
                new FrequentItemsSet<int>(0.5, 3, 2, 5)
            };
            var expectedAssociationRules = new List<IAssociationRule<int>>()
            {
                new AssociationRule<int>(frequentItems[0], frequentItems[1], 1.0),
                new AssociationRule<int>(frequentItems[1], frequentItems[0], 0.666),
                new AssociationRule<int>(frequentItems[1], frequentItems[2], 0.666),
                new AssociationRule<int>(frequentItems[2], frequentItems[1], 0.666),
                new AssociationRule<int>(frequentItems[1], frequentItems[3], 0.666),
                new AssociationRule<int>(frequentItems[3], frequentItems[1], 0.666),
                new AssociationRule<int>(frequentItems[2], frequentItems[3], 1.0),
                new AssociationRule<int>(frequentItems[3], frequentItems[2], 1.0),

                new AssociationRule<int>(frequentItems[1], frequentItems[7], 0.666),
                new AssociationRule<int>(frequentItems[7], frequentItems[1], 0.666),
                new AssociationRule<int>(frequentItems[2], frequentItems[7], 0.666),
                new AssociationRule<int>(frequentItems[2], frequentItems[6], 0.666),
                new AssociationRule<int>(frequentItems[7], frequentItems[2], 1.0),
                new AssociationRule<int>(frequentItems[6], frequentItems[2], 1.0),
                new AssociationRule<int>(frequentItems[3], frequentItems[5], 0.666),
                new AssociationRule<int>(frequentItems[5], frequentItems[3], 1.0)

            };

            var subject = new AprioriAlgorithm<int>(confidenceThreshold: 0.5);

            //When
            IList<IAssociationRule<int>> actualAssociationRules = subject.FindAssociationRules(frequentItems).ToList();

            //Then
            Assert.AreEqual(14, actualAssociationRules.Count);
            foreach (var actualAssociationRule in actualAssociationRules)
            {
                Assert.IsTrue(expectedAssociationRules.Any(expectedAssociationRule => expectedAssociationRule.Antecedent.Equals(actualAssociationRule.Antecedent) &&
                                                                                      expectedAssociationRule.Consequent.Equals(actualAssociationRule.Consequent) &&
                                                                 AssertAreAlmostEqual(expectedAssociationRule.Confidence, actualAssociationRule.Confidence, 0.009)));
            }
        }

        # endregion Association rules finding methods

        # region Helper methods

        [TestMethod()]
        public void CombineAndBuildItemsOfLength_Test_Combination_Of_Length2()
        {
            //Given
            var existingItems = new List<IFrequentItemsSet<int>>()
            {
                new FrequentItemsSet<int>(0.5, 1),
                new FrequentItemsSet<int>(0.75, 3),
                new FrequentItemsSet<int>(0.75, 2),
                new FrequentItemsSet<int>(0.75, 5),
            };

            var expectedItems = new List<ISet<int>>()
            {
                new HashSet<int>(){ 1, 3 },
                new HashSet<int>(){ 1, 2 },
                new HashSet<int>(){ 1, 5 },
                new HashSet<int>(){ 3, 2 },
                new HashSet<int>(){ 3, 5 },
                new HashSet<int>(){ 2, 5 }
            };

            var subject = new AprioriAlgorithm<int>();

            //When
            IEnumerable<ISet<int>> combinedItems = subject.CombineAndBuildNewItemSets(existingItems, 2);

            //Then
            Assert.IsTrue(
                expectedItems.All(expectedItemsSet => combinedItems.Any(combinedItemsSet => combinedItemsSet.SetEquals(expectedItemsSet)))
                );
        }

        [TestMethod()]
        public void CombineAndBuildItemsOfLength_Test_Combination_Of_Length3()
        {
            //Given
            var existingItems = new List<IFrequentItemsSet<int>>()
            {
                new FrequentItemsSet<int>(0.5, 1, 3),
                new FrequentItemsSet<int>(0.5, 1, 2),
                new FrequentItemsSet<int>(0.5, 1, 5),
                new FrequentItemsSet<int>(0.5, 3, 2),
                new FrequentItemsSet<int>(0.5, 3, 5),
                new FrequentItemsSet<int>(0.5, 2, 5)
            };

            var expectedItems = new List<ISet<int>>()
            {
                new HashSet<int>(){ 1, 3, 2 },
                new HashSet<int>(){ 1, 3, 5 },
                new HashSet<int>(){ 2, 3, 5 }
            };

            var subject = new AprioriAlgorithm<int>();

            //When
            IEnumerable<ISet<int>> combinedItems = subject.CombineAndBuildNewItemSets(existingItems, 3);

            //Then
            Assert.IsTrue(
                expectedItems.All(expectedItemsSet => combinedItems.Any(combinedItemsSet => combinedItemsSet.SetEquals(expectedItemsSet)))
                );
        }

        [TestMethod()]
        public void SelectFrequentItems_Test()
        {
            //Given
            var dataSet = AprioriAlgorithmTestDataBuilder.BuildAbstratTestData();
            var combinedItems = new List<ISet<int>>()
            {
                new HashSet<int>(){ 1, 3 },
                new HashSet<int>(){ 1, 2 },
                new HashSet<int>(){ 1, 5 },
                new HashSet<int>(){ 3, 2 },
                new HashSet<int>(){ 3, 5 },
                new HashSet<int>(){ 2, 5 }
            };

            var expectedItemsSets = new List<IFrequentItemsSet<int>>()
            {
                new FrequentItemsSet<int>(0.5, 1, 3),
                new FrequentItemsSet<int>(0.5, 3, 2),
                new FrequentItemsSet<int>(0.5, 3, 5),
                new FrequentItemsSet<int>(0.75, 2, 5)
            };

            var subject = new AprioriAlgorithm<int>();

            //When
            IEnumerable<IFrequentItemsSet<int>> actualFrequentItems = subject.SelectFrequentItems(combinedItems, dataSet);

            //Then
            Assert.IsTrue(expectedItemsSets.All(expectedItemSet => actualFrequentItems.Any(actualItemsSet => actualItemsSet.Equals(expectedItemSet))));
        }

        # endregion Helper methods

        # region Utilities

        public static bool AssertAreAlmostEqual(double num1, double num2, double tolerance)
        {
            Assert.AreEqual(num1, num2, tolerance);
            return true;
        }

        # endregion Utilities
    }
}