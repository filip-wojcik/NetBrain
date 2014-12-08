using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.QualityCheckers
{
    public interface IClassificationQualityData<T> : IQualityData
    {
        IContingencyTable<T> ContingencyTable { get; set; }
    }
}
