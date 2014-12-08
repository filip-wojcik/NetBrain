using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Models;
using NetBrain.Defaults.Logic.Evaluators;
using NetBrain.Defaults.MachineLearning.Algorithms.ExpertSystem.Evaluators;
using NetBrain.Defaults.MachineLearning.Algorithms.ExpertSystem.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.ExpertSystem.Evaluators
{
    [TestClass()]
    public class RulesEvaluatorTests
    {
        # region Rule definitions finding

        [TestMethod]
        public void FindRuleDefinitionsTest()
        {
            //Given
            var testData = new CarHelplineTestData();

            testData.ElectronicComponentsState.Value = "broken";
            testData.FuelLevelState.Value = "empty";
            testData.Place.Value = "wasteland";

            var subject = new RulesEvaluator<string>(new SentenceEvaluator<string>(testData.KnowledgeBase),
                new IRule<string>[]
                {
                    testData.IfSensorsDeadThenEngineDead, testData.IfEngineDeadOrFuelEmptyThenCarDead,
                    testData.IfAlternatorDeadThenCarDead, testData.IfCarDeadAndInTheMiddleOfNowhereThenCallForHelp,
                    testData.IfElectronicIsDeadThenSensorsDead
                });
            //When
            var definitions = subject.FindRuleDefinitions(testData.IfCarDeadAndInTheMiddleOfNowhereThenCallForHelp);

            //Then
            Assert.AreEqual(3, definitions.Count);
            Assert.IsTrue(
                definitions.Contains(
                    new VariableDefinition<string>(testData.SensorsState, new HashSet<IRule<string>>(){ testData.IfElectronicIsDeadThenSensorsDead })
                ));
            Assert.IsTrue(
                definitions.Contains(
                    new VariableDefinition<string>(testData.EngineState, new HashSet<IRule<string>>() { testData.IfSensorsDeadThenEngineDead})
                ));
            Assert.IsTrue(
                definitions.Contains(
                    new VariableDefinition<string>(testData.CarState, new HashSet<IRule<string>>() { testData.IfEngineDeadOrFuelEmptyThenCarDead})
                ));
        }

        # endregion Rule definitions finding

        # region Foreward chaining

        [TestMethod]
        public void ReadonForeward_EvaluationSuccessfull_Test1()
        {
            //Given
            var testData = new CarHelplineTestData();

            testData.ElectronicComponentsState.Value = "broken";
            testData.FuelLevelState.Value = "empty";
            testData.Place.Value = "wasteland";

            var subject = new RulesEvaluator<string>(new SentenceEvaluator<string>(testData.KnowledgeBase), new IRule<string>[]
                {
                    testData.IfSensorsDeadThenEngineDead, testData.IfEngineDeadOrFuelEmptyThenCarDead,
                    testData.IfAlternatorDeadThenCarDead, testData.IfCarDeadAndInTheMiddleOfNowhereThenCallForHelp,
                    testData.IfElectronicIsDeadThenSensorsDead
                });

            //When
            var results = subject.ReasonForeward(new[] { testData.ElectronicComponentsState, testData.FuelLevelState, testData.Place });

            //Then
            Assert.AreEqual(4, results.Count);
            Assert.IsTrue(
                results.Any(
                result => result.Variable.Equals(testData.SensorsState) && result.ProposedValue.Equals("broken"))
            );

            Assert.IsTrue(
                results.Any(
                result => result.Variable.Equals(testData.EngineState) && result.ProposedValue.Equals("dead"))
            );

            Assert.IsTrue(
                results.Any(
                result => result.Variable.Equals(testData.CarState) && result.ProposedValue.Equals("dead"))
            );

            Assert.IsTrue(
                results.Any(
                result => result.Variable.Equals(testData.Action) && result.ProposedValue.Equals("call for help"))
            );
        }

        # endregion Foreward chaining
    }
}
