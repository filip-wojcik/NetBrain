using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.Logic.Exceptions
{
    public class InvalidInputsCountException : Exception
    {
        private static string MESSAGE = "Invalid inputs count! Expected {0}, got {1}";

        public InvalidInputsCountException(int expected, int actual) 
            : base(string.Format(MESSAGE, expected, actual))
        {
        }
    }
}
