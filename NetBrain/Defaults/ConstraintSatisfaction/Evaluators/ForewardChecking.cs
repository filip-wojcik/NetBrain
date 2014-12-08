using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.ConstraintSatisfaction.Evaluators;
using NetBrain.Abstracts.ConstraintSatisfaction.Models;

namespace NetBrain.Defaults.ConstraintSatisfaction.Evaluators
{
    public class ForewardChecking<TValueType> : IProblemUpdater<TValueType>
    {
        public void UpdateProblem(
            IVariableDomain<TValueType> currentlyProcessdVariable,
            IConstraintSatisfactionProblem<TValueType> currentProblem)
        {
            foreach (var constraint in currentProblem.GetConstraintsForVariable(currentlyProcessdVariable))
            {
                constraint.ReduceVariableDomains(currentlyProcessdVariable.Variable);
                if (constraint.ConstrainedVariablesDefinitions.Any(variableDef => variableDef.IsEmpty))
                {
                    currentProblem.IsConsistent = false;
                    break;
                }
            }
        }
    }
}
