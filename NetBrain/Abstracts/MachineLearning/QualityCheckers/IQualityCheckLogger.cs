using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.QualityCheckers
{
    /// <summary>
    /// Tracks error changes during execution of algorithms
    /// </summary>
    public interface IQualityCheckLogger
    {
        /// <summary>
        /// Reports error checnge in the next iteration
        /// </summary>
        /// <param name="qualityData">Information about data quality change</param>
        void LogProgress(IQualityData qualityData);
    }
}
