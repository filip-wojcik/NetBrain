using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;

namespace NetBrain.Utils.CollectionExtensions
{
    public static class CollectionsExtensions
    {
        public static void AddKeyIfNotExists<K, V>(this IDictionary<K, V> dictionary, K key)
            where V : new()
        {
            if(!(dictionary.ContainsKey(key))) dictionary.Add(key, new V());
        }

        public static void Schuffle<T>(this IList<T> elements, Random random = null)
        {
            var randomizer = random ?? new Random();
            for (int i = 0; i < elements.Count; i++)
            {
                int newRandomIndex = randomizer.Next(0, elements.Count);
                T elementUnderNewIndex = elements[newRandomIndex];
                elements[newRandomIndex] = elements[i];
                elements[i] = elementUnderNewIndex;
            }
        }

        public static IEnumerable<ISet<T>> FindAllCombinatonsOfLegth<T>(this IEnumerable<T> items, int combinationLength)
        {
            if(items == null || combinationLength < 0 || combinationLength > items.Count()) throw new ArgumentException();
            for (int i = 0; i < (items.Count() - combinationLength + 1); i++)
            {
                var currentCombination = new HashSet<T>() { items.ElementAt(i) };
                foreach (
                    var combination in
                        FindAllCombinatonsOfLegth<T>(items, combinationLength - 1, i + 1, currentCombination))
                {
                    yield return combination;
                }
            }
        }

        static IEnumerable<ISet<T>> FindAllCombinatonsOfLegth<T>(
            IEnumerable<T> items, 
            int combinationLength, 
            int currentIndex,
            HashSet<T> currentCombination)
        {
            if (combinationLength == 0) yield return currentCombination;
            else if(items.Count() - currentIndex >= combinationLength)
            {
                for (int i = currentIndex; i < (items.Count() - combinationLength + 1); i++)
                {
                    var nextCombination = new HashSet<T>(currentCombination);
                    nextCombination.Add(items.ElementAt(i));
                    foreach (
                        var combination in
                            FindAllCombinatonsOfLegth<T>(items, combinationLength - 1, i + 1, nextCombination))
                    {
                        yield return combination;
                    }
                }
                
            }
        }
    }
}
