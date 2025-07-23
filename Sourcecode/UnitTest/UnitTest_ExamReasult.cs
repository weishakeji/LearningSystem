using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Song.ServiceInterfaces;
using Song.ServiceImpls;
using Song.Entities;
using WeiSha.Data;
using WeiSha.Core;

namespace UnitTest
{
    /// <summary>
    /// 考试成绩的处理
    /// </summary>
    [TestClass]
    public class UnitTest_ExamReasult
    {
        [TestMethod]
        public void CreateObject()
        {
            //int n = Business.Do<IAccounts>().AccountsOfCount(1, true, 1);
            //Assert.IsTrue(n > 0);

            //string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;

            string path = Helper.Path.GetXML();
            string xml = System.IO.File.ReadAllText(path);
            Song.ServiceImpls.Exam.Results results = new Song.ServiceImpls.Exam.Results(xml);

            string text = results.OutputXML(false);
            
        }


    }
}
