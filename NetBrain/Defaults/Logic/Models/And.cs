using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Defaults.Logic.Models
{
    public class And<V> : BaseOperator<V>
    {
        private static string NAME = "AND";

        public And() : base(NAME)
        {
        }

        protected override bool PerformEvaluation(IList<bool> results)
        {
            foreach (var result in results)
            {
                if (!result) return false;
            }
            return true;
        }

        public override IOperator<V> Negate()
        {
            return new Or<V>();
        }

        public override IList<ISentence<V>> NegateSentences(IList<ISentence<V>> sentences)
        {
            return sentences.Select(sentence => sentence.Negate()).ToList();
        }
    }
}
