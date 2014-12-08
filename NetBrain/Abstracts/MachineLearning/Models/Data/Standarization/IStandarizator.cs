using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBrain.Abstracts.MachineLearning.Models.Data.Standarization
{
    public interface IStandarizator<T>
    {
        Func<IEnumerable<T>, IEncoder<T>> CategoricalDataEncoderFactory { get; }
        Func<IEnumerable<T>, INumericalStandardizer<T>> NumericalDataStandardizerFactory { get; }
        Func<IEnumerable<T>, IEncoder<T>> CategoricalValuesEncoderFactory { get; }
        Func<T, double> DirectToDoubleConverter { get; }
        Func<double, T> DirectFromDoubleConverter { get; }
        IList<IEncoder<T>> Encoders { get; set; }

        void PrepareEncoders(IDataSet<T> dataSet);
        IDataSet<double> Standardize(IDataSet<T> featuresVectorsSet);
        IFeatureVector<double> StandardizeVector(IFeatureVector<T> rawVector);

        IDataSet<T> BuildRawData(IDataSet<double> standardizedData);
        IFeatureVector<T> BuildRawVector(IFeatureVector<double> featureVector);
    }
}