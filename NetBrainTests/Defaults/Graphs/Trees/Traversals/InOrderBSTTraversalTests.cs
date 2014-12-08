using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Defaults.Graphs.Trees.BinarySearchTrees;
using NetBrain.Defaults.Graphs.Trees.Traversals;

namespace NetBrainTests.Defaults.Graphs.Trees.Traversals
{
    [TestClass()]
    public class InOrderBSTTraversalTests
    {
        [TestMethod()]
        public void TraverseTreeTest()
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

            var subject = new InOrderBSTTraversal<string, double>();
            var expected = new string[] { "A", "B", "C", "D", "E", "F", "G" };

            //When
            var nodes = subject.TraverseTree(root).Select(node => node.Value);

            //Then
            Assert.IsTrue(expected.SequenceEqual(nodes));
        }
    }
}
