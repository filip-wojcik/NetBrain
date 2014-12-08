using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models;

namespace NetBrain.Abstracts.MachineLearning.Exceptions
{
    public class VectorsSizeNotEqualException<T> : Exception
    {
        private static string MESSAGE = "MultiValueVectors size is not equal. Vector1: {0}, Vector2: {1}";

        public VectorsSizeNotEqualException(IEnumerable<T> vector1, IEnumerable<T> vector2) 
            : base(string.Format(MESSAGE, vector1.Count(), vector2.Count()))
        {

        }
    }
}
