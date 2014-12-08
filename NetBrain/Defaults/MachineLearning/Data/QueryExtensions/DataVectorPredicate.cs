using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicModels = NetBrain.Defaults.Logic.Models;
using NetBrain.Abstracts.MachineLearning.Models.Data.QueryExtensions;

namespace NetBrain.Defaults.MachineLearning.Data.QueryExtensions
{
    public abstract class DataVectorPredicate<T> : LogicModels.Predicate<T>, IDataVectorPredicate<T>
    {
        public T ExpectedValue { get; private set; }

        public DataVectorPredicate(string name, int arity, T expectedValue) 
            : base(name, arity)
        {
            this.ExpectedValue = expectedValue;
        }

        public abstract bool IsSatisfied(T vectorValue);

        protected bool Equals(IDataVectorPredicate<T> other)
        {
            return base.Equals(other) && Equals(this.ExpectedValue.Equals(other.ExpectedValue));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IDataVectorPredicate<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (this.ExpectedValue.GetHashCode());
            }
        }
    }
}
