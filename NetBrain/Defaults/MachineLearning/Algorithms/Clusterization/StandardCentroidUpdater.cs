using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Clusterization
{
    public class StandardCentroidUpdater : ICentroidUpdater
    {
        public void UpdateCentroid(ICentroid centroid, IDataSet<double> dataSet, bool treatCentroidValuesAsVector = false)
        {
            object locker = new object();
            var sumForEachAxis = new double[centroid.Values.Count];
            Parallel.ForEach(centroid.AssignedVectorsIndexes, vectorIdx =>
            {
                IFeatureVector<double> vector = dataSet.ElementAt(vectorIdx);
                for (int i = 0; i < vector.Count(); i++)
                {
                    lock (locker)
                    {
                        sumForEachAxis[i] += vector.ElementAt(i);
                    }
                }
            });

            double meanDenominator = (treatCentroidValuesAsVector == true)
                ? centroid.AssignedVectorsIndexes.Count + 1
                : centroid.AssignedVectorsIndexes.Count;

            for (int i = 0; i < sumForEachAxis.Length; i++)
            {
                double nominator = (treatCentroidValuesAsVector == true)
                    ? sumForEachAxis[i] + centroid.Values[i]
                    : sumForEachAxis[i];
                sumForEachAxis[i] = nominator / meanDenominator;
            }
            centroid.Values = sumForEachAxis;
        }

        public void UpdateCentroids(IList<ICentroid> centroids, IDataSet<double> dataSet, bool treatCentroidValuesAsVector = false)
        {
            foreach (var centroid in centroids) { this.UpdateCentroid(centroid, dataSet, treatCentroidValuesAsVector); }
        }

        public virtual void AssignVector(ICentroid centroid, IDataSet<double> dataSet, int vectorIdxInDataSet)
        {
            centroid.AssignedVectorsIndexes.Add(vectorIdxInDataSet);
        }
    }
}
