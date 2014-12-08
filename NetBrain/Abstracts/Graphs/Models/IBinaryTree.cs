using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.Graphs.Models
{
    public interface IBinaryTree<T, V> : ITree<T, V>
    {
        IBinaryTree<T, V> ParentTreeNode { get; set; }
        IBinaryTree<T, V> LeftChild { get; }
        IBinaryTree<T, V> RightChild { get; }

        bool HasLeftChild { get; }
        bool HasRightChild { get; }

        V LeftChildLinkValue { get; set; }
        V RightChildLinkValue { get; set; }

        void SetLeftChild(ITree<T, V> child, V linkValue = default(V));
        void SetRightChild(ITree<T, V> child, V linkValue = default(V));
        T MinimumValue();
        IBinaryTree<T, V> MinimumNode();

        T MaximumValue();
        IBinaryTree<T, V> MaximumNode();

        void RotateLeft();
        void RotateRight();
    }
}
