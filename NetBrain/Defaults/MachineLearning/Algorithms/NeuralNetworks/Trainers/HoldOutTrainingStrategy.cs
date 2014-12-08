using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;
using NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Models;
using NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Trainers;
using NetBrain.Abstracts.MachineLearning.Models.Data;
using NetBrain.Abstracts.MachineLearning.Models.Data.QueryExtensions;
using NetBrain.Utils.CollectionExtensions;

namespace NetBrain.Defaults.MachineLearning.Algorithms.NeuralNetworks.Trainers
{
    public class HoldOutTrainingStrategy : ITrainingStrategy
    {
        public double TrainingDataPercent { get; private set; }
        public int Iterations { get; private set; }
        public double StoppingError { get; private set; }
        public bool Break { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public HoldOutTrainingStrategy(double trainingDataPercent, int iterations, double stoppingError, CancellationToken cancellationToken)
        {
            TrainingDataPercent = trainingDataPercent;
            Iterations = iterations;
            StoppingError = stoppingError;
            CancellationToken = cancellationToken;
        }

        public void Train(INeuralNetwork neuralNetwork, IDataSet<double> testData, INeuralNetworkTrainer trainer)
        {
            var vectorIndexes = Enumerable.Range(0, testData.Count()).ToList();
            vectorIndexes.Schuffle();
            int trainingDataCount = Convert.ToInt32(vectorIndexes.Count*this.TrainingDataPercent);
            var indexesToTake = vectorIndexes.Take(trainingDataCount);
            Tuple<IList<IFeatureVector<double>>, IList<IFeatureVector<double>>> trainingAndTestData =
                this.DivideData(indexesToTake, testData);

            for (int i = 0; i < this.Iterations; i++)
            {
                if (this.CancellationToken.IsCancellationRequested || this.Break) break;
                trainer.ProcessTrainingData(neuralNetwork, trainingAndTestData.Item1, i);
            }

            trainer.ProcessTestData(neuralNetwork, trainingAndTestData.Item2, this.Iterations + 1);
        }

        protected Tuple<IList<IFeatureVector<double>>, IList<IFeatureVector<double>>> DivideData(
            IEnumerable<int> trainingIndexes, IDataSet<double> data)
        {
            var trainingData = new List<IFeatureVector<double>>();
            var testData = new List<IFeatureVector<double>>();
            for (int i = 0; i < data.Count(); i++)
            {
                if (trainingIndexes.Contains(i)) trainingData.Add(data.ElementAt(i));
                else testData.Add(data.ElementAt(i));
            }
            return new Tuple<IList<IFeatureVector<double>>, IList<IFeatureVector<double>>>(trainingData, testData);
        }
    }
}
