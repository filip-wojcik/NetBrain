using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NetBrain.Abstracts.Common.Models;
using NetBrain.Abstracts.Logic.Exceptions;
using NetBrain.Abstracts.Logic.Models;
using NetBrain.Defaults.Common.Models;

namespace NetBrain.Defaults.Logic.Models
{
    public class ComplexSentence<V> : IComplexSentence<V>, IEquatable<ComplexSentence<V>>
    {
        # region Consts

        private static string IMPLICIT_VARIABLE_NAME = "implicit_variable_sentence_{0}_no_{1}";

        # endregion Consts

        # region Public properties

        public IList<ISentence<V>> SubSentences { get; private set; }

        public IList<IVariable<V>> ImplicitVariables { get; private set; }

        public int ImplicitVariablesCount 
        {
            get { return this.ImplicitVariables == null ? 0 : this.ImplicitVariables.Count; }
        }

        public IOperator<V> Operator { get; private set; }

        public IDictionary<int, IList<int>> ParametersMapping { get; private set; }

        public string Name { get; private set; }

        public int Arity { get; private set; }

        # endregion Public properties

        # region Constructor

        public ComplexSentence(string name, int arity, IOperator<V> @operator)
        {
            Name = name;
            Arity = arity;
            Operator = @operator;
            this.ImplicitVariables = new List<IVariable<V>>();
            this.ParametersMapping = new ConcurrentDictionary<int, IList<int>>();
            this.SubSentences = new List<ISentence<V>>();
        }

        public ComplexSentence(string name, int arity, IList<ISentence<V>> subSentences, IOperator<V> @operator, IDictionary<int, IList<int>> parametersMapping = null)
            : this(name, arity, @operator)
        {
            this.SubSentences = subSentences;
            this.ImplicitVariables = new List<IVariable<V>>();
            if (parametersMapping != null) this.ParametersMapping = parametersMapping;
        }

        # endregion Constructor

        # region Processing methods

        public void AddSubSentenceWithMapping(ISentence<V> subSentence, IList<int> mappings)
        {
            foreach (var mappingIdx in mappings)
            {
                if(mappingIdx > this.Arity) throw new InvalidMappingIndexException(this.Arity, mappingIdx);
                if (mappingIdx < 0)
                {
                    int implicitVariableIdx = this.ResolveImplicitVariableIndex(mappingIdx);
                    if (implicitVariableIdx >= this.ImplicitVariablesCount)
                    {
                        foreach (var i in Enumerable.Range(this.ImplicitVariablesCount, implicitVariableIdx - this.ImplicitVariablesCount + 1))
                        {
                            this.ImplicitVariables.Add(new Variable<V>(string.Format(IMPLICIT_VARIABLE_NAME, this.Name, i), default(V)));
                        }
                    }
                }
            }

            this.SubSentences.Add(subSentence);
            int subSentenceIndex = (this.SubSentences.Count - 1);

            if(!this.ParametersMapping.ContainsKey(subSentenceIndex)) this.ParametersMapping.Add(subSentenceIndex, mappings);
        }

        public ISentence<V> Negate()
        {
            var negatedSentences = this.Operator.NegateSentences(this.SubSentences);
            var negatedOperator = this.Operator.Negate();

            //TODO: add automatic name negation
            return new ComplexSentence<V>(this.Name, this.Arity, negatedSentences, negatedOperator, new Dictionary<int, IList<int>>(this.ParametersMapping));
        }

        # endregion Processing methods

        # region Equality methods

        public bool Equals(IComplexSentence<V> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!this.SubSentences.All(other.SubSentences.Contains)) return false;
            if (!this.ImplicitVariables.All(other.ImplicitVariables.Contains)) return false;
            foreach (var paramMapping in this.ParametersMapping)
            {
                if (!other.ParametersMapping.ContainsKey(paramMapping.Key)) return false;
                if (!paramMapping.Value.SequenceEqual(other.ParametersMapping[paramMapping.Key])) return false;
            }
            return this.Operator.Equals(other.Operator) && this.Name.Equals(other.Name) &&
                   this.Arity.Equals(other.Arity);
        }

        public bool Equals(ISentence<V> other)
        {
            return this.Equals(other as IComplexSentence<V>);
        }

        public bool Equals(ComplexSentence<V> other)
        {
            return this.Equals(other as IComplexSentence<V>);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ComplexSentence<V>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (SubSentences != null ? SubSentences.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Operator != null ? Operator.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Arity;
                foreach (var implicitVariable in this.ImplicitVariables)
                {
                    hashCode = (hashCode * 397) ^ implicitVariable.GetHashCode();
                }
                foreach (var subSentence in this.SubSentences)
                {
                    hashCode = (hashCode * 397) ^ subSentence.GetHashCode();
                }
                foreach (var paramsMapping in this.ParametersMapping)
                {
                    hashCode = (hashCode * 397) ^ paramsMapping.Key.GetHashCode();
                    foreach (var value in paramsMapping.Value)
                    {
                        hashCode = (hashCode * 397) ^ value.GetHashCode();
                    }
                }
                return hashCode;
            }
        }

        # endregion Equality methods

        # region Helper methods

        public int ResolveImplicitVariableIndex(int negativeIndex)
        {
            return -1 *(negativeIndex + 1);
        }

        # endregion Helper methods
    }
}
