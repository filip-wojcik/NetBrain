using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.FrequentItems
{
    public static class AprioriAlgorithmTestDataBuilder
    {
        public static IList<IEnumerable<int>> BuildAbstratTestData()
        {
            return new List<IEnumerable<int>>()
            {
                new int[3]{ 1, 3, 4 },
                new int[3]{ 2, 3, 5},
                new int[4]{ 1, 2, 3, 5 },
                new int[2]{ 2, 5 }
            };
        }

        public static IDataSet<string> BuildGamesTestData()
        {
            return new DataSet<string>(new string[] {"theme", "perspective", "platform", "producer"}, 4,
                new int[0],
                new List<IFeatureVector<string>>()
                {
                    new FeatureVector<string>(new string[4] {"SF", "izometric", "PC", "EAGames"}),
                    new FeatureVector<string>(new string[4] {"modern", "FPP", "console", "Call of Duty"}),
                    new FeatureVector<string>(new string[4] {"modern", "FPP", "console", "Battlefield"}),
                    new FeatureVector<string>(new string[4] {"SF", "TPP", "PC", "Dead space"}),
                    new FeatureVector<string>(new string[4] {"SF", "TPP", "PC", "Watch dogs"})
                });
        } 
    }
}
