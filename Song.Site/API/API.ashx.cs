using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace Song.Site.API
{
    /// <summary>
    /// API 的摘要说明
    /// </summary>
    public class API : System.Web.UI.Page, IHttpHandler, IRequiresSessionState
    {

        public new void ProcessRequest(HttpContext context)
        {
            Song.ViewData.Letter p = new Song.ViewData.Letter(context);
            Song.ViewData.DataResult result = Song.ViewData.ExecuteMethod.ExecToResult(p);

            string json = result.ToJson();
            context.Response.ContentType = "text/plain";
            context.Response.Write(json);
            context.Response.End();
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