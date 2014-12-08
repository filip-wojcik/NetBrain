using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Utils;
using NetBrain.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Classification.DecisionTrees
{
    public class DecisionTreeClassifierStub<T, V> : DecisionTreeClassifier<T, V>
    {
        public DecisionTreeClassifierStub(TrueEquivalent<V> trueEquivalent, FalseEquivalent<V> falseEquivalent, IsValueApplicable<T, V> applicabilityChecker) : base(trueEquivalent, falseEquivalent, applicabilityChecker)
        {
        }
    }
}
