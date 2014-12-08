using System;
using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Defaults.Logic.Models
{
    public class Predicate<V> : IPredicate<V>, IEquatable<Predicate<V>>
    {
        # region Public properties

        public string Name { get; private set; }

        public int Arity { get; private set; }

        # endregion Public properties

        # region Constructor

        public Predicate(string name, int arity)
        {
            this.Name = name;
            this.Arity = arity;
        }

        # endregion Constructor

        # region Processing methods

        public ISentence<V> Negate()
        {
            var notOperator = new Not<V>();
            var mapping = new Dictionary<int, IList<int>>()
            {
                {0, Enumerable.Range(0, this.Arity).ToList() }
            };
            
            //TODO: add negated sentences names building
            return new ComplexSentence<V>(notOperator.Name + " " + this.Name, this.Arity, new ISentence<V>[]{ this }, notOperator, mapping);
        }

        # endregion Processing methods

        # region Equality methods

        public bool Equals(Predicate<V> other)
        {
            return this.Equals(other as IPredicate<V>);
        }

        public bool Equals(ISentence<V> other)
        {
            return this.Equals(other as IPredicate<V>);
        }

        public bool Equals(IPredicate<V> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && Arity == other.Arity;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Predicate<V>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ Arity;
            }
        }

        # endregion Equality methods

    }
}
