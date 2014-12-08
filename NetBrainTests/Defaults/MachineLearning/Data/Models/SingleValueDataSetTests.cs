using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Data.Models
{
    [TestClass()]
    public class SingleValueDataSetTests
    {
        [TestMethod()]
        public void AddVectorTest_VectorIsValid()
        {
            //Given
            var vectorsSet = new SingleValueDataSet<string>(
                new string[] {"elem1", "elem2", "elem3", "elem4"},
                4, 1
                );
            
            //When
            vectorsSet.AddVector(
                new SingleValueFeatureVector<string>(
                    new string[] { "a", "b", "c", "d" }
                    ));
            
            //Then
            Assert.AreEqual(1, vectorsSet.Count());
        }

        [TestMethod()]
        public void AddVectorTest_VectorIsInValid_VectorLengthIsInvalid()
        {
            //Given
            var vectorsSet = new SingleValueDataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, 1
                );

            //When
            vectorsSet.AddVector(
                new SingleValueFeatureVector<string>(
                    new string[] { "a", "b", "c", "d", "e" }
                    ));

            //Then
            Assert.AreEqual(0, vectorsSet.Count());
        }

        [TestMethod()]
        public void UniqValuesUnderIndexTest()
        {
            //Given
            var vectorsSet = new SingleValueDataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, 1
                );

            vectorsSet.AddVector(
                new SingleValueFeatureVector<string>(
                    new string[] { "a1", "b1", "c1", "d1" }
                    ));

            vectorsSet.AddVector(
                new SingleValueFeatureVector<string>(
                    new string[] { "a2", "b2", "c2", "d2" })
                );

            var expected = new HashSet<string> { "b2", "b1" };

            //When
            IEnumerable<string> uniqValues = vectorsSet.UniqValuesInColumn(1);

            //Then
            Assert.IsTrue(expected.SetEquals(uniqValues));
        }

        [TestMethod()]
        public void Indexer_ColumnName_Test()
        {
            //Given
            var vectorsSet = new SingleValueDataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, 1
                );

            vectorsSet.AddVector(
                new SingleValueFeatureVector<string>(
                    new string[] { "a1", "b1", "c1", "d1" }
                    ));

            vectorsSet.AddVector(
                new SingleValueFeatureVector<string>(
                    new string[] { "a2", "b2", "c2", "d2" })
                );

            var expected = new HashSet<string> { "b2", "b1" };

            //When
            IEnumerable<string> uniqValues = vectorsSet.UniqValuesInColumn("elem2");

            //Then
            Assert.IsTrue(expected.SetEquals(uniqValues));
        }

        [TestMethod()]
        public void Indexer_RowIdx_ColumnName_Test()
        {
            //Given
            var vectorsSet = new SingleValueDataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, 1
                );

            vectorsSet.AddVector(
                new SingleValueFeatureVector<string>(
                    new string[] { "a1", "b1", "c1", "d1" }
                    ));

            vectorsSet.AddVector(
                new SingleValueFeatureVector<string>(
                    new string[] { "a2", "b2", "c2", "d2" })
                );

            var expected = "b2";

            //When
            string actual = vectorsSet[1, "elem2"];

            //Then
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void ToLabeledFeatureVectorTest()
        {
            //Given
            var vectorsSet = new SingleValueDataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, 1
                );

            var vector = new SingleValueFeatureVector<string>(
                new string[] {"a", "b", "c", "d"}, 1
                );

            var expectedElems = new ILabeledFeature<string>[]
            {
                new LabeledFeature<string>("a", "elem1"), 
                new LabeledFeature<string>("b", "elem2"),
                new LabeledFeature<string>("c", "elem3"),
                new LabeledFeature<string>("d", "elem4"),
            };

            //When
            var labeledVector = vectorsSet.ToLabeledFeatureVector(vector) as ISingleValueFeatureVector<ILabeledFeature<string>>;

            //Then
            Assert.IsTrue(expectedElems.SequenceEqual(labeledVector.Features));
            Assert.IsTrue(vectorsSet.ValueIndex.Equals(labeledVector.ValueIndex));
        }

        [TestMethod()]
        public void DataSetValues_Test()
        {
            //Given
            var vectorsSet = new SingleValueDataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, 1
                );

            vectorsSet.AddVector(
                new SingleValueFeatureVector<string>(
                    new string[] { "a1", "b1", "c1", "d1" }
                    )
                );

            vectorsSet.AddVector(
                new SingleValueFeatureVector<string>(
                    new string[] { "a2", "b2", "c2", "d2" }
                    )
                );
            var expectedValues = new HashSet<string>() {"b2", "b1"};
            

            //When
            var valuesVectorsSet = vectorsSet.Values;

            //Then
            Assert.IsTrue(
                expectedValues.SetEquals(valuesVectorsSet)
                );
        }

        [TestMethod()]
        public void NonValuesVectorsSet_Test()
        {
            //Given
            var vectorsSet = new SingleValueDataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, 1
                );

            vectorsSet.AddVector(
                new SingleValueFeatureVector<string>(
                    new string[] { "a1", "b1", "c1", "d1" }
                    )
                );

            vectorsSet.AddVector(
                new SingleValueFeatureVector<string>(
                    new string[] { "a2", "b2", "c2", "d2" }
                    )
                );

            var expectedVectors = new SingleValueFeatureVector<string>[]
            {
                new SingleValueFeatureVector<string>(new string[] { "a1", "c1", "d1" }),
                new SingleValueFeatureVector<string>(new string[] { "a2", "c2", "d2" }),
            };

            //When
            var nonValuesVectors = vectorsSet.NonValueVectorsSet;

            //Then
            Assert.IsTrue(
                nonValuesVectors.All(vec => expectedVectors.Contains(vec))
                );
        }
    }
}
