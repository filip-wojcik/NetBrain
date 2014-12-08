using System.Collections.Generic;
using System.Security.Policy;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    public class SplitOption<T> : ISplitOption<T>
    {
        public bool SplitOnConcreteValue { get; private set; }
        public bool IsDataNumberic { get; private set; }
        public bool IsSplitBinary { get; private set; }
        public string SplitLabel { get; private set; }
        public T ConcreteValueToSplit { get; private set; }
        public double ConcreteNumbericValueToSplit { get; private set; }
        public int SplitAxis { get; private set; }

        public SplitOption(
            int splitAxis,
            string splitLabel,
            bool isDataNumberic,
            bool splitOnConcreteValue,
            bool isSplitBinary = false,
            T concreteValueToSplit = default(T),
            double concreteNumbericValueToSplit = 0)
        {
            SplitAxis = splitAxis;
            SplitLabel = splitLabel;
            IsSplitBinary = isSplitBinary;
            IsDataNumberic = isDataNumberic;
            SplitOnConcreteValue = splitOnConcreteValue;
            ConcreteValueToSplit = concreteValueToSplit;
            ConcreteNumbericValueToSplit = concreteNumbericValueToSplit;
        }

        protected bool Equals(SplitOption<T> other)
        {
            return SplitOnConcreteValue.Equals(other.SplitOnConcreteValue) && 
                IsDataNumberic.Equals(other.IsDataNumberic) && 
                this.IsSplitBinary.Equals(other.IsSplitBinary) &&
                this.SplitLabel.Equals(other.SplitLabel) &&
                EqualityComparer<T>.Default.Equals(ConcreteValueToSplit, other.ConcreteValueToSplit) && 
                SplitAxis == other.SplitAxis;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SplitOption<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = SplitOnConcreteValue.GetHashCode();
                hashCode = (hashCode*397) ^ IsDataNumberic.GetHashCode();
                hashCode = (hashCode*397) ^ EqualityComparer<T>.Default.GetHashCode(ConcreteValueToSplit);
                hashCode = (hashCode*397) ^ SplitAxis;
                hashCode = (hashCode * 397) ^ IsSplitBinary.GetHashCode();
                hashCode = (hashCode*397) ^ SplitLabel.GetHashCode();
                return hashCode;
            }
        }
    }
}
