using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Functions;
using NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Models;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.NeuralNetworks.Models
{
    [TestClass()]
    public class StandardNetworkLayerTests
    {
        /// <summary>
        /// A test for initializing networkLayer with initial weights
        /// </summary>
        [TestMethod]
        public void InitializationWithWeightsTest()
        {
            //When
            var networkLayer = new StandardNetworkLayer(2, 2, new BipolarActivation(), new FeatureDomain(0.01, 1));

            //Then
            for (int i = 0; i < 2; i++)
            {
                Assert.IsTrue(networkLayer.Biases[i] >= 0 && networkLayer.Biases[i] <= 1);
                for (int j = 0; j < 2; j++)
                {
                    Assert.IsTrue(networkLayer.NextLayerWeights[i, j] >= 0 && networkLayer.NextLayerWeights[i, j] <= 1);
                }
            }
        }

        /// <summary>
        ///A test for CalcuateOutputs
        ///</summary>
        [TestMethod()]
        public void CalcuateOutputs_BipolarActivationTest()
        {
            //Given
            var subject = new StandardNetworkLayer(2, 0, new BipolarActivation());
            subject.Biases = new double[] { 1, 2 };
            var inputs = new double[] { 5, -5 };

            //When
            double[] outputs = subject.CalculateOutput(inputs);

            //Then
            Assert.AreEqual(5.0, subject.Sums[0]);
            Assert.AreEqual(-5.0, subject.Sums[1]);
            Assert.AreEqual(1.0, outputs[0]);
            Assert.AreEqual(-1.0, outputs[1]);
            Assert.AreEqual(outputs[0], subject.Outputs[0]);
            Assert.AreEqual(outputs[1], subject.Outputs[1]);
        }

        [TestMethod]
        public void CalculateOutputsWithSoftmaxTest()
        {
            //Given
            var subject = new StandardNetworkLayer(2, 2, new SoftmaxActivation());
            subject.Biases = new double[] { 1, 2 };
            var inputs = new double[] { 5, -5 };

            //When
            double[] outputs = subject.CalculateOutput(inputs);

            //Then
            Assert.AreEqual(5.0, subject.Sums[0]);
            Assert.AreEqual(-5.0, subject.Sums[1]);
            Assert.AreEqual(1.0, outputs[0], 0.09);
            Assert.AreEqual(0, outputs[1], 0.09);
            Assert.AreEqual(outputs[0], subject.Outputs[0]);
            Assert.AreEqual(outputs[1], subject.Outputs[1]);
        }
    }
}
