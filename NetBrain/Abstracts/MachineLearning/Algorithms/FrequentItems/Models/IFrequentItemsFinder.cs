using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.FrequentItems.Models
{
    public delegate IFrequentItemsFinder<T> FrequentItemsFinderFactory<T>(double supportThreshold);

    /// <summary>
    /// Algorithm that find frequent items in some set of transacstions
    /// </summary>
    /// <typeparam name="T">Type parameter</typeparam>
    public interface IFrequentItemsFinder<T>
    {
        /// <summary>
        /// Percentage value, items's presence in transactions must be greater or equal
        /// </summary>
        double SupportThreshold { get; }

        /// <summary>
        /// Finds a group of frequently present items in some data set.
        /// </summary>
        /// <param name="transactions">
        /// List of transactions - number of groups of items. Example:
        /// Tran1: milk, chips, bread
        /// Tran2: milk, soya, chips
        /// Tran3: soya, bread, beer
        /// </param>
        /// <returns>List of frequent items sets</returns>
        IEnumerable<IFrequentItemsSet<T>> FindFrequentItemsSets(IEnumerable<IEnumerable<T>> transactions);
    }
}
