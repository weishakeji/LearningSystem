using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Song.Entities;
using WeiSha.Data;
using WeiSha.Core;
using System.Xml;

namespace UnitTest
{
    [TestClass]
    public class UnitTest_Testpaper
    {
        [TestMethod]
        public void TestMethod1()
        {
            Helper.DbProvider.SetDbGateway();

            XmlDocument doc = Song.ServiceImpls.Exam.TestPaperHandler.Putout(635).ToXml();

            string xml = doc.OuterXml;

            Assert.IsNotNull(xml);
        }
    }
}
