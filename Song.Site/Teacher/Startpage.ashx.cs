using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Teacher
{
    /// <summary>
    /// Startpage 的摘要说明
    /// </summary>
    public class Startpage : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //所属教师
            Song.Entities.Teacher th = Extend.LoginState.Accounts.Teacher;
            if (th == null) return;
            //所属机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //当前教师负责的课程
            List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseAll(org.Org_ID, -1, th.Th_ID, null);
            this.Document.SetValue("courses", cous);
            this.Document.SetValue("path", Upload.Get["Course"].Virtual);
        }
    }
}