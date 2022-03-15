using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthMate.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace RecognitionServiceTest

{
    [TestClass]
    public class ValueInputTests
    {

        [TestMethod]
        public void WeighingInputTest()
        {
            var eval = new TextEvaluation();
            string input = "My weight was 23,4 today at 9 pm";
            var retval = eval.GetFirstValue(input);
            Assert.AreEqual<double?>(23.4, retval);

        }

        [TestMethod]
        public void BloodPressureInputInputTest1()
        {
            var eval = new TextEvaluation();
            string input = "My blood pressure was 120 to 80 yesterday at 22:15";
            var retval1 = eval.GetFirstValue(input);
            Assert.AreEqual(120, retval1);
            var retval2 = eval.GetSecondValue(input);
            Assert.AreEqual<double?>(80, retval2);

        }

        [TestMethod]
        public void BloodPressureInputInputTest2()
        {
            var eval = new TextEvaluation();
            string input = "My blood pressure was 220 to 78 yesterday at 22:15";
            var retval1 = eval.GetFirstValue(input);
            Assert.AreEqual(220, retval1);
            var retval2 = eval.GetSecondValue(input);
            Assert.AreEqual<double?>(78, retval2);

        }
        [TestMethod]
        public void BloodPressureInputInputTest3()
        {
            var eval = new TextEvaluation();
            string input = "systolic 114 and diastolic 93 measured now";
            var retval1 = eval.GetFirstValue(input);
            Assert.AreEqual<double?>(114, retval1);
            var retval2 = eval.GetSecondValue(input);
            Assert.AreEqual<double?>(93, retval2);

        }
    }
}
