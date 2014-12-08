namespace NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    public interface ISplitOption<T>
    {
        bool SplitOnConcreteValue { get; }
        bool IsDataNumberic { get; }
        bool IsSplitBinary { get; }
        
        int SplitAxis { get; }
        string SplitLabel { get; }
        T ConcreteValueToSplit { get; }
        double ConcreteNumbericValueToSplit { get; }
    }
}
