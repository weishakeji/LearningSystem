using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Text.RegularExpressions;
using WeiSha.Common;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 积分详情
    /// </summary>
    public class PointDetails : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页
                //当前学员
                Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
                int stid = st == null ? -1 : st.Ac_ID;
                int sumcount = 0;
                Song.Entities.PointAccount[] details = Business.Do<IAccounts>().PointPager(-1, stid, 0, null, null, null, size, index, out sumcount);
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