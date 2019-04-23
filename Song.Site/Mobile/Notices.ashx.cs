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
    /// 通知公告
    /// </summary>
    public class Notices : BasePage
    {       
        protected override void InitPageTemplate(HttpContext context)
        {
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string sear = WeiSha.Common.Request.QueryString["sear"].String;  //搜索
                int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页                
                int sumcount = 0;
                Song.Entities.Notice[] nots = Business.Do<INotice>().GetPager(Organ.Org_ID, true, sear, size, index, out sumcount);
                string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",";
                json += "\"items\":[";
                for (int i = 0; i < nots.Length; i++)
                {
                    Song.Entities.Notice not = nots[i];
                    //处理详情
                    not.No_Context = "";
                    not.No_Ttl = not.No_Ttl.Replace("\"", "&quot;");
                    not.No_Ttl = not.No_Ttl;                  
                    json += not.ToJson() + ",";
                }
                if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
                json += "]}";
                Response.Write(json);
                Response.End();
            }
        }
    }
}