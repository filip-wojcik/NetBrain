using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.QualityCheckers;
using NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Functions;
using NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Models;
using NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Trainers;
using NetBrain.Defaults.MachineLearning.Algorithms.OptimizationAlgorithms.ParticleSwarm;
using NetBrain.Defaults.MachineLearning.Data.Models;
using NetBrain.Defaults.MachineLearning.QualityCheckers;
using NetBrainTests.Defaults.MachineLearning.Algorithms.Clusterization;
using NetBrainTests.Defaults.MachineLearning.QualityCheckers;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.NeuralNetworks.Trainers
{
    [TestClass()]
    public class OptimizationBasedTrainerTests
    {
        # region Transformation methods tests

        [TestMethod()]
        public void BuildVectorOfParamsTest()
        {
            //Given
            var subject = new OptimizationBasedTrainer();
            var neuralNetwork = new NeuralNetwork(new NeuralNetworkBuildingParams()
            {
                InputsCount = 2,
                HiddenLayersCount = 3,
                HiddenNeuronsCount = 3,
                OutputsCount = 2,
                HiddenNeuronsActivationFunction = new BipolarActivation(),
                OutputActivationFunction = new SoftmaxActivation(),
                WeightsMinMaxValues = new FeatureDomain(0.01, 1),
                InputLayerFactoryMethod = InputNetworkLayer.InputLayerFactory,
                LayerFactoryMethod = StandardNetworkLayer.StandardLayerFactory
            });

            neuralNetwork[0].NextLayerWeights = new double[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            neuralNetwork[1].Biases = new double[] { 7, 8, 9 };
            neuralNetwork[1].NextLayerWeights = new double[,]
            {
                { 10, 11, 12 },
                { 13, 14, 15 },
                { 16, 17, 18 }
            };

            neuralNetwork[2].Biases = new double[] { 19, 20, 21 };
            neuralNetwork[2].NextLayerWeights = new double[,]
            {
                { 22, 23, 24 },
                { 25, 26, 27 },
                { 28, 29, 30 }
            };
            neuralNetwork[3].Biases = new double[] { 31, 32, 33 };
            neuralNetwork[3].NextLayerWeights = new double[,]
            {
               { 34, 35 },
               { 36, 37 },
               { 38, 39 }
            };
            neuralNetwork[4].Biases = new double[] { 40, 41 };

            double[] expected = Array.ConvertAll<int, double>(Enumerable.Range(1, 41).ToArray(), Convert.ToDouble);

            //When
            IList<double> actual = subject.BuildVectorOfParams(neuralNetwork).ToList();

            //Then
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod()]
        public void SetWeightsFromVectorOfParamsTest()
        {
            //Given
            var subject = new OptimizationBasedTrainer();
            var neuralNetwork = new NeuralNetwork(new NeuralNetworkBuildingParams()
            {
                InputsCount = 2,
                HiddenLayersCount = 3,
                HiddenNeuronsCount = 3,
                OutputsCount = 2,
                HiddenNeuronsActivationFunction = new BipolarActivation(),
                OutputActivationFunction = new SoftmaxActivation(),
                WeightsMinMaxValues = new FeatureDomain(0.01, 1),
                InputLayerFactoryMethod = InputNetworkLayer.InputLayerFactory,
                LayerFactoryMethod = StandardNetworkLayer.StandardLayerFactory
            });

            var parameters = Array.ConvertAll<int, double>(Enumerable.Range(1, 41).ToArray(), Convert.ToDouble);

            //When
            subject.SetWeightsFromVectorOfParams(parameters, neuralNetwork);

            //Then
            var inputToHiddenWeights = new double[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };
            Assert.IsTrue(inputToHiddenWeights.Cast<double>().SequenceEqual(neuralNetwork[0].NextLayerWeights.Cast<double>()));

            //First hidden layer 

            Assert.IsTrue(new double[] { 7, 8, 9 }.SequenceEqual(neuralNetwork[1].Biases));

            var hidden1ToHidden2Weights = new double[,]
            {
                { 10, 11, 12 },
                { 13, 14, 15 },
                { 16, 17, 18 }
            };
            Assert.IsTrue(hidden1ToHidden2Weights.Cast<double>().SequenceEqual(neuralNetwork[1].NextLayerWeights.Cast<double>()));

            //Second hidden layer

            Assert.IsTrue(new double[] { 19, 20, 21 }.SequenceEqual(neuralNetwork[2].Biases));
            var hidden2ToHidden3Weights = new double[,]
            {
                { 22, 23, 24 },
                { 25, 26, 27 },
                { 28, 29, 30 }
            };
            Assert.IsTrue(hidden2ToHidden3Weights.Cast<double>().SequenceEqual(neuralNetwork[2].NextLayerWeights.Cast<double>()));

            //Third hidden layer

            Assert.IsTrue(new double[] { 31, 32, 33 }.SequenceEqual(neuralNetwork[3].Biases));
            var hidden3ToOutputsWeights = new double[,]
            {
                { 34, 35 },
                { 36, 37 },
                { 38, 39 }
            };
            Assert.IsTrue(hidden3ToOutputsWeights.Cast<double>().SequenceEqual(neuralNetwork[3].NextLayerWeights.Cast<double>()));

            //Output layer

            Assert.IsTrue(new double[] { 40, 41 }.SequenceEqual(neuralNetwork[4].Biases));
        }

        # endregion Transformation methods tests

        # region Training methods test

        [TestMethod]
        public void TrainTest_HoldOutTraining()
        {
            //Given
            var cancellationTokenSource = new CancellationTokenSource();
            var strategy = new HoldOutTrainingStrategy(0.8, 1, 0.001, cancellationTokenSource.Token);
            var neuralNetwork = new NeuralNetwork(new NeuralNetworkBuildingParams()
            {
                InputsCount = 4,
                HiddenLayersCount = 1,
                HiddenNeuronsCount = 6,
                OutputsCount = 3,
                HiddenNeuronsActivationFunction = new HyperbolicTangentActivation(),
                InputLayerFactoryMethod = InputNetworkLayer.InputLayerFactory,
                LayerFactoryMethod = StandardNetworkLayer.StandardLayerFactory,
                OutputActivationFunction = new SoftmaxActivation(),
                WeightsMinMaxValues = new FeatureDomain(-0.001, 0.001)
            });
            var particleSwarm = new ParticleSwarmOptimization();
            var logger = new QualityCheckerLoggerStub();
            var testData = ClusterizationTestDataBuilder.BuildIrisSetosaDataSet();
            var trainer = new OptimizationBasedTrainer()
            {
                Logger = logger,
                MinMaxWeights = new FeatureDomain(-10, 10),
                OptimizationAlgorithm = particleSwarm,
                OptimizationAlgorithmIterations = 500,
                QualityChecker = new ClassificationQualityChecker<double>(),
                SolutionInterpreter = new SoftmaxSolutionInterpreter(),
                TrainingStrategy = strategy
            };

            //When
            trainer.Train(neuralNetwork, testData);

            //Then
            double error = Double.MaxValue;
            IQualityData lastError = logger.Results.Last();
            Assert.IsTrue(lastError.ErrorRate < 0.08);
        }

        # endregion Training methods test
    }
}
