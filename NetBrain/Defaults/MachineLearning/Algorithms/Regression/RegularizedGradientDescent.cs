namespace NetBrain.Defaults.MachineLearning.Algorithms.Regression
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Abstracts.MachineLearning.MathematicalFunctions;
    using Abstracts.MachineLearning.MathematicalFunctions.Regression;
    using Abstracts.MachineLearning.Models.Data;
    using Abstracts.MachineLearning.QualityCheckers;

    public class RegularizedGradientDescent : GradientDescent
    {
        public static RegularizedGradientDescent RegularizedGradientDescentFactory(
            IRegressionFunction regressionFunction,
            StatisticalFunctions.NumericErrorChecker numericErrorChecker,
            double learningRate = 0.01,
            int iterations = 1000,
            double stopTrainingErrorThreshold = 0.001,
            double lambda = 0.2,
            IQualityCheckLogger logger = null)
        {
            return new RegularizedGradientDescent(regressionFunction, numericErrorChecker, learningRate, iterations, stopTrainingErrorThreshold, lambda, logger);
        }

        private readonly double _lambda;

        public RegularizedGradientDescent(
            IRegressionFunction regressionFunction, 
            StatisticalFunctions.NumericErrorChecker numericErrorChecker, 
            double learningRate = 0.01, 
            int iterations = 1000, 
            double stopTrainingErrorThreshold = 0.001,
            double lambda = 0.2,
            IQualityCheckLogger logger = null)
            : base(regressionFunction, numericErrorChecker, learningRate, iterations, stopTrainingErrorThreshold, logger)
        {
            _lambda = lambda;
        }

        protected override void UpdateWeights(List<double> expectedOutcomes, IList<double> actualOutcomes, ISingleValueDataSet<double> dataSet, List<double> weights)
        {
            var weightsDeltas = this.RegressionFunction.CalculateDerivativeWithRespectToWeights(expectedOutcomes,
               actualOutcomes, dataSet, dataSet.SingleVectorSize);
            Parallel.For(0, weightsDeltas.Count, weightIdx =>
            {
                var regularizator = (this._lambda/(double) dataSet.Count) * weights[weightIdx];
                weights[weightIdx] -= (weightsDeltas[weightIdx]*base.LearningRate) + regularizator;
            });
        }
    }
}
