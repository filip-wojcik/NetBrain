using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.MathematicalFunctions
{
    public static class NeuralNetowrksFunctions
    {
        # region Bipolar function

        public static double BipolarActivation(double input)
        {
            if (input >= 0.0)
            {
                return 1.0;
            }
            else
            {
                return -1;
            }
        }

        # endregion Bipolar function

        # region Sigmoid function

        public static double SigmoidActivation(double input)
        {
            if (input < -45)
            {
                return 0;
            }
            else if (input > 45)
            {
                return 1;
            }
            else
            {
                return (1.0 / (1.0 + Math.Exp(-input)));
            }
        }

        public static double SigmoidDerivative(double functionValue)
        {
            return (1 - functionValue) * functionValue;
        }

        # endregion Sigmoid function

        # region Hyperbolic tangens function

        public static double HyperbolicTangentActivation(double input)
        {
            if (input < -20.0)
            {
                return 0.0;
            }
            else if (input > 20.0)
            {
                return 1;
            }
            else
            {
                return Math.Tanh(input);
            }
        }

        public static double HyperbolicTangentDerivative(double functionValue)
        {
            return (1 - functionValue) * (1 + functionValue);
        }

        # endregion Hyperbolic tangens function

        # region Softmax function

        /// <summary>
        /// Softmax activation function, used for classifying 
        /// data
        /// </summary>
        /// <param name="input">Vector of values</param>
        /// <returns>Vector of calculated values</returns>
        public static double[] SoftmaxActivation(double[] input)
        {
            var inputSize = input.Count();
            var outputs = new double[inputSize];
            var maxInput = input.Max();
            double scale = 0;
            for (int i = 0; i < inputSize; i++)
            {
                scale += Math.Exp(input[i] - maxInput);
            }
            for (int i = 0; i < inputSize; i++)
            {
                outputs[i] = Math.Exp(input[i] - maxInput) / scale;
            }

            return outputs;
        }

        /// <summary>
        /// Softmax activation derivative is the same as 
        /// the sigmoid derivative
        /// </summary>
        /// <param name="functionValue">Function atomic value</param>
        /// <returns>Single double output</returns>
        public static double SoftmaxDerivative(double functionValue)
        {
            return SigmoidActivation(functionValue);
        }

        # endregion Softmax function
    }
}
