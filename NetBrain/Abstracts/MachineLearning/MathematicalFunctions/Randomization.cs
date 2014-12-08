using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.MathematicalFunctions
{
    public static class Randomization
    {
        private static Random _randomizer = new Random();

        public static double RandomDoubleInRange(double min, double max)
        {
            return (max - min) * _randomizer.NextDouble() + min;
        }
    }
}
