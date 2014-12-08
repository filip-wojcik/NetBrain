namespace NetBrain.Abstracts.MachineLearning.QualityCheckers
{
    using System.Collections.Generic;

    public interface IExpectedActualPair<T>
    {
        IList<T> ExpectedOutcome { get; set; }
        IList<T> ActualOutcome { get; set; } 
    }
}
