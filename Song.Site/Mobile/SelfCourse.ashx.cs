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
    /// 我的课程
    /// </summary>
    public class SelfCourse : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前课程
            Song.Entities.Course currCou = Extend.LoginState.Accounts.Course();
            if (currCou != null) this.Document.SetValue("currCou", currCou);
        }
    }
}