using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Algorithms.OptimizationAlgorithms.ParticleSwarm;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.OptimizationAlgorithms.ParticleSwarm
{
    [TestClass()]
    public class ParticleSwarmOptimizationTests
    {
        # region Test problem description

        /// <summary>
        /// Test problem used in this test cases has been taken from http://visualstudiomagazine.com/Articles/2013/11/01/Particle-Swarm-Optimization.aspx?Page=1 by James McCaffrey - it is refered to as "Two Dip Function"
        /// </summary>

        private IProblemDomain ProblemDomain = new ProblemDomain(new IFeatureDomain[]
        {
            new FeatureDomain(-10.0, 10.0),
            new FeatureDomain(-10.0, 10.0)
        });

        private static double VerifySolution(IEnumerable<double> solution)
        {
            double trueMin = -0.42888194;
            double z = solution.First() *
              Math.Exp(-((solution.First() * solution.First()) + (solution.Last() * solution.Last())));
            return (z - trueMin) * (z - trueMin);
        }

        # endregion Test problem description

        internal ParticleSwarmOptimizerStub Subject = new ParticleSwarmOptimizerStub(VerifySolution, particlesCount: 40);

        [TestMethod()]
        public void BuildRandomParticles()
        {
            //Given
            double[] bestPosition = new double[this.ProblemDomain.Size];
            double lowestError = double.MaxValue;

            //When
            IList<Particle> particles = Subject.BuildRandomParticles(this.ProblemDomain, out bestPosition,
                out lowestError);

            //Then
            Assert.AreEqual(40, particles.Count);
            Assert.IsTrue(particles.All(
                particle => particle.Error != 0.0 && particle.BestPosition.SequenceEqual(particle.CurrentPosition) &&
                particle.CurrentPosition[0] >= this.ProblemDomain[0].MinValue && particle.CurrentPosition[0] <= this.ProblemDomain[0].MaxValue &&
                particle.CurrentPosition[1] >= this.ProblemDomain[1].MinValue && particle.CurrentPosition[1] <= this.ProblemDomain[1].MaxValue
                ));
            Assert.IsTrue(particles.Any(particle => particle.Error.Equals(lowestError) && particle.CurrentPosition.SequenceEqual(bestPosition)));
        }

        [TestMethod]
        public void SolveTest()
        {
            //Given
            var logger = new OptimizationAlgorithmsTestLogger();
            this.Subject.Logger = logger;
            IEnumerable<double> solution = this.Subject.Solve(this.ProblemDomain, 100);

            //When
            double finalSolutionError = VerifySolution(solution);

            //Then
            Assert.IsTrue(finalSolutionError <= 0.00001);

            double previousLowestError = double.MaxValue;

            Assert.IsFalse(logger.EachIterationParams.Select(param => param.LowestErrorFound).Distinct().Count() == 1);
            foreach (var iterationParams in logger.EachIterationParams)
            {
                Assert.IsTrue(iterationParams.LowestErrorFound <= previousLowestError);
                previousLowestError = iterationParams.LowestErrorFound;
            }
        }

        //TODO: add unit test for updating particle position
    }
}
