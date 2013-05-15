using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void Add_TwoNumbers_ReturnsSum()
        {
            var calculator = new Calc();
            
            var sum = calculator.Add(1, 2);

            Assert.AreEqual(3, sum);
        }
    }
}
