using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Graphs.Models;

namespace NetBrain.Abstracts.Graphs.Traversals
{
    public interface ITreeTraversal<T, V> : IGraphTraversal<T, V> 
    {
        IList<ITree<T, V>> SearchPath(ITree<T, V> startNode, ITree<T, V> targetNode);
    }
}
