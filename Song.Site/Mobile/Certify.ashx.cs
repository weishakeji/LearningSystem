using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using System.Data;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 学员学习证明二维码扫描后的显示
    /// 
    /// </summary>
    public class Certify : BasePage
    {
        //学员ID
        private int accid = WeiSha.Common.Request.QueryString["acid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前学员
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(accid);
            //学员的学习情况记录
            DataTable dt = Business.Do<IStudent>().StudentStudyCourseLog(0, acc.Ac_ID);
            //
            this.Document.Variables.SetValue("acc", acc);
            this.Document.Variables.SetValue("logs", dt);
        } 
    }
}