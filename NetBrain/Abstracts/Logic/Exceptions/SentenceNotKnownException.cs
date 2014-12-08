using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Abstracts.Logic.Exceptions
{
    public class SentenceNotKnownException<V> : Exception
    {
        private static string MESSAGE = "Sentence not known: {0}";

        public SentenceNotKnownException(ISentence<V> sentence ) 
            : base(string.Format(MESSAGE, sentence))
        {
        }
    }
}
