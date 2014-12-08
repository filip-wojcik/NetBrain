using System.Linq;
using NetBrain.Abstracts.MachineLearning.Algorithms.FrequentItems.Models;
using NetBrain.Utils.CollectionExtensions;
using System.Collections.Generic;

namespace NetBrain.Defaults.MachineLearning.Algorithms.FrequentItems
{
    public class AprioriAlgorithm<T> : IFrequentItemsFinder<T>, IAssociationsFinder<T>
    {
        public double SupportThreshold { get; private set; }
        public double ConfidenceThreshold { get; private set; }


        public AprioriAlgorithm(double supportThreshold = 0.5, double confidenceThreshold = 0.85)
        {
            SupportThreshold = supportThreshold;
            ConfidenceThreshold = confidenceThreshold;
        }

        public static IFrequentItemsFinder<T> AprioriAlgorithmAsFrequentItemsFinderFactory(double thresholdValue)
        {
            return new AprioriAlgorithm<T>(thresholdValue);
        }

        public static IAssociationsFinder<T> AprioriAlgorithmAsAssociationFinderFactoryFactory(double thresholdValue)
        {
            return new AprioriAlgorithm<T>(confidenceThreshold: thresholdValue);
        }

        # region Frequent items finding

        public IEnumerable<IFrequentItemsSet<T>> FindFrequentItemsSets(IEnumerable<IEnumerable<T>> transactions)
        {
            int longestTransactionFound = 1;
            IEnumerable<IFrequentItemsSet<T>> initialFrequentItemsSet = this.FindInitialFrequentItems(transactions, out longestTransactionFound);
            if (!initialFrequentItemsSet.Any()) return initialFrequentItemsSet;

            var frequentItemsOfGivenLength = new Dictionary<int, IEnumerable<IFrequentItemsSet<T>>>()
            {
                {1, initialFrequentItemsSet}
            };
            
            for(int i = 2; i < longestTransactionFound; i++)
            {
                IEnumerable<ISet<T>> combinedItems = this.CombineAndBuildNewItemSets(frequentItemsOfGivenLength[i - 1], i);
                if (!combinedItems.Any()) break;
                IEnumerable<IFrequentItemsSet<T>> newFrequentItems = this.SelectFrequentItems(combinedItems,
                    transactions);
                if (!newFrequentItems.Any()) break;
                frequentItemsOfGivenLength.Add(i, newFrequentItems);
            }

            return frequentItemsOfGivenLength.Values.SelectMany(itemsSet => itemsSet);
        }

        public IEnumerable<IFrequentItemsSet<T>> FindInitialFrequentItems(IEnumerable<IEnumerable<T>> transactions, out int longestTransactionFound)
        {
            var presenceCounters = new Dictionary<T, double>();
            int transactionsCount = 0;
            longestTransactionFound = 0;
            foreach (var transaction in transactions)
            {
                if (transaction.Count() > longestTransactionFound) longestTransactionFound = transaction.Count();
                foreach (var item in transaction)
                {
                    presenceCounters.AddKeyIfNotExists(item);
                    presenceCounters[item] += 1;
                }
                transactionsCount += 1;
            }

            var frequentItems = new HashSet<IFrequentItemsSet<T>>();
            foreach (var itemAndPresence in presenceCounters)
            {
                double supportValue = itemAndPresence.Value / (double)transactionsCount;
                if (supportValue >= this.SupportThreshold)
                {
                    frequentItems.Add(new FrequentItemsSet<T>(supportValue, itemAndPresence.Key));
                }
            }
            return frequentItems;
        }

        public IEnumerable<IFrequentItemsSet<T>> SelectFrequentItems(IEnumerable<ISet<T>> candidateItemsSets,
            IEnumerable<IEnumerable<T>> transactions)
        {
            var presenceCounters = new Dictionary<ISet<T>, double>();
            int transactionsCount = 0;
            foreach (var transaction in transactions)
            {
                foreach (var candidateItemsSet in candidateItemsSets)
                {
                    if (candidateItemsSet.IsSubsetOf(transaction))
                    {
                        presenceCounters.AddKeyIfNotExists(candidateItemsSet);
                        presenceCounters[candidateItemsSet] += 1;
                    }
                }
                transactionsCount += 1;
            }

            var frequentItems = new HashSet<IFrequentItemsSet<T>>();
            foreach (var itemAndPresence in presenceCounters)
            {
                double supportValue = itemAndPresence.Value / (double)transactionsCount;
                if (supportValue >= this.SupportThreshold)
                {
                    frequentItems.Add(new FrequentItemsSet<T>(itemAndPresence.Key, supportValue));
                }
            }
            return frequentItems;
        }

        public IEnumerable<ISet<T>> CombineAndBuildNewItemSets(
            IEnumerable<IFrequentItemsSet<T>> currentItems, int desiredLength)
        {
            var itemsSets = new HashSet<ISet<T>>();
            for (int i = 0; i < currentItems.Count(); i++)
            {
                for (int j = i + 1; j < currentItems.Count(); j++)
                {
                    IEnumerable<T> combinedItems = currentItems.ElementAt(i).Items.Union(currentItems.ElementAt(j).Items);
                    if (combinedItems.Count() == desiredLength)
                    {
                        itemsSets.Add(new HashSet<T>(combinedItems));
                    }
                }
            }
            return itemsSets;
        }

        # endregion Frequent items finding

        # region Association rules finding

        public IEnumerable<IAssociationRule<T>> FindAssociationRules(IEnumerable<IFrequentItemsSet<T>> frequentItems)
        {
            var dictionaryOfItems = new Dictionary<HashSet<T>, double>(HashSet<T>.CreateSetComparer());
            var frequentItemsGroupedByItemsLength = new Dictionary<int, IList<IFrequentItemsSet<T>>> ();
            RegroupFrequentItems(frequentItems, dictionaryOfItems, frequentItemsGroupedByItemsLength);
            var associationRules = new HashSet<IAssociationRule<T>>();
            for (int i = 2; i < frequentItemsGroupedByItemsLength.Keys.Max() + 1; i++)
            {
                foreach (
                    var associationRule in
                        this.FindAssociationRules(i, dictionaryOfItems, frequentItemsGroupedByItemsLength))
                {
                    if (associationRule != null)
                    {
                        yield return associationRule;
                    }
                }
            }
        }

        public IEnumerable<IAssociationRule<T>> FindAssociationRules(
            int itemsLengthToProcess,
            IDictionary<HashSet<T>, double> frequentItems,
            IDictionary<int, IList<IFrequentItemsSet<T>>> frequentItemsGroupedByLength
            
            )
        {
            foreach (var frequentItemsSet in frequentItemsGroupedByLength[itemsLengthToProcess])
            {
                for (int combinationLength = 1; combinationLength < itemsLengthToProcess; combinationLength++)
                {
                    foreach (var antecedentCombination in frequentItemsSet.Items.FindAllCombinatonsOfLegth(combinationLength))
                    {
                        var consequentCombination = new HashSet<T>(frequentItemsSet.Items.Except(antecedentCombination));
                        double confidence = this.CalculateConfidence(antecedentCombination, consequentCombination,
                            frequentItems);
                        if (confidence >= this.ConfidenceThreshold)
                        {
                            yield return this.BuildAssociationRule(
                                antecedentCombination, consequentCombination, confidence,
                                frequentItemsGroupedByLength.Values.SelectMany(values => values));
                        }
                    }
                }
            }
        }

        public double CalculateConfidence(ISet<T> antecedent, ISet<T> consequent, IDictionary<HashSet<T>, double> frequentItems)
        {
            var antecedentAndConsequent = new HashSet<T>(antecedent.Union(consequent));
            double supportOfAntecedentAndConsequent = 0.0;
            if (frequentItems.TryGetValue(antecedentAndConsequent, out supportOfAntecedentAndConsequent))
            {
                double supportOfAntecedent = 0;
                if (frequentItems.TryGetValue(new HashSet<T>(antecedent), out supportOfAntecedent))
                {
                    //Equivalent of P(A|B) = P(A && B) / P(B)
                    return supportOfAntecedentAndConsequent / supportOfAntecedent;
                }
                return 0;
            }
            return 0;
        }

        private IAssociationRule<T> BuildAssociationRule(ISet<T> antecedentItems, ISet<T> consequentItems, double confidence,
            IEnumerable<IFrequentItemsSet<T>> frequentItems)
        {
            IFrequentItemsSet<T> antecedent = null;
            IFrequentItemsSet<T> consequent = null;
            bool antecedentFound = false;
            bool consequentFound = false;
            foreach (var frequentItemsSet in frequentItems)
            {
                if (antecedentFound && consequentFound) break;
                if (frequentItemsSet.Items.SetEquals(antecedentItems))
                {
                    antecedentFound = true;
                    antecedent = frequentItemsSet;
                    continue;
                }
                else if (frequentItemsSet.Items.SetEquals(consequentItems))
                {
                    consequentFound = true;
                    consequent = frequentItemsSet;
                    continue;
                }
            }
            return new AssociationRule<T>(antecedent, consequent, confidence);
        }

        private static void RegroupFrequentItems(IEnumerable<IFrequentItemsSet<T>> frequentItems, Dictionary<HashSet<T>, double> dictionaryOfItems,
            Dictionary<int, IList<IFrequentItemsSet<T>>> frequentItemsGroupedByItemsLength)
        {
            foreach (var frequentItemSet in frequentItems)
            {
                int itemsCount = frequentItemSet.Items.Count;
                dictionaryOfItems.Add(new HashSet<T>(frequentItemSet.Items), frequentItemSet.SupportValue);
                if (!frequentItemsGroupedByItemsLength.ContainsKey(itemsCount))
                    frequentItemsGroupedByItemsLength.Add(itemsCount, new List<IFrequentItemsSet<T>>());
                frequentItemsGroupedByItemsLength[itemsCount].Add(frequentItemSet);
            }
        }

        # endregion Association rules findings

        
    }
}
