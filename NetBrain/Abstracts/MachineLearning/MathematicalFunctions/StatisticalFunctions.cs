namespace NetBrain.Abstracts.MachineLearning.MathematicalFunctions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Models.Data;

    public static class StatisticalFunctions
    {
        public delegate double NumericErrorChecker(IList<double> expected, IList<double> actual);

        public static double Mean(IEnumerable<double> numbers)
        {
            return numbers.Sum()/numbers.Count();
        }

        public static double Mean(ISingleValueDataSet<double> dataSet)
        {
            double totalElemsCount = dataSet.Count();
            var sumOfValues = dataSet.Values.Sum();
            return sumOfValues/totalElemsCount;
        }

        public static double Variance(IEnumerable<double> numbers)
        {
            if (numbers != null || numbers.Any())
            {
                int numbersCount = numbers.Count();
                double mean = numbers.Sum()/(double)numbersCount;
                double varianceBase = numbers.Sum(num => Math.Pow((num - mean), 2));
                return varianceBase/(double)numbersCount;
            }
            return 0;
        }

        public static double Variance(ISingleValueDataSet<double> dataSet)
        {
            if (dataSet != null && dataSet.Any() && dataSet.HasValues)
            {
                var totalCount = dataSet.Count();
                double mean = StatisticalFunctions.Mean(dataSet);
                double variance = 0;
                foreach (var value in dataSet.Values)
                {
                    variance += Math.Pow((value - mean), 2);
                }
                return variance / totalCount;
            }
            return 0;
        }

        public static double GaussianCurve(double value, double sigma = 10.0)
        {
            double val = Math.Pow((-1*value), 2)/(2*Math.Pow(sigma, 2));
            return Math.Pow(Math.E, val);
        }

        public static double InverseWeight(double value, double constans=0.1)
        {
            return 1/(value + constans);
        }

        public static double RootMeanSquareErrorFunction(IList<double> expected, IList<double> actual)
        {
            var results = new double[expected.Count];
            Parallel.For(0, expected.Count, i =>
            {
                results[i] = Math.Pow(actual[i] - expected[i], 2);
            });
            double denominator = expected.Count;
            return Math.Sqrt(results.Sum()) / denominator;
        }
    }
}
