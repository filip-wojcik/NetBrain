using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.QualityCheckers;

namespace NetBrain.Defaults.MachineLearning.QualityCheckers
{
    public class ExpectedActualPair<T> : IExpectedActualPair<T>
    {
        public IList<T> ExpectedOutcome { get; set; }

        public IList<T> ActualOutcome { get; set; }

        public ExpectedActualPair(IList<T> expectedOutcome, IList<T> actualOutcome)
        {
            ExpectedOutcome = expectedOutcome;
            ActualOutcome = actualOutcome;
        }

        protected bool Equals(IExpectedActualPair<T> other)
        {
            if (other == null) return false;
            return this.ExpectedOutcome.SequenceEqual(other.ExpectedOutcome) &&
                   this.ActualOutcome.SequenceEqual(other.ActualOutcome);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IExpectedActualPair<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 23;
                foreach (var element in this.ExpectedOutcome) hash ^= element.GetHashCode();
                foreach (var element in this.ActualOutcome) hash ^= element.GetHashCode();
                return hash;
            }
        }
    }
}
