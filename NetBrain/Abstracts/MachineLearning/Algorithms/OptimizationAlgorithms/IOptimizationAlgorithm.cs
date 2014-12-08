using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.OptimizationAlgorithms
{
    /// <summary>
    /// Method for performing evaluation of optimization algorithm's solutions
    /// </summary>
    /// <param name="solution">Solution</param>
    /// <returns>Error level for solution</returns>
    public delegate double SolutionChecker(IEnumerable<double> solution);

    /// <summary>
    /// Algorithm for finding optimal solution to some numeric problem
    /// </summary>
    public interface IOptimizationAlgorithm
    {
        IOptimizationAlgorithmLogger Logger { get; set; }
        SolutionChecker SolutionChecker { get; set; }
        IEnumerable<double> Solve(IProblemDomain problemDomain, int iterations);
    }
}
