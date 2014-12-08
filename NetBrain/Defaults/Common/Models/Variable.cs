using System;
using System.Collections.Generic;
using NetBrain.Abstracts.Common.Models;

namespace NetBrain.Defaults.Common.Models
{
    public class Variable<V> : IVariable<V>, IEquatable<Variable<V>>
    {
        private readonly string _name;
        private V _value;

        public string Name
        {
            get { return _name; }
        }

        public V Value
        {
            get { return this._value; }
            set
            {
                this._value = value;
                this.IsSet = (value == null) ? false : true;
            }
        }

        public bool IsSet { get; private set; }

        public bool IsImplicit { get; private set; }

        public Variable(string name, V value = default(V), bool isImplicit = false)
        {
            this._name = name;
            this.Value = value;
            this.IsSet = !(value == null || value.Equals(default(V)));
            this.IsImplicit = isImplicit;
        }

        public bool Equals(Variable<V> other)
        {
            return this.Equals(other as IVariable<V>);
        }

        public IVariable<V> Clone()
        {
            return new Variable<V>(this.Name, this.Value, this.IsImplicit);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Variable<V> && Equals((Variable<V>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ EqualityComparer<V>.Default.GetHashCode(Value);
            }
        }

        public bool Equals(IVariable<V> other)
        {
            return string.Equals(Name, other.Name) && EqualityComparer<V>.Default.Equals(Value, other.Value);
        }
    }
}
