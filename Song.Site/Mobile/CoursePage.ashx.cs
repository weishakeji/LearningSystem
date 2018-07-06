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
    /// 课程学习首页
    /// </summary>
    public class CoursePage : BasePage
    {
        //当前课程的id
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //是否购买当前课程
        bool isBuy = false;
        protected override void InitPageTemplate(HttpContext context)
        {
            Song.Entities.Course course = null;
            if (couid > 0)
            {
                course = Business.Do<ICourse>().CourseSingle(couid);
                Extend.LoginState.Accounts.Course(course);
            }
            else
            {
                course = Extend.LoginState.Accounts.Course();
            }
            if (course == null) return;
            this.Document.SetValue("course", course);
            //课程资源路径
            this.Document.SetValue("coupath", Upload.Get["Course"].Virtual);
            //学习该课程的总人数，包括已经过期的
            int studyCount = Business.Do<ICourse>().CourseStudentSum(course.Cou_ID, null);
            this.Document.Variables.SetValue("studyCount", studyCount);
            //是否学习当前课程
            if (this.Account != null)
            {
                //是否购买
                isBuy = Business.Do<ICourse>().StudyIsCourse(this.Account.Ac_ID, course.Cou_ID);
                this.Document.Variables.SetValue("isBuy", isBuy);                
            }
        }
    }
}