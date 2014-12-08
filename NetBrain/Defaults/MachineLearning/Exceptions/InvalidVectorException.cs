using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Defaults.MachineLearning.Exceptions
{
    public class InvalidVectorException<T> : Exception
    {
        public InvalidVectorException(string message) : base(message)
        {
        }
    }
}
