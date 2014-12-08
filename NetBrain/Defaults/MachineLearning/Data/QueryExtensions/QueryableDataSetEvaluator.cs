using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Logic.Exceptions;
using NetBrain.Abstracts.Logic.Models;
using NetBrain.Abstracts.MachineLearning.Exceptions;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Abstracts.MachineLearning.Models.Data.QueryExtensions;
using NetBrain.Defaults.Logic.Evaluators;

namespace NetBrain.Defaults.MachineLearning.Data.QueryExtensions
{
    public class QueryableDataSetEvaluator<T>
    {
        private const string MAXIMAL_ARITY_EXCEEDED_MESSAGE = "Allowed arity is 1, found: {0}";

        # region Public properties

        public IDataSet<T> DataSet { get; private set; }

        # endregion Public properties

        # region Construction

        public QueryableDataSetEvaluator(IDataSet<T> dataSet)
        {
            DataSet = dataSet;
        }

        # endregion Construction

        # region Processong methods

        public IEnumerable<IFeatureVector<T>> VectorsWhere(ISentence<T> sentence)
        {
            ValidateSentence(sentence);
            foreach (var vector in this.DataSet)
            {
                if (EvaluateVector(sentence, vector)) yield return vector;
            }
        }

        public bool EvaluateVector(ISentence<T> sentence, IFeatureVector<T> vector)
        {
            if (sentence is IComplexSentence<T>)
            {
                return EvaluateComplexSentence(sentence, vector);
            }
            else if (sentence is IDataVectorPredicate<T>)
            {
                return this.EvaluatePredicate(sentence as IDataVectorPredicate<T>, vector);
            }
            else
            {
                throw new UnknownSentenceTypeException<T>(sentence);
            }
        }

        protected virtual bool EvaluateComplexSentence(ISentence<T> sentence, IFeatureVector<T> vector)
        {
            var complexSentence = sentence as IComplexSentence<T>;
            var subSetencesEvaluationResults =
                complexSentence.SubSentences.Select(
                    subSentence => this.EvaluateVector(subSentence, vector)).ToList();
            return complexSentence.Operator.Evaluate(subSetencesEvaluationResults);
        }

        protected virtual bool EvaluatePredicate(IDataVectorPredicate<T> predicate , IFeatureVector<T> vector)
        {
            int featureIndex = this.DataSet.Columns.IndexOf(predicate.Name);
            return predicate.IsSatisfied(vector[featureIndex]);
        }

        # endregion Processong methods

        # region Validation methods

        protected void ValidateSentence(ISentence<T> sentence)
        {
            var dataVectorPredicateFound = false;
            this.ValidateSentence(sentence, out dataVectorPredicateFound);
            if (!dataVectorPredicateFound)
            {
                throw new NoValidDataVectorPredicateExeption();
            }
        }

        protected void ValidateSentence(ISentence<T> sentence, out bool dataVectorPredicateFound)
        {
            dataVectorPredicateFound = false;
            if (sentence is IDataVectorPredicate<T>)
            {
                this.ValidatePredicate(sentence as IDataVectorPredicate<T>);
                dataVectorPredicateFound = true;
            }
            if(sentence is IComplexSentence<T>) this.ValidateComplexSentence(sentence as IComplexSentence<T>, out dataVectorPredicateFound);
        }

        protected virtual void ValidatePredicate(IDataVectorPredicate<T> predicate)
        {
            if (!this.DataSet.Columns.Contains(predicate.Name)) throw new UnknownFeatureLabelInPredicateException(predicate.Name, this.DataSet.Columns);
            if(predicate.Arity != 1)
            {
                throw new ArgumentException(string.Format(MAXIMAL_ARITY_EXCEEDED_MESSAGE, predicate.Arity));
            }
        }

        protected virtual void ValidateComplexSentence(IComplexSentence<T> complexSentence, out bool dataVectorPredicateFound)
        {
            dataVectorPredicateFound = false;
            if(complexSentence.ImplicitVariablesCount != 0) throw new ImplicitVariablesNotAllowedException();
            foreach(var subsetence in complexSentence.SubSentences) this.ValidateSentence(complexSentence, out dataVectorPredicateFound);
        }

        # endregion Validation methods
    }
}