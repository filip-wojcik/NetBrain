using System;
using System.Collections.Generic;

namespace NetBrain.Abstracts.MachineLearning.Models.Data
{
    public interface ISingleValueFeatureVector<T> : IFeatureVector<T>, IEquatable<ISingleValueFeatureVector<T>>
    {
        int ValueIndex { get; }
        T Value { get; }
        ISingleValueFeatureVector<T> NonValuesVector { get; } 
    }
}
