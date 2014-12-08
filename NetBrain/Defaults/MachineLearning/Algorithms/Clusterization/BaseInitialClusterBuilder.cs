using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Clusterization
{
    public abstract class BaseInitialClusterBuilder : IInitialClustersBuilder
    {
        private const string TOO_MANY_CLUSTERS = "Clusters count cannot be bigger than data vectors count!";

        protected Random Random;

        protected BaseInitialClusterBuilder(Random random = null)
        {
            Random = random ?? new Random();
        }

        public IList<ICentroid> BuildInitialClusters(IDataSet<double> data, int clustersCount, ICentroidUpdater centroidUpdater)
        {
            if (clustersCount > data.Count())
            {
                throw new ArgumentException(TOO_MANY_CLUSTERS);
            }
            return BuildClusters(data, clustersCount, centroidUpdater);
        }

        protected abstract IList<ICentroid> BuildClusters(IDataSet<double> data, int clustersCount,
            ICentroidUpdater centroidUpdater);
    }
}
