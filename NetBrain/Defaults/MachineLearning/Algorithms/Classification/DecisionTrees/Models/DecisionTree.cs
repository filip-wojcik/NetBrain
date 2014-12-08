using System;
using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.Graphs.Models;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.Graphs.Trees.Base;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Models
{
    public class DecisionTree<T, V> : BaseTree<T, V>, IDecisionTree<T, V>
    {
        # region Protected members

        protected IList<IProbabilityBasedChildContainer<T, V>> WeightedChildrenLinks; 

        # endregion Protected members

        #region IDecisionTree<T,V> Members

        public ISplitOption<T> SplitOption { get; private set; } 

        public bool IsLeafDecisionNode
        {
            get { return this.SplitOption != null && this.FeatureVectors != null && this.FeatureVectors.Any(); }
        }

        public IEnumerable<ISingleValueFeatureVector<T>> FeatureVectors { get; private set; }

        public IEnumerable<IProbabilityBasedChildContainer<T, V>> WeightedChildrenValues { 
            get
            {
                return this.WeightedChildrenLinks;
            }
        }

        public override IEnumerable<IChildContainer<T, V>> ChildrenWithValues
        {
            get
            {
                return from probabilityBasedChildContainer in this.WeightedChildrenValues
                    select probabilityBasedChildContainer as IChildContainer<T, V>;
            }
        }

        protected override IList<IChildContainer<T, V>> ChildrenNodesWithValues
        {
            get { return this.WeightedChildrenLinks.Select(childLink => childLink as IChildContainer<T, V>).ToList(); }
            set
            {
                this.WeightedChildrenLinks =
                    value.Select(childLink => childLink as IProbabilityBasedChildContainer<T, V>).ToList();
            }
        }

        #endregion

        # region Construction

        protected DecisionTree(IEnumerable<ISingleValueFeatureVector<T>> featureVectors = null)
        {
            this.FeatureVectors = featureVectors ?? new List<ISingleValueFeatureVector<T>>();
            this.WeightedChildrenLinks = new List<IProbabilityBasedChildContainer<T, V>>();
        }

        /// <summary>
        /// Constructor used for leaf nodes
        /// </summary>
        /// <param name="value">Value of the leaf node</param>
        /// <param name="featureVectors">Feature vectors to be stored in node</param>
        public DecisionTree(T value, IEnumerable<ISingleValueFeatureVector<T>> featureVectors)
            : this(featureVectors)
        {
            base.Value = value;
        }

        /// <summary>
        /// Constructor mostly used with single value binary split
        /// </summary>
        /// <param name="splitOption">split option that created node</param>
        public DecisionTree(ISplitOption<T> splitOption)
            : this()
        {
            this.SplitOption = splitOption;
        }

        # endregion Construction

        # region Processing methods

        public void AddWeightedChild(ITree<T, V> child, V value = default(V), double probability = 1.0)
        {
            IProbabilityBasedChildContainer<T, V> matchingChild = null;
            foreach (var weightedChildLink in this.WeightedChildrenLinks)
            {
                if (weightedChildLink != null && weightedChildLink.ChildTree.Equals(child))
                {
                    matchingChild = weightedChildLink;
                }
            }
            if (matchingChild != null) this.WeightedChildrenLinks.Remove(matchingChild);
            this.WeightedChildrenLinks.Add(new ProbabilityBasedChildContainer<T, V>(child, value, probability));

        }

        protected override IChildContainer<T, V> BuildChildContainer(ITree<T, V> child, V value = default(V))
        {
            return new ProbabilityBasedChildContainer<T, V>(child, value);
        }

        public override void RemoveEdge(INode<T> from, INode<T> to)
        {
            throw new NotImplementedException();
        }

        public override ITree<T, V> RemoveSelf()
        {
            throw new NotImplementedException();
        }

        # endregion Processing methods

        # region Helper methods

        # endregion Helper methods
    }
}
