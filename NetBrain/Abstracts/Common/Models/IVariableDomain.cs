using System.Collections.Generic;
using NetBrain.Utils;

namespace NetBrain.Abstracts.Common.Models
{
    /// <summary>
    /// Shows domain of a given variable - all possible
    /// values that variable can take
    /// </summary>
    /// <typeparam name="TValueType">Type of value stored in variable</typeparam>
    public interface IVariableDomain<TValueType> : ICloneableType<IVariableDomain<TValueType>>
    {
        IVariable<TValueType> Variable { get; } 
        IList<TValueType> AllowedValues { get; }
        bool IsEmpty { get; }
        
        void RemoveAllowedValue(TValueType value);
        void AddAllowedValue(TValueType value);
    }

    public delegate IVariableDomain<V> VariableWithDomainFactory<V>(IVariable<V> variable, IList<V> allowedValues);
}
