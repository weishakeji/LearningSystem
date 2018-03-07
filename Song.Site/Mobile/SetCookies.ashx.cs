using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
namespace Song.Site.Mobile
{
    /// <summary>
    /// SetCookies 的摘要说明
    /// </summary>
    public class SetCookies : IHttpHandler
    {
        //要写入的账号id
        int accid = WeiSha.Common.Request.QueryString["accid"].Int32 ?? 0;
        //要返回的网址
        string url = WeiSha.Common.Request.QueryString["url"].UrlDecode;
        public void ProcessRequest(HttpContext context)
        {
            Extend.LoginState.Accounts.Write(accid);
            context.Response.Redirect(url);
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}