using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.Logic.Exceptions
{
    public class InvalidMappingIndexException : Exception
    {
        private static string MESSAGE = "Invalid mappings index! Maximal index {0}, got {1}";

        public InvalidMappingIndexException(int max, int actual) 
            : base(string.Format(MESSAGE, max, actual))
        {
        }
    }
}
