using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Song.ServiceImpls.Exam;

namespace UnitTest
{
    [TestClass]
    public class UnitTestConvert
    {
        [TestMethod]
        public void TestMethod1()
        {
            string text = "1747301879000";
            DateTime t = text.Convert<DateTime>();

            long lng= text.Convert<long>();
            DateTime tt = lng.ToDateTime();

            Assert.AreEqual(t, tt);
            
        }
    }
}
