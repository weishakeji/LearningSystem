using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Song.Extend;
using System.Web.SessionState;

namespace Song.Site.Admin
{
    /// <summary>
    /// Logout1 的摘要说明
    /// </summary>
    public class Logout : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            LoginState.Admin.Logout();
            if (context.Request.UrlReferrer != null)
            {
                context.Response.Redirect(context.Request.UrlReferrer.ToString());
            }
            else
            {
                context.Response.Redirect("index.ashx");
            }
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