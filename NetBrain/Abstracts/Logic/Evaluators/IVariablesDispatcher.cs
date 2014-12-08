using System.Collections.Generic;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Abstracts.Logic.Evaluators
{
    public interface IVariablesDispatcher<V>
    {
        IDictionary<int, IList<IVariable<V>>> DispatchVariables(IComplexSentence<V> sentence,
            IList<IVariable<V>> variablesToDispatch);
    }
}
