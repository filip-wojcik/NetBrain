using NetBrain.Abstracts.Graphs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.Graphs.Traversals
{
    public interface IBinaryTreeTraversal<T, V>
    {
        IList<IBinaryTree<T, V>> TraverseTree(IBinaryTree<T, V> startNode);
    }
}
