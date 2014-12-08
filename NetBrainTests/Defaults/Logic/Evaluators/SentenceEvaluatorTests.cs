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
    public class SentenceEvaluatorTests
    {
        # region Predicates evaluation

        [TestMethod()]
        public void EvaluateSentenceTest_EvaluatePredicate_ResultTrue()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var knowledgeBase = new NetBrain.Defaults.Logic.Models.KnowledgeBase<string>();
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person A", "Person B" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person B", "Person C" });

            var firstPerson = new Variable<string>("peson1");
            var secondPerson = new Variable<string>("peson2");

            var subject = new SentenceEvaluator<string>(knowledgeBase);

            //When
            firstPerson.Value = "Person B";
            secondPerson.Value = "Person C";

            //Then
            Assert.IsTrue(subject.EvaluateSentence(predicate, new IVariable<string>[] { firstPerson, secondPerson }));
        }

        [TestMethod()]
        public void EvaluateSentenceTest_EvaluatePredicate_ResultFalse()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var knowledgeBase = new NetBrain.Defaults.Logic.Models.KnowledgeBase<string>();
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person A", "Person B" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person B", "Person C" });

            var subject = new SentenceEvaluator<string>(knowledgeBase);

            var firstPerson = new Variable<string>("peson1");
            var secondPerson = new Variable<string>("peson2");

            //When
            firstPerson.Value = "Person A";
            secondPerson.Value = "Person C";

            //Then
            Assert.IsFalse(subject.EvaluateSentence(predicate, new IVariable<string>[] { firstPerson, secondPerson }));
        }

        # endregion Predicates evaluation

        # region Complex sentences evaluation, without unset variables

        [TestMethod()]
        public void EvaluateComplexSentenceTest_ResultTrue()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var complexSentence = new NetBrain.Defaults.Logic.Models.ComplexSentence<string>("A knows B or B knows A", 2, new NetBrain.Defaults.Logic.Models.Or<string>());
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { 0, 1 });
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { 1, 0 });

            var knowledgeBase = new NetBrain.Defaults.Logic.Models.KnowledgeBase<string>();
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person A", "Person B" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person Q", "Person W" });

            var person1 = new Variable<string>("person1");
            var person2 = new Variable<string>("person2");

            var subject = new SentenceEvaluator<string>(knowledgeBase);

            //When
            person1.Value = "Person Q";
            person2.Value = "Person W";

            //Then
            Assert.IsTrue(subject.EvaluateSentence(complexSentence, new IVariable<string>[] { person1, person2 }));
        }

        [TestMethod()]
        public void EvaluateComplexSentenceTest_ResultFalse()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var complexSentence = new NetBrain.Defaults.Logic.Models.ComplexSentence<string>("A knows B or B knows A", 2, new NetBrain.Defaults.Logic.Models.Or<string>());
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { 0, 1 });
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { 1, 0 });

            var knowledgeBase = new NetBrain.Defaults.Logic.Models.KnowledgeBase<string>();
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person A", "Person B" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person Q", "Person W" });

            var person1 = new Variable<string>("person1");
            var person2 = new Variable<string>("person2");

            var subject = new SentenceEvaluator<string>(knowledgeBase);

            //When
            person1.Value = "Person A";
            person2.Value = "Person Q";

            //Then
            Assert.IsFalse(subject.EvaluateSentence(complexSentence, new IVariable<string>[] { person1, person2 }));
        }

        [TestMethod()]
        public void EvaluateComplexSentenceTest_OneImplicitVariable_ResultTrue()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var complexSentence = new NetBrain.Defaults.Logic.Models.ComplexSentence<string>("A knows some X and some X knows B", 2, new And<string>());
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { 0, -1 });
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { -1, 1 });

            var knowledgeBase = new NetBrain.Defaults.Logic.Models.KnowledgeBase<string>();
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person A", "Person B" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person B", "Person C" });

            var person1 = new Variable<string>("person1");
            var person2 = new Variable<string>("person2");

            var subject = new SentenceEvaluator<string>(knowledgeBase);

            //When
            person1.Value = "Person A";
            person2.Value = "Person C";

            //Then
            Assert.IsTrue(subject.EvaluateSentence(complexSentence, new IVariable<string>[] { person1, person2 }));
        }

        [TestMethod()]
        public void EvaluateComplexSentenceTest_OneImplicitVariable_ResultFalse()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var complexSentence = new NetBrain.Defaults.Logic.Models.ComplexSentence<string>("A knows some X and some X knows B", 2, new And<string>());
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { 0, -1 });
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { -1, 1 });

            var knowledgeBase = new NetBrain.Defaults.Logic.Models.KnowledgeBase<string>();
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person A", "Person B" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person B", "Person C" });

            var person1 = new Variable<string>("person1");
            var person2 = new Variable<string>("person2");

            var subject = new SentenceEvaluator<string>(knowledgeBase);

            //When
            person1.Value = "Person C";
            person2.Value = "Person B";

            //Then
            Assert.IsFalse(subject.EvaluateSentence(complexSentence, new IVariable<string>[] { person1, person2 }));
        }

        # endregion Complex sentences evaluation, without unset variables

        # region Finding possible substitutions

        [TestMethod()]
        public void FindSubstitutions_ForPredicate_SubstitutionsExist()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var knowledgeBase = new NetBrain.Defaults.Logic.Models.KnowledgeBase<string>();

            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person A", "Person B" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person B", "Person C" });

            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person Q", "Person W" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person W", "Person X" });

            var firstPerson = new Variable<string>("peson1");
            var secondPerson = new Variable<string>("peson2");

            var subject = new SentenceEvaluator<string>(knowledgeBase);

            //When
            IEnumerable<ISubstitutionsSet<string>> substitutions = subject.FindResolvingSubstitutions(predicate,
                new IVariable<string>[] { firstPerson, secondPerson });

            //Then
            Assert.AreEqual(4, substitutions.Count());

            Assert.IsTrue(
                substitutions.Any(substSet =>
                    substSet.VariableSubstitutions.Any(variableSubst => variableSubst.Variable.Equals(firstPerson) && variableSubst.ProposedValue.Equals("Person A")) &&
                    substSet.VariableSubstitutions.Any(variableSubst => variableSubst.Variable.Equals(secondPerson) && variableSubst.ProposedValue.Equals("Person B"))
                )
            );
            Assert.IsTrue(
                substitutions.Any(substSet =>
                    substSet.VariableSubstitutions.Any(variableSubst => variableSubst.Variable.Equals(firstPerson) && variableSubst.ProposedValue.Equals("Person B")) &&
                    substSet.VariableSubstitutions.Any(variableSubst => variableSubst.Variable.Equals(secondPerson) && variableSubst.ProposedValue.Equals("Person C"))
                )
            );
            Assert.IsTrue(
                substitutions.Any(substSet =>
                    substSet.VariableSubstitutions.Any(variableSubst => variableSubst.Variable.Equals(firstPerson) && variableSubst.ProposedValue.Equals("Person Q")) &&
                    substSet.VariableSubstitutions.Any(variableSubst => variableSubst.Variable.Equals(secondPerson) && variableSubst.ProposedValue.Equals("Person W"))
                )
            );

            Assert.IsTrue(
                substitutions.Any(substSet =>
                    substSet.VariableSubstitutions.Any(variableSubst => variableSubst.Variable.Equals(firstPerson) && variableSubst.ProposedValue.Equals("Person W")) &&
                    substSet.VariableSubstitutions.Any(variableSubst => variableSubst.Variable.Equals(secondPerson) && variableSubst.ProposedValue.Equals("Person X"))
                )
            );

        }

        [TestMethod()]
        public void FindSubstitutions_ForComplexSentence_SubstitutionsExist()
        {
            //Given
            var predicate = new NetBrain.Defaults.Logic.Models.Predicate<string>("knows", 2);
            var complexSentence = new NetBrain.Defaults.Logic.Models.ComplexSentence<string>("A knows some X and some X knows B", 2, new And<string>());
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { 0, -1 });
            complexSentence.AddSubSentenceWithMapping(predicate, new[] { -1, 1 });

            var knowledgeBase = new NetBrain.Defaults.Logic.Models.KnowledgeBase<string>();
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person A", "Person B" });
            knowledgeBase.AddSentenceWithAllowedValues(predicate, new string[] { "Person B", "Person C" });

            var person1 = new Variable<string>("person1");
            var person2 = new Variable<string>("person2");

            var subject = new SentenceEvaluator<string>(knowledgeBase);

            //When
            IEnumerable<ISubstitutionsSet<string>> substitutions = subject.FindResolvingSubstitutions(complexSentence,
                new IVariable<string>[] {person1, person2});

            //Then
            Assert.AreEqual(1, substitutions.Count());
            Assert.IsTrue(
                substitutions.Any(substSet =>
                    substSet.VariableSubstitutions.Any(variableSubst => variableSubst.Variable.Equals(person1) && variableSubst.ProposedValue.Equals("Person A")) &&
                    substSet.VariableSubstitutions.Any(variableSubst => variableSubst.Variable.Equals(complexSentence.ImplicitVariables.First()) && variableSubst.ProposedValue.Equals("Person B")) &&
                    substSet.VariableSubstitutions.Any(variableSubst => variableSubst.Variable.Equals(person2) && variableSubst.ProposedValue.Equals("Person C"))
                )
            );
            
        }

        # endregion Finding possible substitutions
    }
}
