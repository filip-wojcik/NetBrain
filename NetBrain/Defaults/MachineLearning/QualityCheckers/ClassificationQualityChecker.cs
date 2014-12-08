using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using NetBrain.Abstracts.MachineLearning.QualityCheckers;
using NetBrain.Utils.CollectionExtensions;

namespace NetBrain.Defaults.MachineLearning.QualityCheckers
{
    public class ClassificationQualityChecker<T> : IClassificationQualityChecker<T>
    {
        public double AccuracyRate(IEnumerable<IExpectedActualPair<T>> outcomes)
        {
            uint totalElementsCount = 0;
            uint correctClassifications = 0;
            foreach (var outcome in outcomes)
            {
                totalElementsCount++;
                if (outcome.ActualOutcome.SequenceEqual(outcome.ExpectedOutcome))
                {
                    correctClassifications++;
                }
            }
            return correctClassifications/(double)totalElementsCount;
        }

        public double AccuracyRate(IContingencyTable<T> contingencyTable)
        {
            long totalCount = contingencyTable.RealClassesCounts.Sum(num => num);
            uint positiveClassifications = 0;
            for (int i = 0; i < contingencyTable.Classes.Count; i++)
            {
                positiveClassifications += contingencyTable.ContingencyValues[i, i];
            }
            return positiveClassifications / (double)totalCount;
        }

        public IContingencyTable<T> BuildContingencyTable(IEnumerable<IExpectedActualPair<T>> outcomes)
        {
            var classes = new List<IList<T>>();
            var classificationsCounts = new Dictionary<int, Dictionary<int, uint>>();
            var realClassesCount = new Dictionary<int, uint>();
            foreach (var outcome in outcomes)
            {
                ProcessOutcome(outcome, classes, realClassesCount, classificationsCounts);
            }

            var contingencyTable = new uint[classes.Count, classes.Count];
            FillConsistencyTable(classes, classificationsCounts, contingencyTable);

            return new ContingencyTable<T>()
            {
                Classes = classes,
                ContingencyValues = contingencyTable,
                RealClassesCounts = realClassesCount.Values.ToList()
            };
        }

        private static void FillConsistencyTable(List<IList<T>> classes, Dictionary<int, Dictionary<int, uint>> classificationsCounts, uint[,] contingencyTable)
        {
            for (int i = 0; i < classes.Count; i++)
            {
                for (int j = 0; j < classes.Count; j++)
                {
                    uint classificationCount = 0;
                    classificationsCounts[i].TryGetValue(j, out classificationCount);
                    contingencyTable[i, j] = classificationCount;
                }
            }
        }

        private static void ProcessOutcome(IExpectedActualPair<T> outcome, List<IList<T>> classes, Dictionary<int, uint> realClassesCount,
            Dictionary<int, Dictionary<int, uint>> classificationsCounts)
        {
            var expectedClass = outcome.ExpectedOutcome;
            var actualClass = outcome.ActualOutcome;

            int expectedClassIdx = SearchClassIdxOrAdd(classes, expectedClass);
            realClassesCount.AddKeyIfNotExists(expectedClassIdx);
            realClassesCount[expectedClassIdx]++;

            if (expectedClass.SequenceEqual(actualClass))
            {
                IncrementMultiLevelDictionary(classificationsCounts, expectedClassIdx, expectedClassIdx);
            }
            else
            {
                int actualClassIdx = SearchClassIdxOrAdd(classes, actualClass);
                IncrementMultiLevelDictionary(classificationsCounts, expectedClassIdx, actualClassIdx);
            }
        }

        private static int SearchClassIdxOrAdd(List<IList<T>> classes, IList<T> currentClass)
        {
            int currentClassIdx = classes.FindIndex(knownClass => knownClass.SequenceEqual(currentClass));
            if (currentClassIdx < 0)
            {
                currentClassIdx = classes.Count;
                classes.Add(currentClass);
            }
            return currentClassIdx;
        }

        private static void IncrementMultiLevelDictionary(
            IDictionary<int, Dictionary<int, uint>> classedDict, 
            int keyLevel1,
            int keyLevel2)
        {
            classedDict.AddKeyIfNotExists(keyLevel1);
            classedDict[keyLevel1].AddKeyIfNotExists(keyLevel2);
            classedDict[keyLevel1][keyLevel2]++;
        }

        public double ErrorRate(IEnumerable<IExpectedActualPair<T>> outcomes)
        {
            return 1.0 - this.AccuracyRate(outcomes);
        }

        public IQualityData MeasureQualityData(IEnumerable<IExpectedActualPair<T>> outcomes, int iterationNumber, bool testData)
        {
            IContingencyTable<T> contingencyTable = this.BuildContingencyTable(outcomes);
            double accuracy = this.AccuracyRate(contingencyTable);

            return new ClassificationDataQuality<T>()
            {
                ContingencyTable = this.BuildContingencyTable(outcomes),
                ErrorRate = 1 - accuracy,
                Accuracy = accuracy,
                Iteration = iterationNumber,
                TestData = testData
            };
        }
    }
}
