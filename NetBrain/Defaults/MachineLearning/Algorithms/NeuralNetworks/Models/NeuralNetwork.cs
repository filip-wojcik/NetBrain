using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Models;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Models
{
    public class NeuralNetworkBuildingParams
    {
        public int HiddenLayersCount { get; set; }
        public int HiddenNeuronsCount { get; set; }
        public int InputsCount { get; set; }
        public int OutputsCount { get; set; }
        public IActivationFunction HiddenNeuronsActivationFunction { get; set; }
        public IActivationFunction OutputActivationFunction { get; set; }
        public IFeatureDomain WeightsMinMaxValues { get; set; }
        
        /// <summary>
        /// Injectable factory method for building input layer
        /// </summary>
        public BaseNetworkLayer.NetworkLayerFactoryMethod InputLayerFactoryMethod { get; set; }

        public BaseNetworkLayer.NetworkLayerFactoryMethod LayerFactoryMethod { get; set; }
    }

    public class NeuralNetwork : INeuralNetwork
    {
        # region Public properties

        public IList<INetworkLayer> Layers { get; set; }

        public int AllLayersCount { get { return this.HiddenLayersCount + 2; } }

        public int HiddenLayersCount { get; private set; }

        public int HiddenNeuronsCount { get; private set; }

        public int InputsCount { get; private set; }

        public int OutputsCount { get; private set; }

        public INetworkLayer this[int index]
        {
            get { return this.Layers[index]; }
            set { this.Layers[index] = value; }
        }

        # endregion Public properties

        public NeuralNetwork(NeuralNetworkBuildingParams neuralNetworkBuildingParams)
        {
            HiddenLayersCount = neuralNetworkBuildingParams.HiddenLayersCount;
            HiddenNeuronsCount = neuralNetworkBuildingParams.HiddenNeuronsCount;
            InputsCount = neuralNetworkBuildingParams.InputsCount;
            OutputsCount = neuralNetworkBuildingParams.OutputsCount;

            this.SetupStructure(
                neuralNetworkBuildingParams.HiddenNeuronsActivationFunction,
                neuralNetworkBuildingParams.OutputActivationFunction,
                neuralNetworkBuildingParams.InputLayerFactoryMethod,
                neuralNetworkBuildingParams.LayerFactoryMethod,
                neuralNetworkBuildingParams.WeightsMinMaxValues);
        }

        private void SetupStructure(
            IActivationFunction hiddenNeuronsActivationFunction, 
            IActivationFunction outputActivationFunction, 
            BaseNetworkLayer.NetworkLayerFactoryMethod inputLayerFactoryMethod,
            BaseNetworkLayer.NetworkLayerFactoryMethod layerFactoryMethod,
            IFeatureDomain weightsMinMaxValues = null)
        {
            this.Layers = new List<INetworkLayer>();
            this.Layers.Add(inputLayerFactoryMethod(this.InputsCount, this.HiddenNeuronsCount, weightsMinMaxValues));
            for (int i = 0; i < this.HiddenLayersCount - 1; i++)
            {
                this.Layers.Add(layerFactoryMethod(this.HiddenNeuronsCount, this.HiddenNeuronsCount, weightsMinMaxValues, hiddenNeuronsActivationFunction));
            }
            this.Layers.Add(layerFactoryMethod(this.HiddenNeuronsCount, this.OutputsCount, weightsMinMaxValues, hiddenNeuronsActivationFunction));
            this.Layers.Add(layerFactoryMethod(this.OutputsCount, 0, weightsMinMaxValues, outputActivationFunction));
        }

        # region Processing methods

        public double[] CalculateOutput(double[] input)
        {
            double[] previousLayerOutput = new double[input.Length];
            input.CopyTo(previousLayerOutput, 0);
            foreach (var layer in this.Layers)
            {
                double[] layerOutput = layer.CalculateOutput(previousLayerOutput);
                if (layer.NextLayerNeuronsCount == 0)
                {
                    previousLayerOutput = layerOutput;
                    //layerOutput.CopyTo(previousLayerOutput, 0);
                    break;
                }
                else
                {
                    double[] sums = new double[layer.NextLayerNeuronsCount];
                    for (int currentLayerNeuronIdx = 0;
                        currentLayerNeuronIdx < layer.NeuronsCount;
                        currentLayerNeuronIdx++)
                    {
                        for (int nextLayerNeuronIdx = 0;
                            nextLayerNeuronIdx < layer.NextLayerNeuronsCount;
                            nextLayerNeuronIdx++)
                        {
                            sums[nextLayerNeuronIdx] += layerOutput[currentLayerNeuronIdx]*
                                                       layer.NextLayerWeights[currentLayerNeuronIdx, nextLayerNeuronIdx];
                        }
                    }
                   // sums.CopyTo(previousLayerOutput, 0);
                    previousLayerOutput = sums;
                }
            }
            return previousLayerOutput;
        }

        # endregion Processing methods

        # region Enumerable methods

        public IEnumerator<INetworkLayer> GetEnumerator()
        {
            return this.Layers.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Layers.AsEnumerable().GetEnumerator();
        }

        # endregion Enumerable methods
    }
}
