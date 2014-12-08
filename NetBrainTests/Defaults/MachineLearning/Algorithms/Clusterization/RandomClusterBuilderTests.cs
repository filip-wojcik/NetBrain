using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans;
using NetBrain.Defaults.MachineLearning.Algorithms.Clusterization;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Clusterization
{
    [TestClass()]
    public class RandomClusterBuilderTests
    {
        [TestMethod()]
        public void BuildInitialClustersTest()
        {
            //Given
            var dataSet = ClusterizationTestDataBuilder.AbstractClusteriztaionDataSet();
            var subject = new RandomClusterBuilder();

            //When
            IList<ICentroid> clusters = subject.BuildInitialClusters(dataSet, 3, new StandardCentroidUpdater());

            //Then
            Assert.AreEqual(3, clusters.Count);
            Assert.IsTrue(clusters.All(
                cluster => cluster.AssignedVectorsIndexes.Count() >= 6 &&                      //All clusters have 20/3 vectors, except one
                           !cluster.AssignedVectorsIndexes.Any(idx => idx > dataSet.Count())) //None vector index is bigger than vectors count
                           );  

            Assert.IsTrue(
                //All vector indexes has been assigned - so ordered vector indexes should be equal 0..dataSet count;
                clusters.SelectMany(cluster => cluster.AssignedVectorsIndexes).OrderBy(num => num).SequenceEqual(Enumerable.Range(0, dataSet.Count()))
                );

            Assert.IsFalse(
                //But assigned vector indexes are in randomized order in centroid
                clusters.SelectMany(cluster => cluster.AssignedVectorsIndexes).SequenceEqual(Enumerable.Range(0, dataSet.Count()))
                );
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildInitialClustersTest_ExpectedException()
        {
            //Given
            var dataSet = ClusterizationTestDataBuilder.AbstractClusteriztaionDataSet();
            var subject = new RandomClusterBuilder();

            //When
            IList<ICentroid> clusters = subject.BuildInitialClusters(dataSet, 21, new StandardCentroidUpdater());
        }
    }
}
