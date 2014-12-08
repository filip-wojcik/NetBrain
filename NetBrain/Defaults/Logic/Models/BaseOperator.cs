using System;
using System.Collections.Generic;
using NetBrain.Abstracts.Logic.Exceptions;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Defaults.Logic.Models
{
    public abstract class BaseOperator<V> : IOperator<V>, IEquatable<BaseOperator<V>>
    {
        public string Name { get; private set; }
        public int Arity { get; private set; }

        protected BaseOperator(string name, int arity = 0)
        {
            this.Name = name;
            this.Arity = arity;
        }

        public bool Evaluate(IList<bool> results)
        {
            if (this.ValidateResults(results)) return PerformEvaluation(results);
            throw new InvalidInputsCountException(this.Arity, results.Count);
        }

        public abstract IOperator<V> Negate();

        public abstract IList<ISentence<V>> NegateSentences(IList<ISentence<V>> sentences);

        protected abstract bool PerformEvaluation(IList<bool> results);

        protected bool ValidateResults(IList<bool> results)
        {
            if (this.Arity == 0) return true;
            return results.Count.Equals(this.Arity);
        }

        public bool Equals(BaseOperator<V> other)
        {
            return this.Equals(other as IOperator<V>);
        }

        public bool Equals(IOperator<V> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BaseOperator<V>) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}
