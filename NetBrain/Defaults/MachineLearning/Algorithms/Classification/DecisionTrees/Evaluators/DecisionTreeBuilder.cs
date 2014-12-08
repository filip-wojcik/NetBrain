using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Utils;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Utils;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    public class DecisionTreeBuilder<T, V> : IDecisionTreeBuilder<T, V>
    {
        # region Public properties

        public IDataSplitter<T, V> DataSplitter { get; set; }
        public TrueEquivalent<V> TrueEquivalent { get; set; }
        public FalseEquivalent<V> FalseEquivalent { get; set; }
        public IBestSplitSelector<T, V> BestSplitSelector { get; set; } 

        # endregion Public properties

        # region Construction

        public DecisionTreeBuilder()
        {
        }

        public DecisionTreeBuilder(
            IDataSplitter<T, V> dataSplitter,
            IBestSplitSelector<T, V> bestSplitSelector, 
            TrueEquivalent<V> trueEquivalent,
            FalseEquivalent<V> falseEquivalent
            )
        {
            BestSplitSelector = bestSplitSelector;
            DataSplitter = dataSplitter;
            TrueEquivalent = trueEquivalent;
            FalseEquivalent = falseEquivalent;
        }

        # endregion Construction

        # region Processing methods

        public IDecisionTree<T, V> BuildDecisionTree(ISingleValueDataSet<T> singleValueDataSet)
        {
            if (singleValueDataSet != null && singleValueDataSet.Any())
            {
                if (singleValueDataSet.UniqueValues.Count() == 1) return this.BuildLeafNode(singleValueDataSet);
                ISplitOption<T> bestSplitOption = this.BestSplitSelector.ChooseBestSplitOption(singleValueDataSet, this.DataSplitter);
                if (bestSplitOption.SplitAxis == -1)
                {
                    return this.BuildLeafNode(singleValueDataSet);
                }
                else
                {
                    return this.BuildDecisionNode(singleValueDataSet, bestSplitOption);
                }
            }
            else
            {
                return new DecisionTree<T, V>(default(T), null);
            }
        }

        protected IDecisionTree<T, V> BuildLeafNode(ISingleValueDataSet<T> singleValueDataSet)
        {
            var uniqValues = singleValueDataSet.UniqueValues;
            if (uniqValues.Count() == 1)
            {
                return new DecisionTree<T, V>(uniqValues.First(), singleValueDataSet.SingleValueVectors);
            }
            else
            {
                //Majority vote
                double totalVectorsCount = 0.0;
                IDictionary<T, double> countsDictionary = uniqValues.ToDictionary(value => value, value => 0.0);
                foreach (var vector in singleValueDataSet.SingleValueVectors)
                {
                    countsDictionary[vector.Value] += 1;
                    totalVectorsCount += 1;
                }
                double highestProbability = 0;
                T bestValue = countsDictionary.Keys.First();
                foreach (var valueWithCount in countsDictionary)
                {
                    double probability = valueWithCount.Value/totalVectorsCount;
                    if (probability > highestProbability)
                    {
                        highestProbability = probability;
                        bestValue = valueWithCount.Key;
                    }
                }
                return new DecisionTree<T, V>(bestValue, singleValueDataSet.SingleValueVectors);
            }
        }

        protected IDecisionTree<T, V> BuildDecisionNode(ISingleValueDataSet<T> singleValueDataSet,
            ISplitOption<T> bestSplitOption)
        {
            long allDataCount = singleValueDataSet.Count();

            if (bestSplitOption.SplitOnConcreteValue)
            {
                var decisionNode = new DecisionTree<T, V>(bestSplitOption);
                if (bestSplitOption.IsDataNumberic)
                {
                    foreach (
                        var splittedNumbericData in
                            this.DataSplitter.SplitNumbericData(singleValueDataSet, bestSplitOption))
                    {
                        V value = (splittedNumbericData.Value == true) ?
                            this.TrueEquivalent() : this.FalseEquivalent();
                        this.ExpandTree(
                            currentNode: decisionNode,
                            allDataCount: allDataCount,
                            splittingResult: splittedNumbericData,
                            childLinkValue: value
                            );
                    }
                }
                else
                {
                    foreach (
                        var splittedData in this.DataSplitter.SplitFeatureVectors(singleValueDataSet, bestSplitOption))
                    {
                        this.ExpandTree(decisionNode, allDataCount, splittedData, splittedData.Value);
                    }
                }
                return decisionNode;
            }
            else
            {
                var decisionNode = new DecisionTree<T, V>(bestSplitOption);
                foreach (var splittedData in this.DataSplitter.SplitFeatureVectors(singleValueDataSet, bestSplitOption))
                {
                    this.ExpandTree(decisionNode, allDataCount, splittedData, splittedData.Value);
                }
                return decisionNode;
            }
        }

        # endregion Processing methods

        # region Helper methods

        protected virtual void ExpandTree<W>(
            IDecisionTree<T, V> currentNode, 
            long allDataCount,
            ISplittingResult<T, W> splittingResult,
            V childLinkValue)
        {
            long splittedDataCount = splittingResult.SingleValuesDataSet.Count();
            double probability = splittedDataCount/(double) allDataCount;
            currentNode.AddWeightedChild(
                child: this.BuildDecisionTree(splittingResult.SingleValuesDataSet),
                value: childLinkValue,
                probability: probability
                );
        }

        # endregion Helper methods
    }
}
