using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans;
using NetBrain.Defaults.MachineLearning.Algorithms.Clusterization;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Clusterization
{
    [TestClass()]
    public class ForgyClusterBuilderTests
    {
        [TestMethod()]
        public void BuildInitialClustersTest()
        {
            //Given
            var dataSet = ClusterizationTestDataBuilder.AbstractClusteriztaionDataSet();
            var subject = new ForgyClusterBuilder();

            //When
            IList<ICentroid> clusters = subject.BuildInitialClusters(dataSet, 3, new StandardCentroidUpdater());

            //Then
            Assert.AreEqual(3, clusters.Count);
            Assert.IsTrue(clusters.All(cluster => !cluster.AssignedVectorsIndexes.Any()));
            Assert.IsTrue(
                clusters.All(cluster => dataSet.Vectors.Any(vector => vector.SequenceEqual(cluster.Values)))
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
