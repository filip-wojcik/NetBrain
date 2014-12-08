using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Trainers;

namespace NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Trainers
{
    public class SoftmaxSolutionInterpreter : ISolutionInterpreter
    {

        public IEnumerable<double> InterpretSolution(IEnumerable<double> solution)
        {
            int maxIdx = 0;
            double maxValue = double.MinValue;
            for(int i = 0; i < solution.Count(); i++)
            {
                if (solution.ElementAt(i) > maxValue)
                {
                    maxValue = solution.ElementAt(i);
                    maxIdx = i;
                }
            }
            var interpretedSolution = Enumerable.Repeat(0.0, solution.Count()).ToList();
            interpretedSolution[maxIdx] = 1.0;
            return interpretedSolution;
        }
    }
}
