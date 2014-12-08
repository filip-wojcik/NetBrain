using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans
{
    public interface ICentroidUpdater
    {
        void UpdateCentroids(IList<ICentroid> centroids, IDataSet<double> dataSet, bool treatCentroidValuesAsVector = false);
        void UpdateCentroid(ICentroid centroid, IDataSet<double> dataSet, bool treatCentroidValuesAsVector = false);
        void AssignVector(ICentroid centroid, IDataSet<double> dataSet, int vectorIdxInDataSet);
    }
}
