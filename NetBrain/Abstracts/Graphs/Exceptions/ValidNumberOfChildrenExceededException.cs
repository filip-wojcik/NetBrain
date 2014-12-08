using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.Graphs.Exceptions
{
    public class ValidNumberOfChildrenExceededException : Exception
    {
        private static string MESSAGE = "NUMBER OF VALID CHILDREN COUNT EXCEEDED. MAXIMAL: {0}, ACTUAL: {1}";

        public ValidNumberOfChildrenExceededException(int max, int actual) : 
            base(string.Format(MESSAGE, max, actual))
        {
        }

        public ValidNumberOfChildrenExceededException(string message) : base(message)
        {
        }
    }
}
