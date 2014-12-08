using System.Collections;
using System.Collections.Generic;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.ConstraintSatisfaction.Models;

namespace NetBrain.Abstracts.ConstraintSatisfaction.Evaluators
{
    /// <summary>
    /// Heuristic used by CSP solver to find next variable
    /// to process
    /// </summary>
    /// <typeparam name="TValueType">Variable value type</typeparam>
    public interface ISelectVariableHeuristic<TValueType>
    {
        IVariableDomain<TValueType> FindNextVariableToProcess(
            IConstraintSatisfactionProblem<TValueType> problemDefinition, 
            IList<string> alreadyProcessedVariablesNames);
    }
}