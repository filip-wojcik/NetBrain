using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Models
{
    public interface IVariableDefinition<T>
    {
        IVariable<T> Variable { get; }
        ISet<IRule<T>> DefiningRules { get; }

        void AddRuleToDefinition(IRule<T> ruleToBeAdded);
    }
}
