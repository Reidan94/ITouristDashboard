using System;
using ITourist.Models.DataModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITourist.Tests.Models.DataModels
{
    [TestClass]
    public class CheckPointTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string s = AverageTimeFormatted(11);
            s = AverageTimeFormatted(32);
            s = AverageTimeFormatted(132);
            s = AverageTimeFormatted(1463);
            s = AverageTimeFormatted(1565);
            s = AverageTimeFormatted(99999);
            Assert.IsNotNull(s);
        }

        private string AverageTimeFormatted(double avg, Culture culture = Culture.En)
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(avg);
            return NumberTranslator.TimeToString(timeSpan, Culture.Ru);
        }
    }
}
