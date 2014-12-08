using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Models
{
    /// <summary>
    /// Activation function of neural network layer
    /// </summary>
    public interface IActivationFunction
    {
        /// <summary>
        /// Calculates outputs given the inputs
        /// </summary>
        /// <param name="input">Input numbers</param>
        /// <returns>Vector of results</returns>
        double[] CalculateOutputs(double[] input);

        /// <summary>
        /// Calcuates derivative of the function, given the input data
        /// </summary>
        /// <param name="input">Input data vector</param>
        /// <returns>Vector of results</returns>
        double[] CalculateDerivative(double[] input);
    }
}
