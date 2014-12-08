using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Models;

namespace NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Functions
{
    public abstract class BaseActivationFunction : IActivationFunction
    {
        protected Func<double, double> activationCalculation;
        protected Func<double, double> derivativeCalculation;

        protected BaseActivationFunction(Func<double, double> activationCalculation, Func<double, double> derivativeCalculation)
        {
            this.activationCalculation = activationCalculation;
            this.derivativeCalculation = derivativeCalculation;
        }

        public virtual double[] CalculateOutputs(double[] input)
        {
            var outputs = new double[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                outputs[i] = this.activationCalculation(input[i]);
            }
            return outputs;
        }

        public double[] CalculateDerivative(double[] input)
        {
            var outputs = new double[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                outputs[i] = this.derivativeCalculation(input[i]);
            }
            return outputs;
        }

        protected static double NotApplicableFunction(double input)
        {
            throw new NotImplementedException();
        }
    }
}
