using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Ajax
{
    /// <summary>
    /// 系统的版权信息，来自copyright.xml
    /// </summary>
    public class Copyright : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //版权信息
            WeiSha.Common.Copyright<string, string> copyright = WeiSha.Common.Request.Copyright;
            string json = copyright.ToJson();
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