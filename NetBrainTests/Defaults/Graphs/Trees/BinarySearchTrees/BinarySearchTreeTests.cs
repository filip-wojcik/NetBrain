using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.Graphs.Exceptions;
using NetBrain.Abstracts.Graphs.Models;
using NetBrain.Defaults.Graphs.Trees.BinarySearchTrees;

namespace NetBrainTests.Defaults.Graphs.Trees.BinarySearchTrees
{
    [TestClass()]
    public class BinarySearchTreeTests
    {
        [TestMethod()]
        public void AddChildTest()
        {
            //Given
            var root = new BinarySearchTree<string, double>("B");
            var child = new BinarySearchTree<string, double>("A");

            //When
            root.AddChild(child);

            //Then
            Assert.AreEqual(child, root.LeftChild);
        }

        [TestMethod()]
        public void RemoveNodeTest_RemoveLeftChild()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var B = new BinarySearchTree<string, double>("B");
            var A = new BinarySearchTree<string, double>("A");
            var C = new BinarySearchTree<string, double>("C");

            var F = new BinarySearchTree<string, double>("F");
            var E = new BinarySearchTree<string, double>("E");
            var G = new BinarySearchTree<string, double>("G");

            root.AddChild(B);
            B.AddChild(A);
            B.AddChild(C);

            root.AddChild(E);
            root.AddChild(F);
            root.AddChild(G);

            var expected = new string[] { "A", "C", "D", "E", "F", "G" };

            //When
            root.RemoveChildNode(B);

            //Then
            Assert.AreEqual(A, root.LeftChild);
            Assert.AreEqual(null, A.LeftChild);
            Assert.AreEqual(null, C.RightChild);

            Assert.AreEqual(E, root.RightChild);
        }

        [TestMethod()]
        public void RemoveNodeTest_RemoveRightChild()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var B = new BinarySearchTree<string, double>("B");
            var A = new BinarySearchTree<string, double>("A");
            var C = new BinarySearchTree<string, double>("C");

            var F = new BinarySearchTree<string, double>("F");
            var E = new BinarySearchTree<string, double>("E");
            var G = new BinarySearchTree<string, double>("G");

            root.AddChild(B);
            B.AddChild(A);
            B.AddChild(C);

            root.AddChild(F);
            root.AddChild(E);
            root.AddChild(G);

            var expected = new string[] { "A", "C", "D", "E", "F", "G" };

            //When
            root.RemoveChildNode(F);

            //Then
            Assert.AreEqual(B, root.LeftChild);
            Assert.AreEqual(A, B.LeftChild);
            Assert.AreEqual(C, B.RightChild);
            
            Assert.AreEqual(E, root.RightChild);
            Assert.AreEqual(null, G.RightChild);
            Assert.AreEqual(null, G.LeftChild);
        }

        [TestMethod()]
        public void RemoveNodeTest_RemoveRoot_LeftChildIsFullSubtree()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var B = new BinarySearchTree<string, double>("B");
            var A = new BinarySearchTree<string, double>("A");
            var C = new BinarySearchTree<string, double>("C");

            var F = new BinarySearchTree<string, double>("F");
            var E = new BinarySearchTree<string, double>("E");
            var G = new BinarySearchTree<string, double>("G");

            root.AddChild(B);
            B.AddChild(A);
            B.AddChild(C);

            root.AddChild(F);
            root.AddChild(E);
            root.AddChild(G);

            //When
            var updatedTreeRoot = root.RemoveSelf() as IBinaryTree<string, double>;
            
            //Then
            Assert.AreEqual(C, updatedTreeRoot);
            
            Assert.AreEqual(B, updatedTreeRoot.LeftChild);
            Assert.AreEqual(A, B.LeftChild);
            Assert.AreEqual(null, B.RightChild);

            Assert.AreEqual(F, updatedTreeRoot.RightChild);
            Assert.AreEqual(E, F.LeftChild);
            Assert.AreEqual(G, F.RightChild);
        }

        [TestMethod()]
        public void RemoveNodeTest_RemoveRoot_LeftChildIsLeftMax()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var B = new BinarySearchTree<string, double>("B");
            var A = new BinarySearchTree<string, double>("A");

            var F = new BinarySearchTree<string, double>("F");
            var E = new BinarySearchTree<string, double>("E");
            var G = new BinarySearchTree<string, double>("G");

            root.AddChild(B);
            B.AddChild(A);

            root.AddChild(F);
            root.AddChild(E);
            root.AddChild(G);

            //When
            var updatedTreeRoot = root.RemoveSelf() as IBinaryTree<string, double>;

            //Then
            Assert.AreEqual(B, updatedTreeRoot);
            Assert.AreEqual(A, updatedTreeRoot.LeftChild);
            Assert.AreEqual(F, updatedTreeRoot.RightChild);
        }

        [TestMethod()]
        public void RemoveNodeTest_RemoveRoot_NoLeftChildRightChildIsFullSubtree()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var F = new BinarySearchTree<string, double>("F");
            var E = new BinarySearchTree<string, double>("E");
            var G = new BinarySearchTree<string, double>("G");

            root.AddChild(F);
            root.AddChild(E);
            root.AddChild(G);

            var expected = new string[] { "E", "F", "G" };

            //When
            var updatedTreeRoot = root.RemoveSelf() as IBinaryTree<string, double>;
        
            //Then
            Assert.AreEqual(E, updatedTreeRoot);
            Assert.AreEqual(null, updatedTreeRoot.LeftChild);
            Assert.AreEqual(F, updatedTreeRoot.RightChild);
            Assert.AreEqual(null, F.LeftChild);
            Assert.AreEqual(G, F.RightChild);
        }

        [TestMethod()]
        public void RemoveNodeTest_RemoveRoot_NoLeftChildRightChildIsNewRoot()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var E = new BinarySearchTree<string, double>("E");
            var F = new BinarySearchTree<string, double>("F");
            var G = new BinarySearchTree<string, double>("G");

            root.AddChild(E);
            root.AddChild(F);
            root.AddChild(G);
            

            //When
            var updatedTreeRoot = root.RemoveSelf() as IBinaryTree<string, double>;

            //Then
            Assert.AreEqual(E, updatedTreeRoot);
            Assert.AreEqual(null, updatedTreeRoot.LeftChild);
            Assert.AreEqual(F, updatedTreeRoot.RightChild);
            Assert.AreEqual(G, F.RightChild);
        }

        [TestMethod()]
        public void RotateLeftTest()
        {
            //TODO: implement it
            Assert.Fail();
        }

        [TestMethod()]
        public void RotateRightTest_NoParentNode_LeftChildHasrightChild()
        {
            //Given
            var root = new BinarySearchTree<int, int>(6);
            var leftChild = new BinarySearchTree<int, int>(4);
            var leftGrandChild = new BinarySearchTree<int, int>(3);
            var rightGrandChild = new BinarySearchTree<int, int>(5);
            var rightChild = new BinarySearchTree<int, int>(7);

            root.AddChild(leftChild, 2);
            root.AddChild(rightChild, 3);
            root.AddChild(leftGrandChild, 5);
            root.AddChild(rightGrandChild, 1);

            //When
            root.RotateRight();

            //Then
            Assert.IsNull(leftChild.ParentTreeNode);
            Assert.AreEqual(leftGrandChild, leftChild.LeftChild);
            Assert.AreEqual(root, leftChild.RightChild);
            Assert.AreEqual(rightGrandChild, root.LeftChild);
            Assert.AreEqual(rightChild, root.RightChild);

        }

        [TestMethod()]
        public void RotateRightTest_NoParentNode_LeftChildDoesntHaveChild()
        {
            //TODO: implement it
            Assert.Fail();
        }

        [TestMethod()]
        public void RotateRightTest_ParentNodePresent_LeftChildHasrightChild()
        {
            //TODO: implement it
            Assert.Fail();
        }

        [TestMethod()]
        public void RotateRightTest_ParentNodePresent_LeftChildDoesntHaveChild()
        {
            //TODO: implement it
            Assert.Fail();
        }

        [TestMethod()]
        public void EdgeValueForChildTest()
        {
            //Given
            var root = new BinarySearchTree<string, double>("B");
            var child = new BinarySearchTree<string, double>("A");
            root.AddChild(child, 100);

            //When
            double edgeValueForChild = root.EdgeValueForChild(child);

            //Then
            Assert.AreEqual(100, edgeValueForChild);
        }

        [TestMethod()]
        public void AddEdgeTest_AddingEdgeToRoot()
        {
            //Given
            var root = new BinarySearchTree<string, double>("B");
            var child = new BinarySearchTree<string, double>("A");
            
            //When
            root.AddEdge(root, child, 10);

            //Then
            Assert.AreEqual(1, root.Children.Count(chd => chd != null));
            Assert.AreEqual(10, root.EdgeValueForChild(child));
        }

        [TestMethod()]
        public void AddEdgeTest_AddingEdgeToChild()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var B = new BinarySearchTree<string, double>("B");
            var A = new BinarySearchTree<string, double>("A");

            root.AddChild(B, 10);
            root.AddEdge(B, A, 11);

            //When
            IEdge<string, double> edge = root.GetEdge(B, A);

            //Then
            Assert.AreEqual(B, edge.NodeFrom);
            Assert.AreEqual(A, edge.NodeTo);
            Assert.AreEqual(11, edge.Value);
        }

        [TestMethod()]
        [ExpectedException(typeof(NodeNotFoundException<string>))]
        public void AddEdgeTest_AddingEdgeToNonExisting()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var B = new BinarySearchTree<string, double>("B");
            var A = new BinarySearchTree<string, double>("A");
            var X = new BinarySearchTree<string, double>("X");

            root.AddChild(B, 10);
            root.AddEdge(B, A, 11);

            //When
            IEdge<string, double> edge = root.GetEdge(X, B);
        }


        [TestMethod()]
        public void GetEdgeTest_EdgeFromRoot()
        {
            //Given
            var root = new BinarySearchTree<string, double>("B");
            var child = new BinarySearchTree<string, double>("A");
            root.AddEdge(root, child, 10);

            //When
            IEdge<string, double> edge = root.GetEdge(root, child);

            //Then
            Assert.AreEqual(root, edge.NodeFrom);
            Assert.AreEqual(child, edge.NodeTo);
            Assert.AreEqual(10, edge.Value);
            
        }

        [TestMethod()]
        public void GetEdgeTest_EdgeFromChild()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var B = new BinarySearchTree<string, double>("B");
            var A = new BinarySearchTree<string, double>("A");
            var C = new BinarySearchTree<string, double>("C");

            var F = new BinarySearchTree<string, double>("F");
            var E = new BinarySearchTree<string, double>("E");
            var G = new BinarySearchTree<string, double>("G");

            root.AddChild(B, 10);
            B.AddChild(A, 11);
            B.AddChild(C, 12);

            root.AddChild(F, 13);
            root.AddChild(E, 14);
            root.AddChild(G, 15);

            //When
            IEdge<string, double> edge = root.GetEdge(B, A);

            //Then
            Assert.AreEqual(B, edge.NodeFrom);
            Assert.AreEqual(A, edge.NodeTo);
            Assert.AreEqual(11, edge.Value);

        }

        [TestMethod()]
        public void HasEdgeTest_EdgeFromRoot()
        {
            //Given
            var root = new BinarySearchTree<string, double>("B");
            var child = new BinarySearchTree<string, double>("A");
            root.AddEdge(root, child, 10);

            //The
            Assert.IsTrue(root.HasEdge(root, child));
        }

        [TestMethod()]
        public void HasEdgeTest_EdgeFromChild()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var B = new BinarySearchTree<string, double>("B");
            var A = new BinarySearchTree<string, double>("A");
            var C = new BinarySearchTree<string, double>("C");

            var F = new BinarySearchTree<string, double>("F");
            var E = new BinarySearchTree<string, double>("E");
            var G = new BinarySearchTree<string, double>("G");

            root.AddChild(B);
            B.AddChild(A);
            B.AddChild(C);

            root.AddChild(F);
            root.AddChild(E);
            root.AddChild(G);

            //Then
            Assert.IsTrue(root.HasEdge(B, A));
        }

        [TestMethod()]
        public void RemoveEdgeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetNeighborsOfTest_NeighborsOfRoot()
        {
            //Given
            var root = new BinarySearchTree<string, double>("B");
            var child1 = new BinarySearchTree<string, double>("A");
            var child2 = new BinarySearchTree<string, double>("C");
            root.AddEdge(root, child1, 10);
            root.AddEdge(root, child2, 10);

            //When
            IEnumerable<INode<string>> neighbors = root.GetNeighborsOf(root);

            //Then
            Assert.AreEqual(2, neighbors.Count());
            Assert.IsTrue(neighbors.Contains(child1));
            Assert.IsTrue(neighbors.Contains(child2));
            
        }

        [TestMethod()]
        public void GetNeighborsOfTest_NeighborsOfChild()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");
            
            var B = new BinarySearchTree<string, double>("B");
            var A = new BinarySearchTree<string, double>("A");
            var C = new BinarySearchTree<string, double>("C");

            var F = new BinarySearchTree<string, double>("F");
            var E = new BinarySearchTree<string, double>("E");
            var G = new BinarySearchTree<string, double>("G");

            root.AddChild(B);
            B.AddChild(A);
            B.AddChild(C);

            root.AddChild(F);
            root.AddChild(E);
            root.AddChild(G);


            //When
            IEnumerable<INode<string>> neighbors = root.GetNeighborsOf(B);

            //Then
            Assert.AreEqual(3, neighbors.Count(neighbor => neighbor != null));
            Assert.IsTrue(neighbors.Contains(A));
            Assert.IsTrue(neighbors.Contains(C));
            Assert.IsTrue(neighbors.Contains(root));
        }

        [TestMethod()]
        public void GetValuedEdgesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void MinValueTest()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var B = new BinarySearchTree<string, double>("B");
            var A = new BinarySearchTree<string, double>("A");
            var C = new BinarySearchTree<string, double>("C");
            
            var F = new BinarySearchTree<string, double>("F");
            var E = new BinarySearchTree<string, double>("E");
            var G = new BinarySearchTree<string, double>("G");

            root.AddChild(B);
            B.AddChild(A);
            B.AddChild(C);

            root.AddChild(E);
            root.AddChild(F);
            root.AddChild(G);


            //When
            string minimalValue = root.MinimumValue();

            //Then
            Assert.AreEqual("A", minimalValue);
        }

        [TestMethod()]
        public void MinNodeTest()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var B = new BinarySearchTree<string, double>("B");
            var A = new BinarySearchTree<string, double>("A");
            var C = new BinarySearchTree<string, double>("C");

            var F = new BinarySearchTree<string, double>("F");
            var E = new BinarySearchTree<string, double>("E");
            var G = new BinarySearchTree<string, double>("G");

            root.AddChild(B);
            B.AddChild(A);
            B.AddChild(C);

            root.AddChild(E);
            root.AddChild(F);
            root.AddChild(G);


            //When
            IBinaryTree<string, double> minimalValue = root.MinimumNode();

            //Then
            Assert.AreEqual(A, minimalValue);
        }

        [TestMethod()]
        public void MaxValueTest()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var B = new BinarySearchTree<string, double>("B");
            var A = new BinarySearchTree<string, double>("A");
            var C = new BinarySearchTree<string, double>("C");

            var F = new BinarySearchTree<string, double>("F");
            var E = new BinarySearchTree<string, double>("E");
            var G = new BinarySearchTree<string, double>("G");

            root.AddChild(B);
            B.AddChild(A);
            B.AddChild(C);

            root.AddChild(E);
            root.AddChild(F);
            root.AddChild(G);


            //When
            string maximalValue = root.MaximumValue();

            //Then
            Assert.AreEqual("G", maximalValue);
        }

        [TestMethod()]
        public void MaxNodeTest()
        {
            //Given
            var root = new BinarySearchTree<string, double>("D");

            var B = new BinarySearchTree<string, double>("B");
            var A = new BinarySearchTree<string, double>("A");
            var C = new BinarySearchTree<string, double>("C");

            var F = new BinarySearchTree<string, double>("F");
            var E = new BinarySearchTree<string, double>("E");
            var G = new BinarySearchTree<string, double>("G");

            root.AddChild(B);
            B.AddChild(A);
            B.AddChild(C);

            root.AddChild(E);
            root.AddChild(F);
            root.AddChild(G);


            //When
            IBinaryTree<string, double> maximalValue = root.MaximumNode();

            //Then
            Assert.AreEqual(G, maximalValue);
        }
    }
}
