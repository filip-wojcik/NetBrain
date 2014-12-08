using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Abstracts.Logic.Exceptions
{
    public class UnassignedVariablePassedToEvaluationException<T> : Exception
    {
        private static readonly string MESSAGE = "Uninitialized variable has been passed for evaluation: {0}!";

        public UnassignedVariablePassedToEvaluationException(IVariable<T> variable)
            : base(string.Format(MESSAGE, variable))
        {
        }
    }
}
