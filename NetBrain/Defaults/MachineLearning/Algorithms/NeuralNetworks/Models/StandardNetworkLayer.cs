using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Models;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Models
{
    public class StandardNetworkLayer : BaseNetworkLayer
    {
        public static StandardNetworkLayer StandardLayerFactory(int neuronsCount, int nextLayerNeuronsCount,
            IFeatureDomain minMaxWeightsValues,
            IActivationFunction activationFunction = null)
        {
            return new StandardNetworkLayer(neuronsCount, nextLayerNeuronsCount, activationFunction, minMaxWeightsValues);
        }

        public StandardNetworkLayer(
            int neuronsCount, 
            int nextLayerNeuronsCount,
            IActivationFunction activationFunction,
            IFeatureDomain minMaxWeightsValues = null
            )
            : base(neuronsCount, nextLayerNeuronsCount, minMaxWeightsValues, activationFunction)
        {
        }

        public override double[] CalculateOutput(double[] input)
        {
            input.CopyTo(this.Sums, 0);
            for (int i = 0; i < input.Length; i++)
            {
                input[i] += this.Biases[i];
            }
            this.Outputs = this.ActivationFunction.CalculateOutputs(input);
            return this.Outputs;
        }
    }
}
