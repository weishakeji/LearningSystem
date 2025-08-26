using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Song.Entities;
using System.Reflection;
using Newtonsoft.Json.Linq;
using WeiSha.Core;

namespace Song.ServiceImpls.Exam
{
    /// <summary>
    /// 试卷处理类，用于生成试卷
    /// </summary>
    public class TestPaperHandler
    {
        private static readonly TestPaperCom tpcom = new Song.ServiceImpls.TestPaperCom();
        private static readonly QuestionsCom quescom = new Song.ServiceImpls.QuestionsCom();
        /// <summary>
        /// 试卷ID
        /// </summary>
        public long TestPaperID { get; set; }
        /// <summary>
        /// 试卷实体对象
        /// </summary>
        public Song.Entities.TestPaper TestPaperEntity { get; set; }
        /// <summary>
        /// 生成的试卷的内容，包括试题和得分等
        /// </summary>
        public Dictionary<TestPaperItem, List<Questions>> PagerContents { get; set; }

        /// <summary>
        /// 构建方法
        /// </summary>
        /// <param name="tpid"></param>
        public TestPaperHandler(long tpid)
        {
            this.TestPaperID = tpid;
            this.TestPaperEntity = tpcom.PaperSingle(tpid);
        }
        public TestPaperHandler()
        {

        }
        #region 随机生成试卷
        /// <summary>
        /// 生成试卷
        /// </summary>
        /// <param name="tpid">试卷id</param>
        /// <returns></returns>
        public static TestPaperHandler Putout(long tpid) => Putout(tpid, false);
        /// <summary>
        /// 生成试卷
        /// </summary>
        /// <param name="tpid">试卷id</param>
        /// <param name="isanswer">试题是否带答案，模拟考试一般带答案，方便前端计算成绩</param>
        /// <returns></returns>
        public static TestPaperHandler Putout(long tpid, bool isanswer)
        {
            TestPaperHandler tp = new TestPaperHandler(tpid);            
            tp.PagerContents = tpcom.Putout(tp.TestPaperEntity, isanswer);  //生成试卷
            return tp;
        }
        #endregion

        #region 通过答题信息，反向生成试卷
        /// <summary>
        /// 通过答题信息，反向生成试卷
        /// </summary>
        /// <param name="exr">考试答题信息的记录对象</param>
        /// <returns></returns>
        public static TestPaperHandler Putout(ExamResults exr) => Putout(exr.Exr_Results);
        /// <summary>
        /// 通过答题信息，反向生成试卷
        /// </summary>
        /// <param name="tr">测试的答题信息</param>
        /// <returns></returns>
        public static TestPaperHandler Putout(TestResults tr) => Putout(tr.Tr_Results);
        /// <summary>
        /// 通过答题信息，反向生成试卷
        /// </summary>
        /// <param name="results">答题信息的xml文本</param>
        /// <returns></returns>
        public static TestPaperHandler Putout(string results)
        {
            XmlDocument docxml = new XmlDocument();
            docxml.XmlResolver = null;
            docxml.LoadXml(results, false);
            return Putout(docxml);
        }
        /// <summary>
        ///  通过答题信息，反向生成试卷
        /// </summary>
        /// <param name="resxml">答题信息的xml对象</param>
        /// <returns></returns>
        public static TestPaperHandler Putout(XmlDocument resxml)
        {
            TestPaperHandler tp = new TestPaperHandler();
            //生成试卷内容，即试题
            tp.PagerContents = tpcom.Putout(resxml, false);
            return tp;
        }
        #endregion

        #region 输出试卷
        /// <summary>
        /// 输出试卷为Json
        /// </summary>
        /// <returns></returns>
        public JArray ToJson()
        {
            //生成试卷
            Dictionary<TestPaperItem, List<Questions>> dics = this.PagerContents;
            JArray jarr = new JArray();
            foreach (var di in dics)
            {
                //按题型输出
                Song.Entities.TestPaperItem pi = (Song.Entities.TestPaperItem)di.Key;   //试题类型                
                List<Questions> questions = (List<Questions>)di.Value;   //当前类型的试题
                int type = (int)pi.TPI_Type;    //试题类型
                int count = questions.Count;  //试题数目
                float num = (float)pi.TPI_Number;   //占用多少分
                if (count < 1) continue;
                JObject jo = new JObject();
                jo.Add("type", type);
                jo.Add("count", count);
                jo.Add("number", num);
                JArray ques = new JArray();
                foreach (Song.Entities.Questions q in questions)
                {
                    string json = q.ToJson("", "Qus_CrtTime,Qus_LastTime");
                    ques.Add(JObject.Parse(json));
                }
                jo.Add("ques", ques);
                jarr.Add(jo);
            }
            return jarr;
        }
        /// <summary>
        /// 输出试卷为xml，答题状态
        /// </summary>
        /// <returns></returns>
        public XmlDocument ToXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xmlDeclaration);
            //创建根节点
            XmlElement root = doc.CreateElement("results");
            root.SetAttribute("tpid", this.TestPaperEntity.Tp_Id.ToString());   //试卷ID
            root.SetAttribute("sbjid", this.TestPaperEntity.Sbj_ID.ToString());   //专业id
            root.SetAttribute("sbjname", this.TestPaperEntity.Sbj_Name.ToString());   //专业名称
            doc.AppendChild(root);
            Dictionary<TestPaperItem, List<Questions>> dics = this.PagerContents;
            foreach (var di in dics)
            {
                //按题型输出
                Song.Entities.TestPaperItem pi = (Song.Entities.TestPaperItem)di.Key;   //试题类型
                List<Questions> questions = (List<Questions>)di.Value;   //当前类型的试题
                XmlElement elques = doc.CreateElement("ques");
                elques.SetAttribute("type", pi.TPI_Type.ToString());
                elques.SetAttribute("count", questions.Count.ToString());
                elques.SetAttribute("number", pi.TPI_Number.ToString());
                //
                foreach (Song.Entities.Questions q in questions)
                {
                    XmlElement elq = doc.CreateElement("q");
                    elq.SetAttribute("id", q.Qus_ID.ToString());
                    elq.SetAttribute("num", q.Qus_Number.ToString());
                    if (q.Qus_Type == 4 || q.Qus_Type == 5)
                    {
                        elq.InnerText = string.Empty;
                    }
                    else elq.SetAttribute("ans", "");
                    elq.SetAttribute("file", "");
                    elq.SetAttribute("sucess", "false");
                    elq.SetAttribute("score", "0");
                    elques.AppendChild(elq);
                }
                root.AppendChild(elques);
            }
            return doc;
        }
        /// <summary>
        /// 根据试卷生成答题的xml解析对象
        /// </summary>
        /// <returns></returns>
        public Results ToResults()
        {
            XmlDocument doc = this.ToXml();
            Results results = new Results(doc.OuterXml);
            return results;
        }
        /// <summary>
        /// 根据试卷生成答题的xml解析对象
        /// </summary>
        /// <returns></returns>
        public Results ToResults(Examination exam, Accounts acc)
        {
            XmlDocument doc = this.ToXml();
            Results results = new Results(doc.OuterXml);
            results.Examid = exam.Exam_ID;
            results.ExamUid = exam.Exam_UID;
            results.ExamTheme = exam.Exam_Title;    //考试主题
            //学员信息
            results.AccountID = acc.Ac_ID;
            results.AccountName = acc.Ac_Name;
            results.IDCardNumber = acc.Ac_IDCardNumber;
            results.Gender = acc.Ac_Sex;
            results.SortID = acc.Sts_ID;
            
            return results;
        }
        #endregion
    }
}
