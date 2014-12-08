using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Models;
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Trainers
{
    public interface ITrainingStrategy
    {
        double TrainingDataPercent { get; }
        int Iterations { get; }
        double StoppingError { get; }
        bool Break { get; set; }
        void Train(INeuralNetwork neuralNetwork, IDataSet<double> testData, INeuralNetworkTrainer trainer);
    }
}
