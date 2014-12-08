using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Models;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Defaults.MachineLearning.Algorithms.ExpertSystem.Models
{
    public class CausalRule<V> : IRule<V>
    {
        # region Protected properties

        protected IReadOnlyList<IVariable<V>> variables;

        # endregion Protected properties

        # region Public properties

        public string Name { get; private set; }

        public IList<IVariable<V>> AntecedentVariables
        {
            get { return this.variables.ToList(); }
        }

        public ISentence<V> Antecedent { get; private set; }

        public IVariableSubstitution<V> Consequent { get; private set; } 

        # endregion Public properties

        # region Construction

        public CausalRule(string name, IReadOnlyList<IVariable<V>> variables, ISentence<V> antecedent, IVariableSubstitution<V> consequent)
        {
            this.Name = name;
            this.variables = variables;
            this.Antecedent = antecedent;
            this.Consequent = consequent;
        }

        # endregion Construction

        # region Processing methods

        public void ApplyConsequent()
        {
            this.Consequent.Apply();
        }

        # endregion Processing methods

        # region Equality methods

        public bool Equals(IRule<V> other)
        {
            if (other == null) return false;
            if (!this.Consequent.Equals(other.Consequent)) return false;
            if (!this.variables.SequenceEqual(other.AntecedentVariables)) return false;
            return this.Antecedent.Equals(other.Antecedent);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IRule<V>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 101;
                foreach (var variable in this.variables) hashCode ^= variable.GetHashCode();
                hashCode ^= this.Consequent.GetHashCode();
                hashCode ^= this.Antecedent.GetHashCode();
                return hashCode;
            }
        }

        # endregion Equality methods
    }
}
