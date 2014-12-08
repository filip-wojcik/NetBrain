using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Trainers
{
    /// <summary>
    /// Interprets the solution returned by neural network, according to task which evaluated,
    /// either classification or regression
    /// </summary>
    public interface ISolutionInterpreter
    {
        IEnumerable<double> InterpretSolution(IEnumerable<double> solution);
    }
}
