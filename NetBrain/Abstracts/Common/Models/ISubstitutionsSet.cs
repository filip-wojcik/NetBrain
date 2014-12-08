using System;
using System.Collections.Generic;

namespace NetBrain.Abstracts.Common.Models
{
    public interface ISubstitutionsSet<V> : IEquatable<ISubstitutionsSet<V>>
    {
        IEnumerable<IVariableSubstitution<V>> VariableSubstitutions { get; } 
    }
}
