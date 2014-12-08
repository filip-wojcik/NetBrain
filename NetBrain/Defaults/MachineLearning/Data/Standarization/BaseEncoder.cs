using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data.Standarization;

namespace NetBrain.Defaults.MachineLearning.Data.Standarization
{
    public abstract class BaseEncoder<T> : IEncoder<T>
    {
        # region Protected fileds

        protected IEnumerable<T> DistinctValues;
        protected readonly T DefaultValue;
        protected IDictionary<T, IList<double>> ValuesToArrayMappings;

        # endregion Protected fileds

        # region Public properties

        public abstract int EncodedDataCount { get; }

        # endregion Public properties

        # region Construction

        protected BaseEncoder(T defaultValue)
        {
            DefaultValue = defaultValue;
        }

        protected BaseEncoder(IEnumerable<T> distinctValues, T defaultValue)
        {
            DistinctValues = distinctValues;
            ValuesToArrayMappings = new Dictionary<T, IList<double>>();
            DefaultValue = defaultValue;
        }

        public void Prepare(IEnumerable<T> data)
        {
            this.DistinctValues = new List<T>(data.Distinct());
        }

        # endregion Construction

        # region Processing methods

        public virtual IList<double> Encode(T data)
        {
            IList<double> encodedData;
            if (this.ValuesToArrayMappings.TryGetValue(data, out encodedData))
            {
                return encodedData;
            }
            else
            {
                return this.ValuesToArrayMappings[this.DefaultValue];
            }
        }

        public virtual T Decode(IList<double> encodedData)
        {
            foreach (var kvp in this.ValuesToArrayMappings)
            {
                if (kvp.Value.SequenceEqual(encodedData)) return kvp.Key;
            }
            return this.DefaultValue;
        }

        # endregion Processing methods
    }
}
