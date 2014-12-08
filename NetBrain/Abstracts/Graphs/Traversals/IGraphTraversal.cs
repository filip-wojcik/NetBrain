using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Graphs.Models;

namespace NetBrain.Abstracts.Graphs.Traversals
{
    public interface IGraphTraversal<T, V> 
    {
        IList<INode<T>> SearchPath(INode<T> startNode, INode<T> targetNode, IGraph<T, V> graph);
    }
}
