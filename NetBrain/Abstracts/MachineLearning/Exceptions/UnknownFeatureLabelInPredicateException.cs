using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Exceptions
{
    public class UnknownFeatureLabelInPredicateException : Exception
    {
        private const string MESSAGE = "Unknown feature label found in predicate: {0}. Allowed values: {1}";

        public UnknownFeatureLabelInPredicateException(string unknownFeatureLabel, IEnumerable<string> allowedLabels)
            : base(string.Format(MESSAGE, unknownFeatureLabel, string.Join(", ", allowedLabels)))
        {
            
        }
    }
}
