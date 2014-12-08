using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.FrequentItems.Models;

namespace NetBrain.Defaults.MachineLearning.Algorithms.FrequentItems
{
    public class FrequentItemsSet<T> : IFrequentItemsSet<T>
    {
        public ISet<T> Items { get; private set; }

        public double SupportValue { get; set; }

        public FrequentItemsSet(ISet<T> items, double supportValue)
        {
            Items = items;
            SupportValue = supportValue;
        }

        public FrequentItemsSet(double supportValue, params T[] items)
            : this(new HashSet<T>(items), supportValue)
        {
        }

        protected bool Equals(FrequentItemsSet<T> other)
        {
            return this.Items.SetEquals(other.Items) && SupportValue.Equals(other.SupportValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FrequentItemsSet<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 234;
                foreach (var item in this.Items) hash ^= item.GetHashCode();
                hash ^= this.SupportValue.GetHashCode();
                return hash;
            }
        }
    }
}
