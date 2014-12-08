using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Clusterization
{
    public class Centroid : ICentroid
    {
        private object _lock = new object();
        public IList<double> Values { get; set; }
        public ISet<int> AssignedVectorsIndexes { get; set; }

        public Centroid(IList<double> values)
        {
            Values = values;
            AssignedVectorsIndexes = new HashSet<int>();
        }

        public Centroid(ISet<int> assignedVectorsIndexes, IList<double> values)
        {
            this.AssignedVectorsIndexes = assignedVectorsIndexes;
            Values = values;
        }

        public void AddVectorIndex(int vectorIdx)
        {
            lock (this._lock)
            {
                this.AssignedVectorsIndexes.Add(vectorIdx);
            }
        }

        public void ClearAssignments()
        {
            this.AssignedVectorsIndexes = new HashSet<int>();
        }
    }
}
