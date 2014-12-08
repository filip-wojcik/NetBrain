using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Utils.CollectionExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetBrain.Utils.CollectionExtensions.Tests
{
    [TestClass()]
    public class CollectionsExtensionsTests
    {
        [TestMethod()]
        public void SchuffleTest()  
        {
            //Given
            var initialNumbers = Enumerable.Range(0, 10).ToArray();
            var schuffledNumbers = Enumerable.Range(0, 10).ToArray();

            //When
            schuffledNumbers.Schuffle();

            //Then
            Assert.AreEqual(10, schuffledNumbers.Count());
            Assert.AreEqual(schuffledNumbers.Count(), schuffledNumbers.Distinct().Count());
            Assert.IsTrue(initialNumbers.All(schuffledNumbers.Contains));
            Assert.IsFalse(initialNumbers.SequenceEqual(schuffledNumbers));
        }

        [TestMethod()]
        public void FindAllCombinatonsOfLegthTest_Length1()
        {
            //Given
            var items = new HashSet<string>() { "A", "B", "C", "D" };
            var expectedCombinations = new List<HashSet<string>>()
            {
                new HashSet<string>(){ "A" },
                new HashSet<string>(){ "B" },
                new HashSet<string>(){ "C" },
                new HashSet<string>(){ "D" },
            };

            //When
            IEnumerable<ISet<string>> actualCcombinations = items.FindAllCombinatonsOfLegth(1);

            //Then
            Assert.AreEqual(4, expectedCombinations.Count());
            Assert.IsTrue(
                expectedCombinations.TrueForAll(expectedCombination => actualCcombinations.Any(actualCombination => actualCombination.SetEquals(expectedCombination)))
                );
        }

        [TestMethod()]
        public void FindAllCombinatonsOfLegthTest_Length2()
        {
            //Given
            var items = new HashSet<string>() { "A", "B", "C", "D" };
            var expectedCombinations = new List<HashSet<string>>()
            {
                new HashSet<string>(){ "A", "B" },
                new HashSet<string>(){ "A", "C" },
                new HashSet<string>(){ "A", "D" },
                new HashSet<string>(){ "B", "C" },
                new HashSet<string>(){ "B", "D" },
                new HashSet<string>(){ "C", "D" },
            };

            //When
            IEnumerable<ISet<string>> actualCcombinations = items.FindAllCombinatonsOfLegth(2);

            //Then
            Assert.AreEqual(6, actualCcombinations.Count());
            Assert.IsTrue(
                expectedCombinations.TrueForAll(expectedCombination => actualCcombinations.Any(actualCombination => actualCombination.SetEquals(expectedCombination)))
                );
        }

        [TestMethod()]
        public void FindAllCombinatonsOfLegthTest_Length3()
        {
            //Given
            var items = new HashSet<string>() { "A", "B", "C", "D" };
            var expectedCombinations = new List<HashSet<string>>()
            {
                new HashSet<string>(){ "A", "B", "C" },
                new HashSet<string>(){ "A", "B", "D" },
                new HashSet<string>(){ "A", "C", "D" },
                new HashSet<string>(){ "B", "C", "D" },
            };

            //When
            IEnumerable<ISet<string>> actualCcombinations = items.FindAllCombinatonsOfLegth(3);

            //Then
            Assert.AreEqual(4, actualCcombinations.Count());
            Assert.IsTrue(
                expectedCombinations.TrueForAll(expectedCombination => actualCcombinations.Any(actualCombination => actualCombination.SetEquals(expectedCombination)))
                );
        }
    }
}
