using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.ConstraintSatisfaction.Models;

namespace NetBrainTests.Defaults.ConstraintSatisfaction.SampleProblems.SimpleSudoku
{
    internal class RowColumnDiagonalConstraint : IConstraint<int>
    {
        public IList<IVariableDomain<int>> ConstrainedVariablesDefinitions { get; private set; }

        public RowColumnDiagonalConstraint(IList<IVariableDomain<int>> constrainedVariablesDefinitions = null)
        {
            ConstrainedVariablesDefinitions = constrainedVariablesDefinitions ?? new List<IVariableDomain<int>>();
        }

        public bool IsSatisfied()
        {
            var values = new HashSet<int>();
            int processedVariablesCount = 0;

            foreach (var variableDefinition in this.ConstrainedVariablesDefinitions)
            {
                if (!variableDefinition.Variable.IsSet) return false;
                values.Add(variableDefinition.Variable.Value);
                processedVariablesCount++;
                if (values.Count != processedVariablesCount) return false;
            }

            return values.Count == processedVariablesCount;
        }

        public void AddConstrainedVariable(IVariableDomain<int> variableDefinition)
        {
            if(!this.ConstrainedVariablesDefinitions.Contains(variableDefinition)) this.ConstrainedVariablesDefinitions.Add(variableDefinition);
        }

        public void RemoveConstrainedVariable(IVariableDomain<int> variableDefinition)
        {
            if(this.ConstrainedVariablesDefinitions.Contains(variableDefinition)) this.ConstrainedVariablesDefinitions.Add(variableDefinition);
        }

        public void ReduceVariableDomains(IVariable<int> currentVariable)
        {
            foreach (var constrainedVariableDefinition in this.ConstrainedVariablesDefinitions)
            {
                if (constrainedVariableDefinition.Variable.Name != currentVariable.Name)
                {
                    constrainedVariableDefinition.RemoveAllowedValue(currentVariable.Value);
                }
            }
        }

        public IConstraint<int> Clone()
        {
            return new RowColumnDiagonalConstraint(this.ConstrainedVariablesDefinitions.Select(def => def.Clone()).ToList());
        }
    }
}
