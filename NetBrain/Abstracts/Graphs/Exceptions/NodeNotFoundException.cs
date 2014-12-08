using System;
using NetBrain.Abstracts.Graphs.Models;


namespace NetBrain.Abstracts.Graphs.Exceptions
{
    public class NodeNotFoundException<T> : Exception
    {
        private static string MESSAGE = "Node not found! {0}";

        public NodeNotFoundException(INode<T> node)
            : base(string.Format(MESSAGE, node.ToString()))
        {   
        }
    }
}
