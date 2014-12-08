using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.Logic.Exceptions;
using NetBrain.Abstracts.Logic.Models;
using NetBrain.Defaults.Logic.Models;

namespace NetBrainTests.Defaults.Logic
{
    [TestClass()]
    public class ComplexSentenceTests
    {
        [TestMethod()]
        public void AddSubSentenceWithMappingTest_NewSentenceAdded()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var subject = new ComplexSentence<string>("knows or not", 2, new Or<string>());

            //When
            subject.AddSubSentenceWithMapping(predicate, new []{ 0, 1 });

            //Then
            Assert.IsTrue(subject.SubSentences.Contains(predicate));
            Assert.IsTrue(subject.ParametersMapping[0].SequenceEqual(new int[] { 0, 1 }));
        }

        [TestMethod()]
        public void AddSubSentenceWithMappingTest_AddedMappingToExistingSentence()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var subject = new ComplexSentence<string>("knows or not", 2, new Or<string>());

            //When
            subject.AddSubSentenceWithMapping(predicate, new[] { 0, 1 });
            subject.AddSubSentenceWithMapping(predicate, new[] { 1, 0 });

            //Then
            Assert.IsTrue(subject.SubSentences.Contains(predicate));
            Assert.AreEqual(2, subject.SubSentences.Count);
            Assert.AreEqual(2, subject.ParametersMapping.Count);
            Assert.IsTrue(subject.ParametersMapping[0].SequenceEqual(new int[] { 0, 1 }));
            Assert.IsTrue(subject.ParametersMapping[1].SequenceEqual(new int[] { 1, 0 }));
        }

        [TestMethod()]
        public void AddSubSentenceWithMappingTest_AddedImplicitVariablesMapping()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var subject = new ComplexSentence<string>("knows or not", 2, new Or<string>());

            //When
            subject.AddSubSentenceWithMapping(predicate, new[] { 0, 1 });
            subject.AddSubSentenceWithMapping(predicate, new[] { -1, -2 });

            //Then
            Assert.AreEqual(2, subject.ImplicitVariablesCount);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidMappingIndexException))]
        public void AddSubSentenceWithMappingTest_ParametersIndexTooHigh()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var subject = new ComplexSentence<string>("knows or not", 2, new Or<string>());

            //When
            subject.AddSubSentenceWithMapping(predicate, new[] { 3, 4 });
        }

        [TestMethod()]
        public void NegateTest()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var subject = new ComplexSentence<string>("knows or not", 2, new Or<string>());
            subject.AddSubSentenceWithMapping(predicate, new List<int>() { 1, 0 });
            subject.AddSubSentenceWithMapping(predicate, new List<int>() { 0, 1 });

            //When
            var negatedSentence = subject.Negate() as IComplexSentence<string>;
            
            //Then
            Assert.AreEqual(typeof(And<string>), negatedSentence.Operator.GetType());
            Assert.AreEqual(2, negatedSentence.SubSentences.Count);

            //First subsentence
            var firstSubSentence = negatedSentence.SubSentences.First();
            Assert.AreEqual(typeof(ComplexSentence<string>), firstSubSentence.GetType());
            Assert.AreEqual(typeof(Not<string>), (firstSubSentence as IComplexSentence<string>).Operator.GetType());
            Assert.AreEqual(predicate, (firstSubSentence as IComplexSentence<string>).SubSentences.First());

            //Second subsentence
            var secondSubsentence = negatedSentence.SubSentences.Last();
            Assert.AreEqual(typeof(ComplexSentence<string>), secondSubsentence.GetType());
            Assert.AreEqual(typeof(Not<string>), (secondSubsentence as IComplexSentence<string>).Operator.GetType());
            Assert.AreEqual(predicate, (secondSubsentence as IComplexSentence<string>).SubSentences.First());
        }

        [TestMethod()]
        public void EqualsTest_ResultTrue()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var complexSentence1 = new ComplexSentence<string>("knows or not", 2, new Or<string>());
            complexSentence1.AddSubSentenceWithMapping(predicate, new List<int>() { 1, 0 });
            complexSentence1.AddSubSentenceWithMapping(predicate, new List<int>() { 0, 1 });

            var complexSentence2 = new ComplexSentence<string>("knows or not", 2, new Or<string>());
            complexSentence2.AddSubSentenceWithMapping(predicate, new List<int>() { 1, 0 });
            complexSentence2.AddSubSentenceWithMapping(predicate, new List<int>() { 0, 1 });

            //Then
            Assert.AreEqual(complexSentence1, complexSentence2);

        }

        [TestMethod()] 
        public void EqualsTest_ResultFalse_MappingsDiffer()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var complexSentence1 = new ComplexSentence<string>("knows or not", 2, new Or<string>());
            complexSentence1.AddSubSentenceWithMapping(predicate, new List<int>() { 1, 0 });
            complexSentence1.AddSubSentenceWithMapping(predicate, new List<int>() { 0, 1 });

            var complexSentence2 = new ComplexSentence<string>("knows or not", 2, new Or<string>());
            complexSentence2.AddSubSentenceWithMapping(predicate, new List<int>() { 1, 0 });
            complexSentence2.AddSubSentenceWithMapping(predicate, new List<int>() { 1, 0 });

            //Then
            Assert.AreNotEqual(complexSentence1, complexSentence2);
        }

        [TestMethod()]
        public void EqualsTest_ResultFalse_OperatorDiffers()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var complexSentence1 = new ComplexSentence<string>("knows or not", 2, new Or<string>());
            complexSentence1.AddSubSentenceWithMapping(predicate, new List<int>() { 1, 0 });
            complexSentence1.AddSubSentenceWithMapping(predicate, new List<int>() { 0, 1 });

            var complexSentence2 = new ComplexSentence<string>("knows or not", 2, new And<string>());
            complexSentence2.AddSubSentenceWithMapping(predicate, new List<int>() { 1, 0 });
            complexSentence2.AddSubSentenceWithMapping(predicate, new List<int>() { 0, 1 });

            //Then
            Assert.AreNotEqual(complexSentence1, complexSentence2);
        }

        [TestMethod()]
        public void EqualsTest_ResultFalse_SubsentencesDiffer()
        {
            //Given
            var predicate1 = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows 1", 2);
            var complexSentence1 = new ComplexSentence<string>("knows or not", 2, new Or<string>());
            complexSentence1.AddSubSentenceWithMapping(predicate1, new List<int>() { 1, 0 });
            complexSentence1.AddSubSentenceWithMapping(predicate1, new List<int>() { 0, 1 });

            var predicate2 = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows 2", 2);
            var complexSentence2 = new ComplexSentence<string>("knows or not", 2, new Or<string>());
            complexSentence2.AddSubSentenceWithMapping(predicate2, new List<int>() { 1, 0 });
            complexSentence2.AddSubSentenceWithMapping(predicate2, new List<int>() { 0, 1 });

            //Then
            Assert.AreNotEqual(complexSentence1, complexSentence2);
        }
    }
}
