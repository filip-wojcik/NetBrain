using System.Collections.Generic;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans
{
    public interface ICentroid
    {
        IList<double> Values { get; set; }
        ISet<int> AssignedVectorsIndexes { get; set; }
        void AddVectorIndex(int vectorIdx);
        void ClearAssignments();
    }
}