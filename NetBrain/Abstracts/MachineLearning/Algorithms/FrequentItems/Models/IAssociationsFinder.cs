using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.FrequentItems.Models
{
    public delegate IAssociationsFinder<T> AssociationsFinderFactory<T>(double confidenceThreshold);

    public interface IAssociationsFinder<T>
    {
        double ConfidenceThreshold { get; }
        IEnumerable<IAssociationRule<T>> FindAssociationRules(IEnumerable<IFrequentItemsSet<T>> frequentItems);
    }
}
