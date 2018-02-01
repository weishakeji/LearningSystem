using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
namespace Song.Site
{
    /// <summary>
    /// 退出登录
    /// </summary>
    public class CourseTryout : IHttpHandler
    {
        //来源页
        string from = WeiSha.Common.Request.QueryString["from"].String;
        //要试用的课程id
        int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        public void ProcessRequest(HttpContext context)
        {
            Extend.LoginState.Accounts.Tryout();
            if (couid > 0)
            {
                Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
                if (course != null)
                {
                    //设置当前学习的课程
                    Extend.LoginState.Student.Course(course);
                    if (course.Cou_IsFree)
                    {
                    }
                    context.Response.Redirect("Default.ashx");
                }
            }
            else
            {
                context.Response.Redirect("Subject.ashx");
            }
            //if (string.IsNullOrWhiteSpace(from))
            //{
            //    context.Response.Redirect("Default.ashx");
            //}
            //else
            //{
            //    context.Response.Redirect(from);
            //}
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