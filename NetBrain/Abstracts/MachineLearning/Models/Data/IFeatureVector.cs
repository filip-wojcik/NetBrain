using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Models.Data
{
    public interface IFeatureVector<T> : IEnumerable<T>, IEquatable<IFeatureVector<T>>
    {
        IReadOnlyList<T> Features { get; }
        IList<int> NonValueIndexes { get; }
        IReadOnlyList<int> ValueIndexes { get; }
        IFeatureVector<T> ValuesVector { get; }
        IFeatureVector<T> NonValuesVector { get; }

        bool HasValues { get; }
        T this[int idx] { get; }
    }
}
