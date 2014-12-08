
using System;
using System.Linq;
using System.Linq.Expressions;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using System.Collections.Generic;
using NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Defaults.MachineLearning.Data.Models;
using NetBrain.Utils;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Utils;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    public class MultiValueDataSplitter<T> : BaseDataSplitter<T, T>
    {
        protected T FalseQuivalent
        {
            get { return default(T); }
        }

        public MultiValueDataSplitter()
        {
        }

        public MultiValueDataSplitter(NumbericDataMiddlepointFinder numbericDataMiddlepointFinder, TrueEquivalent<T> trueEquivalent, FalseEquivalent<T> falseEquivalent) 
            : base(trueEquivalent, falseEquivalent, numbericDataMiddlepointFinder)
        {
        }

        public override IEnumerable<ISplitOption<T>> GenerateSplitOptionsForAxis(ISingleValueDataSet<T> singleValueDataSet, int axis)
        {
            string featureLabel = singleValueDataSet.Columns[axis];
            if (typeof (T).IsNumeric() || singleValueDataSet.Vectors.First()[axis].IsNumeric())
            {
                yield return base.MiddlePointSplitOption(singleValueDataSet, axis, featureLabel);
            }
            else
            {
                yield return new SplitOption<T>(axis, featureLabel, false, false);
            }
            
        }

        protected override IEnumerable<ISplittingResult<T, T>> SplitValidData(ISingleValueDataSet<T> singleValueDataSet, ISplitOption<T> splitOption)
        {
            base.ValidateData(singleValueDataSet, splitOption);

            var newFeatureSets = new Dictionary<T, IList<ISingleValueFeatureVector<T>>>();

            if (splitOption.IsDataNumberic)
            {
                return base.SplitNumbericData(singleValueDataSet, splitOption).Select(result =>
                {
                    T value = (result.Value == true) ? base.TrueEquivalent() : base.FalseEquivalent();
                    return new SplittingResult<T, T>(value, result.SingleValuesDataSet);
                });
            }
            else
            {
                if (splitOption.SplitOnConcreteValue)
                {
                    foreach (var vector in singleValueDataSet.SingleValueVectors)
                    {
                        T valueUnderIndex = vector[splitOption.SplitAxis];
                        if (valueUnderIndex.Equals(splitOption.ConcreteValueToSplit))
                        {
                            base.AddValue(newFeatureSets, valueUnderIndex, vector);
                        }
                        else
                        {
                            //Simulates binary split behavior - value false
                            base.AddValue(newFeatureSets, FalseQuivalent, vector);
                        }
                    }
                }
                else
                {
                    foreach (var vector in singleValueDataSet.SingleValueVectors)
                    {
                        T valueUnderIndex = vector[splitOption.SplitAxis];
                        base.AddValue(newFeatureSets, valueUnderIndex, vector);
                    }
                }

            }
            return base.BuildResults(singleValueDataSet, newFeatureSets);
        }
    }
}
