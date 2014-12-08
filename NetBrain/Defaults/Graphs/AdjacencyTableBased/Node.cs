using System.Collections.Generic;
using NetBrain.Abstracts.Graphs.Models;

namespace NetBrain.Defaults.Graphs.AdjacencyTableBased
{
    public class Node<T> :  INode<T>
    {
        public T Value { get; set; }

        public Node(T value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        # region Equality methods

        protected bool Equals(Node<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Node<T>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }

        # endregion Equality methods
    }
}
