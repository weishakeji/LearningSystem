using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using System.Data;
using System.Net;
using System.IO;
namespace Song.Site
{
    /// <summary>
    /// 系统启始页Default
    /// </summary>
    public class Default : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            if (WeiSha.Common.Browser.IsMobile)
            {
                context.Response.Redirect("/Mobile/default.ashx");
            } 
            //是否允许注册
            WeiSha.Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
            this.Document.SetValue("IsRegStudent", config["IsRegStudent"].Value.Boolean ?? true);

            //学员成绩排行，取最后一次考试
            //考试当前考试
            Tag exrTag = this.Document.GetChildTagById("exrTag");
            if (exrTag != null)
            {
                Song.Entities.Examination exam = Business.Do<IExamination>().ExamLast();
                if (exam != null)
                {
                    int count = int.Parse(exrTag.Attributes.GetValue("count", "5"));
                    Song.Entities.ExamResults[] exr = Business.Do<IExamination>().Results(exam.Exam_ID, count);
                    this.Document.SetValue("exr", exr);
                    //学员分组排行
                    DataTable dt = Business.Do<IExamination>().Result4StudentSort(exam.Exam_ID);
                    this.Document.SetValue("exrdt", dt);
                }
            }
            //string access_token = WeiSha.Common.Request.QueryString["access_token"].String;
            //if (string.IsNullOrWhiteSpace(access_token)) access_token = WeiSha.Common.Request.QueryString["#access_token"].String;
            //string url = "https://graph.qq.com/oauth2.0/me?format=xml&access_token=" + access_token;
            //string openid = RequestUrl(url);
            //context.Response.Write(access_token+"<br/>");
            //context.Response.Write(openid);
        }
        /// <summary>
        /// 请求指定url地址并返回结果
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string RequestUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.MaximumAutomaticRedirections = 3;
            request.Timeout = 0x2710;
            Stream responseStream = ((HttpWebResponse)request.GetResponse()).GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string str = reader.ReadToEnd();
            reader.Close();
            responseStream.Close();
            return str;
        }
    }
}