using NetBrain.Abstracts.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Models;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.ExpertSystem.Exceptions
{
    public class CannotFindFullyCoveredDefinitionException<V> : Exception
    {
        public static string MESSAGE = "Cannot find fully instantiated definition for variable {0} in rule {1}";

        public CannotFindFullyCoveredDefinitionException(IVariable<V> variable, IRule<V> rule)
            : base(string.Format(MESSAGE, variable.Name, rule.Name))
        {
        }

        public CannotFindFullyCoveredDefinitionException(string message) 
            : base(message)
        {
        }
    }
}
