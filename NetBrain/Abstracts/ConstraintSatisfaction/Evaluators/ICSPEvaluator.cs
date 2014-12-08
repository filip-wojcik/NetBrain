using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.ConstraintSatisfaction.Models;

namespace NetBrain.Abstracts.ConstraintSatisfaction.Evaluators
{
    /// <summary>
    /// CSP problem evaluator used to solve given problem
    /// </summary>
    /// <typeparam name="TValueType"></typeparam>
    public interface ICSPEvaluator<TValueType>
    {
        ISelectVariableHeuristic<TValueType> SelectVariableHeuristic { get; set; }
        ISelectValueHeuristic<TValueType> SelectValueHeuristic { get; set; }
        IProblemUpdater<TValueType> ProblemUpdater { get; set; }

        IEnumerable<ISubstitutionsSet<TValueType>> Solve(IConstraintSatisfactionProblem<TValueType> problemDefinition);
    }
}
