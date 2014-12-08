using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Abstracts.Logic.Evaluators
{
    public interface ISentenceEvaluator<V>
    {
        IVariablesDispatcher<V> VariablesDispatcher { get; }
        IKnowledgeBase<V> SentencesBase { get; }
        ISubstitutionsFinder<V> SubstitutionsFinder { get; } 

        bool EvaluateSentence(
            ISentence<V> sentenceToEvaluate, 
            IList<IVariable<V>> variables
            );
    }
}
