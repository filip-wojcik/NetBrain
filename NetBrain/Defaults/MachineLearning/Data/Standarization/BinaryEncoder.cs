using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Defaults.MachineLearning.Data.Standarization
{
    public class BinaryEncoder<T> : BaseEncoder<T>
    {
        public override int EncodedDataCount
        {
            get { return 1; }
        }

        public BinaryEncoder(IEnumerable<T> distinctValues, T defaultValue)
            : base(distinctValues, defaultValue)
        {
            ValidateData();
            this.BuildMappingTable();
        }

        private void ValidateData()
        {
            if (base.DistinctValues.Count() != 2)
            {
                throw new ArgumentException("Too many values for binary encoding. Expected 2 got " + base.DistinctValues.Count());
            }
        }

        private void BuildMappingTable()
        {
            T firstValue = base.DistinctValues.First();
            base.ValuesToArrayMappings.Add(firstValue, new double[] { 1.0 });

            T secondValue = base.DistinctValues.Last();
            base.ValuesToArrayMappings.Add(secondValue, new double[] { -1.0 });
        }
    }
}
