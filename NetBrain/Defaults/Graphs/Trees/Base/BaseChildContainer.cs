using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Graphs.Models;

namespace NetBrain.Defaults.Graphs.Trees.Base
{
    public class BaseChildContainer<T, V> : IChildContainer<T, V>
    {
        public ITree<T, V> ChildTree { get; private set; }

        public V ChildValue { get; private set; }

        public BaseChildContainer(ITree<T, V> childTree, V childValue)
        {
            ChildTree = childTree;
            ChildValue = childValue;
        }

        protected bool Equals(BaseChildContainer<T, V> other)
        {
            return Equals(ChildTree, other.ChildTree) && EqualityComparer<V>.Default.Equals(ChildValue, other.ChildValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BaseChildContainer<T, V>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ChildTree != null ? ChildTree.GetHashCode() : 0)*397) ^ EqualityComparer<V>.Default.GetHashCode(ChildValue);
            }
        }
    }
}
