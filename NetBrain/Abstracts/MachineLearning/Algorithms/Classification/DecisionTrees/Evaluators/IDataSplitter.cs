using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Utils;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    public delegate double NumbericDataMiddlepointFinder(IEnumerable<double> dataVector);

    public interface IDataSplitter<T, V>
    {
        NumbericDataMiddlepointFinder NumbericDataMiddlepointFinder { get; }
        TrueEquivalent<V> TrueEquivalent { get; } 
        FalseEquivalent<V> FalseEquivalent { get; } 
        IEnumerable<ISplittingResult<T, V>> SplitFeatureVectors(ISingleValueDataSet<T> singleValueDataSet, ISplitOption<T> splitOption);

        IEnumerable<ISplitOption<T>> GenerateSplitOptionsForAxis(ISingleValueDataSet<T> singleValueDataSet, int axis);
        IEnumerable<ISplittingResult<T, bool>> SplitNumbericData(ISingleValueDataSet<T> singleValueDataSet, ISplitOption<T> splitOption);
    }
}
