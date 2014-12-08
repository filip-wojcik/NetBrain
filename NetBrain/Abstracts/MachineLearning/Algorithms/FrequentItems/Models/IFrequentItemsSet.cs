using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.FrequentItems.Models
{
    /// <summary>
    /// Represents a group of items, appearing commonly in some transactions.
    /// Support value expresses such frequency ratio.
    /// </summary>
    /// <typeparam name="T">Any type parameter</typeparam>
    public interface IFrequentItemsSet<T>
    {
        /// <summary>
        /// Group of items
        /// </summary>
        ISet<T> Items { get; }

        /// <summary>
        /// Frequency ratio - The support of an itemset is defined as the percentage of the dataset that contains
        ///this itemsets (definition by Machine Learinig in Aciton by Peter Harrington
        /// </summary>
        double SupportValue { get; set; }
    }
}
