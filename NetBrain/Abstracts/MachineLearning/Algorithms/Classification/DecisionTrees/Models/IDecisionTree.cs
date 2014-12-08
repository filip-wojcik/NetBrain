using System.Collections;
using System.Collections.Generic;
using NetBrain.Abstracts.Graphs.Models;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models
{
    public interface IDecisionTree<T, V> : ITree<T, V>
    {
        ISplitOption<T> SplitOption { get; } 
        bool IsLeafDecisionNode { get; }
        IEnumerable<IProbabilityBasedChildContainer<T, V>> WeightedChildrenValues { get; }
        IEnumerable<ISingleValueFeatureVector<T>> FeatureVectors { get; }

        /// <summary>
        /// Adds child to decision tree, where link is weighted by the probability
        /// attached to that child
        /// </summary>
        /// <param name="child">Child to be added</param>
        /// <param name="value">Value of child node link</param>
        /// <param name="probability">Probability (weight) of that link</param>
        void AddWeightedChild(ITree<T, V> child, V value = default(V), double probability = 1.0);
    }
}
