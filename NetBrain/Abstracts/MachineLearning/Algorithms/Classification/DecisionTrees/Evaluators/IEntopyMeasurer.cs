using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    public interface IEntopyMeasurer<T>
    {
        bool AllValuesAreNumberic { get; set; }
        DiscreteDomainChaosMeasure<T> DiscreteDomainChaosMeasure { get; }
        ContinousValuesChaosMeasurer ContinuousDomainChaosMeasure { get; }

        double MeasureEntropyOnAxis(ISingleValueDataSet<T> dataSet, int axis);
        double MeasureTotalEntropyOfValues(ISingleValueDataSet<T> dataSet);
    }

    public delegate IEntopyMeasurer<T> EntropyMeasurerFactory<T>(
        DiscreteDomainChaosMeasure<T> discreteDomainChaosMeasure,
        ContinousValuesChaosMeasurer continousValuesChaosMeasurer,
        bool allValuesAreNumeric = false);
}
