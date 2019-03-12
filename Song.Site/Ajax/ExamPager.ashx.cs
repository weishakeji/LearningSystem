using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Text;
using WeiSha.Common;
using System.Xml;
namespace Song.Site.Ajax
{
    /// <summary>
    /// ExamPager1 的摘要说明
    /// </summary>
    public class ExamPager : IHttpHandler
    {
        //试卷id，考试id,学生id
        private int tpid = WeiSha.Common.Request.Form["tpid"].Int32 ?? 0;
        private int examid = WeiSha.Common.Request.Form["examid"].Int32 ?? 0;
        private int stid = WeiSha.Common.Request.Form["stid"].Int32 ?? 0;
        public HttpResponse Response;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Charset = "utf-8";
            Response = context.Response;
            //获取答题信息
            Song.Entities.ExamResults exr = Business.Do<IExamination>().ResultSingleForCache(examid, tpid, stid);
            string json = string.Empty;
            if (exr == null)
            {   
                //第一次，随机出题
                json = randomJson();                
            }
            else
            {
                //如果已经交过卷，输出2     
                json = exr.Exr_IsSubmit ? "2" : resultJson(exr);                              
            }
            Response.Write(json);
            Response.End();
        }
        #region 输出json
        /// <summary>
        /// 随机出题
        /// </summary>
        /// <returns></returns>
        private string randomJson()
        {
            //取果是第一次打开，则随机生成试题，此为获取试卷
            Song.Entities.TestPaper pager = Business.Do<ITestPaper>().PagerSingle(tpid);
            Dictionary<TestPaperItem, Questions[]> dics = Business.Do<ITestPaper>().Putout(pager);   //生成试卷
            //
            List<Questions> list = new List<Questions>();   //缓存
            string json = "[";
            foreach (var di in dics)
            {
                //++++++++按题型输出
                Song.Entities.TestPaperItem pi = (Song.Entities.TestPaperItem)di.Key;   //试题类型                
                Song.Entities.Questions[] ques = (Song.Entities.Questions[])di.Value;   //当前类型的试题
                int type = (int)pi.TPI_Type;    //试题类型
                int count = ques.Length;  //试题数目
                float num = (float)pi.TPI_Number;   //占用多少分
                if (count < 1) continue;
                string tpiJosn = "{'type':'" + type + "','count':'" + count + "','number':'" + num + "',";
                tpiJosn += "'ques':[";
                tpiJosn = tpiJosn.Replace("'", "\"");
                    //++++++++试题输出
                    string quesJson = string.Empty;
                    for (int n = 0; n < ques.Length; n++)
                    {
                        quesJson += getQuesJson(ques[n]) + ",";
                        list.Add(ques[n]);  //存入列表，用于缓存存储
                    }
                    if (quesJson.EndsWith(",")) quesJson = quesJson.Substring(0, quesJson.Length - 1);
                    //--------试题输出结束
                tpiJosn += quesJson + "]}";
                json += tpiJosn + ",";
                //--------按题型输出结束
            }
            if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
            json += "]";
            //缓存
            if (list.Count > 0)
            {
                string uid = string.Format("ExamResults：{0}-{1}-{2}", examid, tpid, stid);    //缓存的uid
                Song.Entities.Examination exam = Business.Do<IExamination>().ExamSingle(examid);
                if (exam != null)
                {
                    Random rn = new Random(stid);
                    Business.Do<IQuestions>().CacheAdd(list.ToArray<Song.Entities.Questions>(), exam.Exam_Span + rn.Next(10), uid);
                }
            }
            return json;
        }
        /// <summary>
        /// 如果已经提交过答案，通过提交的答题返回试题
        /// </summary>
        /// <param name="exr"></param>
        /// <returns></returns>
        private string resultJson(Song.Entities.ExamResults exr)
        {
            XmlDocument resXml = new XmlDocument();
            resXml.XmlResolver = null; 
            resXml.LoadXml(exr.Exr_Results, false);
            string json = "[";
            XmlNodeList ques = resXml.GetElementsByTagName("ques");
            for (int i = 0; i < ques.Count;i++ )
            {
                XmlNode node = ques[i];
                string type = node.Attributes["type"].Value;
                string count = node.Attributes["count"].Value;
                string num = node.Attributes["number"].Value;
                string quesObj = "{";
                quesObj += "'type':'" + type + "','count':'" + count + "','number':'" + num + "',";
                quesObj += "'ques':[";
                quesObj = quesObj.Replace("'", "\"");
                for (int n = 0; n < node.ChildNodes.Count; n++)
                {
                    int id = Convert.ToInt32(node.ChildNodes[n].Attributes["id"].Value);
                    Song.Entities.Questions q = null;
                    q = Business.Do<IQuestions>().QuesSingle4Cache(id);
                    if (q == null) q = Business.Do<IQuestions>().QuesSingle(id);
                    if (q == null) continue;
                    q.Qus_Number = (float)Convert.ToDouble(node.ChildNodes[n].Attributes["num"].Value);
                    q.Qus_Explain = "";
                    q.Qus_Answer = "";
                    string js = getQuesJson(q);
                    //如果是单选题，或多选题，或填空题
                    if (q.Qus_Type == 1 || q.Qus_Type == 2 || q.Qus_Type == 5)
                        quesObj += getAnserJson(q, js);
                    else
                        quesObj += js;
                    if (n < node.ChildNodes.Count - 1) quesObj += ",";
                }
                quesObj += "]";
                quesObj += "}";
                if (i < ques.Count - 1) quesObj += ",";
                json += quesObj;
            }
            json += "]";
            return json;
        }
        #endregion

        /// <summary>
        /// 输出主式题的Json
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private string getQuesJson(Song.Entities.Questions q)
        {
            //q = quesClear(q);
            //q = Extend.Questions.TranText(q);   //处理文件
            //q.Qus_Title = q.Qus_Title.Replace("\"", "&quot;");  //转换双引号
            string quesJs = q.ToJson("Qus_ID,Qus_Title,Qus_Diff,Qus_Type,Qus_UID,Qus_Number", null);
            //如果是单选题，或多选题，或填空题
            if (q.Qus_Type == 1 || q.Qus_Type == 2 || q.Qus_Type == 5)
                quesJs = getAnserJson(q, quesJs);
            return quesJs;
        }
        /// <summary>
        /// 试题选择项的Json输出
        /// </summary>
        /// <param name="q"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        private string getAnserJson(Song.Entities.Questions q, string json)
        {
            //当前试题的答案
            Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(q, null);
            string ansStr = "[";
            for (int i = 0; i < ans.Length; i++)
            {
                ans[i] = Extend.Questions.TranText(ans[i]);
                ansStr += ans[i].ToJson();
                if (i < ans.Length - 1) ansStr += ",";
            }
            ansStr += "]";
            json = json.Replace("}", ",\"Answer\":" + ansStr + "}");
            return json;
        }        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}