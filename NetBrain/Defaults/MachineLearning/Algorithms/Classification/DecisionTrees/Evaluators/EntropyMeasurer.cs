using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Utils;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    public class EntropyMeasurer<T> : IEntopyMeasurer<T>
    {
        #region IEntopyMeasurer<T,V> Members

        public bool AllValuesAreNumberic { get; set; }

        public DiscreteDomainChaosMeasure<T> DiscreteDomainChaosMeasure { get; private set; }
        public ContinousValuesChaosMeasurer ContinuousDomainChaosMeasure { get; private set; }

        #endregion

        # region Static factory

        public static IEntopyMeasurer<T> StandardEntropyMeasurerFactory(
            DiscreteDomainChaosMeasure<T> discreteDomainChaosMeasure,
            ContinousValuesChaosMeasurer continousValuesChaosMeasurer,
            bool allValuesAreNumeric = false)
        {
            return new EntropyMeasurer<T>(discreteDomainChaosMeasure, continousValuesChaosMeasurer, allValuesAreNumeric);
        }

        # endregion Static factory

        # region Construction

        public EntropyMeasurer(DiscreteDomainChaosMeasure<T> discreteDomainChaosMeasure, ContinousValuesChaosMeasurer continuousDomainChaosMeasure, bool allValuesAreNumberic = false)
        {
            DiscreteDomainChaosMeasure = discreteDomainChaosMeasure;
            ContinuousDomainChaosMeasure = continuousDomainChaosMeasure;
            AllValuesAreNumberic = (typeof(T).IsNumericType() == true) || allValuesAreNumberic;
        }

        # endregion Construction

        #region IEntopyMeasurer<T> Members

        public double MeasureEntropyOnAxis(ISingleValueDataSet<T> dataSet, int axis)
        {
            if (this.AllValuesAreNumberic || dataSet.First()[axis].IsNumericType())
            {
                return this.ContinuousDomainChaosMeasure(dataSet.ValuesInColumn(axis).Select(val => Convert.ToDouble(val)));
            }
            else
            {
                return this.DiscreteDomainChaosMeasure(dataSet, axis);
            }
        }

        public double MeasureTotalEntropyOfValues(ISingleValueDataSet<T> dataSet)
        {
            double totalEntropy = 0;
            if(dataSet.HasValues) totalEntropy += this.MeasureEntropyOnAxis(dataSet, dataSet.ValueIndex);
            return totalEntropy;
        }

        #endregion
        
    }
}
