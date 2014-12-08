namespace NetBrain.Abstracts.MachineLearning.MathematicalFunctions
{
    using Models.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public delegate double ContinuousDomainChaosMeasure(ISingleValueDataSet<double> dataSet, int axisToMeasure);
    public delegate double ContinousValuesChaosMeasurer(IEnumerable<double> vector);

    public delegate double DiscreteDomainChaosMeasure<T>(ISingleValueDataSet<T> dataSet, int axisToMeasure);

    public static class ChaosMeasureFunctions
    {
        public static double ShannonEntropy<T>(ISingleValueDataSet<T> dataSet, int axisToMeasure)
        {
            var uniqValuesDict = new Dictionary<T, double>();
            var allElementsCount = dataSet.Count();
            double entropy = 0;
            foreach (var valuesVector in dataSet.Vectors)
            {
                if(!uniqValuesDict.ContainsKey(valuesVector[axisToMeasure])) uniqValuesDict.Add(valuesVector[axisToMeasure], 0);
                uniqValuesDict[valuesVector[axisToMeasure]] += 1;
            }
            foreach (var countOfUniqValue in uniqValuesDict.Values)
            {
                double probability = countOfUniqValue/allElementsCount;
                entropy -= probability*Math.Log(probability, 2);
            }
            return entropy;

        }

        public static double GiniImpurity<T>(ISingleValueDataSet<T> dataSet, int axisToMeasure)
        {
            var uniqValuesDict = new Dictionary<T, double>();
            var allElementsCount = dataSet.Count();
            double impurity = 1;
            foreach (var valuesVector in dataSet.Vectors)
            {
                if (!uniqValuesDict.ContainsKey(valuesVector[axisToMeasure])) uniqValuesDict.Add(valuesVector[axisToMeasure], 0);
                uniqValuesDict[valuesVector[axisToMeasure]] += 1;
            }
            foreach (var countOfUniqValue in uniqValuesDict.Values)
            {
                double probability = countOfUniqValue / allElementsCount;
                impurity -= Math.Pow(probability, 2);
            }
            return impurity;
        }
    }
}
