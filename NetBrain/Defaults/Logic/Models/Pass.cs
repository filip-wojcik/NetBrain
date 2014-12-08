using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Defaults.Logic.Models
{
    public class Pass<V> : BaseOperator<V>
    {
        private static string NAME = string.Empty;
        private static int Arity = 1;

        public Pass() : base(NAME, Arity)
        {
        }

        public override IOperator<V> Negate()
        {
            return new Not<V>();
        }

        protected override bool PerformEvaluation(IList<bool> results)
        {
            return results[0];
        }

        public override IList<ISentence<V>> NegateSentences(IList<ISentence<V>> sentences)
        {
            return sentences.Select(sentence => sentence.Negate()).ToList();
        }
    }
}
