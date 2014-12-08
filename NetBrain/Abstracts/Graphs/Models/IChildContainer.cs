using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.Graphs.Models
{
    /// <summary>
    /// Represents child - value pair in trees
    /// </summary>
    /// <typeparam name="T">First graph type parameter</typeparam>
    /// <typeparam name="V">Graph value parameter</typeparam>
    public interface IChildContainer<T, V>
    {
        ITree<T, V> ChildTree { get; }
        V ChildValue { get; }
    }
}
