using System;
using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Utils;
using NetBrain.Utils.CollectionExtensions;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    public class DecisionTreeClassifier<T, V> : IDecisionTreeClassifier<T, V>
    {
        public TrueEquivalent<V> TrueEquivalent { get; private set; }

        public FalseEquivalent<V> FalseEquivalent { get; private set; }

        public IsValueApplicable<T, V> ApplicabilityChecker { get; private set; }

        public DecisionTreeClassifier(TrueEquivalent<V> trueEquivalent, FalseEquivalent<V> falseEquivalent, IsValueApplicable<T, V> applicabilityChecker)
        {
            TrueEquivalent = trueEquivalent;
            FalseEquivalent = falseEquivalent;
            ApplicabilityChecker = applicabilityChecker;
        }

        public T Classify(
            ISingleValueFeatureVector<T> vector,
            IDecisionTree<T, V> decisionTree
            )
        {
            if (decisionTree.IsLeaf) return decisionTree.Value;
            else
            {
                T vectorValue = vector[decisionTree.SplitOption.SplitAxis];

                if (vectorValue == null)
                {
                    return this.HandleMissingValueOfVector(vector, decisionTree);
                }

                int splitAxis = decisionTree.SplitOption.SplitAxis;
                IDecisionTree<T, V> childToCheckAgainst;
                if (decisionTree.SplitOption.SplitOnConcreteValue)
                {
                    if (decisionTree.SplitOption.IsDataNumberic)
                    {
                        childToCheckAgainst = HandleNumbericSplitValue(vector, decisionTree, splitAxis);
                    }
                    else
                    {
                        childToCheckAgainst = HandleSplitOnDiscreteValue(decisionTree, vectorValue);
                    }
                }
                else
                {
                    childToCheckAgainst = decisionTree.ChildrenWithValues.FirstOrDefault(
                            childContainer => this.ApplicabilityChecker(vectorValue, childContainer.ChildValue)
                            ).ChildTree as IDecisionTree<T, V>;
                }
                return this.Classify(vector, childToCheckAgainst);
            }
        }


        private T HandleMissingValueOfVector(
            ISingleValueFeatureVector<T> vector,
            IDecisionTree<T, V> decisionTree)
        {
            var weightedValues = new Dictionary<T, double>();
            double weightsSum = 0;
            foreach (var weightedChild in decisionTree.WeightedChildrenValues)
            {
                IDecisionTree<T, V> decisionTreeChild = weightedChild.ChildTree as IDecisionTree<T, V>;
                double linkProbability = weightedChild.Probability;

                T classificationValue = this.Classify(vector, decisionTreeChild);
                weightedValues.AddKeyIfNotExists(classificationValue);
                weightedValues[classificationValue] += linkProbability;
                weightsSum += linkProbability;
            }

            return weightedValues.OrderByDescending(elem => elem.Value / weightsSum).First().Key;
        }

        private IDecisionTree<T, V> HandleSplitOnDiscreteValue(IDecisionTree<T, V> decisionTree, T vectorValue)
        {
            IDecisionTree<T, V> childToCheckAgainst;
            T splitValue = decisionTree.SplitOption.ConcreteValueToSplit;
            if (vectorValue.Equals(splitValue))
            {
                childToCheckAgainst = decisionTree.ChildrenWithValues.FirstOrDefault(
                    child => child.ChildValue.Equals(this.TrueEquivalent())
                    ).ChildTree as IDecisionTree<T, V>;
            }
            else
            {
                childToCheckAgainst = decisionTree.ChildrenWithValues.FirstOrDefault(
                    child => child.ChildValue.Equals(this.FalseEquivalent())
                    ).ChildTree as IDecisionTree<T, V>;
            }
            return childToCheckAgainst;
        }

        private IDecisionTree<T, V> HandleNumbericSplitValue(ISingleValueFeatureVector<T> vector, IDecisionTree<T, V> decisionTree,
            int splitAxis)
        {
            IDecisionTree<T, V> childToCheckAgainst;
            double splitValue = decisionTree.SplitOption.ConcreteNumbericValueToSplit;
            double vectorNumbericValue = Convert.ToDouble(vector[splitAxis]);
            if (vectorNumbericValue < splitValue)
            {
                childToCheckAgainst = decisionTree.ChildrenWithValues.FirstOrDefault(
                    child => child.ChildValue.Equals(this.FalseEquivalent())
                    ).ChildTree as IDecisionTree<T, V>;
            }
            else
            {
                childToCheckAgainst = decisionTree.ChildrenWithValues.FirstOrDefault(
                    child => child.ChildValue.Equals(this.TrueEquivalent())
                    ).ChildTree as IDecisionTree<T, V>;
            }
            return childToCheckAgainst;
        }
    }
}