using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Evaluators;
using NetBrain.Abstracts.Logic.Exceptions;
using NetBrain.Abstracts.Logic.Models;
using NetBrain.Defaults.Common.Models;
using NetBrain.Defaults.Logic.Models;

namespace NetBrain.Defaults.Logic.Evaluators
{
    public class SentenceEvaluator<V> : ISentenceEvaluator<V>
    {
        #region Public properties

        public IVariablesDispatcher<V> VariablesDispatcher { get; private set; }
        public IKnowledgeBase<V> SentencesBase { get; private set; }
        public ISubstitutionsFinder<V> SubstitutionsFinder { get; private set; } 

        #endregion Public properties

        # region Constructor

        public SentenceEvaluator(IKnowledgeBase<V> sentencesBase, IVariablesDispatcher<V> variablesDispatcher, Func<IVariablesDispatcher<V>, IKnowledgeBase<V>, ISubstitutionsFinder<V>> substitutionFinderFactory)
        {
            VariablesDispatcher = variablesDispatcher;
            SentencesBase = sentencesBase;
            SubstitutionsFinder = substitutionFinderFactory(this.VariablesDispatcher, this.SentencesBase);
        }

        /// <summary>
        /// Default constructor using all default implementations of processing classes
        ///<param name="knowledgeBase">Allowed values for sentences base</param>
        /// </summary>
        public SentenceEvaluator(IKnowledgeBase<V> knowledgeBase)
        {
            VariablesDispatcher = new VariableDispatcher<V>();
            SentencesBase = knowledgeBase;
            SubstitutionsFinder = new SubstitutionsFinder<V>(this.VariablesDispatcher, this.SentencesBase);
        }

        # endregion Constructor

        # region Processing methods

        /// <summary>
        /// Evaluates sentence against given set of variables all of which must be set
        /// </summary>
        /// <param name="sentenceToEvaluate">Sentence to perform evaluation on</param>
        /// <param name="variables">Variables to evaluate sentence against</param>
        /// <returns>True or false</returns>
        public bool EvaluateSentence(ISentence<V> sentenceToEvaluate, IList<IVariable<V>> variables)
        {
           ValidateInput(sentenceToEvaluate, variables);

            if (sentenceToEvaluate is IPredicate<V>)
                return this.EvaluatePredicate(sentenceToEvaluate as IPredicate<V>, variables, new Dictionary<string, IVariableSubstitution<V>>());
            if (sentenceToEvaluate is IComplexSentence<V>)
            {
                var complexSentence = sentenceToEvaluate as IComplexSentence<V>;
                IDictionary<IVariable<V>, IList<V>> substitutionsForImplicitVariables = this.SubstitutionsFinder.FindPossibleSubstitutionsForUnsetVariables(complexSentence, variables);
                if (substitutionsForImplicitVariables.Any())
                {
                    IEnumerable<ISubstitutionsSet<V>> resolvingSubstitutionsSet =
                        this.BacktrackResolvingSubstitutions(complexSentence, variables,
                            new Dictionary<string, IVariableSubstitution<V>>(), substitutionsForImplicitVariables, 1);
                    if (resolvingSubstitutionsSet.Any()) return true;
                    else return false;
                }
                return this.EvaluateComplexSentence(sentenceToEvaluate as IComplexSentence<V>, variables, new Dictionary<string, IVariableSubstitution<V>>());
            }
                
            throw new UnknownSentenceTypeException<V>(sentenceToEvaluate);
        }

        /// <summary>
        /// If any variable for evaluation is unset - proper substitution must be found.
        /// This method searches for substitutions of variable, that guarentee correct evaluation of sentence
        /// </summary>
        /// <param name="sentenceToEvaluate">Sentence to evaluate</param>
        /// <param name="variables">Variables to perform evaluation on</param>
        /// <param name="solutionLimit">Limit of solutions to process</param>
        /// <returns>
        /// Substitutions set, which guarantees that sentence can be 
        /// positively evaluated
        /// </returns>
        public IEnumerable<ISubstitutionsSet<V>> FindResolvingSubstitutions(
            ISentence<V> sentenceToEvaluate,
            IList<IVariable<V>> variables,
            int solutionLimit = 3)
        {
            IDictionary<IVariable<V>, IList<V>> possibleValuesForVariables = this.SubstitutionsFinder.FindPossibleSubstitutionsForUnsetVariables(sentenceToEvaluate, variables);
            if (possibleValuesForVariables.Any())
            {
                return this.BacktrackResolvingSubstitutions(sentenceToEvaluate, variables,
                    new Dictionary<string, IVariableSubstitution<V>>(), possibleValuesForVariables, solutionLimit);
            }
            else
            {
                return new ISubstitutionsSet<V>[0];
            }
        }

        private static void ValidateInput(ISentence<V> sentenceToEvaluate, IList<IVariable<V>> variables)
        {
            if (sentenceToEvaluate == null) throw new ArgumentNullException();
            var uninitializedVariable = variables.FirstOrDefault(variable => !variable.IsSet && !variable.IsImplicit);
            if (uninitializedVariable != null)
                throw new UnassignedVariablePassedToEvaluationException<V>(uninitializedVariable);
        }

        # region Backtracking processing methods

        protected IEnumerable<ISubstitutionsSet<V>> BacktrackResolvingSubstitutions(
            ISentence<V> sentenceToResolve,
            IList<IVariable<V>> variables,
            IDictionary<string, IVariableSubstitution<V>> actualSubstitutions,
            IDictionary<IVariable<V>, IList<V>> potentialSubstitutions,
            int solutionsLimit = 1)
        {
            if (potentialSubstitutions.Any())
            {
                KeyValuePair<IVariable<V>, IList<V>> nextVariable = potentialSubstitutions.First();
                var reducedPotentialSubstitutions =
                    new Dictionary<IVariable<V>, IList<V>>(potentialSubstitutions);
                reducedPotentialSubstitutions.Remove(nextVariable.Key);
                foreach (var value in nextVariable.Value)
                {
                    var substitution = new VariableSubstitution<V>(nextVariable.Key, value);
                    var newActualSubstitutions = new Dictionary<string, IVariableSubstitution<V>>(actualSubstitutions);
                    newActualSubstitutions[substitution.Variable.Name] = substitution;
                    int solutionsCounter = 0;
                    foreach (
                        var substitutionsSet in
                            this.BacktrackResolvingSubstitutions(sentenceToResolve, variables, newActualSubstitutions,
                                reducedPotentialSubstitutions, solutionsLimit))
                    {
                        if (solutionsCounter > solutionsLimit) break;
                        yield return substitutionsSet;
                        solutionsCounter += 1;
                    }
                }
            }
            else
            {
                bool result = this.EvaluateSentence(sentenceToResolve, variables, actualSubstitutions);
                if(result) yield return new SubstitutionsSet<V>(actualSubstitutions.Values);
            }
        }

        public bool EvaluateSentence(ISentence<V> sentenceToEvaluate, IList<IVariable<V>> variables,
            IDictionary<string, IVariableSubstitution<V>> actualSubstitutions)
        {
            if (sentenceToEvaluate is IPredicate<V>)
                return this.EvaluatePredicate(sentenceToEvaluate as IPredicate<V>, variables, actualSubstitutions);
            if (sentenceToEvaluate is IComplexSentence<V>)
            {
                return this.EvaluateComplexSentence(sentenceToEvaluate as IComplexSentence<V>, variables, actualSubstitutions);
            }

            throw new UnknownSentenceTypeException<V>(sentenceToEvaluate);
        }

        protected virtual bool EvaluatePredicate(IPredicate<V> predicate, IList<IVariable<V>> variables, IDictionary<string, IVariableSubstitution<V>> actualSubstitutions)
        {
            if (this.SentencesBase.SentenceIsKnown(predicate))
            {
                var variablesValues = new List<V>();
                foreach (var variable in variables)
                {
                    if(variable.IsSet) variablesValues.Add(variable.Value);
                    else variablesValues.Add(actualSubstitutions[variable.Name].ProposedValue);
                }
                var allowedValues = this.SentencesBase.AllowedValuesForSentence(predicate);
                return allowedValues.Any(
                    valuesSet => valuesSet.SequenceEqual(variablesValues)
                    );
            }
            throw new SentenceNotKnownException<V>(predicate);
        }

        protected bool EvaluateComplexSentence(IComplexSentence<V> complexSentence, IList<IVariable<V>> variabels, IDictionary<string, IVariableSubstitution<V>> actualSubstitutions)
        {
            var subSentencesEvaluationResults = new List<bool>();
            IDictionary<int, IList<IVariable<V>>> dispatchedVariables =
                this.VariablesDispatcher.DispatchVariables(complexSentence, variabels);
            foreach (var subSentenceIdxWithVariables in dispatchedVariables)
            {
                var currentSubsentenceSubstitutions = new Dictionary<string, IVariableSubstitution<V>>();
                foreach (var variable in subSentenceIdxWithVariables.Value)
                {
                    if (!variable.IsSet)
                    {
                        currentSubsentenceSubstitutions.Add(variable.Name, actualSubstitutions[variable.Name]);
                    }

                }
                subSentencesEvaluationResults.Add(
                    this.EvaluateSentence(
                        complexSentence.SubSentences[subSentenceIdxWithVariables.Key],
                        subSentenceIdxWithVariables.Value,
                        actualSubstitutions
                        )
                    );
            }
            return complexSentence.Operator.Evaluate(subSentencesEvaluationResults);
        }

        # endregion Backtracking processing methods

        # endregion Processing methods

        # region Helper methods

        /// <summary>
        /// Searches for unsubstitured variables in a given list
        /// </summary>
        /// <param name="actualVariables">Actual variables passed to processing</param>
        /// <param name="knownSubstitutions">Already known variables substitutions</param>
        /// <returns>List of unsubstituted variables</returns>
        public IList<IVariable<V>> FindUnsubstitutedVariables(
            IList<IVariable<V>> actualVariables,
            IList<IVariableSubstitution<V>> knownSubstitutions)
        {
            var unsubstitutedVariables = new List<IVariable<V>>();
            if (!knownSubstitutions.Any()) return actualVariables;
            
            foreach (var variable in actualVariables)
            {
                if (
                    (!knownSubstitutions.Any(subst => subst.Variable.Equals(variable))) ||
                    (!variable.IsSet)
                   )
                {
                    unsubstitutedVariables.Add(variable);
                }
            }
            return unsubstitutedVariables;
        }

        # endregion Helper methods
    }
}
