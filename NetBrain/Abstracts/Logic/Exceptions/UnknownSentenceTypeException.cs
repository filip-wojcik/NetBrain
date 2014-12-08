using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Abstracts.Logic.Exceptions
{
    public class UnknownSentenceTypeException<V> : Exception
    {
        private static string MESSAGE = "Unknown sentence type {0}!";

        public UnknownSentenceTypeException(ISentence<V> sentence) 
            : base(string.Format(MESSAGE, sentence))
        {
        }
    }
}
