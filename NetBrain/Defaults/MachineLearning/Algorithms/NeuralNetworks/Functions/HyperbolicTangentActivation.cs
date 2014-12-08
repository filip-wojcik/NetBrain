using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;

namespace NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Functions
{
    public class HyperbolicTangentActivation : BaseActivationFunction
    {
        public HyperbolicTangentActivation() 
            : base(NeuralNetowrksFunctions.HyperbolicTangentActivation, NeuralNetowrksFunctions.HyperbolicTangentDerivative)
        {
        }
    }
}
