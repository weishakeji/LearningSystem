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
    /// 教师的详情
    /// </summary>
    public class Teacher : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //教师
            int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
            Song.Entities.Teacher th = Business.Do<ITeacher>().TeacherSingle(id);
            if ((WeiSha.Common.Request.Cookies["Teacher_" + th.Th_ID].Int32 ?? 0) == 0)
            {
                th.Th_ViewNum++;
                Business.Do<ITeacher>().TeacherSave(th);
                context.Response.Cookies["Teacher_" + th.Th_ID].Value = th.Th_ID.ToString();
            }
            th.Th_Photo = Upload.Get["Teacher"].Virtual + th.Th_Photo;
            this.Document.Variables.SetValue("th", th);
            //教师的履历
            Song.Entities.TeacherHistory[] history = Business.Do<ITeacher>().HistoryAll(th.Th_ID);
            this.Document.Variables.SetValue("history", history);
            //教师的课程
            List<Song.Entities.Course> courses = Business.Do<ICourse>().CourseCount(-1, -1, th.Th_ID, -1, null, true, -1);
            for (int i = 0; i < courses.Count; i++)
            {
                Song.Entities.Course c = courses[i];
                c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                c.Cou_Logo = Upload.Get["Course"].Virtual + c.Cou_Logo;
            }
            this.Document.Variables.SetValue("courses", courses);
        }
    }
}