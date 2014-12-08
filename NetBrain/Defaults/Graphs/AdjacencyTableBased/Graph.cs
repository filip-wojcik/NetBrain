using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NetBrain.Abstracts.Graphs.Exceptions;
using NetBrain.Abstracts.Graphs.Models;
using NetBrain.Defaults.Graphs.Common;

namespace NetBrain.Defaults.Graphs.AdjacencyTableBased
{
    public class Graph<T, V> : IGraph<T, V> where V : IComparable<V>
    {
        # region Protected fields

        protected V valueForUnassigned { get; set; }
        protected Func<INode<T>, INode<T>, V, IEdge<T, V>> EdgeFactory { get; set; } 

        #endregion Protected fields

        # region Public properties

        public IList<INode<T>> Nodes { get; set; }
        public V[,] AdjacencyMatrix { get; private set; }
        public bool IsDirected { get; private set; }

        # endregion Public properties

        # region Construction

        public Graph(IList<INode<T>> nodes, V valueForUnassigned = default(V), bool directed = false)
        {
            this.Nodes = nodes;
            this.AdjacencyMatrix = new V[this.Nodes.Count, this.Nodes.Count];
            this.valueForUnassigned = valueForUnassigned;
            this.IsDirected = directed;
            this.EdgeFactory = (from, to, value) => new Edge<T, V>(from, to, value);
            this.FillMatrixWithDefaultValues();
        }

        public Graph(IList<INode<T>> nodes, Func<INode<T>, INode<T>, V, IEdge<T, V>> edgesFactory, V valueForUnassigned = default(V), bool directed = false)
        {
            this.Nodes = nodes;
            this.AdjacencyMatrix = new V[this.Nodes.Count, this.Nodes.Count];
            this.valueForUnassigned = valueForUnassigned;
            this.IsDirected = directed;
            this.EdgeFactory = edgesFactory;
            this.FillMatrixWithDefaultValues();
        }



        protected void FillMatrixWithDefaultValues()
        {
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                for (int j = 0; j < this.Nodes.Count; j++)
                {
                    this.AdjacencyMatrix[i, j] = this.valueForUnassigned;
                }
            }
        }

        # endregion Construction

        public IEnumerable<INode<T>> GetNeighborsOf(INode<T> node)
        {
            return this.IterateOnNeighborsAndGet<INode<T>>(node, (mainNode, neighbor, value) => neighbor);
        }

        public IEnumerable<IEdge<T, V>> GetValuedEdges(INode<T> node)
        {
            return this.IterateOnNeighborsAndGet<IEdge<T, V>>(node, this.EdgeFactory);
        }

        protected IEnumerable<U> IterateOnNeighborsAndGet<U>(INode<T> node, Func<INode<T>, INode<T>, V, U> factory)
        {
            int indexOfNode = this.Nodes.IndexOf(node);
            if (indexOfNode == -1) throw new NodeNotFoundException<T>(node);
            for (int neighborIdx = 0; neighborIdx < this.Nodes.Count; neighborIdx++)
            {
                if (
                    (!this.AdjacencyMatrix[indexOfNode, neighborIdx].Equals(this.valueForUnassigned)) ||
                    (!this.IsDirected && !this.AdjacencyMatrix[neighborIdx, indexOfNode].Equals(this.valueForUnassigned))
                    )
                {
                    yield return factory(node, this.Nodes[neighborIdx], this.AdjacencyMatrix[indexOfNode, neighborIdx]);
                }
            }
        } 

        public virtual void AddEdge(INode<T> from, INode<T> to, V value = default(V))
        {
            int indexFrom = this.Nodes.IndexOf(from);
            if (indexFrom < 0) throw new NodeNotFoundException<T>(from);

            int indexTo = this.Nodes.IndexOf(to);
            if (indexTo < 0) throw new NodeNotFoundException<T>(to);

            this.AdjacencyMatrix[indexFrom, indexTo] = value;
            if (!this.IsDirected) this.AdjacencyMatrix[indexTo, indexFrom] = value;
        }

        public bool HasEdge(INode<T> from, INode<T> to)
        {
            int indexFrom = this.Nodes.IndexOf(from);
            if (indexFrom < 0) throw new NodeNotFoundException<T>(from);

            int indexTo = this.Nodes.IndexOf(to);
            if (indexTo < 0) throw new NodeNotFoundException<T>(to);

            V value = this.AdjacencyMatrix[indexFrom, indexTo];
            if (!value.Equals(this.valueForUnassigned)) return true;
            return false;
        }

        public IEdge<T, V> GetEdge(INode<T> from, INode<T> to)
        {
            if (this.HasEdge(from, to))
            {
                int indexFrom = this.Nodes.IndexOf(from);
                if (indexFrom < 0) throw new NodeNotFoundException<T>(from);

                int indexTo = this.Nodes.IndexOf(to);
                if (indexTo < 0) throw new NodeNotFoundException<T>(to);

                V value = this.AdjacencyMatrix[indexFrom, indexTo];
                if (!value.Equals(this.valueForUnassigned))
                {
                    return new Edge<T, V>(from, to, value);
                }
            }
            return null;
        }

        public void RemoveEdge(INode<T> from, INode<T> to)
        {
            if (this.HasEdge(from, to))
            {
                int indexFrom = this.Nodes.IndexOf(from);
                if (indexFrom < 0) throw new NodeNotFoundException<T>(from);

                int indexTo = this.Nodes.IndexOf(to);
                if (indexTo < 0) throw new NodeNotFoundException<T>(to);

                this.AdjacencyMatrix[indexFrom, indexTo] = this.valueForUnassigned;
            }
        }
    }
}
