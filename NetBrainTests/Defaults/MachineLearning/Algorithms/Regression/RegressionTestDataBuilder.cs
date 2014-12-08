using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Defaults.MachineLearning.Data.Models;

namespace NetBrainTests.Defaults.MachineLearning.Algorithms.Regression
{
    internal class RegressionTestDataBuilder
    {
        public ISingleValueDataSet<double> TrainingDataSet { get; private set; }
        public ISingleValueDataSet<double> TestDataSet { get; private set; }
        public IList<double> IdealTestOutputs { get; private set; } 

        public RegressionTestDataBuilder()
        {
            BuildTrainingDataSet();
            BuildTestDataSet();
            BuildIdealOutputs();
        }

        private void BuildTrainingDataSet()
        {
            this.TrainingDataSet = new SingleValueDataSet<double>(
                new[] { "f1", "f2", "f3" }, 4, 3, new List<IFeatureVector<double>>
                {
                    new SingleValueFeatureVector<double>(new double[] {1.0, 0.18, 0.89, 109.85}),
                    new SingleValueFeatureVector<double>(new double[] {1.0, 1.0, 0.26, 155.72}),
                    new SingleValueFeatureVector<double>(new double[] {1.0, 0.92, 0.11, 137.66}),
                    new SingleValueFeatureVector<double>(new double[] {1.0, 0.07, 0.37, 76.17}),
                    new SingleValueFeatureVector<double>(new double[] {1.0, 0.85, 0.16, 139.75}),
                    new SingleValueFeatureVector<double>(new double[] {1.0, 0.99, 0.41, 162.6}),
                    new SingleValueFeatureVector<double>(new double[] {1.0, 0.87, 0.47, 151.77})
                });
        }

        private void BuildTestDataSet()
        {
            this.TestDataSet = new SingleValueDataSet<double>(
                new[] { "f1", "f2" }, 3, 2, new List<IFeatureVector<double>>
                {
                    new SingleValueFeatureVector<double>(new double[] { 1.0, 0.49, 0.18 }),
                    new SingleValueFeatureVector<double>(new double[] { 1.0, 0.57, 0.83 }),
                    new SingleValueFeatureVector<double>(new double[] { 1.0, 0.56, 0.64 }),
                    new SingleValueFeatureVector<double>(new double[] { 1.0, 0.76, 0.18 })
                });
        }

        private void BuildIdealOutputs()
        {
            this.IdealTestOutputs = new double[] { 105.22, 142.68, 132.94, 129.71 };
        }
    }
}
