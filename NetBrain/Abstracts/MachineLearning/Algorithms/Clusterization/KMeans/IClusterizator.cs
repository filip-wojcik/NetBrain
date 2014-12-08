using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans
{
    public interface IClusterizator
    {
        IInitialClustersBuilder InitialClustersBuilder { get; }
        ICentroidUpdater CentroidUpdater { get; }
        DistanceCalculator DistanceCalculator { get; }
        int ClustersCount { get; }

        IList<ICentroid> PerformClusterization(IDataSet<double> dataSet);
    }
}
