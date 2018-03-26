using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;
namespace Song.Site.Student
{
    /// <summary>
    /// 学员管理界面
    /// </summary>
    public class Panel : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            Song.Entities.Accounts student = this.Account;
            if (student == null)
            {
                context.Response.Redirect("index.ashx");
                return;
            }
            this.Document.Variables.SetValue("CurrUser", student);
            //菜单项
            //Song.Entities.ManageMenu[] mms = Business.Do<IPurview>().GetAll4Org(this.Organ.Org_ID, "student");
        }           
    }
}