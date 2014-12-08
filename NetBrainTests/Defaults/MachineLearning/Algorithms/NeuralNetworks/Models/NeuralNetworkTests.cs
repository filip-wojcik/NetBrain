using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Functions;
using NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Models;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.NeuralNetworks.Models
{
    [TestClass()]
    public class NeuralNetworkTests
    {
        [TestMethod()]
        public void NeuralNetwork_BuildStructureTest()
        {
            //Given
            var networkParams = new NeuralNetworkBuildingParams()
            {
                HiddenLayersCount = 3,
                HiddenNeuronsCount = 3,
                InputsCount = 2,
                OutputsCount = 4,
                HiddenNeuronsActivationFunction = new BipolarActivation(),
                OutputActivationFunction = new SoftmaxActivation(),
                WeightsMinMaxValues = new FeatureDomain(0.01, 1),
                InputLayerFactoryMethod = InputNetworkLayer.InputLayerFactory,
                LayerFactoryMethod = StandardNetworkLayer.StandardLayerFactory
            };
            var network = new NeuralNetwork(networkParams);

            //Then
            Assert.AreEqual(5, network.Layers.Count);

            //Input layer
            Assert.AreEqual(typeof(InputNetworkLayer), network[0].GetType());
            Assert.AreEqual(2, network[0].Sums.Length);
            Assert.AreEqual(2, network[0].Biases.Length);
            Assert.AreEqual(3, network[0].NextLayerNeuronsCount);
            Assert.AreEqual(2, network[0].NextLayerWeights.GetLength(0));
            Assert.AreEqual(3, network[0].NextLayerWeights.GetLength(1));

            //1st hidden layer
            Assert.AreEqual(typeof(StandardNetworkLayer), network[1].GetType());
            Assert.AreEqual(typeof(BipolarActivation), network[1].ActivationFunction.GetType());
            Assert.AreEqual(3, network[1].Sums.Length);
            Assert.AreEqual(3, network[1].Biases.Length);
            Assert.AreEqual(3, network[1].NextLayerNeuronsCount);
            Assert.AreEqual(3, network[1].NextLayerWeights.GetLength(0));
            Assert.AreEqual(3, network[1].NextLayerWeights.GetLength(1));

            //2st hidden layer
            Assert.AreEqual(typeof(StandardNetworkLayer), network[2].GetType());
            Assert.AreEqual(typeof(BipolarActivation), network[2].ActivationFunction.GetType());
            Assert.AreEqual(3, network[2].Sums.Length);
            Assert.AreEqual(3, network[2].Biases.Length);
            Assert.AreEqual(3, network[2].NextLayerNeuronsCount);
            Assert.AreEqual(3, network[2].NextLayerWeights.GetLength(0));
            Assert.AreEqual(3, network[2].NextLayerWeights.GetLength(1));

            //3rd hidden layer
            Assert.AreEqual(typeof(StandardNetworkLayer), network[3].GetType());
            Assert.AreEqual(typeof(BipolarActivation), network[3].ActivationFunction.GetType());
            Assert.AreEqual(3, network[3].Sums.Length);
            Assert.AreEqual(3, network[3].Biases.Length);
            Assert.AreEqual(4, network[3].NextLayerNeuronsCount);
            Assert.AreEqual(3, network[3].NextLayerWeights.GetLength(0));
            Assert.AreEqual(4, network[3].NextLayerWeights.GetLength(1));

            //Output layer
            Assert.AreEqual(typeof(StandardNetworkLayer), network[4].GetType());
            Assert.AreEqual(typeof(SoftmaxActivation), network[4].ActivationFunction.GetType());
            Assert.AreEqual(4, network[4].Sums.Length);
            Assert.AreEqual(4, network[4].Biases.Length);
            Assert.AreEqual(0, network[4].NextLayerNeuronsCount);
            Assert.AreEqual(4, network[4].NextLayerWeights.GetLength(0));
            Assert.AreEqual(0, network[4].NextLayerWeights.GetLength(1));
        }

        [TestMethod()]
        public void CalculateOutputTest()
        {
            //Given
            var networkParams = new NeuralNetworkBuildingParams()
            {
                HiddenLayersCount = 1,
                HiddenNeuronsCount = 2,
                InputsCount = 2,
                OutputsCount = 2,
                HiddenNeuronsActivationFunction = new BipolarActivation(),
                OutputActivationFunction = new BipolarActivation(),
                WeightsMinMaxValues = new FeatureDomain(0.01, 1),
                InputLayerFactoryMethod = InputNetworkLayer.InputLayerFactory,
                LayerFactoryMethod = StandardNetworkLayer.StandardLayerFactory
            };
            var subject = new NeuralNetwork(networkParams);
            subject[0].NextLayerWeights = new double[,]
            {
                { 4, 2 },
                { 1, 3 }
            };
            subject[0].Biases = new double[] { 1, 2 };

            subject[1].NextLayerWeights = new double[,]
            {
                { 1, 4 },
                { 3, 2 }
            };
            subject[1].Biases = new double[] { 1, 1 };
            subject[2].Biases = new double[] { 0, 0 };

            var expected = new double[] { -1, 1 };
            var inputs = new double[] {2, -3};

            //When
            double[] actual = subject.CalculateOutput(inputs);

            //Then
            Assert.IsTrue(expected.SequenceEqual(actual));

            //Input layer
            Assert.IsTrue(new double[]{ 2, -3 }.SequenceEqual(subject[0].Outputs));

            //Hidden layer
            Assert.IsTrue(new double[] { 5, -5 }.SequenceEqual(subject[1].Sums));
            Assert.IsTrue(new double[] { 1, -1 }.SequenceEqual(subject[1].Outputs));

            //Output layer
            Assert.IsTrue(new double[] {-2, 2 }.SequenceEqual(subject[2].Sums));
            Assert.IsTrue(new double[] { -1, 1 }.SequenceEqual(subject[2].Outputs));
        }
    }
}
