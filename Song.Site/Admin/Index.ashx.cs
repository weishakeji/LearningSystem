using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Song.Extend;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
namespace Song.Site.Admin
{
    /// <summary>
    /// Index 的摘要说明
    /// </summary>
    public class Index : BasePage
    {

        protected override void InitPageTemplate(HttpContext context)
        {           
            //课程列表           
            Tag couTag = this.Document.GetChildTagById("courseList");
            if (couTag != null)
            {
                int count = int.Parse(couTag.Attributes.GetValue("count", "10"));
                List<Song.Entities.Course> cour = Business.Do<ICourse>().CourseCount(this.Organ.Org_ID, -1, -1, -1, null, true, count);
                foreach (Song.Entities.Course c in cour)
                {
                    c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                    c.Cou_Logo = Upload.Get["Course"].Virtual + c.Cou_Logo;
                    c.Cou_Intro = HTML.ClearTag(c.Cou_Intro);
                }
                this.Document.SetValue("courseList", cour);
            }
            //各种数据
            if (Extend.LoginState.Admin.IsLogin)
            {
                //专业数
                int sbjcount = Business.Do<ISubject>().SubjectOfCount(this.Organ.Org_ID, true, -1);
                this.Document.SetValue("sbjcount", sbjcount);
                //课程数
                int couCount = Business.Do<ICourse>().CourseOfCount(this.Organ.Org_ID, -1, -1);
                this.Document.SetValue("couCount", couCount);
                //考试数
                int testCount = Business.Do<ITestPaper>().PagerOfCount(this.Organ.Org_ID, -1, -1, -1, true);
                this.Document.SetValue("testCount", testCount);
                //试题数
                int quesCount = Business.Do<IQuestions>().QuesOfCount(this.Organ.Org_ID, -1, -1, -1, -1, true);
                this.Document.SetValue("quesCount", quesCount);
                //学员数
                int stCount = Business.Do<IAccounts>().AccountsOfCount(this.Organ.Org_ID, null);
                this.Document.SetValue("stCount", stCount);
                //资讯数
                int newsCount = Business.Do<IContents>().ArticleOfCount(this.Organ.Org_ID, -1);
                this.Document.SetValue("newsCount", newsCount);
            }
        }
    }
}