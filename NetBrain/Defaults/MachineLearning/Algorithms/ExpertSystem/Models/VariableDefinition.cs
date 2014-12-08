using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Models;
using NetBrain.Abstracts.Common.Models;

namespace NetBrain.Defaults.MachineLearning.Algorithms.ExpertSystem.Models
{
    public class VariableDefinition<T> : Abstracts.MachineLearning.Algorithms.ExpertSystem.Models.IVariableDefinition<T>
    {
        public IVariable<T> Variable { get; private set; }
        public ISet<IRule<T>> DefiningRules { get; private set; }

        public VariableDefinition(IVariable<T> variable)
        {
            Variable = variable;
            this.DefiningRules = new HashSet<IRule<T>>();
        }

        public VariableDefinition(IVariable<T> variable, ISet<IRule<T>> definingRules)
            : this(variable)
        {
            Variable = variable;
            foreach(var rule in definingRules) this.AddRuleToDefinition(rule);
        }

        public VariableDefinition(IVariable<T> variable, IEnumerable<IRule<T>> definingRules)
            :this(variable)
        {
            foreach (var rule in definingRules) this.AddRuleToDefinition(rule);
        }

        public void AddRuleToDefinition(IRule<T> ruleToBeAdded)
        {
            if(ruleToBeAdded.Consequent.Variable.Equals(Variable))
            {
                this.DefiningRules.Add(ruleToBeAdded);
            }
            else
            {
                //TODO: refactor later as separate exception
                throw new ArgumentException("Invalid rule passed as defining for variable!");
            }
        }

        protected bool Equals(Abstracts.MachineLearning.Algorithms.ExpertSystem.Models.IVariableDefinition<T> other)
        {
            if (other == null) return false;
            if (!this.Variable.Equals(other.Variable)) return false;
            if (!this.DefiningRules.SetEquals(other.DefiningRules)) return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Abstracts.MachineLearning.Algorithms.ExpertSystem.Models.IVariableDefinition<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 105;
                hash ^= this.Variable.GetHashCode();
                foreach (var rule in this.DefiningRules) hash ^= rule.GetHashCode();
                return hash;
            }
        }
    }
}
