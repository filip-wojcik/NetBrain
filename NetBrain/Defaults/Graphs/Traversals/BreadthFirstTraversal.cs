using System;
using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.Graphs.Traversals;
using NetBrain.Abstracts.Graphs.Models;
using NetBrain.Defaults.Graphs.AdjacencyTableBased;
using NetBrain.Defaults.Graphs.Traversals.Helpers;

namespace NetBrain.Defaults.Graphs.Traversals
{
    public class BreadthFirstTraversal<T, V> : TraversalBase<T, V> where V : IComparable<V>
    {
        protected override IList<INode<T>> Process(INode<T> startNode, INode<T> targetNode, IGraph<T, V> graph)
        {
            var nodesToVisit = new Queue<NodeWithAncestor<T>>();
            nodesToVisit.Enqueue(new NodeWithAncestor<T>(null, startNode));

            var alreadyVisitedNodes = new HashSet<INode<T>>();

            while (nodesToVisit.Any())
            {
                NodeWithAncestor<T> currentPathNode = nodesToVisit.Dequeue();
                
                if(alreadyVisitedNodes.Contains(currentPathNode.CurrentNode)) continue;
                alreadyVisitedNodes.Add(currentPathNode.CurrentNode);

                if (currentPathNode.CurrentNode.Equals(targetNode))
                {
                    return this.Backtrace(currentPathNode, new List<INode<T>>());
                }
                foreach (var child in graph.GetNeighborsOf(currentPathNode.CurrentNode))
                {
                    nodesToVisit.Enqueue(new NodeWithAncestor<T>(currentPathNode, child));
                }
            }
            
            return null;
        }

        protected IList<INode<T>> Backtrace(NodeWithAncestor<T> currentPathNode, List<INode<T>> path)
        {
            path.Add(currentPathNode.CurrentNode);
            if (!currentPathNode.HasParent())
            {
                path.Reverse();
                return path;

            }
            return this.Backtrace(currentPathNode.AncestorNode, path);
        }
    }
}
