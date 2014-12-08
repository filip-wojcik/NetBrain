using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Models
{
    public interface IRule<V> : IEquatable<IRule<V>>
    {
        string Name { get; }
        IList<IVariable<V>> AntecedentVariables { get; }
        ISentence<V> Antecedent { get; }
        IVariableSubstitution<V> Consequent { get; }  


        void ApplyConsequent();
    }
}