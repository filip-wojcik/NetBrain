using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Abstracts.Logic.Exceptions;
using NetBrain.Defaults.Logic.Models;

namespace NetBrainTests.Defaults.Logic
{
    [TestClass()]
    public class NotTests
    {
        [TestMethod()]
        public void NotTest_ResultTrue()
        {
            //Given
            var not = new Not<string>();
            var input = new bool[] { false };

            //Then
            Assert.IsTrue(not.Evaluate(input));
        }

        [TestMethod()]
        public void NotTest_ResultFalse()
        {
            //Given
            var not = new Not<string>();
            var input = new bool[] { true };

            //Then
            Assert.IsFalse(not.Evaluate(input));
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidInputsCountException))]
        public void NotTest_ExceptionTooManyArguments()
        {
            //Given
            var not = new Not<string>();
            var input = new bool[] { true, false, true };

            //When
            not.Evaluate(input);
        }

        [TestMethod()]
        public void NegateTest()
        {
            //Given
            var not = new Not<string>();
            var not_not = not.Negate();

            //Then
            Assert.AreEqual(typeof(Pass<string>), not_not.GetType());
        }
    }
}
