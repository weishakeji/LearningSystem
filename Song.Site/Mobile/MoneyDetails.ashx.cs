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
    /// 资金流水详情
    /// </summary>
    public class MoneyDetails : BasePage
    {        
        protected override void InitPageTemplate(HttpContext context)
        { 
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页
                int stid=Extend.LoginState.Accounts.CurrentUser.Ac_ID;   //当前学员

                int sumcount = 0;
                Song.Entities.MoneyAccount[] details = Business.Do<IAccounts>().MoneyPager(Organ.Org_ID, stid, 0, null, size, index, out sumcount);
                string json = "{'size':'" + size + "','index':'" + index + "','sumcount':'" + sumcount + "',";
                json += "'items':[";
                for (int n = 0; n < details.Length; n++)
                    json += details[n].ToJson() + ",";
                if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
                json += "]}";
                Response.Write(json);
                Response.End();               
            }
            
        }
    }
}