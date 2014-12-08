using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrainTests.Defaults.Graphs.TestUtils;
using NetBrain.Abstracts.Graphs.Models;
using NetBrain.Defaults.Graphs.Traversals;

namespace NetBrainTests.Defaults.Graphs.Traversals
{
    [TestClass]
    public class UniformCostSearchTests
    {
        [TestMethod]
        public void TraverseTest()
        {
            //Given
            IGraph<string, double> romaniaTravelGraph = GraphsBuilder.BuildRomaniaTravelGraph();
            var subject = new UniformCostSearchTraversal<string>();

            var nodeStart = romaniaTravelGraph.Nodes.FirstOrDefault(node => node.Value.Equals("Arad"));
            var nodeEnd = romaniaTravelGraph.Nodes.FirstOrDefault(node => node.Value.Equals("Bucharest"));

            //When
            string path = string.Join(" ", subject.SearchPath(nodeStart, nodeEnd, romaniaTravelGraph).Select(node => node.Value).ToList());

            //Then
            Assert.AreEqual("Arad Sibiu RimnicuVicca Pitesti Bucharest", path);
        }
    }
}
