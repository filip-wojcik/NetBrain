using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;
using NetBrain.Abstracts.MachineLearning.Exceptions;
using NetBrain.Abstracts.MachineLearning.Models;

namespace NetBrain.Abstracts.MachineLearning.MathematicalFunctions
{
    public delegate double DistanceCalculator(IEnumerable<double> vector1, IEnumerable<double> vector2);

    public static class DistanceFunctions
    {
        public static double EuclideanVectorsDistance(IEnumerable<double> vector1, IEnumerable<double> vector2)
        {
            ValidateVectors(vector1, vector2);

            double vectorsSquareSum = 0;
            for (int i = 0; i < vector1.Count(); i++)
            {
                vectorsSquareSum += Math.Pow((vector1.ElementAt(i) - vector2.ElementAt(i)), 2);
            }
            return Math.Sqrt(vectorsSquareSum);
        }

        public static double ManhattanDistance(IEnumerable<double> vector1, IEnumerable<double> vector2)
        {
            ValidateVectors(vector1, vector2);

            double vectorsSum = 0;
            for (int i = 0; i < vector1.Count(); i++) vectorsSum += Math.Abs(vector1.ElementAt(i) - vector2.ElementAt(i));
            return vectorsSum;
        }

        private static void ValidateVectors(IEnumerable<double> vector1, IEnumerable<double> vector2)
        {
            if (vector1 == null || vector2 == null) throw new ArgumentNullException();
            if (vector1.Count() != vector2.Count()) throw new VectorsSizeNotEqualException<double>(vector1, vector2);
        }
    }
}
