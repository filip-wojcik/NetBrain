namespace NetBrain.Defaults.MachineLearning.Algorithms.Regression
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstracts.MachineLearning.Models.Data;
    using Abstracts.MachineLearning.MathematicalFunctions;
    using Abstracts.MachineLearning.MathematicalFunctions.Regression;
    using Abstracts.MachineLearning.QualityCheckers;

    public class StochasticGradientDescent : BaseGradient
    {
        public static StochasticGradientDescent StochasticGradientDescentFactory(
            IRegressionFunction regressionFunction,
            StatisticalFunctions.NumericErrorChecker numericErrorChecker,
            double learningRate = 0.01,
            int iterations = 1000,
            double stopTrainingErrorThreshold = 0.001,
            IQualityCheckLogger logger = null,
            double learningRateNominator = 4.0)
        {
            return new StochasticGradientDescent(regressionFunction, numericErrorChecker, learningRate, iterations, stopTrainingErrorThreshold, logger, learningRateNominator);
        }

        private readonly double _learningRateNominator;
        private readonly Random _randomizer = new Random();

        public StochasticGradientDescent(
            IRegressionFunction regressionFunction,
            StatisticalFunctions.NumericErrorChecker numericErrorChecker,
            double learningRate = 0.01,
            int iterations = 1000,
            double stopTrainingErrorThreshold = 0.001,
            IQualityCheckLogger logger = null,
            double learningRateNominator = 4.0) 
            : base(regressionFunction, numericErrorChecker, learningRate, iterations, stopTrainingErrorThreshold, logger)
        {
            _learningRateNominator = learningRateNominator;
        }

        public override IList<double> PredictWeights(ISingleValueDataSet<double> dataToOperate)
        {
            var nonValuesDataSet = dataToOperate.NonValueVectorsSet as ISingleValueDataSet<double>;
            var weights = Enumerable.Repeat(0.5, nonValuesDataSet.SingleVectorSize).ToList();
            var expectedOutcomes = dataToOperate.ValuesInColumn(dataToOperate.ValueIndex).ToList();
            for (int iteration = 0; iteration < base.Iterations; iteration++)
            {
                var dataIndicesToUse = Enumerable.Range(0, nonValuesDataSet.Count).ToList();
                for (int rowIdx = 0; rowIdx < nonValuesDataSet.Count; rowIdx++)
                {
                    var learnRate = FindLearningRateForIteration(iteration, rowIdx);
                    int vectorIdx = dataIndicesToUse[this._randomizer.Next(dataIndicesToUse.Count)];
                    IFeatureVector<double> vector = nonValuesDataSet[vectorIdx];

                    double actualOutputForVector = base.RegressionFunction.CalculateOutput(vector.ToList(), weights);
                    double expectedOutputForVector = expectedOutcomes[vectorIdx];

                    double error = actualOutputForVector - expectedOutputForVector;
                    if (Math.Abs(error) < base.StopTrainingErrorThreshold)
                    {
                        break;
                    }
                    base.LogError(error, iteration);
                    for (int weightIdx = 0; weightIdx < weights.Count; weightIdx++)
                    {
                        weights[weightIdx] -= learnRate * error * vector[weightIdx];
                    }
                    dataIndicesToUse.Remove(vectorIdx);
                }
            }
            return weights;
        }

        private double FindLearningRateForIteration(int iteration, int rowIdx)
        {
            var learnRate = this._learningRateNominator/(1.0 + iteration + rowIdx) + 0.01;
            return learnRate;
        }
    }
}
