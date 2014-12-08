using System.Collections.Generic;
using System.Data;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Models;
using NetBrain.Abstracts.Logic.Evaluators;

namespace NetBrain.Defaults.Logic.Evaluators
{
    public class VariableDispatcher<V> : IVariablesDispatcher<V>
    {
        public IDictionary<int, IList<IVariable<V>>> DispatchVariables(IComplexSentence<V> sentence, IList<IVariable<V>> variablesToDispatch)
        {
            var dispatchedVariables = new Dictionary<int, IList<IVariable<V>>>();
            foreach (var mapping in sentence.ParametersMapping)
            {
                dispatchedVariables.Add(mapping.Key, new List<IVariable<V>>());
                foreach (var variableIdx in mapping.Value)
                {
                    if (variableIdx >= 0) dispatchedVariables[mapping.Key].Add(variablesToDispatch[variableIdx]);
                    else
                    {
                        int implicitVariableIdx = variableIdx + 1;
                        dispatchedVariables[mapping.Key].Add(sentence.ImplicitVariables[implicitVariableIdx]);
                    }
                }
            }

            return dispatchedVariables;
        }
    }
}
