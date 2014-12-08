using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Data.Models
{
    public class ProblemDomain : IProblemDomain
    {
        private readonly IList<IFeatureDomain> _featureDomains;

        public IFeatureDomain this[int idx]
        {
            get { return this._featureDomains[idx]; }
        }

        public int Size
        {
            get { return this._featureDomains.Count; }
        }

        public IEnumerable<IFeatureDomain> FeatureDomains
        {
            get { return new List<IFeatureDomain>(_featureDomains); }
        }

        public ProblemDomain(IList<IFeatureDomain> featureDomains)
        {
            _featureDomains = featureDomains;
        }
    }
}
