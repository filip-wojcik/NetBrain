using System;
using System.Collections.Generic;
using NetBrain.Abstracts.Common.Models;

namespace NetBrain.Defaults.Common.Models
{
    public class VariableSubstitution<V> : IVariableSubstitution<V>, IEquatable<VariableSubstitution<V>>
    {
        # region Public properties

        public IVariable<V> Variable { get; private set; }

        public V ProposedValue { get; private set; }

        # endregion Public properties

        # region Constructor

        public VariableSubstitution(IVariable<V> variable, V proposedValue)
        {
            Variable = variable;
            ProposedValue = proposedValue;
        }

        # endregion Constructor

        # region Processing methods

        public void Apply()
        {
            this.Variable.Value = this.ProposedValue;
        }

        # endregion Processing methods

        # region Equality members

        public bool Equals(IVariableSubstitution<V> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Variable, other.Variable) && EqualityComparer<V>.Default.Equals(ProposedValue, other.ProposedValue);
        }

        public bool Equals(VariableSubstitution<V> other)
        {
            return this.Equals(other as IVariableSubstitution<V>);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VariableSubstitution<V>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Variable != null ? Variable.GetHashCode() : 0)*397) ^ EqualityComparer<V>.Default.GetHashCode(ProposedValue);
            }
        }

        # endregion Equality members
    }
}
