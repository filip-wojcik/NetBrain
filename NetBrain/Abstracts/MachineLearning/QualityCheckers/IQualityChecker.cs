namespace NetBrain.Abstracts.MachineLearning.QualityCheckers
{
    using System.Collections.Generic;

    /// <summary>
    /// General - purpose interface for checkign quality of
    /// machine-learning algorithms processing
    /// </summary>
    public interface IQualityChecker<T>
    {
        double ErrorRate(IEnumerable<IExpectedActualPair<T>> outcomes);
        IQualityData MeasureQualityData(IEnumerable<IExpectedActualPair<T>> outcomes, int iterationNumber, bool testData);
    }
}
