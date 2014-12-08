using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.UI;
using NetBrain.Abstracts.Graphs.Exceptions;
using NetBrain.Abstracts.Graphs.Models;
using NetBrain.Defaults.Graphs.AdjacencyTableBased;
using NetBrain.Defaults.Graphs.Trees.Base;

namespace NetBrain.Defaults.Graphs.Trees.BinarySearchTrees
{
    public class BinarySearchTree<T, V> : BaseTree<T, V>, IBinaryTree<T, V>
        where T: IComparable<T>
        where V :IComparable<V>
    {
        # region Public properties

        public IBinaryTree<T, V> ParentTreeNode
        {
            get { return this.ParentNode as IBinaryTree<T, V>; }
            set { this.ParentNode = value; }
        }

        public IBinaryTree<T, V> LeftChild
        {
            get 
            { 
                if(this.HasLeftChild) return base.ChildrenNodesWithValues[0].ChildTree as IBinaryTree<T, V>;
                else
                {
                    return null;
                }
            }
        }

        public IBinaryTree<T, V> RightChild
        {
            get
            {
                if (this.HasRightChild) return base.ChildrenNodesWithValues[1].ChildTree as IBinaryTree<T, V>;
                else
                {
                    return null;
                }
            }
        }

        public bool HasLeftChild
        {
            get { return base.ChildrenNodesWithValues[0] != null; }
        }

        public bool HasRightChild
        {
            get { return base.ChildrenNodesWithValues[1] != null; }
        }

        public V LeftChildLinkValue
        {
            get 
            {
                if (this.HasLeftChild) return base.ChildrenNodesWithValues[0].ChildValue;
                else
                {
                    return default(V);
                } 
            }

            set
            {
                if (this.HasLeftChild)
                {
                    IChildContainer<T, V> existingChild = base.ChildrenNodesWithValues[0];
                    base.ChildrenNodesWithValues[0] = new BaseChildContainer<T, V>(existingChild.ChildTree, value);
                } else throw new ArgumentNullException();
            }
        }

        public V RightChildLinkValue
        {
            get
            {
                if (this.HasRightChild) return base.ChildrenNodesWithValues[1].ChildValue;
                else
                {
                    return default(V);
                }
            }
            set
            {
                if (this.HasRightChild)
                {
                    IChildContainer<T, V> existingChild = base.ChildrenNodesWithValues[1];
                    base.ChildrenNodesWithValues[1] = new BaseChildContainer<T, V>(existingChild.ChildTree, value);
                }
                else throw new ArgumentNullException();
            }
        }


        # endregion Public properties

        # region Construction

        public BinarySearchTree(T value)
            : base(value)
        {
            base.ChildrenNodesWithValues = new List<IChildContainer<T, V>>(){ null, null };
        }

        public BinarySearchTree(T value, IEnumerable<IChildContainer<T, V>>children)
            : base(value)
        {
            if (ChildrenCountIsValid(children))
            {
                foreach(var child in children) this.AddChild(child.ChildTree, child.ChildValue);
            }
        }

        # endregion Construction

        # region Processing methods

        public void SetLeftChild(ITree<T, V> child, V linkValue = default(V))
        {
            this.AddChildUnderIndex(child, 0, linkValue);
        }

        public void SetRightChild(ITree<T, V> child, V linkValue = default(V))
        {
            this.AddChildUnderIndex(child, 1, linkValue);
        }

        protected void AddChildUnderIndex(ITree<T, V> child, int idx, V value = default(V))
        {
            if(child != null) child.ParentNode = this;
            base.ChildrenNodesWithValues[idx] = new BaseChildContainer<T, V>(child, value);
        }

        public override void AddChild(ITree<T, V> child, V value = default(V))
        {
            if (child.Value.CompareTo(this.Value) < 0)
            {
                if (this.HasLeftChild) this.LeftChild.AddChild(child, value);
                else this.SetLeftChild(child, value);
            }
            else
            {
                if (this.HasRightChild) this.RightChild.AddChild(child, value);
                else this.SetRightChild(child, value);
            }
        }

        public override void RemoveChildNode(ITree<T, V> nodeToRemove)
        {
            if (this.HasLeftChild && this.LeftChild.Equals(nodeToRemove))
            {
                var updatedLeftSide = this.LeftChild.RemoveSelf();
                var previousLeftChildLinkValue = this.LeftChildLinkValue;
                this.SetLeftChild(updatedLeftSide, previousLeftChildLinkValue);
            }
            else if (this.HasRightChild && this.RightChild.Equals(nodeToRemove))
            {
                var updatedRightSide = this.RightChild.RemoveSelf();
                var previousRightChildLinkValue = this.RightChildLinkValue;
                this.SetRightChild(updatedRightSide, previousRightChildLinkValue);
            }
            else
            {
                throw new NodeNotFoundException<T>(nodeToRemove);
            }
        }

        public override void RemoveEdge(INode<T> from, INode<T> to)
        {
            throw new NotImplementedException();
        }

        public override ITree<T, V> RemoveSelf()
        {
            IBinaryTree<T, V> replacement = null;
            if (this.HasLeftChild) replacement = this.LeftChild.MaximumNode();
            else if (this.HasRightChild) replacement = this.RightChild.MinimumNode();
            else return null;
            
            if(this.HasLeftChild && this.LeftChild != replacement) replacement.AddChild(this.LeftChild, this.LeftChildLinkValue);
            if(this.HasRightChild && this.RightChild != replacement) replacement.AddChild(this.RightChild, this.RightChildLinkValue);

            UpdateParentReferenceAfterRemoval(this);
            UpdateParentReferenceAfterRemoval(replacement);
            return replacement;
        }

        public T MinimumValue()
        {
            if (this.HasLeftChild) return this.LeftChild.MinimumValue();
            else return this.Value;
        }

        public IBinaryTree<T, V> MinimumNode()
        {
            if (this.HasLeftChild) return this.LeftChild.MinimumNode();
            else return this;
        }

        public T MaximumValue()
        {
            if (this.HasRightChild) return this.RightChild.MaximumValue();
            else return this.Value;
        }

        public IBinaryTree<T, V> MaximumNode()
        {
            if (this.HasRightChild) return this.RightChild.MaximumNode();
            else return this;
        }

        public void RotateLeft()
        {
            throw new NotImplementedException();
        }

        public void RotateRight()
        {
            throw new NotImplementedException();
        }

        # endregion Processing methods

        # region Helper methods

        protected static void UpdateParentReferenceAfterRemoval(IBinaryTree<T, V> nodeToBeUpdated)
        {
            if (nodeToBeUpdated.ParentTreeNode != null)
            {
                if (nodeToBeUpdated.ParentTreeNode.HasLeftChild &&
                    nodeToBeUpdated.Equals(nodeToBeUpdated.ParentTreeNode.LeftChild))
                {
                    nodeToBeUpdated.ParentTreeNode.SetLeftChild(null, default(V));
                }
                else if (nodeToBeUpdated.ParentTreeNode.HasRightChild &&
                         nodeToBeUpdated.Equals(nodeToBeUpdated.ParentTreeNode.RightChild))
                {
                    nodeToBeUpdated.ParentTreeNode.SetRightChild(null, default(V));
                }
            }
        }

        public static bool ChildrenCountIsValid(IEnumerable<IChildContainer<T, V>> children)
        {
            if (children == null || children.Count() > 2)
            {
                return false;
            }
            return true;
        }
        
        # endregion Helper methods
    }
}
