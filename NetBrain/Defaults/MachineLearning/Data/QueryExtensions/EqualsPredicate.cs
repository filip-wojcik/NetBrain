using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Defaults.MachineLearning.Data.QueryExtensions
{
    public class EqualsPredicate<T> : BaseDataVectorPredicate<T>
    {
        public EqualsPredicate(string name, T expectedValue) : base(name, expectedValue)
        {
        }

        public override bool IsSatisfied(T vectorValue)
        {
            return base.ExpectedValue.Equals(vectorValue);
        }
    }
}
