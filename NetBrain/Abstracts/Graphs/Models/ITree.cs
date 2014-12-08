using System;
using System.Collections.Generic;

namespace NetBrain.Abstracts.Graphs.Models
{
    public interface ITree<T, V> : IGraph<T, V>, INode<T> 
    {
        ITree<T, V> ParentNode { get; set; } 
        IEnumerable<ITree<T, V>> Children { get; }
        IEnumerable<IChildContainer<T, V>> ChildrenWithValues { get; }
        bool IsLeaf { get; }
        bool HasChildren { get; }

        V EdgeValueForChild(ITree<T, V> childNode);

        void AddChild(ITree<T, V> child, V value = default(V));
        void RemoveChildNode(ITree<T, V> child);
        ITree<T, V> RemoveSelf();
    }
}
