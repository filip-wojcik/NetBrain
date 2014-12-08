using System;
using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Utils;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Utils;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    public class BinaryDataSplitter<T> : BaseDataSplitter<T, bool>
    {
        private static string MESSAGE = "Concrete value to split is not set";

        public BinaryDataSplitter(NumbericDataMiddlepointFinder numbericDataMiddlepointFinder, TrueEquivalent<bool> trueEquivalent, FalseEquivalent<bool> falseEquivalent) 
            : base(trueEquivalent, falseEquivalent, numbericDataMiddlepointFinder)
        {
        }

        public BinaryDataSplitter()
        {
        }

        protected override IEnumerable<ISplittingResult<T, bool>> SplitValidData(ISingleValueDataSet<T> singleValueDataSet, ISplitOption<T> splitOption)
        {
            var splittedDataSets = new Dictionary<bool, IList<ISingleValueFeatureVector<T>>>();
            T desiredValue = splitOption.ConcreteValueToSplit;
            if (base.CheckNumericallity(singleValueDataSet, splitOption.SplitAxis))
            {
                return base.SplitNumbericData(singleValueDataSet, splitOption);
            }
            
            foreach (var vector in singleValueDataSet)
            {
                var valueEqualsDesired = vector[splitOption.SplitAxis].Equals(splitOption.ConcreteValueToSplit);
                base.AddKeyIfNotExists(splittedDataSets, valueEqualsDesired);
                splittedDataSets[valueEqualsDesired].Add(vector as ISingleValueFeatureVector<T>);
            }

            return base.BuildResults(singleValueDataSet, splittedDataSets);
        }

        public override IEnumerable<ISplitOption<T>> GenerateSplitOptionsForAxis(ISingleValueDataSet<T> singleValueDataSet, int axis)
        {
            string featureLabel = singleValueDataSet.Columns[axis];
            bool isDataNumberic = base.CheckNumericallity(singleValueDataSet, axis);
            if (base.CheckNumericallity(singleValueDataSet, axis))
            {
                yield return base.MiddlePointSplitOption(singleValueDataSet, axis, featureLabel);
            }
            else
            {
                foreach (var value in singleValueDataSet.UniqValuesInColumn(axis))
                {
                    yield return new SplitOption<T>(axis, featureLabel, false, true, true, concreteValueToSplit: value);
                }
            }
        }

        protected override void ValidateData(ISingleValueDataSet<T> singleValueDataSet, ISplitOption<T> splitOption)
        {
            base.ValidateData(singleValueDataSet, splitOption);
            if (splitOption.SplitOnConcreteValue)
            {
                if (!splitOption.IsDataNumberic && splitOption.ConcreteValueToSplit == null)
                {
                    throw new ArgumentNullException(MESSAGE);
                }
            }
        }
    }
}
