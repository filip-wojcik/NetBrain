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
    public class SubstitutionsFinderTests
    {
        [TestMethod()]
        public void FindPossibleSubstitutionsForUnsetVariables_PredicateTest()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var knowledgeBase = new NetBrain.Defaults.Logic.Models.KnowledgeBase<string>();
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person A", "Person B" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person B", "Person C" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person C", "Person D" });

            var subject = new SubstitutionsFinder<string>(new VariableDispatcher<string>(), knowledgeBase);

            var variable1 = new Variable<string>("person1", "person1");
            var variable2 = new Variable<string>("person2");

            var expectedSubstitutionsForVariable2 = new string[] { "Person B", "Person C", "Person D" };
            //When
            var possibleSubstitutions = new Dictionary<IVariable<string>, IList<string>>();
            subject.FindPossibleSubstitutionsForUnsetVariables(predicate,
                new IVariable<string>[] { variable1, variable2 }, new IVariable<string>[] { variable2 }, possibleSubstitutions);

            //Then
            Assert.IsTrue(expectedSubstitutionsForVariable2.SequenceEqual(possibleSubstitutions[variable2]));
        }

        [TestMethod()]
        public void FindPossibleSubstitutionsForUnsetVariables_ComplexSentence_NoImplicitVariables_Test()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var complexSentence = new NetBrain.Defaults.Logic.Models.ComplexSentence<string>("A knows B or B knows C", 3, new NetBrain.Defaults.Logic.Models.Or<string>());
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { 0, 1 });
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { 1, 2 });

            var knowledgeBase = new NetBrain.Defaults.Logic.Models.KnowledgeBase<string>();
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person A", "Person B" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person Q", "Person W" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person X", "Person Y" });

            var person1 = new Variable<string>("person1", "A");
            var person2 = new Variable<string>("person2", "B");
            var person3 = new Variable<string>("person3");

            var subject = new SubstitutionsFinder<string>(new VariableDispatcher<string>(), knowledgeBase);

            var expectedValuesForPerson3 = new string[] { "Person B", "Person W", "Person Y" };
            var substitutions = new Dictionary<IVariable<string>, IList<string>>();

            //When
            subject.FindPossibleSubstitutionsForUnsetVariables(complexSentence, new IVariable<string>[] { person1, person2, person3 }, substitutions);

            //Then
            Assert.IsTrue(expectedValuesForPerson3.SequenceEqual(substitutions[person3]));
        }

        [TestMethod()]
        public void FindPossibleSubstitutionsForUnsetVariables_ComplexSentence_WithImplicitVariables_Test()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var complexSentence = new NetBrain.Defaults.Logic.Models.ComplexSentence<string>("A knows some X and some X knows B", 2, new NetBrain.Defaults.Logic.Models.And<string>());
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { 0, -1 });
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { -1, 1 });

            var knowledgeBase = new NetBrain.Defaults.Logic.Models.KnowledgeBase<string>();
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person A", "Person B" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person B", "Person C" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person Q", "Person W" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person X", "Person Y" });

            var person1 = new Variable<string>("person1", "Person A");
            var person2 = new Variable<string>("person2", "Person C");

            var subject = new SubstitutionsFinder<string>(new VariableDispatcher<string>(), knowledgeBase);

            var expectedValuesForImplicitVariable = new HashSet<string>(
                new string[] { "Person A", "Person Q", "Person X", "Person B", "Person C", "Person W", "Person Y" }
                );
            var substitutions = new Dictionary<IVariable<string>, IList<string>>();

            //When
            subject.FindPossibleSubstitutionsForUnsetVariables(complexSentence, new IVariable<string>[] { person1, person2 }, substitutions);

            //Then
            var implicitComplexSentenceVariable = complexSentence.ImplicitVariables.First();
            Assert.IsTrue(expectedValuesForImplicitVariable.SetEquals(substitutions[implicitComplexSentenceVariable]));
        }

        [TestMethod]
        public void FindPossibleSubstitutionsForUnsetVariables_NestedComplexSentence_WithImplicitVariables_Test()
        {
            //Given
            var knows = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var dealsWith = new NetBrain.Defaults.Logic.Models.Predicate<string>("deals with", 2);
            var innerComplexSentence = new NetBrain.Defaults.Logic.Models.ComplexSentence<string>("B knows some X who deals with C", 2, new And<string>());
            innerComplexSentence.AddSubSentenceWithMapping(knows, new int[]{ 0, -1 });
            innerComplexSentence.AddSubSentenceWithMapping(dealsWith, new int[] { -1, 1 });

            var outerComplexSentence =
                new NetBrain.Defaults.Logic.Models.ComplexSentence<string>("A knows C or (B knows some X who deals with C)", 3,
                    new Or<string>());
            outerComplexSentence.AddSubSentenceWithMapping(knows, new []{ 0, 2 });
            outerComplexSentence.AddSubSentenceWithMapping(innerComplexSentence, new[] { 1, 2 });

            var knowledgeBase = new NetBrain.Defaults.Logic.Models.KnowledgeBase<string>();
            knowledgeBase.AddSentenceWithAllowedValues(knows, new string[] { "Person A", "Person B" });
            knowledgeBase.AddSentenceWithAllowedValues(knows, new string[] { "Person B", "Person C" });
            knowledgeBase.AddSentenceWithAllowedValues(dealsWith, new string[] { "Person Q", "Person W" });
            knowledgeBase.AddSentenceWithAllowedValues(dealsWith, new string[] { "Person X", "Person Y" });

            var subject = new SubstitutionsFinder<string>(new VariableDispatcher<string>(), knowledgeBase);

            var person1 = new Variable<string>("Person1", "Person A");
            var person2 = new Variable<string>("Person1", "Person B");
            var person3 = new Variable<string>("Person1", "Person C");

            var expected = new HashSet<string>(new string[] { "Person B", "Person C", "Person Q", "Person X" });
            //When
            var possibleSubstitutions = new Dictionary<IVariable<string>, IList<string>>();
            subject.FindPossibleSubstitutionsForUnsetVariables(outerComplexSentence, new IVariable<string>[]{ person1, person2, person3 }, possibleSubstitutions);

            //Then
            var implicitVariable = innerComplexSentence.ImplicitVariables.First();
            Assert.IsTrue(expected.SetEquals(possibleSubstitutions[implicitVariable]));
        }
    }
}

