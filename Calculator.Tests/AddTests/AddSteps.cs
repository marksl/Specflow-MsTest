using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace Calculator.Tests.AddTests
{
    [Binding]
    public class AddSteps
    {
        private int _a;
        private int _b;
        private int _result;

        [Given(@"I have entered (.*) and (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int a, int b)
        {
            _a = a;
            _b = b;
        }
        
        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            var calc = new Calc();
            _result = calc.Add(_a, _b);
        }

        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            Assert.AreEqual(p0, _result);
        }
    }
}
