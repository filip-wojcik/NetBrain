using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Defaults.MachineLearning.Data.QueryExtensions
{
    public class NumericObjectComparer : IComparer<object>
    {
        public int Compare(object x, object y)
        {
            double numberX = Convert.ToDouble(x);
            double numberY = Convert.ToDouble(y);
            return Comparer<double>.Default.Compare(numberX, numberY);
        }
    }
}
