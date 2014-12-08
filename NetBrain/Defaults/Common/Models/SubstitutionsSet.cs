using System;
using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.Common.Models;

namespace NetBrain.Defaults.Common.Models
{
    public class SubstitutionsSet<V> : ISubstitutionsSet<V>, IEquatable<SubstitutionsSet<V>>
    {
        public IEnumerable<IVariableSubstitution<V>> VariableSubstitutions { get; set; }

        public SubstitutionsSet(IEnumerable<IVariableSubstitution<V>> variableSubstitutions)
        {
            VariableSubstitutions = variableSubstitutions;
        }

        public bool Equals(ISubstitutionsSet<V> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.VariableSubstitutions.SequenceEqual(other.VariableSubstitutions);
        }

        public bool Equals(SubstitutionsSet<V> other)
        {
            return this.Equals(other as ISubstitutionsSet<V>);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SubstitutionsSet<V>) obj);
        }

        public override int GetHashCode()
        {
            return (VariableSubstitutions != null ? VariableSubstitutions.GetHashCode() : 0);
        }
    }
}
