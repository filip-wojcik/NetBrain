using NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


namespace NetBrain.Defaults.MachineLearning.Algorithms.Clusterization
{
    public class KMeansClusterizator : IClusterizator
    {
        public IInitialClustersBuilder InitialClustersBuilder { get; private set; }

        public ICentroidUpdater CentroidUpdater { get; private set; }

        public DistanceCalculator DistanceCalculator { get; private set; }

        public int ClustersCount { get; private set; }

        public KMeansClusterizator(int clustersCount, DistanceCalculator distanceCalculator, ICentroidUpdater centroidUpdater, IInitialClustersBuilder initialClustersBuilder)
        {
            ClustersCount = clustersCount;
            DistanceCalculator = distanceCalculator;
            CentroidUpdater = centroidUpdater;
            InitialClustersBuilder = initialClustersBuilder;
        }

        public virtual IList<ICentroid> PerformClusterization(IDataSet<double> dataSet)
        {
            IList<ICentroid> clusters = this.InitialClustersBuilder.BuildInitialClusters(dataSet, this.ClustersCount,
                this.CentroidUpdater);
            IDictionary<int, int> actualVectorAssignments = new Dictionary<int, int>();
            if (clusters.All(cluster => !cluster.AssignedVectorsIndexes.Any()))
            {
                actualVectorAssignments = this.AssignVectorsToCentroids(clusters, dataSet).Item1;
                if (this.NewClustersAssignmentIsValid(actualVectorAssignments))
                    this.UpdateClusters(actualVectorAssignments, clusters, dataSet);
                else
                {
                    return clusters;
                }
            }
            else
            {
                actualVectorAssignments = this.PreviousAssignmentsDictionary(clusters);
            }
            while (true)
            {
                Tuple<IDictionary<int, int>, bool> result = this.AssignVectorsToCentroids(clusters, dataSet,
                    actualVectorAssignments);
                if (result.Item2 == false || !this.NewClustersAssignmentIsValid(result.Item1)) break;
                actualVectorAssignments = new Dictionary<int, int>(result.Item1);
                this.UpdateClusters(actualVectorAssignments, clusters, dataSet);
            }
            return clusters;
        }

        public bool NewClustersAssignmentIsValid(IDictionary<int, int> newAssignment)
        {
            return (Enumerable.Range(0, this.ClustersCount)).All(newAssignment.Values.Contains);
        }

        public void UpdateClusters(IDictionary<int, int> newAssignment, IList<ICentroid> clusters, IDataSet<double> dataSet)
        {
            ClearCentroidsAssignments(clusters);
            foreach (var vectorAndCluster in newAssignment)
            {
                this.CentroidUpdater.AssignVector(clusters[vectorAndCluster.Value], dataSet, vectorAndCluster.Key);
            }
        }

        public virtual Tuple<IDictionary<int, int>, bool> AssignVectorsToCentroids(
            IList<ICentroid> centroids,
            IDataSet<double> dataSet
            )
        {
            
            var previousAssignmetns = this.PreviousAssignmentsDictionary(centroids);
            return this.AssignVectorsToCentroids(centroids, dataSet, previousAssignmetns);
        }

        public virtual Tuple<IDictionary<int, int>, bool> AssignVectorsToCentroids(
            IList<ICentroid> centroids,
            IDataSet<double> dataSet,
            IDictionary<int, int> previousAssignments 
            )
        {
            var assignment = new Dictionary<int, int>();
            bool anythingChanged = false;

            for(int vectorIdx = 0; vectorIdx < dataSet.Count(); vectorIdx++)
                       {
                IFeatureVector<double> vector = dataSet.ElementAt(vectorIdx);
                var closestCentroidIdx = FindClosestCentroid(vector, centroids);

                int previousAssignment = -2;
                if (!previousAssignments.TryGetValue(vectorIdx, out previousAssignment)) anythingChanged = true;
                if (closestCentroidIdx != previousAssignment)
                {
                    anythingChanged = true;
                }
                assignment.Add(vectorIdx, closestCentroidIdx);
            }
            return new Tuple<IDictionary<int, int>, bool>(assignment, anythingChanged);
        }

        public int FindClosestCentroid(IFeatureVector<double> vector, IList<ICentroid> centroids)
        {
            int closestClusterIdx = -1;
            double closestDistance = double.MaxValue;
            for (int clusterIdx = 0; clusterIdx < centroids.Count; clusterIdx++)
            {
                double distance = this.DistanceCalculator(vector, centroids[clusterIdx].Values);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestClusterIdx = clusterIdx;
                }
            }
            return closestClusterIdx;
        }

        public IDictionary<int, int> PreviousAssignmentsDictionary(IList<ICentroid> clusters)
        {
            var previousAssignments = new ConcurrentDictionary<int, int>();
            for(int clusterIdx = 0; clusterIdx < this.ClustersCount; clusterIdx++)
            {
                ICentroid centroid = clusters[clusterIdx];
                foreach (var vectorIndex in centroid.AssignedVectorsIndexes)
                {
                    previousAssignments.TryAdd(vectorIndex, clusterIdx);
                }
            }
            return previousAssignments;
        }

        protected static void ClearCentroidsAssignments(IList<ICentroid> clusters)
        {
            foreach (var cluster in clusters) cluster.ClearAssignments();
        }
    }
}
