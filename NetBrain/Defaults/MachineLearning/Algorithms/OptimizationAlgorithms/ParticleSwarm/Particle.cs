using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Defaults.MachineLearning.Algorithms.OptimizationAlgorithms.ParticleSwarm
{
    /// <summary>
    /// Represents a particle - part of swarm.
    /// </summary>
    public class Particle
    {
        /// <summary>
        /// Current position of particle - as vector of coordinates
        /// </summary>
        public double[] CurrentPosition { get; set; }

        /// <summary>
        /// Best known particle position
        /// </summary>
        public double[] BestPosition { get; set; }

        /// <summary>
        /// Velocity of particle - as vector of values for each coorinate
        /// </summary>
        public double[] CurrentVelocity { get; set; }
        
        /// <summary>
        /// Error rate of particle connected with current position
        /// </summary>
        public double Error { get; set; }

        /// <summary>
        /// Lowest known error for particle
        /// </summary>
        public double LowestError { get; set; }

        public Particle()
        {
        }

        public Particle(double[] currentPosition, double[] currentVelocity, double error)
        {
            CurrentPosition = currentPosition;
            CurrentVelocity = currentVelocity;
            Error = error;
        }
    }
}
