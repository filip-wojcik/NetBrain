using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Data.QueryExtensions
{
    internal static class QueryableTestData
    {
        public static IDataSet<object> AbstactMixedDataSet()
        {
            return new DataSet<object>(
                new []{ "name", "surname", "income", "height"},
                4, new int[0], new List<IFeatureVector<object>>()
                {
                    new FeatureVector<object>(new object[]{ "Johnny", "First", 10000, 181 }),
                    new FeatureVector<object>(new object[]{ "Max", "Second", 1500, 150 }),
                    new FeatureVector<object>(new object[]{ "Johnny", "Third", 2000, 171 }),
                    new FeatureVector<object>(new object[]{ "Greg", "Fourth", 3300, 190 }),
                    new FeatureVector<object>(new object[]{ "Max", "Fifth", 250, 167 }),
                });
        } 
    }
}
