using HealthMate.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RecognitionServiceTest
{
    [TestClass]
    public class CommandTests
    {
        [TestMethod]
        public void TestCommandInput1()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("Open the input page");
            Assert.AreEqual(Commands.Input, retval);
        }
        [TestMethod]
        public void TestCommandInput2()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("Show me the charts page");
            Assert.AreEqual(Commands.ShowChart, retval);
        }
        [TestMethod]
        public void TestCommandInput3()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("CHARTS");
            Assert.AreEqual(Commands.ShowChart, retval);
        }
        [TestMethod]
        public void TestCommandInput4()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("Open liste");
            Assert.AreEqual(Commands.ShowList, retval);
        }
        [TestMethod]
        public void TestCommandInput5()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("I want to put in data");
            Assert.AreEqual(Commands.Input, retval);
        }
        [TestMethod]
        public void TestCommandInput6()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("I want to enter data");
            Assert.AreEqual(Commands.Input, retval);
        }
        [TestMethod]
        public void TestCommandInput7()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("Let's put in some data");
            Assert.AreEqual(Commands.Input, retval);
        }
        [TestMethod]
        public void TestCommandInput8()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("Let's have a look at the values");
            Assert.AreEqual(Commands.Undefined, retval);
        }
        [TestMethod]
        public void TestStartInput()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("I want to enter my weight");
            Assert.AreEqual(Commands.InputWeight, retval);
        }
        [TestMethod]
        public void TestValueInput()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("My temperature is 37.5");
            Assert.AreEqual(Commands.InputTemperature, retval);
        }
        [TestMethod]
        public void TestValueInput2()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("My blood pressure was 120 to 67 today");
            Assert.AreEqual(Commands.InputBloodPressure, retval);
        }
        [TestMethod]
        public void TestCommanPause()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("pause the input");
            Assert.AreEqual(Commands.Pause, retval);
        }
        [TestMethod]
        public void TestCommanContinue()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("ok, let's continue");
            Assert.AreEqual(Commands.Continue, retval);
        }
        [TestMethod]
        public void TestCommanSomeText()
        {
            var eval = new TextEvaluation();
            var retval = eval.GetCommandInText("ok just some arbitrary text");
            Assert.AreEqual(Commands.Undefined, retval);
        }
    }
}
