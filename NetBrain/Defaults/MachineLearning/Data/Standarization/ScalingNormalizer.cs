using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data.Standarization;

namespace NetBrain.Defaults.MachineLearning.Data.Standarization
{
    public class ScalingNormalizer<T> : INumericalStandardizer<T>
    {
        private double _min, _range;
        private readonly Func<double, T> _mappingFunc;

        public int EncodedDataCount
        {
            get { return 1; }
        }

        public ScalingNormalizer(Func<double, T> mappingFunc, double min, double max)
        {
            _range = max - min;
            _min = min;
            _mappingFunc = _mappingFunc;
        }

        public ScalingNormalizer(Func<double, T> mappingFunc, IEnumerable<double> data)
        {
            _mappingFunc = mappingFunc;
            this.Prepare(data);
        }

        public void Prepare(IEnumerable<T> data)
        {
           this.Prepare(data.Cast<double>());
        }

        protected void Prepare(IEnumerable<double> numericData)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            foreach (var elem in numericData)
            {
                if (elem < min) min = elem;
                if (elem > max) max = elem;
            }
            this._range = max - min;
            this._min = min;
        }

        public double Normalize(T input)
        {
            double numericInput = Convert.ToDouble(input);
            return (numericInput - this._min)/(double)this._range;
        }

        public double Denormalize(double inputData)
        {
            return inputData*this._range + this._min;
        }

        public IList<double> Encode(T data)
        {
            return new double[]{ this.Normalize(data) };
        }

        public T Decode(IList<double> encodedData)
        {
            return this._mappingFunc(this.Denormalize(encodedData.First()));
        }
    }
}
