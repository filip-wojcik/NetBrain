using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans
{
    public interface IIncrementalCentroidUpdater : ICentroidUpdater
    {
        void IncrementallyUpdateCentroid(ICentroid centroid, IFeatureVector<double> vector);
    }
}
