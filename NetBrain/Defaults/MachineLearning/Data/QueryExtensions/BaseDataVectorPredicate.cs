using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data.QueryExtensions;
using Logic = NetBrain.Defaults.Logic.Models;

namespace NetBrain.Defaults.MachineLearning.Data.QueryExtensions
{
    public abstract class BaseDataVectorPredicate<T> : Logic.Models.Predicate<T>, IDataVectorPredicate<T>
    {
        # region Public properties

        public T ExpectedValue { get; private set; }

        # endregion Public properties

        # region Construction

        public BaseDataVectorPredicate(string name, T expectedValue) 
            : base(name, 1)
        {
            ExpectedValue = expectedValue;
        }

        # endregion Construction

        # region Processing methods

        public abstract bool IsSatisfied(T vectorValue);

        # endregion Processing methods
    }
}