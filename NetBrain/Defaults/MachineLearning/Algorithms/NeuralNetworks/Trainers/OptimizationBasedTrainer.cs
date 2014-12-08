namespace NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Trainers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Abstracts.MachineLearning.Algorithms.NeuralNetworks.Models;
    using Abstracts.MachineLearning.Algorithms.NeuralNetworks.Trainers;
    using Abstracts.MachineLearning.Algorithms.OptimizationAlgorithms;
    using Abstracts.MachineLearning.Models.Data;
    using Abstracts.MachineLearning.QualityCheckers;
    using Data.Models;
    using QualityCheckers;

    public class OptimizationBasedTrainer : INeuralNetworkTrainer
    {
        # region Public properties

        public ITrainingStrategy TrainingStrategy { get; set; }
        public Abstracts.MachineLearning.QualityCheckers.IQualityChecker<double> QualityChecker { get; set; }
        public IFeatureDomain MinMaxWeights { get; set; }
        public ISolutionInterpreter SolutionInterpreter { get; set; }
        public int OptimizationAlgorithmIterations { get; set; }
        public IOptimizationAlgorithm OptimizationAlgorithm { get; set; }
        public IQualityCheckLogger Logger { get; set; }

        # endregion Public properties

        # region Processing methods

        public void Train(INeuralNetwork neuralNetwork, IDataSet<double> dataSet)
        {
            this.TrainingStrategy.Train(neuralNetwork, dataSet, this);
        }

        public void ProcessTrainingData(INeuralNetwork neuralNetwork, IList<IFeatureVector<double>> trainingData, int iteration)
        {
            IProblemDomain problemDomain = this.BuildProblemDomain(neuralNetwork);
            this.OptimizationAlgorithm.SolutionChecker = solution => this.CheckSolution(solution, neuralNetwork, trainingData, iteration, false);
            IEnumerable<double> bestSolutionFound = this.OptimizationAlgorithm.Solve(problemDomain, this.OptimizationAlgorithmIterations);
            this.SetWeightsFromVectorOfParams(bestSolutionFound, neuralNetwork);
        }

        public void ProcessTestData(INeuralNetwork neuralNetwork, IList<IFeatureVector<double>> testData, int iteration)
        {
            IProblemDomain problemDomain = this.BuildProblemDomain(neuralNetwork);
            this.EvaluateData(neuralNetwork, testData, iteration, true);
        }

        public double CheckSolution(
            IEnumerable<double> solution, 
            INeuralNetwork neuralNetwork, 
            IEnumerable<IFeatureVector<double>> vectorsToTest, 
            int iteration,
            bool testData)
        {
            this.SetWeightsFromVectorOfParams(solution, neuralNetwork);
            return EvaluateData(neuralNetwork, vectorsToTest, iteration, testData);
        }

        private double EvaluateData(INeuralNetwork neuralNetwork, IEnumerable<IFeatureVector<double>> vectorsToTest, int iteration, bool testData)
        {
            var actualSolutions = new ConcurrentBag<IExpectedActualPair<double>>();
            Parallel.ForEach(vectorsToTest, vector =>
            {
                IFeatureVector<double> expected = vector.ValuesVector;
                IEnumerable<double> actualResult = this.SolutionInterpreter.InterpretSolution(
                    neuralNetwork.CalculateOutput(vector.NonValuesVector.ToArray())
                    );
                actualSolutions.Add(new ExpectedActualPair<double>(expected.ToList(), actualResult.ToList()));
            });

            IQualityData qualityData = this.QualityChecker.MeasureQualityData(actualSolutions, iteration, testData);
            if (qualityData.ErrorRate < this.TrainingStrategy.StoppingError) this.TrainingStrategy.Break = true;
            if (this.Logger != null) this.Logger.LogProgress(qualityData);
            return qualityData.ErrorRate;
        }

        public IProblemDomain BuildProblemDomain(INeuralNetwork neuralNetwork)
        {
            var featureDomains = new IFeatureDomain[this.ParametersCount(neuralNetwork)];
            for(int i = 0; i < featureDomains.Length; i++) featureDomains[i] = new FeatureDomain(this.MinMaxWeights.MinValue, this.MinMaxWeights.MaxValue);
            return new ProblemDomain(featureDomains);
        }

        # endregion Processing methods

        # region Transformations methods

        public int ParametersCount(INeuralNetwork neuralNetwork)
        {
            return
               (neuralNetwork.InputsCount * neuralNetwork.HiddenNeuronsCount) +                // Inputs to hidden weights count
               (neuralNetwork.HiddenNeuronsCount * neuralNetwork.HiddenLayersCount) +          // Hidden layers biases
               (
                    (
                       (neuralNetwork.HiddenNeuronsCount * neuralNetwork.HiddenNeuronsCount) *    // Hidden layers to hidden layers weights (except last hidden layer)
                       (neuralNetwork.HiddenLayersCount - 1)
                    )
               ) +
               (neuralNetwork.HiddenNeuronsCount * neuralNetwork.OutputsCount) +               // Last hidden layer to outputs weights
               (neuralNetwork.OutputsCount);                                                   // Outputs biases;
        }

        public IEnumerable<double> BuildVectorOfParams(INeuralNetwork neuralNetwork)
        {
            var vectorOfParams = new List<double>();
            for (int i = 0; i < neuralNetwork.Layers.Count; i++)
            {
                INetworkLayer currentNetworkLayer = neuralNetwork[i];
                if (i != 0)
                {
                    vectorOfParams.AddRange(currentNetworkLayer.Biases);
                }
                if (i != neuralNetwork.Layers.Count - 1)
                {
                    for (int currentLayerNeuronIdx = 0; currentLayerNeuronIdx < currentNetworkLayer.NeuronsCount; currentLayerNeuronIdx++)
                    {
                        for (int nextLayerNeuronIdx = 0;
                            nextLayerNeuronIdx < currentNetworkLayer.NextLayerNeuronsCount;
                            nextLayerNeuronIdx++)
                        {
                            vectorOfParams.Add(currentNetworkLayer.NextLayerWeights[currentLayerNeuronIdx, nextLayerNeuronIdx]);
                        }
                    }
                }
            }
            return vectorOfParams;
        }

        public void SetWeightsFromVectorOfParams(IEnumerable<double> parameterValues, INeuralNetwork neuralNetwork)
        {
            if(parameterValues.Count() != this.ParametersCount(neuralNetwork)) throw new ArgumentException("Invalid parameters count");
            int offset = 0;

            for (int i = 0; i < neuralNetwork.Layers.Count; i++)
            {
                INetworkLayer currentLayer = neuralNetwork[i];

                if (i != 0)
                {
                    currentLayer.Biases = parameterValues.Skip(offset).Take(currentLayer.NeuronsCount).ToArray();
                    offset += currentLayer.NeuronsCount;
                }
                if (i != neuralNetwork.Layers.Count - 1)
                {
                    int weightsCount = currentLayer.NextLayerNeuronsCount*currentLayer.NeuronsCount;
                    IList<double> weights = parameterValues.Skip(offset).Take(weightsCount).ToList();
                    offset += weightsCount;
                    int currentWeightIdx = 0;
                    for (int currentLayerNeuronIdx = 0; currentLayerNeuronIdx < currentLayer.NeuronsCount; currentLayerNeuronIdx++)
                    {
                        for (int nextLayerNeuronIdx = 0;
                            nextLayerNeuronIdx < currentLayer.NextLayerNeuronsCount;
                            nextLayerNeuronIdx++)
                        {
                            currentLayer.NextLayerWeights[currentLayerNeuronIdx, nextLayerNeuronIdx] =
                                weights[currentWeightIdx];
                            currentWeightIdx++;
                        }
                    }
                }
            }
        }

        # endregion Transformations methods
    }
}
