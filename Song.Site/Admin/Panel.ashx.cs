using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Song.Extend;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Admin
{
    /// <summary>
    /// 机构管理员的界面
    /// </summary>
    public class Panel : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前版本
            string version = WeiSha.Common.License.Value.VersionName;
            //如果是至尊版，则不再显示
            if(WeiSha.Common.License.Value.VersionLevel<6)
                this.Document.Variables.SetValue("version", version);
            //
            Song.Entities.EmpAccount acc = this.Admin;
            if (acc == null)
            {
                context.Response.Redirect("index.ashx");
                return;
            }
            this.Document.Variables.SetValue("CurrUser", acc);
        }        
    }
}