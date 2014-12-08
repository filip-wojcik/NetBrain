using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Logic.Exceptions;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Defaults.Logic.Models
{
    public class KnowledgeBase<V> : IKnowledgeBase<V>
    {
        # region Private fields

        private Dictionary<ISentence<V>, IList<IList<V>>> sentencesWithAllowedValues;

        # endregion Private fields

        # region Public properties

        public IEnumerable<ISentence<V>> KnownSentences
        {
            get { return this.sentencesWithAllowedValues.Keys; }
        }

        public IDictionary<ISentence<V>, IList<IList<V>>> SentencesWithAllowedValues
        {
            get { return this.sentencesWithAllowedValues; }
        }

        # endregion Public properties

        public KnowledgeBase()
        {
            this.sentencesWithAllowedValues = new Dictionary<ISentence<V>, IList<IList<V>>>();
        }

        #region IKnowledgeBase<V> Members

        public void AddSentenceWithAllowedValues(ISentence<V> newSentence, IList<V> allowedValues)
        {
            this.AddNewSentenceIfNotPresent(newSentence);
            this.sentencesWithAllowedValues[newSentence].Add(allowedValues);
        }


        public void AddSentenceWithAllowedValues(ISentence<V> newSentence, IEnumerable<IList<V>> allowedValues)
        {
            this.AddNewSentenceIfNotPresent(newSentence);
            foreach(var valuesSet in allowedValues) this.sentencesWithAllowedValues[newSentence].Add(valuesSet);
        }

        public IList<IList<V>> AllowedValuesForSentence(ISentence<V> sentence)
        {
            if (this.SentenceIsKnown(sentence))
            {
                return this.sentencesWithAllowedValues[sentence];
            }
            throw new SentenceNotKnownException<V>(sentence);
        }

        public bool SentenceIsKnown(ISentence<V> sentence)
        {
            return this.sentencesWithAllowedValues.ContainsKey(sentence);
        }

        private void AddNewSentenceIfNotPresent(ISentence<V> newSentence)
        {
            if (!this.sentencesWithAllowedValues.ContainsKey(newSentence))
            {
                this.sentencesWithAllowedValues.Add(newSentence, new List<IList<V>>());
            }
        }

        #endregion

    }
}
