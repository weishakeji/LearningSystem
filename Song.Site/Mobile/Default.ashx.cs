using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 手机端的首页
    /// </summary>
    public class Default : BasePage
    {
        //当前课程的id
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            if (couid > 0)
            {
                Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
                Extend.LoginState.Accounts.Course(course);
            }
            //当前选中的课程
            Song.Entities.Course currCourse = Extend.LoginState.Accounts.Course();
            this.Document.SetValue("currCourse", currCourse);
            //微信登录
            this.Document.SetValue("WeixinLoginIsUse", Business.Do<ISystemPara>()["WeixinLoginIsUse"].Boolean ?? false);
        }
    }
}