using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Exceptions
{
    public class EmptyFeatureVectorsSetException : Exception
    {
        public EmptyFeatureVectorsSetException()
        {
        }

        public EmptyFeatureVectorsSetException(string message) 
            : base(message)
        {
        }
    }
}
