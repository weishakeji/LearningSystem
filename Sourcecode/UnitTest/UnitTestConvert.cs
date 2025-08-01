using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Song.ServiceImpls.Exam;
using WeiSha.Core;

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

            string fl = "5.6";
            float lng= fl.Convert<float>();


            //DateTime tt = lng.ToDateTime();

            Assert.AreEqual(lng, 5.6);
            
        }
    }
}
