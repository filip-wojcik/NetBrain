using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using NetBrain.Abstracts.MachineLearning.Algorithms.OptimizationAlgorithms;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Algorithms.OptimizationAlgorithms.ParticleSwarm
{
    /// <summary>
    /// Optimization algorithm based on the swarm of bees
    /// </summary>
    public class ParticleSwarmOptimization : OptimizationAlgorithmBase
    {
        # region Private fields

        /// <summary>
        /// Intertia used in training - determining the influnce of current velocity on particle movement
        /// </summary>
        private readonly double _intertia;

        /// <summary>
        /// Weight for perception of a given particle
        /// </summary>
        private readonly double _cognitiveWeight;

        /// <summary>
        /// Weight of social influence of the whole swarm on given particle
        /// </summary>
        private readonly double _socialWeight;

        /// <summary>
        /// Probability that a given particle will be kiled during
        /// solution-search process. This can significantly improve 
        /// process of bypassing local minima/maxima of functions
        /// </summary>
        private readonly double _particleDeathProbability;

        private readonly int _particlesCount;

        private object _locker = new object();
        private Random _randomizer = new Random();

        # endregion Private fields

        # region Constructor

        public ParticleSwarmOptimization(
            double intertia = 0.729,
            double cognitiveWeight = 1.49445,
            double socialWeight = 1.49445,
            double particleDeathProbability = 0.01,
            int particlesCount = 30)
            : base()
        {
            _intertia = intertia;
            _cognitiveWeight = cognitiveWeight;
            _socialWeight = socialWeight;
            _particlesCount = particlesCount;
            _particleDeathProbability = particleDeathProbability;
        }

        public ParticleSwarmOptimization(
            SolutionChecker solutionChecker,
            double intertia = 0.729,
            double cognitiveWeight = 1.49445,
            double socialWeight = 1.49445,
            double particleDeathProbability = 0.01,
            int particlesCount = 30)
                :this(intertia, cognitiveWeight, socialWeight, particleDeathProbability, particlesCount)
        {
            base.SolutionChecker = solutionChecker;
        }


        # endregion Constructor

        # region Processing methods

        protected override IEnumerable<double> SearchSolution(IProblemDomain problemDomain, int iterations)
        {
            double lowestKnownError = double.MaxValue;
            double[] bestPosition = new double[problemDomain.Size];

            double highestError = double.MinValue;
            double[] worstPosition = new double[problemDomain.Size];

            IList<Particle> particles = this.BuildRandomParticles(problemDomain, out bestPosition, out lowestKnownError);
            for (int i = 0; i < iterations; i++)
            {
                double currentIterationLowestError = double.MaxValue;
                double[] currentIterationLowestErrorPosition = new double[problemDomain.Size];

                double currentIterationHighestError = double.MinValue;
                double[] currentIterationHighestErrorPosition = new double[problemDomain.Size];

                foreach (var particle in particles)
                {
                    if (this.ShouldKillParticle()) this.KillParticle(particle, problemDomain);
                    else
                    {
                        this.UpdateParticlePosition(particle, problemDomain, lowestKnownError, bestPosition);
                    }
                    if (particle.Error < lowestKnownError)
                    {
                        lowestKnownError = particle.Error;
                        particle.CurrentPosition.CopyTo(bestPosition, 0);

                        SetCurrentIterationValues(particle, out currentIterationLowestError, out currentIterationLowestErrorPosition);
                    }
                    else if (particle.Error > highestError)
                    {
                        highestError = particle.Error;
                        particle.CurrentPosition.CopyTo(worstPosition, 0);
                        SetCurrentIterationValues(particle, out currentIterationHighestError, out currentIterationHighestErrorPosition);
                    }
                    else
                    {
                        if (particle.Error < currentIterationLowestError)
                        {
                            SetCurrentIterationValues(particle, out currentIterationLowestError, out currentIterationLowestErrorPosition);
                        }
                        else if (particle.Error > currentIterationHighestError)
                        {
                            SetCurrentIterationValues(particle, out currentIterationHighestError, out currentIterationHighestErrorPosition);
                        }
                    }
                }

                if (base.LoggerIsSet)
                {
                    base.LogProgress(new OptimizationAlgorithmLoggerParams()
                    {
                        CurrentIteration = i,
                        CurrentIterationHighestError = currentIterationHighestError,
                        CurrentIterationHighestErrorSolution = currentIterationHighestErrorPosition,
                        CurrentIterationLowestError = currentIterationLowestError,
                        CurrentIterationLowestErrorSolution = currentIterationLowestErrorPosition,
                        HighestErrorFound = highestError,
                        HighestErrorSolution = worstPosition,
                        LowestErrorFound = lowestKnownError,
                        LowestErrorSolution = bestPosition
                    });
                }
            }
            return bestPosition;
        }

        protected void UpdateParticlePosition(Particle particle, IProblemDomain problemDomain, double lowestKnownError,
            double[] bestKnownPosition)
        {
            for (int i = 0; i < particle.CurrentVelocity.Length; i++)
            {
                IFeatureDomain featureDomain = problemDomain[i];

                particle.CurrentVelocity[i] = (this._intertia * particle.CurrentVelocity[i]) +
                                              (this._randomizer.NextDouble() * this._cognitiveWeight *
                                               (particle.BestPosition[i] - particle.CurrentPosition[i])) +
                                              (this._randomizer.NextDouble() * this._socialWeight *
                                               (bestKnownPosition[i] - particle.CurrentPosition[i]));

                particle.CurrentPosition[i] = particle.CurrentPosition[i] + particle.CurrentVelocity[i];
                if (particle.CurrentPosition[i] < featureDomain.MinValue)
                    particle.CurrentPosition[i] = featureDomain.MinValue;

                if (particle.CurrentPosition[i] > featureDomain.MaxValue)
                    particle.CurrentPosition[i] = featureDomain.MaxValue;
            }

            particle.Error = this.SolutionChecker(particle.CurrentPosition);
            if (particle.Error < particle.LowestError)
            {
                particle.LowestError = particle.Error;
                particle.CurrentPosition.CopyTo(particle.BestPosition, 0);
            }
        }

        protected bool ShouldKillParticle()
        {
            return this._randomizer.NextDouble() <= this._particleDeathProbability;
        }

        protected void KillParticle(Particle particle, IProblemDomain problemDomain)
        {
            double[] randomPosition = new double[problemDomain.Size];
            RandomlyFillArray(randomPosition, problemDomain);
            particle.Error = this.SolutionChecker(randomPosition);
            if (particle.Error < particle.LowestError)
            {
                particle.LowestError = particle.Error;
                particle.CurrentPosition.CopyTo(particle.BestPosition, 0);
            }
        }

        # endregion Processing methods

        # region Helper methods

        protected IList<Particle> BuildRandomParticles(IProblemDomain problemDomain, out double[] bestPosition,
            out double lowestError)
        {
            var particles = new Particle[this._particlesCount];
            double lowestKnownError = Double.MaxValue;
            var bestKnownPosition = new double[problemDomain.Size];
            Parallel.For(0, this._particlesCount, (particleIdx =>
            {
                particles[particleIdx] = this.BuildRandomParticle(problemDomain);
                lock (this._locker)
                {
                    if (particles[particleIdx].Error < lowestKnownError)
                    {
                        lowestKnownError = particles[particleIdx].Error;
                        Array.Copy(particles[particleIdx].CurrentPosition, bestKnownPosition, problemDomain.Size);
                    }
                }
            }));

            bestPosition = bestKnownPosition;
            lowestError = lowestKnownError;
            return particles;
        }

        protected Particle BuildRandomParticle(IProblemDomain problemDomain)
        {
            double[] randomPosition = new double[problemDomain.Size];
            double[] randomVelocity = new double[problemDomain.Size];

            RandomlyFillArray(randomPosition, problemDomain);
            RandomlyFillArray(randomVelocity, problemDomain);

            double error = base.SolutionChecker(randomPosition);
            return new Particle()
            {
                CurrentPosition = randomPosition.ToArray(),
                BestPosition = randomPosition.ToArray(),
                CurrentVelocity = randomVelocity.ToArray(),
                Error = error,
                LowestError = error
            };
        }

        private static void RandomlyFillArray(double[] arrayToFill, IProblemDomain problemDomain)
        {
            for (int i = 0; i < arrayToFill.Length; i++)
            {
                arrayToFill[i] = SetRandomFeatureValue(problemDomain, i);
            }
        }

        private static double SetRandomFeatureValue(IProblemDomain problemDomain, int i)
        {
            IFeatureDomain featureDomain = problemDomain[i];
            return Randomization.RandomDoubleInRange(featureDomain.MinValue, featureDomain.MaxValue);
        }

        private static void SetCurrentIterationValues(
            Particle sourceParticle,
            out double iterationErrorValue, 
            out double[] iterationPosition)
        {
            iterationErrorValue = sourceParticle.Error;
            iterationPosition = new double[sourceParticle.CurrentPosition.Length];
            Array.Copy(sourceParticle.CurrentPosition, iterationPosition, 0);
        }

        # endregion Helper methods
    }
}