using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.QualityCheckers;

namespace NetBrainTests.Defaults.MachineLearning.QualityCheckers
{
    internal class QualityCheckerLoggerStub : IQualityCheckLogger
    {
        public IList<IQualityData> Results { get; set; }

        public QualityCheckerLoggerStub()
        {
            this.Results = new List<IQualityData>();
        }

        public void LogProgress(IQualityData qualityData)
        {
            this.Results.Add(qualityData);
        }
    }
}
