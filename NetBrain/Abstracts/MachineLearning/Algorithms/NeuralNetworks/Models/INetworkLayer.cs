using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Models
{
    /// <summary>
    /// Represents one layer of neural network
    /// </summary>
    public interface INetworkLayer
    {
        int NeuronsCount { get; }
        int NextLayerNeuronsCount { get; }

        IActivationFunction ActivationFunction { get; }

        double[,] NextLayerWeights { get; set; }
        double[] Biases { get; set; }
        double[] Sums { get; set; }
        double[] Outputs { get; set; }

        double[] CalculateOutput(double[] input);
    }
}
