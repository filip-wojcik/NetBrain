using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NetBrain.Abstracts.Graphs.Exceptions;
using NetBrain.Abstracts.Graphs.Models;
using NetBrain.Defaults.Graphs.Common;

namespace NetBrain.Defaults.Graphs.Trees.Base
{
    public abstract class BaseTree<T, V> : ITree<T, V>
    {
        # region Protected fields

        protected virtual IList<IChildContainer<T, V>> ChildrenNodesWithValues { get; set; }

        # endregion Protected fields

        # region Public properties

        public ITree<T, V> ParentNode { get; set; }

        public IEnumerable<ITree<T, V>> Children
        {
            get 
            { 
                return this.ChildrenNodesWithValues.Select(node =>
                {
                    ITree<T, V> childToReturn = null;
                    if (node != null) childToReturn = node.ChildTree;
                    return childToReturn;
                }); 
            }
        }

        public virtual IEnumerable<IChildContainer<T, V>> ChildrenWithValues
        {
            get { return this.ChildrenNodesWithValues; }
        }

        public bool IsLeaf
        {
            get { return !this.ChildrenNodesWithValues.Any(); }
        }

        public IList<INode<T>> Nodes
        {
            get { return this.ChildrenNodesWithValues.Select(node => node.ChildTree as INode<T>).ToList(); }
        }

        public bool HasChildren
        {
            get { return this.Children.Any() && Children.All(child => child != null); }
        }

        public T Value { get; set; }

        # endregion Public properties

        # region Construction

        protected BaseTree()
        {
            this.ChildrenNodesWithValues = new List<IChildContainer<T, V>>();
        }

        protected BaseTree(T value)
            : this()
        {
            Value = value;
        }

        protected BaseTree(T value, IEnumerable<IChildContainer<T, V>> children)
            : this(value)
        {
            if (children != null)
            {
                this.ChildrenNodesWithValues = new List<IChildContainer<T, V>>(children);
            }
            
        } 

        # endregion Construction

        # region Processing methods

        public V EdgeValueForChild(ITree<T, V> childNode)
        {
            foreach (var childWithValue in this.ChildrenNodesWithValues)
            {
                if (childWithValue.ChildTree.Equals(childNode)) return childWithValue.ChildValue;
            }
            throw new NodeNotFoundException<T>(childNode);
        }

        public virtual void AddChild(ITree<T, V> child, V value = default(V))
        {
            IChildContainer<T, V> matchingChild = null;
            foreach (var childWithValue in this.ChildrenNodesWithValues)
            {
                if (childWithValue.ChildTree.Equals(child)) matchingChild = childWithValue;
            }
            if (matchingChild != null) this.ChildrenNodesWithValues.Remove(matchingChild);
            this.ChildrenNodesWithValues.Add(BuildChildContainer(child, value));
        }

        protected virtual IChildContainer<T, V> BuildChildContainer(ITree<T, V> child, V value = default(V))
        {
            return new BaseChildContainer<T, V>(child, value);
        }

        public virtual void RemoveChildNode(ITree<T, V> child)
        {
            var currentChildren = new List<IChildContainer<T, V>>(this.ChildrenNodesWithValues);
            IChildContainer<T, V> matchingChild = null;
            foreach (var childWithValue in this.ChildrenNodesWithValues)
            {
                if (childWithValue.ChildTree.Equals(child)) matchingChild = childWithValue;
            }
            if (matchingChild != null)
            {
                currentChildren.Remove(matchingChild);
            }
            else
            {
                throw new NodeNotFoundException<T>(child);
            }
        }

        public virtual void AddEdge(INode<T> from, INode<T> to, V value = default(V))
        {
            Func<ITree<T, V>, ITree<T, V>, bool> processingMethod =
               (fromTree, toTree) =>
               {
                   fromTree.AddChild(toTree, value);
                   return true;
               };
            bool result = this.ProcessTwoNodes(from, to, processingMethod);
            if (!result) throw new NodeNotFoundException<T>(from);
        }

        public IEdge<T, V> GetEdge(INode<T> from, INode<T> to)
        {
            Func<ITree<T, V>, ITree<T, V>, IEdge<T, V>> processingMethod =
                (fromTree, toTree) =>
                {
                    foreach (var childWithValue in fromTree.ChildrenWithValues)
                    {
                        if (childWithValue.ChildTree.Equals(toTree))
                        {
                            return new Edge<T, V>(fromTree, toTree, childWithValue.ChildValue);
                        }
                    }
                    throw new NodeNotFoundException<T>(toTree);
                };
            return this.ProcessTwoNodes(from, to, processingMethod);
        }

        public bool HasEdge(INode<T> from, INode<T> to)
        {
            Func<ITree<T, V>, ITree<T, V>, bool> processingMethod =
                (fromTree, toTree) =>
                {
                    foreach (var childWithValue in fromTree.ChildrenWithValues)
                    {
                        if (childWithValue.ChildTree.Equals(toTree))
                        {
                            return true;
                        }
                    }
                    throw new NodeNotFoundException<T>(toTree);
                };
            return this.ProcessTwoNodes(from, to, processingMethod);
        }

        public abstract void RemoveEdge(INode<T> from, INode<T> to);
        public abstract ITree<T, V> RemoveSelf();

        public IEnumerable<INode<T>> GetNeighborsOf(INode<T> node)
        {
            if (node != null)
            {
                if (node is ITree<T, V>)
                {
                    var treeNode = node as ITree<T, V>;
                    if (treeNode.Equals(this))
                    {
                        foreach (var child in this.Children) yield return child;
                        if(this.ParentNode != null) yield return this.ParentNode;
                    }
                    else
                    {
                        bool matchingNodeFound = false;
                        foreach (var child in this.Children)
                        {
                            if (child.Equals(node))
                            {
                                foreach (var grandCHild in child.GetNeighborsOf(node)) yield return grandCHild;
                                matchingNodeFound = true;
                                break;
                            }
                        }
                        if (!matchingNodeFound) throw new NodeNotFoundException<T>(node);
                    }
                } else throw new NodeNotFoundException<T>(node);
            }
            
        }

        public IEnumerable<IEdge<T, V>> GetValuedEdges(INode<T> node)
        {
            if (node is ITree<T, V>)
            {
                var treeNode = node as ITree<T, V>;
                if (treeNode.Equals(this))
                {
                    foreach (var child in this.ChildrenNodesWithValues)
                    {
                        yield return new Edge<T, V>(this, child.ChildTree, child.ChildValue);
                    }
                }
                else
                {
                    bool matchingNodeFound = false;
                    foreach (var child in this.Children)
                    {
                        if (child.Equals(node))
                        {
                            foreach (var edge in child.GetValuedEdges(node)) yield return edge;
                            matchingNodeFound = true;
                            break;
                        }
                    }
                    if (!matchingNodeFound) throw new NodeNotFoundException<T>(node);
                }
            }
            throw new NodeNotFoundException<T>(node);
        }

        # endregion Processing methods

        # region Helper methods

        public U ProcessTwoNodes<U>(INode<T> from, INode<T> to,
            Func<ITree<T, V>, ITree<T, V>, U> processingMethod)
        {
            if (from is ITree<T, V> && to is ITree<T, V>)
            {
                var fromTree = from as ITree<T, V>;
                var toTree = to as ITree<T, V>;
                if (fromTree.Equals(this))
                {
                    return processingMethod(fromTree, toTree);
                }
                else
                {
                    foreach(var child in this.Children)
                    {
                        if (fromTree.Equals(child))
                        {
                            return processingMethod(fromTree, toTree);
                        }
                    }
                    throw new NodeNotFoundException<T>(fromTree);
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        # endregion Helper methods


    }
}
