using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.ConstraintSatisfaction.Evaluators;
using NetBrain.Abstracts.ConstraintSatisfaction.Models;


namespace NetBrain.Defaults.ConstraintSatisfaction.Evaluators
{
    /// <summary>
    /// Placeholder heuristic. Does nothing
    /// </summary>
    /// <typeparam name="TValueType"></typeparam>
    public class TakeAllValues<TValueType> : ISelectValueHeuristic<TValueType>
    {
        public IEnumerable<TValueType> SelectNextValues(IConstraintSatisfactionProblem<TValueType> problemDefinition, IVariableDomain<TValueType> variableDefinition)
        {
            return variableDefinition.AllowedValues;
        }
    }
}
