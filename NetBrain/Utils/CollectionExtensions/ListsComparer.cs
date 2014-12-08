using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Utils.CollectionExtensions
{
    class ListsComparer<T> : IEqualityComparer<IList<T>>
    {
        public bool Equals(IList<T> x, IList<T> y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode(IList<T> obj)
        {
            int hashcode = 0;
            foreach (T t in obj)
            {
                hashcode ^= t.GetHashCode();
            }
            return hashcode;
        }
    }
}
