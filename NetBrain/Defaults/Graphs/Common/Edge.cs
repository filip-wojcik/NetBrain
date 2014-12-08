using NetBrain.Abstracts.Graphs.Models;
using System;

namespace NetBrain.Defaults.Graphs.Common
{
    public class Edge<T, V> : IEdge<T, V>
    {
        public INode<T> NodeFrom { get; private set; }
        public INode<T> NodeTo { get; private set; }
        public V Value { get; private set; }

        public Edge(INode<T> nodeFrom, INode<T> nodeTo, V value)
        {
            NodeFrom = nodeFrom;
            NodeTo = nodeTo;
            Value = value;
        }

        # region Equality metods

        protected bool Equals(Edge<T, V> other)
        {
            return Equals(NodeFrom, other.NodeFrom) && Equals(NodeTo, other.NodeTo) && Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Edge<T, V>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (NodeFrom != null ? NodeFrom.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (NodeTo != null ? NodeTo.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Value.GetHashCode();
                return hashCode;
            }
        }

        # endregion Equality metods
    }
}
