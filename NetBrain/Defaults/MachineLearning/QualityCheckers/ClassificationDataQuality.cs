namespace NetBrain.Defaults.MachineLearning.QualityCheckers
{
    using Abstracts.MachineLearning.QualityCheckers;

    public class ClassificationDataQuality<T> : IClassificationQualityData<T>
    {
        public IContingencyTable<T> ContingencyTable { get; set; }
        public int Iteration { get; set; }
        public double ErrorRate { get; set; }
        public double Accuracy { get; set; }
        public bool TestData { get; set; }
    }
}
