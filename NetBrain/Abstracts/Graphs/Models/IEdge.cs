using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.Graphs.Models
{
    public interface IEdge<T, V>
    {
        INode<T> NodeFrom { get; }
        INode<T> NodeTo { get; }
        V Value { get; }
    }
}
