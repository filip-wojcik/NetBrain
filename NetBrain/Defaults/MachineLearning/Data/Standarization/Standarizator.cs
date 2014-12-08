using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data.Standarization;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Data.Models;
using NetBrain.Utils;

namespace NetBrain.Defaults.MachineLearning.Data.Standarization
{
    public class Standarizator<T> : IStandarizator<T>
    {
        # region Consts

        private const string MISSING_TO_DOUBLE_CONVERTER =
            "Unable to convert numeric value: {0} directly to double - missing converter";

        # endregion Consts

        # region Public properties

        public Func<IEnumerable<T>, IEncoder<T>> CategoricalDataEncoderFactory { get; private set; }
        public Func<IEnumerable<T>, INumericalStandardizer<T>> NumericalDataStandardizerFactory { get; private set; }
        public Func<IEnumerable<T>, IEncoder<T>> CategoricalValuesEncoderFactory { get; private set; }
        public Func<T, double> DirectToDoubleConverter { get; private set; }
        public Func<double, T> DirectFromDoubleConverter { get; private set; }

        public IList<IEncoder<T>> Encoders { get; set; }
         
        # endregion Public properties

        # region Construction

        public Standarizator(
            Func<IEnumerable<T>, IEncoder<T>> categoricalDataEncoderFactory,
            Func<IEnumerable<T>, INumericalStandardizer<T>> numericalDataStandardizerFactory,
            Func<IEnumerable<T>, IEncoder<T>> categoricalValuesEncoderFactory,
            Func<T, double> directToDoubleConverter = null,
            Func<double, T> directFromDoubleConverter = null)
        {
            CategoricalDataEncoderFactory = categoricalDataEncoderFactory;
            NumericalDataStandardizerFactory = numericalDataStandardizerFactory;
            CategoricalValuesEncoderFactory = categoricalValuesEncoderFactory;
            DirectToDoubleConverter = directToDoubleConverter;
            DirectFromDoubleConverter = directFromDoubleConverter;
        }

        # endregion Construction

        # region Processing methods

        public void PrepareEncoders(IDataSet<T> dataSet)
        {
            var encoders = new IEncoder<T>[dataSet.SingleVectorSize];
            var valuesPerIndex = new ConcurrentDictionary<int, ConcurrentBag<T>>();
            Parallel.ForEach(dataSet, (vector) =>
            {
                for (int i = 0; i < dataSet.SingleVectorSize; i++)
                {
                    if (!valuesPerIndex.ContainsKey(i))
                    {
                        valuesPerIndex.AddOrUpdate(i, idx => new ConcurrentBag<T>(), (idx, existingList) => existingList);
                    }
                    valuesPerIndex[i].Add(vector[i]);
                }
            });
            foreach (var idxWithValues in valuesPerIndex)
            {
                if (dataSet.ValueColumnsIndexes.Contains(idxWithValues.Key))
                {
                    if (idxWithValues.Value.All(val => val.IsNumeric()))
                    {
                        if(this.DirectToDoubleConverter == null) 
                            throw new ArgumentNullException(string.Format(MISSING_TO_DOUBLE_CONVERTER,  idxWithValues.Value.First()));
                        encoders[idxWithValues.Key] = new ToDoubleEncoder<T>(this.DirectToDoubleConverter, this.DirectFromDoubleConverter);
                    }
                    else
                    {
                        IEncoder<T> encoder = this.CategoricalValuesEncoderFactory(idxWithValues.Value);
                        encoders[idxWithValues.Key] = encoder;
                    }
                }
                else
                {
                    if (idxWithValues.Value.All(val => val.IsNumeric()))
                    {
                        INumericalStandardizer<T> encoder = this.NumericalDataStandardizerFactory(idxWithValues.Value);
                        encoders[idxWithValues.Key] = encoder;
                    }
                    else
                    {
                        IEncoder<T> encoder = this.CategoricalDataEncoderFactory(idxWithValues.Value);
                        encoders[idxWithValues.Key] = encoder;
                    }
                }
            }
            
            this.Encoders = encoders;
        }

        public IDataSet<double> Standardize(IDataSet<T> featuresVectorsSet)
        {
            var standardizedVectors = new ConcurrentBag<IFeatureVector<double>>();
            Parallel.ForEach(featuresVectorsSet, vector => standardizedVectors.Add(this.StandardizeVector(vector)));
            
            var singleVectorSize = standardizedVectors.First().Count();
            return new DataSet<double>(
                this.BuildStandardizedDataLabels(featuresVectorsSet.Columns),
                singleVectorSize,
                standardizedVectors.First().ValueIndexes.ToList(),
                standardizedVectors
                );
        }

        public IFeatureVector<double> StandardizeVector(IFeatureVector<T> rawVector)
        {
            var newValueIndexes = new List<int>();
            var newFeatures = new List<double>();
            for (int i = 0; i < rawVector.Count(); i++)
            {
                if (this.Encoders[i] == null)
                {
                    T value = rawVector[i];
                    if (rawVector.ValueIndexes.Contains(i)) newValueIndexes.Add(newFeatures.Count);
                    newFeatures.Add(Convert.ToDouble(value));
                }
                else
                {
                    T feature = rawVector[i];
                    IList<double> encodedFeature = this.Encoders[i].Encode(feature);

                    if (rawVector.ValueIndexes.Contains(i))
                    {
                        var currentNewFeaturesCount = newFeatures.Count;
                        newValueIndexes.AddRange(Enumerable.Range(currentNewFeaturesCount, encodedFeature.Count));
                    }
                    newFeatures.AddRange(encodedFeature);
                }
            }
            return new FeatureVector<double>(newFeatures, newValueIndexes);
        }

        public IDataSet<T> BuildRawData(IDataSet<double> standardizedData)
        {
            var decodedVectors = new ConcurrentBag<IFeatureVector<T>>();
            List<int> valueIndexes = null;
            int singleVectorSize = -1;
            Parallel.ForEach(standardizedData, standardizedVector =>
            {
                var decodedVector = this.BuildRawVector(standardizedVector);
                decodedVectors.Add(decodedVector);
                if (valueIndexes == null) valueIndexes = new List<int>(decodedVector.ValueIndexes);
                if (singleVectorSize == -1) singleVectorSize = decodedVector.Count();
            });
            var rawDataLabels = this.BuildRawDataLabels(standardizedData.Columns);

            return new DataSet<T>(rawDataLabels, singleVectorSize, valueIndexes, decodedVectors);
        }

        public IFeatureVector<T> BuildRawVector(IFeatureVector<double> featureVector)
        {
            var newFeatures = new List<T>();
            var newValueIndexes = new List<int>();
            int offset = 0;
            for (int i = 0; i < this.Encoders.Count; i++)
            {
                var encoder = this.Encoders[i];
                if(featureVector.ValueIndexes.Contains(offset)) newValueIndexes.Add(i);
                IList<double> features = featureVector.Skip(offset).Take(encoder.EncodedDataCount).ToList();
                newFeatures.Add(encoder.Decode(features));
                offset += encoder.EncodedDataCount;
            }

            return new FeatureVector<T>(newFeatures, newValueIndexes);
        }

        # endregion Processing methods

        # region Utility methods

        protected IList<string> BuildStandardizedDataLabels(IList<string> oldFeatureLabels)
        {
            var newFeatureLabels = new List<string>();
            for (int i = 0; i < oldFeatureLabels.Count(); i++)
            {
                IEncoder<T> currentIndexEncoder = this.Encoders[i];
                if (currentIndexEncoder == null)
                {
                    newFeatureLabels.Add(oldFeatureLabels[i]);
                    continue;
                }
                for (int j = 0; j < currentIndexEncoder.EncodedDataCount; j++)
                {
                    newFeatureLabels.Add(oldFeatureLabels[i]);
                }
            }
            return newFeatureLabels;
        }

        protected IList<string> BuildRawDataLabels(IList<string> standardizedDataLabels)
        {
            var newFeatureLabels = new List<string>();
            int labelsOffset = 0;
            for (int i = 0; i < this.Encoders.Count; i++)
            {
                string firstLabelOfNextFeature = standardizedDataLabels.Skip(labelsOffset).Take(1).First();
                newFeatureLabels.Add(firstLabelOfNextFeature);
                labelsOffset += this.Encoders[i].EncodedDataCount;

            }

            return newFeatureLabels;
        }

        # endregion Utility methods
    }
}
