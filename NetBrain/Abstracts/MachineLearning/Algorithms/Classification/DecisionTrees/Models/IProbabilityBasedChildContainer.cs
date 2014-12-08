using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Graphs.Models;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models
{
    public interface IProbabilityBasedChildContainer<T, V> : IChildContainer<T, V>
    {
        double Probability { get; }
    }
}
