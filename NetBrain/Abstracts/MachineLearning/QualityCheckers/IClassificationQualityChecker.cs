using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.QualityCheckers
{
    public interface IClassificationQualityChecker<T> : IQualityChecker<T>
    {
        /// <summary>
        /// Checks accuracy of classification process, calculated as:
        /// acc = (1/testdataCount) * (count of elements where predicted_class = expected_class)
        /// </summary>
        /// <param name="outcomes">Expected - actual outcomes</param>
        /// <returns>Rate of accuracy</returns>
        double AccuracyRate(IEnumerable<IExpectedActualPair<T>> outcomes);

        /// <summary>
        /// Checks accuracy of classification process, calculated as:
        /// acc = (1/testdataCount) * (count of elements where predicted_class = expected_class)
        /// </summary>
        ///<param name="contingencyTable">Table to calculate accuracy from</param>
        /// <returns>Rate of accuracy</returns>
        double AccuracyRate(IContingencyTable<T> contingencyTable);

        IContingencyTable<T> BuildContingencyTable(IEnumerable<IExpectedActualPair<T>> outcomes);
    }
}
