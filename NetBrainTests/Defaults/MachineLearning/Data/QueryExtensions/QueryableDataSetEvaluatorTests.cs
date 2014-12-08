using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Data.Models;
using NetBrain.Defaults.MachineLearning.Data.QueryExtensions;

namespace NetBrainTests.Defaults.MachineLearning.Data.QueryExtensions
{
    [TestClass()]
    public class QueryableDataSetEvaluatorTests
    {
        [TestMethod()]
        public void VectorsWhereTest_EqualPredicate()
        {
            //Given
            var dataSet = QueryableTestData.AbstactMixedDataSet();
            var subject = new QueryableDataSetEvaluator<object>(dataSet);
            var nameIsJohnny = new EqualsPredicate<object>("name", "Johnny");
            var expectedVectors = new IFeatureVector<object>[]
            {
                new FeatureVector<object>(new object[]{ "Johnny", "First", 10000, 181 }),
                new FeatureVector<object>(new object[]{ "Johnny", "Third", 2000, 171 })
            };

            //When
            IEnumerable<IFeatureVector<object>> actualVectors = subject.VectorsWhere(nameIsJohnny);

            //Then
            Assert.AreEqual(2, actualVectors.Count());
            Assert.IsTrue(
                expectedVectors.All(expectedVector => actualVectors.Any(actualVector => actualVector.Equals(expectedVector)))
                );
        }

        [TestMethod()]
        public void VectorsWhereTest_GreaterThanPredicate()
        {
            //Given
            var dataSet = QueryableTestData.AbstactMixedDataSet();
            var subject = new QueryableDataSetEvaluator<object>(dataSet);
            var greaterThanPredicate = new GreaterThanPredicate<object>("income", 2000, new NumericObjectComparer());
            var expectedVectors = new IFeatureVector<object>[]
            {
                new FeatureVector<object>(new object[]{ "Greg", "Fourth", 3300, 190 }),
                new FeatureVector<object>(new object[]{ "Johnny", "First", 10000, 181 })
            };

            //When
            IList<IFeatureVector<object>> actualVectors = subject.VectorsWhere(greaterThanPredicate).ToList();

            //Then
            Assert.AreEqual(2, actualVectors.Count());
            Assert.IsTrue(
                expectedVectors.All(expectedVector => actualVectors.Any(actualVector => actualVector.Equals(expectedVector)))
                );
        }
    }
}
