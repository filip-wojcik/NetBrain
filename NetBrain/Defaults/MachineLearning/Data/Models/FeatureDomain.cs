using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Defaults.MachineLearning.Data.Models
{
    public class FeatureDomain : IFeatureDomain
    {
        public double MinValue { get; private set; }
        public double MaxValue { get; private set; }

        public FeatureDomain(double minValue, double maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        protected bool Equals(IFeatureDomain other)
        {
            return MinValue.Equals(other.MinValue) && MaxValue.Equals(other.MaxValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IFeatureDomain) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (MinValue.GetHashCode()*397) ^ MaxValue.GetHashCode();
            }
        }
    }
}
