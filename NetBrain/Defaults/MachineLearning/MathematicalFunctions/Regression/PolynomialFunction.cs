namespace NetBrain.Defaults.MachineLearning.MathematicalFunctions.Regression
{
    using Abstracts.MachineLearning.MathematicalFunctions.Regression;
    using Abstracts.MachineLearning.Models.Data;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PolynomialFunction : IRegressionFunction
    {
        public double CalculateOutput(IList<double> input, IList<double> weights)
        {
            var outputs = new double[input.Count];
            Parallel.For(0, input.Count, i =>
            {
                outputs[i] = input[i] * weights[i];
            });
            return outputs.Sum();
        }

        public IList<double> CalculateOutput(ISingleValueDataSet<double> data, IList<double> weights)
        {
            var outputs = new double[data.Count];
            Parallel.For(0, data.Count, i =>
            {
                outputs[i] = this.CalculateOutput(data[i].NonValuesVector.ToList(), weights);
            });
            return outputs;
        }

        public IList<double> CalculateDerivativeWithRespectToWeights(IList<double> expected, IList<double> actual, IDataSet<double> dataSet, int weightsCount)
        {
            var results = new double[weightsCount];
            var actualExpectedDiffs = this.ActualExpectedDiffs(expected, actual);
            Parallel.For(0, weightsCount, weightIdx => {
                double result = 0;
                for (int vectorIdx = 0; vectorIdx < dataSet.Count; vectorIdx++)
                {
                    result += actualExpectedDiffs[vectorIdx] * dataSet[vectorIdx, weightIdx];
                }
                results[weightIdx] = result / dataSet.Count;
            });
            return results;
        }

        private IList<double> ActualExpectedDiffs(IList<double> expected, IList<double> actual)
        {
            var results = new double[expected.Count];
            Parallel.For(0, expected.Count, i =>
            {
                results[i] = actual[i] - expected[i];
            });
            return results;
        }
    }
}
