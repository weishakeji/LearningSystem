using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Song.Entities;
using System.Reflection;

namespace Song.ServiceImpls.Exam
{
    public class TestPaperHandler
    {
        private static readonly TestPaperCom tpcom = new Song.ServiceImpls.TestPaperCom();
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
        /// <summary>
        /// 出卷
        /// </summary>
        public XmlDocument Putout()
        {
            //生成试卷
            Dictionary<TestPaperItem, List<Questions>> dics = tpcom.Putout(TestPaperEntity, false);
            foreach (var di in dics)
            {
                //按题型输出
                Song.Entities.TestPaperItem pi = (Song.Entities.TestPaperItem)di.Key;   //试题类型                
                List<Questions> questions = (List<Questions>)di.Value;   //当前类型的试题
                int type = (int)pi.TPI_Type;    //试题类型
                int count = questions.Count;  //试题数目
                float num = (float)pi.TPI_Number;   //占用多少分
                if (count < 1) continue;
                //JObject jo = new JObject();
                //jo.Add("type", type);
                //jo.Add("count", count);
                //jo.Add("number", num);
                //JArray ques = new JArray();
                //foreach (Song.Entities.Questions q in questions)
                //{
                //    string json = q.ToJson("", "Qus_CrtTime,Qus_LastTime");
                //    ques.Add(JObject.Parse(json));
                //}
                //jo.Add("ques", ques);
                //jarr.Add(jo);
            }
            XmlDocument doc = new XmlDocument();

            return doc;
        }
        public static TestPaperHandler Putout(long tpid)
        {
            TestPaperHandler tp = new TestPaperHandler(tpid);
            //生成试卷
            tp.PagerContents = tpcom.Putout(tp.TestPaperEntity, false);
            return tp;
        }
        public XmlDocument ToXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xmlDeclaration);
            Dictionary<TestPaperItem, List<Questions>> dics = this.PagerContents;
            foreach (var di in dics)
            {
            }

            return doc;
        }
        public Results ToResults()
        {           

            return null;
        }
    }
}
