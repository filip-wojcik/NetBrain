using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.OptimizationAlgorithms;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.OptimizationAlgorithms
{
    public class OptimizationAlgorithmsTestLogger : IOptimizationAlgorithmLogger
    {
        public IList<IOptimizationAlgorithmLoggerParams> EachIterationParams { get; set; }

        public OptimizationAlgorithmsTestLogger()
        {
            this.EachIterationParams = new List<IOptimizationAlgorithmLoggerParams>();
        }

        public void LogProgress(IOptimizationAlgorithmLoggerParams loggerParams)
        {
            this.EachIterationParams.Add(loggerParams);
        }
    }
}
