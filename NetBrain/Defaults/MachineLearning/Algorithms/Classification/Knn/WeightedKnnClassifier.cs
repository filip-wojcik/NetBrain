namespace NetBrain.Defaults.MachineLearning.Algorithms.Classification.Knn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstracts.MachineLearning.Algorithms.Classification.Knn;
    using Abstracts.MachineLearning.Models.Data;
    using Abstracts.MachineLearning.MathematicalFunctions;

    public class WeightedKnnClassifier : IKnnClassifier
    {
        public Func<double, double> WeightFunc { get; private set; }
        public DistanceCalculator DisaCalculator { get; private set; }

        public int KNeighbors { get; private set; }

        public static WeightedKnnClassifier WeightedKnnClassifierFactory(DistanceCalculator distanceCalculator,
            int kNeighbors)
        {
            return new WeightedKnnClassifier(distanceCalculator, kNeighbors);
        }

        public WeightedKnnClassifier(DistanceCalculator disaCalculator, int kNeighbors, Func<double, double> weightFunc)
        {
            this.DisaCalculator = disaCalculator;
            if(kNeighbors < 1) throw new ArgumentOutOfRangeException(string.Format("kNeighbors count must be greater than 0, given: ", kNeighbors));
            this.KNeighbors = kNeighbors;
            this.WeightFunc = weightFunc;
        }

        public WeightedKnnClassifier(DistanceCalculator disaCalculator, int kNeighbors)
        {
            this.DisaCalculator = disaCalculator;
            this.KNeighbors = kNeighbors;
            
            //Default weight function, taking the distance as the measure
            this.WeightFunc = (distance) => distance;
        }

        public double Classify(ISingleValueFeatureVector<double> vectorToClassify, ISingleValueDataSet<double> dataSet)
        {
            var neighborsWithDistances = new List<Tuple<double, ISingleValueFeatureVector<double>>> ();
            foreach (var vector in dataSet.SingleValueVectors)
            {
                double distance = this.DisaCalculator(vectorToClassify.NonValuesVector, vector.NonValuesVector);
                neighborsWithDistances.Add(new Tuple<double, ISingleValueFeatureVector<double>>(distance, vector));
            }
            var bestNeighbors = neighborsWithDistances.OrderBy(elem => elem.Item1).Take(3).ToList();
            return WeightedValueOfNeighbors(bestNeighbors);
        }

        public double WeightedValueOfNeighbors(IList<Tuple<double, ISingleValueFeatureVector<double>>> neighbors)
        {
            double nominator = 0;
            double denominator = 0;
            foreach (var weightedNeighbor in neighbors)
            {
                double vectorValue = weightedNeighbor.Item2.Value;
                double weight = this.WeightFunc(weightedNeighbor.Item1);
                denominator += weight;
                nominator += vectorValue*weight;
            }

            return nominator/denominator;
        }
    }
}
