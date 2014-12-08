namespace NetBrain.Defaults.MachineLearning.QualityCheckers
{
    using Abstracts.MachineLearning.QualityCheckers;
    using System.Collections.Generic;

    public class ContingencyTable<T> : IContingencyTable<T>
    {
        public IList<IList<T>> Classes { get; set; }
        public IList<uint> RealClassesCounts { get; set; } 
        public uint[,] ContingencyValues { get; set; }
    }
}
