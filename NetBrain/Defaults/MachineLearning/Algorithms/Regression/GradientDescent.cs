using NetBrain.Abstracts.MachineLearning.Algorithms.Regression;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Regression
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Abstracts.MachineLearning.QualityCheckers;
    using Abstracts.MachineLearning.MathematicalFunctions;
    using Abstracts.MachineLearning.MathematicalFunctions.Regression;
    using Abstracts.MachineLearning.Models.Data;

    public class GradientDescent : BaseGradient
    {
        public static IRegressor GradientDescentFactory(IRegressionFunction regressionFunction,
            StatisticalFunctions.NumericErrorChecker numericErrorChecker,
            double learningRate = 0.01,
            int iterations = 1000,
            double stopTrainingErrorThreshold = 0.001,
            IQualityCheckLogger logger = null)
        {
            return new GradientDescent(regressionFunction, numericErrorChecker, learningRate, iterations, stopTrainingErrorThreshold, logger);
        }

        public GradientDescent(
            IRegressionFunction regressionFunction,
            StatisticalFunctions.NumericErrorChecker numericErrorChecker,
            double learningRate = 0.01,
            int iterations = 1000,
            double stopTrainingErrorThreshold = 0.001,
            IQualityCheckLogger logger = null) 
            : base(regressionFunction, numericErrorChecker, learningRate, iterations, stopTrainingErrorThreshold, logger)
        {
        }

        public override IList<double> PredictWeights(ISingleValueDataSet<double> dataToOperate)
        {
            var nonValuesDataSet = dataToOperate.NonValueVectorsSet as ISingleValueDataSet<double>;
            var weights = Enumerable.Repeat(0.5, nonValuesDataSet.SingleVectorSize).ToList();
            var expectedOutcomes = dataToOperate.ValuesInColumn(dataToOperate.ValueIndex).ToList();
            var actualOutcomes = base.RegressionFunction.CalculateOutput(nonValuesDataSet, weights);
            var initialError = base.NumericErrorChecker(expectedOutcomes, actualOutcomes);
            if (initialError <= base.StopTrainingErrorThreshold)
            {
                return actualOutcomes;
            }
            for (int i = 0; i < base.Iterations; i++)
            {
                actualOutcomes = base.RegressionFunction.CalculateOutput(nonValuesDataSet, weights);
                var error = base.NumericErrorChecker(expectedOutcomes, actualOutcomes);
                if (error <= base.StopTrainingErrorThreshold)
                {
                    break;
                }
                base.LogError(error, i);

                UpdateWeights(expectedOutcomes, actualOutcomes, nonValuesDataSet, weights);
            }
            return weights;
        }

        protected virtual void UpdateWeights(List<double> expectedOutcomes, IList<double> actualOutcomes, ISingleValueDataSet<double> dataSet,
            List<double> weights)
        {
            var weightsDeltas = this.RegressionFunction.CalculateDerivativeWithRespectToWeights(expectedOutcomes,
                actualOutcomes, dataSet, dataSet.SingleVectorSize);
            Parallel.For(0, weightsDeltas.Count, weightIdx =>
            {
                weights[weightIdx] -= weightsDeltas[weightIdx] * this.LearningRate;
            });
        }
    }
}
