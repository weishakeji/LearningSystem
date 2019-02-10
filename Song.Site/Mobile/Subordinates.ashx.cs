using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using System.Text.RegularExpressions;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 当前学员的下级会员
    /// </summary>
    public class Subordinates : BasePage
    {

        protected override void InitPageTemplate(HttpContext context)
        {
            //下级会员数量
            int subcount = Business.Do<IAccounts>().SubordinatesCount(this.Account.Ac_ID, false);
            this.Document.Variables.SetValue("subcount", subcount);
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;

                int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页
                int sumcount = 0;
                Song.Entities.Accounts[] eas = null;
                eas = Business.Do<IAccounts>().AccountsPager(this.Organ.Org_ID, -1, st.Ac_ID, null, "", "", "", size, index, out sumcount);          
                string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",";
                json += "\"items\":[";
                for (int i = 0; i < eas.Length; i++)
                {
                    Song.Entities.Accounts acc = eas[i];
                    if(!string.IsNullOrWhiteSpace(acc.Ac_Photo))
                        acc.Ac_Photo = Upload.Get["Accounts"].Virtual + acc.Ac_Photo;
                    json += acc.ToJson() + ",";
                }
                if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
                json += "]}";
                Response.Write(json);
                Response.End();
            }
        }
    }
}