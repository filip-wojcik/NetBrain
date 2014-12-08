using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.OptimizationAlgorithms;

namespace NetBrain.Defaults.MachineLearning.Algorithms.OptimizationAlgorithms
{
    public class OptimizationAlgorithmLoggerParams : IOptimizationAlgorithmLoggerParams
    {
        public int CurrentIteration { get; set; }

        public double LowestErrorFound { get; set; }

        public double[] LowestErrorSolution { get; set; }

        public double HighestErrorFound { get; set; }

        public double[] HighestErrorSolution { get; set; }

        public double CurrentIterationLowestError { get; set; }

        public double[] CurrentIterationLowestErrorSolution { get; set; }

        public double CurrentIterationHighestError { get; set; }

        public double[] CurrentIterationHighestErrorSolution { get; set; }
    }
}
