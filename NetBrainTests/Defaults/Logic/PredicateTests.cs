using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.Logic.Models;
using NetBrain.Defaults.Logic.Models;

namespace NetBrainTests.Defaults.Logic
{
    [TestClass()]
    public class PredicateTests
    {
        [TestMethod()]
        public void NegateTest()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);

            //When
            var negatedPredicate = predicate.Negate() as IComplexSentence<string>;

            //Then
            Assert.AreEqual(typeof(Not<string>), negatedPredicate.Operator.GetType());
            Assert.AreEqual(predicate, negatedPredicate.SubSentences.First());
            Assert.IsTrue(new int[]{ 0, 1 }.SequenceEqual(negatedPredicate.ParametersMapping[0]));
        }

        [TestMethod()]
        public void EqualsTest_ResultTrue()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var predicate2 = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);

            //Then
            Assert.AreEqual(predicate, predicate2);
        }

        [TestMethod()]
        public void EqualsTest_ResultFalse()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var predicate2 = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 3);

            //Then
            Assert.AreNotEqual(predicate, predicate2);
        }

        [TestMethod()]
        public void GetHashCodeTest_HashCodeTheSame()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var predicate2 = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);

            //Then
            Assert.AreEqual(predicate.GetHashCode(), predicate2.GetHashCode());
        }

        [TestMethod()]
        public void GetHashCodeTest_HashCodeNotTheSame()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var predicate2 = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 3);

            //Then
            Assert.AreNotEqual(predicate.GetHashCode(), predicate2.GetHashCode());
        }
    }
}
