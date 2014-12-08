using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Defaults.MachineLearning.Data.QueryExtensions
{
    public class GreaterThanOrEqualPredicate<T> : ComparingPredicate<T>
    {
        public GreaterThanOrEqualPredicate(string name, T expectedValue, IComparer<T> customComparer = null) : base(name, expectedValue, customComparer)
        {
        }

        public override bool IsSatisfied(T vectorValue)
        {
            if (base.customComparer.Compare(vectorValue, base.ExpectedValue) >= 0) return true;
            else return false;
        }
    }
}
