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
    public class DateTimeInputTest
    {
        [TestMethod]
        public void YesterdayAndTimeTest()
        {
            var eval = new TextEvaluation();
            string input = "My blood pressure was 120 to 80 yesterday at 22:15";
            var retval1 = eval.GetDateValue(input);
            Assert.IsNotNull(retval1);
            DateTime expectedDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            Assert.AreEqual(expectedDate.ToString("d"), retval1.Value.ToString("d"));
            var retval2 = eval.GetTime(input);
            Assert.IsNotNull(retval2);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 22, 15, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retval2);

        }

        [TestMethod]
        public void TodayAndTimeTest()
        {
            var eval = new TextEvaluation();
            string input = "My blood pressure was 120 to 80 heute morgen um 08:00";
            var retval1 = eval.GetDateValue(input);
            Assert.IsNotNull(retval1);
            DateTime expectedDate = DateTime.Now;
            Assert.AreEqual(expectedDate.ToString("d"), retval1.Value.ToString("d"));
            var retval2 = eval.GetTime(input);
            Assert.IsNotNull(retval2);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 8, 0, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retval2);

        }

        [TestMethod]
        public void PMTimeTest()
        {
            var eval = new TextEvaluation();
            string input = "I measuerd today at 8 PM";
            var retval2 = eval.GetTime(input);
            Assert.IsNotNull(retval2);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 20, 0, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retval2);

        }

        [TestMethod]
        public void TimeTest1()
        {
            var eval = new TextEvaluation();
            string input = "Ich habe heute morgen um 8 gemessen";
            var retval2 = eval.GetTime(input);
            Assert.IsNotNull(retval2);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 8, 0, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retval2);

        }

        [TestMethod]
        public void TimeTest2()
        {
            var eval = new TextEvaluation();
            string input = "Ich habe heute morgen um 11 gemessen";
            var retval2 = eval.GetTime(input);
            Assert.IsNotNull(retval2);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 11, 0, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retval2);

        }
        [TestMethod]
        public void TimeTest3()
        {
            var eval = new TextEvaluation();
            string input = "Ich habe heute morgen viertel vor 8 gemessen";
            var retval2 = eval.GetTime(input);
            Assert.IsNotNull(retval2);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 7, 45, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retval2);

        }

        [TestMethod]
        public void TimeTest4()
        {
            var eval = new TextEvaluation();
            string input = "measurement was a quarter to 10";
            var retval2 = eval.GetTime(input);
            Assert.IsNotNull(retval2);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 9, 45, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retval2);

        }

        [TestMethod]
        public void TimeTest5()
        {
            var eval = new TextEvaluation();
            string input = "measurement was at half past 10";
            var retval2 = eval.GetTime(input);
            Assert.IsNotNull(retval2);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 10, 30, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retval2);

        }

        [TestMethod]
        public void TimeTest6()
        {
            var eval = new TextEvaluation();
            string input = "measurement was at a quarter past 11";
            var retval2 = eval.GetTime(input);
            Assert.IsNotNull(retval2);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 11, 15, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retval2);

        }
        [TestMethod]
        public void DateTest1()
        {
            var eval = new TextEvaluation();
            string input = "Am 31. Januar 2022";
            var retval1 = eval.GetTime(input);
            Assert.IsNull(retval1);
            var retDate = eval.GetDateValue(input);
            Assert.IsNotNull(retDate);
            DateTime expectedDate = DateTime.Parse("2022-01-31");
            Assert.AreEqual(expectedDate.ToString("d"), retDate.Value.ToString("d"));

        }

        [TestMethod]
        public void DateTest2()
        {
            var eval = new TextEvaluation();
            string input = "Am 7. Juli 2022 um 13:45";
            var retval2 = eval.GetTime(input);
            Assert.IsNotNull(retval2);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 13, 45, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retval2);
            var retDate = eval.GetDateValue(input);
            Assert.IsNotNull(retDate);
            DateTime expectedDate = DateTime.Parse("2022-07-07");
            Assert.AreEqual(expectedDate.ToString("d"), retDate.Value.ToString("d"));

        }

        [TestMethod]
        public void DateTest3()
        {
            var eval = new TextEvaluation();
            string input = "Am 12.08.2022 um 22:14";
            var retval2 = eval.GetTime(input);
            Assert.IsNotNull(retval2);
            DateTime expectedTime = DateTime.MinValue + new TimeSpan(0, 22, 14, 0);
            Assert.AreEqual<DateTime?>(expectedTime, retval2);
            var retDate = eval.GetDateValue(input);
            Assert.IsNotNull(retDate);
            DateTime expectedDate = DateTime.Parse("2022-08-12");
            Assert.AreEqual(expectedDate.ToString("d"), retDate.Value.ToString("d"));

        }

        [TestMethod]
        public void DateTest4()
        {
            var eval = new TextEvaluation();
            string input = "3. March 2022";
            var retval2 = eval.GetTime(input);
            Assert.IsNull(retval2);
            var retDate = eval.GetDateValue(input);
            Assert.IsNotNull(retDate);
            DateTime expectedDate = DateTime.Parse("2022-03-03");
            Assert.AreEqual(expectedDate.ToString("d"), retDate.Value.ToString("d"));

        }


    }
}
