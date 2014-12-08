using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.QualityCheckers
{
    /// <summary>
    /// Single record of quality checking process, if processing spans on multiple iterations
    /// </summary>
    public interface IQualityData
    {
        int Iteration { get; set; }
        double ErrorRate { get; set; }
        bool TestData { get; set; }
    }
}
