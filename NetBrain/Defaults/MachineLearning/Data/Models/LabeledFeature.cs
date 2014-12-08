using System;
using System.Collections.Generic;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Data.Models
{
    public struct LabeledFeature<T> : ILabeledFeature<T>, IEquatable<LabeledFeature<T>>
    {
        # region Public properties

        public T Value { get; private set; }

        public string Label { get; private set; }

        # endregion Public properties

        # region Constructor

        public LabeledFeature(T value, string label) : this()
        {
            Value = value;
            Label = label;
        }

        # endregion Constructor

        # region Equality methods

        public bool Equals(ILabeledFeature<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value) && string.Equals(Label, other.Label);
        }

        public bool Equals(LabeledFeature<T> other)
        {
            return this.Equals(other as ILabeledFeature<T>);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is LabeledFeature<T> && Equals((LabeledFeature<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(Value)*397) ^ (Label != null ? Label.GetHashCode() : 0);
            }
        }

        # endregion Equality methods
    }
}
