using System.Collections.Generic;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Models;

namespace NetBrain.Abstracts.Logic.Evaluators
{
    public interface ISubstitutionsFinder<T>
    {
        IVariablesDispatcher<T> Dispatcher { get; set; }
        IKnowledgeBase<T> SentencesBase { get; set; }

        void FindPossibleSubstitutionsForUnsetVariables(
            ISentence<T> sentence,
            IList<IVariable<T>> allVariables,
            IList<IVariable<T>> unsetVariables,
            IDictionary<IVariable<T>, IList<T>> alreadyFoundSubstitutions
            );

        /// <summary>
        /// Overload returning dictionary of possible values for variable
        /// </summary>
        /// <param name="sentence">Sentence to be checked</param>
        /// <param name="allVariables">Variables to be checked</param>
        /// <returns>Dictionary of possible values</returns>
        IDictionary<IVariable<T>, IList<T>> FindPossibleSubstitutionsForUnsetVariables(
            ISentence<T> sentence,
            IList<IVariable<T>> allVariables
            );

        void FindPossibleSubstitutionsForUnsetVariables(
            IPredicate<T> predicate,
            IList<IVariable<T>> allVariables,
            IList<IVariable<T>> unsetVariables,
            IDictionary<IVariable<T>, IList<T>> alreadyFoundSubstitutions);

        /// <summary>
        /// Overload returning dictionary of unset values for predicate
        /// </summary>
        /// <param name="predicate">Predicate to be checked</param>
        /// <param name="allVariables">all variables passed for predicate evaluation</param>
        /// <returns>Dictionary of possible values</returns>
        IDictionary<IVariable<T>, IList<T>> FindPossibleSubstitutionsForUnsetVariables(
            IPredicate<T> predicate,
            IList<IVariable<T>> allVariables);

        /// <summary>
        /// Overload returning dictionary of possible values
        /// </summary>
        /// <param name="complexSentence">Complex sentence to check</param>
        /// <param name="allVariables">All variables to check</param>
        /// <returns>Dictionary of possile values for each variable</returns>
        IDictionary<IVariable<T>, IList<T>>  FindPossibleSubstitutionsForUnsetVariables(
            IComplexSentence<T> complexSentence,
            IList<IVariable<T>> allVariables);

        void FindPossibleSubstitutionsForUnsetVariables(
            IComplexSentence<T> complexSentence,
            IList<IVariable<T>> allVariables,
            IDictionary<IVariable<T>, IList<T>> alreadyFoundSubstitutions);
    }
}