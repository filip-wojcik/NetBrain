using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Utils;

namespace NetBrain.Abstracts.ConstraintSatisfaction.Models
{
    /// <summary>
    /// Domain-specific constraint that needs to be satisfied by variables
    /// </summary>
    /// <typeparam name="TValueType"></typeparam>
    public interface IConstraint<TValueType> : ICloneableType<IConstraint<TValueType>>
    {
        IList<IVariableDomain<TValueType>> ConstrainedVariablesDefinitions { get; }

        void AddConstrainedVariable(IVariableDomain<TValueType> variableDefinition);
        void RemoveConstrainedVariable(IVariableDomain<TValueType> variableDefinition);

        /// <summary>
        /// Checks if constraint is satisfied in respect to all variables it  contains
        /// </summary>
        /// <returns></returns>
        bool IsSatisfied();

        /// <summary>
        /// Adjusts other variables domains, given currentVariable actual value
        /// </summary>
        /// <param name="currentVariable">Current variable with its actual value</param>
        /// <returns>Updated version of constrained satisfaction problem, with reduced domains</returns>
        void ReduceVariableDomains(IVariable<TValueType> currentVariable);
    }
}