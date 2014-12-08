
using NetBrain.Abstracts.MachineLearning.Models.Data;

namespace NetBrain.Abstracts.MachineLearning.Algorithms.Regression
{
    using MathematicalFunctions;
    using MathematicalFunctions.Regression;
    using System.Collections.Generic;

    /// <summary>
    /// Performs linear regression on a given data set, using some regression function
    /// </summary>
    public interface IRegressor
    {
        /// <summary>
        /// Regression function which will be used in process
        /// </summary>
        IRegressionFunction RegressionFunction { get; }

        /// <summary>
        /// Function for checking the error associated with predictions
        /// </summary>
        StatisticalFunctions.NumericErrorChecker NumericErrorChecker { get; }

        /// <summary>
        /// Performs regression on a given data set - tries to find weights which best fit the data
        /// </summary>
        /// <param name="dataToOperate">Data on which regression will be performed</param>
        /// <returns>Set of weights</returns>
        IList<double> PredictWeights(ISingleValueDataSet<double> dataToOperate);
    }
}
