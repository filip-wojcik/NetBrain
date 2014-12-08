using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Models.Data
{
    /// <summary>
    /// Describes domain of the feature - min max value that a given 
    /// feature can take
    /// </summary>
    public interface IFeatureDomain
    {
        double MinValue { get; }
        double MaxValue { get; }
    }
}
