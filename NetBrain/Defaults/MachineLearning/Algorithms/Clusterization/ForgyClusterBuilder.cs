using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Clusterization
{
    public class ForgyClusterBuilder : BaseInitialClusterBuilder
    {
        protected override IList<ICentroid> BuildClusters(IDataSet<double> data, int clustersCount, ICentroidUpdater centroidUpdater)
        {
            var centroids = new ICentroid[clustersCount];
            var usedVariableIndexes = new HashSet<int>();
            for (int i = 0; i < clustersCount; i++)
            {
                var vectorIdx = -1;
                while (vectorIdx == -1)
                {
                    var randomIndex = base.Random.Next(0, data.Vectors.Count());
                    if (!usedVariableIndexes.Contains(randomIndex)) vectorIdx = randomIndex;
                }
                var selectedVector = data.Vectors.ElementAt(vectorIdx);
                centroids[i] = new Centroid(selectedVector.Features.ToList());

            }
            return centroids;
        }
    }
}
