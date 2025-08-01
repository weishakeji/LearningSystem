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


            string path = Helper.Path.GetFilePath();
            string xml = System.IO.File.ReadAllText(path);
            Song.ServiceImpls.Exam.Results results = new Song.ServiceImpls.Exam.Results(xml);

            string text = results.OutputXML(false);
            //计算分数
            float score = results.Score;
            Assert.IsTrue(score > 0);

            ////
            //Song.ServiceImpls.Exam.QuesAnswer qa = results.QuesTypes[1].QuesAnswers[0];
            //qa.ToWrong();
            //qa.ToWrong();
            //qa.ToWrong();
        }
        

        /// <summary>
        /// 生成简答题的正确答案
        /// </summary>

        [TestMethod]
        public void QuesType4ToAnswer()
        {
            Helper.DbProvider.SetDbGateway();
            Song.APIHub.LLM.Gatway.SetApiKey("sk-");
            Song.APIHub.LLM.Gatway.SetApiUrl("https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions");
            Song.APIHub.LLM.Gatway.SetApiModel("qwen-turbo-latest");
            Song.APIHub.LLM.Gatway.SetTmpRootPath(@"E:\SourceCode\02_LearningSystem_dev\Sourcecode\Song.WebSite");

            Song.Entities.Questions ques =Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == 129132493668093952).ToFirst<Questions>();
            string ans = Song.ServiceImpls.Exam.QuesAnswer.QuesType4ToAnswer(ques, 8,3);
        }
        /// <summary>
        /// 测试重新生成答案的XML节点
        /// </summary>

        [TestMethod]
        public void RebuildAnsNode()
        {
            Helper.DbProvider.SetDbGateway();
            string path = Helper.Path.GetFilePath();
            string xml = System.IO.File.ReadAllText(path);
            Song.ServiceImpls.Exam.Results results = new Song.ServiceImpls.Exam.Results(xml);

            Song.ServiceImpls.Exam.QuesAnswer qa = results.QuesTypes[4].QuesAnswers[0];
            qa.ToWrong(3);
            qa.RebuildNode();
            string ans = qa.Node.OuterXml;

        }
        /// <summary>
        /// 重新计分
        /// </summary>
        [TestMethod]
        public void TypeSetScore()
        {
            Helper.DbProvider.SetDbGateway();
            string path = Helper.Path.GetFilePath();
            string xml = System.IO.File.ReadAllText(path);
            Song.ServiceImpls.Exam.Results results = new Song.ServiceImpls.Exam.Results(xml);

            //Song.ServiceImpls.Exam.QuesType qtype = results.QuesTypes[2];
            //float scoretype=qtype.SetScore(16);

            float score = results.SetScore(75);

            string text = results.OutputXML(false);
            //计算分数
            //float score = results.Score;
            Assert.IsTrue(score > 0);
        }
    }
}
