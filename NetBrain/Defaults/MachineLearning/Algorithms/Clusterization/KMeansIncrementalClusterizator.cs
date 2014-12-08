using System.Collections.Concurrent;
using NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans;
using System;
using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Clusterization
{
    public class KMeansIncrementalClusterizator : KMeansClusterizator
    {
        # region Construction

        public IIncrementalCentroidUpdater IncrementalCentroidUpdater { get; private set; }

        public KMeansIncrementalClusterizator(int clustersCount, DistanceCalculator distanceCalculator, IIncrementalCentroidUpdater centroidUpdater, IInitialClustersBuilder initialClustersBuilder)
            : base(clustersCount, distanceCalculator, centroidUpdater, initialClustersBuilder)
        {
            this.IncrementalCentroidUpdater = centroidUpdater;
        }

        # endregion Construction

        # region Processing methods

        public override IList<ICentroid> PerformClusterization(IDataSet<double> dataSet)
        {
            IList<ICentroid> centroids = this.InitialClustersBuilder.BuildInitialClusters(dataSet, this.ClustersCount,
                this.CentroidUpdater);
            IDictionary<int, int> actualVectorAssignments = new Dictionary<int, int>();
            if (centroids.All(cluster => !cluster.AssignedVectorsIndexes.Any()))
            {
                IList<ICentroid> clonedCentroids = this.CloneCentroids(centroids);
                ClearCentroidsAssignments(clonedCentroids);
                Tuple<IDictionary<int, int>, bool> newAssignments = this.IncrementallyAssignVectorsToClusters(clonedCentroids,
                    dataSet, actualVectorAssignments);
                if (this.NewClustersAssignmentIsValid(newAssignments.Item1))
                {
                    centroids = clonedCentroids;
                    actualVectorAssignments = new Dictionary<int, int>(newAssignments.Item1);
                }
                else return centroids;
            }
            else
            {
                actualVectorAssignments = this.PreviousAssignmentsDictionary(centroids);
            }
            while (true)
            {
                IList<ICentroid> clonedCentroids = this.CloneCentroids(centroids);
                ClearCentroidsAssignments(clonedCentroids);
                Tuple<IDictionary<int, int>, bool> result = this.IncrementallyAssignVectorsToClusters(clonedCentroids, dataSet,
                    actualVectorAssignments);
                if (result.Item2 == false || !this.NewClustersAssignmentIsValid(result.Item1)) break;
                centroids = clonedCentroids;
                actualVectorAssignments = new Dictionary<int, int>(result.Item1);
            }
            return centroids;
        }

        public override Tuple<IDictionary<int, int>, bool> AssignVectorsToCentroids(IList<ICentroid> centroids, IDataSet<double> dataSet)
        {
            var previousAssignmetns = this.PreviousAssignmentsDictionary(centroids);
            return this.IncrementallyAssignVectorsToClusters(centroids, dataSet, previousAssignmetns);
        }

        public virtual Tuple<IDictionary<int, int>, bool> IncrementallyAssignVectorsToClusters(
             IList<ICentroid> centroids,
            IDataSet<double> dataSet,
            IDictionary<int, int> previousAssignments
            )
        {
            var assignment = new Dictionary<int, int>();
            bool anythingChanged = false;

            for (int vectorIdx = 0; vectorIdx < dataSet.Count(); vectorIdx++)
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
                this.IncrementalCentroidUpdater.AssignVector(centroids[closestCentroidIdx], dataSet, vectorIdx);
            }
            return new Tuple<IDictionary<int, int>, bool>(assignment, anythingChanged);
        }

        # endregion Processing methods

        # region Utility methods

        public IList<ICentroid> CloneCentroids(IList<ICentroid> centroids)
        {
            return centroids.Select(this.CloneCentroid).Cast<ICentroid>().ToList();
        }

        protected ICentroid CloneCentroid(ICentroid existingCentroid)
        {
            return new Centroid(new SortedSet<int>(existingCentroid.AssignedVectorsIndexes),
                new List<double>(existingCentroid.Values));
        }

        # endregion Utility methods
    }
}
