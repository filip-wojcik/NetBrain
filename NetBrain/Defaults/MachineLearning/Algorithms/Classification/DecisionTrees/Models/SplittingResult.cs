using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Models
{
    public class SplittingResult<T, V> : ISplittingResult<T, V>
    {
        #region ISplittingResult<T,V> Members

        public V Value { get; private set; }

        public ISingleValueDataSet<T> SingleValuesDataSet { get; private set; }

        #endregion

        public SplittingResult(V value, ISingleValueDataSet<T> singleValuesDataSet)
        {
            Value = value;
            SingleValuesDataSet = singleValuesDataSet;
        }
    }
}
