using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Models;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Models
{
    public class InputNetworkLayer : BaseNetworkLayer
    {
        public static InputNetworkLayer InputLayerFactory(int neuronsCount, int nextLayerNeuronsCount,
            IFeatureDomain minMaxWeightsValues,
            IActivationFunction activationFunction = null)
        {
            return new InputNetworkLayer(neuronsCount, nextLayerNeuronsCount, minMaxWeightsValues);
        }

        public InputNetworkLayer(int neuronsCount, int nextLayerNeuronsCount, IFeatureDomain minMaxWeightsValues)
            : base(neuronsCount, nextLayerNeuronsCount, minMaxWeightsValues)
        {
        }

        public override double[] CalculateOutput(double[] input)
        {
            input.CopyTo(this.Outputs, 0);
            return input;
        }
    }
}
