namespace NetBrain.Abstracts.MachineLearning.Models.Data
{
    using System.Collections.Generic;

    public interface ISingleValueDataSet<T> : IDataSet<T>
    {
        int ValueIndex { get; }

        IEnumerable<T> Values { get; } 
        IEnumerable<T> UniqueValues { get; }
        IEnumerable<ISingleValueFeatureVector<T>> SingleValueVectors { get; } 
    }
}
