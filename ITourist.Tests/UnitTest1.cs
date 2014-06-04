using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITourist.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string s = "1;2;";
            string[] split = s.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreNotEqual(split, null);
            List<string> l = new List<string>(new[]{"2","1"});
            string s2 = String.Join(";", l);
            Assert.AreNotEqual(s2,"2");
        }
    }
}
