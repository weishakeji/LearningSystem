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
            Helper.DbProvider.SetDbGateway();


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
        [TestMethod]
        public void DbTest()
        {
            string path = Helper.Path.WebSitePath();
            string dbconfig= System.IO.Path.Combine(path, "db.config");
            if (System.IO.File.Exists(dbconfig))
            {
                System.Xml.XmlDocument xml =new System.Xml.XmlDocument();
                xml.Load(dbconfig);
                System.Xml.XmlNode nodeconn = xml.LastChild;
                foreach (System.Xml.XmlNode node in nodeconn.ChildNodes)
                {
                    if (node.NodeType != System.Xml.XmlNodeType.Element) continue;

                    string name = node.Attributes["name"]?.Value;
                    string conn = node.Attributes["connectionString"]?.Value; 
                    string provider = node.Attributes["providerName"]?.Value;          
                }
            }
        }

    }
}
