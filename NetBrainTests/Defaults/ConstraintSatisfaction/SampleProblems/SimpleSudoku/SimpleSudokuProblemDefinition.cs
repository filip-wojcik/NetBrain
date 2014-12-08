using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.ConstraintSatisfaction.Models;
using NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Models;
using NetBrain.Defaults.Common.Models;
using NetBrain.Utils.CollectionExtensions;

namespace NetBrainTests.Defaults.ConstraintSatisfaction.SampleProblems.SimpleSudoku
{
    /// <summary>
    /// Simplified version of sudoku - only one square nxn
    /// values cannot be repeated
    /// </summary>
    internal class SimpleSudokuProblemDefinition : IConstraintSatisfactionProblem<int>
    {
        # region Private properties

        private IList<IConstraint<int>>[,] _variablesConstraints;
        private int _squareSize, _domainMax;

        # endregion Private properties

        # region Public properties

        public IList<IConstraint<int>> Constraints { get; private set; }
        public VariableWithDomainFactory<int> VariableDomainFactory { get; set; }
        public bool IsConsistent { get; set; }

        public IList<IVariableDomain<int>> VariablesWithDomains
        {
            get
            {
                var results = new List<IVariableDomain<int>>();
                for (int rowIdx = 0; rowIdx < this.VariablesDefinitions.GetLength(0); rowIdx++)
                {
                    for (int columnIdx = 0; columnIdx < this.VariablesDefinitions.GetLength(1); columnIdx++)
                    {
                        results.Add(this.VariablesDefinitions[rowIdx, columnIdx]);
                    }
                }
                return results;
            }
        }

        public IVariableDomain<int>[,] VariablesDefinitions { get; private set; } 

        # endregion Public properties

        # region Protected properties

        # endregion Protected properties

        # region Construction

        public SimpleSudokuProblemDefinition(IList<IVariableDomain<int>> variablesWithDomains, int squareSize, VariableWithDomainFactory<int> variableDomianFactory)
        {
            this.VariableDomainFactory = variableDomianFactory;
            this.VariablesDefinitions = new IVariableDomain<int>[squareSize, squareSize];
            this._squareSize = squareSize;
            int variableIdx = 0;
            for (int rowIdx = 0; rowIdx < squareSize; rowIdx++)
            {
                for (int columnIdx = 0; columnIdx < squareSize; columnIdx++)
                {
                    this.VariablesDefinitions[rowIdx, columnIdx] = variablesWithDomains[variableIdx];
                    variableIdx++;
                }
            }
            this.BuildConstraints();
        }

        public SimpleSudokuProblemDefinition(int squareSize, int domainMax, VariableWithDomainFactory<int> variableDomianFactory)
        {
            this.VariableDomainFactory = variableDomianFactory;
            this._squareSize = squareSize;
            this._domainMax = domainMax;
            var allowedValues = Enumerable.Range(1, domainMax).ToList();
            this.BuildProblemDefinition(squareSize, allowedValues);
            this.BuildConstraints();
        }

        private void BuildProblemDefinition(int squareSize, IList<int> allowedValues)
        {
            this.BuildVariableDefinitions(squareSize, allowedValues);
        }

        private void BuildVariableDefinitions(int problemSize, IList<int> allowedValues)
        {
            this.VariablesDefinitions = new IVariableDomain<int>[problemSize, problemSize];
            for (int rowIdx = 0; rowIdx < problemSize; rowIdx++)
            {
                for (int columnIdx = 0; columnIdx < problemSize; columnIdx++)
                {
                    this.VariablesDefinitions[rowIdx, columnIdx] = BuildNewVariableDefinition(allowedValues, rowIdx, columnIdx);
                }
            }
        }

        private void BuildConstraints()
        {
            this._variablesConstraints = new IList<IConstraint<int>>[this.VariablesDefinitions.GetLength(0), this.VariablesDefinitions.GetLength(1)];
            this.Constraints = new List<IConstraint<int>>();

            for (int rowIdx = 0; rowIdx < this.VariablesDefinitions.GetLength(0); rowIdx++)
            {
                this.BuildRowConstraints(rowIdx);
            }
            this.BuildColumnsConstraints();
        }

        private void BuildRowConstraints(int rowIdx)
        {
            var constraint = this.BuildConstraint();
            for (int columnIdx = 0; columnIdx < this.VariablesDefinitions.GetLength(1); columnIdx++)
            {
                AddVariableToConstraintAndConstraintToVariable(rowIdx, columnIdx, constraint);
            }
            this.Constraints.Add(constraint);
        }

        private void BuildColumnsConstraints()
        {
            for (int columnIdx = 0; columnIdx < this.VariablesDefinitions.GetLength(1); columnIdx++)
            {
                var constraint = this.BuildConstraint();
                for (int rowIdx = 0; rowIdx < this.VariablesDefinitions.GetLength(0); rowIdx++)
                {
                    AddVariableToConstraintAndConstraintToVariable(rowIdx, columnIdx, constraint);
                }
                this.Constraints.Add(constraint);
            }
        }

        # endregion Construction

        # region Processing methods

        public IList<IConstraint<int>> GetConstraintsForVariable(IVariableDomain<int> variableDefinition)
        {
            for (int rowIdx = 0; rowIdx < this.VariablesDefinitions.GetLength(0); rowIdx++)
            {
                for (int columnIdx = 0; columnIdx < this.VariablesDefinitions.GetLength(1); columnIdx++)
                {
                    if (this.VariablesDefinitions[rowIdx, columnIdx].Equals(variableDefinition))
                    {
                        return this._variablesConstraints[rowIdx, columnIdx];
                    }
                }
            }
            return new List<IConstraint<int>>();
        }

        private IVariableDomain<int>[,] RewriteExistingDefinitions()
        {
            var rewrittenDefinitions = new IVariableDomain<int>[this.VariablesDefinitions.GetLength(0),
                    this.VariablesDefinitions.GetLength(1)];
            for (int rowIdx = 0; rowIdx < this.VariablesDefinitions.GetLength(0); rowIdx++)
            {
                for (int columnIdx = 0; columnIdx < this.VariablesDefinitions.GetLength(1); columnIdx++)
                {
                    var existingDefinition = this.VariablesDefinitions[rowIdx, columnIdx];
                    rewrittenDefinitions[rowIdx, columnIdx] = new VariableDomain<int>(existingDefinition.Variable, new List<int>(existingDefinition.AllowedValues));
                }
            }
            return rewrittenDefinitions;
        }

        private Tuple<int, int> ParseVariableName(string variableName)
        {
            int[] coordinates = variableName.Split(new char[] {'_'}, 2).Select(Int32.Parse).ToArray();
            return new Tuple<int, int>(coordinates[0], coordinates[1]);
        }

        public bool IsSolved()
        {
            if (this.VariablesWithDomains.All(varDef => varDef.Variable.IsSet))
            {
                var q = "aaa";
            }
            return this.Constraints.All(constraint => constraint.IsSatisfied());
        }

        public IConstraintSatisfactionProblem<int> Clone()
        {
            var newVariables = this.VariablesWithDomains.Select(varDef => varDef.Clone()).ToList();
            return new SimpleSudokuProblemDefinition(newVariables, this._squareSize, this.VariableDomainFactory){ IsConsistent = this.IsConsistent};
        }

        # endregion Processing methods

        # region Helper methods

        public string BuildVariableName(int rowIdx, int columnIdx)
        {
            return string.Format("{0}_{1}", rowIdx, columnIdx);
        }

        private VariableDomain<int> BuildNewVariableDefinition(IList<int> allowedValues, int rowIdx, int columnIdx)
        {
            return new VariableDomain<int>(
                new Variable<int>(this.BuildVariableName(rowIdx, columnIdx)),
                allowedValues
                );
        }

        private void AddOrUpdateConstraintForVariable(int rowIdx, int columnIdx, IConstraint<int> constraint)
        {
            if (this._variablesConstraints[rowIdx, columnIdx] != null)
                this._variablesConstraints[rowIdx, columnIdx].Add(constraint);
            else
            {
                this._variablesConstraints[rowIdx, columnIdx] = new List<IConstraint<int>>() { constraint };
            }
        }

        private void AddVariableToConstraintAndConstraintToVariable(int rowIdx, int columnIdx, IConstraint<int> constraint)
        {
            IVariableDomain<int> variableToAdd = this.VariablesDefinitions[rowIdx, columnIdx];
            constraint.AddConstrainedVariable(variableToAdd);
            this.AddOrUpdateConstraintForVariable(rowIdx, columnIdx, constraint);
        }

        private IConstraint<int> BuildConstraint()
        {
            return new RowColumnDiagonalConstraint();
        }

        # endregion Helper methods

    }
}
