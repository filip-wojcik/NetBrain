using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Models;
using NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Utils;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.Classification.DecisionTrees.Evaluators
{
    public delegate bool IsValueApplicable<T, V>(T vectorValue, V childLinkValue);

    public interface IDecisionTreeClassifier<T, V>
    {
        IsValueApplicable<T, V> ApplicabilityChecker { get; }
        TrueEquivalent<V> TrueEquivalent { get; } 
        FalseEquivalent<V> FalseEquivalent { get; } 
        
        T Classify(ISingleValueFeatureVector<T> vector, IDecisionTree<T, V> decisionTree);
    }
}
