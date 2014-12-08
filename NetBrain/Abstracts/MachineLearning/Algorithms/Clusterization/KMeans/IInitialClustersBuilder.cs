using System.Collections.Generic;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans
{
    public interface IInitialClustersBuilder
    {
        IList<ICentroid> BuildInitialClusters(IDataSet<double> data, int clustersCount, ICentroidUpdater centroidUpdater);
    }
}
