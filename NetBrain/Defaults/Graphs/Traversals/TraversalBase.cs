using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Graphs.Traversals;
using NetBrain.Abstracts.Graphs.Models;

namespace NetBrain.Defaults.Graphs.Traversals
{
    public abstract class TraversalBase<T, V> : IGraphTraversal<T, V> where V : IComparable<V>
    {
        public IList<INode<T>> SearchPath(INode<T> startNode, INode<T> targetNode, IGraph<T, V> graph)
        {
            if (!InputIsValid(startNode, targetNode, graph))
            {
                return new INode<T>[0];
            }
            if (startNode.Equals(targetNode)) return new INode<T>[] { startNode };
            return Process(startNode, targetNode, graph);
        }

        protected bool InputIsValid(INode<T> startNode, INode<T> targetNode, IGraph<T, V> graph)
        {
            if (graph == null || !(graph.Nodes.Any()) || targetNode == null || startNode == null) return false;
            if (!(graph.Nodes.Contains(startNode)) || !(graph.Nodes.Contains(targetNode))) return false;
            return true;
        }

        protected abstract IList<INode<T>> Process(INode<T> startNode, INode<T> targetNode, IGraph<T, V> graph);
    }
}
