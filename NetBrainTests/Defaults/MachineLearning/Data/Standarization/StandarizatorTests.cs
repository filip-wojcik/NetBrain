using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Abstracts.MachineLearning.Models.Data.Standarization;
using NetBrain.Defaults.MachineLearning.Data.Models;
using NetBrain.Defaults.MachineLearning.Data.Standarization;

namespace NetBrainTests.Defaults.MachineLearning.Data.Standarization
{
    [TestClass()]
    public class StandarizatorTests
    {
        [TestMethod()]
        public void EncodeTest()
        {
            //Given
            var dataSet = new DataSet<object>(
                new string[] { "Categorical not value", "Categorical binary", "Numerical not value", "Numerical value", "Categorical value" },
                5, new int[] { 3, 4 }, new List<IFeatureVector<object>>()
                {
                    new FeatureVector<object>(new List<object>(){ "A", false, 1, 100, "AA" }),
                    new FeatureVector<object>(new List<object>(){ "B", true, 2, 200, "BB" }),
                    new FeatureVector<object>(new List<object>(){ "C", true, 3, 300, "CC" }),
                    new FeatureVector<object>(new List<object>(){ "D", false, 4, 400, "DD" }),
                });

            var subject = new Standarizator<object>(
                categoricalDataEncoderFactory: objects =>
                {
                    if (objects.Distinct().Count() == 2) return new BinaryEncoder<object>(objects.Distinct(), string.Empty);
                    else
                    {
                        return new OneOfNEncoder<object>(objects, string.Empty);
                    }
                },
                numericalDataStandardizerFactory: objects => new ScalingNormalizer<object>(num => num, objects.Select(Convert.ToDouble)),
                categoricalValuesEncoderFactory: objects =>
                {
                    if (objects.Count() == 2) return new BinaryEncoder<object>(objects, string.Empty);
                    else return new OneOfNEncoder<object>(objects, string.Empty);
                },
                directToDoubleConverter: Convert.ToDouble,
                directFromDoubleConverter: num => num as object);

            subject.PrepareEncoders(dataSet);

            var expectedNewFeatureLabels = new List<string>()
            {
                "Categorical not value", "Categorical not value", "Categorical not value", "Categorical not value", "Categorical not value",
                "Categorical binary",
                 "Numerical not value",
                 "Numerical value",
                 "Categorical value", "Categorical value", "Categorical value", "Categorical value", "Categorical value"
            };
            var expectedValuesIndexes = new int[] { 7, 8, 9, 10, 11, 12 };

            //When
            var encodedDataSet = subject.Standardize(dataSet);

            //Then
            Assert.AreEqual(4, encodedDataSet.Count());
            Assert.IsTrue(expectedNewFeatureLabels.SequenceEqual(encodedDataSet.Columns));
            Assert.IsTrue(expectedValuesIndexes.SequenceEqual(encodedDataSet.ValueColumnsIndexes));
            Assert.IsTrue(encodedDataSet.Vectors.All(vec => vec.Features.Count.Equals(13)));
        }

        [TestMethod()]
        public void StandardizeVectorTest()
        {
            //Given
            var axis_0_possibleValues = new[] { "A", "B", "C" };
            var axis_1_possibleValues = new[] { "qq", "ww", "ee", "rr" };
            var axis_2_possibleValues = new[] { "vv", "bb", "nn" };

            var axis_0_encoder = new OneOfNEncoder<string>(axis_0_possibleValues, string.Empty);
            var axis_1_encoder = new OneOfNEncoder<string>(axis_1_possibleValues, string.Empty);
            var axis_2_encoder = new OneOfNEncoder<string>(axis_2_possibleValues, string.Empty);

            var vector = new FeatureVector<string>(
                new string[] { "A", "ww", "nn" },
                new int[] { 0, 2 }
                );
            var expectedVector = new FeatureVector<double>(
                new double[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0 },
                new int[] { 0, 1, 2, 3, 9, 10, 11, 12 }
                );

            var standarizator = new Standarizator<string>(null, null, null)
            {
                Encoders = new IEncoder<string>[] {axis_0_encoder, axis_1_encoder, axis_2_encoder}
            };

            //When
            var standardizedVector = standarizator.StandardizeVector(vector);

            //Then
            Assert.AreEqual(expectedVector, standardizedVector);
        }

        [TestMethod]
        public void BuildRawVectorTest()
        {
            //Given
            var axis_0_possibleValues = new object[] { "A", "B", "C" };
            var axis_1_possibleValues = new object[] { "qq", "ww", "ee", "rr" };
            var axis_2_possibleValues = new object[] { "vv", "bb", "nn" };
            var axis_3_possibleValues = new object[] { 13.0, 14.0, 15.0, 17.0 };

            var axis_0_encoder = new OneOfNEncoder<object>(axis_0_possibleValues, string.Empty);
            var axis_1_encoder = new OneOfNEncoder<object>(axis_1_possibleValues, string.Empty);
            var axis_2_encoder = new OneOfNEncoder<object>(axis_2_possibleValues, string.Empty);
            var axis_3_encoder = new ToDoubleEncoder<object>(Convert.ToDouble, num => num as object);

            var encodedVector = new FeatureVector<double>(
                new double[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 13.0 },
                new int[] { 0, 1, 2, 3, 9, 10, 11, 12 }
                );

            var expectedVector = new FeatureVector<object>(
                new object[] { "A", "ww", "nn", 13.0 },
                new int[] { 0, 2 }
                );

            var standarizator = new Standarizator<object>(null, null, null) { Encoders = new IEncoder<object>[] { axis_0_encoder, axis_1_encoder, axis_2_encoder, axis_3_encoder } };

            //When
            IFeatureVector<object> actualVector = standarizator.BuildRawVector(encodedVector);

            //Then
            Assert.AreEqual(expectedVector, actualVector);
        }

        [TestMethod]
        public void Encode_Decode_DataSet_Test()
        {
            //Given
            var rawDataSet = new DataSet<object>(
                new string[] { "Categorical not value", "Categorical binary", "Numerical not value", "Numerical value", "Categorical value" },
                5, new int[] { 3, 4 }, new List<IFeatureVector<object>>()
                {
                    new FeatureVector<object>(new List<object>(){ "A", false, 1.0, 100.0, "AA" }),
                    new FeatureVector<object>(new List<object>(){ "B", true, 2.0, 200.0, "BB" }),
                    new FeatureVector<object>(new List<object>(){ "C", true, 3.0, 300.0, "CC" }),
                    new FeatureVector<object>(new List<object>(){ "D", false, 4.0, 400.0, "DD" }),
                });

            var subject = new Standarizator<object>(
                categoricalDataEncoderFactory: objects =>
                {
                    if (objects.Distinct().Count() == 2) return new BinaryEncoder<object>(objects.Distinct(), string.Empty);
                    else
                    {
                        return new OneOfNEncoder<object>(objects, string.Empty);
                    }
                },
                numericalDataStandardizerFactory: objects => new ScalingNormalizer<object>(num => num, objects.Select(Convert.ToDouble)),
                categoricalValuesEncoderFactory: objects =>
                {
                    if (objects.Count() == 2) return new BinaryEncoder<object>(objects, string.Empty);
                    else return new OneOfNEncoder<object>(objects, string.Empty);
                },
                directToDoubleConverter: Convert.ToDouble,
                directFromDoubleConverter: num => num as object);

            //When
            subject.PrepareEncoders(rawDataSet);
            var encodedDataSet = subject.Standardize(rawDataSet);
            var decodedDataSet = subject.BuildRawData(encodedDataSet);

            //Then
            Assert.IsTrue(decodedDataSet.ValueColumnsIndexes.SequenceEqual(rawDataSet.ValueColumnsIndexes));
            Assert.IsTrue(decodedDataSet.NonValueColumnIndexes.SequenceEqual(rawDataSet.NonValueColumnIndexes));
            Assert.IsTrue(decodedDataSet.Columns.SequenceEqual(rawDataSet.Columns));
            Assert.IsTrue(
                decodedDataSet.Vectors.All(decodedVector => rawDataSet.Any(rawVector => rawVector.Equals(decodedVector)))
                );
        }
    }
}
