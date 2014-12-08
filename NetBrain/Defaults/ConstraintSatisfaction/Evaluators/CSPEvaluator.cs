using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.ConstraintSatisfaction.Evaluators;
using NetBrain.Abstracts.ConstraintSatisfaction.Models;
using NetBrain.Defaults.Common.Models;

namespace NetBrain.Defaults.ConstraintSatisfaction.Evaluators
{
    public class CSPEvaluator<TValueType> : ICSPEvaluator<TValueType>
    {
        public ISelectValueHeuristic<TValueType> SelectValueHeuristic { get; set; }
        public ISelectVariableHeuristic<TValueType> SelectVariableHeuristic { get; set; }
        public IProblemUpdater<TValueType> ProblemUpdater { get; set; } 

        # region Processing methods

        public IEnumerable<ISubstitutionsSet<TValueType>> Solve(IConstraintSatisfactionProblem<TValueType> problemDefinition)
        {
            var alreadyProcessedVariables = new List<string>();
            return this.Solve(problemDefinition, alreadyProcessedVariables);
        }

        protected IEnumerable<ISubstitutionsSet<TValueType>> Solve(IConstraintSatisfactionProblem<TValueType> problemDefinition,
            IList<string> alreadyProcessedVariablesNames)
        {
            if (problemDefinition.IsSolved())
            {
                var solutions = this.BuildSubstitutions(problemDefinition.VariablesWithDomains).ToList();
                yield return new SubstitutionsSet<TValueType>(solutions);
            }

            while (this.KeepSearching(problemDefinition, alreadyProcessedVariablesNames))
            {
                IVariableDomain<TValueType> nextVariableToProcess = this.SelectNextVariableToProcess(problemDefinition,
                    alreadyProcessedVariablesNames);
                if (nextVariableToProcess != null)
                {
                    alreadyProcessedVariablesNames.Add(nextVariableToProcess.Variable.Name);
                    foreach (
                        var solution in
                            this.ProcessVariable(nextVariableToProcess, problemDefinition, alreadyProcessedVariablesNames)
                        )
                    {
                        yield return solution;
                    }
                }
            }
        }

        protected IEnumerable<ISubstitutionsSet<TValueType>> ProcessVariable(IVariableDomain<TValueType> selectedVariableDomain,
            IConstraintSatisfactionProblem<TValueType> problemDefinition, IList<string> alreadyProcessedVariablesNames)
        {
            foreach (var possibleValue in this.SelectNextValues(problemDefinition, selectedVariableDomain))
            {
                IConstraintSatisfactionProblem<TValueType> currentSearchBranchProblemCopy = problemDefinition.Clone();
                IList<string> currentSearchBranchProcessedVariablesNames = new List<string>(alreadyProcessedVariablesNames);

                IVariableDomain<TValueType> aligningVariable =
                    currentSearchBranchProblemCopy.VariablesWithDomains.FirstOrDefault(variable => variable.Variable.Name.Equals(selectedVariableDomain.Variable.Name));
                if (aligningVariable != null)
                {
                    aligningVariable.Variable.Value = possibleValue;
                    this.ProblemUpdater.UpdateProblem(aligningVariable, currentSearchBranchProblemCopy);

                    if (!currentSearchBranchProblemCopy.IsConsistent) continue;

                    foreach (var solution in this.Solve(currentSearchBranchProblemCopy, currentSearchBranchProcessedVariablesNames))
                    {
                        yield return solution;
                    }
                }
            }
        }

        protected bool KeepSearching(IConstraintSatisfactionProblem<TValueType> problemDefinition, IList<string> alreadyProcessedVariablesNames)
        {
            return !problemDefinition.IsSolved() && problemDefinition.VariablesWithDomains.All(def => !def.IsEmpty) && !alreadyProcessedVariablesNames.Count.Equals(problemDefinition.VariablesWithDomains.Count);
        }

        # endregion Processing methods

        # region Helper methods

        protected IList<IVariableSubstitution<TValueType>> BuildSubstitutions(IList<IVariableDomain<TValueType>> variablesDefinitions)
        {
            return (from variableDefinition in variablesDefinitions
                    let variable = variableDefinition.Variable
                    select new VariableSubstitution<TValueType>(variable, variable.Value) as IVariableSubstitution<TValueType>)
                   .ToList();
        }

        protected IVariableDomain<TValueType> SelectNextVariableToProcess(IConstraintSatisfactionProblem<TValueType> problemDefinition,
             IList<string> alreadyProcessedVariablesNames)
        {
            return this.SelectVariableHeuristic.FindNextVariableToProcess(problemDefinition, alreadyProcessedVariablesNames);
        }

        protected IEnumerable<TValueType> SelectNextValues(IConstraintSatisfactionProblem<TValueType> problemDefinition, IVariableDomain<TValueType> variableDefinition)
        {
            return this.SelectValueHeuristic.SelectNextValues(problemDefinition, variableDefinition);
        }

        # endregion Helper methods
    }
}
