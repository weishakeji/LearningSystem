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
            WeiSha.Data.DbProvider provider = WeiSha.Data.DbProviderFactory.CreateDbProvider("WeiSha.Data.PostgreSQL.PostgreSQLProvider", "Server=localhost;Port=5432;Database=gxmk;User Id=postgres;Password=weishakeji;");
            WeiSha.Data.Gateway.SetDefault(provider);


            string path = Helper.Path.GetXML();
            string xml = System.IO.File.ReadAllText(path);
            Song.ServiceImpls.Exam.Results results = new Song.ServiceImpls.Exam.Results(xml);

            //string text = results.OutputXML(false);
            //Gateway.SetDefault("Server = localhost; Port = 5432; Database = gxmk; User Id = postgres; Password = weishakeji;");
            //计算分数
            float score = results.Score;
            Assert.IsTrue(score > 0);

            //
            Song.ServiceImpls.Exam.QuesAnswer qa = results.QuesTypes[1].QuesAnswers[0];
            qa.ToWrong();
            qa.ToWrong();
            qa.ToWrong();
        }


    }
}
