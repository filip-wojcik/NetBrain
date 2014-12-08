using System.Collections.Generic;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Models;
using NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Models;
using NetBrain.Defaults.Common.Models;
using NetBrain.Defaults.Logic.Models;
using NetBrain.Defaults.MachineLearning.Algorithms.ExpertSystem.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.ExpertSystem
{
    public class CarHelplineTestData
    {
        # region VariablesDefinitions

        public IVariable<string> Action = new Variable<string>("action");
        public IVariable<string> CarState = new Variable<string>("car state"); 
        public IVariable<string> AlternatorState = new Variable<string>("alternator state");
        public IVariable<string> ElectronicComponentsState = new Variable<string>("electronic elements state");
        public IVariable<string> SensorsState = new Variable<string>("sensors state");
        public IVariable<string> EngineState = new Variable<string>("engine state");
        public IVariable<string> FuelLevelState =  new Variable<string>("fuel level");
        public IVariable<string> Place = new Variable<string>("place");

        # endregion VariablesDefinitions

        # region Predicates

        public IPredicate<string> FuelIsEmpty = new NetBrain.Defaults.Logic.Models.Predicate<string>("Fuel is empty", 1);
        public IPredicate<string> SensorsAreBroken = new NetBrain.Defaults.Logic.Models.Predicate<string>("sensors are broken", 1);
        public IPredicate<string> EngineIsDead = new NetBrain.Defaults.Logic.Models.Predicate<string>("engine is dead", 1);
        public IPredicate<string> AlternatorIsDead = new NetBrain.Defaults.Logic.Models.Predicate<string>("alternator is dead", 1);
        public IPredicate<string> ElectronicIsBroken = new NetBrain.Defaults.Logic.Models.Predicate<string>("electronic elements are broken", 1);
        public IPredicate<string> CarIsDead = new NetBrain.Defaults.Logic.Models.Predicate<string>("car is dead", 1);
        public IPredicate<string> Nowhere = new NetBrain.Defaults.Logic.Models.Predicate<string>("nowhere", 1);

        # endregion Predicates

        # region Complex sentences

        public IComplexSentence<string> EngineDeadOrFuelEmpty { get; private set; }
        public IComplexSentence<string> CarDeadAndDeadEnd { get; private set; }

        # endregion Complex sentences

        # region Rules

        public IRule<string> IfElectronicIsDeadThenSensorsDead { get; private set; }
        public IRule<string> IfSensorsDeadThenEngineDead { get; private set; }
        public IRule<string> IfEngineDeadOrFuelEmptyThenCarDead { get; private set; }
        public IRule<string> IfAlternatorDeadThenCarDead { get; private set; }
        public IRule<string> IfCarDeadAndInTheMiddleOfNowhereThenCallForHelp { get; private set; }

        # endregion Rules

        # region Knowledge base

        public IKnowledgeBase<string> KnowledgeBase { get; private set; } 

        # endregion Knowledge base

        # region Construction

        public CarHelplineTestData()
        {
            BuildComplexSentences();
            BuildRules();
            BuildKnowledgeBase();
        }

        private void BuildKnowledgeBase()
        {
            this.KnowledgeBase = new KnowledgeBase<string>();
            this.KnowledgeBase.AddSentenceWithAllowedValues(FuelIsEmpty, new string[] { "empty" });
            this.KnowledgeBase.AddSentenceWithAllowedValues(SensorsAreBroken, new string[] { "broken" });
            this.KnowledgeBase.AddSentenceWithAllowedValues(EngineIsDead, new List<IList<string>>()
            {
                new string[] { "dead" }, 
                new string[]{ "broken" }
            });
            this.KnowledgeBase.AddSentenceWithAllowedValues(ElectronicIsBroken, new List<IList<string>>()
            {
                new string[] { "dead" }, 
                new string[]{ "broken" }
            });
            this.KnowledgeBase.AddSentenceWithAllowedValues(CarIsDead, new List<IList<string>>()
            {
                new string[] { "dead" }, 
                new string[] { "broken" }
            });
            this.KnowledgeBase.AddSentenceWithAllowedValues(AlternatorIsDead, new List<IList<string>>() 
            {
                new string[] { "dead" },
                new string[] { "broken" }
            });
            this.KnowledgeBase.AddSentenceWithAllowedValues(Nowhere,
                new List<IList<string>>()
                {
                    new string[] {"nowhere"},
                    new string[] {"dead end"},
                    new string[] {"wasteland"}
                });
        }

        private void BuildRules()
        {
            this.IfElectronicIsDeadThenSensorsDead = new CausalRule<string>(
                name: "if (electronic elements are broken then (sensors are broken)",
                variables: new List<IVariable<string>>(){ ElectronicComponentsState }, 
                antecedent: this.ElectronicIsBroken,
                consequent: new VariableSubstitution<string>(this.SensorsState, "broken")
                );

            this.IfSensorsDeadThenEngineDead= new CausalRule<string>(
                "if (sensors are broken) then (engine is dead)",
                variables: new List<IVariable<string>>() { SensorsState },
                antecedent: this.SensorsAreBroken,
                consequent: new VariableSubstitution<string>(this.EngineState, "dead")
                );

            this.IfEngineDeadOrFuelEmptyThenCarDead= new CausalRule<string>(
                "if (engine is dead) or (fuel is empty) then (car is dead)",
                variables: new List<IVariable<string>>() { EngineState, FuelLevelState},
                antecedent: this.EngineDeadOrFuelEmpty,
                consequent: new VariableSubstitution<string>(this.CarState, "dead")
                );

            this.IfAlternatorDeadThenCarDead= new CausalRule<string>(
                "if (alternator is dead) then (car is dead)",
                variables: new List<IVariable<string>>() { AlternatorState },
                antecedent: this.AlternatorIsDead,
                consequent: new VariableSubstitution<string>(this.CarState, "dead")
                );

            this.IfCarDeadAndInTheMiddleOfNowhereThenCallForHelp = new CausalRule<string>(
                "if (car is dead) and (you are in the middle of nowhere) then (call for help)",
                variables: new List<IVariable<string>>() { CarState, Place },
                antecedent: this.CarDeadAndDeadEnd,
                consequent: new VariableSubstitution<string>(this.Action, "call for help")
                );
        }

        private void BuildComplexSentences()
        {
            this.EngineDeadOrFuelEmpty = new ComplexSentence<string>("engine is dead or fuel is empty", 2,
                new ISentence<string>[] {this.EngineIsDead, this.FuelIsEmpty}, new Or<string>(),
                new Dictionary<int, IList<int>>()
                {
                    {0, new int[] {0}},
                    {1, new int[] {1}},
                });
            this.CarDeadAndDeadEnd = new ComplexSentence<string>("car is dead and you are in the middle of nowhere", 2,
                new ISentence<string>[] {this.EngineIsDead, this.FuelIsEmpty}, new Or<string>(),
                new Dictionary<int, IList<int>>()
                {
                    {0, new int[] {0}},
                    {1, new int[] {1}},
                });
        }

        # endregion Construction
    }
}
