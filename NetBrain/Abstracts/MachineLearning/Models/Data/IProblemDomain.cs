using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Models.Data
{
    /// <summary>
    /// Represents numeric domain of the given problem solved by algorithm
    /// Contains multiple feature domains (one for every feature)
    /// </summary>
    public interface IProblemDomain
    {
        IFeatureDomain this[int idx] { get; }
        IEnumerable<IFeatureDomain> FeatureDomains { get; }
        int Size { get; }
    }
}
