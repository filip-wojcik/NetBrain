using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;

namespace NetBrainTests.Abstracts.MachineLearning.MathematicalFunctions
{
    [TestClass()]
    public class DistanceFunctionsTests
    {
        [TestMethod]
        public void EuclideanDistaneTest()
        {
            //Given
            var vector1 = new double[] { 20, 80 };
            var vector2 = new double[] { 30, 44 };

            //When
            var result = DistanceFunctions.EuclideanVectorsDistance(vector1, vector2);
            //Then
            Assert.AreEqual(37.36, result, 0.02);
        }

        [TestMethod()]
        public void ManhattanDistanceTest()
        {
            //Given
            var vector1 = new double[] { 20, 80 };
            var vector2 = new double[] { 30, 44 };

            //When
            var result = DistanceFunctions.ManhattanDistance(vector1, vector2);
            //Then
            Assert.AreEqual(46, result);
        }
    }
}
