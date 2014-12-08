using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Utils
{
    /// <summary>
    /// Generics - based interface
    /// </summary>
    /// <typeparam name="T">Type to be cloned</typeparam>
    public interface ICloneableType<out T>
    {
        T Clone();
    }
}
