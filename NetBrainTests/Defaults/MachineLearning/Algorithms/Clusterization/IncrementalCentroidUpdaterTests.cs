using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Defaults.MachineLearning.Algorithms.Clusterization;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Clusterization
{
    [TestClass()]
    public class IncrementalCentroidUpdaterTests
    {
        [TestMethod()]
        public void IncrementallyUpdateCentroidTest()
        {
            //Given
            var dataSet = new DataSet<double>(new string[0], 2, new int[0], new List<FeatureVector<double>>()
            {
                new FeatureVector<double>(new double[]{ 1, 2 }),
                new FeatureVector<double>(new double[]{ 3, 4 })
            });
            var existingCentroid = new Centroid(new HashSet<int>() { 0, 1 }, new double[] { 6, 8 });
            var subject = new IncrementalCentroidUpdater();

            //When
            subject.AssignVector(existingCentroid, dataSet, 0);
            subject.AssignVector(existingCentroid, dataSet, 1);

            //Then
            Assert.AreEqual(2.75, existingCentroid.Values[0]);
            Assert.AreEqual(4.0, existingCentroid.Values[1]);
        }
    }
}
