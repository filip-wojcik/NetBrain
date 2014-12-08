using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.Common.Models;

namespace NetBrain.Defaults.Common.Models
{
    public class VariableDomain<TValueType> : IVariableDomain<TValueType>
    {
        public IVariable<TValueType> Variable { get; private set; }
        public IList<TValueType> AllowedValues { get; private set; }

        public static IVariableDomain<TValueType> DefaultFactory(IVariable<TValueType> variable, IList<TValueType> allowedValues)
        {
            return new VariableDomain<TValueType>(variable, allowedValues);
        }

        public bool IsEmpty
        {
            get { return this.AllowedValues.Count == 0; }
        }

        public VariableDomain(IVariable<TValueType> variable, IList<TValueType> allowedValues)
        {
            Variable = variable;
            AllowedValues = allowedValues;
        }

        public void RemoveAllowedValue(TValueType value)
        {
            if(this.AllowedValues.Contains(value)) this.AllowedValues.Remove(value);
        }

        public void AddAllowedValue(TValueType value)
        {
            this.AllowedValues.Add(value);
        }

        public IVariableDomain<TValueType> Clone()
        {
            return new VariableDomain<TValueType>(this.Variable.Clone(), new List<TValueType>(this.AllowedValues));
        }
    }
}
