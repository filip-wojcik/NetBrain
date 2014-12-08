using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Clusterization
{
    public class IncrementalCentroidUpdater : StandardCentroidUpdater, IIncrementalCentroidUpdater
    {
        public override void AssignVector(ICentroid centroid, IDataSet<double> dataSet, int vectorIdxInDataSet)
        {
            base.AssignVector(centroid, dataSet, vectorIdxInDataSet);
            IFeatureVector<double> featureVector = dataSet.ElementAt(vectorIdxInDataSet);
            this.IncrementallyUpdateCentroid(centroid, featureVector);
        }

        public void IncrementallyUpdateCentroid(ICentroid centroid, IFeatureVector<double> vector)
        {
            centroid.Values = Enumerable.Range(0, centroid.Values.Count).Select(valueIdx => (centroid.Values[valueIdx] + vector[valueIdx]) / (double)2).ToList();
        }
    }
}
