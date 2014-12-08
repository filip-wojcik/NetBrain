using System;
using System.Collections.Generic;
using NetBrain.Abstracts.Common.Models;

namespace NetBrain.Abstracts.Logic.Models
{
    public interface IComplexSentence<V> : ISentence<V>, IEquatable<IComplexSentence<V>>
    {
        IList<ISentence<V>> SubSentences { get; }
        IList<IVariable<V>> ImplicitVariables { get; }
        int ImplicitVariablesCount { get; }
        
        IOperator<V> Operator { get; }

        IDictionary<int, IList<int>> ParametersMapping { get; }

        void AddSubSentenceWithMapping(ISentence<V> subSentence, IList<int> mappings);
    }
}
