using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Song.Site.Ajax
{
    /// <summary>
    /// 获取试题
    /// </summary>
    public class GetQuestion : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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