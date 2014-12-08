namespace NetBrain.Defaults.MachineLearning.Data.Models
{
    using Abstracts.MachineLearning.Models.Data;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class DataSet<T> : IDataSet<T>
    {
        # region Consts

        private readonly string INVALID_VECTOR_MESSAGE = "Selected vector is invalid!";

        # endregion Consts

        # region Private fields

        private readonly object _locker = new object();

        # endregion Private fields

        # region Public properties

        T IDataSet<T>.this[int rowIndex, int columnIdx]
        {
            get { return this.Vectors[rowIndex][columnIdx]; }
        }

        public T this[int rowIndex, string columnName]
        {
            get
            {
                int columnIndx = this.GetColumnIndex(columnName);
                return (this as IDataSet<T>)[rowIndex, columnIndx];
            }
        }

        public int SingleVectorSize { get; private set; }

        public IReadOnlyList<int> ValueColumnsIndexes { get; private set; }

        public IList<string> Columns { get; private set; }

        public IList<IFeatureVector<T>> Vectors { get; private set; }

        public virtual IList<int> NonValueColumnIndexes
        {
            get
            {
                var nonValuesIndexes = new List<int>();
                if (this.ValueColumnsIndexes.Any())
                {
                    for (int i = 0; i < this.SingleVectorSize; i++)
                    {
                        if(!this.ValueColumnsIndexes.Contains(i)) nonValuesIndexes.Add(i);
                    }
                }
                return nonValuesIndexes;
            }
        }

        public virtual bool HasValues
        {
            get { return this.ValueColumnsIndexes != null && this.ValueColumnsIndexes.Any(); }
        }

        public virtual IDataSet<T> ValuesVectorsSet
        {
            get
            {
                var valuesVector = this.Vectors.Select(vector => vector.ValuesVector);
                return new DataSet<T>(
                    this.Columns.Where((label, idx) => this.ValueColumnsIndexes.Contains(idx)).ToList(),
                    this.ValueColumnsIndexes.Count,
                    null,
                    valuesVector
                    );
            }
        }

        public virtual IDataSet<T> NonValueVectorsSet
        {
            get
            {
                var nonValuesVectors = this.Vectors.Select(vector => vector.NonValuesVector);
                return new DataSet<T>(
                    this.Columns.Where((label, idx) => !this.ValueColumnsIndexes.Contains(idx)).ToList(),
                    (this.SingleVectorSize - this.ValueColumnsIndexes.Count),
                    null,
                    nonValuesVectors
                    );
            }
        }

        public virtual IDataSet<ILabeledFeature<T>> LabeledDataSet
        {
            get
            {
                var valueIndexes = this.ValueColumnsIndexes.ToList();
                var valuesVectors = this.Vectors.Select(this.ToLabeledFeatureVector);
                return new DataSet<ILabeledFeature<T>>(
                    this.Columns,
                    (this.SingleVectorSize - this.ValueColumnsIndexes.Count),
                    valueIndexes,
                    valuesVectors
                    );
            }
        }

        # endregion Public properties

        # region Constructor

        public DataSet(IList<string> featureLabels, int singleVectorSize, IList<int> valueIndexes, IEnumerable<IFeatureVector<T>> featureVectors = null)
        {
            this.SingleVectorSize = singleVectorSize;
            this.Columns = new List<string>(featureLabels);
            this.ValueColumnsIndexes = (valueIndexes == null) ? new ReadOnlyCollection<int>(new List<int>()) : new ReadOnlyCollection<int>(valueIndexes);
            this.Vectors = new List<IFeatureVector<T>>();
            if (featureVectors != null)
            {
                this.AddVectors(featureVectors);
            }
        }

        # endregion Constructor

        # region Setup methods


        # endregion Setup methods

        # region Processing methods

        protected virtual void AddVectors(IEnumerable<IFeatureVector<T>> vectors)
        {
            foreach (var vector in vectors)
            {
                this.AddVector(vector);
            }
        }

        public virtual void AddVector(IFeatureVector<T> newVector)
        {
            if (VectorIsValid(newVector))
            {
                if (newVector.HasValues && newVector.ValueIndexes.SequenceEqual(this.ValueColumnsIndexes))
                {
                    this.Vectors.Add(newVector);
                }
                else
                {
                    this.Vectors.Add(new FeatureVector<T>(newVector.Features.ToList(),
                        this.ValueColumnsIndexes.ToList()));
                }

            }
        }

        protected virtual bool VectorIsValid(IFeatureVector<T> newVector)
        {
            return newVector != null && newVector.Count().Equals(this.SingleVectorSize);
        }

        public IEnumerable<T> ValuesInColumn(string columnName)
        {
            int columnIndex = this.GetColumnIndex(columnName);
            return this.ValuesInColumn(columnIndex);
        }

        public virtual IFeatureVector<ILabeledFeature<T>> ToLabeledFeatureVector(IFeatureVector<T> vector)
        {
            if (this.VectorIsValid(vector))
            {
                var labeledFeatures =
                    vector.Features.Select((feature, idx) => new LabeledFeature<T>(feature, this.Columns[idx]) as ILabeledFeature<T>)
                        .ToList();
                return new FeatureVector<ILabeledFeature<T>>(labeledFeatures, vector.ValueIndexes.ToList());
            }
            else
            {
                throw new InvalidVectorException<T>(INVALID_VECTOR_MESSAGE);
            }
        }

        public virtual IFeatureVector<ILabeledFeature<T>> ToLabeledFeatureVector(int vectorIdx)
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

        public IEnumerable<T> UniqValuesInColumn(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex > this.SingleVectorSize) throw new IndexOutOfRangeException();
            var data = new HashSet<T>();
            foreach (var vector in this.Vectors)
            {
                data.Add(vector[columnIndex]);
            }
            return data;
        }

        public IEnumerable<T> UniqValuesInColumn(string columnName)
        {
            int columnIdx = this.Columns.IndexOf(columnName);
            return this.UniqValuesInColumn(columnIdx);
        }

        public IEnumerable<T> ValuesInColumn(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex > this.SingleVectorSize) throw new IndexOutOfRangeException();
            foreach (var vector in this.Vectors)
            {
                yield return vector[columnIndex];
            }
        }

        # endregion Processing methods

        # region Helper methods

        protected int GetColumnIndex(string columnName)
        {
            int columnIndex = this.Columns.IndexOf(columnName);
            if (columnIndex < 0)
            {
                throw new ArgumentException(string.Format("Unknown column name: {0}", columnName));
            }
            return columnIndex;
        }

        # endregion Helper methods

        # region Enumeration methods

        public IEnumerator<IFeatureVector<T>> GetEnumerator()
        {
            return this.Vectors.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Vectors.AsEnumerable().GetEnumerator();
        }

        # endregion Enumeration methods

        # region List mehtods

        public void Add(IFeatureVector<T> item)
        {
            this.AddVector(item);
        }

        public void Clear()
        {
            lock (this._locker)
            {
                this.Vectors.Clear();
            }
        }

        public bool Contains(IFeatureVector<T> item)
        {
            return this.Vectors.Any(vec => vec.Equals(item));
        }

        public void CopyTo(IFeatureVector<T>[] array, int arrayIndex)
        {
            int vectorsOffset = 0;
            for (int i = arrayIndex; i < array.Length; i++)
            {
                array[i] = this.Vectors[vectorsOffset];
                vectorsOffset++;
            }
        }

        public bool Remove(IFeatureVector<T> item)
        {
            int index = this.Vectors.IndexOf(item);
            if (index >= 0)
            {
                this.Vectors.RemoveAt(index);
                return true;
            }
            return false;
        }

        public int Count { get { return this.Vectors.Count; } }
        public bool IsReadOnly 
        {
            get { return false; }
        }

        public int IndexOf(IFeatureVector<T> item)
        {
            return this.Vectors.IndexOf(item);
        }

        public void Insert(int index, IFeatureVector<T> item)
        {
            this.Vectors.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.Vectors.RemoveAt(index);
        }

        public IFeatureVector<T> this[int index]
        {
            get { return this.Vectors[index]; }
            set { this.Vectors[index] = value; }
        }

        # endregion List mehtods
    }
}
