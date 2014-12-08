namespace NetBrain.Defaults.MachineLearning.Data.Models
{
    using Abstracts.MachineLearning.Models.Data;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SingleValueDataSet<T> : DataSet<T>, ISingleValueDataSet<T>
    {
        # region Consts

        private readonly string INVALID_VECTOR_MESSAGE = "Selected vector is invalid!";

        # endregion Consts

        # region Public properties

        public int ValueIndex { get; private set; }

        public IEnumerable<T> Values
        {
            get 
            {
                return this.Vectors.Select(vector => vector[this.ValueIndex]);
            }
        }

        public IEnumerable<T> UniqueValues
        {
            get
            {
                return this.Values.Distinct();
            }
        }

        public override IList<int> NonValueColumnIndexes
        {
            get
            {
                var nonValuesIndexes = new List<int>();
                for (int i = 0; i < this.SingleVectorSize; i++)
                {
                    if(this.HasValue && this.ValueIndex == i) continue;
                    else nonValuesIndexes.Add(i);
                }
                return nonValuesIndexes;
            }
        }

        public bool HasValue
        {
            get { return this.ValueIndex != -1; }
        }

        public override IDataSet<T> NonValueVectorsSet
        {
            get
            {
                var nonValuesVectors = this.Vectors.Select(vector => (vector as ISingleValueFeatureVector<T>).NonValuesVector);
                return new SingleValueDataSet<T>(
                    this.Columns.Where((label, idx) => idx != this.ValueIndex).ToList(),
                    (this.SingleVectorSize - 1),
                    -1,
                    nonValuesVectors
                    );
            }
        }

        public override IDataSet<ILabeledFeature<T>> LabeledDataSet
        {
             get
            {
                var valuesVector = this.Vectors.Select(this.ToLabeledFeatureVector);
                return new SingleValueDataSet<ILabeledFeature<T>>(
                    this.Columns,
                    this.SingleVectorSize,
                    this.ValueIndex,
                    valuesVector
                    );
            }
        }

        public override bool HasValues
        {
            get { return this.HasValue; }
        }

        public override IDataSet<T> ValuesVectorsSet
        {
            get
            {
                var values = this.UniqueValues.Select(value => new SingleValueFeatureVector<T>(new T[]{ value }));
                return new SingleValueDataSet<T>(new string[] {this.Columns[this.ValueIndex]}, 1, -1, values);
            }
        }

        public IEnumerable<ISingleValueFeatureVector<T>> SingleValueVectors
        {
            get { return base.Vectors.Cast<SingleValueFeatureVector<T>>(); }
        }

        # endregion Public properties

        # region Constructor

        public SingleValueDataSet(IList<string> featureLabels, int singleVectorSize, int valueIndex, IEnumerable<IFeatureVector<T>> featureVectors = null)
            : base(featureLabels, singleVectorSize, new []{ valueIndex }, null)
        {
            this.ValueIndex = valueIndex;
            if (featureVectors != null)
            {
                base.AddVectors(featureVectors);
            }
        }

        # endregion Constructor

        # region Setup methods

        # endregion Setup methods

        public override void AddVector(IFeatureVector<T> newVector)
        {
            if (base.VectorIsValid(newVector))
            {
                base.Vectors.Add(new SingleValueFeatureVector<T>(newVector.Features.ToList(), this.ValueIndex));
            }
        }

        public override IFeatureVector<ILabeledFeature<T>> ToLabeledFeatureVector(IFeatureVector<T> vector)
        {
            if (this.VectorIsValid(vector) && vector is ISingleValueFeatureVector<T>)
            {
                var labeledFeatures =
                    vector.Features.Select((feature, idx) => new LabeledFeature<T>(feature, this.Columns[idx]) as ILabeledFeature<T>)
                        .ToList();
                return new SingleValueFeatureVector<ILabeledFeature<T>>(labeledFeatures, (vector as ISingleValueFeatureVector<T>).ValueIndex);
            }
            else
            {
                throw new InvalidVectorException<T>(INVALID_VECTOR_MESSAGE);
            }
        }

        public override IFeatureVector<ILabeledFeature<T>> ToLabeledFeatureVector(int vectorIdx)
        {
            if (vectorIdx >= 0 && vectorIdx <= this.Vectors.Count)
            {
                return this.ToLabeledFeatureVector(this.Vectors.ElementAt(vectorIdx));
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public new System.Collections.IEnumerator GetEnumerator()
        {
            return base.Vectors.GetEnumerator();
        }
    }
}
