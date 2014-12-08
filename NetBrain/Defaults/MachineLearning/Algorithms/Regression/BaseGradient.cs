namespace NetBrain.Defaults.MachineLearning.Algorithms.Regression
{
    using System.Collections.Generic;
    using Abstracts.MachineLearning.Models.Data;
    using QualityCheckers;
    using Abstracts.MachineLearning.Algorithms.Regression;
    using Abstracts.MachineLearning.MathematicalFunctions;
    using Abstracts.MachineLearning.MathematicalFunctions.Regression;
    using Abstracts.MachineLearning.QualityCheckers;

    public abstract class BaseGradient : IRegressor
    {
        protected readonly int Iterations;
        protected readonly double StopTrainingErrorThreshold;
        protected readonly double LearningRate;

        public IRegressionFunction RegressionFunction { get; private set; }
        public StatisticalFunctions.NumericErrorChecker NumericErrorChecker { get; private set; }

        public IQualityCheckLogger Logger { get; private set; }

        protected BaseGradient(
            IRegressionFunction regressionFunction,
            StatisticalFunctions.NumericErrorChecker numericErrorChecker,
            double learningRate = 0.4,
            int iterations = 1000,
            double stopTrainingErrorThreshold = 0.001,
            IQualityCheckLogger logger = null)
        {
            RegressionFunction = regressionFunction;
            NumericErrorChecker = numericErrorChecker;
            LearningRate = learningRate;
            Iterations = iterations;
            StopTrainingErrorThreshold = stopTrainingErrorThreshold;
            Logger = logger;
        }

        public abstract IList<double> PredictWeights(ISingleValueDataSet<double> dataToOperate);

        protected void LogError(double errorRate, int iteration)
        {
            if (this.Logger != null)
            {
                this.Logger.LogProgress(new NumericDataQuality() { ErrorRate = errorRate, Iteration = iteration });
            }
        }
    }
}
