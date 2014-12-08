using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models
{
    public interface ISplittingResult<T, V>
    {
        V Value { get; }
        ISingleValueDataSet<T> SingleValuesDataSet { get; } 
    }
}
