using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBrain.Defaults.Logic.Models;

namespace NetBrainTests.Defaults.Logic
{
    [TestClass()]
    public class OrTests
    {
        [TestMethod()]
        public void OrTest_ResultTrue()
        {
            //Given
            var or = new Or<string>();
            var inputs = new bool[] { false, false, true };

            //Then
            Assert.IsTrue(or.Evaluate(inputs));
        }

        [TestMethod()]
        public void OrTest_ResultFalse()
        {
            //Given
            var or = new Or<string>();
            var inputs = new bool[] { false, false, false};

            //Then
            Assert.IsFalse(or.Evaluate(inputs));
        }

        [TestMethod()]
        public void NegateTest()
        {
            //Given
            var or = new Or<string>();

            //When
            var not_or = or.Negate();

            //Then
            Assert.AreEqual(typeof(And<string>), not_or.GetType());
        }
    }
}
