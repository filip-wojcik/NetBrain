using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using System.Collections.ObjectModel;

namespace NetBrain.Defaults.MachineLearning.Data.Models
{
    public class FeatureVector<T> :  IFeatureVector<T>, IEquatable<IFeatureVector<T>>
    {
        # region Public properties

        public IReadOnlyList<T> Features { get; private set; }
        public IReadOnlyList<int> ValueIndexes { get; private set; }

        public T this[int idx]
        {
            get { return this.Features[idx]; }
        }

        public virtual IList<int> NonValueIndexes
        {
            get
            {
                var nonValuesIndexes = new List<int>();
                if (this.ValueIndexes.Any())
                {
                    for (int i = 0; i < this.Count(); i++)
                    {
                        if (!this.ValueIndexes.Contains(i)) nonValuesIndexes.Add(i);
                    }
                }
                return nonValuesIndexes;
            }
        }

        public IFeatureVector<T> ValuesVector
        {
            get
            {
                var values = new List<T>();
                if (this.HasValues)
                {
                    values.AddRange(this.ValueIndexes.Select(valueIdx => this[valueIdx]));
                }
                return new FeatureVector<T>(values);
            }
        }

        public IFeatureVector<T> NonValuesVector
        {
            get
            {
                if (this.HasValues)
                {
                    var nonValues = new List<T>();
                    for (int i = 0; i < this.Features.Count; i++)
                    {
                        if(this.ValueIndexes.Contains(i)) continue;
                        nonValues.Add(this[i]);
                    }
                    return new FeatureVector<T>(nonValues);
                }
                return this;

            }
        }

        public virtual bool HasValues
        {
            get { return this.ValueIndexes.Any(); }
        }

        # endregion Public properties

        # region Constructor

        public FeatureVector(IList<T> features, IList<int> valueIndexes = null)
        {
            this.Features = new ReadOnlyCollection<T>(features);
            this.ValueIndexes = (valueIndexes == null)
                ? new ReadOnlyCollection<int>(new int[0])
                : new ReadOnlyCollection<int>(valueIndexes);
        }

        # endregion Constructor

        # region Equality methods

        public bool Equals(IFeatureVector<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (this.HasValues && other.HasValues)
            {
                if (!this.ValueIndexes.SequenceEqual(other.ValueIndexes)) return false;
            }
            else if (
                (this.HasValues && !other.HasValues) ||
                (!this.HasValues && other.HasValues)
                )
            {
                return false;
            }
            return this.SequenceEqual(other);
        }

        public bool Equals(FeatureVector<T> other)
        {
            return this.Equals(other as IFeatureVector<T>);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals(obj as IFeatureVector<T>);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 397;
                foreach (var feature in this.Features) hash ^= feature.GetHashCode();
                if (this.HasValues)
                {
                    foreach (var valueIdx in this.ValueIndexes)
                    {
                        hash ^= this[valueIdx].GetHashCode();
                    }
                    
                }
                return hash;
            }
        }

        # endregion Equality methods

        # region Enumeration members

        public IEnumerator<T> GetEnumerator()
        {
            return this.Features.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Features.AsEnumerable().GetEnumerator();
        }

        # endregion Enumeration members
    }
}
