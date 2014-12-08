using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Algorithms.Clusterization;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Clusterization
{
    [TestClass()]
    public class StandardClusterUpdaterTests
    {
        [TestMethod()]
        public void UpdateClusterTest_ValuesNotAsVector()
        {
            //Given
            var vectorsSet = new DataSet<double>(
                new string[0], 3, new int[0], new List<IFeatureVector<double>>()
                {
                    new FeatureVector<double>(new double[] {1, 2, 3}),
                    new FeatureVector<double>(new double[] { 2, 1, 2 }),
                    new FeatureVector<double>(new double[] { 3, 3, 1 })
                });
            var cluster = new Centroid(new HashSet<int>(){ 0, 1, 2 }, new[] {0.0, 0.0, 0.0});

            var subject = new StandardCentroidUpdater();

            var expectedValues = new double[] { 2, 2, 2 };
            //When
            subject.UpdateCentroid(cluster, vectorsSet);

            //Then
            Assert.IsTrue(expectedValues.SequenceEqual(cluster.Values));
        }

        [TestMethod()]
        public void UpdateClusterTest_ValuesAsVector()
        {
            //Given
            var vectorsSet = new DataSet<double>(
                new string[0], 3, new int[0], new List<IFeatureVector<double>>()
                {
                    new FeatureVector<double>(new double[] {1, 2, 3}),
                    new FeatureVector<double>(new double[] { 2, 1, 2 }),
                    new FeatureVector<double>(new double[] { 3, 3, 1 })
                });
            var cluster = new Centroid(new HashSet<int>() { 0, 1, 2 }, new[] { 4.0, 4.0, 4.0 });

            var subject = new StandardCentroidUpdater();

            var expectedValues = new double[] { 2.5, 2.5, 2.5 };
            //When
            subject.UpdateCentroid(cluster, vectorsSet, true);

            //Then
            Assert.IsTrue(expectedValues.SequenceEqual(cluster.Values));
        }
    }
}
