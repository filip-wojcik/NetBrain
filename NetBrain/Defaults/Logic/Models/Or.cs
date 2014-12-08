using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Defaults.Logic.Models
{
    public class Or<V> : BaseOperator<V>
    {
        private static string NAME = "Or";

        public Or() : base(NAME)
        {
        }

        protected override bool PerformEvaluation(IList<bool> results)
        {
            foreach (var result in results)
            {
                if (result) return true;
            }
            return false;
        }

        public override IOperator<V> Negate()
        {
            return new And<V>();
        }

        public override IList<ISentence<V>> NegateSentences(IList<ISentence<V>> sentences)
        {
            return sentences.Select(sentence => sentence.Negate()).ToList();
        }
    }
}
