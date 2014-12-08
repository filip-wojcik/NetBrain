using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.Graphs.Models
{
    public interface IGraph<T, V>
    {
        IList<INode<T>> Nodes { get; }
        void AddEdge(INode<T> from, INode<T> to, V value = default(V));
        IEdge<T, V> GetEdge(INode<T> from, INode<T> to);
        bool HasEdge(INode<T> from, INode<T> to);
        void RemoveEdge(INode<T> from, INode<T> to);
        IEnumerable<INode<T>> GetNeighborsOf(INode<T> node);
        IEnumerable<IEdge<T, V>> GetValuedEdges(INode<T> node);
    }
}
