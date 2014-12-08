using System;

namespace NetBrain.Abstracts.MachineLearning.Models.Data
{
    public interface ILabeledFeature<T> : IEquatable<ILabeledFeature<T>>
    {
        T Value { get; }
        string Label { get; }
    }
}
