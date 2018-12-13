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
    /// 模拟考场的首页
    /// </summary>
    public class TestPapers : BasePage
    {
        //搜索字符
        protected string search = WeiSha.Common.Request.Form["search"].String;
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            this.Document.SetValue("couid", couid); 
            this.Document.SetValue("search", search);
            if (!Extend.LoginState.Accounts.IsLogin)
                this.Response.Redirect("login.ashx");           
            //当前选中的课程
            Song.Entities.Course currCourse = Business.Do<ICourse>().CourseSingle(couid);
            if (currCourse == null) return;
            if (currCourse != null)
            {
                this.Document.SetValue("currCourse", currCourse);
            }
            //试卷列表
            Song.Entities.TestPaper[] tps = null;
            if (string.IsNullOrWhiteSpace(search))
            {
                tps = Business.Do<ITestPaper>().PagerCount(-1, -1, currCourse.Cou_ID, -1, true, 0);
            }
            else
            {
                tps = Business.Do<ITestPaper>().PagerCount(search, -1, -1, currCourse.Cou_ID, -1, true, 0);
            }           

            foreach (Song.Entities.TestPaper s in tps)
            {
                s.Tp_Logo = string.IsNullOrWhiteSpace(s.Tp_Logo) ? s.Tp_Logo : Upload.Get["TestPaper"].Virtual + s.Tp_Logo;
            }
            this.Document.SetValue("tps", tps);

            //是否购买该课程
            Song.Entities.Course couBuy = Business.Do<ICourse>().IsBuyCourse(currCourse.Cou_ID, Extend.LoginState.Accounts.CurrentUser.Ac_ID, 1);
            bool isBuy = couBuy != null;
            if (!isBuy)
            {
                //this.Response.Redirect("CourseBuy.ashx?couid=" + currCourse.Cou_ID);
            }
            //是否购买，如果免费也算已经购买
            this.Document.SetValue("isBuy", isBuy || currCourse.Cou_IsFree);
            if (couBuy == null) couBuy = currCourse;
            this.Document.SetValue("couBuy", couBuy);
        }
    }
}