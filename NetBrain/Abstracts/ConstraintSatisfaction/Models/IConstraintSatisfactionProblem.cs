using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Models;
using NetBrain.Utils;

namespace NetBrain.Abstracts.ConstraintSatisfaction.Models
{
    /// <summary>
    /// Definition of constraint satisfaction roblem, containing:
    /// constraints, variables, and variable domains
    /// </summary>
    /// <typeparam name="TValueType">Type stored in variable</typeparam>
    public interface IConstraintSatisfactionProblem<TValueType> : ICloneableType<IConstraintSatisfactionProblem<TValueType>>
    {
        bool IsConsistent { get; set; }
        IList<IConstraint<TValueType>> Constraints { get; }
        IList<IVariableDomain<TValueType>> VariablesWithDomains { get; }
        IList<IConstraint<TValueType>> GetConstraintsForVariable(IVariableDomain<TValueType> variable);

        bool IsSolved();
    }
}
