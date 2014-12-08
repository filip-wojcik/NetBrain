namespace NetBrain.Abstracts.MachineLearning.MathematicalFunctions.Regression
{
    using Models.Data;
    using System.Collections.Generic;

    /// <summary>
    /// Function used in linear regression for calculating error of model
    /// </summary>
    public interface IRegressionFunction
    {
        /// <summary>
        /// Calculates outputs given input vector and weights
        /// </summary>
        /// <param name="input">Input vector</param>
        /// <param name="weights">Weights vector</param>
        /// <returns>Single output</returns>
        double CalculateOutput(IList<double> input, IList<double> weights);

        /// <summary>
        /// Calculates outputs from the whole data set, given weights
        /// </summary>
        /// <param name="data">Data set</param>
        /// <param name="weights">Weights vector</param>
        /// <returns>Outputs for every vector in data set</returns>
        IList<double> CalculateOutput(ISingleValueDataSet<double> data, IList<double> weights);

        /// <summary>
        /// Calculates error with respect to subsequent weights
        /// </summary>
        /// <param name="expected">Expected values</param>
        /// <param name="actual">Actual values</param>
        /// <param name="weightsCount">Count of weights used in calculation</param>
        /// <param name="dataSet">Data set used for calculations</param>
        /// <returns>Erros calculated for every weights</returns>
        IList<double> CalculateDerivativeWithRespectToWeights(IList<double> expected, IList<double> actual, IDataSet<double> dataSet, int weightsCount);

    }
}
