using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Models;
using NetBrain.Defaults.Common.Models;
using NetBrain.Defaults.Logic.Evaluators;
using NetBrain.Defaults.Logic.Models;

namespace NetBrainTests.Defaults.Logic.Evaluators
{
    [TestClass()]
    public class VariableDispatcherTests
    {
        [TestMethod()]
        public void DispatchVariablesTest_NormalVariablesDispatching()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 3);
            var complexSentence = new ComplexSentence<string>("A knows B and B knows C", 3, new Or<string>());
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { 0, 1 });
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { 1, 2 });

            var subject = new VariableDispatcher<string>();

            var personA = new Variable<string>("person A");
            var personB = new Variable<string>("person B");
            var personC = new Variable<string>("person C");

            var firstSubSentenceVariablesExpected = new List<IVariable<string>>() { personA, personB };
            var secondSubSentenceVariablesExpected = new List<IVariable<string>>() { personB, personC };

            //When
            IDictionary<int, IList<IVariable<string>>> dispatchedVariables = subject.DispatchVariables(complexSentence,
                new IVariable<string>[] {personA, personB, personC});

            //Then
            Assert.IsTrue(firstSubSentenceVariablesExpected.SequenceEqual(dispatchedVariables[0]));
            Assert.IsTrue(secondSubSentenceVariablesExpected.SequenceEqual(dispatchedVariables[1]));
        }

        [TestMethod()]
        public void DispatchVariablesTest_ImplicitVariablesDispatching()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var complexSentence = new ComplexSentence<string>("A knows some X and some X knows B", 2, new Or<string>());
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { 0, -1 });
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { -1, 1 });

            var subject = new VariableDispatcher<string>();

            var personA = new Variable<string>("person A");
            var implicitPerson = complexSentence.ImplicitVariables.First();
            var personB = new Variable<string>("person B");

            var firstSubSentenceVariablesExpected = new List<IVariable<string>>() { personA, implicitPerson };
            var secondSubSentenceVariablesExpected = new List<IVariable<string>>() { implicitPerson, personB };

            //When
            IDictionary<int, IList<IVariable<string>>> dispatchedVariables = subject.DispatchVariables(complexSentence,
                new IVariable<string>[] { personA, personB });

            //Then
            Assert.IsTrue(firstSubSentenceVariablesExpected.SequenceEqual(dispatchedVariables[0]));
            Assert.IsTrue(secondSubSentenceVariablesExpected.SequenceEqual(dispatchedVariables[1]));
        }
    }
}
