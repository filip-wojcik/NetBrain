using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Models;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;

namespace NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Functions
{
    public class SoftmaxActivation : IActivationFunction
    {
        public SoftmaxActivation()
        {
        }

        public double[] CalculateOutputs(double[] input)
        {
            return NeuralNetowrksFunctions.SoftmaxActivation(input);
        }

        public double[] CalculateDerivative(double[] input)
        {
            var outputs = new double[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                outputs[i] = NeuralNetowrksFunctions.SoftmaxDerivative(input[i]);
            }
            return outputs;
        }
    }
}
