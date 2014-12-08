using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Evaluators;
using NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Evaluators;
using NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Exceptions;
using NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Models;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.Common.Models;
using NetBrain.Defaults.MachineLearning.Algorithms.ExpertSystem.Models;

namespace NetBrain.Defaults.MachineLearning.Algorithms.ExpertSystem.Evaluators
{
    public class RulesEvaluator<V> : IRulesEvaluator<V>
    {
        # region Protected properties

        protected IList<IRule<V>> knownRules;

        # endregion Protected properties

        # region Public properties

        public ISentenceEvaluator<V> SentenceEvaluator { get; private set; }

        public IEnumerable<IRule<V>> KnownRules
        {
            get { return new List<IRule<V>>(this.knownRules); }
        }

        # endregion Public properties

        # region Construction

        public RulesEvaluator(ISentenceEvaluator<V> sentenceEvaluator, IEnumerable<IRule<V>> knownRules)
        {
            SentenceEvaluator = sentenceEvaluator;
            this.knownRules = new List<IRule<V>>(knownRules);
        }

        # endregion Construction

        # region Processing methods

        public IEnumerable<IRule<V>> FindRulesCoveredByVariables(
            IEnumerable<IRule<V>> rules,
            IList<IVariable<V>> variables, 
            IEnumerable<IVariableSubstitution<V>> concludedVariableSubstitutions = null)
        {
            var variablesSet = new HashSet<IVariable<V>>(variables);
            var results = new List<IRule<V>>();
            foreach (var rule in rules)
            {
                if(
                    rule.AntecedentVariables.All(variable => variables.Contains(variable) || 
                    (concludedVariableSubstitutions != null && concludedVariableSubstitutions.Any(subst => subst.Variable.Equals(variable)))
                 ))
                {
                    results.Add(rule);
                }
            }
            return results;
        }

        public IList<Abstracts.MachineLearning.Algorithms.ExpertSystem.Models.IVariableDefinition<V>> FindRuleDefinitions(IRule<V> definedRule)
        {
            var foundDefinitions = new List<Abstracts.MachineLearning.Algorithms.ExpertSystem.Models.IVariableDefinition<V>>();
            this.FindRuleDefinitions(definedRule, foundDefinitions);
            return foundDefinitions;
        }

        protected void FindRuleDefinitions(IRule<V> definedRule,
            IList<Abstracts.MachineLearning.Algorithms.ExpertSystem.Models.IVariableDefinition<V>> definitionsSoFar)
        {
            foreach (var variable in definedRule.AntecedentVariables.Where(variable => !variable.IsSet))
            {
                if(definitionsSoFar.Any(varDefinition => varDefinition.Variable.Equals(variable))) continue;

                var rulesDefiningVariable =
                    this.knownRules.Where(
                        rule => rule.Consequent.Variable.Equals(variable));

                foreach (var definition in this.ResolveDefinitionConflict(rulesDefiningVariable))
                {
                    if (!definition.AntecedentVariables.All(defVariable => defVariable.IsSet))
                    {
                        this.FindRuleDefinitions(definition, definitionsSoFar);
                    }
                    if (this.RuleIsFullyCovered(definition, definitionsSoFar))
                    {
                        var variableDefinition =
                            definitionsSoFar.FirstOrDefault(varDefinition => varDefinition.Variable.Equals(variable));
                        if (variableDefinition == null)
                        {
                            variableDefinition = new VariableDefinition<V>(variable,
                                new HashSet<IRule<V>>() {definition});
                            definitionsSoFar.Add(variableDefinition);
                        }
                        else
                        {
                            variableDefinition.AddRuleToDefinition(definition);
                        }
                    }
                }
            }
            foreach (var variable in definedRule.AntecedentVariables.Where(variable => !variable.IsSet))
            {
                if(!definitionsSoFar.Any(varDefinition => varDefinition.Variable.Equals(variable)))
                    throw new CannotFindFullyCoveredDefinitionException<V>(variable, definedRule);
            }
        }

        public IList<IVariableSubstitution<V>> ReasonForeward(IList<IVariable<V>> variables)
        {
            var conclusions = new List<IVariableSubstitution<V>>();

            //Tracks all rules processed so far, even the rejected variable definitions
            var allProcessedRules = this.FindRulesCoveredByVariables(this.KnownRules, variables, conclusions);
            bool anythingNewFound = true;

            if (allProcessedRules.Any())
            {
                //Resolves definitions conflicts for reach variable, and applies consequents for positive rules
                this.EvaluateFoundRules(this.ResolveDefinitionConflicts(allProcessedRules), conclusions);
            }
            else return conclusions;

            while (anythingNewFound)
            {
                anythingNewFound = false;
                
                //Finds rules covered rules for current iteration, EXCEPT all processed rules (even rejected possibilities)
                var currentProcessedRules = new HashSet<IRule<V>>(this.FindRulesCoveredByVariables(
                    this.KnownRules.Except(allProcessedRules),
                    variables, 
                    conclusions));
                //Adds current iteration rules to all processed rules
                allProcessedRules = allProcessedRules.Union(currentProcessedRules);

                if (currentProcessedRules.Any())
                {
                    anythingNewFound = true;
                    
                    //Evaluates current iteration rules, and resolves definition conflicts
                    this.EvaluateFoundRules(this.ResolveDefinitionConflicts(currentProcessedRules), conclusions);
                }
                
            }
            return conclusions;
        }

        private IEnumerable<IRule<V>> ResolveDefinitionConflicts(IEnumerable<IRule<V>> rules)
        {
            return from rule in rules
                   group rule by rule.Consequent.Variable
                   into grp
                   let resolvedGroup = this.ResolveDefinitionConflict(grp)
                   from resolvedRule in resolvedGroup
                   select resolvedRule;
        }

        private void EvaluateFoundRules(IEnumerable<IRule<V>> rulesToEvaluate, List<IVariableSubstitution<V>> conclusions)
        {
            foreach (var rule in rulesToEvaluate)
            {
                if (this.SentenceEvaluator.EvaluateSentence(rule.Antecedent, rule.AntecedentVariables))
                {
                    conclusions.Add(rule.Consequent);
                    rule.ApplyConsequent();
                }
            }
        }

        # endregion Processing methods

        # region Utility methods


        public IEnumerable<IRule<V>> ResolveDefinitionConflict(IEnumerable<IRule<V>> definitionRules)
        {
            return new IRule<V>[]
            {
                definitionRules.OrderByDescending(rule => rule.AntecedentVariables.Count).First()
            };
        }

        public void AddKnownRule(IRule<V> rule)
        {
            this.knownRules.Add(rule);
        }

        public bool RuleIsKnown(IRule<V> rule)
        {
            return this.KnownRules.Any(knownRule => knownRule.Equals(rule));
        }

        public bool RuleIsFullyCovered(IRule<V> rule, IList<Abstracts.MachineLearning.Algorithms.ExpertSystem.Models.IVariableDefinition<V>> variableDefinitions)
        {
            return rule.AntecedentVariables.All(variable => variable.IsSet ||
                (variableDefinitions.Any(varDef => varDef.Variable.Equals(variable) && varDef.DefiningRules.Any())));
        }

        # endregion Utility methods
    }
}
