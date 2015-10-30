using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator.Fakes;
using System.Fakes;
using Microsoft.QualityTools.Testing.Fakes;

namespace Calculator.Tests
{
    [TestClass]
    public class MathTests
    {
        private CalculatorViewModel calculator;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.calculator = new CalculatorViewModel(
                new BuiltInRandom());
        }

        [TestMethod]
        public void AddSimple()
        {
            calculator.KeyCommand.Execute("1");
            calculator.AddCommand.Execute(null);
            calculator.KeyCommand.Execute("2");
            calculator.EquateCommand.Execute(null);

            Assert.AreEqual(3, calculator.CurrentValue);
        }

        [TestMethod]
        public void DivideSimple()
        {
            calculator.CurrentValue = 200;
            calculator.DivideCommand.Execute(null);
            calculator.CurrentValue = 100;
            calculator.EquateCommand.Execute(null);

            Assert.AreEqual(2, calculator.CurrentValue);
        }

        [TestMethod]
        public void MultipleOperations()
        {
            calculator.CurrentValue = 10;
            calculator.DivideCommand.Execute(null);
            calculator.CurrentValue = 2;
            calculator.MultiplyCommand.Execute(null);
            calculator.CurrentValue = 7;
            calculator.SubtractCommand.Execute(null);
            calculator.CurrentValue = 12;
            calculator.AddCommand.Execute(null);
            calculator.CurrentValue = 7;
            calculator.EquateCommand.Execute(null);

            Assert.AreEqual(30, calculator.CurrentValue);
        }


        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "add", DataAccessMethod.Sequential)]
        public void AddDataDriven()
        {
            int first = int.Parse(this.TestContext.DataRow["first"].ToString());
            int second = int.Parse(this.TestContext.DataRow["second"].ToString());
            int sum = int.Parse(this.TestContext.DataRow["sum"].ToString());

            this.calculator.CurrentValue = first;
            this.calculator.AddCommand.Execute(null);
            this.calculator.CurrentValue = second;
            this.calculator.EquateCommand.Execute(null);
            Assert.AreEqual(sum, this.calculator.CurrentValue);
        }

        [TestMethod]
        public void Back()
        {
            calculator.CurrentValue = 100;
            calculator.BackCommand.Execute(null);

            Assert.AreEqual(10, calculator.CurrentValue);
        }

        [TestMethod]
        public void AddRandomSimple()
        {
            var stubRandom = new StubIRandom();
            stubRandom.GetRandomNumber = () => { return -10; };

            calculator = new CalculatorViewModel(stubRandom);

            calculator.KeyCommand.Execute("100");
            calculator.AddCommand.Execute(null);
            calculator.AddRandomCommand.Execute(null);
            calculator.EquateCommand.Execute(null);

            Assert.AreEqual(90, calculator.CurrentValue);
        }

        [TestMethod]
        public void AddSeconds()
        {
            using (ShimsContext.Create())
            {
                ShimDateTime.NowGet = () =>
                    {
                        return new DateTime(2014, 6, 12, 11, 11, 50);
                    };


                calculator.KeyCommand.Execute("100");
                calculator.AddCommand.Execute(null);
                calculator.AddSecondsCommand.Execute(null);
                calculator.EquateCommand.Execute(null);

                Assert.AreEqual(150, calculator.CurrentValue);
            }

        }

    }


}
