using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.FrequentItems.Models
{
    /// <summary>
    /// Represents rule in form
    /// if item1, item2, ..., itemN are present then
    /// itemY1, itemY2, ..., itemYM will be present
    /// with probability = confidence
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAssociationRule<T>
    {
        /// <summary>
        /// Antecedent of the rule ("if" part)
        /// </summary>
        IFrequentItemsSet<T> Antecedent { get; }

        /// <summary>
        /// "Then" part of the rule
        /// </summary>
        IFrequentItemsSet<T> Consequent { get; }

        /// <summary>
        /// Represents certainty factor that the rule is true
        /// </summary>
        double Confidence { get; set; }
    }
}
