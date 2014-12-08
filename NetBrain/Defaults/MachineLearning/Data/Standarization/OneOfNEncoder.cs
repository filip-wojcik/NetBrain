using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using NetBrain.Abstracts.MachineLearning.Models.Data.Standarization;

namespace NetBrain.Defaults.MachineLearning.Data.Standarization
{
    public class OneOfNEncoder<T> : BaseEncoder<T>
    {
        # region Public properties

        public override int EncodedDataCount
        {
            get { return base.DistinctValues.Count() + 1; }
        }

        # endregion Public properties

        # region Constructor

        public OneOfNEncoder(IEnumerable<T> distinctValues, T defaultValue)
            : base(distinctValues, defaultValue)
        {
           this.BuildMappingTable();
        }

        public OneOfNEncoder(T defaultValue) : base(defaultValue)
        {
        }

        # endregion Constructor

        # region Processing methods

        private void BuildMappingTable()
        {
            BuildDefaultValueMapping();

            int currentIdx = 0;
            for (int i = (base.DistinctValues.Count() -1); i >= 0; i--)
            {
                T valueUnderIndex = base.DistinctValues.ElementAt(i);
                var mappingTable = new double[base.DistinctValues.Count() + 1];
                mappingTable[i] = 1.0;
                base.ValuesToArrayMappings.Add(valueUnderIndex, mappingTable);
            }
        }

        private void BuildDefaultValueMapping()
        {
            var defaultValueMapping = new double[base.DistinctValues.Count() + 1];
            defaultValueMapping[defaultValueMapping.Length - 1] = 1.0;
            base.ValuesToArrayMappings.Add(base.DefaultValue, defaultValueMapping);
        }

        # endregion Processing methods
    }
}
