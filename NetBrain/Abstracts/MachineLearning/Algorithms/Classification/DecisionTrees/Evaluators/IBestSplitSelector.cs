using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    public interface IBestSplitSelector<T, V>
    {
        IEntopyMeasurer<T> EntropyMeasurer { get; set; } 
        bool UseAverageMeasureToSplitNumbers { get; set; }

        ISplitOption<T> ChooseBestSplitOption(ISingleValueDataSet<T> singleValueDataSet, IDataSplitter<T, V> dataSplitter);
        IList<int> FindApplicableSplitAxes(ISingleValueDataSet<T> singleValuesDataSet);
        double MeasureEntropy(ISingleValueDataSet<T> singleValueDataSet);
    }

    public delegate IBestSplitSelector<T, V> BestSplitSelectorFactory<T, V>(
        IEntopyMeasurer<T> entropyMeasurer,
        bool useAverageMeasureToSplitNumbers
        );
}
