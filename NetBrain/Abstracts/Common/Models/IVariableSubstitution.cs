using System;

namespace NetBrain.Abstracts.Common.Models
{
    public interface IVariableSubstitution<V> : IEquatable<IVariableSubstitution<V>>
    {
        IVariable<V> Variable { get; }
        V ProposedValue { get; }

        void Apply();
    }
}
