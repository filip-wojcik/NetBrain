namespace NetBrain.Defaults.MachineLearning.QualityCheckers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstracts.MachineLearning.QualityCheckers;

    public class RootMeanSquareErrorChecker : IQualityChecker<double>
    {
        public double ErrorRate(IEnumerable<IExpectedActualPair<double>> outcomes)
        {
            return Math.Sqrt(
                outcomes.Sum(outcome => Math.Pow((outcome.ActualOutcome.Sum() - outcome.ExpectedOutcome.Sum()), 2)) / outcomes.Count());
        }

        public IQualityData MeasureQualityData(IEnumerable<IExpectedActualPair<double>> outcomes, int iterationNumber, bool testData)
        {
            return new NumericDataQuality()
            {
                ErrorRate = this.ErrorRate(outcomes),
                Iteration = iterationNumber,
                TestData = testData
            };
        }
    }
}