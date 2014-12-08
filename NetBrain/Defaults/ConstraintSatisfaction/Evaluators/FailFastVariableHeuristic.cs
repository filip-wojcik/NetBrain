using System;
using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.ConstraintSatisfaction.Evaluators;
using NetBrain.Abstracts.ConstraintSatisfaction.Models;

namespace NetBrain.Defaults.ConstraintSatisfaction.Evaluators
{
    /// <summary>
    /// Heuristic that selects variable with the smallest domain to be processed,
    /// which will detect inconsistency very quickly.
    /// </summary>
    /// <typeparam name="TValueType"></typeparam>
    public class FailFastVariableHeuristic<TValueType> : ISelectVariableHeuristic<TValueType>
    {
        public IVariableDomain<TValueType> FindNextVariableToProcess(IConstraintSatisfactionProblem<TValueType> problemDefinition,
            IList<string> alreadyProcessedVariablesNames)
        {
            var variables =
                problemDefinition.VariablesWithDomains.Where(SelectApplicableVariables(alreadyProcessedVariablesNames));
            IVariableDomain<TValueType> selectedDefinition = null;
            int smallestDomainSoFar = int.MaxValue;
            if (variables.Any())
            {
                foreach (var variableDefinition in variables)
                {
                    if (variableDefinition.AllowedValues.Count < smallestDomainSoFar)
                    {
                        smallestDomainSoFar = variableDefinition.AllowedValues.Count;
                        selectedDefinition = variableDefinition;
                    }
                }
            }
            return selectedDefinition;
        }

        private static Func<IVariableDomain<TValueType>, bool> SelectApplicableVariables(IList<string> alreadyProcessedVariablesNames)
        {
            return variableDef => !alreadyProcessedVariablesNames.Contains(variableDef.Variable.Name) && !variableDef.Variable.IsSet;
        }
    }
}
