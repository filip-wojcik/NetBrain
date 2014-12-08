using System.Net.Mail;
using NetBrain.Abstracts.Graphs.Models;
using NetBrain.Defaults.Graphs.AdjacencyTableBased;

namespace NetBrainTests.Defaults.Graphs.TestUtils
{
    internal static class GraphsBuilder
    {
        public static IGraph<string, double> BuildUndirectedTestGraph1()
        {
            var root = new Node<string>("a");
            var b = new Node<string>("b");
            var c = new Node<string>("c");
            var d = new Node<string>("d");
            var e = new Node<string>("e");
            var f = new Node<string>("f");
            var g = new Node<string>("g");
            var h = new Node<string>("h");

            var graph = new Graph<string, double>(new INode<string>[] {root, b, c, d, e, f, g, h}, valueForUnassigned: double.MinValue);
            graph.AddEdge(root, b, 10);
            graph.AddEdge(b, d, 3);
            graph.AddEdge(b, e, 1);

            graph.AddEdge(root, c, 5);
            graph.AddEdge(c, f, 2);
            graph.AddEdge(c, g, 6);

            graph.AddEdge(e, h, 1);
            graph.AddEdge(f, h, 3);

            return graph;
        }

        public static IGraph<string, double> BuildDirectedTestGraph1()
        {
            var root = new Node<string>("a");
            var b = new Node<string>("b");
            var c = new Node<string>("c");
            var d = new Node<string>("d");
            var e = new Node<string>("e");
            var f = new Node<string>("f");
            var g = new Node<string>("g");
            var h = new Node<string>("h");

            var graph = new Graph<string, double>(new INode<string>[] { root, b, c, d, e, f, g, h }, directed: true);
            graph.AddEdge(root, b, 10);
            graph.AddEdge(b, d, 3);
            graph.AddEdge(b, e, 1);

            graph.AddEdge(root, c, 5);
            graph.AddEdge(c, f, 2);
            graph.AddEdge(c, g, 6);

            graph.AddEdge(e, h, 1);
            graph.AddEdge(f, h, 3);

            return graph;
        }

        public static IGraph<string, double> BuildRomaniaTravelGraph()
        {
            var root = new Node<string>("Arad");
            var zerind = new Node<string>("Zerind");
            var oradea = new Node<string>("Oradea");
            var sibiu = new Node<string>("Sibiu");
            var fagaras = new Node<string>("Fagaras");
            var bucharest = new Node<string>("Bucharest");
            var rimnicuVicca = new Node<string>("RimnicuVicca");
            var pitesti = new Node<string>("Pitesti");
            var timisoara = new Node<string>("Timisoara");
            var lugoj = new Node<string>("Lugoj");
            var mehadia = new Node<string>("Mehadia");
            var dorbeta = new Node<string>("Dorbeta");
            var craiova = new Node<string>("Craiova");

            var graph = new Graph<string, double>(new INode<string>[]
                {
                    root, 
                    zerind, 
                    oradea, 
                    sibiu, 
                    fagaras, 
                    bucharest, 
                    rimnicuVicca, 
                    pitesti, 
                    timisoara, 
                    lugoj, 
                    mehadia, 
                    dorbeta, 
                    craiova
                }, 
                directed: true
             );

            //"North path"
            graph.AddEdge(root, zerind, 75);
            graph.AddEdge(zerind, oradea, 71);
            graph.AddEdge(oradea, sibiu, 151);
            graph.AddEdge(sibiu, fagaras, 99);
            graph.AddEdge(fagaras, bucharest, 211);

            //"East path"
            graph.AddEdge(root, sibiu, 140);
            graph.AddEdge(root, sibiu, 140);
            graph.AddEdge(sibiu, rimnicuVicca, 80);

                //"South-east path"
                graph.AddEdge(sibiu, rimnicuVicca, 80);
                graph.AddEdge(rimnicuVicca, pitesti, 97);
                graph.AddEdge(rimnicuVicca, craiova, 146);
                graph.AddEdge(craiova, pitesti, 138);
                graph.AddEdge(pitesti, bucharest, 101);
            
            //"South path"
            graph.AddEdge(root, timisoara, 118);
            graph.AddEdge(timisoara, lugoj, 111);
            graph.AddEdge(lugoj, mehadia, 70);
            graph.AddEdge(mehadia, dorbeta, 75);
            graph.AddEdge(dorbeta, craiova, 120);
            

            return graph;
        }
    }
}