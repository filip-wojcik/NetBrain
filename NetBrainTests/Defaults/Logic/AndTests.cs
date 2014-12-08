using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Defaults.Logic.Models;

namespace NetBrainTests.Defaults.Logic
{
    [TestClass()]
    public class AndTests
    {
        [TestMethod()]
        public void AndTest_ResultTrue()
        {
            //Given
            var and = new And<string>();
            var inputs = new bool[] { true, true, true };

            //Then
            Assert.IsTrue(and.Evaluate(inputs));
        }

        [TestMethod()]
        public void AndTest_ResultFalse()
        {
            //Given
            var and = new And<string>();
            var inputs = new bool[] { true, false, true };

            //Then
            Assert.IsFalse(and.Evaluate(inputs));
        }

        [TestMethod()]
        public void NegateTest()
        {
            //Given
            var and = new And<string>();
            
            //When
            var not_and = and.Negate();

            //Then
            Assert.AreEqual(typeof(Or<string>), not_and.GetType());
        }
    }
}
