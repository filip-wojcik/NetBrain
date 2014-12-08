using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Algorithms.Clusterization.KMeans;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Defaults.MachineLearning.Algorithms.Clusterization;
using NetBrain.Defaults.MachineLearning.Data.Standarization;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Clusterization
{
    [TestClass()]
    public class KMeansIncrementalClusterizatorTests
    {
        [TestMethod]
        public void PerformClusterizationTest_ClusterizeAbstractDataSet()
        {
            //Given
            var testData = ClusterizationTestDataBuilder.AbstractClusteriztaionDataSet();
            var standardizer = new Standarizator<double>(null, data => new ScalingNormalizer<double>(num => num, data),
                null, num => num, num => num);
            var subject = new KMeansIncrementalClusterizator(3, DistanceFunctions.EuclideanVectorsDistance, new IncrementalCentroidUpdater(),
               new RandomClusterBuilder());
            standardizer.PrepareEncoders(testData);
            var standardizedData = standardizer.Standardize(testData);

            //When
            IList<ICentroid> clusters = subject.PerformClusterization(standardizedData);
          
            //Then
            Assert.AreEqual(3, clusters.Count);
            Assert.IsFalse(clusters.Any(cluster => cluster.AssignedVectorsIndexes.Count == 0));
            int testDataCount = testData.Count();
            int clusterMaxCount = Convert.ToInt32((testDataCount / 3) + (testDataCount * 0.15));  //Maximal deviation in plus
            int clusterMinCount = Convert.ToInt32((testDataCount / 3) - (testDataCount * 0.15)); //Maximal deviation in minus
            Assert.IsTrue(
                clusters.All(
                    cluster => cluster.AssignedVectorsIndexes.Count() >= clusterMinCount && cluster.AssignedVectorsIndexes.Count() <= clusterMaxCount //Every cluster should lay in +/- deviation limits
                  )
              );
        }

        [TestMethod]
        public void PerformClusterizationTest_ClusterizeIrisSetosa()
        {
            //Given
            var testData = ClusterizationTestDataBuilder.BuildIrisSetosaDataSet();
            var standardizer = new Standarizator<double>(null, data => new ScalingNormalizer<double>(num => num, data),
                null, num => num, num => num);
            var subject = new KMeansIncrementalClusterizator(3, DistanceFunctions.EuclideanVectorsDistance, new IncrementalCentroidUpdater(),
               new RandomClusterBuilder());
            standardizer.PrepareEncoders(testData);
            var standardizedData = standardizer.Standardize(testData);

            //When
            IList<ICentroid> clusters = subject.PerformClusterization(standardizedData);

            //Then
            Assert.AreEqual(3, clusters.Count);
            Assert.IsFalse(clusters.Any(cluster => cluster.AssignedVectorsIndexes.Count == 0));
            int testDataCount = testData.Count();
            int clusterMaxCount = Convert.ToInt32((testDataCount / 3) + (testDataCount * 0.01));  //Maximal deviation in plus
            int clusterMinCount = Convert.ToInt32((testDataCount / 3) - (testDataCount * 0.01)); //Maximal deviation in minus
            Assert.IsTrue(
                clusters.All(
                    cluster => cluster.AssignedVectorsIndexes.Count() >= clusterMinCount && cluster.AssignedVectorsIndexes.Count() <= clusterMaxCount //Every cluster should lay in +/- deviation limits
                  )
              );

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
    }
}
