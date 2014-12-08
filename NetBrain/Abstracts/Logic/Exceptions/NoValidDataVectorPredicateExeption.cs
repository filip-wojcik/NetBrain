using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.Logic.Exceptions
{
    public class NoValidDataVectorPredicateExeption : Exception
    {
        public const string MESSAGE = "No valid predicate found for searching data vectors!";

        public NoValidDataVectorPredicateExeption() 
            : base(MESSAGE)
        {
        }
    }
}
