using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Models
{
    /// <summary>
    /// Neural network built from the sequence of consecutive layers
    /// </summary>
    public interface INeuralNetwork : IEnumerable<INetworkLayer>
    {
        IList<INetworkLayer> Layers { get; set; } 

        int AllLayersCount { get; }
        int HiddenLayersCount { get; }
        int HiddenNeuronsCount { get; }
        int InputsCount { get; }
        int OutputsCount { get; }
        INetworkLayer this[int index] { get; set; }

        /// <summary>
        /// Calculates output of the network
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        double[] CalculateOutput(double[] input);
    }
}
