using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Xml;
using System.Runtime.InteropServices;
using WeiSha.Common;
namespace Song.Site.Ajax
{
    /// <summary>
    /// 测试成绩的记录
    /// </summary>
    public class InTestResult : IHttpHandler
    {
        //获取答题结果
        private string result = WeiSha.Common.Request.Form["result"].String;
        //答题结果的xml对象
        private XmlDocument resXml = new XmlDocument();
        public HttpResponse Response;
        public HttpRequest Request;       
        public void ProcessRequest (HttpContext context) {
            context.Response.ContentType = "text/plain";
            Response = context.Response;
            Request = context.Request;
            Response.ContentType = "text/xml";
            Response.Flush();
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddHours(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            result =context. Server.UrlDecode(result);
            this.Response.Write(initData());
            Response.End();
        }
        /// <summary>
        /// 初始化常用数据
        /// </summary>
        /// <returns>如果有错误，返回-1，如果考试结束或不存在，则返回0；正常返回1</returns>
        private string initData()
        {
            //如果为空，则返回-1，表示错误
            if (result == "") return "{\"score\":\"0\",\"trid\":\"0\"}";
            resXml.LoadXml(result, false);
            XmlNode xn = resXml.SelectSingleNode("results");
            //试卷id，试卷名称
            int tpid = 0;
            int.TryParse(getAttr(xn, "tpid"), out tpid);
            string tpname = getAttr(xn, "tpname");
            //课程id,课程名称
            int couid = 0;
            int.TryParse(getAttr(xn, "couid"), out couid);
            string couname = getAttr(xn, "couname");
            //专业id,专业名称
            int sbjid = 0;
            int.TryParse(getAttr(xn, "sbjid"), out sbjid);
            string sbjname = getAttr(xn, "sbjname");
            //学员Id,学员名称
            int stid = 0, stsid = 0;
            int.TryParse(getAttr(xn, "stid"), out stid);
            int.TryParse(getAttr(xn, "stsid"), out stsid);
            string stname = getAttr(xn, "stname");
            string stsname = getAttr(xn, "stsname");
            //UID与考试主题
            string uid = getAttr(xn, "uid");
            //string theme = xn.Attributes["theme"].Value.ToString();
            //提交方式，1为自动提交，2为交卷
            int patter = Convert.ToInt32(xn.Attributes["patter"].Value);
            float score = (float)Convert.ToDouble(getAttr(xn, "score"));    //考试成绩
            bool isClac = getAttr(xn, "isclac") == "true" ? true : false;   //是否在客户端计算过成绩
            //
            Song.Entities.TestResults exr = new TestResults();
            exr.Tp_Id = tpid;
            exr.Tp_Name = tpname;
            exr.Cou_ID = couid;
            exr.Sbj_ID = sbjid;
            exr.Sbj_Name = sbjname;
            exr.Ac_ID = stid;
            exr.Ac_Name = stname;
            exr.Sts_ID = stid;
            exr.Sts_Name = stsname;
            exr.Tr_IP = WeiSha.Common.Browser.IP;
            exr.Tr_Mac = "";
            exr.Tr_UID = uid;
            exr.Tr_Results = result;
            exr.Tr_Score = score;
            //机构信息
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            exr.Org_ID = org.Org_ID;
            exr.Org_Name = org.Org_Name;
            //得分
            score=Business.Do<ITestPaper>().ResultsSave(exr);
            //返回得分与成绩id
            string json = "{\"score\":" + score + ",\"trid\":" + exr.Tr_ID + ",\"tpid\":" + exr.Tp_Id + "}";
            return json;
            
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="xn"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        private string getAttr(XmlNode xn,string attr)
        {
            if (xn.Attributes[attr] == null) return string.Empty;
            return xn.Attributes[attr].Value.Trim();
        }
        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}