using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Exceptions
{
    public class ImplicitVariablesNotAllowedException : Exception
    {
        private const string MESSAGE = "Implicit variables in data set queries are not allowed";

        public ImplicitVariablesNotAllowedException()
            : base(MESSAGE)
        {
            
        }
    }
}
