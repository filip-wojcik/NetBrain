namespace NetBrain.Defaults.MachineLearning.QualityCheckers
{
    using Abstracts.MachineLearning.QualityCheckers;

    public struct NumericDataQuality : IQualityData
    {
        public int Iteration { get; set; }
        public double ErrorRate { get; set; }
        public bool TestData { get; set; }
    }
}
