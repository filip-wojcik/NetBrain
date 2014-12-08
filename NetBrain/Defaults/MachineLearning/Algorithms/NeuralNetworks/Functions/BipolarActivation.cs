using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Models;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;

namespace NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Functions
{
    public class BipolarActivation : BaseActivationFunction
    {
        public BipolarActivation()
            : base(NeuralNetowrksFunctions.BipolarActivation, BaseActivationFunction.NotApplicableFunction)
        {

        }
    }
}
