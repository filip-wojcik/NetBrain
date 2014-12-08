using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Data.Models
{
    [TestClass()]
    public class FeatureVectorsSetTests
    {
        [TestMethod()]
        public void AddNewVectorWithValues_Test()
        {
            //Given
            var vectorsSet = new DataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, new[] { 1, 3 }
                );

            //When
            vectorsSet.AddVector(
                new FeatureVector<string>(
                    new string[] { "a", "b", "c", "d" },
                    new int[] { 1, 3 }
                    )
                );

            //Then
            Assert.AreEqual(1, vectorsSet.Count());
            Assert.IsTrue(new int[]{ 1, 3}.SequenceEqual(vectorsSet.Vectors.First().ValueIndexes));
        }

        [TestMethod()]
        public void AddNewVectorWithoutValues_Test()
        {
            //Given
            var vectorsSet = new DataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, new[] { 1, 3 }
                );

            //When
            vectorsSet.AddVector(
                new FeatureVector<string>(
                    new string[] { "a", "b", "c", "d" }
                    )
                );

            //Then
            Assert.AreEqual(1, vectorsSet.Count());
            Assert.IsTrue(new int[] { 1, 3 }.SequenceEqual(vectorsSet.Vectors.First().ValueIndexes));
        }

        [TestMethod()]
        public void AddVectorTest_VectorIsInValid_VectorLengthIsInvalid()
        {
            //Given
            var vectorsSet = new DataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, new[] { 1, 3 }
                );

            //When
            vectorsSet.AddVector(
                new FeatureVector<string>(
                    new string[] { "a", "b", "c", "d", "e" },
                    new int[] { 1, 3 }
                    )
                );

            //Then
            Assert.AreEqual(0, vectorsSet.Count());
        }

        [TestMethod()]
        public void AddVectorTest_VectorIsInValid_ValuesIndexesAreInvalid_ValidVectorAdded()
        {
            //Given
            var vectorsSet = new DataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, new[] { 1, 3 }
                );

            //When
            vectorsSet.AddVector(
                new FeatureVector<string>(
                    new string[] { "a", "b", "c", "d" },
                    new int[] { 1, 2, 3 }
                    )
                );

            //Then
            Assert.AreEqual(1, vectorsSet.Count());
            Assert.IsTrue(new int[] { 1, 3 }.SequenceEqual(vectorsSet.Vectors.First().ValueIndexes));
        }

        [TestMethod()]
        public void UniqValuesUnderIndexTest()
        {
            //Given
            var vectorsSet = new DataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, new[] { 1, 3 }
                );

            vectorsSet.AddVector(
                new FeatureVector<string>(
                    new string[] { "a1", "b1", "c1", "d1" },
                    new int[] { 1, 3 }
                    )
                );

            vectorsSet.AddVector(
                new FeatureVector<string>(
                    new string[] { "a2", "b2", "c2", "d2" },
                    new int[] { 1, 3 }
                    )
                );

            var expected = new HashSet<string> { "b2", "b1" };

            //When
            IEnumerable<string> uniqValues = vectorsSet.UniqValuesInColumn(1);

            //Then
            Assert.IsTrue(expected.SetEquals(uniqValues));
        }

        [TestMethod()]
        public void ToLabeledFeatureVectorTest()
        {
            //Given
            var vectorsSet = new DataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, new[] { 1, 3 }
                );

            var vector = new FeatureVector<string>(
                new string[] { "a", "b", "c", "d" },
                new int[] { 1, 3 }
                );

            var expectedElems = new ILabeledFeature<string>[]
            {
                new LabeledFeature<string>("a", "elem1"), 
                new LabeledFeature<string>("b", "elem2"),
                new LabeledFeature<string>("c", "elem3"),
                new LabeledFeature<string>("d", "elem4"),
            };

            //When
            var labeledVector = vectorsSet.ToLabeledFeatureVector(vector);

            //Then
            Assert.IsTrue(expectedElems.SequenceEqual(labeledVector.Features));
            Assert.IsTrue(vectorsSet.ValueColumnsIndexes.SequenceEqual(labeledVector.ValueIndexes));
        }

        [TestMethod()]
        public void ValuesVectorsSet_Test()
        {
            //Given
            var vectorsSet = new DataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, new[] { 1, 3 }
                );

            vectorsSet.AddVector(
                new FeatureVector<string>(
                    new string[] { "a1", "b1", "c1", "d1" },
                    new int[] { 1, 3 }
                    )
                );

            vectorsSet.AddVector(
                new FeatureVector<string>(
                    new string[] { "a2", "b2", "c2", "d2" },
                    new int[] { 1, 3 }
                    )
                );

            var expectedVectors = new FeatureVector<string>[]
            {
                new FeatureVector<string>(new string[] { "b1", "d1" }),
                new FeatureVector<string>(new string[] { "b2", "d2" }),
            };

            //When
            var valuesVectorsSet = vectorsSet.ValuesVectorsSet;

            //Then
            Assert.IsTrue(
                expectedVectors.SequenceEqual(valuesVectorsSet.Vectors)
                );
        }

        [TestMethod()]
        public void NonValuesVectorsSet_Test()
        {
            //Given
            var vectorsSet = new DataSet<string>(
                new string[] { "elem1", "elem2", "elem3", "elem4" },
                4, new[] { 1, 3 }
                );

            vectorsSet.AddVector(
                new FeatureVector<string>(
                    new string[] { "a1", "b1", "c1", "d1" },
                    new int[] { 1, 3 }
                    )
                );

            vectorsSet.AddVector(
                new FeatureVector<string>(
                    new string[] { "a2", "b2", "c2", "d2" },
                    new int[] { 1, 3 }
                    )
                );

            var expectedVectors = new FeatureVector<string>[]
            {
                new FeatureVector<string>(new string[] { "a1", "c1" }),
                new FeatureVector<string>(new string[] { "a2", "c2" }),
            };

            //When
            var valuesVectorsSet = vectorsSet.NonValueVectorsSet;

            //Then
            Assert.IsTrue(
                expectedVectors.SequenceEqual(valuesVectorsSet.Vectors)
                );
        }
    }
}
