using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Data.Models
{
    public class SingleValueFeatureVector<T> : FeatureVector<T>, ISingleValueFeatureVector<T>, IEquatable<SingleValueFeatureVector<T>>
    {
        # region Public properties

        public int ValueIndex { get; private set; }

        public T Value
        {
            get
            {
                if (this.HasValue)
                {
                    return base.Features[this.ValueIndex];
                }
                else
                {
                    return default(T);
                }
                
            }
        }

        public override IList<int> NonValueIndexes
        {
            get
            {
                var nonValuesIndexes = new List<int>();
                for (int i = 0; i < this.Count(); i++)
                {
                    if(this.HasValue && i == this.ValueIndex) continue;
                    else nonValuesIndexes.Add(i);
                }
                return nonValuesIndexes;
            }
        }

        public ISingleValueFeatureVector<T> NonValuesVector
        {
            get
            {
                if (this.HasValue)
                {
                    var nonValues = new List<T>();
                    for (int i = 0; i < this.Features.Count; i++)
                    {
                        if(this.HasValue && i == this.ValueIndex) continue;
                        nonValues.Add(this[i]);
                    }
                    return new SingleValueFeatureVector<T>(nonValues);
                }
                return this;

            }
        }

        public bool HasValue
        {
            get { return this.ValueIndex != -1; }
        }

        public override bool HasValues
        {
            get { return this.HasValue; }
        }

        # endregion Public properties

        # region Constructor

        public SingleValueFeatureVector(IList<T> features, int valueIndex = -1)
            : base(features, new []{ valueIndex })
        {
            this.ValueIndex = valueIndex;
        }

        # endregion Constructor

        public bool Equals(ISingleValueFeatureVector<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (this.HasValue && other.HasValues)
            {
                if (this.ValueIndex != other.ValueIndex) return false;
            }
            else if (
                (this.HasValue && !other.HasValues) ||
                (!this.HasValue && other.HasValues)
                )
            {
                return false;
            }
            return this.SequenceEqual(other);
        }

        public bool Equals(SingleValueFeatureVector<T> other)
        {
            return this.Equals(other as ISingleValueFeatureVector<T>);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SingleValueFeatureVector<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 397;
                foreach (var feature in this.Features) hash ^= feature.GetHashCode();
                if (this.HasValue)
                {
                    hash ^= this.ValueIndex.GetHashCode();

                }
                return hash;
            }
        }

        #region ISingleValueFeatureVector<T> Members

        #endregion
    }
}
