using System.Collections.Generic;

namespace NetBrain.Abstracts.Logic.Models
{
    public interface IKnowledgeBase<V>
    {
        IEnumerable<ISentence<V>> KnownSentences { get; }
        void AddSentenceWithAllowedValues(ISentence<V> newSentence, IList<V> allowedValues);
        void AddSentenceWithAllowedValues(ISentence<V> newSentence, IEnumerable<IList<V>> allowedValues);

        IList<IList<V>> AllowedValuesForSentence(ISentence<V> sentence);

        bool SentenceIsKnown(ISentence<V> sentence);
    }
}