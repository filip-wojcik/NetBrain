using System;
using System.Collections;
using NetBrain.Abstracts.Graphs.Models;
using System.Collections.Generic;

namespace NetBrain.Defaults.Graphs.Traversals.Helpers
{
    public class NodeWithAncestor<T> : IComparable<NodeWithAncestor<T>>
    {
        public NodeWithAncestor<T> AncestorNode { get; set; }
        public INode<T> CurrentNode { get; set; }
        public double TotalPathCost { get; set; }

        public NodeWithAncestor(NodeWithAncestor<T> ancestorNode, INode<T> currentNode, double totalPathCost = 0)
        {
            AncestorNode = ancestorNode;
            CurrentNode = currentNode;
            this.TotalPathCost = totalPathCost;
        }

        public bool HasParent()
        {
            return this.AncestorNode != null;
        }

        public int CompareTo(NodeWithAncestor<T> other)
        {
            return this.TotalPathCost.CompareTo(other.TotalPathCost);
        }

        protected bool Equals(NodeWithAncestor<T> other)
        {
            return Equals(AncestorNode, other.AncestorNode) && Equals(CurrentNode, other.CurrentNode) && TotalPathCost.Equals(other.TotalPathCost);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NodeWithAncestor<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (AncestorNode != null ? AncestorNode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (CurrentNode != null ? CurrentNode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ TotalPathCost.GetHashCode();
                return hashCode;
            }
        }
    }
}
