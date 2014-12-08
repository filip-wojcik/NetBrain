using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.Graphs.Models
{
    public interface INode<T>
    {
        T Value { get; set; }
    }
}
