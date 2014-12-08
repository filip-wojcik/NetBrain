
namespace NetBrain.Abstracts.MachineLearning.Models.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// Basic inteface for data processing in a library. It mimics a structure of data frame in R or Pandas, but
    /// (becase library is not intended for live analysis) is more static. Structure of data set can be imagined as
    /// excel sheet or table in database
    /// </summary>
    /// <typeparam name="T">Type parameter stored in data set</typeparam>
    public interface IDataSet<T> : IList<IFeatureVector<T>>
    {
        /// <summary>
        /// Allows to get data from column of specific column and row on specific index
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <param name="columnName">Column columnName</param>
        /// <returns>Value in specified cell</returns>
        T this[int rowIndex, string columnName] { get; }

        /// <summary>
        /// Allows to get data from column of specific column and row on specific index
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <param name="columnIdx">Column index</param>
        /// <returns>Value in specified cell</returns>
        T this[int rowIndex, int columnIdx] { get; }

        /// <summary>
        /// Gets size of single data vector
        /// </summary>
        int SingleVectorSize { get; }

        /// <summary>
        /// Checks if data set contains values columns
        /// </summary>
        bool HasValues { get; }

        /// <summary>
        /// Columns names
        /// </summary>
        IList<string> Columns { get; }

        /// <summary>
        /// List of value column indexes
        /// </summary>
        IReadOnlyList<int> ValueColumnsIndexes { get; }

        /// <summary>
        /// List of non-value column indexes
        /// </summary>
        IList<int> NonValueColumnIndexes { get; }

        /// <summary>
        /// Vectors stored in a data set
        /// </summary>
        IList<IFeatureVector<T>> Vectors { get; }

        /// <summary>
        /// Returs new data set with all vectors containing labels of columns.
        /// This allows to detach vectors from data set without loosing information
        /// about which attriubute is in specific column
        /// </summary>
        IDataSet<ILabeledFeature<T>> LabeledDataSet { get; }

        /// <summary>
        /// Creates new Data Set without values columns
        /// </summary>
        IDataSet<T> NonValueVectorsSet { get; }

        /// <summary>
        /// Creates new Data Set from only value columns
        /// </summary>
        IDataSet<T> ValuesVectorsSet { get; }

        /// <summary>
        /// Adds new vector to set
        /// </summary>
        /// <param name="newVector">New vector to be added</param>
        void AddVector(IFeatureVector<T> newVector);

        /// <summary>
        /// Gets unique values under column index
        /// </summary>
        /// <param name="columnIndex">Index of column to be checked</param>
        /// <returns>List of values</returns>
        IEnumerable<T> UniqValuesInColumn(int columnIndex);

        /// <summary>
        /// Gets unique values in column
        /// </summary>
        /// <param name="columnName">Name of the column to be checked</param>
        /// <returns>List of values</returns>
        IEnumerable<T> UniqValuesInColumn(string columnName);

        /// <summary>
        /// Gets all values under column index
        /// </summary>
        /// <param name="columnIndex">Index of column to be checked</param>
        /// <returns>List of all values</returns>
        IEnumerable<T> ValuesInColumn(int columnIndex);

        /// <summary>
        /// Gets all values under column
        /// </summary>
        /// <param name="columnName">Name of column to be checked</param>
        /// <returns>List of all values</returns>
        IEnumerable<T> ValuesInColumn(string columnName);

        /// <summary>
        /// Labels vector with column names
        /// </summary>
        /// <param name="vector">Vector to be labelled</param>
        /// <returns>Labelled vector</returns>
        IFeatureVector<ILabeledFeature<T>> ToLabeledFeatureVector(IFeatureVector<T> vector);

        /// <summary>
        /// Labels vector under specified index
        /// </summary>
        /// <param name="vectorIdx">Index of vector to be labelled</param>
        /// <returns>Labelled vector</returns>
        IFeatureVector<ILabeledFeature<T>> ToLabeledFeatureVector(int vectorIdx);
    }
}
