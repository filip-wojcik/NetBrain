namespace NetBrain.Abstracts.MachineLearning.QualityCheckers
{
    using System.Collections.Generic;

    public interface IContingencyTable<T>
    {
        IList<IList<T>> Classes { get; }
        IList<uint> RealClassesCounts { get; set; } 
        uint[,] ContingencyValues { get; set; }
    }
}
