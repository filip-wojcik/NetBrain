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
    /// Updated problem definition using some CSP specific algorithms
    /// like arc- or path- consistency
    /// </summary>
    /// <typeparam name="TValueType">Type of value to be used</typeparam>
    public interface IProblemUpdater<TValueType>
    {
        void UpdateProblem(
            IVariableDomain<TValueType> currentlyProcessdVariable,
            IConstraintSatisfactionProblem<TValueType> currentProblem);
    }
}
