using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.Logic.Models
{
    public interface IOperator<V> : IEquatable<IOperator<V>>
    {
        string Name { get; }
        int Arity { get; }
        bool Evaluate(IList<bool> results);
        IOperator<V> Negate();

        IList<ISentence<V>> NegateSentences(IList<ISentence<V>> sentences);
    }
}
