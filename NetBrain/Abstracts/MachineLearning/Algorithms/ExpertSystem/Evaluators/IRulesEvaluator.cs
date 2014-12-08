using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Evaluators;
using NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Models;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Evaluators
{
    public interface IRulesEvaluator<V>
    {
        ISentenceEvaluator<V> SentenceEvaluator { get; }
        IEnumerable<IRule<V>> KnownRules { get; }

        void AddKnownRule(IRule<V> rule);

        bool RuleIsKnown(IRule<V> rule);
        bool RuleIsFullyCovered(IRule<V> rule, IList<Models.IVariableDefinition<V>> variableDefinitions);

        /// <summary>
        /// If several rules define the same - this methods can reduce
        /// the number of definitions, to select the most precise ones
        /// </summary>
        /// <param name="definitionRules">Rule definitions</param>
        /// <returns>Reduced rule definitions</returns>
        IEnumerable<IRule<V>> ResolveDefinitionConflict(IEnumerable<IRule<V>> definitionRules);

        /// <summary>
        /// Finds rules fully covered by variables
        /// </summary>
        /// <param name="rules">Rules to be checked</param>
        /// <param name="variables">Variables to be checked against</param>
        /// <param name="concludedVariables">Variables concluded in process of evaluation</param>
        /// <returns>List of rules covered by variables</returns>
        IEnumerable<IRule<V>> FindRulesCoveredByVariables(
            IEnumerable<IRule<V>> rules,
            IList<IVariable<V>> variables, 
            IEnumerable<IVariableSubstitution<V>> concludedVariables = null);

        /// <summary>
        /// Searches rules, where consequents include at least one antecedent variable
        /// from defined rule
        /// </summary>
        /// <param name="definedRule">Rule to be defined</param>
        /// <returns>List of rule definitions grouped by variable defined</returns>
        IList<Models.IVariableDefinition<V>> FindRuleDefinitions(IRule<V> definedRule);

        /// <summary>
        /// Finds all conclusions, that can be inferred from the variables
        /// </summary>
        /// <param name="variables">Variables list for conclusion making</param>
        /// <returns>List of variables</returns>
        IList<IVariableSubstitution<V>> ReasonForeward(IList<IVariable<V>> variables);
    }
}
