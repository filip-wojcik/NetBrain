using System;
using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.Graphs.Models;
using NetBrain.Defaults.Graphs.Traversals.Helpers;

namespace NetBrain.Defaults.Graphs.Traversals
{
    public class UniformCostSearchTraversal<T> : BreadthFirstTraversal<T, double>
    {
        protected override IList<INode<T>> Process(INode<T> startNode, INode<T> targetNode, IGraph<T, double> graph)
        {
            var visitedNodes = new Dictionary<INode<T>, NodeWithAncestor<T>>();

            var sortedNodesToExpand = new SortedList<double, NodeWithAncestor<T>>();
            sortedNodesToExpand.Add(0, new NodeWithAncestor<T>(null, startNode));
            
            bool solutionFound = false;

            while (sortedNodesToExpand.Any())
            {
                var firstToExpand = sortedNodesToExpand.First();
                var currentPathNode = firstToExpand.Value;

                if (currentPathNode.CurrentNode.Equals(targetNode)) solutionFound = true;
                sortedNodesToExpand.Remove(firstToExpand.Key);
                foreach (var edge in graph.GetValuedEdges(currentPathNode.CurrentNode))
                {
                    double totalPathCost = edge.Value + currentPathNode.TotalPathCost;
                    NodeWithAncestor<T> nodeToExpand;
                    if (visitedNodes.TryGetValue(edge.NodeTo, out nodeToExpand))
                    {
                        if (nodeToExpand.TotalPathCost > totalPathCost)
                        {
                            nodeToExpand.AncestorNode = currentPathNode;
                            nodeToExpand.TotalPathCost = totalPathCost;
                        }
                        else
                        {
                            nodeToExpand = new NodeWithAncestor<T>(currentPathNode, edge.NodeTo, totalPathCost);
                        }
                    }
                    else
                    {
                        nodeToExpand = new NodeWithAncestor<T>(currentPathNode, edge.NodeTo, totalPathCost);
                        visitedNodes.Add(edge.NodeTo, nodeToExpand);
                    }
                    sortedNodesToExpand.Add(nodeToExpand.TotalPathCost, nodeToExpand);
                }
            }

            if (solutionFound)
            {
                return base.Backtrace(visitedNodes[targetNode], new List<INode<T>>());
            }
            else
            {
                return new INode<T>[0];
            }
        }
    }
}
