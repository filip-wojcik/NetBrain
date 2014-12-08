using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Defaults.Common.Models;
using NetBrain.Defaults.ConstraintSatisfaction.Evaluators;
using NetBrainTests.Defaults.ConstraintSatisfaction.SampleProblems.SimpleSudoku;

namespace NetBrainTests.Defaults.ConstraintSatisfaction.Evaluators
{
    [TestClass()]
    public class CSPEvaluatorTests
    {
       
        [TestMethod()]
        public void CSPEvaluatorTest()
        {
            //Given
            var simpleSudoku = new SimpleSudokuProblemDefinition(3, 3, VariableDomain<int>.DefaultFactory){ IsConsistent = true};
            var solver = new CSPEvaluator<int>()
            {
                SelectValueHeuristic = new TakeAllValues<int>(),
                SelectVariableHeuristic = new FailFastVariableHeuristic<int>(),
                ProblemUpdater = new ForewardChecking<int>()
            };

            //When
            var solutions = solver.Solve(simpleSudoku).ToList();

            //Then
            Assert.AreEqual(12, solutions.Count);
        }
    }
}
