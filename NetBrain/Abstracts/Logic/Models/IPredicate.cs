using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.Logic.Models
{
    public interface IPredicate<V> : ISentence<V>, IEquatable<IPredicate<V>>
    {
    }
}
