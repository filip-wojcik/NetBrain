using System;

namespace NetBrain.Abstracts.Logic.Models
{
    public interface ISentence<V> : IEquatable<ISentence<V>>
    {
        string Name { get; }
        int Arity { get; }

        ISentence<V> Negate();
    }
}
