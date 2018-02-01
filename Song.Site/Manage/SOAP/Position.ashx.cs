using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace Song.Site.Manage.SOAP
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Position : IHttpHandler
    {
        //地址
        private string address = WeiSha.Common.Request.QueryString["address"].String ?? "";
        public void ProcessRequest(HttpContext context)
        {
            WeiSha.Common.Param.Method.Position posi = WeiSha.Common.Request.Position(address);
            context.Response.ContentType = "text/plain";
            string json = "{\"lng\":" + posi.Longitude + ",\"lat\":" + posi.Latitude + "}";
            context.Response.Write(json);
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
