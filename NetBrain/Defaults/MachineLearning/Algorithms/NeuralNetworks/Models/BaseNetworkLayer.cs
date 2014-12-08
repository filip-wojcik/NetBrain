using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Models;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Models
{
    public abstract class BaseNetworkLayer : INetworkLayer
    {
        public delegate INetworkLayer NetworkLayerFactoryMethod(
            int neuronsCount, int nextLayerNeuronsCount, IFeatureDomain minMaxWeightsValues,
            IActivationFunction activationFunction = null);

        # region Public properties

        public int NeuronsCount { get; protected set; }
        public int NextLayerNeuronsCount { get; protected set; }

        /// <summary>
        /// Redundant in input layer
        /// </summary>
        public IActivationFunction ActivationFunction { get; protected set; }

        public double[,] NextLayerWeights { get; set; }

        /// <summary>
        /// Redundant in input layer
        /// </summary>
        public double[] Biases { get; set; }

        /// <summary>
        /// Redundant in input layer
        /// </summary>
        public double[] Sums { get; set; }

        public double[] Outputs { get; set; }

        # endregion Public properties

        # region Construction

        protected BaseNetworkLayer(int neuronsCount, int nextLayerNeuronsCount, IFeatureDomain minMaxWeightsValues, IActivationFunction activationFunction = null)
        {
            NeuronsCount = neuronsCount;
            NextLayerNeuronsCount = nextLayerNeuronsCount;
            ActivationFunction = activationFunction;
            this.BuildStructure(minMaxWeightsValues);
        }

        private void BuildStructure(IFeatureDomain minMaxWeightsValues)
        {
            this.Sums = new double[this.NeuronsCount];
            this.Outputs = new double[this.NeuronsCount];
            this.Biases = new double[this.NeuronsCount];

            this.NextLayerWeights = new double[this.NeuronsCount, this.NextLayerNeuronsCount];

            if (minMaxWeightsValues != null)
            {
                this.PopulateStructures(minMaxWeightsValues);
            }
            
        }

        private void PopulateStructures(IFeatureDomain minMaxWeightsValues)
        {
            for (int i = 0; i < this.NeuronsCount; i++)
            {
                this.Biases[i] = Randomization.RandomDoubleInRange(minMaxWeightsValues.MinValue,
                    minMaxWeightsValues.MaxValue);

                for (int j = 0; j < this.NextLayerNeuronsCount; j++)
                {
                    this.NextLayerWeights[i, j] = Randomization.RandomDoubleInRange(minMaxWeightsValues.MinValue,
                    minMaxWeightsValues.MaxValue);
                }
            }
        }

        # endregion Construction

        public abstract double[] CalculateOutput(double[] input);
    }
}
