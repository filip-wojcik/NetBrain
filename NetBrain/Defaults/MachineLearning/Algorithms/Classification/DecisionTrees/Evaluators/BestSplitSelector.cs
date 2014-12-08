using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Utils;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    public class BestSplitSelector<T, V> : IBestSplitSelector<T, V>
    {
        # region Public properties

        public IEntopyMeasurer<T> EntropyMeasurer { get; set; }
        public bool UseAverageMeasureToSplitNumbers { get; set; }

        # endregion Public properties

        # region Static factory

        public static IBestSplitSelector<T, V> StandardBestSplitSelectorFactory(
            IEntopyMeasurer<T> entropyMeasurer,
            bool useAverageMeasureToSplitNumbers
            )
        {
            return new BestSplitSelector<T, V>(entropyMeasurer, useAverageMeasureToSplitNumbers);
        }

        # endregion Static factory

        # region Construction

        public BestSplitSelector()
        {
        }

        public BestSplitSelector(IEntopyMeasurer<T> entropyMeasurer, bool useAverageMeasureToSplitNumbers)
        {
            EntropyMeasurer = entropyMeasurer;
            UseAverageMeasureToSplitNumbers = useAverageMeasureToSplitNumbers;
        }

        # endregion Construction

        # region Processing methods

        public ISplitOption<T> ChooseBestSplitOption(ISingleValueDataSet<T> singleValueDataSet, IDataSplitter<T, V> dataSplitter)
        {
            var applicableSplitAxes = this.FindApplicableSplitAxes(singleValueDataSet);
            if(!applicableSplitAxes.Any()) return new SplitOption<T>(-1, null, false, false);
            ISplitOption<T> bestSplitOption = null;
            int totalVectorsCount = singleValueDataSet.Count();
            double initialEntopy = this.MeasureEntropy(singleValueDataSet);
            double bestEntopyGain = double.MinValue;
            foreach (var applicableAxis in applicableSplitAxes)
            {
                foreach (var splitOption in dataSplitter.GenerateSplitOptionsForAxis(singleValueDataSet, applicableAxis))
                {
                    double splitEntopy = 0;
                    ISplitOption<T> axisSplitOption = null;

                    if (splitOption.IsDataNumberic && this.UseAverageMeasureToSplitNumbers)
                    {
                        IEnumerable<ISplittingResult<T, bool>> splittedData =
                            dataSplitter.SplitNumbericData(singleValueDataSet, splitOption);
                        foreach (var splittedSet in splittedData) splitEntopy += this.MeasureEntropy(splittedSet.SingleValuesDataSet);
                    }
                    else
                    {
                        IEnumerable<ISplittingResult<T, V>> splittedData = dataSplitter.SplitFeatureVectors(singleValueDataSet,
                            splitOption);
                        foreach (var splittedSet in splittedData)
                        {
                            double entr = (this.MeasureEntropy(splittedSet.SingleValuesDataSet));
                            double prob = (splittedSet.SingleValuesDataSet.Count() / (double)totalVectorsCount);
                            splitEntopy += (this.MeasureEntropy(splittedSet.SingleValuesDataSet) *
                (splittedSet.SingleValuesDataSet.Count() / (double)totalVectorsCount));
                        }

                    }

                    double entopyGain = initialEntopy - splitEntopy;
                    if (entopyGain > bestEntopyGain)
                    {
                        bestEntopyGain = entopyGain;
                        bestSplitOption = splitOption;
                    }
                }
            }
            return bestSplitOption;
        }

        public IList<int> FindApplicableSplitAxes(ISingleValueDataSet<T> singleValuesDataSet)
        {
            var applicableIndexes = new List<int>();
            foreach (var nonValueIndex in singleValuesDataSet.NonValueColumnIndexes)
            {
                if (singleValuesDataSet.UniqValuesInColumn(nonValueIndex).Count() > 1)
                {
                    applicableIndexes.Add(nonValueIndex);
                }
            }
            return applicableIndexes;
        }

        public double MeasureEntropy(ISingleValueDataSet<T> singleValueDataSet)
        {
            if (typeof(T).IsNumeric() || singleValueDataSet.First()[singleValueDataSet.ValueIndex].IsNumeric())
            {
                return this.EntropyMeasurer.ContinuousDomainChaosMeasure(singleValueDataSet.Values.Select(val => Convert.ToDouble(val)));
            }
            else
            {
                return this.EntropyMeasurer.DiscreteDomainChaosMeasure(singleValueDataSet, singleValueDataSet.ValueIndex);
            }
        }

        # endregion Processing methods
    }
}
