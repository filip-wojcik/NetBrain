using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Utils;
using NetBrain.Abstracts.MachineLearning.Exceptions;
using NetBrain.Abstracts.MachineLearning.MathematicalFunctions;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Defaults.MachineLearning.Data.Models;
using NetBrain.Utils;

namespace NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    public abstract class BaseDataSplitter<T, V> : IDataSplitter<T, V>
    {
        # region Public properties

        public NumbericDataMiddlepointFinder NumbericDataMiddlepointFinder { get; private set; }
        public TrueEquivalent<V> TrueEquivalent { get; private set; }
        public FalseEquivalent<V> FalseEquivalent { get; private set; } 

        # endregion Public properties

        # region Construction

        protected BaseDataSplitter()
        {
        }

        protected BaseDataSplitter(TrueEquivalent<V> trueEquivalent, FalseEquivalent<V> falseEquivalent, NumbericDataMiddlepointFinder numbericDataMiddlepointFinder = null)
        {
            NumbericDataMiddlepointFinder = numbericDataMiddlepointFinder ?? StatisticalFunctions.Mean;
            TrueEquivalent = trueEquivalent;
            FalseEquivalent = falseEquivalent;
        }

        # endregion Construction

        # region Processing methods

        public virtual IEnumerable<ISplittingResult<T, V>> SplitFeatureVectors(ISingleValueDataSet<T> singleValueDataSet,
            ISplitOption<T> splitOption)
        {
            this.ValidateData(singleValueDataSet, splitOption);
            return this.SplitValidData(singleValueDataSet, splitOption);
        }

        public abstract IEnumerable<ISplitOption<T>> GenerateSplitOptionsForAxis(ISingleValueDataSet<T> singleValueDataSet,
            int axis);

        public virtual IEnumerable<ISplittingResult<T, bool>> SplitNumbericData(ISingleValueDataSet<T> singleValueDataSet, ISplitOption<T> splitOption)
        {
            int axis = splitOption.SplitAxis;

            this.ValidateAxis(singleValueDataSet, axis);
            this.ValidateFeatureVectors(singleValueDataSet);

            if (this.CheckNumericallity(singleValueDataSet, axis))
            {
                double splittingPoint = (splitOption.SplitOnConcreteValue == true)
                    ? splitOption.ConcreteNumbericValueToSplit
                    : this.FindMiddlePoint(singleValueDataSet, axis);
                var splittedDataSets = new Dictionary<bool, IList<ISingleValueFeatureVector<T>>>();
                foreach (var vector in singleValueDataSet.SingleValueVectors)
                {
                    var valueOfVector = Convert.ToDouble(vector[axis]);
                    var vectorValueIsBigger = valueOfVector >= splittingPoint;
                    this.AddKeyIfNotExists(splittedDataSets, vectorValueIsBigger);
                    splittedDataSets[vectorValueIsBigger].Add(vector);
                }
                return this.BuildResults(singleValueDataSet, splittedDataSets);
            }
            else
            {
                throw new ArgumentException("Passed data is not numberical!");
            }
            return null;
        }

        # endregion Processing methods

        # region Protected methods

        protected abstract IEnumerable<ISplittingResult<T, V>> SplitValidData(ISingleValueDataSet<T> singleValueDataSet,
            ISplitOption<T> splitOption);

        protected void AddValue(Dictionary<V, IList<ISingleValueFeatureVector<T>>> newFeatureSets, V valueToBeAdded, ISingleValueFeatureVector<T> vector)
        {
            AddKeyIfNotExists(newFeatureSets, valueToBeAdded);
            newFeatureSets[valueToBeAdded].Add(vector);
        }

        protected void AddKeyIfNotExists<W>(Dictionary<W, IList<ISingleValueFeatureVector<T>>> newFeatureSets, W valueUnderIndex)
        {
            if (!newFeatureSets.ContainsKey(valueUnderIndex))
            {
                newFeatureSets.Add(valueUnderIndex, new List<ISingleValueFeatureVector<T>>());
            }
        }

        protected virtual void ValidateData(ISingleValueDataSet<T> singleValueDataSet, ISplitOption<T> splitOption)
        {
            ValidateFeatureVectors(singleValueDataSet);
            ValidateAxis(singleValueDataSet, splitOption.SplitAxis);
        }

        protected virtual IEnumerable<ISplittingResult<T, W>> BuildResults<W>(
            ISingleValueDataSet<T> baseFeturesDataSet,
            IDictionary<W, IList<ISingleValueFeatureVector<T>>> gatheredvectors
            )
        {
            foreach (var valueAndVectors in gatheredvectors)
            {
                var newFeaturesVectorSet = new SingleValueDataSet<T>(baseFeturesDataSet.Columns,
                    baseFeturesDataSet.SingleVectorSize, baseFeturesDataSet.ValueIndex, valueAndVectors.Value);
                yield return new SplittingResult<T, W>(valueAndVectors.Key, newFeaturesVectorSet);
            }
        }

        protected virtual void ValidateFeatureVectors(ISingleValueDataSet<T> singleValueDataSet)
        {
            if (!singleValueDataSet.Vectors.Any()) throw new EmptyFeatureVectorsSetException();
        }

        protected virtual void ValidateAxis(ISingleValueDataSet<T> singleValueData, int axis)
        {
            if(axis < 0 || axis > singleValueData.SingleVectorSize) throw new ArgumentOutOfRangeException();
        }

        protected bool CheckNumericallity(ISingleValueDataSet<T> singleValueDataSet, int axis)
        {
            if (typeof(T).IsNumeric() || singleValueDataSet.Vectors.First()[axis].IsNumeric())
            {
                return true;
            }
            return false;
        }

        protected double FindMiddlePoint(ISingleValueDataSet<T> singleValueDataSet, int axis)
        {
            return this.NumbericDataMiddlepointFinder(singleValueDataSet.ValuesInColumn(axis)
                .Select(val => Convert.ToDouble(val)));
        }

        protected ISplitOption<T> MiddlePointSplitOption(ISingleValueDataSet<T> singleValueDataSet, int axis, string featureLabel)
        {
            double middlePoint = this.FindMiddlePoint(singleValueDataSet, axis);
            return new SplitOption<T>(axis, featureLabel, true, true, true, concreteNumbericValueToSplit: middlePoint);
        }


        # endregion Protected methods
    }
}
