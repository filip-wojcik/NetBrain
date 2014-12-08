using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.OptimizationAlgorithms
{
    public interface IOptimizationAlgorithmLogger
    {
        void LogProgress(IOptimizationAlgorithmLoggerParams loggerParams);
    }
}
