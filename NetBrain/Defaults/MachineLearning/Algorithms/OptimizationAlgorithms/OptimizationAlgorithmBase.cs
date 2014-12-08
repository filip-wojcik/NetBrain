using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.OptimizationAlgorithms;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Algorithms.OptimizationAlgorithms
{
    public abstract class OptimizationAlgorithmBase : IOptimizationAlgorithm
    {
        public const string INVALID_PROBLEM_DOMAIN = "Problem domain is invalid!";

        public SolutionChecker SolutionChecker { get; set; }
        public IOptimizationAlgorithmLogger Logger { get; set; }

        public bool LoggerIsSet { get { return this.Logger != null; } }

        protected OptimizationAlgorithmBase() { }

        protected OptimizationAlgorithmBase(SolutionChecker solutionChecker)
        {
            SolutionChecker = solutionChecker;
        }

        public IEnumerable<double> Solve(IProblemDomain problemDomain, int iterations)
        {
            return SearchSolution(problemDomain, iterations);
        }

        protected abstract IEnumerable<double> SearchSolution(IProblemDomain problemDomain, int iterations);

        protected void ValidateProblemDomain(IProblemDomain problemDomain)
        {
            if (problemDomain == null || problemDomain.Size < 1)
            {
                throw new ArgumentException(INVALID_PROBLEM_DOMAIN);
            }
        }

        protected void LogProgress(IOptimizationAlgorithmLoggerParams loggerParams)
        {
            this.Logger.LogProgress(loggerParams);
        }
    }
}
