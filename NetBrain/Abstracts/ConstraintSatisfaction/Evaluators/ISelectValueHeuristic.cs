using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.ConstraintSatisfaction.Models;

namespace NetBrain.Abstracts.ConstraintSatisfaction.Evaluators
{
    /// <summary>
    /// Heuristic used to pick next value to be assigned to variable in
    /// CSP solving process
    /// </summary>
    /// <typeparam name="TValueType"></typeparam>
    public interface ISelectValueHeuristic<TValueType>
    {
        IEnumerable<TValueType> SelectNextValues(
            IConstraintSatisfactionProblem<TValueType> problemDefinition,
            IVariableDomain<TValueType> variableDefinition
            );
    }
}
