using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Evaluators;
using NetBrain.Abstracts.Logic.Exceptions;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Defaults.Logic.Evaluators
{
    public class SubstitutionsFinder<T> : ISubstitutionsFinder<T>
    {
        # region Public properties

        public IVariablesDispatcher<T> Dispatcher { get; set; }
        public IKnowledgeBase<T> SentencesBase { get; set; }

        # endregion Public properties

        # region Constructors

        public SubstitutionsFinder(IVariablesDispatcher<T> dispatcher, IKnowledgeBase<T> sentencesBase)
        {
            Dispatcher = dispatcher;
            SentencesBase = sentencesBase;
        }

        public SubstitutionsFinder()
        {
        }

        # endregion Constructors

        # region Processing methods

        public IDictionary<IVariable<T>, IList<T>> FindPossibleSubstitutionsForUnsetVariables(
            ISentence<T> sentence,
            IList<IVariable<T>> allVariables
            )
        {
            var unsetVariables = allVariables.Where(variable => !variable.IsSet).ToList();
            var alreadyFoundSubstitutions = new Dictionary<IVariable<T>, IList<T>>();

            if (sentence is IComplexSentence<T>)
            {
                this.FindPossibleSubstitutionsForUnsetVariables(sentence as IComplexSentence<T>, allVariables,
                    alreadyFoundSubstitutions);
            }
            else if (sentence is IPredicate<T>)
            {
                this.FindPossibleSubstitutionsForUnsetVariables(sentence as IPredicate<T>, allVariables, unsetVariables,
                    alreadyFoundSubstitutions);
            }
            else
            {
                throw new SentenceNotKnownException<T>(sentence);
            }
            return alreadyFoundSubstitutions;
        }

        public void FindPossibleSubstitutionsForUnsetVariables(
            ISentence<T> sentence,
            IList<IVariable<T>> allVariables,
            IList<IVariable<T>> unsetVariables,
            IDictionary<IVariable<T>, IList<T>> alreadyFoundSubstitutions
            )
        {
            if (sentence is IComplexSentence<T>)
            {
                this.FindPossibleSubstitutionsForUnsetVariables(sentence as IComplexSentence<T>, allVariables,
                    alreadyFoundSubstitutions);
            }
            else if (sentence is IPredicate<T>)
            {
                this.FindPossibleSubstitutionsForUnsetVariables(sentence as IPredicate<T>, allVariables, unsetVariables,
                    alreadyFoundSubstitutions);
            }
            else
            {
                throw new SentenceNotKnownException<T>(sentence);
            }
        }

        public IDictionary<IVariable<T>, IList<T>> FindPossibleSubstitutionsForUnsetVariables(
            IPredicate<T> predicate,
            IList<IVariable<T>> allVariables)
        {
            var variablePropositions = new Dictionary<IVariable<T>, IList<T>>();
            var unsetVariables = allVariables.Where(variable => !variable.IsSet).ToList();
            this.FindPossibleSubstitutionsForUnsetVariables(predicate, allVariables, unsetVariables,
                variablePropositions);
            return variablePropositions;
        }

        public void FindPossibleSubstitutionsForUnsetVariables(
            IPredicate<T> predicate,
            IList<IVariable<T>> allVariables,
            IList<IVariable<T>> unsetVariables,
            IDictionary<IVariable<T>, IList<T>> alreadyFoundSubstitutions)
        {
            IList<IList<T>> allAllowedValuesSets = this.SentencesBase.AllowedValuesForSentence(predicate);
            foreach (var unsetVariable in unsetVariables)
            {
                int indexInDispatchedVariables = allVariables.IndexOf(unsetVariable);
                if(!alreadyFoundSubstitutions.ContainsKey(unsetVariable)) alreadyFoundSubstitutions.Add(unsetVariable, new List<T>());
                foreach (var valueSet in allAllowedValuesSets)
                {
                    if (!alreadyFoundSubstitutions[unsetVariable].Contains(valueSet[indexInDispatchedVariables]))
                    {
                        alreadyFoundSubstitutions[unsetVariable].Add(valueSet[indexInDispatchedVariables]);
                    }
                }
            }            
        }



        public IDictionary<IVariable<T>, IList<T>> FindPossibleSubstitutionsForUnsetVariables(IComplexSentence<T> complexSentence, IList<IVariable<T>> allVariables)
        {
            var possibleValues = new Dictionary<IVariable<T>, IList<T>>();
            this.FindPossibleSubstitutionsForUnsetVariables(complexSentence, allVariables, possibleValues);
            return possibleValues;
        }

        public void FindPossibleSubstitutionsForUnsetVariables(
            IComplexSentence<T> complexSentence,
            IList<IVariable<T>> allVariables,
            IDictionary<IVariable<T>, IList<T>> alreadyFoundSubstitutions)
        {
            IDictionary<int, IList<IVariable<T>>> dispatchedVariables = this.Dispatcher.DispatchVariables(complexSentence, allVariables);
            foreach (var sentenceIdxWithVariables in dispatchedVariables)
            {
                var unsetVariablesForSubsentence = sentenceIdxWithVariables.Value.Where(variable => !variable.IsSet).ToList();
                var subsentence = complexSentence.SubSentences[sentenceIdxWithVariables.Key];
                //If a given sentence has some unassigned variables OR
                // is a complex sentence (which can contain implicit variables on it own)
                if (unsetVariablesForSubsentence.Any() || (subsentence is IComplexSentence<T>))
                {
                    this.FindPossibleSubstitutionsForUnsetVariables(
                        subsentence,
                        sentenceIdxWithVariables.Value,
                        unsetVariablesForSubsentence,
                        alreadyFoundSubstitutions
                        );
                }
            }
        }

        # endregion Processing methods
    }
}
