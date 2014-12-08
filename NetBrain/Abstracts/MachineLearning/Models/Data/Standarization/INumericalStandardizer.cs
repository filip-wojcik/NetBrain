using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Models.Data.Standarization
{
    public interface INumericalStandardizer<T> : IEncoder<T>
    {
        double Normalize(T input);
        double Denormalize(double inputData);
    }
}
