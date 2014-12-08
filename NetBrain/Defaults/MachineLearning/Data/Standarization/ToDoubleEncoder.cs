using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data.Standarization;

namespace NetBrain.Defaults.MachineLearning.Data.Standarization
{
    public class ToDoubleEncoder<T> : IEncoder<T>
    {
        public Func<T, double> ToDoubleAdapter { get; private set; }
        public Func<double, T> ToTypeAdapter { get; private set; }

        public ToDoubleEncoder(Func<T, double> toDoubleAdapter, Func<double, T> toTypeAdapter)
        {
            ToDoubleAdapter = toDoubleAdapter;
            ToTypeAdapter = toTypeAdapter;
        }

        public int EncodedDataCount
        {
            get { return 1; }
        }

        public void Prepare(IEnumerable<T> data){ }

        public IList<double> Encode(T data)
        {
            return new[] {this.ToDoubleAdapter(data)};
        }

        public T Decode(IList<double> encodedData)
        {
            return this.ToTypeAdapter(encodedData.First());
        }
    }
}