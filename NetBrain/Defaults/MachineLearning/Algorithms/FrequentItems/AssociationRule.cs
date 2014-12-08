using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.FrequentItems.Models;

namespace NetBrain.Defaults.MachineLearning.Algorithms.FrequentItems
{
    public class AssociationRule<T> : IAssociationRule<T>
    {
        public IFrequentItemsSet<T> Antecedent { get; private set; }
        public IFrequentItemsSet<T> Consequent { get; private set; }
        public double Confidence { get; set; }

        public AssociationRule(IFrequentItemsSet<T> antecedent, IFrequentItemsSet<T> consequent, double confidence = 0)
        {
            Antecedent = antecedent;
            Consequent = consequent;
            Confidence = confidence;
        }

        protected bool Equals(AssociationRule<T> other)
        {
            return Equals(Antecedent, other.Antecedent) && Equals(Consequent, other.Consequent) && Confidence.Equals(other.Confidence);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AssociationRule<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Antecedent != null ? Antecedent.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Consequent != null ? Consequent.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Confidence.GetHashCode();
                return hashCode;
            }
        }
    }
}
