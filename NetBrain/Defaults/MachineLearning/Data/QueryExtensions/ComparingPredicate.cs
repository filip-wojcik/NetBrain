using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data.QueryExtensions;

namespace NetBrain.Defaults.MachineLearning.Data.QueryExtensions
{
    public abstract class ComparingPredicate<T> : BaseDataVectorPredicate<T>
    {
        protected IComparer<T> customComparer;

        public ComparingPredicate(string name, T expectedValue, IComparer<T> customComparer = null)
            : base(name, expectedValue)
        {
            this.customComparer = customComparer ?? Comparer<T>.Default;
        }
    }
}
