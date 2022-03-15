using HealthMate.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognitionServiceTest
{
    [TestClass]
    public class FullDataEntryTest
    {
        [TestMethod]
        public void CompleteInput1()
        {
            var eval = new TextEvaluation();
            string input = "Heute mrgen um 8 war mein Gewicht 93 kg";
            var commandVal = eval.GetCommandInText(input);
            Assert.AreEqual(Commands.InputWeight, commandVal);
            var value = eval.GetFirstValue(input);
            Assert.AreEqual<double?>(93, value);
            var retTime = eval.GetTime(input);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 8, 0, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retTime);
            var retDate = eval.GetDateValue(input);
            Assert.IsNotNull(retDate);
            DateTime expectedDate = DateTime.Now;
            Assert.AreEqual(expectedDate.ToString("d"), retDate.Value.ToString("d"));
        }
        [TestMethod]
        public void CompleteInput2()
        {
            var eval = new TextEvaluation();
            string input = "Am 23.12.2023 um 9:10 war der Blutdruck 123 zu 77";
            var commandVal = eval.GetCommandInText(input);
            Assert.AreEqual(Commands.InputBloodPressure, commandVal);
            var value = eval.GetFirstValue(input);
            Assert.AreEqual<double?>(123, value);
            var secondValue = eval.GetSecondValue(input);
            Assert.AreEqual<double?>(77, secondValue);
            var retTime = eval.GetTime(input);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 9, 10, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retTime);
            var retDate = eval.GetDateValue(input);
            Assert.IsNotNull(retDate);
            DateTime expectedDate = DateTime.Parse("2023-12-23");
            Assert.AreEqual(expectedDate.ToString("d"), retDate.Value.ToString("d"));
        }
        [TestMethod]
        public void CompleteInput3()
        {
            var eval = new TextEvaluation();
            string input = "Gestern um 11 PM hatte ich 38,2 Temperatur";
            var commandVal = eval.GetCommandInText(input);
            Assert.AreEqual(Commands.InputTemperature, commandVal);
            var value = eval.GetFirstValue(input);
            Assert.AreEqual<double?>(38.2, value);
            var retTime = eval.GetTime(input);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 23, 0, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retTime);
            var retDate = eval.GetDateValue(input);
            Assert.IsNotNull(retDate);
            DateTime expectedDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            Assert.AreEqual(expectedDate.ToString("d"), retDate.Value.ToString("d"));
        }
        [TestMethod]
        public void CompleteInput4()
        {
            var eval = new TextEvaluation();
            string input = "Um 10 war mein Blutdruck 120 zu 99";
            var commandVal = eval.GetCommandInText(input);
            Assert.AreEqual(Commands.InputBloodPressure, commandVal);
            var value = eval.GetFirstValue(input);
            Assert.AreEqual<double?>(120, value);
            var secondValue = eval.GetSecondValue(input);
            Assert.AreEqual<double?>(99, secondValue);
            var retTime = eval.GetTime(input);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 10, 0, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retTime);
            var retDate = eval.GetDateValue(input);
            Assert.IsNull(retDate);
        }
    }
}
