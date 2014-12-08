using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.OptimizationAlgorithms;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Algorithms.OptimizationAlgorithms.ParticleSwarm;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.OptimizationAlgorithms.ParticleSwarm
{
    internal class ParticleSwarmOptimizerStub : ParticleSwarmOptimization
    {
        public ParticleSwarmOptimizerStub(SolutionChecker solutionChecker, double intertia = 0.729, double cognitiveWeight = 1.49445, double socialWeight = 1.49445, double particleDeathProbability = 0.01, int particlesCount = 30) : base(solutionChecker, intertia, cognitiveWeight, socialWeight, particleDeathProbability, particlesCount)
        {
        }

        public new IList<Particle> BuildRandomParticles(IProblemDomain problemDomain, out double[] bestPosition,
            out double lowestError)
        {
            return base.BuildRandomParticles(problemDomain, out bestPosition, out lowestError);
        }

        public new void UpdateParticlePosition(Particle particle, IProblemDomain problemDomain, double lowestKnownError,
            double[] bestKnownPosition)
        {
            base.UpdateParticlePosition(particle, problemDomain, lowestKnownError, bestKnownPosition);
        }
    }
}
