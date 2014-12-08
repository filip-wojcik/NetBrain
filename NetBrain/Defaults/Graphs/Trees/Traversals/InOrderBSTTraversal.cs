using System;
using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.Graphs.Exceptions;
using NetBrain.Abstracts.Graphs.Traversals;
using NetBrain.Abstracts.Graphs.Models;

namespace NetBrain.Defaults.Graphs.Trees.Traversals
{
    public class InOrderBSTTraversal<T, V> : IBinaryTreeTraversal<T, V>
        where T : IComparable<T>
        where V : IComparable<V>
    {

        public IList<IBinaryTree<T, V>> TraverseTree(IBinaryTree<T, V> startNode)
        {
            if (startNode != null)
            {
                var dataToReturn = new List<IBinaryTree<T, V>>();
                if(startNode.HasLeftChild) dataToReturn.AddRange(this.TraverseTree(startNode.LeftChild));
                dataToReturn.Add(startNode);
                if (startNode.HasRightChild) dataToReturn.AddRange(this.TraverseTree(startNode.RightChild));
                return dataToReturn;
            } else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
