using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Models.Data.Standarization
{
    public interface IEncoder<T>
    {
        int EncodedDataCount { get; }
        void Prepare(IEnumerable<T> data);
        IList<double> Encode(T data);
        T Decode(IList<double> encodedData);
    }
}
