using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Song.Site.Utility.Ckplayer.ckplayer
{
    /// <summary>
    /// CKplayer的分享文件
    /// </summary>
    public class Share : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //获取share.xml内容
            string shareXml = WeiSha.Common.Request.Page.AbsolutePath + "share.xml";
            shareXml = context.Server.MapPath(shareXml);
            string xml = string.Empty;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(shareXml))
            {
                xml = sr.ReadToEnd();
                sr.Close();
            }
            //替换其中的域名信息
            string domain = WeiSha.Common.Request.Domain.CurrtentDomain;
            string port = WeiSha.Common.Server.Port;
            domain = port != "80" ? domain + ":" + port : domain;
            xml = xml.Replace("$domain$", "http://" + domain);
            context.Response.ContentType = "text/plain";
            context.Response.Write(xml);
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