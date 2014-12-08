namespace NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    using Models;
    using Utils;
    using MachineLearning.Models.Data;

    public interface IDecisionTreeBuilder<T, V>
    {
        IDataSplitter<T, V> DataSplitter { get; set; }
        TrueEquivalent<V> TrueEquivalent { get; set; }
        FalseEquivalent<V> FalseEquivalent { get; set; }
        IBestSplitSelector<T, V> BestSplitSelector { get; set; } 
        IDecisionTree<T, V> BuildDecisionTree(ISingleValueDataSet<T> singleValueDataSet);
    }
}
