using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 教师评论列表
    /// </summary>
    public class TeacherComments : BasePage
    {
        //当前教师id
        private int thid = WeiSha.Common.Request.QueryString["thid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                Song.Entities.Teacher th = Business.Do<ITeacher>().TeacherSingle(thid);
                this.Document.SetValue("teacher", th); 
            }
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页                
                int sumcount = 0;
                Song.Entities.TeacherComment[] comments = null;
                comments = Business.Do<ITeacher>().CommentPager(-1, thid, true, true, size, index, out sumcount);
                string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",";
                json += "\"items\":[";
                for (int i = 0; i < comments.Length; i++)
                {
                    comments[i].Thc_Comment = Server.UrlEncode(comments[i].Thc_Comment);
                    comments[i].Thc_Reply = Server.UrlEncode(comments[i].Thc_Reply);
                    json += comments[i].ToJson() + ",";
                }
                if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
                json += "]}";
                Response.Write(json);
                Response.End();
            }
        }
    }
}