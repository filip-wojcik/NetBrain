using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.ConstraintSatisfaction.Evaluators;
using NetBrain.Abstracts.ConstraintSatisfaction.Models;

namespace NetBrain.Defaults.ConstraintSatisfaction.Evaluators
{
    public class MostConstrainedVariableHeuristic<TValueType> : ISelectVariableHeuristic<TValueType>
    {
        public IVariableDomain<TValueType> FindNextVariableToProcess(IConstraintSatisfactionProblem<TValueType> problemDefinition, IList<string> alreadyProcessedVariablesNames)
        {
            var applicableVariables =
                problemDefinition.VariablesWithDomains.Where(
                    variableDef =>
                        !variableDef.Variable.IsSet &&
                        !alreadyProcessedVariablesNames.Contains(variableDef.Variable.Name));
            if (applicableVariables.Any())
            {
                IVariableDomain<TValueType> mostConstrainedVariable = null;
                var biggestConstraintsCount = -1;

                foreach (var variableDefinition in applicableVariables)
                {
                    var constraintsCount = problemDefinition.GetConstraintsForVariable(variableDefinition).Count;
                    if (constraintsCount > biggestConstraintsCount)
                    {
                        mostConstrainedVariable = variableDefinition;
                        biggestConstraintsCount = constraintsCount;
                    }
                }
                return mostConstrainedVariable;
            }
            
            return null;
        }
    }
}
