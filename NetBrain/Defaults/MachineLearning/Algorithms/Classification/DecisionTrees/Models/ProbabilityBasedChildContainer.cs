using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Graphs.Models;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Defaults.Graphs.Trees.Base;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Models
{
    public class ProbabilityBasedChildContainer<T, V> : BaseChildContainer<T, V>, IProbabilityBasedChildContainer<T, V>
    {
        public double Probability { get; private set; }

        public ProbabilityBasedChildContainer(ITree<T, V> childTree, V childValue = default(V), double probability = 1.0) 
            : base(childTree, childValue)
        {
            Probability = probability;
        }

        protected bool Equals(ProbabilityBasedChildContainer<T, V> other)
        {
            return base.Equals(other) && Probability.Equals(other.Probability);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProbabilityBasedChildContainer<T, V>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ Probability.GetHashCode();
            }
        }
    }
}
