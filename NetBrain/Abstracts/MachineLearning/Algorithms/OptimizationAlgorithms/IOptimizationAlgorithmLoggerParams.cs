using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.OptimizationAlgorithms
{
    public interface IOptimizationAlgorithmLoggerParams
    {
        int CurrentIteration { get; set; }
        double LowestErrorFound { get; set; }
        double[] LowestErrorSolution { get; set; }

        double HighestErrorFound { get; set; }
        double[] HighestErrorSolution { get; set; }

        double CurrentIterationLowestError { get; set; }
        double[] CurrentIterationLowestErrorSolution { get; set; }

        double CurrentIterationHighestError { get; set; }
        double[] CurrentIterationHighestErrorSolution { get; set; }

    }
}
