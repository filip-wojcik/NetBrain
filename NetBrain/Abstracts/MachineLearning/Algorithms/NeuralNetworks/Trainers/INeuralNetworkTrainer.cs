namespace NetBrain.Abstracts.MachineLearning.Algorithms.NeuralNetworks.Trainers
{
    using System.Collections.Generic;
    using Models;
    using MachineLearning.Models.Data;
    using QualityCheckers;

    /// <summary>
    /// Trainer for neural networks, which can use different training options.
    /// </summary>
    public interface INeuralNetworkTrainer
    {
        /// <summary>
        /// General training stragegy - responsible for partitioning data for iterations
        /// </summary>
        ITrainingStrategy TrainingStrategy { get; set; }

        /// <summary>
        /// Monitors error changes during iterations
        /// </summary>
        IQualityChecker<double> QualityChecker { get; set; }

        /// <summary>
        /// Neural network output interpreter - depends on the task being solved
        /// </summary>
        ISolutionInterpreter SolutionInterpreter { get; set; }

        /// <summary>
        /// Reports error changes
        /// </summary>
        IQualityCheckLogger Logger { get; set; }

        /// <summary>
        /// Performs neural network training
        /// </summary>
        /// <param name="neuralNetwork">Neural network to train</param>
        /// <param name="dataSet">Data set to operate on</param>
        void Train(INeuralNetwork neuralNetwork, IDataSet<double> dataSet);

        /// <summary>
        /// Performs processing on training data - it involves changing neural network internal weights
        /// </summary>
        /// <param name="neuralNetwork">Neural network to be tested</param>
        /// <param name="trainingData">Training data</param>
        /// <param name="iteration">Iteration number (necessary for logging)</param>
        void ProcessTrainingData(INeuralNetwork neuralNetwork, IList<IFeatureVector<double>> trainingData, int iteration);

        /// <summary>
        /// Performs processing on test data - training data DOES NOT involve updating neural network weights!
        /// </summary>
        /// <param name="neuralNetwork">Neural network to be tested</param>
        /// <param name="testData">Test data</param>
        /// <param name="iteration">Iteration number (necessary for logging)</param>
        void ProcessTestData(INeuralNetwork neuralNetwork, IList<IFeatureVector<double>> testData, int iteration);

    }
}
