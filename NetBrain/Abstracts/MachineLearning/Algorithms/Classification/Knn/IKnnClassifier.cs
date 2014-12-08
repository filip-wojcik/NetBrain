namespace NetBrain.Abstracts.MachineLearning.Algorithms.Classification.Knn
{
    using MathematicalFunctions;
    using Models.Data;

    public delegate IKnnClassifier KnnClassifierFactory(DistanceCalculator distanceCalculator, int kNeighbors);

    public interface IKnnClassifier
    {
        DistanceCalculator DisaCalculator { get; }
        int KNeighbors { get; }

        double Classify(ISingleValueFeatureVector<double> vectorToClassify, ISingleValueDataSet<double> dataSet);
    }
}
