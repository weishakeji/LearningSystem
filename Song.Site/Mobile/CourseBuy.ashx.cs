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
    /// 购买课程
    /// </summary>
    public class CourseBuy : BasePage
    {
        //课程ID
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前课程id写入cookies
            if (couid > 0)
            {
                context.Response.Cookies.Add(new HttpCookie("couid", couid.ToString()));
            }
            else
            {
                couid = WeiSha.Common.Request.Cookies["couid"].Int32 ?? 0;
            }
            this.Document.Variables.SetValue("couid", couid);
            //判断，如果已经购买，则直接跳转
            if (Extend.LoginState.Accounts.IsLogin)
            {
                int accid = Extend.LoginState.Accounts.CurrentUser.Ac_ID;
                Song.Entities.Course buyCou = Business.Do<ICourse>().IsBuyCourse(couid, accid, 1);
                if (buyCou != null)
                {
                    Extend.LoginState.Accounts.Course(buyCou);
                    this.Response.Redirect("default.ashx");
                    return;
                }
            }
            //当前课程
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null || !course.Cou_IsUse) return;
            this.Document.Variables.SetValue("course", course);
            //所属专业
            Song.Entities.Subject subject = Business.Do<ISubject>().SubjectSingle(course.Sbj_ID);
            this.Document.Variables.SetValue("subject", subject);
            //章节数
            int olCount = Business.Do<IOutline>().OutlineOfCount(course.Cou_ID, 0, true);
            this.Document.Variables.SetValue("olCount", olCount);
            //试题数
            int quesCount = Business.Do<IQuestions>().QuesOfCount(this.Organ.Org_ID, course.Sbj_ID, course.Cou_ID, 0, 0, true);
            this.Document.Variables.SetValue("quesCount", quesCount);
            //价格
            Song.Entities.CoursePrice[] prices = Business.Do<ICourse>().PriceCount(-1, course.Cou_UID, true, 0);
            this.Document.Variables.SetValue("prices", prices);
            //支付接口
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganRoot();
            Song.Entities.PayInterface[] pis = Business.Do<IPayInterface>().PayAll(org.Org_ID, "mobi", true);
            this.Document.Variables.SetValue("pis", pis);
        }
    }
}