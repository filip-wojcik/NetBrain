using System.Collections.Generic;
using NetBrain.Abstracts.Logic.Models;
using NetBrain.Defaults.Logic.Models;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Models.Data.QueryExtensions
{
    public interface IDataVectorPredicate<T> : IPredicate<T>
    {
        T ExpectedValue { get; }
        bool IsSatisfied(T vectorValue);
    }
}
