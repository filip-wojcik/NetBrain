using System;
using NetBrain.Utils;

namespace NetBrain.Abstracts.Common.Models
{
    public interface IVariable<TValueType> : IEquatable<IVariable<TValueType>>, ICloneableType<IVariable<TValueType>>
    {
        string Name { get; }
        bool IsSet { get; }
        bool IsImplicit { get; }
        TValueType Value { get; set; }
    }
}
