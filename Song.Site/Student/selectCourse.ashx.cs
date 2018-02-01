using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Student
{
    /// <summary>
    /// selectCourse 的摘要说明
    /// </summary>
    public class selectCourse : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        { 
            //课程id
            int couid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
            //当前学生
            Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            //是否学习当前课程
            bool isStudy = Business.Do<ICourse>().StudyIsCourse(st.Ac_ID, couid);
            //
            if (isStudy)
            {
                //取消学习
                Business.Do<ICourse>().DelteCourseBuy(st.Ac_ID, couid);
            }
            else
            {
                //选择学习
                Business.Do<ICourse>().CourseBuy(st.Ac_ID, couid, 0, DateTime.Now, DateTime.Now.AddYears(100));
            }
            if (context.Request.UrlReferrer != null)
            {
                context.Response.Redirect(context.Request.UrlReferrer.ToString());
            }
            else
            {
                context.Response.Redirect("index.ashx");
            }
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