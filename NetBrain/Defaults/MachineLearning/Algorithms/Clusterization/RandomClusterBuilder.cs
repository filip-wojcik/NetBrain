using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Utils.CollectionExtensions;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Clusterization
{
    public class RandomClusterBuilder : BaseInitialClusterBuilder
    {
        protected override IList<ICentroid> BuildClusters(IDataSet<double> data, int clustersCount, ICentroidUpdater centroidUpdater)
        {
            var centroids = new ICentroid[clustersCount];
            var vectorsPerCluster = (int)Math.Ceiling(data.Count()/(double)clustersCount);
            var vectorIndexes = Enumerable.Range(0, data.Vectors.Count()).ToArray();
            vectorIndexes.Schuffle();
            for (int i = 0; i < clustersCount; i++)
            {
                var vectorIndexesPerCluster = vectorIndexes.Skip(i*vectorsPerCluster).Take(vectorsPerCluster);
                centroids[i] = new Centroid(new HashSet<int>(vectorIndexesPerCluster), new double[data.SingleVectorSize]);
                centroidUpdater.UpdateCentroid(centroids[i], data);
            }

            return centroids;
        }
    }
}
