using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.Graphs.Models;
using NetBrainTests.Defaults.Graphs.TestUtils;

namespace NetBrainTests.Defaults.Graphs.AdjacencyTableBased
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void GetNeighborsOfTest()
        {
            //Given
            IGraph<string, double> testGraph = GraphsBuilder.BuildUndirectedTestGraph1();
            INode<string> nodeB = testGraph.Nodes.FirstOrDefault(node => node.Value.Equals("b"));

            //When
            IEnumerable<INode<string>> neighbors = testGraph.GetNeighborsOf(nodeB);

            //Then
            Assert.IsTrue(neighbors.Any(node => node.Value.Equals("a")));
            Assert.IsTrue(neighbors.Any(node => node.Value.Equals("d")));
            Assert.IsTrue(neighbors.Any(node => node.Value.Equals("e")));
        }

        [TestMethod]
        public void GetValuedEdgesTest()
        {
            //Given
            IGraph<string, double> testGraph = GraphsBuilder.BuildUndirectedTestGraph1();
            INode<string> nodeB = testGraph.Nodes.FirstOrDefault(node => node.Value.Equals("b"));

            //When
            IEnumerable<IEdge<string, double>> valuedEdges = testGraph.GetValuedEdges(nodeB);

            //Then
            Assert.IsTrue(valuedEdges.Any(edge => edge.Value.Equals(10) && edge.NodeTo.Value.Equals("a")));
            Assert.IsTrue(valuedEdges.Any(edge => edge.Value.Equals(3) && edge.NodeTo.Value.Equals("d")));
            Assert.IsTrue(valuedEdges.Any(edge => edge.Value.Equals(1) && edge.NodeTo.Value.Equals("e")));
        }

        [TestMethod]
        public void AddEdgeTest()
        {
            //Given
            IGraph<string, double> testGraph = GraphsBuilder.BuildUndirectedTestGraph1();
            INode<string> nodeB = testGraph.Nodes.FirstOrDefault(node => node.Value.Equals("b"));
            INode<string> nodeH = testGraph.Nodes.FirstOrDefault(node => node.Value.Equals("h"));
            Assert.IsFalse(testGraph.HasEdge(nodeB, nodeH));

            //When
            testGraph.AddEdge(nodeB, nodeH);

            //Then
            Assert.IsTrue(testGraph.HasEdge(nodeB, nodeH));
        }

        [TestMethod]
        public void GetEdgeTest()
        {
            //Given
            IGraph<string, double> testGraph = GraphsBuilder.BuildUndirectedTestGraph1();
            INode<string> nodeA = testGraph.Nodes.FirstOrDefault(node => node.Value.Equals("a"));
            INode<string> nodeB = testGraph.Nodes.FirstOrDefault(node => node.Value.Equals("b"));

            //When
            IEdge<string, double> edge = testGraph.GetEdge(nodeA, nodeB);

            //Then
            Assert.AreEqual(10, edge.Value);
        }

        [TestMethod]
        public void RemoveEdgeTest()
        {
            //Given
            IGraph<string, double> testGraph = GraphsBuilder.BuildUndirectedTestGraph1();
            INode<string> nodeA = testGraph.Nodes.FirstOrDefault(node => node.Value.Equals("a"));
            INode<string> nodeB = testGraph.Nodes.FirstOrDefault(node => node.Value.Equals("b"));
            Assert.IsTrue(testGraph.HasEdge(nodeA, nodeB));

            //When
            testGraph.RemoveEdge(nodeA, nodeB);

            //Then
            Assert.IsFalse(testGraph.HasEdge(nodeA, nodeB));
        }
    }
}