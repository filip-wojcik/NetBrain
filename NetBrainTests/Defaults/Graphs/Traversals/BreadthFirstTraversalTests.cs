using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.Graphs.Models;
using NetBrain.Defaults.Graphs.Traversals;
using NetBrainTests.Defaults.Graphs.TestUtils;
using System.Collections.Generic;

namespace NetBrainTests.Defaults.Graphs.Traversals
{
    [TestClass]
    public class BreadthFirstTraversalTests
    {
        [TestMethod]
        public void TraverseTest()
        {
            //Given
            IGraph<string, double> testGraph = GraphsBuilder.BuildDirectedTestGraph1();
            var subject = new BreadthFirstTraversal<string, double>();

            var nodeStart = testGraph.Nodes.FirstOrDefault(node => node.Value.Equals("a"));
            var nodeEnd = testGraph.Nodes.FirstOrDefault(node => node.Value.Equals("h"));

            //When
            string path = string.Join(" ", subject.SearchPath(nodeStart, nodeEnd, testGraph).Select(node => node.Value).ToList());
            
            //Then
            Assert.AreEqual("a b e h", path);
        }
    }
}