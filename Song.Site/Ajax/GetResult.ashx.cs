using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using System.Xml;

namespace Song.Site.Ajax
{
    /// <summary>
    /// 获取考试的答题信息
    /// </summary>
    public class GetResult : IHttpHandler
    {

        //试卷id，考试id,学生id
        private int tpid = WeiSha.Common.Request.QueryString["tpid"].Int32 ?? 0;
        private int examid = WeiSha.Common.Request.QueryString["examid"].Int32 ?? 0;
        private int stid = WeiSha.Common.Request.QueryString["stid"].Int32 ?? 0;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //获取答题信息
            Song.Entities.ExamResults exr = Business.Do<IExamination>().ResultSingleForCache(examid, tpid, stid);
            if (exr == null) context.Response.Write("0");
            if (exr != null) context.Response.Write(getResult(exr.Exr_Results));
            context.Response.End();
        }
        /// <summary>
        /// 输出json
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string getResult(string result)
        {
            XmlDocument resXml = new XmlDocument();
            resXml.XmlResolver = null; 
            resXml.LoadXml(result, false);
            string json = "{";
            XmlNode root = resXml.LastChild;
            foreach (XmlAttribute attr in root.Attributes)
            {
                json += "'" + attr.Name + "':'" + attr.Value + "',";
            }
            string jsonQues = "'ques':[";
            XmlNodeList ques = resXml.GetElementsByTagName("ques");
            for (int i = 0; i < ques.Count; i++)
            {
                XmlNode node = ques[i];
                int type = Convert.ToInt32(node.Attributes["type"].Value);
                for (int n = 0; n < node.ChildNodes.Count; n++)
                {
                    XmlNode q = node.ChildNodes[n];
                    string id = q.Attributes["id"].Value;
                    string cls = q.Attributes["class"].Value;
                    string ans = string.Empty, file = string.Empty;
                    //如果是单选、多选、判断
                    if (type == 1 || type == 2 || type == 3)
                        ans = q.Attributes["ans"].Value;                        
                    if (type == 4 || type == 5) ans = q.InnerText;
                    if (type == 5) file = q.Attributes["file"] != null ? q.Attributes["file"].Value : "";
                    jsonQues += "{";
                    jsonQues += string.Format("'type':{0},'id':{1},'cls':'{2}','ans':'{3}','file':'{4}'", type, id, cls, ans, file);
                    jsonQues += "},";
                }
            }
            if (jsonQues.IndexOf(',') > -1) jsonQues = jsonQues.Substring(0,jsonQues.Length - 1);
            jsonQues += "]";
            json += jsonQues + "}";
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