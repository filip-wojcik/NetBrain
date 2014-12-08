using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Defaults.MachineLearning.Algorithms.Clusterization;
using NetBrain.Defaults.MachineLearning.Data.Models;
using NetBrain.Defaults.MachineLearning.Data.Standarization;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Clusterization
{
    [TestClass()]
    public class ClusterizatorTests
    {
        # region Utility methods

        [TestMethod()]
        public void AssignVectorsToClustersTest_WithoutPreviousAssignment_AnythingChangedExpected()
        {
            //Given
            var testData = ClusterizationTestDataBuilder.AbstractClusteriztaionDataSet();
            var clusters = new ICentroid[]
            {
                new Centroid(new HashSet<int>(){ 0, 1, 2 }, new double[]{ 0.6, 0.1 }), 
                new Centroid(new HashSet<int>(){ 3, 4, 5 }, new double[]{ 0.725, 0.25 }),
                new Centroid(new HashSet<int>(){ 6, 7, 8 }, new double[]{ 0.65, 0.22 }),
            };
            var subject = new KMeansClusterizator(3, DistanceFunctions.EuclideanVectorsDistance, new StandardCentroidUpdater(),
                new ForgyClusterBuilder());

            //When
            Tuple<IDictionary<int, int>, bool> newAssignment = subject.AssignVectorsToCentroids(clusters, testData);

            //Then
            Assert.IsTrue(newAssignment.Item2);
        }

        [TestMethod]
        public void AssignmentIsValid_ReusltTrue()
        {
            //Given
            var subject = new KMeansClusterizator(3, DistanceFunctions.EuclideanVectorsDistance, new StandardCentroidUpdater(),
              new ForgyClusterBuilder());
            var assignment = new Dictionary<int, int>()
            {
                {1, 0},
                {2, 1},
                {3, 2}
            };

            //Then
            Assert.IsTrue(subject.NewClustersAssignmentIsValid(assignment));
        }

        [TestMethod]
        public void AssignmentIsValid_ReusltFalse()
        {
            //Given
            var subject = new KMeansClusterizator(3, DistanceFunctions.EuclideanVectorsDistance, new StandardCentroidUpdater(),
              new ForgyClusterBuilder());
            var assignment = new Dictionary<int, int>()
            {
                {1, 1},
                {2, 1},
                {3, 2}
            };

            //Then
            Assert.IsFalse(subject.NewClustersAssignmentIsValid(assignment));
        }

        [TestMethod()]
        public void PreviousAssignmentsDictionaryTest()
        {
            //Given
            var testData = ClusterizationTestDataBuilder.AbstractClusteriztaionDataSet();
            var clusters = new ICentroid[]
            {
                new Centroid(new HashSet<int>(){ 0, 1, 2 }, new double[0]), 
                new Centroid(new HashSet<int>(){ 3, 4, 5 }, new double[0]),
                new Centroid(new HashSet<int>(){ 6, 7, 8 }, new double[0]),
            };
            var subject = new KMeansClusterizator(3, DistanceFunctions.EuclideanVectorsDistance, new StandardCentroidUpdater(),
                new ForgyClusterBuilder());

            //When
            var assignemntDictionary = subject.PreviousAssignmentsDictionary(clusters);

            //Then
            //First centroid vectors
            Assert.AreEqual(0, assignemntDictionary[0]);
            Assert.AreEqual(0, assignemntDictionary[1]);
            Assert.AreEqual(0, assignemntDictionary[2]);

            //Second centroid vectors
            Assert.AreEqual(1, assignemntDictionary[3]);
            Assert.AreEqual(1, assignemntDictionary[4]);
            Assert.AreEqual(1, assignemntDictionary[5]);

            //First centroid vectors
            Assert.AreEqual(2, assignemntDictionary[6]);
            Assert.AreEqual(2, assignemntDictionary[7]);
            Assert.AreEqual(2, assignemntDictionary[8]);
        }

        # endregion Utility methods

        # region Processing methods

        [TestMethod]
        public void FindClosestCentroidIdx_Test()
        {
            //Given
            var vector = new FeatureVector<double>(new[] {2.0, 2.0, 2.0});
            var centroids = new List<ICentroid>()
            {
                new Centroid(new[] {1.1, 1.1, 1.1}),
                new Centroid(new[] {1.7, 1.7, 1.7}),
                new Centroid(new[] {2.2, 2.2, 2.2})
            };
            var subject = new KMeansClusterizator(3, DistanceFunctions.EuclideanVectorsDistance, new StandardCentroidUpdater(),
                new ForgyClusterBuilder());

            //When
            int closestCluster = subject.FindClosestCentroid(vector, centroids);

            //Then
            Assert.AreEqual(2, closestCluster);
        }

        [TestMethod]
        public void PerformClusterizationTest_ClusterizeAbstractDataSet()
        {
            //Given
            var testData = ClusterizationTestDataBuilder.AbstractClusteriztaionDataSet();
            var standardizer = new Standarizator<double>(null, data => new ScalingNormalizer<double>(num => num, data),
                null, num => num, num => num);
            var subject = new KMeansClusterizator(3, DistanceFunctions.EuclideanVectorsDistance, new StandardCentroidUpdater(),
               new RandomClusterBuilder());
            standardizer.PrepareEncoders(testData);
            var standardizedData = standardizer.Standardize(testData);


            //When
            IList<ICentroid> clusters = subject.PerformClusterization(standardizedData);
           
            //Then
            Assert.AreEqual(3, clusters.Count);
            Assert.IsFalse(clusters.Any(cluster => cluster.AssignedVectorsIndexes.Count == 0));
            
        }

        [TestMethod]
        public void PerformClusterizationTest_ClusterizeIrisSetosa()
        {
            //Given
            var testData = ClusterizationTestDataBuilder.BuildIrisSetosaDataSet();
            var standardizer = new Standarizator<double>(null, data => new ScalingNormalizer<double>(num => num, data),
                null, num => num, num => num);
            var subject = new KMeansClusterizator(3, DistanceFunctions.EuclideanVectorsDistance, new StandardCentroidUpdater(),
               new RandomClusterBuilder());
            standardizer.PrepareEncoders(testData);
            var standardizedData = standardizer.Standardize(testData);

            //When
            IList<ICentroid> clusters = subject.PerformClusterization(standardizedData);
            
            //Then
            Assert.AreEqual(3, clusters.Count);
            Assert.IsFalse(clusters.Any(cluster => cluster.AssignedVectorsIndexes.Count == 0));


            var classes = new IList<double>[]
            {
                new double[] {0, 0, 1},
                new double[] {0, 1, 0},
                new double[] {1, 0, 0}
            };
            
            var performanceMeasure = new double[3,3];
            for (int i = 0; i < 3; i++)
            {
                foreach (var vectorIdx in clusters[i].AssignedVectorsIndexes)
                {
                    var vector = standardizedData.ElementAt(vectorIdx);
                    var values = vector.ValuesVector.ToList();
                    for (int classIdx = 0; classIdx < 3; classIdx++)
                    {
                        if (values.SequenceEqual(classes[classIdx]))
                        {
                            performanceMeasure[i, classIdx] += 1;
                        }
                    }
                }
            }
          
        }

        # endregion Processing methods
    }
}
